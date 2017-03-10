using System;
using System.Drawing;
using System.Windows.Forms;

namespace GridGraphics
{
    public partial class MainForm : Form
    {
        private readonly Grid grid = new Grid();

        public MainForm()
        {
            InitializeComponent();

            grid.AnchorX.AsCoef = true;
            grid.AnchorY.AsCoef = true;

            grid.AnchorGridX.AsCoef = true;
            grid.AnchorGridY.AsCoef = true;
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            var di = Convert.ToInt32(e.KeyCode == Keys.Left) - Convert.ToInt32(e.KeyCode == Keys.Right);
            grid.AnchorGridX.ShiftCells(di);

            var dj = Convert.ToInt32(e.KeyCode == Keys.Up) - Convert.ToInt32(e.KeyCode == Keys.Down);
            grid.AnchorGridY.ShiftCells(di);

            Invalidate();
        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
            if (e.KeyCode == Keys.Enter)
            {
                grid.MoveToCenter();
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
            grid.AnchorX.Size = ClientSize.Width;
            grid.AnchorY.Size = ClientSize.Height;
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

            grid.AnchorX.Shift += e.X - mouseLocation.X;
            grid.AnchorY.Shift += e.Y - mouseLocation.Y;

            mouseLocation = e.Location;

            Invalidate();
        }

        private void MainForm_MouseWheel(object sender, MouseEventArgs e)
        {
            const int WHEEL_DELTA = 120; // TODO: How get it from system?
            var delta = e.Delta / WHEEL_DELTA;

            grid.AnchorGridX.Step = Math.Max(1, grid.AnchorGridX.Step - delta);
            grid.AnchorGridY.Step = Math.Max(1, grid.AnchorGridY.Step - delta);

            Invalidate();
        }
    }
}
