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

        public int Width { get; set; }
        public int Height { get; set; }

        public Scene(int width, int height)
        {
            Width = width;
            Height = height;
            Canvas = new Bitmap(width, height);
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