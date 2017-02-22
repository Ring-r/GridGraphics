using System;
using System.Drawing;
using System.Windows.Forms;

namespace GridGraphics
{
    public partial class MainForm : Form
    {
        private class Grid
        {
            public int X { get; set; }
            public int Y { get; set; }

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

            public void ScaleToFit(int width, int height)
            {
                if (iCount == 0 || jCount == 0)
                    return;

                Step = Math.Min(width / iCount, height / jCount);
            }

            public void MoveToCenter(int width, int height)
            {
                X = (width - Step * iCount) / 2;
                Y = (height - Step * jCount) / 2;
            }

            public void Draw(Graphics graphics, Pen pen)
            {
                var x = X;
                for (var i = 0; i < iCount - 1; i++)
                {
                    var y = Y;
                    for (var j = 0; j < jCount - 1; j++)
                    {
                        graphics.DrawRectangle(pen, x, y, Step, Step);
                        y += Step;
                    }
                    x += Step;
                }
            }
        }

        private readonly Grid grid = new Grid();

        private readonly Pen gridPen = Pens.Silver;

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
            grid.Draw(e.Graphics, gridPen);
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

            grid.X += e.X - mouseLocation.X;
            grid.Y += e.Y - mouseLocation.Y;

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
