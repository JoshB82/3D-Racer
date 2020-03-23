namespace _3D_Racer
{
    public abstract partial class Mesh
    {
        public void Translate_X(float distance) => Translation += new Vector3D(distance, 0, 0);
        public void Translate_Y(float distance) => Translation += new Vector3D(0, distance, 0);
        public void Translate_Z(float distance) => Translation += new Vector3D(0, 0, distance);
        public void Translate(Vector3D distance) => Translation += distance;

        public void Scale_X(float scale_factor) => Scaling += new Vector3D(scale_factor, 0, 0);
        public void Scale_Y(float scale_factor) => Scaling += new Vector3D(0, scale_factor, 0);
        public void Scale_Z(float scale_factor) => Scaling += new Vector3D(0, 0, scale_factor);
        public void Scale(float scale_factor) => Scaling += new Vector3D(scale_factor, scale_factor, scale_factor);

        public void Rotate(Vector3D direction) => World_Direction = direction;
    }
}