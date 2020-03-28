using System;
using System.Drawing;

namespace _3D_Racer
{
    public partial class Vertex : Vector4D
    {
        public Color Colour { get; set; }
        public int Diameter { get; set; }
        public bool Visible { get; set; }

        public Vertex(Vector3D position, Color? colour = null, bool visibility = true, int diameter = 10) : this(position, 1, colour, visibility, diameter) { }
        /// <summary>
        /// Create a new vertex.
        /// </summary>
        /// <param name="x">x - co-ordinate of the vertex.</param>
        /// <param name="y">y - co-ordinate of the vertex.</param>
        /// <param name="z">z - co-ordinate of the vertex.</param>
        /// <param name="colour">Eight digit hexadecimal ARGB colour value.</param>
        public Vertex(float x, float y, float z, Color? colour = null, bool visibility = true, int diameter = 10) : this(x, y, z, 1, colour, visibility, diameter) {}

        public Vertex(Vector3D position, float w, Color? colour = null, bool visibility = true, int diameter = 10) : this(position.X, position.Y, position.Z, w, colour, visibility, diameter) { }
        public Vertex(float x, float y, float z, float w, Color? colour = null, bool visibility = true, int diameter = 10) : base(x, y, z, w)
        {
            Colour = colour ?? Color.Black;
            Diameter = diameter;
            Visible = visibility;
        }

        public void Round()
        {
            X = (int)Math.Round(X, MidpointRounding.AwayFromZero);
            Y = (int)Math.Round(Y, MidpointRounding.AwayFromZero);
        }

        #region Vertex Operations (Operator Overloading)
        public static Vertex operator +(Vertex v1, Vector4D v2) => new Vertex(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z, v1.Colour, v1.Visible, v1.Diameter);
        public static Vertex operator -(Vertex v1, Vector4D v2) => new Vertex(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z, v1.Colour, v1.Visible, v1.Diameter);
        public static Vertex operator *(Vertex v, float scalar) => new Vertex(v.X * scalar, v.Y * scalar, v.Z * scalar, v.Colour, v.Visible, v.Diameter);
        public static Vertex operator /(Vertex v, float scalar) => new Vertex(v.X / scalar, v.Y / scalar, v.Z / scalar, v.Colour, v.Visible, v.Diameter);
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

    public class Edge_2
    {
        public Vector3D P1 { get; set; }
        public Vector3D P2 { get; set; }
        public Color Colour { get; set; }
        public bool Visible { get; set; }

        public Edge_2(Vector3D p1, Vector3D p2, Color colour, bool visibility = true)
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

        public Face(int p1, int p2, int p3, Color colour, bool visibility = true)
        {
            P1 = p1;
            P2 = p2;
            P3 = p3;
            Colour = colour;
            Visible = visibility;
        }
    }

    public class Face_2
    {
        public Vector3D P1 { get; set; }
        public Vector3D P2 { get; set; }
        public Vector3D P3 { get; set; }
        public Color Colour { get; set; }
        public bool Visible { get; set; }
        
        public Face_2(Vector3D p1, Vector3D p2, Vector3D p3, Color colour, bool visibility = true)
        {
            P1 = p1;
            P2 = p2;
            P3 = p3;
            Colour = colour;
            Visible = visibility;
        }
    }
}