using System.Drawing;
using System.Windows.Forms;

namespace GridGraphics
{
    public partial class MainForm : Form
    {
        private class Grid
        {
            private const int iCount = 10;
            private const int jCount = 10;

            private const float xStep = 10;
            private const float yStep = 10;

            public void Draw(Graphics graphics, Pen pen, float dx, float dy, float scale)
            {
                var x = dx;
                for (var i = 0; i < iCount - 1; i++)
                {
                    var y = dy;
                    for (var j = 0; j < jCount - 1; j++)
                    {
                        graphics.DrawRectangle(pen, x, y, xStep, yStep);
                        y += yStep;
                    }
                    x += xStep;
                }
            }
        }

        private float x = 0;
        private float y = 0;

        private float scale = 1;

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
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            grid.Draw(e.Graphics, gridPen, x, y, scale);
        }

        private bool isMouseDown = false;
        private int mouseDownX;
        private int mouseDownY;
        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            isMouseDown = true;

            mouseDownX = e.X;
            mouseDownY = e.Y;
        }

        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isMouseDown)
                return;

            x += e.X - mouseDownX;
            mouseDownX = e.X;

            y += e.Y - mouseDownY;
            mouseDownY = e.Y;

            Invalidate();
        }

        private void MainForm_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
        }

        private void MainForm_MouseWheel(object sender, System.EventArgs e)
        {

        }
    }
}
