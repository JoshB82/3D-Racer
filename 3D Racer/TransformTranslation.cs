namespace _3D_Racer
{
    public static partial class Transform
    {
        /// <summary>
        /// Creates a matrix for translation in the x-direction
        /// </summary>
        /// <param name="distance">Distance to move by</param>
        /// <returns>A translation matrix</returns>
        public static Matrix4x4 Translate_X(float distance)
        {
            Matrix4x4 translation = Matrix4x4.IdentityMatrix();
            translation.Data[0][3] = distance;
            return translation;
        }

        /// <summary>
        /// Creates a matrix for translation in the y-direction
        /// </summary>
        /// <param name="distance">Distance to move by</param>
        /// <returns>A translation matrix</returns>
        public static Matrix4x4 Translate_Y(float distance)
        {
            Matrix4x4 translation = Matrix4x4.IdentityMatrix();
            translation.Data[1][3] = distance;
            return translation;
        }

        /// <summary>
        /// Creates a matrix for translation in the z-direction
        /// </summary>
        /// <param name="distance">Distance to move by</param>
        /// <returns>A translation matrix</returns>
        public static Matrix4x4 Translate_Z(float distance)
        {
            Matrix4x4 translation = Matrix4x4.IdentityMatrix();
            translation.Data[2][3] = distance;
            return translation;
        }

        /// <summary>
        /// Creates a matrix for translation in all directions
        /// </summary>
        /// <param name="distance_x">Distance to move by in x-direction</param>
        /// <param name="distance_y">Distance to move by in y-direction</param>
        /// <param name="distance_z">Distance to move by in z-direction</param>
        /// <returns>A translation matrix</returns>
        public static Matrix4x4 Translate(float distance_x, float distance_y, float distance_z)
        {
            Matrix4x4 translation = Matrix4x4.IdentityMatrix();
            translation.Data[0][3] = distance_x;
            translation.Data[1][3] = distance_y;
            translation.Data[2][3] = distance_z;
            return translation;
        }
    }
}