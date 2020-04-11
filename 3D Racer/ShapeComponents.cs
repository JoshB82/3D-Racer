using System;
using System.Drawing;

namespace _3D_Racer
{
    public partial class Vertex : Vector4D
    {
        public Color Colour { get; set; }
        public int Diameter { get; set; }
        public bool Visible { get; set; }

        /// <summary>
        /// Create a new vertex.
        /// </summary>
        /// <param name="x">x - co-ordinate of the vertex.</param>
        /// <param name="y">y - co-ordinate of the vertex.</param>
        /// <param name="z">z - co-ordinate of the vertex.</param>
        /// <param name="colour">Eight digit hexadecimal ARGB colour value.</param>
        public Vertex(double x, double y, double z, Color? colour = null, bool visibility = true, int diameter = 10) : this(x, y, z, 1, colour, visibility, diameter) {}

        public Vertex(Vector3D position, double w = 1, Color? colour = null, bool visibility = true, int diameter = 10) : this(position.X, position.Y, position.Z, w, colour, visibility, diameter) {}

        public Vertex(double x, double y, double z, double w, Color? colour = null, bool visibility = true, int diameter = 10) : base(x, y, z, w)
        {
            Colour = colour ?? Color.Black;
            Diameter = diameter;
            Visible = visibility;
        }

        #region Vertex Operations (Operator Overloading)
        public static Vertex operator +(Vertex v1, Vector4D v2) => new Vertex(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z, v1.Colour, v1.Visible, v1.Diameter);
        public static Vertex operator -(Vertex v1, Vector4D v2) => new Vertex(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z, v1.Colour, v1.Visible, v1.Diameter);
        public static Vertex operator *(Vertex v, double scalar) => new Vertex(v.X * scalar, v.Y * scalar, v.Z * scalar, v.Colour, v.Visible, v.Diameter);
        public static Vertex operator /(Vertex v, double scalar) => new Vertex(v.X / scalar, v.Y / scalar, v.Z / scalar, v.Colour, v.Visible, v.Diameter);
        #endregion
    }

    public class Texture_Vertex : Vector3D
    {
        public Texture_Vertex(double x, double y) : base(x, y, 1)
        {

        }
    }

    public class Texture_Face
    {
        public int P1 { get; set; }
        public int P2 { get; set; }
        public int P3 { get; set; }
        public int Texture { get; set; }

        public Texture_Face(int p1, int p2, int p3, int texture)
        {
            P1 = p1;
            P2 = p2;
            P3 = p3;
            Texture = texture;
        }
    }

    public class Bitmap_Texture
    {
        public string File_Path { get; set; }
        public Bitmap Texture { get; private set; }

        public Bitmap_Texture(string file_path)
        {
            File_Path = file_path;
            Texture = new Bitmap(file_path);
        }
    }

    public class Edge
    {
        public int P1 { get; set; }
        public int P2 { get; set; }
        public Color Colour { get; set; }
        public bool Visible { get; set; }

        public Edge(int p1, int p2, Color colour, bool visibility = true)
        {
            P1 = p1;
            P2 = p2;
            Colour = colour;
            Visible = visibility;
        }
    }

    public class Clipped_Edge
    {
        public Vector4D P1 { get; set; }
        public Vector4D P2 { get; set; }
        public Color Colour { get; set; }
        public bool Visible { get; set; }

        public Clipped_Edge(Vector4D p1, Vector4D p2, Color colour, bool visibility = true)
        {
            P1 = p1;
            P2 = p2;
            Colour = colour;
            Visible = visibility;
        }
    }

    public class Face
    {
        public int P1 { get; set; }
        public int P2 { get; set; }
        public int P3 { get; set; }
        public Color Colour { get; set; }
        public bool Draw_Outline { get; set; }
        public bool Visible { get; set; }
        public string Texture_Path { get; set; }

        public Face(int p1, int p2, int p3, Color colour, string texture_path = "", bool draw_outline = false, bool visibility = true)
        {
            P1 = p1;
            P2 = p2;
            P3 = p3;
            Colour = colour;
            Draw_Outline = draw_outline;
            Visible = visibility;
            Texture_Path = texture_path;
        }
    }

    public class Clipped_Face
    {
        public Vector4D P1 { get; set; }
        public Vector4D P2 { get; set; }
        public Vector4D P3 { get; set; }
        public Color Colour { get; set; }
        public bool Draw_Outline { get; set; }
        public bool Visible { get; set; }
        public string Texture_Path { get; set; }

        public Clipped_Face(Vector4D p1, Vector4D p2, Vector4D p3, Color colour, string texture_path = "", bool draw_outline = false, bool visibility = true)
        {
            P1 = p1;
            P2 = p2;
            P3 = p3;
            Colour = colour;
            Draw_Outline = draw_outline;
            Visible = visibility;
            Texture_Path = texture_path;
        }
    }
}