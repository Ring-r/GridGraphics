using System;
using System.Drawing;
using System.Windows.Forms;

namespace GridGraphics
{
    public partial class MainForm : Form
    {
        private class Grid
        {
            private int X { get; set; }
            private int Y { get; set; }

            private int step = 1;
            public int Step
            {
                get
                {
                    return step;
                }
                set
                {
                    step = Math.Max(1, value);
                }
            }

            private int iCount = 10;
            private int jCount = 10;

            private int MinX
            {
                get
                {
                    return 0;
                }
            }

            private int MaxX
            {
                get
                {
                    return Step * iCount;
                }
            }

            private int MinY
            {
                get
                {
                    return 0;
                }
            }

            private int MaxY
            {
                get
                {
                    return Step * jCount;
                }
            }

            public void ScaleToFit(int width, int height)
            {
                if (iCount == 0 || jCount == 0)
                    return;

                Step = Math.Min(width / iCount, height / jCount);
            }

            public void MoveBy(int width, int height, int dx, int dy)
            {
                X = Math.Min(Math.Max(MinX, X + dx), width - MaxX);
                Y = Math.Min(Math.Max(MinY, Y + dy), height - MaxY);
            }

            public void MoveToCenter(int width, int height)
            {
                X = (width - Step * iCount) / 2;
                Y = (height - Step * jCount) / 2;
            }

            private readonly Pen pen = Pens.Silver;
            public void Draw(Graphics graphics)
            {
                // TODO: Find correct start value closed to min.
                var minX = X + MinX;
                var maxX = X + MaxX;
                var minY = Y + MinY;
                var maxY = Y + MaxY;

                for (var x = minX; x <= maxX; x += Step)
                    graphics.DrawLine(pen, x, minY, x, maxY);

                for (var y = minY; y <= maxY; y += Step)
                    graphics.DrawLine(pen, minX, y, maxX, y);
            }
        }

        private readonly Grid grid = new Grid();


        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
            if (e.KeyCode == Keys.Enter)
            {
                grid.ScaleToFit(ClientSize.Width, ClientSize.Height);
                grid.MoveToCenter(ClientSize.Width, ClientSize.Height);

                Invalidate();
            }
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            grid.Draw(e.Graphics);
        }

        private Point mouseLocation;
        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            mouseLocation = e.Location;
        }

        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            grid.MoveBy(ClientSize.Width, ClientSize.Height, e.X - mouseLocation.X, e.Y - mouseLocation.Y);

            mouseLocation = e.Location;

            Invalidate();
        }

        private void MainForm_MouseWheel(object sender, MouseEventArgs e)
        {
            const int WHEEL_DELTA = 120; // TODO: How get it from system.
            grid.Step -= e.Delta / WHEEL_DELTA;

            Invalidate();
        }
    }
}
