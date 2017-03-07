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
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            var di = 0;
            if (e.KeyCode == Keys.Left)
                di += 1;
            if (e.KeyCode == Keys.Right)
                di -= 1;
            if (di != 0)
                grid.AxisSettingsX.MoveBy(di * grid.AxisSettingsX.Step);

            var dj = 0;
            if (e.KeyCode == Keys.Up)
                dj += 1;
            if (e.KeyCode == Keys.Down)
                dj -= 1;
            if (dj != 0)
                grid.AxisSettingsY.MoveBy(dj * grid.AxisSettingsY.Step);

            Invalidate();
        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
            if (e.KeyCode == Keys.Enter)
            {
                grid.AxisSettingsX.AsCoef = true;
                grid.AxisSettingsX.Coef = 0.5f;

                grid.AnchorViewX.AsCoef = true;
                grid.AnchorViewX.Coef = 0.5f;

                grid.AxisSettingsY.AsCoef = true;
                grid.AxisSettingsY.Coef = 0.5f;

                grid.AnchorViewY.AsCoef = true;
                grid.AnchorViewY.Coef = 0.5f;

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
            grid.AnchorViewX.Size = ClientSize.Width;
            grid.AnchorViewY.Size = ClientSize.Height;
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

            grid.AnchorViewX.MoveBy(e.X - mouseLocation.X);
            grid.AnchorViewY.MoveBy(e.Y - mouseLocation.Y);

            mouseLocation = e.Location;

            Invalidate();
        }

        private void MainForm_MouseWheel(object sender, MouseEventArgs e)
        {
            const int WHEEL_DELTA = 120; // TODO: How get it from system?
            var delta = e.Delta / WHEEL_DELTA;

            grid.AxisSettingsX.Step = Math.Max(1, grid.AxisSettingsX.Step - delta);
            grid.AxisSettingsY.Step = Math.Max(1, grid.AxisSettingsY.Step - delta);

            Invalidate();
        }
    }
}
