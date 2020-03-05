namespace _3D_Racer
{
    public sealed class Cuboid : Shape
    {
        public Cuboid(float x, float y, float z, float length, float width, float height, string colour)
        {
            this = new Cube();
            Visible = true;
            Selected = false;
        }
    }
}
