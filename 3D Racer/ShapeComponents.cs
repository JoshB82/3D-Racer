using System.Drawing;

namespace _3D_Racer
{
    public partial class Vertex : Vector4D
    {
        public Color Colour { get; set; }
        public float Radius { get; set; }
        public bool Visible { get; set; }

        /// <summary>
        /// Create a new vertex.
        /// </summary>
        /// <param name="x">x - co-ordinate of the vertex.</param>
        /// <param name="y">y - co-ordinate of the vertex.</param>
        /// <param name="z">z - co-ordinate of the vertex.</param>
        /// <param name="colour">Eight digit hexadecimal ARGB colour value.</param>
        public Vertex(float x, float y, float z, Color colour, bool visibility = true, float radius = 10) : base( x, y, z, 1 )
        {
            Colour = colour;
            Radius = radius;
            Visible = visibility;
        }

        #region Vertex Operations (Operator Overloading)
        public static Vertex operator +(Vertex v1, Vector4D v2) => new Vertex(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z, v1.Colour, v1.Visible, v1.Radius);
        public static Vertex operator -(Vertex v1, Vector4D v2) => new Vertex(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z, v1.Colour, v1.Visible, v1.Radius);
        public static Vertex operator *(Vertex v, float scalar) => new Vertex(v.X * scalar, v.Y * scalar, v.Z * scalar, v.Colour, v.Visible, v.Radius);
        public static Vertex operator /(Vertex v, float scalar) => new Vertex(v.X / scalar, v.Y / scalar, v.Z / scalar, v.Colour, v.Visible, v.Radius);
        #endregion
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

    public class Face
    {
        public int P1 { get; set; }
        public int P2 { get; set; }
        public int P3 { get; set; }
        public Color Colour { get; set; }
        public bool Visible { get; set; }
        public int Z_index { get; set; }

        public Face(int p1, int p2, int p3, Color colour, bool visibility = true)
        {
            P1 = p1;
            P2 = p2;
            P3 = p3;
            Colour = colour;
            Visible = visibility;
        }
    }
}