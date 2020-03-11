using System;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;

namespace _3D_Racer
{
    public sealed class Scene
    {
        private static readonly object locker = new object();
        public readonly List<Shape> Objects = new List<Shape>();
        public Bitmap Canvas { get; set; }
        public Color Background_colour { get; set; } = Color.Black;
        private float[][] z_buffer;
        private float[][] colour_buffer;
        public int Width { get; set; }
        public int Height { get; set; }

        public Scene(int width, int height)
        {
            Width = width;
            Height = height;
            Canvas = new Bitmap(width, height);
            z_buffer = new float[Width][];
            for (int i = 0; i < Width; i++) z_buffer[i] = new float[Height];
            colour_buffer = new float[Width][];
            for (int i = 0; i < Width; i++) colour_buffer[i] = new float[Height];
        }

        private void Reset_Z_Buffer()
        {
            for (int i = 0; i < Width; i++) for (int j = 0; j < Height; j++) z_buffer[Width][Height] = 1;
        }

        private void Reset_Colour_Buffer()
        {
            for (int i = 0; i < Width; i++) for (int j = 0; j < Height; j++) colour_buffer[Width][Height] = 1;
        }

        public void Add(Shape shape)
        {
            lock (locker) Objects.Add(shape);
        }

        public void Add_From_File(string file)
        {

        }

        public void Remove(int ID)
        {
            lock (locker) Objects.RemoveAll(x => x.ID == ID);
        }

        public void Render(PictureBox canvas_box, Camera camera)
        {
            lock (locker)
            {
                camera.Calculate_Model_to_World_Matrix();
                camera.Calculate_World_to_Screen_Matrix();

                Bitmap temp = new Bitmap(Width, Height);

                using (Graphics g = Graphics.FromImage(temp))
                {
                    foreach (Shape shape in Objects)
                    {
                        shape.Calculate_Model_to_World_Matrix();
                        shape.Apply_World_Matrices();
                        shape.Apply_Camera_Matrices(camera);
                        shape.Divide_by_W();
                        shape.Scale_to_Screen(Width, Height);
                        shape.Change_Y_Axis(Height);
                    }

                    Reset_Z_Buffer();
                    Reset_Colour_Buffer();

                    foreach (Shape shape in Objects)
                    {
                        foreach (Face face in shape.Faces)
                        {
                            Vector3D v1 = new Vector3D(shape.Camera_Vertices[face.P2].X, shape.Camera_Vertices[face.P2].Y, shape.Camera_Vertices[face.P2].Z) - new Vector3D(shape.Camera_Vertices[face.P1].X, shape.Camera_Vertices[face.P1].Y, shape.Camera_Vertices[face.P1].Z);
                            Vector3D v2 = new Vector3D(shape.Camera_Vertices[face.P3].X, shape.Camera_Vertices[face.P3].Y, shape.Camera_Vertices[face.P3].Z) - new Vector3D(shape.Camera_Vertices[face.P1].X, shape.Camera_Vertices[face.P1].Y, shape.Camera_Vertices[face.P1].Z);
                            Vector3D normal = v1.Cross_Product(v2);
                            float a = normal.X;
                            float b = normal.Y;
                            float c = normal.Z;
                            float d = -(a * shape.Camera_Vertices[face.P1].X + b * shape.Camera_Vertices[face.P1].Y + c * shape.Camera_Vertices[face.P1].Z);

                            // Get height of triangle
                            float height1 = Math.Abs(shape.Camera_Vertices[face.P1].Y - shape.Camera_Vertices[face.P2].Y);
                            float height2 = Math.Abs(shape.Camera_Vertices[face.P1].Y - shape.Camera_Vertices[face.P3].Y);
                            float height3 = Math.Abs(shape.Camera_Vertices[face.P2].Y - shape.Camera_Vertices[face.P3].Y);
                            float height = Math.Max(Math.Max(height1, height2), height3);

                            // Get lowest point (Round camera vertices to ints later)
                            int lowest = (int)Math.Min(Math.Min(shape.Camera_Vertices[face.P1].Y, shape.Camera_Vertices[face.P2].Y), shape.Camera_Vertices[face.P3].Y);

                            int min_x = (int)Math.Min(Math.Min(shape.Camera_Vertices[face.P1].X, shape.Camera_Vertices[face.P2].X), shape.Camera_Vertices[face.P3].X);
                            int max_x = (int)Math.Max(Math.Max(shape.Camera_Vertices[face.P1].X, shape.Camera_Vertices[face.P2].X), shape.Camera_Vertices[face.P3].X);

                            float z_value = z_init;

                            // Iterate over all possible lines
                            for (int i = lowest; i <= lowest + height; i++)
                            {
                                int x1 = (int)((i - shape.Camera_Vertices[face.P1].Y) * (shape.Camera_Vertices[face.P2].X - shape.Camera_Vertices[face.P1].X) / (shape.Camera_Vertices[face.P1].Y - shape.Camera_Vertices[face.P2].Y));
                                int x2 = (int)((i - shape.Camera_Vertices[face.P1].Y) * (shape.Camera_Vertices[face.P3].X - shape.Camera_Vertices[face.P1].X) / (shape.Camera_Vertices[face.P1].Y - shape.Camera_Vertices[face.P3].Y));
                                int x3 = (int)((i - shape.Camera_Vertices[face.P2].Y) * (shape.Camera_Vertices[face.P3].X - shape.Camera_Vertices[face.P2].X) / (shape.Camera_Vertices[face.P2].Y - shape.Camera_Vertices[face.P3].Y));

                                int final_x_1, final_x_2;

                                if (x1 >= min_x && x1 <= max_x)
                                {
                                    final_x_1 = x1;
                                    final_x_2 = (x2 >= min_x && x2 <= max_x) ? x2 : x3;
                                }
                                else
                                {
                                    final_x_1 = x1;
                                    final_x_2 = x2;
                                }

                                
                                for (int j = final_x_1; j <= final_x_2; j++)
                                {
                                    z_value -= a / c;
                                }
                            }
                        }
                    }

                    foreach (Shape shape in Objects)
                    {
                        using (SolidBrush face_brush = new SolidBrush(shape.Face_Colour))
                        using (SolidBrush vertex_brush = new SolidBrush(shape.Vertex_Colour))
                        using (Pen edge_pen = new Pen(shape.Edge_Colour))
                        {
                            // Draw faces
                            if (shape.Camera_Vertices != null && shape.Faces != null && shape.Draw_Faces)
                            {
                                foreach (Face face in shape.Faces)
                                {
                                    if (face.Visible) g.FillPolygon(face_brush, new PointF[3] {
                                    new PointF(shape.Camera_Vertices[face.P1].X, shape.Camera_Vertices[face.P1].Y),
                                    new PointF(shape.Camera_Vertices[face.P2].X, shape.Camera_Vertices[face.P2].Y),
                                    new PointF(shape.Camera_Vertices[face.P3].X, shape.Camera_Vertices[face.P3].Y) });
                                }
                            }

                            // Draw edges
                            if (shape.Camera_Vertices != null && shape.Edges != null && shape.Draw_Edges)
                            {
                                foreach (Edge edge in shape.Edges)
                                {
                                    if (edge.Visible) g.DrawLine(edge_pen, shape.Camera_Vertices[edge.P1].X, shape.Camera_Vertices[edge.P1].Y, shape.Camera_Vertices[edge.P2].X, shape.Camera_Vertices[edge.P2].Y);
                                }
                            }

                            // Draw vertices
                            if (shape.Camera_Vertices != null && shape.Draw_Vertices)
                            {
                                foreach (Vertex vertex in shape.Camera_Vertices)
                                {
                                    if (vertex.Visible) g.FillEllipse(vertex_brush, vertex.X - vertex.Radius / 2, vertex.Y - vertex.Radius / 2, vertex.Radius, vertex.Radius);
                                }
                            }
                        }
                    }
                }

                Canvas = temp;
                canvas_box.Invalidate();
            }
        }
    }
}