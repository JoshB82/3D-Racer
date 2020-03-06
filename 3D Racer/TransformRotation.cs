using System;

namespace _3D_Racer
{
    public static partial class Transform
    {
        /// <summary>
        /// Creates a matrix for rotation about the x-axis
        /// </summary>
        /// <param name="angle">Angle to rotate</param>
        /// <returns>Rotation matrix</returns>
        public static Matrix4x4 Rotate_X(float angle)
        {
            float sin_angle = (float)Math.Sin(angle);
            float cos_angle = (float)Math.Cos(angle);
            Matrix4x4 rotation = Matrix4x4.IdentityMatrix();
            rotation.Data[1][1] = cos_angle;
            rotation.Data[1][2] = -sin_angle;
            rotation.Data[2][1] = sin_angle;
            rotation.Data[2][2] = cos_angle;
            return rotation;
        }

        /// <summary>
        /// Creates a matrix for rotation about the y-axis
        /// </summary>
        /// <param name="angle">Angle to rotate</param>
        /// <returns>Rotation matrix</returns>
        public static Matrix4x4 Rotate_Y(float angle)
        {
            float sin_angle = (float)Math.Sin(angle);
            float cos_angle = (float)Math.Cos(angle);
            Matrix4x4 rotation = Matrix4x4.IdentityMatrix();
            rotation.Data[0][0] = cos_angle;
            rotation.Data[0][2] = sin_angle;
            rotation.Data[2][0] = -sin_angle;
            rotation.Data[2][2] = cos_angle;
            return rotation;
        }

        /// <summary>
        /// Creates a matrix for rotation about the z-axis
        /// </summary>
        /// <param name="angle">Angle to rotate</param>
        /// <returns>Rotation matrix</returns>
        public static Matrix4x4 Rotate_Z(float angle)
        {
            float sin_angle = (float)Math.Sin(angle);
            float cos_angle = (float)Math.Cos(angle);
            Matrix4x4 rotation = Matrix4x4.IdentityMatrix();
            rotation.Data[0][0] = cos_angle;
            rotation.Data[0][1] = -sin_angle;
            rotation.Data[1][0] = sin_angle;
            rotation.Data[1][1] = cos_angle;
            return rotation;
        }

        /// <summary>
        /// Creates a rotation matrix that would rotate v1 onto v2.
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns>Rotation matrix</returns>
        public static Matrix4x4 Rotation_Between_Vectors(Vector3D v1, Vector3D v2)
        {
            // Create a normalised rotation axis
            Vector3D rotation_axis = v1.Cross_Product(v2).Normalise();
            // Find the angle to rotate and its sine and cosine
            float angle = v1.Angle(v2);
            float sin_angle = (float)Math.Sin(angle);
            float cos_angle = (float)Math.Cos(angle);
            // Construct the rotation matrix
            float[][] data = new float[4][]
                {
                    new float[] {
                        cos_angle + (float)Math.Pow(rotation_axis.X, 2) * (1 - cos_angle),
                        rotation_axis.X * rotation_axis.Y * (1 - cos_angle) - rotation_axis.Z * sin_angle,
                        rotation_axis.X * rotation_axis.Z * (1 - cos_angle) + rotation_axis.Y * sin_angle,
                        0
                    },
                    new float[] {
                        rotation_axis.Y * rotation_axis.X * (1 - cos_angle) + rotation_axis.Z * sin_angle,
                        cos_angle + (float)Math.Pow(rotation_axis.Y, 2) * (1 - cos_angle),
                        rotation_axis.Y * rotation_axis.Z * (1 - cos_angle) - rotation_axis.X * sin_angle,
                        0
                    },
                    new float[] {
                        rotation_axis.Z * rotation_axis.X * (1 - cos_angle) - rotation_axis.Y * sin_angle,
                        rotation_axis.Z * rotation_axis.Y * (1 - cos_angle) + rotation_axis.X * sin_angle,
                        cos_angle + (float)Math.Pow(rotation_axis.Z, 2) * (1 - cos_angle),
                        0
                    },
                    new float[] {
                        0,0,0,1
                    }
                };
            return new Matrix4x4(data);
        }
    }
}