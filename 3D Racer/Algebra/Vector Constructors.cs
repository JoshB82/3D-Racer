namespace _3D_Racer
{
    public partial class Vector
    {
        public float[] Data { get; set; }
        /// <summary>
        /// Number of elements in vector.
        /// </summary>
        public int Size { get; set; }

        public static Vector Unit_X { get; } = new Vector(1, 0, 0);
        public static Vector Unit_Y { get; } = new Vector(0, 1, 0);
        public static Vector Unit_Z { get; } = new Vector(0, 0, 1);
        public static Vector Unit_Negative_X { get; } = new Vector(-1, 0, 0);
        public static Vector Unit_Negative_Y { get; } = new Vector(0, -1, 0);
        public static Vector Unit_Negative_Z { get; } = new Vector(0, 0, -1);

        /// <summary>
        /// Creates a vector with a provided number of elements
        /// </summary>
        /// <param name="no_elements">Number of elements</param>
        public Vector(int no_elements)
        {
            Data = new float[no_elements];
            Size = no_elements;
        }

        /// <summary>
        /// Create a vector with two elements, typically used for 2D co-ordinates.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Vector(float x, float y)
        {
            Data = new float[2] { x, y };
            Size = 2;
        }

        /// <summary>
        /// Create a vector with three elements, typically used for 3D co-ordinates.
        /// </summary>
        /// <param name="x">First parameter - typically a x - co-ordinate</param>
        /// <param name="y">Second parameter - typically a y - co-ordinate</param>
        /// <param name="z">Third parameter - typically a z - co-ordinate</param>
        public Vector(float x, float y, float z)
        {
            Data = new float[3] { x, y, z };
            Size = 3;
        }

        /// <summary>
        /// Create a vector with four elements, typically used for 3D co-ordinates with a spare element for computational purposes.
        /// </summary>
        /// <param name="x">First parameter - typically a x - co-ordinate</param>
        /// <param name="y">Second parameter - typically a y - co-ordinate</param>
        /// <param name="z">Third parameter - typically a z - co-ordinate</param>
        /// <param name="w">Fourth parameter - typically for computational purposes</param>
        public Vector(float x, float y, float z, float w)
        {
            Data = new float[4] { x, y, z, w };
            Size = 4;
        }

        /// <summary>
        /// Create a vector from an array.
        /// </summary>
        /// <param name="data">Array of data.</param>
        public Vector(float[] data)
        {
            Data = data;
            Size = data.Length;
        }
    }
}