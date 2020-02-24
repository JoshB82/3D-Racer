using System;
using System.Diagnostics;

namespace _3D_Racer
{
    private abstract partial class Shape
    {
        // Object structure
        /// <summary>
        /// Parent vertex to all other vertices in object.
        /// </summary>
        public Vertex Origin { get; set; }
        /// <summary>
        /// Array of child vertices.
        /// </summary>
        public Vertex[] Vertices { get; set; }
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
        Matrix model_to_world { get; set; } = new Matrix(4);
        Matrix world_to_camera { get; set; }
        Matrix camera_to_screen { get; set; }
        Matrix model_to_screen { get; set; }

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

        /// <summary>
        /// Rotates a shape by a given angle about the x - axis
        /// </summary>
        /// <param name="angle">Angle to rotate shape by</param>
        public void RotateX(double angle)
        {
            double sin_angle = Math.Sin(angle);
            double cos_angle = Math.Cos(angle);
            Matrix rotation_matrix = new Matrix(1, 0, 0, 0, cos_angle, -sin_angle, 0, sin_angle, cos_angle);
            Origin = rotation_matrix * Origin;
            for (int i = 0; i < Vertices.Length; i++) Vertices[i] = rotation_matrix * Vertices[i];
        }

        /// <summary>
        /// Rotates a shape by a given angle about the y - axis
        /// </summary>
        /// <param name="angle">Angle to rotate shape by</param>
        public void RotateY(double angle)
        {
            double sin_angle = Math.Sin(angle);
            double cos_angle = Math.Cos(angle);
            Matrix rotation_matrix = new Matrix(cos_angle, 0, sin_angle, 0, 1, 0, -sin_angle, 0, cos_angle);
            Origin = rotation_matrix * Origin;
            for (int i = 0; i < Vertices.Length; i++) Vertices[i] = rotation_matrix * Vertices[i];
        }

        /// <summary>
        /// Rotates a shape by a given angle about the z - axis
        /// </summary>
        /// <param name="angle">Angle to rotate shape by</param>
        public void RotateZ(double angle)
        {
            double sin_angle = Math.Sin(angle);
            double cos_angle = Math.Cos(angle);
            Matrix rotation_matrix = new Matrix(cos_angle, -sin_angle, 0, sin_angle, cos_angle, 0, 0, 0, 1);
            Origin = rotation_matrix * Origin;
            for (int i = 0; i < Vertices.Length; i++) Vertices[i] = rotation_matrix * Vertices[i];
        }

        public void RecalculateMatrix()
        {
            model_to_screen = camera_to_screen * (world_to_camera * model_to_world);
        }
    }

    public sealed class Cuboid : Shape
    {
        public Cuboid(double x, double y, double z, double length, double width, double height, string colour)
        {
            Origin = new Vertex(x, y, z, null, false);
            Type = "Cuboid";
            Visible = true;
            Selected = false;
        }
    }
}