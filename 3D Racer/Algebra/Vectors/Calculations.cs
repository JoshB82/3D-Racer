using System;

namespace _3D_Racer2
{
    public partial class Vector
    {
        /// <summary>
        /// Finds the dot product of two vectors.
        /// </summary>
        /// <param name="toDotProduct">The second vector in the dot product.</param>
        /// <returns>The dot product of two vectors.</returns>
        public float DotProduct(Vector toDotProduct)
        {
            if (Data.Length != toDotProduct.Data.Length)
                throw new Exception("The number of elements in each vector must be the same.");

            int length = Data.Length;
            float total = 0;

            for (int i = 0; i < length; i++)
                    total += Data[i] * toDotProduct.Data[i];

            return total;
        }
    }
}