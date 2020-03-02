using System;

namespace _3D_Racer
{
    public static partial class Transform
    {
        /// <summary>
        /// Returns a matrix for rotation about the x-axis
        /// </summary>
        /// <param name="angle">Angle to rotate</param>
        public static Matrix Rotate_X(float angle)
        {
            float sin_angle = (float)Math.Sin(angle);
            float cos_angle = (float)Math.Cos(angle);
            Matrix rotation = new Matrix(4);
            rotation.ChangeSingleValue(2, 2, cos_angle);
            rotation.ChangeSingleValue(2, 3, -sin_angle);
            rotation.ChangeSingleValue(3, 2, sin_angle);
            rotation.ChangeSingleValue(3, 3, cos_angle);
            return rotation;
        }

        /// <summary>
        /// Returns a matrix for rotation about the y-axis
        /// </summary>
        /// <param name="angle">Angle to rotate</param>
        public static Matrix Rotate_Y(float angle)
        {
            float sin_angle = (float)Math.Sin(angle);
            float cos_angle = (float)Math.Cos(angle);
            Matrix rotation = new Matrix(4);
            rotation.ChangeSingleValue(1, 1, cos_angle);
            rotation.ChangeSingleValue(1, 3, sin_angle);
            rotation.ChangeSingleValue(3, 1, -sin_angle);
            rotation.ChangeSingleValue(3, 3, cos_angle);
            return rotation;
        }

        /// <summary>
        /// Returns a matrix for rotation about the z-axis
        /// </summary>
        /// <param name="angle">Angle to rotate</param>
        public static Matrix Rotate_Z(float angle)
        {
            float sin_angle = (float)Math.Sin(angle);
            float cos_angle = (float)Math.Cos(angle);
            Matrix rotation = new Matrix(4);
            rotation.ChangeSingleValue(1, 1, cos_angle);
            rotation.ChangeSingleValue(1, 2, -sin_angle);
            rotation.ChangeSingleValue(2, 1, sin_angle);
            rotation.ChangeSingleValue(2, 2, cos_angle);
            return rotation;
        }

        /// <summary>
        /// Finds the rotation matrix that would rotate v1 onto v2.
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static Matrix Rotation_Between_Vectors(Vector v1, Vector v2)
        {
            v1 = new Vector(v1.Data[0], v1.Data[1], v1.Data[2]);
            v2 = new Vector(v2.Data[0], v2.Data[1], v2.Data[2]);
            Vector rotation_axis = Vector.Cross_Product(v1, v2);
            rotation_axis.Normalise();
            float angle = Vector.Angle_Between_Vectors(v1, v2);
            float sin_angle = (float)Math.Sin(angle);
            float cos_angle = (float)Math.Cos(angle);
            float[,] data = new float[4, 4]
                {
                    {
                        cos_angle + (float)Math.Pow(rotation_axis.Data[0], 2) * (1 - cos_angle),
                        rotation_axis.Data[0] * rotation_axis.Data[1] * (1 - cos_angle) - rotation_axis.Data[2] * sin_angle,
                        rotation_axis.Data[0] * rotation_axis.Data[2] * (1 - cos_angle) + rotation_axis.Data[1] * sin_angle,
                        0
                    },
                    {
                        rotation_axis.Data[1] * rotation_axis.Data[0] * (1 - cos_angle) + rotation_axis.Data[2] * sin_angle,
                        cos_angle + (float)Math.Pow(rotation_axis.Data[1], 2) * (1 - cos_angle),
                        rotation_axis.Data[1] * rotation_axis.Data[2] * (1 - cos_angle) - rotation_axis.Data[0] * sin_angle,
                        0
                    },
                    {
                        rotation_axis.Data[2] * rotation_axis.Data[0] * (1 - cos_angle) - rotation_axis.Data[1] * sin_angle,
                        rotation_axis.Data[2] * rotation_axis.Data[1] * (1 - cos_angle) + rotation_axis.Data[0] * sin_angle,
                        cos_angle + (float)Math.Pow(rotation_axis.Data[2], 2) * (1 - cos_angle),
                        0
                    },
                    {
                        0,0,0,1
                    }
                };
            return new Matrix(data);
        }
    }
}