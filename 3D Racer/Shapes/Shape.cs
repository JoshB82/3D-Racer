using System.Drawing;

namespace _3D_Racer
{
    public abstract partial class Shape
    {
        // Object structure
        /// <summary>
        /// Parent vertex to all other vertices in object.
        /// </summary>
        public Vector Model_Origin { get; set; }
        public Vector World_Origin { get; set; }
        public Vector Camera_Origin { get; set; }

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
        public string Vertex_Colour {get; set;}
        public string Edge_Colour { get; set; }
        public string Face_Colour { get; set; }

        // Object transformations
        public Matrix Model_to_world { get; set; }
        
        // Miscellaneous
        public bool Visible { get; set; }
        public bool Selected { get; set; }

        public void Apply_Matrices(Camera camera)
        {
            World_Origin = Model_to_world * Model_Origin;
            for (int i = 0; i < Model_Vertices.Length; i++) World_Vertices[i] = Model_to_world * Model_Vertices[i];
            Camera_Origin = camera.World_to_screen * World_Origin;
            for (int i = 0; i < Model_Vertices.Length; i++) Camera_Vertices[i] = camera.World_to_screen * World_Vertices[i];
        }

        public void Draw_Shape(Camera camera, Graphics g)
        {
            Apply_Matrices(camera);

            using (SolidBrush green_brush = new SolidBrush(Color.Green))
            using (SolidBrush blue_brush = new SolidBrush(Color.Blue))
            using (Pen pen = new Pen(Color.Black))
            {
                // Draw vertices
                foreach (Vertex vertex in Camera_Vertices)
                {
                    if (vertex.Visible) g.FillEllipse(blue_brush, vertex.X - vertex.Radius / 2, vertex.Y - vertex.Radius / 2, vertex.Radius, vertex.Radius);
                }

                // Draw edges
                foreach (Edge edge in Edges)
                {
                    if (edge.Visible) g.DrawLine(pen, Camera_Vertices[edge.P1].X, Camera_Vertices[edge.P1].Y, Camera_Vertices[edge.P2].X, Camera_Vertices[edge.P2].Y);
                }

                // Draw faces
                foreach (Face face in Faces)
                {
                    if (face.Visible) g.FillPolygon(green_brush, new PointF[3] {
                        new PointF(Camera_Vertices[face.P1].X, Camera_Vertices[face.P1].Y),
                        new PointF(Camera_Vertices[face.P2].X, Camera_Vertices[face.P2].Y),
                        new PointF(Camera_Vertices[face.P3].X, Camera_Vertices[face.P3].Y) });
                }
            }
        }
    }
}