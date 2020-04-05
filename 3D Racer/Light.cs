namespace _3D_Racer
{
    public abstract class Light
    {
        public Vector3D Model_Origin { get; }
        public Vector4D World_Origin { get; set; }
        public Vector4D Camera_Origin { get; protected set; } //?

        public Vector3D Model_Direction { get; } = Vector3D.Unit_X;
        public Vector3D Model_Direction_Up { get; } = Vector3D.Unit_Y;
        public Vector3D Model_Direction_Right { get; } = Vector3D.Unit_Z;

        public Vector3D World_Direction { get; private set; }
        public Vector3D World_Direction_Up { get; private set; }
        public Vector3D World_Direction_Right { get; private set; }
    }
}
