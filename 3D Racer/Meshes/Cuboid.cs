using System.Diagnostics;
using System.Drawing;

namespace _3D_Racer
{
    /// <summary>
    /// Handles creation of a cuboid mesh.
    /// </summary>
    public sealed class Cuboid : Mesh
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

        public double length, width, height;
        public double Length
        {
            get { return length; }
            set
            {
                length = value;
                Scaling = new Vector3D(length, width, height);
            }
        }
        public double Width
        {
            get { return width; }
            set
            {
                width = value;
                Scaling = new Vector3D(length, width, height);
            }
        }
        public double Height
        {
            get { return height; }
            set
            {
                height = value;
                Scaling = new Vector3D(length, width, height);
            }
        }

        public Cuboid(Vector3D position, double length, double width, double height, bool draw_outline = false, bool visible = true,
            Color? vertex_colour = null,
            Color? edge_colour = null,
            Color? face_colour = null)
        {
            Position = position;
            Length = length;
            Width = width;
            Height = height;
            Draw_Outline = draw_outline;
            Visible = visible;

            Vertex_Colour = vertex_colour ?? Color.Blue;
            Edge_Colour = edge_colour ?? Color.Black;
            Face_Colour = face_colour ?? Color.FromArgb(0xFF, 0x00, 0xFF, 0x00); // Green

            World_Origin = new Vertex(position.X, position.Y, position.Z);
            Set_Shape_Direction_1(Vector3D.Unit_X, Vector3D.Unit_Y);

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

            Edges = new Edge[18]
            {
                new Edge(0, 1, Edge_Colour, false), // 0
                new Edge(1, 2, Edge_Colour, false), // 1
                new Edge(0, 2, Edge_Colour, false), // 2
                new Edge(2, 3, Edge_Colour, false), // 3
                new Edge(0, 3, Edge_Colour, false), // 4
                new Edge(1, 5, Edge_Colour, false), // 5
                new Edge(5, 6, Edge_Colour, false), // 6
                new Edge(1, 6, Edge_Colour, false), // 7
                new Edge(2, 6, Edge_Colour, false), // 8
                new Edge(4, 5, Edge_Colour, false), // 9
                new Edge(4, 7, Edge_Colour, false), // 10
                new Edge(5, 7, Edge_Colour, false), // 11
                new Edge(6, 7, Edge_Colour, false), // 12
                new Edge(0, 4, Edge_Colour, false), // 13
                new Edge(3, 4, Edge_Colour, false),  // 14
                new Edge(3, 7, Edge_Colour, false), // 15
                new Edge(3, 6, Edge_Colour, false), // 16
                new Edge(1, 4, Edge_Colour, false) // 17
            };

            Faces = new Face[12]
            {
                new Face(0, 1, 2, Face_Colour, draw_outline, visible), // 0
                new Face(0, 2, 3, Face_Colour, draw_outline, visible), // 1
                new Face(1, 6, 2, Face_Colour, draw_outline, visible), // 2
                new Face(1, 5, 6, Face_Colour, draw_outline, visible), // 3
                new Face(4, 7, 5, Face_Colour, draw_outline, visible), // 4
                new Face(5, 7, 6, Face_Colour, draw_outline, visible), // 5
                new Face(0, 3, 4, Face_Colour, draw_outline, visible), // 6
                new Face(4, 3, 7, Face_Colour, draw_outline, visible), // 7
                new Face(7, 3, 6, Face_Colour, draw_outline, visible), // 8
                new Face(6, 3, 2, Face_Colour, draw_outline, visible), // 9
                new Face(4, 5, 0, Face_Colour, draw_outline, visible), // 10
                new Face(5, 1, 0, Face_Colour, draw_outline, visible) // 11
            };

            Debug.WriteLine($"Cuboid created at ({position.X}, {position.Y}, {position.Z})");
        }

        public Cuboid(Vector3D position, double length, double width, double height, Bitmap texture, bool draw_outline = false, bool visible = true,
            Color? vertex_colour = null,
            Color? edge_colour = null)
        {
            Position = position;
            Length = length;
            Width = width;
            Height = height;
            Draw_Outline = draw_outline;
            Visible = visible;

            Vertex_Colour = vertex_colour ?? Color.Blue;
            Edge_Colour = edge_colour ?? Color.Black;

            World_Origin = new Vertex(position.X, position.Y, position.Z);
            Set_Shape_Direction_1(Vector3D.Unit_X, Vector3D.Unit_Y);

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

            Edges = new Edge[18]
            {
                new Edge(0, 1, Edge_Colour, false), // 0
                new Edge(1, 2, Edge_Colour, false), // 1
                new Edge(0, 2, Edge_Colour, false), // 2
                new Edge(2, 3, Edge_Colour, false), // 3
                new Edge(0, 3, Edge_Colour, false), // 4
                new Edge(1, 5, Edge_Colour, false), // 5
                new Edge(5, 6, Edge_Colour, false), // 6
                new Edge(1, 6, Edge_Colour, false), // 7
                new Edge(2, 6, Edge_Colour, false), // 8
                new Edge(4, 5, Edge_Colour, false), // 9
                new Edge(4, 7, Edge_Colour, false), // 10
                new Edge(5, 7, Edge_Colour, false), // 11
                new Edge(6, 7, Edge_Colour, false), // 12
                new Edge(0, 4, Edge_Colour, false), // 13
                new Edge(3, 4, Edge_Colour, false),  // 14
                new Edge(3, 7, Edge_Colour, false), // 15
                new Edge(3, 6, Edge_Colour, false), // 16
                new Edge(1, 4, Edge_Colour, false) // 17
            };

            Faces = new Face[12]
            {
                new Face(0, 1, 2, 3, 2, 0, texture, draw_outline, visible), // 0
                new Face(0, 2, 3, 3, 0, 1, texture, draw_outline, visible), // 1
                new Face(1, 6, 2, 3, 0, 1, texture, draw_outline, visible), // 2
                new Face(1, 5, 6, 3, 2, 0, texture, draw_outline, visible), // 3
                new Face(4, 7, 5, 2, 0, 3, texture, draw_outline, visible), // 4
                new Face(5, 7, 6, 3, 0, 1, texture, draw_outline, visible), // 5
                new Face(0, 3, 4, 2, 0, 3, texture, draw_outline, visible), // 6
                new Face(4, 3, 7, 3, 0, 1, texture, draw_outline, visible), // 7
                new Face(7, 3, 6, 2, 0, 3, texture, draw_outline, visible), // 8
                new Face(6, 3, 2, 3, 0, 1, texture, draw_outline, visible), // 9
                new Face(4, 5, 0, 0, 1, 2, texture, draw_outline, visible), // 10
                new Face(5, 1, 0, 1, 3, 2, texture, draw_outline, visible) // 11
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

            Debug.WriteLine($"Cuboid created at ({position.X}, {position.Y}, {position.Z})");
        }
    }
}