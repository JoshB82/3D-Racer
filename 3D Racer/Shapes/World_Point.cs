namespace _3D_Racer
{
    public sealed class World_Point : Shape
    {
        public World_Point(float x, float y, float z)
        {
            Origin = new Vector(x, y, z);
            Type = "Point";
            Visible = true;
            Selected = false;

            Vertices = null;
            Edges = null;
            Faces = null;

            Matrix translation = Transform.Translate(x, y, z);
            Model_to_world = translation;
            ApplyWorldMatrix();
            Camera_Origin = Origin;
            Camera_Vertices = null;
        }
    }
}
