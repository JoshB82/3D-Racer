using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace _3D_Racer
{
    public partial class MainForm : Form
    {
        public Bitmap Canvas { get; set; }
        public List<Shape> Entity_List = new List<Shape>();
        private int prev_x, prev_y;
        private int selected_shape;
        private bool mouse_down;

        private const float rotation_dampener = 0.005f;
        private const float grav_acc = -9.81f;

        private Camera Current_camera;
        
        public static int Canvas_width { get; set; }
        public static int Canvas_height { get; set; }

        public MainForm()
        {
            InitializeComponent();

            Canvas_width = Canvas_Panel.Width;
            Canvas_height = Canvas_Panel.Height;

            Cube default_cube = new Cube(0, 0, 0, 100);
            default_cube.Selected = true;
            Entity_List.Add(default_cube);

            World_Point origin = new World_Point(0, 0, 0);
            //Entity_List.Add(origin);
            Current_camera = new Perspective_Camera(200, 200, 200, origin, 100, 100, 50, 250);

            Thread graphics_thread = new Thread(Game_Loop);
            graphics_thread.Start();
            graphics_thread.IsBackground = true;
        }

        private void Game_Loop()
        {
            bool game_running = true;

            Canvas = new Bitmap(Canvas_width, Canvas_height);

            DateTime start_time = DateTime.Now;

            while (game_running)
            {
                // Time check
                DateTime now_time = DateTime.Now;
                TimeSpan time_elapsed = now_time - start_time;
                start_time = now_time;

                // User input
                // Update objects
                // ApplyImpulse();
                // Render
                Render();
            }
        }

        private void Render()
        {
            Bitmap temp = new Bitmap(Canvas_width, Canvas_height);

            using (Graphics g = Graphics.FromImage(temp))
            {
                List<Shape> temp_list = Entity_List;
                foreach (Shape shape in temp_list) shape.Draw_Shape(Current_camera, g);
            }

            Canvas = temp;
            Canvas_Panel.Invalidate();
        }

        private void Canvas_Panel_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImageUnscaled(Canvas, Point.Empty);
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Canvas_Panel_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Entity_List.Add(new Cube(e.X, e.Y, 100, 100));
                Debug.WriteLine("Cube created!");
            }
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            Random rnd = new Random();
            int random_object = rnd.Next(0, Entity_List.Count - 1);
            Entity_List[selected_shape].Selected = false;
            Entity_List[random_object].Selected = true;
            selected_shape = random_object;
        }

        private void Canvas_Panel_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouse_down)
            {
                int delta_x = e.X - prev_x;
                int delta_y = e.Y - prev_y;
                prev_x = e.X;
                prev_y = e.Y;
                Debug.WriteLine("Rotating... (" + delta_x + ", " + delta_y + ")");
                Entity_List[selected_shape].Rotate_X(delta_y * rotation_dampener);
                Entity_List[selected_shape].Rotate_Y(delta_x * rotation_dampener);
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