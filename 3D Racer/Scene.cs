using System;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;

namespace _3D_Racer
{
    public sealed class Scene
    {
        private static readonly object locker = new object();
        public readonly List<Shape> Shape_List = new List<Shape>();
        public Bitmap Canvas { get; set; }
        public Color Background_colour { get; set; }

        // Buffers
        private float[][] z_buffer;
        private Color[][] colour_buffer;

        // Scene dimensions
        public int Offset_x { get; set; }
        public int Offset_y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public Scene(int width, int height, Color? background_colour = null)
        {
            Width = width;
            Height = height;
            Background_colour = background_colour ?? Color.White;

            Canvas = new Bitmap(width, height);
            z_buffer = new float[Width][];
            for (int i = 0; i < Width; i++) z_buffer[i] = new float[Height];
            colour_buffer = new Color[Width][];
            for (int i = 0; i < Width; i++) colour_buffer[i] = new Color[Height];
        }

        /// <summary>
        /// Add a shape to the scene
        /// </summary>
        /// <param name="shape">Shape to add</param>
        public void Add(Shape shape)
        {
            lock (locker) Shape_List.Add(shape);
        }

        /// <summary>
        /// Add multiple shapes to the scene
        /// </summary>
        /// <param name="shapes">Array of shapes to add</param>
        public void Add(Shape[] shapes)
        {
            lock (locker) foreach (Shape shape in shapes) Shape_List.Add(shape);
        }

        public void Add_From_File(string file)
        {

        }

        public void Remove(int ID)
        {
            lock (locker) Shape_List.RemoveAll(x => x.ID == ID);
        }

        public void Render(PictureBox canvas_box, Camera camera)
        {
            lock (locker)
            {
                // Reset buffers
                for (int i = 0; i < Width; i++) for (int j = 0; j < Height; j++) z_buffer[i][j] = 1;
                for (int i = 0; i < Width; i++) for (int j = 0; j < Height; j++) colour_buffer[i][j] = Background_colour;

                camera.Calculate_Model_to_World_Matrix();
                camera.Calculate_World_to_Screen_Matrix();

                Bitmap temp = new Bitmap(Width, Height);

                using (Graphics g = Graphics.FromImage(temp))
                {
                    foreach (Shape shape in Shape_List)
                    {
                        shape.Render_Mesh.Calculate_Model_to_World_Matrix();
                        shape.Render_Mesh.Apply_World_Matrices();
                        shape.Render_Mesh.Apply_Camera_Matrices(camera);
                        shape.Render_Mesh.Divide_by_W();
                        shape.Render_Mesh.Scale_to_Screen(Width, Height);
                        shape.Render_Mesh.Round_Vertices();
                        shape.Render_Mesh.Change_Y_Axis(Height);

                        // Draw vertices
                        if (shape.Render_Mesh.Camera_Vertices != null && shape.Render_Mesh.Draw_Vertices)
                        {
                            foreach (Vertex vertex in shape.Render_Mesh.Camera_Vertices)
                            {
                                if (vertex.Visible)
                                {
                                    // Variable simplification
                                    int point_x = (int)vertex.X;
                                    int point_y = (int)vertex.Y;
                                    float point_z = vertex.Z;

                                    for (int x = point_x - vertex.Diameter / 2; x <= point_x + vertex.Diameter / 2; x++)
                                    {
                                        for (int y = point_y - vertex.Diameter / 2; y <= point_y + vertex.Diameter / 2; y++)
                                        {
                                            // Check against z buffer
                                            if (z_buffer[x][y] > point_z)
                                            {
                                                z_buffer[x][y] = point_z;
                                                colour_buffer[x][y] = shape.Render_Mesh.Vertex_Colour;
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        // Draw edges
                        if (shape.Render_Mesh.Camera_Vertices != null && shape.Render_Mesh.Edges != null && shape.Render_Mesh.Draw_Edges)
                        {
                            foreach (Edge edge in shape.Render_Mesh.Edges)
                            {
                                if (edge.Visible)
                                {
                                    // Variable simplification
                                    int point_1_x = (int)shape.Render_Mesh.Camera_Vertices[edge.P1].X;
                                    int point_1_y = (int)shape.Render_Mesh.Camera_Vertices[edge.P1].Y;
                                    float point_1_z = shape.Render_Mesh.Camera_Vertices[edge.P1].Z;
                                    int point_2_x = (int)shape.Render_Mesh.Camera_Vertices[edge.P2].X;
                                    int point_2_y = (int)shape.Render_Mesh.Camera_Vertices[edge.P2].Y;
                                    float point_2_z = shape.Render_Mesh.Camera_Vertices[edge.P2].Z;

                                    // Calculate furthest left and right values
                                    int min_x = Math.Min(point_1_x, point_2_x);
                                    int max_x = Math.Max(point_1_x, point_2_x);

                                    // Calculate lowest and highest points
                                    int min_y = Math.Min(point_1_y, point_2_y);
                                    int max_y = Math.Max(point_1_y, point_2_y);

                                    // Get starting y_value and z_value
                                    int y_value = (min_x == point_1_x) ? point_1_y : point_2_y;
                                    float z_value = (min_x == point_1_x) ? point_1_z : point_2_z;

                                    // HOW CAN BE INTEGER?
                                    int y_increase = (point_1_x == point_2_x) ? 1 : (point_1_y - point_2_y) / (point_1_x - point_2_x);
                                    float z_increase = (point_1_z - point_2_z) / (point_1_y - point_2_y);

                                    for (int x = min_x; x < max_x; x++)
                                    {
                                        // Check against z buffer
                                        if (z_buffer[x][y_value] > z_value)
                                        {
                                            z_buffer[x][y_value] = z_value;
                                            colour_buffer[x][y_value] = shape.Render_Mesh.Edge_Colour;
                                        }

                                        y_value += y_increase;
                                        z_value += z_increase;
                                    }

                                }
                            }
                        }

                        // Draw faces
                        if (shape.Render_Mesh.Camera_Vertices != null && shape.Render_Mesh.Faces != null && shape.Render_Mesh.Draw_Faces)
                        {
                            foreach (Face face in shape.Render_Mesh.Faces)
                            {
                                // Variable simplification
                                int point_1_x = (int)shape.Render_Mesh.Camera_Vertices[face.P1].X;
                                int point_1_y = (int)shape.Render_Mesh.Camera_Vertices[face.P1].Y;
                                float point_1_z = shape.Render_Mesh.Camera_Vertices[face.P1].Z;
                                int point_2_x = (int)shape.Render_Mesh.Camera_Vertices[face.P2].X;
                                int point_2_y = (int)shape.Render_Mesh.Camera_Vertices[face.P2].Y;
                                float point_2_z = shape.Render_Mesh.Camera_Vertices[face.P2].Z;
                                int point_3_x = (int)shape.Render_Mesh.Camera_Vertices[face.P3].X;
                                int point_3_y = (int)shape.Render_Mesh.Camera_Vertices[face.P3].Y;
                                float point_3_z = shape.Render_Mesh.Camera_Vertices[face.P3].Z;

                                // Calculate normal
                                Vector3D v1 = new Vector3D(point_2_x, point_2_y, point_2_z) - new Vector3D(point_1_x, point_1_y, point_1_z);
                                Vector3D v2 = new Vector3D(point_3_x, point_3_y, point_3_z) - new Vector3D(point_1_x, point_1_y, point_1_z);
                                Vector3D normal = v1.Cross_Product(v2);
                                float a_over_c = normal.X / normal.Z, b_over_c = normal.Y / normal.Z;

                                // Get furthest left and right points
                                int min_x = Math.Min(Math.Min(point_1_x, point_2_x), point_3_x);
                                int max_x = Math.Max(Math.Max(point_1_x, point_2_x), point_3_x);

                                // Get lowest and highest points
                                int lowest = Math.Min(Math.Min(point_1_y, point_2_y), point_3_y);
                                int highest = Math.Max(Math.Max(point_1_y, point_2_y), point_3_y);

                                // Get starting z_value
                                float z_value;
                                if (lowest == point_1_y)
                                {
                                    z_value = point_1_z;
                                }
                                else
                                {
                                    z_value = (lowest == point_2_y) ? point_2_z : point_3_z;
                                }

                                int prevx = 0;
                                int x_1, x_2, x_3, final_x_1, final_x_2, start_x_value, final_x_value;

                                // Iterate over all possible lines
                                for (int y = lowest; y <= highest; y++)
                                {
                                    // Find start and end points of each horizontal line
                                    // CHECK IF LINE
                                    x_1 = (point_1_y - point_2_y == 0) ? -1 : (y - point_2_y) * (point_1_x - point_2_x) / (point_1_y - point_2_y) + point_2_x;
                                    x_2 = (point_1_y - point_3_y == 0) ? -1 : (y - point_3_y) * (point_1_x - point_3_x) / (point_1_y - point_3_y) + point_3_x;
                                    x_3 = (point_2_y - point_3_y == 0) ? -1 : (y - point_3_y) * (point_2_x - point_3_x) / (point_2_y - point_3_y) + point_3_x;

                                    if (x_1 >= min_x && x_1 <= max_x)
                                    {
                                        final_x_1 = x_1;
                                        final_x_2 = (x_2 >= min_x && x_2 <= max_x) ? x_2 : x_3;
                                    }
                                    else
                                    {
                                        final_x_1 = x_1;
                                        final_x_2 = x_2;
                                    }

                                    start_x_value = Math.Min(final_x_1, final_x_2);
                                    final_x_value = Math.Max(final_x_1, final_x_2);

                                    // Check if triangle is flat
                                    if (start_x_value == -1 && final_x_value == -1)
                                    {
                                        start_x_value = min_x;
                                        final_x_value = max_x;
                                    }

                                    // Reset to beginning of new line
                                    if (y != lowest) z_value -= (start_x_value - prevx) * a_over_c;

                                    for (int x = start_x_value; x <= final_x_value; x++)
                                    {
                                        // Check against z buffer
                                        if (z_buffer[x][y] > z_value)
                                        {
                                            z_buffer[x][y] = z_value;
                                            colour_buffer[x][y] = shape.Render_Mesh.Face_Colour;
                                        }
                                        if (x != final_x_value) z_value -= a_over_c;
                                    }

                                    z_value += a_over_c * (final_x_value - start_x_value);
                                    prevx = start_x_value;
                                    z_value -= b_over_c;
                                }
                            }
                        }
                    }
                    
                    // Does foreach use ref?
                    // Draw each pixel in z-buffer
                    for (int x = 0; x < Width; x++)
                    {
                        for (int y = 0; y < Height; y++)
                        {
                            using (SolidBrush face_brush = new SolidBrush(colour_buffer[x][y])) g.FillRectangle(face_brush, x, y, 1, 1);
                        }
                    }
                }

                Canvas = temp;
                canvas_box.Invalidate();
            }
        }
    }
}
 