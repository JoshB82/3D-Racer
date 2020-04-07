namespace _3D_Racer
{
    public abstract partial class Camera
    {
        // Transformations
        public void Translate_X(double distance) => Translation += new Vector3D(distance, 0, 0);
        public void Translate_Y(double distance) => Translation += new Vector3D(0, distance, 0);
        public void Translate_Z(double distance) => Translation += new Vector3D(0, 0, distance);
        public void Translate(Vector3D distance) => Translation += distance;
    }
}