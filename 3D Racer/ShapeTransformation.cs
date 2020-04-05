namespace _3D_Racer
{
    public abstract partial class Mesh
    {
        public void Scale_X(double scale_factor) => Scaling += new Vector3D(scale_factor, 0, 0);
        public void Scale_Y(double scale_factor) => Scaling += new Vector3D(0, scale_factor, 0);
        public void Scale_Z(double scale_factor) => Scaling += new Vector3D(0, 0, scale_factor);
        public void Scale(Vector3D scale_factor) => Scaling += scale_factor;
        public void Scale(double scale_factor) => Scaling += new Vector3D(scale_factor, scale_factor, scale_factor);

        public void Translate_X(double distance) => Translation += new Vector3D(distance, 0, 0);
        public void Translate_Y(double distance) => Translation += new Vector3D(0, distance, 0);
        public void Translate_Z(double distance) => Translation += new Vector3D(0, 0, distance);
        public void Translate(Vector3D distance) => Translation += distance;
    }
}