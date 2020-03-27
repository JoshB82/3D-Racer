namespace _3D_Racer
{
    public sealed class Clipping_Plane
    {
        public Vector3D Point { get; set; }
        public Vector3D Normal { get; set; }

        public Clipping_Plane(Vector3D point, Vector3D normal)
        {
            Point = point;
            Normal = normal;
        }
    }
}
