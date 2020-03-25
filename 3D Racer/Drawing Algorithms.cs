using System;
using System.Drawing;

namespace _3D_Racer
{
    public sealed partial class Scene
    {
        public void Check_Against_Z_Buffer(int x, int y, float z, Color new_colour)
        {
            if (z_buffer[x][y] > z)
            {
                z_buffer[x][y] = z;
                colour_buffer[x][y] = new_colour;
            }
        }

        public void Line(int x1, int y1, float z1, int x2, int y2, float z2, Color colour)
        {
            float z_increase_x = (z1 - z2) / (x1 - x2);
            float z_increase_y = (z1 - z2) / (y1 - y2);

            int delta_x = x2 - x1;
            int delta_y = y2 - y1;

            // Check when 0
            int increment_x = Math.Sign(delta_x);
            int increment_y = Math.Sign(delta_y);

            delta_x = Math.Abs(delta_x);
            delta_y = Math.Abs(delta_y);

            int x = x1, y = y1, R = 0, D = Math.Max(delta_x, delta_y);
            float z_value = z1;

            if (delta_x > delta_y)
            {
                for (int i = 0; i < D; i++)
                {
                    Check_Against_Z_Buffer(x, y, z_value, colour);
                    x += increment_x;
                    z_value += z_increase_x * increment_x;
                    R += delta_y;
                    if (R >= delta_x)
                    {
                        R -= delta_x;
                        y += increment_y;
                        z_value += z_increase_y * increment_y;
                    }
                }
            }
            else
            {
                for (int i = 0; i < D; i++)
                {
                    Check_Against_Z_Buffer(x, y, z_value, colour);
                    y += increment_y;
                    z_value += z_increase_y * increment_y;
                    R += delta_x;
                    if (R >= delta_y)
                    {
                        R -= delta_y;
                        x += increment_x;
                        z_value += z_increase_x * increment_x;
                    }
                }
            }
        }
    }
}