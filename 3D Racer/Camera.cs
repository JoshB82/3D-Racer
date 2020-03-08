using System.Diagnostics;

namespace _3D_Racer
{
    public abstract partial class Camera
    {
        public Vector4D Model_Origin { get; } = Vector4D.Zero;
        public Vector4D World_Origin { get; protected set; }
        public Vector4D Camera_Origin { get; protected set; }

        public Vector3D Model_Direction { get; } = Vector3D.Unit_Negative_Z;
        public Vector3D World_Direction { get; protected set; }
        public Vector3D Camera_Direction { get; protected set; }

        public Vector3D Scaling { get; protected set; }
        public Vector3D Translation { get; protected set; }

        public Matrix4x4 Model_to_world { get; protected set; }
        public Matrix4x4 World_to_camera { get; protected set; }
        public Matrix4x4 Camera_to_screen { get; protected set; } = Matrix4x4.IdentityMatrix();
        public Matrix4x4 World_to_screen { get; set; }

        public float Width { get; set; }
        public float Height { get; set; }
        public float Z_Near { get; set; }
        public float Z_Far { get; set; }

        public void Calculate_Model_to_World_Matrix() => Model_to_world = Transform.Translate(Translation) * Transform.Quaternion_to_Matrix(Transform.Quaternion_Rotation_Between_Vectors(Model_Direction, World_Direction));

        public void Calculate_World_to_Screen_Matrix()
        {
            World_to_camera = Transform.Translate(-Translation) * Transform.Quaternion_to_Matrix(Transform.Quaternion_Rotation_Between_Vectors(World_Direction, Model_Direction));
            World_to_screen = Camera_to_screen * World_to_camera;
        }

        public Camera(Vector3D position, Vector3D direction, float width, float height, float z_near, float z_far)
        {
            Translation = position;
            Model_Direction = Vector3D.Unit_Negative_Z;
            World_Direction = direction;

            Width = width;
            Height = height;
            Z_Near = z_near;
            Z_Far = z_far;
        }
    }

    public class Orthogonal_Camera : Camera
    {
        public Orthogonal_Camera(Vector3D position, Vector3D direction, float width, float height, float z_near, float z_far) : base(position, direction, width, height, z_near, z_far)
        {
            Camera_to_screen = Matrix4x4.IdentityMatrix();
            Camera_to_screen.Data[0][0] = 2 / Width;
            Camera_to_screen.Data[1][1] = 2 / Height;
            Camera_to_screen.Data[2][2] = -2 / (Z_Far - Z_Near);
            Camera_to_screen.Data[2][3] = -(Z_Far + Z_Near) / (Z_Far - Z_Near);
            Debug.WriteLine("Orthogonal camera created at (" + position.X + "," + position.Y + "," + position.Z + ")");
        }
        
        public Orthogonal_Camera(Vector3D position, Shape pointed_at, float width, float height, float z_near, float z_far) : this(position, new Vector3D(pointed_at.World_Origin) - position, width, height, z_near, z_far) {}
    }

    public class Perspective_Camera : Camera
    {
        public Perspective_Camera(Vector3D position, Vector3D direction, float width, float height, float z_near, float z_far) : base(position, direction, width, height, z_near, z_far)
        {
            /*
            Camera_to_screen = Matrix4x4.IdentityMatrix();
            Camera_to_screen.ChangeSingleValue(1, 1, (float)Math.Atan(fov_x / 2));
            Camera_to_screen.ChangeSingleValue(2, 2, (float)Math.Atan(fov_y / 2));
            Camera_to_screen.ChangeSingleValue(3, 3, -(z_far + z_near) / (z_far - z_near));
            Camera_to_screen.ChangeSingleValue(3, 4, -(2 * z_near * z_far) / (z_far - z_near));
            */
            Camera_to_screen = new Matrix4x4();
            Camera_to_screen.Data[0][0] = 2 * z_near / width;
            Camera_to_screen.Data[1][1] = 2 * z_near / height;
            Camera_to_screen.Data[2][2] = -(z_far + z_near) / (z_far - z_near);
            Camera_to_screen.Data[2][3] = -(2 * z_far * z_near) / (z_far - z_near);
            Camera_to_screen.Data[3][2] = -1;
            Debug.WriteLine("Perspective camera created at (" + position.X + "," + position.Y + "," + position.Z + ")");
        }

        /// <summary>
        /// Create a perspective camera pointing in a specific direction
        /// </summary>
        /// <param name="position">Position of the camera as a vector</param>
        /// <param name="direction">Direction the camera is facing as a vector</param>
        /// <param name="width">The width of the camera's view</param>
        /// <param name="height">The height of the camera's view</param>
        /// <param name="z_near">The closest distance from the camera from which an object can be seen</param>
        /// <param name="z_far">The furthest distance from the camera from which an object can be seen</param>
        public Perspective_Camera(Vector3D position, Shape pointed_at, float width, float height, float z_near, float z_far) : this(position, new Vector3D(pointed_at.World_Origin) - position, width, height, z_near, z_far) {}
    }
}