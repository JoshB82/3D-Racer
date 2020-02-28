namespace _3D_Racer
{
    public abstract partial class Shape
    {
        public void Translate_X(float distance)
        {
            Matrix translate = Transform.Translate_X(distance);
            Model_to_world = translate * Model_to_world;
        }

        public void Translate_Y(float distance)
        {
            Matrix translate = Transform.Translate_Y(distance);
            Model_to_world = translate * Model_to_world;
        }

        public void Translate_Z(float distance)
        {
            Matrix translate = Transform.Translate_Z(distance);
            Model_to_world = translate * Model_to_world;
        }

        public void Translate(float distance_x, float distance_y, float distance_z)
        {
            Matrix translate_x = Transform.Translate_X(distance_x);
            Matrix translate_y = Transform.Translate_X(distance_y);
            Matrix translate_z = Transform.Translate_X(distance_z);
            Model_to_world = translate_z * translate_y * translate_x * Model_to_world;
        }
    }
}
