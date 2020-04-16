using System;
using System.Diagnostics;
using System.Drawing;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace _3D_Racer
{
    public sealed partial class Scene
    {
        private static readonly object locker = new object();

        public bool Change_scene { get; set; } = true;

        private readonly List<Light> Light_List = new List<Light>();
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

        public void Add_Light(Light light)
        {
            lock (locker) Light_List.Add(light);
        }

        public bool Add_From_OBJ_File(string file_path)
        {
            if (File.Exists(file_path))
            {
                try
                {
                    List<Vertex> vertices = new List<Vertex>();
                    List<Edge> edges = new List<Edge>();
                    List<Face> faces = new List<Face>();

                    string[] lines = File.ReadAllLines(file_path);
                    foreach (string line in lines)
                    {
                        string[] data = line.Split();
                        switch (data[0])
                        {
                            case "#":
                                // Comment; ignore line
                                break;
                            case "v":
                                // Vertex

                                break;
                            case "l":
                                // Line
                                break;
                        }
                    }

                    return true;
                }
                catch(Exception error)
                {
                    Debug.WriteLine($"Error: {error.Message}");
                    return false;
                }
            }
            else
            {
                Debug.WriteLine($"{file_path} not found.");
                return false;
            }
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
                Vector4D intersection = new Vector4D(Vector3D.Line_Intersect_Plane(point_1, point_2, plane_point, plane_normal, out double d));
                return new Clipped_Edge(new Vector4D(point_1), intersection, e.Colour, e.Visible);
            }
            if (point_1_distance < 0 && point_2_distance >= 0)
            {
                // One point is on the outside, the other on the inside, so clip the line
                Vector4D intersection = new Vector4D(Vector3D.Line_Intersect_Plane(point_2, point_1, plane_point, plane_normal, out double d));
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
            List<Vector3D> inside_texture_points = new List<Vector3D>(3);
            List<Vector3D> outside_texture_points = new List<Vector3D>(3);

            if (Vector3D.Point_Distance_From_Plane(point_1, plane_point, plane_normal) >= 0)
            {
                inside_point_count++;
                inside_points.Add(point_1);
                inside_texture_points.Add(f.T1);
            }
            else
            {
                outside_points.Add(point_1);
                outside_texture_points.Add(f.T1);
            }

            if (Vector3D.Point_Distance_From_Plane(point_2, plane_point, plane_normal) >= 0)
            {
                inside_point_count++;
                inside_points.Add(point_2);
                inside_texture_points.Add(f.T2);
            }
            else
            {
                outside_points.Add(point_2);
                outside_texture_points.Add(f.T2);
            }
            
            if (Vector3D.Point_Distance_From_Plane(point_3, plane_point, plane_normal) >= 0)
            {
                inside_point_count++;
                inside_points.Add(point_3);
                inside_texture_points.Add(f.T3);
            }
            else
            {
                outside_points.Add(point_3);
                outside_texture_points.Add(f.T3);
            }

            Vector4D first_intersection, second_intersection;
            double d;

            switch (inside_point_count)
            {
                case 0:
                    // All points are on the outside, so no valid triangles to return
                    return 0;
                case 1:
                    // One point is on the inside, so only a smaller triangle is needed
                    first_intersection = new Vector4D(Vector3D.Line_Intersect_Plane(inside_points[0], outside_points[0], plane_point, plane_normal, out d));
                    second_intersection = new Vector4D(Vector3D.Line_Intersect_Plane(inside_points[0], outside_points[1], plane_point, plane_normal, out d));
                    if (f.Texture == null)
                    {
                        f1 = new Clipped_Face(new Vector4D(inside_points[0]), first_intersection, second_intersection, f.Colour, f.Draw_Outline, f.Visible);
                    }
                    else
                    {
                        Vector3D t_intersection_1 = (outside_texture_points[0] - inside_texture_points[0]) * d + inside_texture_points[0];
                        Vector3D t_intersection_2 = (outside_texture_points[1] - inside_texture_points[0]) * d + inside_texture_points[0];
                        f1 = new Clipped_Face(new Vector4D(inside_points[0]), first_intersection, second_intersection, inside_texture_points[0], t_intersection_1, t_intersection_2, f.Texture, f.Draw_Outline, f.Visible);
                    }
                    return 1;
                case 2:
                    // Two points are on the inside, so a quadrilateral is formed and split into two triangles
                    first_intersection = new Vector4D(Vector3D.Line_Intersect_Plane(inside_points[0], outside_points[0], plane_point, plane_normal, out d));
                    second_intersection = new Vector4D(Vector3D.Line_Intersect_Plane(inside_points[1], outside_points[0], plane_point, plane_normal, out d));
                    if (f.Texture == null)
                    {
                        f1 = new Clipped_Face(new Vector4D(inside_points[0]), new Vector4D(inside_points[1]), first_intersection, f.Colour, f.Draw_Outline, f.Visible);
                        f2 = new Clipped_Face(new Vector4D(inside_points[1]), second_intersection, first_intersection, f.Colour, f.Draw_Outline, f.Visible);
                    }
                    else
                    {
                        Vector3D t_intersection_1 = (outside_texture_points[0] - inside_texture_points[0]) * d + inside_texture_points[0];
                        Vector3D t_intersection_2 = (outside_texture_points[0] - inside_texture_points[1]) * d + inside_texture_points[1];
                        f1 = new Clipped_Face(new Vector4D(inside_points[0]), new Vector4D(inside_points[1]), first_intersection, inside_texture_points[0], inside_texture_points[1], t_intersection_1, f.Texture, f.Draw_Outline, f.Visible);
                        f2 = new Clipped_Face(new Vector4D(inside_points[1]), second_intersection, first_intersection, inside_texture_points[1], t_intersection_2, t_intersection_1, f.Texture, f.Draw_Outline, f.Visible);
                    }
                    return 2;
                case 3:
                    // All points are on the inside, so return the triangle unchanged
                    if (f.Texture == null)
                    {
                        f1 = new Clipped_Face(f.P1, f.P2, f.P3, f.Colour, f.Draw_Outline, f.Visible);
                    }
                    else
                    {
                        f1 = new Clipped_Face(f.P1, f.P2, f.P3, f.T1, f.T2, f.T3, f.Texture, f.Draw_Outline, f.Visible);
                    }
                    return 1;
            }

            return 0;
        }

        public void Render(PictureBox canvas_box, Perspective_Camera camera)
        {
            // Only render if a change in scene has taken place.
            // if (!Change_scene) return;

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

                // Clipping planes
                Clipping_Plane[] world_clipping_planes = camera.Calculate_Clipping_Planes();
                Vector3D near_bottom_left_point = new Vector3D(-1, -1, -1), far_top_right_point = new Vector3D(1, 1, 1);
                Clipping_Plane[] projection_clipping_planes = new Clipping_Plane[]
                {
                    new Clipping_Plane(near_bottom_left_point, Vector3D.Unit_X), // Left
                    new Clipping_Plane(near_bottom_left_point, Vector3D.Unit_Y), // Bottom
                    new Clipping_Plane(near_bottom_left_point, Vector3D.Unit_Z), // Near
                    new Clipping_Plane(far_top_right_point, Vector3D.Unit_Negative_X), // Right
                    new Clipping_Plane(far_top_right_point, Vector3D.Unit_Negative_Y), // Top
                    new Clipping_Plane(far_top_right_point, Vector3D.Unit_Negative_Z) // Far
                };

                // Draw graphics
                using (Graphics g = Graphics.FromImage(temp_canvas))
                {
                    foreach (Shape shape in Shape_List)
                    {
                        // Move shapes from model space to world space
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

                                    double texture_point_1_x = 0, texture_point_1_y = 0, texture_point_2_x = 0, texture_point_2_y = 0, texture_point_3_x = 0, texture_point_3_y = 0;
                                    Vector3D texture_point_1 = null, texture_point_2 = null, texture_point_3 = null;
                                    if (face.Texture != null)
                                    {
                                        texture_point_1_x = shape.Render_Mesh.Texture_Vertices[face.T1].X;
                                        texture_point_1_y = shape.Render_Mesh.Texture_Vertices[face.T1].Y;
                                        texture_point_2_x = shape.Render_Mesh.Texture_Vertices[face.T2].X;
                                        texture_point_2_y = shape.Render_Mesh.Texture_Vertices[face.T2].Y;
                                        texture_point_3_x = shape.Render_Mesh.Texture_Vertices[face.T3].X;
                                        texture_point_3_y = shape.Render_Mesh.Texture_Vertices[face.T3].Y;
                                        
                                        texture_point_1 = new Vector3D(texture_point_1_x, texture_point_1_y, 1);
                                        texture_point_2 = new Vector3D(texture_point_2_x, texture_point_2_y, 1);
                                        texture_point_3 = new Vector3D(texture_point_3_x, texture_point_3_y, 1);
                                    }

                                    // Triangle point vectors
                                    Vector4D point_1 = new Vector4D(point_1_x, point_1_y, point_1_z);
                                    Vector4D point_2 = new Vector4D(point_2_x, point_2_y, point_2_z);
                                    Vector4D point_3 = new Vector4D(point_3_x, point_3_y, point_3_z);
                                    Vector3D normal = Vector3D.Normal_From_Plane(new Vector3D(point_1), new Vector3D(point_2), new Vector3D(point_3));
                                    Vector3D camera_to_face = new Vector3D(point_1 - camera.World_Origin);

                                    // Adjust colour based on lighting
                                    Color face_colour = face.Colour;
                                    if (Light_List.Count > 0)
                                    {
                                        double max_intensity = 0, true_intensity = 0;
                                        foreach (Light light in Light_List) max_intensity += light.Intensity;
                                        foreach (Light light in Light_List)
                                        {
                                            switch (light.GetType().Name)
                                            {
                                                case "Distant_Light":
                                                    true_intensity = Math.Max(0, -light.World_Direction * normal) * light.Intensity;
                                                    break;
                                                case "Point_Light":
                                                    true_intensity = Math.Max(0, -new Vector3D(point_1 - light.World_Origin).Normalise() * normal) * light.Intensity;
                                                    break;
                                                case "Spot_Light":
                                                    Vector3D light_to_shape = new Vector3D(point_1 - light.World_Origin);
                                                    if (light_to_shape.Angle(light.World_Direction) > ((Spotlight)light).Angle || light_to_shape * light.World_Direction > ((Spotlight)light).Distance) continue;
                                                    true_intensity = Math.Max(0, -light.World_Direction * normal) * light.Intensity;
                                                    break;
                                                case "Ambient_Light":
                                                    break;
                                            }
                                            double scaled_intensity = true_intensity / max_intensity;

                                            if (face.Texture == null)
                                            {

                                            }

                                            byte new_red = (byte)Round_To_Int((face.Colour.R + light.Colour.R) * 255 / 510 * scaled_intensity);
                                            byte new_green = (byte)Round_To_Int((face.Colour.G + light.Colour.G) * 255 / 510 * scaled_intensity);
                                            byte new_blue = (byte)Round_To_Int((face.Colour.B + light.Colour.B) * 255 / 510 * scaled_intensity);

                                            face_colour = Color.FromArgb(face.Colour.A, new_red, new_green, new_blue);
                                        }
                                    }

                                    // Include exemptions to back-face culling?
                                    if (camera_to_face * normal < 0 || shape.Render_Mesh.GetType().Name == "Plane")
                                    {
                                        // Create a queue
                                        Queue<Clipped_Face> world_face_clip = new Queue<Clipped_Face>();

                                        // Add initial triangle to clipping queue
                                        if (face.Texture == null)
                                        {
                                            world_face_clip.Enqueue(new Clipped_Face(point_1, point_2, point_3, face.Colour, face.Draw_Outline, face.Visible));
                                        }
                                        else
                                        {
                                            world_face_clip.Enqueue(new Clipped_Face(point_1, point_2, point_3, texture_point_1, texture_point_2, texture_point_3, face.Texture, face.Draw_Outline, face.Visible));
                                        }
                                        
                                        int no_triangles = 1;

                                        // Clip face against each world clipping plane
                                        foreach (Clipping_Plane world_clipping_plane in world_clipping_planes)
                                        {
                                            while (no_triangles > 0)
                                            {
                                                Clipped_Face triangle = world_face_clip.Dequeue();
                                                Clipped_Face[] triangles = new Clipped_Face[2];
                                                int num_intersection_points = Clip_Face(world_clipping_plane.Point, world_clipping_plane.Normal, triangle, out triangles[0], out triangles[1]);
                                                for (int i = 0; i < num_intersection_points; i++) world_face_clip.Enqueue(triangles[i]);
                                                no_triangles--;
                                            }
                                            no_triangles = world_face_clip.Count;
                                        }

                                        if (no_triangles > 0)
                                        {
                                            Queue<Clipped_Face> projection_face_clip = world_face_clip;

                                            // Move remaining faces into projection space
                                            foreach (Clipped_Face projection_clipped_face in projection_face_clip)
                                            {
                                                projection_clipped_face.P1 = camera.Apply_Camera_Matrices(projection_clipped_face.P1);
                                                projection_clipped_face.P2 = camera.Apply_Camera_Matrices(projection_clipped_face.P2);
                                                projection_clipped_face.P3 = camera.Apply_Camera_Matrices(projection_clipped_face.P3);

                                                projection_clipped_face.P1 = camera.Divide_By_W(projection_clipped_face.P1);
                                                projection_clipped_face.P2 = camera.Divide_By_W(projection_clipped_face.P2);
                                                projection_clipped_face.P3 = camera.Divide_By_W(projection_clipped_face.P3);

                                                if (face.Texture != null)
                                                {
                                                    projection_clipped_face.T1 /= projection_clipped_face.P1.W;
                                                    projection_clipped_face.T2 /= projection_clipped_face.P1.W;
                                                    projection_clipped_face.T3 /= projection_clipped_face.P1.W;
                                                }
                                            }

                                            // Clip face against each projection clipping plane
                                            foreach (Clipping_Plane projection_clipping_plane in projection_clipping_planes)
                                            {
                                                while (no_triangles > 0)
                                                {
                                                    Clipped_Face triangle = projection_face_clip.Dequeue();
                                                    Clipped_Face[] triangles = new Clipped_Face[2];
                                                    int num_intersection_points = Clip_Face(projection_clipping_plane.Point, projection_clipping_plane.Normal, triangle, out triangles[0], out triangles[1]);
                                                    for (int i = 0; i < num_intersection_points; i++) projection_face_clip.Enqueue(triangles[i]);
                                                    no_triangles--;
                                                }
                                                no_triangles = projection_face_clip.Count;
                                            }

                                            foreach (Clipped_Face projection_clipped_face in projection_face_clip)
                                            {
                                                Vector4D result_point_1 = Scale_to_screen(projection_clipped_face.P1);
                                                Vector4D result_point_2 = Scale_to_screen(projection_clipped_face.P2);
                                                Vector4D result_point_3 = Scale_to_screen(projection_clipped_face.P3);

                                                // Why? (and check rounding)
                                                result_point_1 = Change_Y_Axis(result_point_1);
                                                result_point_2 = Change_Y_Axis(result_point_2);
                                                result_point_3 = Change_Y_Axis(result_point_3);

                                                // More variable simplification
                                                int result_point_1_x = (int)Math.Round(result_point_1.X, MidpointRounding.AwayFromZero);
                                                int result_point_1_y = (int)Math.Round(result_point_1.Y, MidpointRounding.AwayFromZero);
                                                double result_point_1_z = result_point_1.Z;
                                                int result_point_2_x = (int)Math.Round(result_point_2.X, MidpointRounding.AwayFromZero);
                                                int result_point_2_y = (int)Math.Round(result_point_2.Y, MidpointRounding.AwayFromZero);
                                                double result_point_2_z = result_point_2.Z;
                                                int result_point_3_x = (int)Math.Round(result_point_3.X, MidpointRounding.AwayFromZero);
                                                int result_point_3_y = (int)Math.Round(result_point_3.Y, MidpointRounding.AwayFromZero);
                                                double result_point_3_z = result_point_3.Z;

                                                // Finally draw the triangle
                                                if (face.Draw_Outline)
                                                {
                                                    // Method for drawing edges?
                                                    Triangle(result_point_1_x , result_point_1_y, result_point_1_z, result_point_2_x, result_point_2_y, result_point_2_z, result_point_3_x, result_point_3_y, result_point_3_z, face_colour);
                                                }
                                                else
                                                {
                                                    if (face.Texture == null)
                                                    {
                                                        Triangle(result_point_1_x, result_point_1_y, result_point_1_z, result_point_2_x, result_point_2_y, result_point_2_z, result_point_3_x, result_point_3_y, result_point_3_z, face_colour);
                                                    }
                                                    else
                                                    {
                                                        // Scale the texture co-ordinates
                                                        int width = face.Texture.Width - 1;
                                                        int height = face.Texture.Height - 1;

                                                        // AFTERWARDS?
                                                        int result_texture_point_1_x = Round_To_Int(texture_point_1_x * width);
                                                        int result_texture_point_1_y = Round_To_Int(texture_point_1_y * height);
                                                        int result_texture_point_2_x = Round_To_Int(texture_point_2_x * width);
                                                        int result_texture_point_2_y = Round_To_Int(texture_point_2_y * height);
                                                        int result_texture_point_3_x = Round_To_Int(texture_point_3_x * width);
                                                        int result_texture_point_3_y = Round_To_Int(texture_point_3_y * height);

                                                        Textured_Triangle(result_point_1_x, result_point_1_y, result_point_1_z, result_point_2_x, result_point_2_y, result_point_2_z, result_point_3_x, result_point_3_y, result_point_3_z, result_texture_point_1_x, result_texture_point_1_y, result_texture_point_2_x, result_texture_point_2_y, result_texture_point_3_x, result_texture_point_3_y, face.Texture);
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    /*
                                            // between [-1,1]
                                            // between [0,2] (+1)
                                            // => between [0,1] (/2)
                                            // => between [0,width-1] (*(width-1))

                                            // RANGE TO DRAW X: [0,WIDTH-1] Y: [0,HEIGHT-1]
                                            

                                            // ROUDNING?
                                    */
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

                                    Clipped_Edge world_new_edge = new Clipped_Edge(point_1, point_2, shape.Render_Mesh.Edge_Colour, shape.Render_Mesh.Visible);
                                    bool world_draw_edge = true;

                                    foreach (Clipping_Plane world_clipping_plane in world_clipping_planes)
                                    {
                                        world_new_edge = Clip_Line(world_clipping_plane.Point, world_clipping_plane.Normal, world_new_edge);
                                        if (world_new_edge == null)
                                        {
                                            // != null
                                            world_draw_edge = false;
                                            break;
                                        }
                                    }

                                    if (world_draw_edge)
                                    {
                                        Clipped_Edge projection_new_edge = world_new_edge;
                                        projection_new_edge.P1 = camera.Apply_Camera_Matrices(projection_new_edge.P1);
                                        projection_new_edge.P2 = camera.Apply_Camera_Matrices(projection_new_edge.P2);

                                        projection_new_edge.P1 = camera.Divide_By_W(projection_new_edge.P1);
                                        projection_new_edge.P2 = camera.Divide_By_W(projection_new_edge.P2);

                                        bool projection_draw_edge = true;

                                        foreach (Clipping_Plane projection_clipping_plane in projection_clipping_planes)
                                        {
                                            projection_new_edge = Clip_Line(projection_clipping_plane.Point, projection_clipping_plane.Normal, projection_new_edge);
                                            if (projection_new_edge == null)
                                            {
                                                projection_draw_edge = false;
                                                break;
                                            }
                                        }

                                        if (projection_draw_edge)
                                        {
                                            // Scale to screen space?
                                            Vector4D result_point_1 = Scale_to_screen(projection_new_edge.P1);
                                            Vector4D result_point_2 = Scale_to_screen(projection_new_edge.P2);

                                            // WHHY?
                                            result_point_1 = Change_Y_Axis(result_point_1);
                                            result_point_2 = Change_Y_Axis(result_point_2);

                                            int result_point_1_x = (int)Math.Round(result_point_1.X, MidpointRounding.AwayFromZero);
                                            int result_point_1_y = (int)Math.Round(result_point_1.Y, MidpointRounding.AwayFromZero);
                                            double result_point_1_z = result_point_1.Z;
                                            int result_point_2_x = (int)Math.Round(result_point_2.X, MidpointRounding.AwayFromZero);
                                            int result_point_2_y = (int)Math.Round(result_point_2.Y, MidpointRounding.AwayFromZero);
                                            double result_point_2_z = result_point_2.Z;

                                            // Finally draw the line
                                            Line(result_point_1_x, result_point_1_y, result_point_1_z, result_point_2_x, result_point_2_y, result_point_2_z, shape.Render_Mesh.Edge_Colour);
                                        }
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
                Change_scene = false;
            }
        }

        public Vector4D Scale_to_screen(Vector4D vertex) => Transform.Scale(0.5 * (Width - 1), 0.5 * (Height - 1), 1) * Transform.Translate(new Vector3D(1, 1, 0)) * vertex;

        // Only do at drawing stage? v
        public Vector4D Change_Y_Axis(Vector4D vertex) => Transform.Translate(new Vector3D(0, Height - 1, 0)) * Transform.Scale_Y(-1) * vertex;
    }
}