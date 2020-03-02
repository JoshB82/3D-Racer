namespace _3D_Racer
{
    public abstract partial class Shape
    {
        /// <summary>
        /// Rotates the shape by a specified angle around the x-axis.
        /// </summary>
        /// <param name="angle">Angle in radians</param>
        public void Rotate_X(float angle)
        {
            Matrix rotation = Transform.Rotate_X(angle);
            Model_to_world = rotation * Model_to_world;
        }

        /// <summary>
        /// Rotates the shape by a specified angle around the y-axis.
        /// </summary>
        /// <param name="angle">Angle in radians</param>
        public void Rotate_Y(float angle)
        {
            Matrix rotation = Transform.Rotate_Y(angle);
            Model_to_world = rotation * Model_to_world;
        }

        /// <summary>
        /// Rotates the shape by a specified angle around the z-axis.
        /// </summary>
        /// <param name="angle">Angle in radians</param>
        public void Rotate_Z(float angle)
        {
            Matrix rotation = Transform.Rotate_Z(angle);
            Model_to_world = rotation * Model_to_world;
        }
    }
}