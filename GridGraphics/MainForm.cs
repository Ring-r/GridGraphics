using System;
using System.Drawing;
using System.Windows.Forms;

namespace GridGraphics
{
    public partial class MainForm : Form
    {
        private class Grid
        {
            private int iCount = 10;
            private int jCount = 10;

            private float gridCoefX;
            private float gridSizeX
            {
                get
                {
                    return Step * iCount;
                }
            }
            private float viewCoefX;
            private float viewSizeX;
            public float ViewSizeX
            {
                get
                {
                    return viewSizeX;
                }
                set
                {
                    if (value < 0)
                        throw new ArgumentException();

                    viewSizeX = value;
                }
            }

            private float gridCoefY;
            private float gridSizeY
            {
                get
                {
                    return Step * jCount;
                }
            }
            private float viewCoefY;
            private float viewSizeY;
            public float ViewSizeY
            {
                get
                {
                    return viewSizeY;
                }
                set
                {
                    if (value < 0)
                        throw new ArgumentException();

                    viewSizeY = value;
                }
            }

            public void SetAnchor(float viewCoefX, float gridCoefX, float viewCoefY, float gridCoefY)
            {
                this.viewCoefX = viewCoefX;
                this.gridCoefX = gridCoefX;

                this.viewCoefY = viewCoefY;
                this.gridCoefY = gridCoefY;
            }

            private float step = 1;
            public float Step
            {
                get
                {
                    return step;
                }
                set
                {
                    if (value <= 0)
                        return;

                    if (step == value)
                        return;

                    step = value;
                }
            }

            public void ScaleToFit()
            {
                if (iCount == 0 || jCount == 0)
                    return;

                Step = Math.Min(viewSizeX / iCount, viewSizeY / jCount);
            }

            public void MoveByCell(int di, int dj)
            {
                if (iCount <= 0 || jCount <= 0)
                    return;

                gridCoefX += (float)di / iCount;
                gridCoefY += (float)dj / jCount;
            }

            public void MoveByView(float dx, float dy)
            {
                if (viewSizeX == 0 || viewSizeY == 0)
                    return;

                viewCoefX += dx / ViewSizeX;
                viewCoefY += dy / ViewSizeY;
            }

            private readonly Pen pen = Pens.Black;
            public void Draw(Graphics graphics)
            {
                // TODO: Find correct start value closed to min.

                var minX = viewCoefX * viewSizeX - gridCoefX * gridSizeX;
                var maxX = minX + gridSizeX;
                var minY = viewCoefY * viewSizeY - gridCoefY * gridSizeY;
                var maxY = minY + gridSizeY;

                var step = Step;
                for (var x = minX; x < maxX - step / 2; x += step)
                {
                    for (var y = minY; y < maxY - step / 2; y += step)
                        graphics.DrawRectangle(pen, x, y, step, step);
                }
            }
        }

        private readonly Grid grid = new Grid();


        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            var di = 0;
            var dj = 0;
            if (e.KeyCode == Keys.Up)
                dj += 1;
            if (e.KeyCode == Keys.Right)
                di -= 1;
            if (e.KeyCode == Keys.Down)
                dj -= 1;
            if (e.KeyCode == Keys.Left)
                di += 1;
            grid.MoveByCell(di, dj);

            Invalidate();
        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
            if (e.KeyCode == Keys.Enter)
            {
                grid.SetAnchor(0.5f, 0.5f, 0.5f, 0.5f);
                grid.ScaleToFit();

                Invalidate();
            }
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            grid.Draw(e.Graphics);
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            grid.ViewSizeX = ClientSize.Width;
            grid.ViewSizeY = ClientSize.Height;
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

            grid.MoveByView(e.X - mouseLocation.X, e.Y - mouseLocation.Y);

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
