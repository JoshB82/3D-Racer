namespace _3D_Racer
{
    public sealed class World_Point : Shape
    {
        public World_Point(float x, float y, float z)
        {
            Model_Origin = new Vector(0, 0, 0);
            
            Visible = true;
            Selected = false;

            Model_Vertices = null;
            Edges = null;
            Faces = null;

            Matrix translation = Transform.Translate(x, y, z);
            Model_to_world = translation;
            Apply_World_Matrices();
        }
    }
}