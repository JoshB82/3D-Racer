using System;
using System.Diagnostics;

namespace _3D_Racer
{
    public abstract class Shape
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
        Matrix object_to_world { get; set; } = new Matrix(4);
        Matrix world_to_camera { get; set; }
        Matrix camera_to_screen { get; set; }
        Matrix object_to_screen { get; set; }

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
            object_to_screen = camera_to_screen * (world_to_camera * object_to_world);
        }
    }

    public sealed class Cube : Shape
    {
        public Cube(double x, double y, double z, double side_length,
            string vertex_colour = "000000",
            string edge_colour = "000000",
            string face_colour = "000000")
        {
            Origin = new Vertex(x, y, z, null, false);
            Type = "Cube";
            Visible = true;
            Selected = false;

            Vertices = new Vertex[8]
            {
                new Vertex(x - side_length / 2, y - side_length / 2, z - side_length / 2, vertex_colour), // 0
                new Vertex(x + side_length / 2, y - side_length / 2, z - side_length / 2, vertex_colour), // 1
                new Vertex(x + side_length / 2, y + side_length / 2, z - side_length / 2, vertex_colour), // 2
                new Vertex(x - side_length / 2, y + side_length / 2, z - side_length / 2, vertex_colour), // 3
                new Vertex(x - side_length / 2, y - side_length / 2, z + side_length / 2, vertex_colour), // 4
                new Vertex(x + side_length / 2, y - side_length / 2, z + side_length / 2, vertex_colour), // 5
                new Vertex(x + side_length / 2, y + side_length / 2, z + side_length / 2, vertex_colour), // 6
                new Vertex(x - side_length / 2, y + side_length / 2, z + side_length / 2, vertex_colour) // 7
            };

            Edges = new Edge[18]
            {
                new Edge(0, 1, edge_colour), // 0
                new Edge(1, 2, edge_colour), // 1
                new Edge(0, 2, edge_colour, false), // 2
                new Edge(2, 3, edge_colour), // 3
                new Edge(0, 3, edge_colour), // 4
                new Edge(1, 5, edge_colour), // 5
                new Edge(5, 6, edge_colour), // 6
                new Edge(1, 6, edge_colour, false), // 7
                new Edge(2, 6, edge_colour), // 8
                new Edge(4, 5, edge_colour), // 9
                new Edge(4, 7, edge_colour), // 10
                new Edge(5, 7, edge_colour, false), // 11
                new Edge(6, 7, edge_colour), // 12
                new Edge(0, 4, edge_colour), // 13
                new Edge(3, 4, edge_colour, false),  // 14
                new Edge(3, 7, edge_colour), // 15
                new Edge(3, 6, edge_colour, false), // 16
                new Edge(1, 4, edge_colour, false) // 17
            };

            Faces = new Face[12]
            {
                new Face(0, 1, 2, face_colour), // 0
                new Face(2, 3, 4, face_colour), // 1
                new Face(5, 6, 7, face_colour), // 2
                new Face(7, 8, 1, face_colour), // 3
                new Face(9, 10, 11, face_colour), // 4
                new Face(11, 12, 6, face_colour), // 5
                new Face(13, 4, 14, face_colour), // 6
                new Face(14, 15, 10, face_colour), // 7
                new Face(3, 8, 16, face_colour), // 8
                new Face(16, 12, 15, face_colour), // 9
                new Face(9, 5, 17, face_colour), // 10
                new Face(17, 0, 13, face_colour) // 11
            };
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