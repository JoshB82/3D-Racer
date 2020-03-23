using System;
using System.Drawing;

namespace _3D_Racer
{
    public abstract partial class Mesh
    {
        // Structure
        public Vertex Model_Origin { get; set; }
        public Vertex World_Origin { get; set; }
        public Vertex Camera_Origin { get; set; }

        /// <summary>
        /// Arrays of child vertices.
        /// </summary>
        public Vertex[] Model_Vertices { get; set; }
        public Vertex[] World_Vertices { get; set; }
        public Vertex[] Camera_Vertices { get; set; }

        public Vector3D Model_Direction { get; set; }
        public Vector3D World_Direction { get; set; }

        public Edge[] Edges { get; set; }
        public Face[] Faces { get; set; }

        // Draw settings
        public bool Draw_Vertices { get; set; } = true;
        public bool Draw_Edges { get; set; } = true;
        public bool Draw_Faces { get; set; } = true;

        // Colours
        public Color Vertex_Colour {get; set;}
        public Color Edge_Colour { get; set; }
        public Color Face_Colour { get; set; }

        // Miscellaneous
        /// <summary>
        /// Determines if the mesh is visible or not.
        /// </summary>
        public bool Visible { get; set; }

        // Object transformations
        public Matrix4x4 Model_to_world { get; set; }
        public Vector3D Scaling { get; protected set; }
        public Vector3D Translation { get; protected set; }

        public void Calculate_Model_to_World_Matrix() => Model_to_world = Transform.Translate(Translation) * Transform.Scale(Scaling.X, Scaling.Y, Scaling.Z) * Transform.Quaternion_to_Matrix(Transform.Quaternion_Rotation_Between_Vectors(Model_Direction, World_Direction));

        public void Apply_World_Matrices()
        {
            World_Origin = Model_to_world * Model_Origin;
            if (Model_Vertices != null) for (int i = 0; i < Model_Vertices.Length; i++) World_Vertices[i] = Model_to_world * Model_Vertices[i];
        }

        public void Apply_Camera_Matrices(Camera camera)
        {
            Camera_Origin = camera.World_to_screen * World_Origin;
            if (World_Vertices != null) for (int i = 0; i < World_Vertices.Length; i++) Camera_Vertices[i] = camera.World_to_screen * World_Vertices[i];
        }

        public void Divide_by_W()
        {
            Camera_Origin /= Camera_Origin.W;
            for (int i = 0; i < Camera_Vertices.Length; i++) Camera_Vertices[i] /= Camera_Vertices[i].W;
        }

        /// <summary>
        /// Round vertex co-ordinates to integers.
        /// </summary>
        public void Round_Vertices()
        {
            Camera_Origin.Round();
            for (int i = 0; i < Camera_Vertices.Length; i++) Camera_Vertices[i].Round();
        }

        public void Scale_to_Screen(int width, int height)
        {
            Camera_Origin = Transform.Scale_Y(0.5f * height) * Transform.Scale_X(0.5f * width) * Transform.Translate(new Vector3D(1, 1, 0)) * Camera_Origin;
            for (int i = 0; i < Camera_Vertices.Length; i++) Camera_Vertices[i] = Transform.Scale_Y(0.5f * height) * Transform.Scale_X(0.5f * width) * Transform.Translate(new Vector3D(1, 1, 0)) * Camera_Vertices[i];
        }

        // Only do at drawing stage? ^v
        public void Change_Y_Axis(int height)
        {
            Camera_Origin = Transform.Translate(new Vector3D(0, height, 0)) * Transform.Scale_Y(-1) * Camera_Origin;
            for (int i = 0; i < Camera_Vertices.Length; i++) Camera_Vertices[i] = Transform.Translate(new Vector3D(0, height, 0)) * Transform.Scale_Y(-1) * Camera_Vertices[i];
        }
    }
}