using System;
using System.Diagnostics;

namespace _3D_Racer
{
    public abstract partial class Camera
    {
        /// <summary>
        /// Location of camera in world space
        /// </summary>
        public Vector Model_Origin { get; } = Vector.Zero;
        public Vector World_Origin { get; protected set; }
        public Vector Camera_Origin { get; protected set; }

        public Vector Model_Direction { get; } = Vector.Unit_Negative_Z;
        public Vector World_Direction { get; protected set; }

        public Matrix Model_to_world { get; protected set; }
        public Matrix World_to_camera { get; protected set; }
        public Matrix Camera_to_screen { get; protected set; } = new Matrix(4);
        public Matrix World_to_screen { get; protected set; }

        public Camera(float x, float y, float z, Vector direction)
        {
            Model_to_world = Transform.Translate(x, y, z) * Transform.Rotation_Between_Vectors(Model_Direction, direction);
            World_to_camera = Transform.Rotation_Between_Vectors(direction, Vector.Unit_Negative_Z) * Transform.Translate(-x, -y, -z);
            World_Direction = direction;
        }

        public Camera(float x, float y, float z, Shape shape) : this(x, y, z, shape.World_Origin - new Vector(x, y, z, 1)) {}

        public void Recalculate_Matrix()
        {
            World_to_screen = Camera_to_screen * World_to_camera;
        }
    }

    public class Orthogonal_Camera : Camera
    {
        public Orthogonal_Camera(float x, float y, float z, Vector direction,
            float width, float height, float z_near, float z_far) : base(x, y, z, direction)
        {
            Camera_to_screen = new Matrix(4);
            Camera_to_screen.ChangeSingleValue(1, 1, 1 / width);
            Camera_to_screen.ChangeSingleValue(2, 2, 1 / height);
            Camera_to_screen.ChangeSingleValue(3, 3, -2 / (z_far - z_near));
            Camera_to_screen.ChangeSingleValue(3, 4, -(z_far + z_near) / (z_far - z_near));
            Debug.WriteLine("Orthogonal camera created at (" + x + "," + y + "," + z + ")");
        }

        /// <summary>
        /// Create an orthogonal camera pointed at the origin of a shape
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="pointed_at"></param>
        public Orthogonal_Camera(float x, float y, float z, Shape pointed_at,
            float width, float height, float z_near, float z_far) : base(x, y, z, pointed_at)
        {
            Camera_to_screen = new Matrix(4);
            Camera_to_screen.ChangeSingleValue(1, 1, 1 / width);
            Camera_to_screen.ChangeSingleValue(2, 2, 1 / height);
            Camera_to_screen.ChangeSingleValue(3, 3, -2 / (z_far - z_near));
            Camera_to_screen.ChangeSingleValue(3, 4, -(z_far + z_near) / (z_far - z_near));
            Debug.WriteLine("Orthogonal camera created at (" + x + "," + y + "," + z + ")");
        }
    }

    public class Perspective_Camera : Camera
    {
        public Perspective_Camera(float x, float y, float z, Vector direction,
            float fov_x, float fov_y, float z_near, float z_far) : base(x, y, z, direction)
        {
            /*
            Camera_to_screen = new Matrix(4);
            Camera_to_screen.ChangeSingleValue(1, 1, (float)Math.Atan(fov_x / 2));
            Camera_to_screen.ChangeSingleValue(2, 2, (float)Math.Atan(fov_y / 2));
            Camera_to_screen.ChangeSingleValue(3, 3, -(z_far + z_near) / (z_far - z_near));
            Camera_to_screen.ChangeSingleValue(3, 4, -(2 * z_near * z_far) / (z_far - z_near));
            */

            Camera_to_screen = new Matrix(4);
            Camera_to_screen.ChangeSingleValue(1,1,);
            Camera_to_screen.ChangeSingleValue(3, 3, -(z_far + z_near) / (z_far - z_near));
            Camera_to_screen.ChangeSingleValue(3, 4, -(2 * z_near * z_far) / (z_far - z_near));
            Debug.WriteLine("Perspective camera created at (" + x + "," + y + "," + z + ")");
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
        public Perspective_Camera(Vector position, Vector direction, float width, float height, float z_near, float z_far) : base(position.Data[0], position.Data[1], position.Data[2], direction)
        {
            Camera_to_screen.ChangeSingleValue(1, 1, 2 * z_near / width);
            Camera_to_screen.ChangeSingleValue(2, 2, 2 * z_near / height);

        }

        /// <summary>
        /// Create a perspective camera pointed at the origin of a shape
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="pointed_at"></param>
        public Perspective_Camera(float x, float y, float z, Shape pointed_at,
            float fov_x, float fov_y, float z_near, float z_far) : base(x, y, z, pointed_at)
        {
            Camera_to_screen = new Matrix(4);
            Camera_to_screen.ChangeSingleValue(1, 1, (float)Math.Atan(fov_x / 2));
            Camera_to_screen.ChangeSingleValue(2, 2, (float)Math.Atan(fov_y / 2));
            Camera_to_screen.ChangeSingleValue(3, 3, -(z_far + z_near) / (z_far - z_near));
            Camera_to_screen.ChangeSingleValue(3, 4, -(2 * z_near * z_far) / (z_far - z_near));
            Debug.WriteLine("Perspective camera created at (" + x + "," + y + "," + z + ")");
        }
    }
}