using System;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;

namespace _3D_Racer
{
    public sealed partial class Scene
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

        private static float Point_Distance_From_Plane(Vector3D point, Vector3D plane_point, Vector3D plane_normal) => point * plane_normal - plane_point * plane_normal;

        // What if intersects at a corner?
        private static int Clip(Vector3D plane_point, Vector3D plane_normal, Face_2 f, out Face_2 f1, out Face_2 f2)
        {
            f1 = null; f2 = null;
            Vector3D point_1 = new Vector3D(f.P1.X, f.P1.Y, f.P1.Z);
            Vector3D point_2 = new Vector3D(f.P2.X, f.P2.Y, f.P2.Z);
            Vector3D point_3 = new Vector3D(f.P3.X, f.P3.Y, f.P3.Z);
            int inside_point_count = 0, outside_point_count = 0;
            List<Vector3D> inside_points = new List<Vector3D>(3);
            List<Vector3D> outside_points = new List<Vector3D>(3);

            if (Point_Distance_From_Plane(point_1, plane_point, plane_normal) >= 0)
            {
                inside_point_count++;
                inside_points.Add(point_1);
            }
            else
            {
                outside_point_count++;
                outside_points.Add(point_1);
            }

            if (Point_Distance_From_Plane(point_2, plane_point, plane_normal) >= 0)
            {
                inside_point_count++;
                inside_points.Add(point_2);
            }
            else
            {
                outside_point_count++;
                outside_points.Add(point_2);
            }
            
            if (Point_Distance_From_Plane(point_3, plane_point, plane_normal) >= 0)
            {
                inside_point_count++;
                inside_points.Add(point_3);
            }
            else
            {
                outside_point_count++;
                outside_points.Add(point_3);
            }

            Vector3D first_intersection, second_intersection;

            switch (inside_point_count)
            {
                case 0:
                    // All points are on the outside, so no valid triangles to return
                    f1 = null; f2 = null;
                    return 0;
                case 1:
                    // One point is on the inside, so a quadrilateral is formed and split into two triangles
                    first_intersection = Vector3D.Line_Intersect_Plane(inside_points[0], outside_points[0], plane_point, plane_normal);
                    second_intersection = Vector3D.Line_Intersect_Plane(inside_points[1], outside_points[0], plane_point, plane_normal);
                    f1 = new Face_2(inside_points[0], inside_points[1], first_intersection, f.Colour, f.Visible);
                    f2 = new Face_2(inside_points[1], second_intersection, first_intersection, f.Colour, f.Visible);
                    return 2;
                case 2:
                    // Two point are on the inside, so only a smaller triangle is needed
                    first_intersection = Vector3D.Line_Intersect_Plane(inside_points[0], outside_points[0], plane_point, plane_normal);
                    second_intersection = Vector3D.Line_Intersect_Plane(inside_points[0], outside_points[1], plane_point, plane_normal);
                    f1 = new Face_2(inside_points[0], first_intersection, second_intersection, f.Colour, f.Visible);
                    f2 = null;
                    return 1;
                case 3:
                    // All points are on the inside, so return the triangle unchanged
                    f1 = new Face_2(f.P1, f.P2, f.P3, f.Colour, f.Visible);
                    f2 = null;
                    return 1;
            }

            return 0;
        }

        public void Render(PictureBox canvas_box, Camera camera)
        {
            lock (locker)
            {
                // Reset buffers
                for (int i = 0; i < Width; i++) for (int j = 0; j < Height; j++) z_buffer[i][j] = 1;
                for (int i = 0; i < Width; i++) for (int j = 0; j < Height; j++) colour_buffer[i][j] = Background_colour;

                // Calculate camera matrices
                camera.Calculate_Model_to_World_Matrix();
                camera.Calculate_World_to_Screen_Matrix();

                // Clipping planes
                List<Clipping_Plane> clipping_planes = new List<Clipping_Plane>
                {
                    new Clipping_Plane(Vector3D.Zero, Vector3D.Unit_Y), // Bottom
                    new Clipping_Plane(Vector3D.Zero, Vector3D.Unit_X), // Left
                    new Clipping_Plane(new Vector3D(Width, 0, 0), Vector3D.Unit_Negative_X), // Right
                    new Clipping_Plane(new Vector3D(0, Height, 0), Vector3D.Unit_Negative_Y), // Top
                    new Clipping_Plane(new Vector3D(0, 0, -1), Vector3D.Unit_Z) // Near z
                };

                // Create temporary canvas
                Bitmap temp_canvas = new Bitmap(Width, Height);

                using (Graphics g = Graphics.FromImage(temp_canvas))
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

                                // Triangle vectors
                                Vector3D point_1 = new Vector3D(point_1_x, point_1_y, point_1_z);
                                Vector3D point_2 = new Vector3D(point_2_x, point_2_y, point_2_z);
                                Vector3D point_3 = new Vector3D(point_3_x, point_3_y, point_3_z);

                                Queue<Face_2> face_clip = new Queue<Face_2>();

                                // Add initial triangle
                                face_clip.Enqueue(new Face_2(point_1, point_2, point_3, shape.Render_Mesh.Face_Colour, shape.Render_Mesh.Visible));
                                int no_triangles = 1;

                                foreach (Clipping_Plane clipping_plane in clipping_planes)
                                {
                                    while(no_triangles > 0)
                                    {
                                        Face_2 triangle = face_clip.Dequeue();
                                        Face_2[] triangles = new Face_2[2];
                                        int num_intersection_points = Clip(clipping_plane.Point, clipping_plane.Normal, triangle, out triangles[0], out triangles[1]);
                                        for (int i = 0; i < num_intersection_points; i++) face_clip.Enqueue(triangles[i]);
                                        no_triangles--;
                                    }
                                    no_triangles = face_clip.Count;
                                }

                                // Draw clipped triangles
                                foreach (Face_2 triangle in face_clip)
                                {
                                    // More variable simplification
                                    point_1_x = (int)Math.Round(triangle.P1.X, MidpointRounding.AwayFromZero);
                                    point_1_y = (int)Math.Round(triangle.P1.Y, MidpointRounding.AwayFromZero);
                                    point_1_z = triangle.P1.Z;
                                    point_2_x = (int)Math.Round(triangle.P2.X, MidpointRounding.AwayFromZero);
                                    point_2_y = (int)Math.Round(triangle.P2.Y, MidpointRounding.AwayFromZero);
                                    point_2_z = triangle.P2.Z;
                                    point_3_x = (int)Math.Round(triangle.P3.X, MidpointRounding.AwayFromZero);
                                    point_3_y = (int)Math.Round(triangle.P3.Y, MidpointRounding.AwayFromZero);
                                    point_3_z = triangle.P3.Z;

                                    Triangle(point_1_x, point_1_y, point_1_z, point_2_x, point_2_y, point_2_z, point_3_x, point_3_y, point_3_z, shape.Render_Mesh.Face_Colour);
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

                                    Line(point_1_x, point_1_y, point_1_z, point_2_x, point_2_y, point_2_z, shape.Render_Mesh.Edge_Colour);
                                }
                            }
                        }

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

                Canvas = temp_canvas;
                canvas_box.Invalidate();
            }
        }
    }
}

/*


// Get furthest left and right points
int min_x = Math.Min(Math.Min(point_1_x, point_2_x), point_3_x);
int max_x = Math.Max(Math.Max(point_1_x, point_2_x), point_3_x);

// Get lowest and highest points
int min_y = Math.Min(Math.Min(point_1_y, point_2_y), point_3_y);
int max_y = Math.Max(Math.Max(point_1_y, point_2_y), point_3_y);

// Get starting z_value
float z_value;
                                if (min_y == point_1_y)
                                {
                                    z_value = point_1_z;
                                }
                                else
                                {
                                    z_value = (min_y == point_2_y) ? point_2_z : point_3_z;
                                }

                                // Check for triangle where all vertices have the same x - co-ordinate. In this case, the triangle should appear as a vertical line.
                                if (point_1_x == point_2_x && point_2_x == point_3_x)
                                {
                                    for (int y = min_y; y <= max_y; y++)
                                    {
                                        // Check against z buffer
                                        if (z_buffer[point_1_x][y] > z_value)
                                        {
                                            z_buffer[point_1_x][y] = z_value;
                                            colour_buffer[point_1_x][y] = shape.Render_Mesh.Face_Colour;
                                        }
                                        z_value -= b_over_c;
                                    }
                                    continue;
                                }

                                // Check for triangle where all vertices have the same y - co-ordinate. In this case, the triangle should appear as a horizontal line.
                                if (point_1_y == point_2_y && point_2_y == point_3_y)
                                {
                                    for (int x = min_x; x <= max_x; x++)
                                    {
                                        // Check against z buffer
                                        if (z_buffer[x][point_1_y] > z_value)
                                        {
                                            z_buffer[x][point_1_y] = z_value;
                                            colour_buffer[x][point_1_y] = shape.Render_Mesh.Face_Colour;
                                        }
                                        z_value -= a_over_c;
                                    }
                                    continue;
                                }

                                int x_1, x_2, y_1, y_2;
int prev_x = 0, prev_y = 0;
int start_x_value, final_x_value, start_y_value, final_y_value;

                                // Check for triangle where two vertices have the same x - co-ordinate. In this case, the triangle should appear with one vertical line.
                                if (point_1_x == point_2_x)
                                {
                                    for (int x = min_x; x <= max_x; x++)
                                    {
                                        y_1 = point_1_y + (point_3_y - point_1_y) * (x - point_1_x) / (point_3_x - point_1_x);
                                        y_2 = point_2_y + (point_3_y - point_2_y) * (x - point_2_x) / (point_3_x - point_2_x);
                                        start_y_value = Math.Min(y_1, y_2);
                                        final_y_value = Math.Max(y_1, y_2);

                                        // Reset to beginning of new line
                                        if (x != min_x) z_value -= (start_y_value - prev_y) * b_over_c;

                                        for (int y = start_y_value; y <= final_y_value; y++)
                                        {
                                            // Check against z buffer
                                            if (z_buffer[x][y] > z_value)
                                            {
                                                z_buffer[x][y] = z_value;
                                                colour_buffer[x][y] = shape.Render_Mesh.Face_Colour;
                                            }
                                            z_value -= b_over_c;
                                        }
                                        z_value += b_over_c* (final_y_value - start_y_value + 1);
                                        prev_y = start_y_value;
                                        z_value -= a_over_c;
                                    }
                                    continue;
                                }
                                if (point_1_x == point_3_x)
                                {
                                    for (int x = min_x; x <= max_x; x++)
                                    {
                                        y_1 = point_1_y + (point_2_y - point_1_y) * (x - point_1_x) / (point_2_x - point_1_x);
                                        y_2 = point_2_y + (point_3_y - point_2_y) * (x - point_2_x) / (point_3_x - point_2_x);
                                        start_y_value = Math.Min(y_1, y_2);
                                        final_y_value = Math.Max(y_1, y_2);

                                        // Reset to beginning of new line
                                        if (x != min_x) z_value -= (start_y_value - prev_y) * b_over_c;

                                        for (int y = start_y_value; y <= final_y_value; y++)
                                        {
                                            // Check against z buffer
                                            if (z_buffer[x][y] > z_value)
                                            {
                                                z_buffer[x][y] = z_value;
                                                colour_buffer[x][y] = shape.Render_Mesh.Face_Colour;
                                            }
                                            z_value -= b_over_c;
                                        }
                                        z_value += b_over_c* (final_y_value - start_y_value + 1);
                                        prev_y = start_y_value;
                                        z_value -= a_over_c;
                                    }
                                    continue;
                                }
                                if (point_2_x == point_3_x)
                                {
                                    for (int x = min_x; x <= max_x; x++)
                                    {
                                        y_1 = point_1_y + (point_2_y - point_1_y) * (x - point_1_x) / (point_2_x - point_1_x);
                                        y_2 = point_1_y + (point_3_y - point_1_y) * (x - point_1_x) / (point_3_x - point_1_x);
                                        start_y_value = Math.Min(y_1, y_2);
                                        final_y_value = Math.Max(y_1, y_2);

                                        // Reset to beginning of new line
                                        if (x != min_x) z_value -= (start_y_value - prev_y) * b_over_c;

                                        for (int y = start_y_value; y <= final_y_value; y++)
                                        {
                                            // Check against z buffer
                                            if (z_buffer[x][y] > z_value)
                                            {
                                                z_buffer[x][y] = z_value;
                                                colour_buffer[x][y] = shape.Render_Mesh.Face_Colour;
                                            }
                                            z_value -= b_over_c;
                                        }
                                        z_value += b_over_c* (final_y_value - start_y_value + 1);
                                        prev_y = start_y_value;
                                        z_value -= a_over_c;
                                    }
                                    continue;
                                }

                                // Check for triangle where two vertices have the same y - co-ordinate. In this case, the triangle should appear with one horizontal line.
                                if (point_1_y == point_2_y)
                                {
                                    for (int y = min_y; y <= max_y; y++)
                                    {
                                        x_1 = (y - point_3_y) * (point_1_x - point_3_x) / (point_1_y - point_3_y) + point_3_x;
                                        x_2 = (y - point_3_y) * (point_2_x - point_3_x) / (point_2_y - point_3_y) + point_3_x;
                                        start_x_value = Math.Min(x_1, x_2);
                                        final_x_value = Math.Max(x_1, x_2);

                                        // Reset to beginning of new line
                                        if (y != min_y) z_value -= (start_x_value - prev_x) * a_over_c;

                                        for (int x = start_x_value; x <= final_x_value; x++)
                                        {
                                            // Check against z buffer
                                            if (z_buffer[x][y] > z_value)
                                            {
                                                z_buffer[x][y] = z_value;
                                                colour_buffer[x][y] = shape.Render_Mesh.Face_Colour;
                                            }
                                            z_value -= a_over_c;
                                        }
                                        z_value += a_over_c* (final_x_value - start_x_value + 1);
                                        prev_x = start_x_value;
                                        z_value -= b_over_c;
                                    }
                                    continue;
                                }
                                if (point_1_y == point_3_y)
                                {
                                    for (int y = min_y; y <= max_y; y++)
                                    {
                                        x_1 = (y - point_2_y) * (point_1_x - point_2_x) / (point_1_y - point_2_y) + point_2_x;
                                        x_2 = (y - point_3_y) * (point_2_x - point_3_x) / (point_2_y - point_3_y) + point_3_x;
                                        start_x_value = Math.Min(x_1, x_2);
                                        final_x_value = Math.Max(x_1, x_2);

                                        // Reset to beginning of new line
                                        if (y != min_y) z_value -= (start_x_value - prev_x) * a_over_c;

                                        for (int x = start_x_value; x <= final_x_value; x++)
                                        {
                                            // Check against z buffer
                                            if (z_buffer[x][y] > z_value)
                                            {
                                                z_buffer[x][y] = z_value;
                                                colour_buffer[x][y] = shape.Render_Mesh.Face_Colour;
                                            }
                                            z_value -= a_over_c;
                                        }
                                        z_value += a_over_c* (final_x_value - start_x_value + 1);
                                        prev_x = start_x_value;
                                        z_value -= b_over_c;
                                    }
                                    continue;
                                }
                                if (point_2_y == point_3_y)
                                {
                                    for (int y = min_y; y <= max_y; y++)
                                    {
                                        x_1 = (y - point_1_y) * (point_2_x - point_1_x) / (point_2_y - point_1_y) + point_1_x;
                                        x_2 = (y - point_3_y) * (point_1_x - point_3_x) / (point_1_y - point_3_y) + point_3_x;
                                        start_x_value = Math.Min(x_1, x_2);
                                        final_x_value = Math.Max(x_1, x_2);

                                        // Reset to beginning of new line
                                        if (y != min_y) z_value -= (start_x_value - prev_x) * a_over_c;

                                        for (int x = start_x_value; x <= final_x_value; x++)
                                        {
                                            // Check against z buffer
                                            if (z_buffer[x][y] > z_value)
                                            {
                                                z_buffer[x][y] = z_value;
                                                colour_buffer[x][y] = shape.Render_Mesh.Face_Colour;
                                            }
                                            z_value -= a_over_c;
                                        }
                                        z_value += a_over_c* (final_x_value - start_x_value + 1);
                                        prev_x = start_x_value;
                                        z_value -= b_over_c;
                                    }
                                    continue;
                                }

                                int x_3, final_x_1, final_x_2;

                                // Otherwise the triangle's vertices have no co-ordinates in common.
                                for (int y = min_y; y <= max_y; y++)
                                {
                                    x_1 = (y - point_2_y) * (point_1_x - point_2_x) / (point_1_y - point_2_y) + point_2_x;
                                    x_2 = (y - point_3_y) * (point_1_x - point_3_x) / (point_1_y - point_3_y) + point_3_x;
                                    x_3 = (y - point_3_y) * (point_2_x - point_3_x) / (point_2_y - point_3_y) + point_3_x;

                                    if (x_1 >= min_x && x_1 <= max_x)
                                    {
                                        final_x_1 = x_1;
                                        final_x_2 = (x_2 >= min_x && x_2 <= max_x) ? x_2 : x_3;
                                    }
                                    else
                                    {
                                        final_x_1 = x_2;
                                        final_x_2 = x_3;
                                    }

                                    start_x_value = Math.Min(final_x_1, final_x_2);
                                    final_x_value = Math.Max(final_x_1, final_x_2);

                                    // Reset to beginning of new line
                                    if (y != min_y) z_value -= (start_x_value - prev_x) * a_over_c;

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

                                    z_value += a_over_c* (final_x_value - start_x_value);
                                    prev_x = start_x_value;
                                    z_value -= b_over_c;
                                }
                                */