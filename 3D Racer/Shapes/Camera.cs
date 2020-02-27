namespace _3D_Racer
{
    public abstract class Camera
    {
        /// <summary>
        /// Location of camera in world space
        /// </summary>
        public Vector Origin { get; set; }
        public Vector Direction { get; set; }

        public Matrix World_to_camera { get; protected set; }
        public Matrix Camera_to_screen { get; protected set; }
        public Matrix World_to_screen { get; protected set; }

        public Camera(float x, float y, float z, Vector direction)
        {
            Origin = new Vector(x, y, z);
            Direction = direction;
            Matrix rotation = Transform.Rotation_Between_Vectors(direction, Vector.Unit_Negative_Z);
            World_to_camera = Transform.Translate(-x, -y, -z) * rotation;
        }

        public Camera(float x, float y, float z, Shape shape) : this(x, y, z, shape.Origin - new Vector(x, y, z)) {}

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
        }
    }

    public class Perspective_Camera : Camera
    {
        public Perspective_Camera(float x, float y, float z, Vector direction) : base(x, y, z, direction)
        {
            Camera_to_screen = null;
        }

        /// <summary>
        /// Create a perspective camera pointed at the origin of a shape
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="pointed_at"></param>
        public Perspective_Camera(float x, float y, float z, Shape pointed_at) : base(x, y, z, pointed_at)
        {
            Camera_to_screen = null;
        }
    }
}