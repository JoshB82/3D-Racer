using System;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;

namespace _3D_Racer
{
    public sealed partial class Scene
    {
        private static readonly object locker = new object();
        private readonly List<Shape> Shape_List = new List<Shape>();
        public Bitmap Canvas { get; set; }
        public Color Background_colour { get; set; }

        // Buffers
        private double[][] z_buffer;
        private Color[][] colour_buffer;

        // Scene dimensions
        private int width, height;
        public int Width
        {
            get { return width; }
            set { lock (locker) { width = value; Set_Buffer(); } }
        }
        public int Height {
            get { return height; }
            set { lock (locker) { height = value; Set_Buffer(); } }
        }

        public Scene(int width, int height, Color? background_colour = null)
        {
            Width = width;
            Height = height;
            Background_colour = background_colour ?? Color.White;

            Canvas = new Bitmap(width, height);
        }

        private void Set_Buffer()
        {
            z_buffer = new double[width][];
            colour_buffer = new Color[width][];
            for (int i = 0; i < width; i++) z_buffer[i] = new double[height];
            for (int i = 0; i < width; i++) colour_buffer[i] = new Color[height];
        }

        /// <summary>
        /// Add a shape to the scene.
        /// </summary>
        /// <param name="shape">Shape to add.</param>
        public void Add(Shape shape)
        {
            lock (locker) Shape_List.Add(shape);
        }

        /// <summary>
        /// Add multiple shapes to the scene.
        /// </summary>
        /// <param name="shapes">Array of shapes to add.</param>
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

        private static Clipped_Edge Clip_Line(Vector3D plane_point, Vector3D plane_normal, Clipped_Edge e)
        {
            Vector3D point_1 = new Vector3D(e.P1), point_2 = new Vector3D(e.P2);
            double point_1_distance = Vector3D.Point_Distance_From_Plane(point_1, plane_point, plane_normal);
            double point_2_distance = Vector3D.Point_Distance_From_Plane(point_2, plane_point, plane_normal);

            if (point_1_distance >= 0 && point_2_distance >= 0)
            {
                // Both points are on the inside, so return line unchanged
                return e;
            }
            if (point_1_distance >= 0 && point_2_distance < 0)
            {
                // One point is on the inside, the other on the outside, so clip the line
                Vector4D intersection = new Vector4D(Vector3D.Line_Intersect_Plane(point_1, point_2, plane_point, plane_normal));
                return new Clipped_Edge(new Vector4D(point_1), intersection, e.Colour, e.Visible);
            }
            if (point_1_distance < 0 && point_2_distance >= 0)
            {
                // One point is on the outside, the other on the inside, so clip the line
                Vector4D intersection = new Vector4D(Vector3D.Line_Intersect_Plane(point_2, point_1, plane_point, plane_normal));
                return new Clipped_Edge(new Vector4D(point_2), intersection, e.Colour, e.Visible);
            }
            // Both points are on the outside, so discard the line
            return null;
        }

        private static int Clip_Face(Vector3D plane_point, Vector3D plane_normal, Clipped_Face f, out Clipped_Face f1, out Clipped_Face f2)
        {
            f1 = null; f2 = null;
            Vector3D point_1 = new Vector3D(f.P1), point_2 = new Vector3D(f.P2), point_3 = new Vector3D(f.P3);
            int inside_point_count = 0;
            List<Vector3D> inside_points = new List<Vector3D>(3);
            List<Vector3D> outside_points = new List<Vector3D>(3);

            if (Vector3D.Point_Distance_From_Plane(point_1, plane_point, plane_normal) >= 0)
            {
                inside_point_count++;
                inside_points.Add(point_1);
            }
            else
            {
                outside_points.Add(point_1);
            }

            if (Vector3D.Point_Distance_From_Plane(point_2, plane_point, plane_normal) >= 0)
            {
                inside_point_count++;
                inside_points.Add(point_2);
            }
            else
            {
                outside_points.Add(point_2);
            }
            
            if (Vector3D.Point_Distance_From_Plane(point_3, plane_point, plane_normal) >= 0)
            {
                inside_point_count++;
                inside_points.Add(point_3);
            }
            else
            {
                outside_points.Add(point_3);
            }

            Vector4D first_intersection, second_intersection;

            switch (inside_point_count)
            {
                case 0:
                    // All points are on the outside, so no valid triangles to return
                    return 0;
                case 1:
                    // One point is on the inside, so only a smaller triangle is needed
                    first_intersection = new Vector4D(Vector3D.Line_Intersect_Plane(inside_points[0], outside_points[0], plane_point, plane_normal));
                    second_intersection = new Vector4D(Vector3D.Line_Intersect_Plane(inside_points[0], outside_points[1], plane_point, plane_normal));
                    f1 = new Clipped_Face(new Vector4D(inside_points[0]), first_intersection, second_intersection, f.Colour, f.Visible);
                    return 1;
                case 2:
                    // Two points are on the inside, so a quadrilateral is formed and split into two triangles
                    first_intersection = new Vector4D(Vector3D.Line_Intersect_Plane(inside_points[0], outside_points[0], plane_point, plane_normal));
                    second_intersection = new Vector4D(Vector3D.Line_Intersect_Plane(inside_points[1], outside_points[0], plane_point, plane_normal));
                    f1 = new Clipped_Face(new Vector4D(inside_points[0]), new Vector4D(inside_points[1]), first_intersection, f.Colour, f.Visible);
                    f2 = new Clipped_Face(new Vector4D(inside_points[1]), second_intersection, first_intersection, f.Colour, f.Visible);
                    return 2;
                case 3:
                    // All points are on the inside, so return the triangle unchanged
                    f1 = new Clipped_Face(f.P1, f.P2, f.P3, f.Colour, f.Visible);
                    return 1;
            }

            return 0;
        }

        public void Render(PictureBox canvas_box, Perspective_Camera camera, bool check)
        {
            lock (locker)
            {
                // Create temporary canvas
                Bitmap temp_canvas = new Bitmap(Width, Height);

                // Reset buffers
                for (int i = 0; i < Width; i++) for (int j = 0; j < Height; j++) z_buffer[i][j] = 2; // 2 is always greater than anything to be rendered.
                for (int i = 0; i < Width; i++) for (int j = 0; j < Height; j++) colour_buffer[i][j] = Background_colour;

                // Calculate camera matrices
                camera.Calculate_Model_to_World_Matrix();
                camera.Apply_World_Matrix();
                camera.Calculate_World_to_Screen_Matrix();
                Clipping_Plane[] clipping_planes = camera.Calculate_Clipping_Planes();

                using (Graphics g = Graphics.FromImage(temp_canvas))
                {
                    foreach (Shape shape in Shape_List)
                    {
                        // Move shapes to world space
                        shape.Render_Mesh.Calculate_Model_to_World_Matrix();
                        shape.Render_Mesh.Apply_World_Matrices();

                        // Draw faces
                        if (shape.Render_Mesh.Faces != null && shape.Render_Mesh.Draw_Faces)
                        {
                            foreach (Face face in shape.Render_Mesh.Faces)
                            {
                                if (face.Visible)
                                {
                                    // Variable simplification
                                    double point_1_x = shape.Render_Mesh.World_Vertices[face.P1].X;
                                    double point_1_y = shape.Render_Mesh.World_Vertices[face.P1].Y;
                                    double point_1_z = shape.Render_Mesh.World_Vertices[face.P1].Z;
                                    double point_2_x = shape.Render_Mesh.World_Vertices[face.P2].X;
                                    double point_2_y = shape.Render_Mesh.World_Vertices[face.P2].Y;
                                    double point_2_z = shape.Render_Mesh.World_Vertices[face.P2].Z;
                                    double point_3_x = shape.Render_Mesh.World_Vertices[face.P3].X;
                                    double point_3_y = shape.Render_Mesh.World_Vertices[face.P3].Y;
                                    double point_3_z = shape.Render_Mesh.World_Vertices[face.P3].Z;

                                    // Triangle point vectors
                                    Vector4D point_1 = new Vector4D(point_1_x, point_1_y, point_1_z);
                                    Vector4D point_2 = new Vector4D(point_2_x, point_2_y, point_2_z);
                                    Vector4D point_3 = new Vector4D(point_3_x, point_3_y, point_3_z);

                                    Queue<Clipped_Face> face_clip = new Queue<Clipped_Face>();

                                    // Add initial triangle to clipping queue
                                    face_clip.Enqueue(new Clipped_Face(point_1, point_2, point_3, shape.Render_Mesh.Face_Colour, shape.Render_Mesh.Visible));
                                    int no_triangles = 1;

                                    // Clip face against each clipping plane
                                    foreach (Clipping_Plane clipping_plane in clipping_planes)
                                    {
                                        while (no_triangles > 0)
                                        {
                                            Clipped_Face triangle = face_clip.Dequeue();
                                            Clipped_Face[] triangles = new Clipped_Face[2];
                                            int num_intersection_points = Clip_Face(clipping_plane.Point, clipping_plane.Normal, triangle, out triangles[0], out triangles[1]);
                                            for (int i = 0; i < num_intersection_points; i++) face_clip.Enqueue(triangles[i]);
                                            no_triangles--;
                                        }
                                        no_triangles = face_clip.Count;
                                    }

                                    Clipped_Face[] face_clip_array = face_clip.ToArray();
                                    if (face_clip_array.Length > 0)
                                    {
                                        for (int i = 0; i < face_clip_array.Length; i++)
                                        {
                                            // Move remaining faces into view space and ----
                                            face_clip_array[i].P1 = camera.Apply_Camera_Matrices(face_clip_array[i].P1);
                                            face_clip_array[i].P2 = camera.Apply_Camera_Matrices(face_clip_array[i].P2);
                                            face_clip_array[i].P3 = camera.Apply_Camera_Matrices(face_clip_array[i].P3);

                                            Vector4D new_point_1 = camera.Divide_By_W(face_clip_array[i].P1);
                                            Vector4D new_point_2 = camera.Divide_By_W(face_clip_array[i].P2);
                                            Vector4D new_point_3 = camera.Divide_By_W(face_clip_array[i].P3);

                                            // Scale to screen space?
                                            new_point_1 = Scale_to_screen(new_point_1);
                                            new_point_2 = Scale_to_screen(new_point_2);
                                            new_point_3 = Scale_to_screen(new_point_3);

                                            // WHHY?
                                            new_point_1 = Change_Y_Axis(new_point_1);
                                            new_point_2 = Change_Y_Axis(new_point_2);
                                            new_point_3 = Change_Y_Axis(new_point_3);

                                            // More variable simplification
                                            int new_point_1_x = (int)Math.Round(new_point_1.X, MidpointRounding.AwayFromZero);
                                            int new_point_1_y = (int)Math.Round(new_point_1.Y, MidpointRounding.AwayFromZero);
                                            double new_point_1_z = new_point_1.Z;
                                            int new_point_2_x = (int)Math.Round(new_point_2.X, MidpointRounding.AwayFromZero);
                                            int new_point_2_y = (int)Math.Round(new_point_2.Y, MidpointRounding.AwayFromZero);
                                            double new_point_2_z = new_point_2.Z;
                                            int new_point_3_x = (int)Math.Round(new_point_3.X, MidpointRounding.AwayFromZero);
                                            int new_point_3_y = (int)Math.Round(new_point_3.Y, MidpointRounding.AwayFromZero);
                                            double new_point_3_z = new_point_3.Z;

                                            // between [-1,1]
                                            // between [0,2] (+1)
                                            // => between [0,1] (/2)
                                            // => between [0,width-1] (*(width-1))

                                            // RANGE TO DRAW X: [0,WIDTH-1] Y: [0,HEIGHT-1]
                                            Triangle(new_point_1_x, new_point_1_y, new_point_1_z, new_point_2_x, new_point_2_y, new_point_2_z, new_point_3_x, new_point_3_y, new_point_3_z, shape.Render_Mesh.Face_Colour);

                                            // ROUDNING?
                                        }
                                    }
                                }
                            }
                        }

                        // Draw edges
                        if (shape.Render_Mesh.Edges != null && shape.Render_Mesh.Draw_Edges)
                        {
                            foreach (Edge edge in shape.Render_Mesh.Edges)
                            {
                                if (edge.Visible)
                                {
                                    // Variable simplification
                                    double point_1_x = shape.Render_Mesh.World_Vertices[edge.P1].X;
                                    double point_1_y = shape.Render_Mesh.World_Vertices[edge.P1].Y;
                                    double point_1_z = shape.Render_Mesh.World_Vertices[edge.P1].Z;
                                    double point_2_x = shape.Render_Mesh.World_Vertices[edge.P2].X;
                                    double point_2_y = shape.Render_Mesh.World_Vertices[edge.P2].Y;
                                    double point_2_z = shape.Render_Mesh.World_Vertices[edge.P2].Z;

                                    // Line points
                                    Vector4D point_1 = new Vector4D(point_1_x, point_1_y, point_1_z);
                                    Vector4D point_2 = new Vector4D(point_2_x, point_2_y, point_2_z);

                                    Clipped_Edge new_edge = new Clipped_Edge(point_1, point_2, shape.Render_Mesh.Edge_Colour, shape.Render_Mesh.Visible);
                                    Clipped_Edge new_edge_2 = null;
                                    bool draw_edge = true;

                                    foreach (Clipping_Plane clipping_plane in clipping_planes)
                                    {
                                        new_edge_2 = Clip_Line(clipping_plane.Point, clipping_plane.Normal, new_edge);
                                        if (new_edge_2 == null)
                                        {
                                            draw_edge = false;
                                            break;
                                        }
                                        else
                                        {
                                            new_edge = new_edge_2;
                                        }
                                    }

                                    if (draw_edge)
                                    {
                                        // Move remaining faces into view space and ----
                                        new_edge_2.P1 = camera.Apply_Camera_Matrices(new_edge_2.P1);
                                        new_edge_2.P2 = camera.Apply_Camera_Matrices(new_edge_2.P2);

                                        Vector4D new_point_1 = camera.Divide_By_W(new_edge_2.P1);
                                        Vector4D new_point_2 = camera.Divide_By_W(new_edge_2.P2);

                                        // Scale to screen space?
                                        new_point_1 = Scale_to_screen(new_point_1);
                                        new_point_2 = Scale_to_screen(new_point_2);

                                        // WHHY?
                                        new_point_1 = Change_Y_Axis(new_point_1);
                                        new_point_2 = Change_Y_Axis(new_point_2);

                                        int new_point_1_x = (int)Math.Round(new_point_1.X, MidpointRounding.AwayFromZero);
                                        int new_point_1_y = (int)Math.Round(new_point_1.Y, MidpointRounding.AwayFromZero);
                                        double new_point_1_z = new_point_1.Z;
                                        int new_point_2_x = (int)Math.Round(new_point_2.X, MidpointRounding.AwayFromZero);
                                        int new_point_2_y = (int)Math.Round(new_point_2.Y, MidpointRounding.AwayFromZero);
                                        double new_point_2_z = new_point_2.Z;

                                        Line(new_point_1_x, new_point_1_y, new_point_1_z, new_point_2_x, new_point_2_y, new_point_2_z, shape.Render_Mesh.Edge_Colour);
                                    }
                                }
                            }
                        }

                        /*
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
                                    double point_z = vertex.Z;

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
                        */
                    }
                    
                    // Draw each pixel from colour buffer
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

        public Vector4D Scale_to_screen(Vector4D vertex) => Transform.Scale(0.5f * (Width - 1), 0.5f * (Height - 1), 1) * Transform.Translate(new Vector3D(1, 1, 0)) * vertex;

        // Only do at drawing stage? v
        public Vector4D Change_Y_Axis(Vector4D vertex) => Transform.Translate(new Vector3D(0, Height - 1, 0)) * Transform.Scale_Y(-1) * vertex;
    }
}

// Clipping planes
/*Clipping_Plane[] clipping_planes = new Clipping_Plane[]
{
    new Clipping_Plane(new Vector3D(1, 1, 0), Vector3D.Unit_Y), // Bottom
    new Clipping_Plane(new Vector3D(1, 1, 0), Vector3D.Unit_X), // Left
    new Clipping_Plane(new Vector3D(Width, 0, 0), Vector3D.Unit_Negative_X), // Right
    new Clipping_Plane(new Vector3D(0, Height, 0), Vector3D.Unit_Negative_Y), // Top
    new Clipping_Plane(new Vector3D(0, 0, -1), Vector3D.Unit_Z), // Near z
    new Clipping_Plane(new Vector3D(0, 0, 1), Vector3D.Unit_Negative_Z) // Far z
};
*/
// Back face culling?