﻿namespace _3D_Racer
{
    public abstract partial class Camera
    {
        public void Translate_X(float distance) => Translation += new Vector3D(distance, 0, 0);
        public void Translate_Y(float distance) => Translation += new Vector3D(0, distance, 0);
        public void Translate_Z(float distance) => Translation += new Vector3D(0, 0, distance);
        public void Translate(Vector3D distance) => Translation += distance;

        public void Rotate(Vector3D direction) => World_Direction = direction;
    }
}