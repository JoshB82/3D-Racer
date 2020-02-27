namespace _3D_Racer
{
    public static partial class Transform
    {
        public static Matrix ScaleX(float scale_factor)
        {
            Matrix scaling = new Matrix(4);
            scaling.ChangeSingleValue(1, 1, scale_factor);
            return scaling;
        }

        public static Matrix ScaleY(float scale_factor)
        {
            Matrix scaling = new Matrix(4);
            scaling.ChangeSingleValue(2, 2, scale_factor);
            return scaling;
        }

        public static Matrix ScaleZ(float scale_factor)
        {
            Matrix scaling = new Matrix(4);
            scaling.ChangeSingleValue(3, 3, scale_factor);
            return scaling;
        }

        public static Matrix Scale(float scale_factor_x, float scale_factor_y, float scale_factor_z)
        {
            Matrix scaling = new Matrix(4);
            scaling.ChangeSingleValue(1, 1, scale_factor_x);
            scaling.ChangeSingleValue(2, 2, scale_factor_y);
            scaling.ChangeSingleValue(3, 3, scale_factor_z);
            return scaling;
        }
    }
}