namespace _3D_Racer
{
    public partial class Vector
    {
        public double[] Data { get; set; }
        /// <summary>
        /// Number of elements in vector.
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// Creates a vector with a provided number of elements
        /// </summary>
        /// <param name="no_elements">Number of elements</param>
        public Vector(int no_elements)
        {
            Data = new double[no_elements];
            Size = no_elements;
        }

        /// <summary>
        /// Create a vector with two elements, typically used for 2D co-ordinates.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Vector(double x, double y)
        {
            Data = new double[2] { x, y };
            Size = 2;
        }

        /// <summary>
        /// Create a vector with three elements, typically used for 3D co-ordinates.
        /// </summary>
        /// <param name="x">First parameter - typically a x - co-ordinate</param>
        /// <param name="y">Second parameter - typically a y - co-ordinate</param>
        /// <param name="z">Third parameter - typically a z - co-ordinate</param>
        public Vector(double x, double y, double z)
        {
            Data = new double[3] { x, y, z };
            Size = 3;
        }

        /// <summary>
        /// Create a vector from an array.
        /// </summary>
        /// <param name="data">Array of data.</param>
        public Vector(double[] data)
        {
            Data = data;
            Size = data.Length;
        }
    }
}