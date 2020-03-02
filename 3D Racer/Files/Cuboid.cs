namespace _3D_Racer
{
    public sealed class Cuboid : Shape
    {
        public Cuboid(float x, float y, float z, float length, float width, float height, string colour)
        {
            Origin = new Vertex(x, y, z, null, false);
            Visible = true;
            Selected = false;
        }
    }
}
