namespace _3D_Racer
{
    public sealed class World_Point : Shape
    {
        public World_Point(float x, float y, float z)
        {
            Model_Origin = new Vector4D(0, 0, 0, 1);
            
            Visible = true;
            Selected = false;

            Model_Vertices = null;

            Edges = null;
            Faces = null;

            Matrix4x4 translation = Transform.Translate(new Vector3D(x, y, z));
            Model_to_world = translation;
            Apply_World_Matrices();
        }
    }
}