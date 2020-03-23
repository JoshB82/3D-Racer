using System.Diagnostics;
using System.Drawing;

namespace _3D_Racer
{
    public sealed class Plane : Mesh
    {
        public Plane(Vector3D position, Vector3D normal, float length, float width, Color? vertex_colour = null, Color? edge_colour = null, Color? face_colour = null)
        {
            Vertex_Colour = vertex_colour ?? Color.FromArgb(0xFF, 0x00, 0x00, 0xFF);
            Edge_Colour = edge_colour ?? Color.FromArgb(0xFF, 0x00, 0x00, 0x00);
            Face_Colour = face_colour ?? Color.FromArgb(0xFF, 0x00, 0xFF, 0x00);
            Visible = true;

            Model_Origin = new Vertex(0, 0, 0, 1);
            World_Origin = new Vertex(position.X, position.Y, position.Z, 1);
            Model_Direction = Vector3D.Unit_Y;
            World_Direction = normal;

            Model_Vertices = new Vertex[4]
            {
                new Vertex(0, 0, 0, Vertex_Colour),
                new Vertex(1, 0, 0, Vertex_Colour),
                new Vertex(1, 1, 0, Vertex_Colour),
                new Vertex(0, 1, 0, Vertex_Colour)
            };

            World_Vertices = new Vertex[4];
            Camera_Vertices = new Vertex[4];

            Edges = new Edge[5]
            {
                new Edge(0, 1, Edge_Colour),
                new Edge(1, 2, Edge_Colour),
                new Edge(0, 2, Edge_Colour, false),
                new Edge(2, 3, Edge_Colour),
                new Edge(0, 3, Edge_Colour)
            };

            Faces = new Face[2]
            {
                new Face(0, 3, 2, Face_Colour),
                new Face(0, 2, 1, Face_Colour)
            };

            Scaling = new Vector3D(length, width, 1);
            Translation = position;

            Debug.WriteLine("Plane created at (" + position.X + "," + position.Y + "," + position.Z + ")");
        }
    }
}
