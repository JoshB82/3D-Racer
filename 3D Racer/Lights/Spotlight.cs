using System;
using System.Diagnostics;
using System.Drawing;

namespace _3D_Racer
{
    public sealed class Spotlight : Light
    {
        private double angle, radius;
        public double Angle
        {
            get { return angle; }
            set
            {
                angle = value;
                radius = Math.Tan(value / 2) * Distance;
            }
        }
        public double Radius
        {
            get { return radius; }
            set
            {
                angle = Math.Atan2(value, Distance) * 2;
                radius = value;
            }
        }

        public double Distance { get; set; }

        public Spotlight(Vector3D position, Vector3D direction, Color? colour, double intensity, double angle, double distance)
        {
            Translation = position;
            World_Origin = new Vector4D(position);
            World_Direction = direction;
            Colour = colour ?? Color.White;
            Intensity = intensity;
            Angle = angle;
            Distance = distance;

            Debug.WriteLine($"Spotlight light created at ({position.X}, {position.Y}, {position.Z})");
        }

        public Spotlight(Vector3D position, Vector3D direction, Color? colour, string ignore, double intensity, double radius, double distance) : this(position, direction, colour, intensity, Math.Atan2(radius, distance) * 2, distance) {}
    }
}
