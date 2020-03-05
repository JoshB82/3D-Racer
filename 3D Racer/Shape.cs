using System.Drawing;

namespace _3D_Racer
{
    public abstract partial class Shape
    {
        // Object structure
        /// <summary>
        /// Parent vertex to all other vertices in object.
        /// </summary>
        public Vector4D Model_Origin { get; set; }
        public Vector4D World_Origin { get; set; }
        public Vector4D Camera_Origin { get; set; }

        /// <summary>
        /// Arrays of child vertices.
        /// </summary>
        public Vertex[] Model_Vertices { get; set; }
        public Vertex[] World_Vertices { get; set; }
        public Vertex[] Camera_Vertices { get; set; }

        /// <summary>
        /// Array of edges comprised of two child vertices.
        /// </summary>
        public Edge[] Edges { get; set; }
        /// <summary>
        /// Array of faces comprised of three edges.
        /// </summary>
        public Face[] Faces { get; set; }

        // Draw settings
        public bool Draw_Vertices { get; set; } = true;
        public bool Draw_Edges { get; set; } = true;
        public bool Draw_Faces { get; set; } = true;

        // Colours
        public int Vertex_Colour {get; set;}
        public int Edge_Colour { get; set; }
        public int Face_Colour { get; set; }

        // Object transformations
        public Matrix4x4 Model_to_world { get; set; }
        
        // Miscellaneous
        /// <summary>
        /// Determines if the shape is visible or not.
        /// </summary>
        public bool Visible { get; set; }
        /// <summary>
        /// Determines if the shape is selected or not.
        /// </summary>
        public bool Selected { get; set; }

        public void Apply_World_Matrices()
        {
            World_Origin = Model_to_world * Model_Origin;
            if (Model_Vertices != null) for (int i = 0; i < Model_Vertices.Length; i++) World_Vertices[i] = Model_to_world * Model_Vertices[i];
        }

        public void Apply_Camera_Matrices(Camera camera)
        {
            camera.Recalculate_Matrix();
            Camera_Origin = camera.World_to_screen * World_Origin;
            if (World_Vertices != null) for (int i = 0; i < Model_Vertices.Length; i++) Camera_Vertices[i] = camera.World_to_screen * World_Vertices[i];
        }

        public void Divide_by_W()
        {
            for (int i = 0; i < Camera_Vertices.Length; i++)
            {
                Camera_Vertices[i].X /= Camera_Vertices[i].W;
                Camera_Vertices[i].Y /= Camera_Vertices[i].W;
                Camera_Vertices[i].Z /= Camera_Vertices[i].W;
                Camera_Vertices[i].W = 1;
            }
        }

        public void Scale_to_Screen()
        {
            for (int i = 0; i < Camera_Vertices.Length; i++)
            {
                Camera_Vertices[i] = Transform.Translate(1, 1, 0) * Camera_Vertices[i];
                Camera_Vertices[i] = Transform.Scale_X(0.5f) * Camera_Vertices[i];
                Camera_Vertices[i] = Transform.Scale_Y(0.5f) * Camera_Vertices[i];
                Camera_Vertices[i] = Transform.Scale_X(MainForm.Canvas_width) * Camera_Vertices[i];
                Camera_Vertices[i] = Transform.Scale_Y(MainForm.Canvas_height) * Camera_Vertices[i];
            }
        }

        public void Draw_Shape(Camera camera, Graphics g)
        {
            Apply_World_Matrices();
            Apply_Camera_Matrices(camera);
            Divide_by_W();
            Scale_to_Screen();

            using (SolidBrush face_brush = new SolidBrush(Color.FromArgb(Face_Colour)))
            using (SolidBrush vertex_brush = new SolidBrush(Color.FromArgb(Vertex_Colour)))
            using (Pen edge_pen = new Pen(Color.FromArgb(Edge_Colour)))
            {
                // Draw faces
                if (Camera_Vertices != null && Faces != null && Draw_Faces)
                {
                    foreach (Face face in Faces)
                    {
                        if (face.Visible) g.FillPolygon(vertex_brush, new PointF[3] {
                        new PointF(Camera_Vertices[face.P1].X, Camera_Vertices[face.P1].Y),
                        new PointF(Camera_Vertices[face.P2].X, Camera_Vertices[face.P2].Y),
                        new PointF(Camera_Vertices[face.P3].X, Camera_Vertices[face.P3].Y) });
                    }
                }

                // Draw edges
                if (Camera_Vertices != null && Edges != null && Draw_Edges)
                {
                    foreach (Edge edge in Edges)
                    {
                        if (edge.Visible) g.DrawLine(edge_pen, Camera_Vertices[edge.P1].X, Camera_Vertices[edge.P1].Y, Camera_Vertices[edge.P2].X, Camera_Vertices[edge.P2].Y);
                    }
                }

                // Draw vertices
                if (Camera_Vertices != null && Draw_Vertices)
                {
                    foreach (ShapeComponents vertex in Camera_Vertices)
                    {
                        if (vertex.Visible) g.FillEllipse(face_brush, vertex.X - vertex.Radius / 2, vertex.Y - vertex.Radius / 2, vertex.Radius, vertex.Radius);
                    }
                }
            }
        }

        public Shape Get_Shape_From_File(string file)
        {
            return null;
        }
    }
}