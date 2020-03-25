using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace _3D_Racer
{
    public partial class MainForm : Form
    {
        private int prev_x, prev_y;
        private bool mouse_down;
        private int selected_shape;

        private const float grav_acc = -9.81f;
        private const float camera_pan = 0.02f;
        private const float camera_tilt = 0.00001f;

        private const int max_frames_per_second = 60;
        private const int max_updates_per_second = 60;

        private Scene scene;
        private Camera Current_camera;

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

            Plane plane_mesh = new Plane(new Vector3D(0, 0, -30), Vector3D.Unit_Y, 100, 100, null, null, Color.Aqua);
            Shape plane = new Shape(plane_mesh);
            scene.Add(plane);

            // Create axes
            Line x_axis_mesh = new Line(new Vector3D(0, 0, 0), new Vector3D(250, 0, 0), null, Color.Red);
            Line y_axis_mesh = new Line(new Vector3D(0, 0, 0), new Vector3D(0, 250, 0), null, Color.Green);
            Line z_axis_mesh = new Line(new Vector3D(0, 0, 0), new Vector3D(0, 0, 250), null, Color.Blue);
            // EMPTY MESH?
            Shape x_axis = new Shape(x_axis_mesh);
            Shape y_axis = new Shape(y_axis_mesh);
            Shape z_axis = new Shape(z_axis_mesh);
            scene.Add(x_axis);
            scene.Add(y_axis);
            scene.Add(z_axis);

            //World_Point origin = new World_Point(0, 0, 0);
            //Entity_List.Add(origin);
            Current_camera = new Perspective_Camera(new Vector3D(0, 0, 500), cube_mesh, 100, 100, 50, 750);

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
                    scene.Render(Canvas_Box, Current_camera);
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
                    Debug.WriteLine("FPS: " + no_frames + ", UPS: " + no_updates);
                    no_frames = 0;
                    no_updates = 0;
                    timer += 1000;
                }

                // User input
            }
        }

        private static long Get_UNIX_Time_Milliseconds() => (long)(DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds;

        #endregion

        private void Canvas_Panel_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImageUnscaled(scene.Canvas, Point.Empty);
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Canvas_Panel_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                scene.Add(new Shape(new Cube(new Vector3D(e.X, e.Y, 100), 100)));
            }
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch(e.KeyCode)
            {
                case Keys.W:
                    // Pan forward
                    Current_camera.Translate(Current_camera.World_Direction.Normalise() * camera_pan * update_time);
                    break;
                case Keys.A:
                    // Pan left
                    Current_camera.Translate(Current_camera.World_Direction.Cross_Product(Current_camera.World_Direction_Up).Normalise() * -camera_pan * update_time);
                    break;
                case Keys.D:
                    // Pan right
                    Current_camera.Translate(Current_camera.World_Direction.Cross_Product(Current_camera.World_Direction_Up).Normalise() * camera_pan * update_time);
                    break;
                case Keys.S:
                    // Pan back
                    Current_camera.Translate(Current_camera.World_Direction.Normalise() * -camera_pan * update_time);
                    break;
                case Keys.Q:
                    // Pan up
                    Current_camera.Translate(Current_camera.World_Direction_Up.Normalise() * camera_pan * update_time);
                    break;
                case Keys.E:
                    // Pan down
                    Current_camera.Translate(Current_camera.World_Direction_Up.Normalise() * -camera_pan * update_time);
                    break;
                case Keys.I:
                    // Rotate up
                    Matrix4x4 transformation_up = Transform.Quaternion_to_Matrix(Transform.Quaternion_Rotation(Current_camera.World_Direction.Cross_Product(Current_camera.World_Direction_Up), camera_tilt * update_time));
                    Current_camera.World_Direction = new Vector3D(transformation_up * new Vector4D(Current_camera.World_Direction));
                    Current_camera.World_Direction_Up = new Vector3D(transformation_up * new Vector4D(Current_camera.World_Direction_Up));
                    break;
                case Keys.J:
                    // Rotate left
                    Matrix4x4 transformation_left = Transform.Quaternion_to_Matrix(Transform.Quaternion_Rotation(Current_camera.World_Direction_Up, camera_tilt * update_time));
                    Current_camera.World_Direction = new Vector3D(transformation_left * new Vector4D(Current_camera.World_Direction));
                    Current_camera.World_Direction_Up = new Vector3D(transformation_left * new Vector4D(Current_camera.World_Direction_Up));
                    break;
                case Keys.L:
                    // Rotate right
                    Matrix4x4 transformation_right = Transform.Quaternion_to_Matrix(Transform.Quaternion_Rotation(Current_camera.World_Direction_Up, -camera_tilt * update_time));
                    Current_camera.World_Direction = new Vector3D(transformation_right * new Vector4D(Current_camera.World_Direction));
                    Current_camera.World_Direction_Up = new Vector3D(transformation_right * new Vector4D(Current_camera.World_Direction_Up));
                    break;
                case Keys.K:
                    // Rotate down
                    Matrix4x4 transformation_down = Transform.Quaternion_to_Matrix(Transform.Quaternion_Rotation(Current_camera.World_Direction.Cross_Product(Current_camera.World_Direction_Up), -camera_tilt * update_time));
                    Current_camera.World_Direction = new Vector3D(transformation_down * new Vector4D(Current_camera.World_Direction));
                    Current_camera.World_Direction_Up = new Vector3D(transformation_down * new Vector4D(Current_camera.World_Direction_Up));
                    break;
                case Keys.U:
                    // Roll left
                    Matrix4x4 transformation_roll_left = Transform.Quaternion_to_Matrix(Transform.Quaternion_Rotation(Current_camera.World_Direction, -camera_tilt * update_time));
                    Current_camera.World_Direction_Up = new Vector3D(transformation_roll_left * new Vector4D(Current_camera.World_Direction_Up));
                    break;
                case Keys.O:
                    // Roll right
                    Matrix4x4 transformation_roll_right = Transform.Quaternion_to_Matrix(Transform.Quaternion_Rotation(Current_camera.World_Direction, camera_tilt * update_time));
                    Current_camera.World_Direction_Up = new Vector3D(transformation_roll_right * new Vector4D(Current_camera.World_Direction_Up));
                    break;
            }
        }

        private void Canvas_Panel_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouse_down)
            {
                int delta_x = e.X - prev_x;
                int delta_y = e.Y - prev_y;
                prev_x = e.X;
                prev_y = e.Y;
                Debug.WriteLine("Translating... (" + delta_x + ", " + delta_y + ")");
                Current_camera.Translate_X(delta_x);
                Current_camera.Translate_Y(delta_y);
            }
        }

        private void Canvas_Panel_MouseUp(object sender, MouseEventArgs e)
        {
            mouse_down = false;
        }

        private void Canvas_Panel_MouseDown(object sender, MouseEventArgs e)
        {
            mouse_down = true;
        }
    }
}