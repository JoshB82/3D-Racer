namespace _3D_Racer
{
    public partial class Vertex : Vector
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float W { get; set; } = 1;

        public int Colour { get; set; }
        public float Radius { get; set; }
        public bool Visible { get; set; }

        /// <summary>
        /// Create a new vertex.
        /// </summary>
        /// <param name="x">x - co-ordinate of the vertex.</param>
        /// <param name="y">y - co-ordinate of the vertex.</param>
        /// <param name="z">z - co-ordinate of the vertex.</param>
        /// <param name="colour">Eight digit hexadecimal colour value.</param>
        public Vertex(float x, float y, float z, int colour = 0x000000FF, bool visibility = true) : base( x, y, z, 1 )
        {
            X = x;
            Y = y;
            Z = z;
            Colour = colour;
            Visible = visibility;
        }
    }

    public class Edge
    {
        public int P1 { get; set; }
        public int P2 { get; set; }
        public int Colour { get; set; }
        public bool Visible { get; set; }

        public Edge(int p1, int p2, int colour = 0x00000000, bool visibility = true)
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
        public int Colour { get; set; }
        public bool Visible { get; set; }
        public int z_index { get; set; }

        public Face(int p1, int p2, int p3, int colour = 0x0000FF00, bool visibility = true)
        {
            P1 = p1;
            P2 = p2;
            P3 = p3;
            Colour = colour;
            Visible = visibility;
        }
    }
}