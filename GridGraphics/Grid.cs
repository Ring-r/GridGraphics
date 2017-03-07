using System;
using System.Drawing;

namespace GridGraphics
{
    class Grid
    {
        public class Anchor
        {
            private float coef;
            private float shift;
            private float size;

            protected void Recalculate(bool asCoef)
            {
                if (size == 0)
                    return;

                if (asCoef)
                    shift = coef * size;
                else
                    coef = shift / size;
            }

            public bool AsCoef { get; set; }

            public float Coef
            {
                get
                {
                    return coef;
                }
                set
                {
                    coef = value;

                    Recalculate(true);
                }
            }
            public float Shift
            {
                get
                {
                    return shift;
                }
                set
                {
                    shift = value;

                    Recalculate(false);
                }
            }
            protected float Size
            {
                get
                {
                    return size;
                }
                set
                {
                    SetSize(value, AsCoef);
                }
            }
            protected void SetSize(float value, bool asCoef)
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException();

                if (size == value)
                    return;

                size = value;

                Recalculate(asCoef);
            }

            public void MoveBy(float delta)
            {
                if (delta == 0)
                    return;

                Shift += delta;
            }
        }

        public class AnchorView : Anchor
        {
            public new float Size
            {
                get
                {
                    return base.Size;
                }
                set
                {
                    base.Size = value;
                }
            }
            public new void SetSize(float value, bool asCoef)
            {
                base.SetSize(value, asCoef);
            }

        }

        public class AxisSettings : Anchor
        {
            private int count;
            private float step = 1;

            public int Count
            {
                get
                {
                    return count;
                }
                set
                {
                    SetCount(value, AsCoef);
                }
            }
            public void SetCount(int value, bool asCoef)
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException();

                count = value;

                SetSize(count * step, AsCoef);
            }
            public float Step
            {
                get
                {
                    return step;
                }
                set
                {
                    SetStep(value, AsCoef);
                }
            }
            public void SetStep(float value, bool asCoef)
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException();

                if (step == value)
                    return;

                step = value;

                SetSize(count * step, AsCoef);
            }

            public new float Size
            {
                get
                {
                    return base.Size;
                }
            }
        }

        public readonly AxisSettings AxisSettingsX = new AxisSettings();
        public readonly AxisSettings AxisSettingsY = new AxisSettings();

        public readonly AnchorView AnchorViewX = new AnchorView();
        public readonly AnchorView AnchorViewY = new AnchorView();

        public Grid()
        {
            AxisSettingsX.SetCount(10, true);
            AxisSettingsY.SetCount(10, true);
        }

        public void ScaleToFit()
        {
            if (AxisSettingsX.Count == 0 || AxisSettingsY.Count == 0)
                return;

            var step = Math.Min(AnchorViewX.Size / AxisSettingsX.Count, AnchorViewY.Size / AxisSettingsY.Count);
            AxisSettingsX.Step = step;
            AxisSettingsY.Step = step;
        }

        private readonly Pen pen = Pens.Black;
        public void Draw(Graphics graphics)
        {
            // TODO: Find correct start value closed to min.

            var minX = AnchorViewX.Shift - AxisSettingsX.Shift;
            var maxX = minX + AxisSettingsX.Size;
            var minY = AnchorViewY.Shift - AxisSettingsY.Shift;
            var maxY = minY + AxisSettingsY.Size;

            var stepX = AxisSettingsX.Step;
            var stepY = AxisSettingsY.Step;
            for (var x = minX; x < maxX - stepX / 2; x += stepX)
            {
                for (var y = minY; y < maxY - stepY / 2; y += stepY)
                    graphics.DrawRectangle(pen, x, y, stepX, stepY);
            }
        }
    }
}
