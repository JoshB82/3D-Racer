using System;
using System.Drawing;

namespace _3D_Racer
{
    public abstract partial class Mesh
    {
        #region Structure
        public Vertex Model_Origin { get; } = new Vertex(0, 0, 0, 1);
        public Vertex World_Origin { get; set; }
        public Vertex Camera_Origin { get; protected set; }

        private Vertex[] model_vertices;
        public Vertex[] Model_Vertices {
            get { return model_vertices; }
            protected set
            {
                model_vertices = value;
                World_Vertices = new Vertex[value.Length];
                Camera_Vertices = new Vertex[value.Length];
            }
        }

        public Vertex[] World_Vertices { get; private set; }
        public Vertex[] Camera_Vertices { get; protected set; }

        public Texture_Vertex[] Texture_Vertices { get; protected set; }
        public Texture_Face[] Texture_Faces { get; protected set; }
        public Bitmap_Texture[] Textures { get; protected set; }

        public Edge[] Edges { get; protected set; }
        public Face[] Faces { get; protected set; }
        #endregion

        #region Directions
        public Vector3D Model_Direction { get; } = Vector3D.Unit_X;
        public Vector3D Model_Direction_Up { get; } = Vector3D.Unit_Y;
        public Vector3D Model_Direction_Right { get; } = Vector3D.Unit_Z;

        public Vector3D World_Direction { get; private set; }
        public Vector3D World_Direction_Up { get; private set; }
        public Vector3D World_Direction_Right { get; private set; }

        public void Set_Shape_Direction_1(Vector3D new_world_direction, Vector3D new_world_direction_up)
        {
            if (new_world_direction * new_world_direction_up != 0) throw new Exception("Shape direction vectors are not orthogonal.");
            new_world_direction = new_world_direction.Normalise(); new_world_direction_up = new_world_direction_up.Normalise();
            World_Direction = new_world_direction;
            World_Direction_Up = new_world_direction_up;
            World_Direction_Right = new_world_direction.Cross_Product(new_world_direction_up);
        }
        public void Set_Shape_Direction_2(Vector3D new_world_direction_up, Vector3D new_world_direction_right)
        {
            if (new_world_direction_up * new_world_direction_right != 0) throw new Exception("Shape direction vectors are not orthogonal.");
            new_world_direction_up = new_world_direction_up.Normalise(); new_world_direction_right = new_world_direction_right.Normalise();
            World_Direction = new_world_direction_up.Cross_Product(new_world_direction_right); ;
            World_Direction_Up = new_world_direction_up;
            World_Direction_Right = new_world_direction_right;
        }
        public void Set_Shape_Direction_3(Vector3D new_world_direction_right, Vector3D new_world_direction)
        {
            if (new_world_direction_right * new_world_direction != 0) throw new Exception("Shape direction vectors are not orthogonal.");
            new_world_direction_right = new_world_direction_right.Normalise(); new_world_direction = new_world_direction.Normalise();
            World_Direction = new_world_direction;
            World_Direction_Up = new_world_direction_right.Cross_Product(new_world_direction);
            World_Direction_Right = new_world_direction_right;
        }
        #endregion

        # region Draw settings
        public bool Draw_Vertices { get; set; } = true;
        public bool Draw_Edges { get; set; } = true;
        public bool Draw_Faces { get; set; } = true;
        #endregion

        #region Colours
        public Color Vertex_Colour { get; set; }
        public Color Edge_Colour { get; set; }
        public Color Face_Colour { get; set; }
        #endregion

        // Miscellaneous
        /// <summary>
        /// Determines if the mesh is visible or not.
        /// </summary>
        public bool Visible { get; set; }

        // Object transformations
        public Matrix4x4 Model_to_world { get; private set; }
        public Vector3D Scaling { get; protected set; }
        public Vector3D Translation { get; protected set; }

        // Scale, then rotate, then translate
        public void Calculate_Model_to_World_Matrix() => Model_to_world = Transform.Translate(Translation) * Transform.Quaternion_Rotation_Matrix(Model_Direction, World_Direction) * Transform.Scale(Scaling.X, Scaling.Y, Scaling.Z);

        public void Apply_World_Matrices()
        {
            World_Origin = Model_to_world * Model_Origin;
            for (int i = 0; i < Model_Vertices.Length; i++) World_Vertices[i] = Model_to_world * Model_Vertices[i];
        }

        /*
        public Mesh(Vertex world_origin, Vertex[] model_vertices, Edge[] edges, Face[] faces)
        {
            World_Origin = world_origin;
            Model_Vertices = model_vertices;
            Edges = edges;
            Faces = faces;
        }
        */
    }
}