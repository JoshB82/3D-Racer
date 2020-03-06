using System.Drawing;

namespace _3D_Racer
{
    public abstract partial class Shape
    {
        public int ID { get; protected set; }
        private static int next_id = -1;
        protected int Get_Next_ID()
        {
            next_id++;
            return next_id;
        }

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
        public Color Vertex_Colour {get; set;}
        public Color Edge_Colour { get; set; }
        public Color Face_Colour { get; set; }

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
            Matrix4x4 all = camera.Camera_to_screen * camera.World_to_camera * Model_to_world;
            camera.World_to_screen = camera.Camera_to_screen * camera.World_to_camera;
            Camera_Origin = camera.World_to_screen * World_Origin;
            if (World_Vertices != null) for (int i = 0; i < World_Vertices.Length; i++) Camera_Vertices[i] = camera.World_to_screen * World_Vertices[i];
        }

        public void Divide_by_W()
        {
            Camera_Origin.X /= Camera_Origin.W;
            Camera_Origin.Y /= Camera_Origin.W;
            Camera_Origin.Z /= Camera_Origin.W;
            Camera_Origin.W = 1;

            for (int i = 0; i < Camera_Vertices.Length; i++)
            {
                Camera_Vertices[i].X /= Camera_Vertices[i].W;
                Camera_Vertices[i].Y /= Camera_Vertices[i].W;
                Camera_Vertices[i].Z /= Camera_Vertices[i].W;
                Camera_Vertices[i].W = 1;
            }
        }

        public void Scale_to_Screen(int width, int height)
        {
            Camera_Origin = Transform.Translate(1, 1, 0) * Camera_Origin;
            Camera_Origin = Transform.Scale_X(0.5f * width) * Camera_Origin;
            Camera_Origin = Transform.Scale_Y(0.5f * height) * Camera_Origin;

            for (int i = 0; i < Camera_Vertices.Length; i++)
            {
                Camera_Vertices[i] = Transform.Translate(1, 1, 0) * Camera_Vertices[i];
                Camera_Vertices[i] = Transform.Scale_X(0.5f * width) * Camera_Vertices[i];
                Camera_Vertices[i] = Transform.Scale_Y(0.5f * height) * Camera_Vertices[i];
            }
        }
    }
}