using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;

namespace _3D_Racer
{
    public sealed class Scene
    {
        private static readonly object locker = new object();
        public readonly List<Shape> Objects = new List<Shape>();
        public Bitmap Canvas { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }

        public Scene(int width, int height)
        {
            Width = width;
            Height = height;
            Canvas = new Bitmap(width, height);
        }

        public void Add(Shape shape)
        {
            lock (locker) Objects.Add(shape);
        }

        public void Add_From_File(string file)
        {
        }

        public void Remove(int ID)
        {
            lock (locker) Objects.RemoveAll(x => x.ID == ID);
        }

        public void Render(PictureBox canvas_box, Camera camera)
        {
            Bitmap temp = new Bitmap(Width, Height);

            using (Graphics g = Graphics.FromImage(temp))
            {
                lock (locker) foreach (Shape shape in Objects) shape.Draw_Shape(g, camera, Width, Height);
            }

            Canvas = temp;
            canvas_box.Invalidate();
        }
    }
}