namespace _3D_Racer
{
    public partial class Vertex : Vector
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public string Colour { get; set; }
        public bool Visible { get; set; }

        /// <summary>
        /// Create a new vertex.
        /// </summary>
        /// <param name="x">x - co-ordinate of the vertex.</param>
        /// <param name="y">y - co-ordinate of the vertex.</param>
        /// <param name="z">z - co-ordinate of the vertex.</param>
        /// <param name="colour">Six digit hexadecimal colour value.</param>
        public Vertex(double x, double y, double z, string colour = "000000", bool visibility = true) : base( x, y, z )
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
        public string Colour { get; set; }
        public bool Visible { get; set; }

        public Edge(int p1, int p2, string colour = "000000", bool visibility = true)
        {
            P1 = p1;
            P2 = p2;
            Colour = colour;
            Visible = visibility;
        }
    }

    public class Face
    {
        public int E1 { get; set; }
        public int E2 { get; set; }
        public int E3 { get; set; }
        public string Colour { get; set; }
        public bool Visible { get; set; }
        public int z_index { get; set; }

        public Face(int e1, int e2, int e3, string colour = "000000", bool visibility = true)
        {
            E1 = e1;
            E2 = e2;
            E3 = e3;
            Colour = colour;
            Visible = visibility;
        }
    }
}