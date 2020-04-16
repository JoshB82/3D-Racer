using System.Diagnostics;
using System.Drawing;

namespace _3D_Racer
{
    /// <summary>
    /// Handles creation of a plane mesh.
    /// </summary>
    public sealed class Plane : Mesh
    {
        public Vector3D position;
        public Vector3D Position
        {
            get { return position; }
            set
            {
                position = value;
                Translation = new Vector3D(position.X, position.Y, position.Z);
            }
        }
        public double length, width;
        public double Length
        {
            get { return length; }
            set
            {
                length = value;
                Scaling = new Vector3D(length, 1, width);
            }
        }
        public double Width
        {
            get { return width; }
            set
            {
                width = value;
                Scaling = new Vector3D(length, 1, width);
            }
        }

        public Plane(Vector3D position, Vector3D direction, Vector3D normal, double length, double width, bool draw_outline = false, bool visible = true,
            Color? vertex_colour = null,
            Color? edge_colour = null,
            Color? face_colour = null)
        {
            Position = position;
            Length = length;
            Width = width;
            Draw_Outline = draw_outline;
            Visible = visible;

            Vertex_Colour = vertex_colour ?? Color.Blue;
            Edge_Colour = edge_colour ?? Color.Black;
            Face_Colour = face_colour ?? Color.FromArgb(0xFF, 0x00, 0xFF, 0x00); // Green

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
                new Face(0, 1, 2, Face_Colour, draw_outline, visible), // 0
                new Face(0, 2, 3, Face_Colour, draw_outline, visible) // 1
            };

            Debug.WriteLine($"Plane created at ({position.X}, {position.Y}, {position.Z})");
        }

        public Plane(Vector3D position, Vector3D direction, Vector3D normal, double length, double width, Bitmap texture, bool draw_outline = false, bool visible = true,
            Color? vertex_colour = null,
            Color? edge_colour = null)
        {
            Position = position;
            Length = length;
            Width = width;
            Draw_Outline = draw_outline;
            Visible = visible;

            Vertex_Colour = vertex_colour ?? Color.Blue;
            Edge_Colour = edge_colour ?? Color.Black;

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
                new Face(0, 1, 2, 3, 2, 0, texture, draw_outline, visible), // 0
                new Face(0, 2, 3, 3, 0, 1, texture, draw_outline, visible) // 1
            };

            Textures = new Bitmap[1]
            {
                texture // 0
            };

            Texture_Vertices = new Texture_Vertex[4]
            {
                new Texture_Vertex(0, 0, 1), // 0
                new Texture_Vertex(1, 0, 1), // 1
                new Texture_Vertex(0, 1, 1), // 2
                new Texture_Vertex(1, 1, 1) // 3
            };

            Debug.WriteLine($"Plane created at ({position.X}, {position.Y}, {position.Z})");
        }
    }
}