using System.Diagnostics;
using System.Drawing;

namespace _3D_Racer
{
    public sealed class Cube : Shape
    {
        public Cube(Vector3D position, float side_length,
            Color? vertex_colour = null,
            Color? edge_colour = null,
            Color? face_colour = null)
        {
            ID = Get_Next_ID();
            
            Vertex_Colour = vertex_colour ?? Color.FromArgb(0xFF,0x00,0x00,0xFF);
            Edge_Colour = edge_colour ?? Color.FromArgb(0xFF, 0x00, 0x00, 0x00);
            Face_Colour = face_colour ?? Color.FromArgb(0xFF, 0x00, 0xFF, 0x00);
            Selected = false;
            Visible = true;

            Model_Origin = new Vector4D(0, 0, 0, 1);
            World_Origin = new Vector4D(position.X, position.Y, position.Z, 1);
            Model_Direction = Vector3D.Unit_X;
            World_Direction = Vector3D.Unit_X;

            Model_Vertices = new Vertex[8]
            {
                new Vertex(0, 0, 0, Vertex_Colour), // 0
                new Vertex(1, 0, 0, Vertex_Colour), // 1
                new Vertex(1, 1, 0, Vertex_Colour), // 2
                new Vertex(0, 1, 0, Vertex_Colour), // 3
                new Vertex(0, 0, 1, Vertex_Colour), // 4
                new Vertex(1, 0, 1, Vertex_Colour), // 5
                new Vertex(1, 1, 1, Vertex_Colour), // 6
                new Vertex(0, 1, 1, Vertex_Colour) // 7
            };

            Camera_Vertices = new Vertex[8];
            World_Vertices = new Vertex[8];

            Edges = new Edge[18]
            {
                new Edge(0, 1, Edge_Colour), // 0
                new Edge(1, 2, Edge_Colour), // 1
                new Edge(0, 2, Edge_Colour, false), // 2
                new Edge(2, 3, Edge_Colour), // 3
                new Edge(0, 3, Edge_Colour), // 4
                new Edge(1, 5, Edge_Colour), // 5
                new Edge(5, 6, Edge_Colour), // 6
                new Edge(1, 6, Edge_Colour, false), // 7
                new Edge(2, 6, Edge_Colour), // 8
                new Edge(4, 5, Edge_Colour), // 9
                new Edge(4, 7, Edge_Colour), // 10
                new Edge(5, 7, Edge_Colour, false), // 11
                new Edge(6, 7, Edge_Colour), // 12
                new Edge(0, 4, Edge_Colour), // 13
                new Edge(3, 4, Edge_Colour, false),  // 14
                new Edge(3, 7, Edge_Colour), // 15
                new Edge(3, 6, Edge_Colour, false), // 16
                new Edge(1, 4, Edge_Colour, false) // 17
            };

            Faces = new Face[12]
            {
                new Face(0, 3, 2, Face_Colour), // 0
                new Face(0, 2, 1, Face_Colour), // 1
                new Face(1, 2, 6, Face_Colour), // 2
                new Face(1, 6, 5, Face_Colour), // 3
                new Face(5, 6, 7, Face_Colour), // 4
                new Face(5, 7, 4, Face_Colour), // 5
                new Face(4, 7, 3, Face_Colour), // 6
                new Face(4, 3, 0, Face_Colour), // 7
                new Face(3, 7, 6, Face_Colour), // 8
                new Face(3, 6, 2, Face_Colour), // 9
                new Face(0, 4, 5, Face_Colour), // 10
                new Face(0, 5, 1, Face_Colour) // 11
            };

            Scaling = new Vector3D(side_length, side_length, side_length);
            Translation = new Vector3D(position.X, position.Y, position.Z);

            Debug.WriteLine("Cube created at (" + position.X + "," + position.Y + "," + position.Z + ")");
        }
    }
}