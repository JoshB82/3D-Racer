using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace _3D_Racer
{
    public partial class MainForm : Form
    {
        private const float grav_acc = -9.81f;
        private const float camera_pan = 0.002f;
        private const float camera_tilt = 0.000001f;

        private const int max_frames_per_second = 60;
        private const int max_updates_per_second = 60;

        private Scene scene;

        private List<Perspective_Camera> cameras = new List<Perspective_Camera>();
        private Perspective_Camera current_camera;
        private int camera_selected = 0;

        private long update_time;

        public MainForm()
        {
            InitializeComponent();

            scene = new Scene(Canvas_Box.Width, Canvas_Box.Height);

            Cube cube_mesh = new Cube(new Vector3D(0, 0, 0), 50, true, null, null, Color.Green);
            Shape default_shape = new Shape(cube_mesh, true);
            scene.Add(default_shape);

            Cuboid cuboid_mesh = new Cuboid(new Vector3D(100, 0, 100), 30, 40, 90, true, null, null, Color.Purple);
            Shape cuboid = new Shape(cuboid_mesh);
            scene.Add(cuboid);

            Plane plane_mesh = new Plane(new Vector3D(0, 0, -30), Vector3D.Unit_X, Vector3D.Unit_Y, 100, 100, null, null, Color.Aqua);
            Shape plane = new Shape(plane_mesh);
            scene.Add(plane);

            // Create axes
            Line x_axis_mesh = new Line(new Vector3D(0, 0, 0), new Vector3D(250, 0, 0), null, Color.Red);
            Line y_axis_mesh = new Line(new Vector3D(0, 0, 0), new Vector3D(0, 250, 0), null, Color.Green);
            Line z_axis_mesh = new Line(new Vector3D(0, 0, 0), new Vector3D(0, 0, 250), null, Color.Blue);

            Shape x_axis = new Shape(x_axis_mesh);
            Shape y_axis = new Shape(y_axis_mesh);
            Shape z_axis = new Shape(z_axis_mesh);

            scene.Add(x_axis);
            scene.Add(y_axis);
            scene.Add(z_axis);

            // Create cameras
            cameras.Add(new Perspective_Camera(new Vector3D(0, 0, 500), cube_mesh, Vector3D.Unit_Y, Canvas_Box.Width / 10, Canvas_Box.Height / 10, 10, 1000));
            cameras.Add(new Perspective_Camera(new Vector3D(0,0, -500), cube_mesh, Vector3D.Unit_Y, Canvas_Box.Width / 10, Canvas_Box.Height / 10, 10, 1000));
            current_camera = cameras[0];

            Distant_Light light = new Distant_Light(new Vector3D(300, 400, 500), cube_mesh, Color.Red, 1);
            scene.Add_Light(light);

            /*
            // Show initial viewing frustum
            double ratio = 750 / 50, width2 = Canvas_Box.Width / 10, height2 = Canvas_Box.Height / 10;

            Vector3D near_bottom_left_point = new Vector3D(current_camera.World_Origin) + current_camera.World_Direction * 50 + current_camera.World_Direction_Right * -width2 / 2 - current_camera.World_Direction_Up * height2 / 2;
            Vector3D near_bottom_right_point = new Vector3D(current_camera.World_Origin) + current_camera.World_Direction * 50 + current_camera.World_Direction_Right * width2 / 2 + current_camera.World_Direction_Up * -height2 / 2;
            Vector3D near_top_left_point = new Vector3D(current_camera.World_Origin) + current_camera.World_Direction * 50 + current_camera.World_Direction_Right * -width2 / 2 + current_camera.World_Direction_Up * height2 / 2;
            Vector3D near_top_right_point = new Vector3D(current_camera.World_Origin) + current_camera.World_Direction * 50 + current_camera.World_Direction_Right * width2 / 2 + current_camera.World_Direction_Up * height2 / 2;
            Vector3D far_top_left_point = new Vector3D(current_camera.World_Origin) + current_camera.World_Direction * 750 + current_camera.World_Direction_Right * -width2 / 2 * ratio + current_camera.World_Direction_Up * height2 / 2 * ratio;
            Vector3D far_top_right_point = new Vector3D(current_camera.World_Origin) + current_camera.World_Direction * 750 + current_camera.World_Direction_Right * width2 / 2 * ratio + current_camera.World_Direction_Up * height2 / 2 * ratio;
            Vector3D far_bottom_left_point = new Vector3D(current_camera.World_Origin) + current_camera.World_Direction * 750 + current_camera.World_Direction_Right * -width2 / 2 * ratio + current_camera.World_Direction_Up * -height2 / 2 * ratio;
            Vector3D far_bottom_right_point = new Vector3D(current_camera.World_Origin) + current_camera.World_Direction * 750 + current_camera.World_Direction_Right * width2 / 2 * ratio + current_camera.World_Direction_Up * -height2 / 2 * ratio;

            Line near_top = new Line(near_top_left_point, near_top_right_point,null,Color.Red);
            Line near_bottom = new Line(near_bottom_left_point, near_bottom_right_point,null,Color.Orange);
            Line near_left = new Line(near_top_left_point, near_bottom_left_point,null,Color.Green);
            Line near_right = new Line(near_top_right_point, near_bottom_right_point,null,Color.Blue);

            Line side_top_left = new Line(near_top_left_point, far_top_left_point, null, Color.Purple);
            Line side_top_right = new Line(near_top_right_point, far_top_right_point, null, Color.Purple);
            Line side_bottom_left = new Line(near_bottom_left_point, far_bottom_left_point, null, Color.Purple);
            Line side_bottom_right = new Line(near_bottom_right_point, far_bottom_right_point, null, Color.Purple);

            Line far_top = new Line(far_top_left_point,far_top_right_point, null, Color.Gold);
            Line far_bottom = new Line(far_bottom_left_point, far_bottom_right_point, null, Color.Gold);
            Line far_left = new Line(far_top_left_point, far_bottom_left_point, null, Color.Gold);
            Line far_right = new Line(far_top_right_point, far_bottom_right_point, null, Color.Gold);

            Shape near_top_mesh = new Shape(near_top);
            Shape near_bottom_mesh = new Shape(near_bottom);
            Shape near_left_mesh = new Shape(near_left);
            Shape near_right_mesh = new Shape(near_right);

            Shape side_top_left_mesh = new Shape(side_top_left);
            Shape side_top_right_mesh = new Shape(side_top_right);
            Shape side_bottom_left_mesh = new Shape(side_bottom_left);
            Shape side_bottom_right_mesh = new Shape(side_bottom_right);

            Shape far_top_mesh = new Shape(far_top);
            Shape far_bottom_mesh = new Shape(far_bottom);
            Shape far_left_mesh = new Shape(far_left);
            Shape far_right_mesh = new Shape(far_right);

            scene.Add(near_top_mesh);
            scene.Add(near_bottom_mesh);
            scene.Add(near_left_mesh);
            scene.Add(near_right_mesh);

            scene.Add(side_top_left_mesh);
            scene.Add(side_top_right_mesh);
            scene.Add(side_bottom_left_mesh);
            scene.Add(side_bottom_right_mesh);

            scene.Add(far_top_mesh);
            scene.Add(far_bottom_mesh);
            scene.Add(far_left_mesh);
            scene.Add(far_right_mesh);

            //scene.Add(new Shape(new Plane(near_bottom_left_point, current_camera.World_Direction, width2, height2, null, null, Color.DarkGreen)));
            scene.Add(new Shape(new Plane(far_top_left_point, -current_camera.World_Direction, width2 * ratio, height2 * ratio, null, null, Color.Brown)));

            //World_Point origin = new World_Point(0, 0, 0);
            //Entity_List.Add(origin);
            */

            Thread graphics_thread = new Thread(Game_Loop);
            graphics_thread.Start();
            graphics_thread.IsBackground = true;
        }

        #region Graphics Thread
        private void Game_Loop()
        {
            bool game_running = true;

            long start_time = Get_UNIX_Time_Milliseconds();
            long timer = Get_UNIX_Time_Milliseconds();
            long frame_delta_time = 0, update_delta_time = 0, now_time;

            const long frame_optimal_time = 1000 / max_frames_per_second;
            const long update_optimal_time = 1000 / max_updates_per_second;

            int no_frames = 0, no_updates = 0;

            // Possible error with this code
            while (game_running)
            {
                now_time = Get_UNIX_Time_Milliseconds();
                frame_delta_time += (now_time - start_time);
                update_delta_time += (now_time - start_time);
                update_time = update_delta_time;
                start_time = now_time;

                if (frame_delta_time >= frame_optimal_time)
                {
                    // Update objects????vv
                    scene.Render(Canvas_Box, current_camera);
                    no_frames++;
                }

                if (update_delta_time >= update_optimal_time)
                {
                    // Render
                    // ApplyImpulse(update_delta_time);
                    no_updates++;
                }

                if (now_time - timer >= 1000)
                {
                    // ??
                    this.Invoke((MethodInvoker) delegate { this.Text = $"3D Racer - FPS: {no_frames}, UPS: {no_updates}"; });
                    no_frames = 0; no_updates = 0;
                    timer += 1000;
                }

                // User input
            }
        }

        private static long Get_UNIX_Time_Milliseconds() => (long)(DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds;
        #endregion

        private void Canvas_Panel_Paint(object sender, PaintEventArgs e) => e.Graphics.DrawImageUnscaled(scene.Canvas, Point.Empty);

        private void quitToolStripMenuItem_Click(object sender, EventArgs e) => Application.Exit();

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch(e.KeyCode)
            {
                case Keys.W:
                    // Pan forward
                    current_camera.Translate(current_camera.World_Direction * camera_pan * update_time);
                    break;
                case Keys.A:
                    // Pan left
                    current_camera.Translate(current_camera.World_Direction_Right * -camera_pan * update_time);
                    break;
                case Keys.D:
                    // Pan right
                    current_camera.Translate(current_camera.World_Direction_Right * camera_pan * update_time);
                    break;
                case Keys.S:
                    // Pan back
                    current_camera.Translate(current_camera.World_Direction * -camera_pan * update_time);
                    break;
                case Keys.Q:
                    // Pan up
                    current_camera.Translate(current_camera.World_Direction_Up * camera_pan * update_time);
                    break;
                case Keys.E:
                    // Pan down
                    current_camera.Translate(current_camera.World_Direction_Up * -camera_pan * update_time);
                    break;
                case Keys.I:
                    // Rotate up
                    Matrix4x4 transformation_up = Transform.Quaternion_Rotation_Axis_Matrix(current_camera.World_Direction_Right, camera_tilt * update_time);
                    current_camera.Set_Camera_Direction_3(new Vector3D(transformation_up * new Vector4D(current_camera.World_Direction_Right)), new Vector3D(transformation_up * new Vector4D(current_camera.World_Direction)));
                    break;
                case Keys.J:
                    // Rotate left
                    Matrix4x4 transformation_left = Transform.Quaternion_Rotation_Axis_Matrix(current_camera.World_Direction_Up, camera_tilt * update_time);
                    current_camera.Set_Camera_Direction_3(new Vector3D(transformation_left * new Vector4D(current_camera.World_Direction_Right)), new Vector3D(transformation_left * new Vector4D(current_camera.World_Direction)));
                    break;
                case Keys.L:
                    // Rotate right
                    Matrix4x4 transformation_right = Transform.Quaternion_Rotation_Axis_Matrix(current_camera.World_Direction_Up, -camera_tilt * update_time);
                    current_camera.Set_Camera_Direction_3(new Vector3D(transformation_right * new Vector4D(current_camera.World_Direction_Right)), new Vector3D(transformation_right * new Vector4D(current_camera.World_Direction)));
                    break;
                case Keys.K:
                    // Rotate down
                    Matrix4x4 transformation_down = Transform.Quaternion_Rotation_Axis_Matrix(current_camera.World_Direction_Right, -camera_tilt * update_time);
                    current_camera.Set_Camera_Direction_3(new Vector3D(transformation_down * new Vector4D(current_camera.World_Direction_Right)), new Vector3D(transformation_down * new Vector4D(current_camera.World_Direction)));
                    break;
                case Keys.U:
                    // Roll left
                    Matrix4x4 transformation_roll_left = Transform.Quaternion_Rotation_Axis_Matrix(current_camera.World_Direction, -camera_tilt * update_time);
                    current_camera.Set_Camera_Direction_2(new Vector3D(transformation_roll_left * new Vector4D(current_camera.World_Direction_Up)), new Vector3D(transformation_roll_left * new Vector4D(current_camera.World_Direction_Right)));
                    break;
                case Keys.O:
                    // Roll right
                    Matrix4x4 transformation_roll_right = Transform.Quaternion_Rotation_Axis_Matrix(current_camera.World_Direction, camera_tilt * update_time);
                    current_camera.Set_Camera_Direction_2(new Vector3D(transformation_roll_right * new Vector4D(current_camera.World_Direction_Up)), new Vector3D(transformation_roll_right * new Vector4D(current_camera.World_Direction_Right)));
                    break;
            }
        }

        private void switchCameraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            camera_selected++;
            if (camera_selected > cameras.Count - 1) camera_selected = 0;
            current_camera = cameras[camera_selected];
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (current_camera != null)
            {
                current_camera.Width = Canvas_Box.Width / 10;
                current_camera.Height = Canvas_Box.Height / 10;
                scene.Width = Canvas_Box.Width;
                scene.Height = Canvas_Box.Height;
            }
        }
    }
}