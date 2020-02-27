using System;
using System.Diagnostics;

namespace _3D_Racer
{
    public abstract partial class Shape
    {
        // Object structure
        /// <summary>
        /// Parent vertex to all other vertices in object.
        /// </summary>
        public Vector Origin { get; set; }
        public Vector Camera_Origin { get; set; }

        /// <summary>
        /// Array of child vertices.
        /// </summary>
        public Vertex[] Vertices { get; set; }
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
        public Matrix Model_to_world { get; set; } = new Matrix(4);
        
        // Miscellaneous
        public bool Visible { get; set; }
        public string Type { get; set; }
        public bool Selected { get; set; }

        /// <summary>
        /// Translate object to a new position vector.
        /// </summary>
        /// <param name="new_location">New position vector.</param>
        public void MoveTo(Vector new_location) => MoveBy(new_location - Origin);

        /// <summary>
        /// Translate object by a given vector.
        /// </summary>
        /// <param name="offset_x">Vector to translate by.</param>
        public void MoveBy(Vector offset_x)
        {
            Origin += offset_x;
            for (int i = 0; i < Vertices.Length; i++) Vertices[i] += offset_x;
        }

        public void ApplyWorldMatrix()
        {
            Origin = Model_to_world * Origin;
            for (int i = 0; i < Vertices.Length; i++) Vertices[i] = Model_to_world * Vertices[i];
        }

        public void ApplyCameraMatrix(Camera camera)
        {
            Origin = camera.World_to_screen * Origin;
            for (int i = 0; i < Camera_Vertices.Length; i++) Camera_Vertices[i] = camera.World_to_screen * Camera_Vertices[i];
        }
    }
}