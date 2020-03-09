using System.Diagnostics;

namespace _3D_Racer
{
    public abstract partial class Camera
    {
        public Vector4D Model_Origin { get; } = Vector4D.Zero;
        public Vector4D World_Origin { get; protected set; }
        public Vector4D Camera_Origin { get; protected set; }

        public Vector3D Model_Direction { get; } = Vector3D.Unit_Negative_Z;
        public Vector3D Model_Direction_Up { get; } = Vector3D.Unit_Y;
        public Vector3D World_Direction { get; protected set; }
        public Vector3D World_Direction_Up { get; protected set; }

        public Vector3D Scaling { get; protected set; }
        public Vector3D Translation { get; protected set; }

        public Matrix4x4 Model_to_world { get; protected set; }
        public Matrix4x4 World_to_camera { get; protected set; }
        public Matrix4x4 Camera_to_screen { get; protected set; }
        public Matrix4x4 World_to_screen { get; set; }

        public void Calculate_Model_to_World_Matrix() => Model_to_world = Transform.Translate(Translation) * Transform.Quaternion_to_Matrix(Transform.Quaternion_Rotation_Between_Vectors(Model_Direction, World_Direction));

        public void Calculate_World_to_Screen_Matrix()
        {
            World_to_camera = Transform.Quaternion_to_Matrix(Transform.Quaternion_Rotation_Between_Vectors(World_Direction, Model_Direction)) * Transform.Translate(-Translation);
            World_to_screen = Camera_to_screen * World_to_camera;
        }

        public void Calculate_Up_Vector()
        {
            World_Direction_Up = new Vector3D(Transform.Quaternion_to_Matrix(Transform.Quaternion_Rotation_Between_Vectors(Model_Direction, World_Direction)) * new Vector4D(Model_Direction_Up));
        }

        public Camera(Vector3D position, Vector3D direction)
        {
            Translation = position;
            Model_Direction = Vector3D.Unit_Negative_Z;
            World_Direction = direction;
        }
    }

    public class Orthogonal_Camera : Camera
    {
        private float width, height, z_near, z_far;
        public float Width
        {
            get { return width; }
            set { width = value; Camera_to_screen.Data[0][0] = 2 / width; }
        }
        public float Height
        {
            get { return height; }
            set { height = value; Camera_to_screen.Data[1][1] = 2 / height; }
        }
        public float Z_Near
        {
            get { return z_near; }
            set
            {
                z_near = value;
                Camera_to_screen.Data[2][2] = -2 / (z_far - z_near);
                Camera_to_screen.Data[2][3] = -(z_far + z_near) / (z_far - z_near);
            }
        }
        public float Z_Far
        {
            get { return z_far; }
            set
            {
                z_far = value;
                Camera_to_screen.Data[2][2] = -2 / (z_far - z_near);
                Camera_to_screen.Data[2][3] = -(z_far + z_near) / (z_far - z_near);
            }
        }

        public Orthogonal_Camera(Vector3D position, Vector3D direction, float width, float height, float z_near, float z_far) : base(position, direction)
        {
            Camera_to_screen = Matrix4x4.IdentityMatrix();
            Width = width;
            Height = height;
            Z_Near = z_near;
            Z_Far = z_far;

            Debug.WriteLine("Orthogonal camera created at (" + position.X + "," + position.Y + "," + position.Z + ")");
        }
        
        public Orthogonal_Camera(Vector3D position, Shape pointed_at, float width, float height, float z_near, float z_far) : this(position, new Vector3D(pointed_at.World_Origin) - position, width, height, z_near, z_far) {}
    }

    public class Perspective_Camera : Camera
    {
        private float width, height, z_near, z_far;
        public float Width
        {
            get { return width; }
            set { width = value; Camera_to_screen.Data[0][0] = 2 * z_near / width; }
        }
        public float Height
        {
            get { return height; }
            set { height = value; Camera_to_screen.Data[1][1] = 2 * z_near / height; }
        }
        public float Z_Near
        {
            get { return z_near; }
            set
            {
                z_near = value;
                Camera_to_screen.Data[2][2] = -(z_far + z_near) / (z_far - z_near);
                Camera_to_screen.Data[2][3] = -(2 * z_far * z_near) / (z_far - z_near);
            }
        }
        public float Z_Far
        {
            get { return z_far; }
            set
            {
                z_far = value;
                Camera_to_screen.Data[2][2] = -(z_far + z_near) / (z_far - z_near);
                Camera_to_screen.Data[2][3] = -(2 * z_far * z_near) / (z_far - z_near);
            }
        }

        public Perspective_Camera(Vector3D position, Vector3D direction, float width, float height, float z_near, float z_far) : base(position, direction)
        {
            /*
            Camera_to_screen = Matrix4x4.IdentityMatrix();
            Camera_to_screen.ChangeSingleValue(1, 1, (float)Math.Atan(fov_x / 2));
            Camera_to_screen.ChangeSingleValue(2, 2, (float)Math.Atan(fov_y / 2));
            Camera_to_screen.ChangeSingleValue(3, 3, -(z_far + z_near) / (z_far - z_near));
            Camera_to_screen.ChangeSingleValue(3, 4, -(2 * z_near * z_far) / (z_far - z_near));
            */
            Camera_to_screen = new Matrix4x4();
            Camera_to_screen.Data[3][2] = -1;
            
            Z_Near = z_near;
            Z_Far = z_far;
            Width = width;
            Height = height;

            Debug.WriteLine("Perspective camera created at (" + position.X + "," + position.Y + "," + position.Z + ")");
        }

        public Perspective_Camera(Vector3D position, Shape pointed_at, float width, float height, float z_near, float z_far) : this(position, new Vector3D(pointed_at.World_Origin) - position, width, height, z_near, z_far) {}
    }
}