using System.Diagnostics;
using System.Drawing;

namespace _3D_Racer
{
    public sealed class Plane : Mesh
    {
        public Plane(Vector3D position, Vector3D normal, double length, double width,
            Color? vertex_colour = null,
            Color? edge_colour = null,
            Color? face_colour = null)
        {
            Vertex_Colour = vertex_colour ?? Color.Blue;
            Edge_Colour = edge_colour ?? Color.Black;
            Face_Colour = face_colour ?? Color.FromArgb(0xFF, 0x00, 0xFF, 0x00); // Green

            Visible = true;

            World_Origin = new Vertex(position.X, position.Y, position.Z, 1);
            Set_Shape_Direction_1(Vector3D.Unit_X, Vector3D.Unit_Y);

            Model_Vertices = new Vertex[4]
            {
                new Vertex(0, 0, 0, Vertex_Colour),
                new Vertex(1, 0, 0, Vertex_Colour),
                new Vertex(1, 0, 1, Vertex_Colour),
                new Vertex(0, 0, 1, Vertex_Colour)
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

            Scaling = new Vector3D(length, 1, width);
            Translation = position;

            Debug.WriteLine("Plane created at (" + position.X + "," + position.Y + "," + position.Z + ")");
        }
    }
}
