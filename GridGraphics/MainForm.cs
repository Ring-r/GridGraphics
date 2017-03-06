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
            grid.AxisSettingsX.MoveByCell(di);

            var dj = 0;
            if (e.KeyCode == Keys.Up)
                dj += 1;
            if (e.KeyCode == Keys.Down)
                dj -= 1;
            grid.AxisSettingsY.MoveByCell(di);

            Invalidate();
        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
            if (e.KeyCode == Keys.Enter)
            {
                grid.AxisSettingsX.AnchorCoef = 0.5f;
                grid.AxisSettingsX.AnchorViewCoef = 0.5f;
                grid.AxisSettingsY.AnchorCoef = 0.5f;
                grid.AxisSettingsY.AnchorViewCoef = 0.5f;
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
            grid.AxisSettingsX.SetViewSize(ClientSize.Width, true);
            grid.AxisSettingsY.SetViewSize(ClientSize.Height, true);
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

            grid.AxisSettingsX.MoveByView(e.X - mouseLocation.X);
            grid.AxisSettingsY.MoveByView(e.Y - mouseLocation.Y);

            mouseLocation = e.Location;

            Invalidate();
        }

        private void MainForm_MouseWheel(object sender, MouseEventArgs e)
        {
            const int WHEEL_DELTA = 120; // TODO: How get it from system?
            grid.SetStep(grid.Step - e.Delta / WHEEL_DELTA, true, true);

            Invalidate();
        }
    }
}
