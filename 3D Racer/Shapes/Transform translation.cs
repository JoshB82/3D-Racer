namespace _3D_Racer
{
    public static partial class Transform
    {
        public static Matrix Translate_X(float distance)
        {
            Matrix translation = new Matrix(4);
            translation.ChangeSingleValue(1, 4, distance);
            return translation;
        }

        public static Matrix Translate_Y(float distance)
        {
            Matrix translation = new Matrix(4);
            translation.ChangeSingleValue(2, 4, distance);
            return translation;
        }

        public static Matrix Translate_Z(float distance)
        {
            Matrix translation = new Matrix(4);
            translation.ChangeSingleValue(3, 4, distance);
            return translation;
        }

        public static Matrix Translate(float distance_x, float distance_y, float distance_z)
        {
            Matrix translation = new Matrix(4);
            translation.ChangeSingleValue(1, 4, distance_x);
            translation.ChangeSingleValue(2, 4, distance_y);
            translation.ChangeSingleValue(3, 4, distance_z);
            return translation;
        }
    }
}