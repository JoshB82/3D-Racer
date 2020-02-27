namespace _3D_Racer
{
    public abstract partial class Shape
    {
        /// <summary>
        /// Scales the shape by a specified scale factor in the x-direction.
        /// </summary>
        /// <param name="scale_factor">Scale factor</param>
        public void Scale_X(float scale_factor)
        {
            Matrix scaling = Transform.Scale_X(scale_factor);
            Model_to_world = scaling * Model_to_world;
        }

        /// <summary>
        /// Scales the shape by a specified scale factor in the y-direction.
        /// </summary>
        /// <param name="scale_factor">Scale factor</param>
        public void Scale_Y(float scale_factor)
        {
            Matrix scaling = Transform.Scale_Y(scale_factor);
            Model_to_world = scaling * Model_to_world;
        }

        /// <summary>
        /// Scales the shape by a specified scale factor in the z-direction.
        /// </summary>
        /// <param name="scale_factor">Scale factor</param>
        public void Scale_Z(float scale_factor)
        {
            Matrix scaling = Transform.Scale_Z(scale_factor);
            Model_to_world = scaling * Model_to_world;
        }

        /// <summary>
        /// Scales the shape by a specified scale factor in all directions.
        /// </summary>
        /// <param name="scale_factor_x">Scale factor for x-direction</param>
        /// <param name="scale_factor_y">Scale factor for y-direction</param>
        /// <param name="scale_factor_z">Scale factor for z-direction</param>
        public void Scale_Z(float scale_factor_x, float scale_factor_y, float scale_factor_z)
        {
            Matrix scaling_x = Transform.Scale_X(scale_factor_x);
            Matrix scaling_y = Transform.Scale_Y(scale_factor_y);
            Matrix scaling_z = Transform.Scale_Z(scale_factor_z);
            Model_to_world = scaling_z * scaling_y * scaling_x * Model_to_world;
        }
    }
}
