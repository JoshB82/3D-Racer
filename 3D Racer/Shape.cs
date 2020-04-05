namespace _3D_Racer
{
    public class Shape
    {
        public Mesh Collision_Mesh { get; set; }
        public Mesh Render_Mesh { get; set; }

        // Settings
        /// <summary>
        /// Determines if the shape is selected or not.
        /// </summary>
        public bool Selected { get; set; }
        public bool Visible { get; set; }

        public int ID { get; protected set; }
        private static int next_id = -1;
        protected static int Get_Next_ID()
        {
            next_id++;
            return next_id;
        }

        public Shape(Mesh render_mesh, bool selected = false) : this(render_mesh, render_mesh, selected) {}

        public Shape(Mesh collision_mesh, Mesh render_mesh, bool selected = false)
        {
            Collision_Mesh = collision_mesh;
            Render_Mesh = render_mesh;

            ID = Get_Next_ID();
            Selected = selected;
        }
    }
}