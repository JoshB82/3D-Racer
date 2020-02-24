namespace _3D_Racer
{
    private abstract partial class Shape
    {
        public void TranslateX(float distance)
        {
            Matrix translation = new Matrix(4);
            translation.ChangeSingleValue(1, 4, distance);
            model_to_world = translation * model_to_world;
        }

        public void TranslateY(float distance)
        {
            Matrix translation = new Matrix(4);
            translation.ChangeSingleValue(2, 4, distance);
            model_to_world = translation * model_to_world;
        }

        public void TranslateZ(float distance)
        {
            Matrix translation = new Matrix(4);
            translation.ChangeSingleValue(3, 4, distance);
            model_to_world = translation * model_to_world;
        }

        public void Translate(float distance_x, float distance_y, float distance_z)
        {
            Matrix translation = new Matrix(4);
            translation.ChangeSingleValue(1, 4, distance_x);
            translation.ChangeSingleValue(2, 4, distance_y);
            translation.ChangeSingleValue(3, 4, distance_z);
            model_to_world = translation * model_to_world;
        }
    }
}
