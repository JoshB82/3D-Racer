using System.Diagnostics;
using System.Drawing;

namespace _3D_Racer
{
    public sealed class Line : Mesh
    {
        public Line(Vector3D start_position, Vector3D end_position, Color? vertex_colour = null, Color? edge_colour = null)
        {
            Vertex_Colour = vertex_colour ?? Color.FromArgb(0xFF, 0x00, 0x00, 0xFF);
            Edge_Colour = edge_colour ?? Color.FromArgb(0xFF, 0x00, 0x00, 0x00);

            Visible = true;

            Model_Origin = new Vertex(0, 0, 0, 1);
            World_Origin = new Vertex(start_position.X, start_position.Y, start_position.Z, 1);
            Model_Direction = Vector3D.Unit_X;
            World_Direction = end_position - start_position;

            Model_Vertices = new Vertex[2]
            {
                new Vertex(0, 0, 0, Vertex_Colour),
                new Vertex(1, 0, 0, Vertex_Colour)
            };

            World_Vertices = new Vertex[2];
            Camera_Vertices = new Vertex[2];

            Edges = new Edge[1]
            {
                new Edge(0, 1, Edge_Colour)
            };

            Draw_Faces = false;

            Scaling = new Vector3D(end_position.X - start_position.X, end_position.Y - start_position.Y, end_position.Z - start_position.Z);
            Translation = start_position;

            Debug.WriteLine("Line created at (" + start_position.X + "," + start_position.Y + "," + start_position.Z + ")");
        }
    }
}