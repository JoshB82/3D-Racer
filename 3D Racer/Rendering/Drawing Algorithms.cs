using System;
using System.Drawing;

namespace _3D_Racer
{
    public sealed partial class Scene
    {
        private void Check_Against_Z_Buffer(int x, int y, double z, Color new_colour)
        {
            if (z <= z_buffer[x][y])
            {
                z_buffer[x][y] = z;
                colour_buffer[x][y] = new_colour;
            }
        }

        private void Line(int x1, int y1, double z1, int x2, int y2, double z2, Color colour)
        {
            if (x1 == x2)
            {
                Vertical_Line(x1,y1,z1,x2,y2,z2,colour);
            }
            else
            {
                if (y1 == y2)
                {
                    Horizontal_Line(x1, y1, z1, x2, y2, z2, colour);
                }
                else
                {
                    double z_increase_x = (z1 - z2) / (x1 - x2), z_increase_y = (z1 - z2) / (y1 - y2);

                    int delta_x = x2 - x1;
                    int delta_y = y2 - y1;

                    int increment_x = Math.Sign(delta_x);
                    int increment_y = Math.Sign(delta_y);

                    delta_x = Math.Abs(delta_x);
                    delta_y = Math.Abs(delta_y);

                    int x = x1, y = y1, R = 0, D = Math.Max(delta_x, delta_y);
                    double z_value = z1;

                    if (delta_x > delta_y)
                    {
                        for (int i = 0; i <= D; i++)
                        {
                            Check_Against_Z_Buffer(x, y, z_value, colour);
                            x += increment_x;
                            z_value += z_increase_x * increment_x;
                            R += 2 * delta_y;
                            if (R >= delta_x)
                            {
                                R -= 2 * delta_x;
                                y += increment_y;
                                z_value += z_increase_y * increment_y;
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i <= D; i++)
                        {
                            Check_Against_Z_Buffer(x, y, z_value, colour);
                            y += increment_y;
                            z_value += z_increase_y * increment_y;
                            R += 2 * delta_x;
                            if (R >= delta_y)
                            {
                                R -= 2 * delta_y;
                                x += increment_x;
                                z_value += z_increase_x * increment_x;
                            }
                        }
                    }
                }
            }
        }

        private void Horizontal_Line(int x1, int y1, double z1, int x2, int y2, double z2, Color colour)
        {
            double z_value, z_increase_x = (z1 - z2) / (x1 - x2);
            int min_x, max_x;
            if (x1 < x2)
            {
                min_x = x1;
                max_x = x2;
                z_value = z1;
            }
            else
            {
                min_x = x2;
                max_x = x1;
                z_value = z2;
            }

            for (int x = min_x; x <= max_x; x++)
            {
                Check_Against_Z_Buffer(x, y1, z_value, colour);
                z_value += z_increase_x;
            }
        }

        private void Vertical_Line(int x1, int y1, double z1, int x2, int y2, double z2, Color colour)
        {
            double z_value, z_increase_y = (z1 - z2) / (y1 - y2);
            int min_y, max_y;
            if (y1 < y2)
            {
                min_y = y1;
                max_y = y2;
                z_value = z1;
            }
            else
            {
                min_y = y2;
                max_y = y1;
                z_value = z2;
            }

            for (int y = min_y; y <= max_y; y++)
            {
                Check_Against_Z_Buffer(x1, y, z_value, colour);
                z_value += z_increase_y;
            }
        }

        private void Triangle(int x1, int y1, double z1, int x2, int y2, double z2, int x3, int y3, double z3, Color colour)
        {
            Vector3D normal = Vector3D.Normal_From_Plane(new Vector3D(x1, y1, z1), new Vector3D(x2, y2, z2), new Vector3D(x3, y3, z3));
            double z_increase_x = -normal.X / normal.Z, z_increase_y = -normal.Y / normal.Z;

            Sort_By_Y(ref x1, ref y1, ref z1, ref x2, ref y2, ref z2, ref x3, ref y3, ref z3);

            int x4;
            if (y1 == y2 && y2 == y3)
            {
                int start_x_value = Math.Min(Math.Min(x1, x2), x3), final_x_value = Math.Max(Math.Max(x1, x2), x3);
                double z_value = (start_x_value == x1) ? z1 : (start_x_value == x2) ? z2 : z3;
                for (int x = start_x_value; x <= final_x_value; x++)
                {
                    Check_Against_Z_Buffer(x, y1, z_value, colour);
                    z_value += z_increase_x;
                }
            }
            else
            {
                if (y2 == y3)
                {
                    double z_value = (x2 < x3) ? z2 : z3;
                    Flat_Bottom_Triangle(x2, y2, x3, y3, x1, y1, z_value, z_increase_x, z_increase_y, colour);
                }
                else
                {
                    if (y1 == y2)
                    {
                        double z_value = z3;
                        Flat_Top_Triangle(x1, y1, x2, y2, x3, y3, z_value, z_increase_x, z_increase_y, colour);
                    }
                    else
                    {
                        x4 = (int)Math.Round((double)((y2 - y1) * (x3 - x1) / (y3 - y1) + x1), MidpointRounding.AwayFromZero);
                        int y4 = y2;
                        double z_value = z3;

                        Flat_Top_Triangle(x2, y2, x4, y4, x3, y3, z_value, z_increase_x, z_increase_y, colour);
                        Flat_Bottom_Triangle(x2, y2, x4, y4, x1, y1, z_value, z_increase_x, z_increase_y, colour);
                    }
                }
            }
        }

        private void Flat_Bottom_Triangle(int x1, int y1, int x2, int y2, int x3, int y3, double z_value, double z_increase_x, double z_increase_y, Color colour)
        {
            // y1 must equal y2
            int[] start_x_values, final_x_values;
            if (x1 < x2)
            {
                Line_2(x1, y1, x3, y3, out start_x_values);
                Line_2(x2, y2, x3, y3, out final_x_values);
            }
            else
            {
                Line_2(x2, y2, x3, y3, out start_x_values);
                Line_2(x1, y1, x3, y3, out final_x_values);
            }

            int start_x_value, final_x_value, prev_x = 0;
            for (int y = y1; y <= y3; y++)
            {
                start_x_value = start_x_values[y - y1];
                final_x_value = final_x_values[y - y1];

                if (y != y1) z_value += (start_x_value - prev_x) * z_increase_x;

                for (int x = start_x_value; x <= final_x_value; x++)
                {
                    Check_Against_Z_Buffer(x, y, z_value, colour);
                    z_value += z_increase_x;
                }
                z_value -= z_increase_x * (final_x_value - start_x_value + 1);
                prev_x = start_x_value;
                z_value += z_increase_y;
            }
        }

        private void Flat_Top_Triangle(int x1, int y1, int x2, int y2, int x3, int y3, double z_value, double z_increase_x, double z_increase_y, Color colour)
        {
            // y1 must equal y2
            int[] start_x_values, final_x_values;
            if (x1 < x2)
            {
                Line_2(x3, y3, x1, y1, out start_x_values);
                Line_2(x3, y3, x2, y2, out final_x_values);
            }
            else
            {
                Line_2(x3, y3, x2, y2, out start_x_values);
                Line_2(x3, y3, x1, y1, out final_x_values);
            }

            int start_x_value, final_x_value, prev_x = 0;
            for (int y = y3; y <= y1; y++)
            {
                start_x_value = start_x_values[y - y3];
                final_x_value = final_x_values[y - y3];

                if (y != y3) z_value += (start_x_value - prev_x) * z_increase_x;

                for (int x = start_x_value; x <= final_x_value; x++)
                {
                    Check_Against_Z_Buffer(x, y, z_value, colour);
                    z_value += z_increase_x;
                }
                z_value -= z_increase_x * (final_x_value - start_x_value + 1);
                prev_x = start_x_value;
                if (y != y1) z_value += z_increase_y;
            }
        }

        private void Line_2(int x1, int y1, int x2, int y2, out int[] x_values)
        {
            int delta_x = x2 - x1;
            int delta_y = y2 - y1;

            int increment_x = Math.Sign(delta_x);
            int increment_y = Math.Sign(delta_y);

            delta_x = Math.Abs(delta_x);
            delta_y = Math.Abs(delta_y);

            int D = Math.Max(delta_x, delta_y);

            x_values = new int[delta_y + 1];
            x_values[0] = x1;

            int x = x1;
            int y = y1;
            int R = 0;
            int y_count = 0;

            if (delta_x > delta_y)
            {
                for (int i = 0; i <= D; i++)
                {
                    x += increment_x;
                    R += 2 * delta_y;
                    if (R >= delta_x)
                    {
                        R -= 2 * delta_x;
                        y += increment_y;
                        if (i != D)
                        {
                            y_count++;
                            x_values[y_count] = x;
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i <= D; i++)
                {
                    y += increment_y;
                    R += 2 * delta_x;
                    if (R >= delta_y)
                    {
                        R -= 2 * delta_y;
                        x += increment_x;
                    }
                    if (i != D)
                    {
                        y_count++;
                        x_values[y_count] = x;
                    }
                }
            }
        }

        /// <summary>
        /// Swaps the values of two variables.
        /// </summary>
        /// <param name="x1">First variable to be swapped.</param>
        /// <param name="x2">Second variable to be swapped.</param>
        private static void Swap<T>(ref T x1, ref T x2)
        {
            T temp = x1;
            x1 = x2;
            x2 = temp;
        }

        private void Sort_By_Y(ref int x1, ref int y1, ref double z1, ref int x2, ref int y2, ref double z2, ref int x3, ref int y3, ref double z3)
        {
            // y1 highest; y3 lowest
            if (y1 < y2)
            {
                Swap(ref x1, ref x2);
                Swap(ref y1, ref y2);
                Swap(ref z1, ref z2);
            }
            if (y1 < y3)
            {
                Swap(ref x1, ref x3);
                Swap(ref y1, ref y3);
                Swap(ref z1, ref z3);
            }
            if (y2 < y3)
            {
                Swap(ref x2, ref x3);
                Swap(ref y2, ref y3);
                Swap(ref z2, ref z3);
            }
        }
    }
}