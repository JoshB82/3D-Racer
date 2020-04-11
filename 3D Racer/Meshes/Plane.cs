using System.Diagnostics;
using System.Drawing;

namespace _3D_Racer
{
    public sealed class Plane : Mesh
    {
        public Plane(Vector3D position, Vector3D direction, Vector3D normal, double length, double width, string texture_path = null,
            Color? vertex_colour = null,
            Color? edge_colour = null,
            Color? face_colour = null)
        {
            Vertex_Colour = vertex_colour ?? Color.Blue;
            Edge_Colour = edge_colour ?? Color.Black;
            Face_Colour = face_colour ?? Color.FromArgb(0xFF, 0x00, 0xFF, 0x00); // Green

            Visible = true;

            World_Origin = new Vertex(position.X, position.Y, position.Z, 1);
            Set_Shape_Direction_1(direction, normal);

            Model_Vertices = new Vertex[4]
            {
                new Vertex(0, 0, 0, Vertex_Colour), // 0
                new Vertex(1, 0, 0, Vertex_Colour), // 1
                new Vertex(1, 0, 1, Vertex_Colour), // 2
                new Vertex(0, 0, 1, Vertex_Colour) // 3
            };

            Edges = new Edge[5]
            {
                new Edge(0, 1, Edge_Colour), // 0
                new Edge(1, 2, Edge_Colour), // 1
                new Edge(0, 2, Edge_Colour, false), // 2
                new Edge(2, 3, Edge_Colour), // 3
                new Edge(0, 3, Edge_Colour) //4
            };

            Faces = new Face[2]
            {
                new Face(0, 1, 2, Face_Colour, texture_path), // 0
                new Face(0, 2, 3, Face_Colour, texture_path) // 1
            };

            Scaling = new Vector3D(length, 1, width);
            Translation = position;

            Debug.WriteLine($"Plane created at ({position.X}, {position.Y}, {position.Z})");
        }
    }
}