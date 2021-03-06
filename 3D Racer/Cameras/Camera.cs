﻿using System;
using System.Diagnostics;

namespace _3D_Racer
{
    public abstract partial class Camera
    {
        // Origins
        public Vector4D Model_Origin { get; } = Vector4D.Zero;
        public Vector4D World_Origin { get; set; }
        public Vector4D Camera_Origin { get; protected set; }

        #region Directions
        public Vector3D Model_Direction { get; } = Vector3D.Unit_Negative_Z;
        public Vector3D Model_Direction_Up { get; } = Vector3D.Unit_Y;
        public Vector3D Model_Direction_Right { get; } = Vector3D.Unit_X;

        public Vector3D World_Direction { get; private set; }
        public Vector3D World_Direction_Up { get; private set; }
        public Vector3D World_Direction_Right { get; private set; }
        #endregion

        #region Matrices
        public Matrix4x4 Model_to_world { get; protected set; }
        public Matrix4x4 World_to_camera { get; protected set; }
        public Matrix4x4 Camera_to_screen { get; protected set; }
        public Matrix4x4 World_to_screen { get; protected set; }
        #endregion

        public void Apply_World_Matrix() => World_Origin = Model_to_world * Model_Origin;

        public void Calculate_Model_to_World_Matrix()
        {
            Matrix4x4 direction_rotation = Transform.Quaternion_Rotation_Matrix(Model_Direction, World_Direction);
            Matrix4x4 direction_up_rotation = Transform.Quaternion_Rotation_Matrix(new Vector3D(direction_rotation * new Vector4D(Model_Direction_Up)), World_Direction_Up);
            Matrix4x4 translation = Transform.Translate(Translation);

            Model_to_world = translation * direction_up_rotation * direction_rotation;
        }
        
        public void Calculate_World_to_Screen_Matrix()
        {
            Matrix4x4 translation = Transform.Translate(-Translation);
            Matrix4x4 direction_up_rotation = Transform.Quaternion_Rotation_Matrix(World_Direction_Up, Model_Direction_Up);
            Matrix4x4 direction_rotation = Transform.Quaternion_Rotation_Matrix(new Vector3D(direction_up_rotation * new Vector4D(World_Direction)), Model_Direction);

            World_to_camera = direction_rotation * direction_up_rotation * translation;
            World_to_screen = Camera_to_screen * World_to_camera;
        }

        public Camera(Vector3D position, Vector3D direction, Vector3D direction_up)
        {
            Translation = position;
            World_Origin = new Vector4D(position);
            Set_Camera_Direction_1(direction, direction_up);
        }
    }

    public class Orthogonal_Camera : Camera
    {
        private double width, height, z_near, z_far;
        public double Width
        {
            get { return width; }
            set { width = value; Camera_to_screen.Data[0][0] = 2 / width; }
        }
        public double Height
        {
            get { return height; }
            set { height = value; Camera_to_screen.Data[1][1] = 2 / height; }
        }
        public double Z_Near
        {
            get { return z_near; }
            set
            {
                z_near = value;
                Camera_to_screen.Data[2][2] = -2 / (z_far - z_near);
                Camera_to_screen.Data[2][3] = -(z_far + z_near) / (z_far - z_near);
            }
        }
        public double Z_Far
        {
            get { return z_far; }
            set
            {
                z_far = value;
                Camera_to_screen.Data[2][2] = -2 / (z_far - z_near);
                Camera_to_screen.Data[2][3] = -(z_far + z_near) / (z_far - z_near);
            }
        }

        public Orthogonal_Camera(Vector3D position, Vector3D direction, Vector3D direction_up, double width, double height, double z_near, double z_far) : base(position, direction, direction_up)
        {
            Camera_to_screen = Matrix4x4.IdentityMatrix();
            Width = width;
            Height = height;
            Z_Near = z_near;
            Z_Far = z_far;

            Debug.WriteLine($"Orthogonal camera created at ({position.X}, {position.Y}, {position.Z})");
        }
        
        public Orthogonal_Camera(Vector3D position, Mesh pointed_at, Vector3D direction_up, double width, double height, double z_near, double z_far) : this(position, new Vector3D(pointed_at.World_Origin) - position, direction_up, width, height, z_near, z_far) {}

        public Orthogonal_Camera(Vector3D position, Vector3D direction, Vector3D direction_up, string ignore, double fov_x, double fov_y, double z_near, double z_far) : this(position, direction, direction_up, Math.Tan(fov_x / 2) * z_near * 2, Math.Tan(fov_y / 2) * z_near * 2, z_near, z_far) {}

        public Orthogonal_Camera(Vector3D position, Mesh pointed_at, Vector3D direction_up, string ignore, double fov_x, double fov_y, double z_near, double z_far) : this(position, new Vector3D(pointed_at.World_Origin), direction_up, Math.Tan(fov_x / 2) * z_near * 2, Math.Tan(fov_y / 2) * z_near * 2, z_near, z_far) {}

        public Clipping_Plane[] Calculate_Clipping_Planes()
        {
            double ratio = Z_Far / Z_Near;

            Vector3D near_point = new Vector3D(World_Origin) + World_Direction * Z_Near;
            Vector3D far_point = new Vector3D(World_Origin) + World_Direction * Z_Far;

            Vector3D near_bottom_left_point =  near_point + World_Direction_Right * -Width / 2 - World_Direction_Up * Height / 2;
            Vector3D near_bottom_right_point = near_point + World_Direction_Right * Width / 2 + World_Direction_Up * -Height / 2;
            Vector3D near_top_left_point = near_point + World_Direction_Right * -Width / 2 + World_Direction_Up * Height / 2;
            Vector3D near_top_right_point = near_point + World_Direction_Right * Width / 2 + World_Direction_Up * Height / 2;
            Vector3D far_top_left_point = far_point + World_Direction_Right * ratio * -Width / 2 + World_Direction_Up * ratio * Height / 2;
            Vector3D far_bottom_right_point = far_point + World_Direction_Right * ratio * Width / 2 + World_Direction_Up * ratio * -Height / 2;

            Vector3D bottom_normal = Vector3D.Normal_From_Plane(near_bottom_left_point, far_bottom_right_point, near_bottom_right_point);
            Vector3D left_normal = Vector3D.Normal_From_Plane(near_bottom_left_point, near_top_left_point, far_top_left_point);
            Vector3D top_normal = Vector3D.Normal_From_Plane(near_top_left_point, near_top_right_point, far_top_left_point);
            Vector3D right_normal = Vector3D.Normal_From_Plane(near_top_right_point, near_bottom_right_point, far_bottom_right_point);

            return new Clipping_Plane[]
            {
                    new Clipping_Plane(near_point, World_Direction), // Near z
                    new Clipping_Plane(far_point, -World_Direction), // Far z
                    new Clipping_Plane(near_bottom_left_point, bottom_normal), // Bottom
                    new Clipping_Plane(near_bottom_left_point, left_normal), // Left
                    new Clipping_Plane(near_top_right_point, top_normal), // Top
                    new Clipping_Plane(near_top_right_point, right_normal) // Right
            };
        }

        public Vector4D Apply_Camera_Matrices(Vector4D vertex) => World_to_screen * vertex;

        public Vector4D Divide_By_W(Vector4D vertex) => vertex / vertex.W;
    }

    public class Perspective_Camera : Camera
    {
        private double width, height, z_near, z_far;
        public double Width
        {
            get { return width; }
            set { width = value; Camera_to_screen.Data[0][0] = 2 * z_near / width; }
        }
        public double Height
        {
            get { return height; }
            set { height = value; Camera_to_screen.Data[1][1] = 2 * z_near / height; }
        }
        public double Z_Near
        {
            get { return z_near; }
            set
            {
                z_near = value;
                Camera_to_screen.Data[2][2] = -(z_far + z_near) / (z_far - z_near);
                Camera_to_screen.Data[2][3] = -(2 * z_far * z_near) / (z_far - z_near);
            }
        }
        public double Z_Far
        {
            get { return z_far; }
            set
            {
                z_far = value;
                Camera_to_screen.Data[2][2] = -(z_far + z_near) / (z_far - z_near);
                Camera_to_screen.Data[2][3] = -(2 * z_far * z_near) / (z_far - z_near);
            }
        }

        public Perspective_Camera(Vector3D position, Vector3D direction, Vector3D direction_up, double width, double height, double z_near, double z_far) : base(position, direction, direction_up)
        {
            Camera_to_screen = new Matrix4x4();
            Camera_to_screen.Data[3][2] = -1;
            
            Z_Near = z_near;
            Z_Far = z_far;
            Width = width;
            Height = height;

            Debug.WriteLine($"Perspective camera created at ({position.X}, {position.Y}, {position.Z})");
        }

        public Perspective_Camera(Vector3D position, Mesh pointed_at, Vector3D direction_up, double width, double height, double z_near, double z_far) : this(position, new Vector3D(pointed_at.World_Origin) - position, direction_up, width, height, z_near, z_far) {}

        public Perspective_Camera(Vector3D position, Vector3D direction, Vector3D direction_up, string ignore, double fov_x, double fov_y, double z_near, double z_far) : this(position, direction, direction_up, Math.Tan(fov_x / 2) * z_near * 2, Math.Tan(fov_y / 2) * z_near * 2, z_near, z_far) { }

        public Perspective_Camera(Vector3D position, Mesh pointed_at, Vector3D direction_up, string ignore, double fov_x, double fov_y, double z_near, double z_far) : this(position, new Vector3D(pointed_at.World_Origin), direction_up, Math.Tan(fov_x / 2) * z_near * 2, Math.Tan(fov_y / 2) * z_near * 2, z_near, z_far) { }

        public Clipping_Plane[] Calculate_Clipping_Planes()
        {
            double ratio = Z_Far / Z_Near;

            Vector3D near_point = new Vector3D(World_Origin) + World_Direction * Z_Near;
            Vector3D far_point = new Vector3D(World_Origin) + World_Direction * Z_Far;

            Vector3D near_bottom_left_point = near_point + World_Direction_Right * -Width / 2 - World_Direction_Up * Height / 2;
            Vector3D near_bottom_right_point = near_point + World_Direction_Right * Width / 2 + World_Direction_Up * -Height / 2;
            Vector3D near_top_left_point = near_point + World_Direction_Right * -Width / 2 + World_Direction_Up * Height / 2;
            Vector3D near_top_right_point = near_point + World_Direction_Right * Width / 2 + World_Direction_Up * Height / 2;
            Vector3D far_top_left_point = far_point + World_Direction_Right * ratio * -Width / 2 + World_Direction_Up * ratio * Height / 2;
            Vector3D far_bottom_right_point = far_point + World_Direction_Right * ratio * Width / 2 + World_Direction_Up * ratio * -Height / 2;

            Vector3D bottom_normal = Vector3D.Normal_From_Plane(near_bottom_left_point, far_bottom_right_point, near_bottom_right_point);
            Vector3D left_normal = Vector3D.Normal_From_Plane(near_bottom_left_point, near_top_left_point, far_top_left_point);
            Vector3D top_normal = Vector3D.Normal_From_Plane(near_top_left_point, near_top_right_point, far_top_left_point);
            Vector3D right_normal = Vector3D.Normal_From_Plane(near_top_right_point, near_bottom_right_point, far_bottom_right_point);

            return new Clipping_Plane[]
            {
                    new Clipping_Plane(near_point, World_Direction), // Near z
                    new Clipping_Plane(far_point, -World_Direction), // Far z
                    new Clipping_Plane(near_bottom_left_point, bottom_normal), // Bottom
                    new Clipping_Plane(near_bottom_left_point, left_normal), // Left
                    new Clipping_Plane(near_top_right_point, top_normal), // Top
                    new Clipping_Plane(near_top_right_point, right_normal) // Right
            };
        }

        public Vector4D Apply_Camera_Matrices(Vector4D vertex) => World_to_screen * vertex;

        public Vector4D Divide_By_W(Vector4D vertex) => vertex / vertex.W;
    }
}