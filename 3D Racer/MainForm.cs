﻿using System;
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

        private const float rotation_dampener = 0.005f;
        private const float grav_acc = -9.81f;

        private const int max_frames_per_second = 60;
        private const int max_updates_per_second = 60;

        private Scene scene;
        private Camera Current_camera;
        
        public MainForm()
        {
            InitializeComponent();

            scene = new Scene(Canvas_Box.Width, Canvas_Box.Height);

            Cube default_cube = new Cube(new Vector3D(0, 0, 0), 50);
            default_cube.Selected = true;
            scene.Add(default_cube);

            Line test_line = new Line(new Vector3D(0,0,0),new Vector3D(100,0,0));
            scene.Add(test_line);

            //World_Point origin = new World_Point(0, 0, 0);
            //Entity_List.Add(origin);
            Current_camera = new Perspective_Camera(new Vector3D(500, 0, 0), default_cube, 100, 100, 50, 750);

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

            while (game_running)
            {
                now_time = Get_UNIX_Time_Milliseconds();
                frame_delta_time += (now_time - start_time);
                update_delta_time += (now_time - start_time);
                start_time = now_time;

                if (frame_delta_time >= frame_optimal_time)
                {
                    // Update objects
                    // ApplyImpulse(frame_delta_time);
                    no_frames++;
                }

                if (update_delta_time >= update_optimal_time)
                {
                    // Render
                    scene.Render(Canvas_Box, Current_camera);
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
                scene.Add(new Cube(new Vector3D(e.X, e.Y, 100), 100));
                Debug.WriteLine("Cube created!");
            }
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch(e.KeyCode)
            {
                case Keys.W:
                    // Pan forward
                    Current_camera.Translate(Current_camera.World_Direction.Normalise() * 30);
                    break;
                case Keys.A:
                    // Pan left
                    Current_camera.Translate(Current_camera.World_Direction.Cross_Product(Current_camera.World_Direction_Up).Normalise() * -30);
                    break;
                case Keys.D:
                    // Pan right
                    Current_camera.Translate(Current_camera.World_Direction.Cross_Product(Current_camera.World_Direction_Up).Normalise() * 30);
                    break;
                case Keys.S:
                    // Pan back
                    Current_camera.Translate(Current_camera.World_Direction.Normalise() * -30);
                    break;
                case Keys.Q:
                    Current_camera.Translate(Current_camera.World_Direction_Up.Normalise() * 30); // UPSIDE DOWN
                    break;
                case Keys.E:
                    Current_camera.Translate(Current_camera.World_Direction_Up.Normalise() * -30); // UPSIDE DOWN
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
                //Entity_List[selected_shape].Rotate_X(delta_y * rotation_dampener);
                //Entity_List[selected_shape].Rotate_Y(delta_x * rotation_dampener);
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