using System;
using System.Drawing;

namespace GridGraphics
{
    class Grid
    {
        public class AxisSettings
        {
            private int count;
            private float step = 1;
            private float size;

            private float anchorCoef;
            private float anchorShift;

            private float anchorViewCoef;
            private float anchorViewShift;
            private float viewSize;

            private void RecalculateAnchor(bool anchorAsCoef)
            {
                if (anchorAsCoef)
                    ; // TODO: Recalculate anchorShift;
                else
                    ; // TODO: Recalculate anchorCoef;
            }

            private void RecalculateAnchorView(bool anchorAsCoef)
            {
                if (anchorAsCoef)
                    ; // TODO: Recalculate anchorViewShift;
                else
                    ; // TODO: Recalculate anchorViewCoef;
            }

            public int Count
            {
                get
                {
                    return count;
                }
            }
            public void SetCount(int value, bool anchorAsCoef)
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException();

                count = value;

                RecalculateAnchor(anchorAsCoef);
            }
            public void SetStep(float value, bool anchorAsCoef)
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException();

                if (step == value)
                    return;

                step = value;

                RecalculateAnchor(anchorAsCoef);
            }

            public float Size
            {
                get
                {
                    return count * step;
                }
            }

            public float AnchorCoef
            {
                get
                {
                    return anchorCoef;
                }
                set
                {
                    anchorCoef = value;

                    RecalculateAnchor(true);
                }
            }
            public float AnchorShift
            {
                get
                {
                    return anchorShift;
                }
                set
                {
                    anchorShift = value;

                    RecalculateAnchor(false);
                }
            }

            public float AnchorViewCoef
            {
                get
                {
                    return anchorViewCoef;
                }
                set
                {
                    anchorViewCoef = value;

                    RecalculateAnchorView(true);
                }
            }
            public float AnchorViewShift
            {
                get
                {
                    return anchorViewShift;
                }
                set
                {
                    anchorViewShift = value;

                    RecalculateAnchorView(false);
                }
            }
            public float ViewSize
            {
                get
                {
                    return viewSize;
                }
            }
            public void SetViewSize(float value, bool anchorViewAsCoef)
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException();

                if (viewSize == value)
                    return;

                viewSize = value;

                RecalculateAnchorView(anchorViewAsCoef);
            }

            public void MoveByCell(int shift)
            {
                if (shift == 0 || Count == 0)
                    return;

                AnchorCoef += (float)shift / Count;
            }

            public void MoveByView(float shift)
            {
                if (shift == 0)
                    return;

                anchorViewShift += shift / ViewSize;
            }
        }

        public readonly AxisSettings AxisSettingsX = new AxisSettings();
        public readonly AxisSettings AxisSettingsY = new AxisSettings();

        public Grid()
        {
            AxisSettingsX.SetCount(10, true);
            AxisSettingsY.SetCount(10, true);
        }

        private float viewSizeX;

        private float viewSizeY;

        private float step = 1;
        public float Step
        {
            get
            {
                return step;
            }
        }
        public void SetStep(float value, bool anchorAsCoefX, bool anchorAsCoefY)
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException();

            if (step == value)
                return;

            step = value;

            AxisSettingsX.SetStep(step, anchorAsCoefX);
            AxisSettingsY.SetStep(step, anchorAsCoefY);
        }

        public void ScaleToFit()
        {
            if (AxisSettingsX.Count == 0 || AxisSettingsY.Count == 0)
                return;

            var step = Math.Min(viewSizeX / AxisSettingsX.Count, viewSizeY / AxisSettingsY.Count);
            AxisSettingsX.SetStep(step, true); // TODO: Is it correct?
            AxisSettingsY.SetStep(step, true); // TODO: Is it correct?
        }

        private readonly Pen pen = Pens.Black;
        public void Draw(Graphics graphics)
        {
            // TODO: Find correct start value closed to min.

            var minX = AxisSettingsX.AnchorViewShift - AxisSettingsX.AnchorShift;
            var maxX = minX + AxisSettingsX.Size;
            var minY = AxisSettingsY.AnchorViewShift - AxisSettingsY.AnchorShift;
            var maxY = minY + AxisSettingsY.Size;

            var step = Step;
            for (var x = minX; x < maxX - step / 2; x += step)
            {
                for (var y = minY; y < maxY - step / 2; y += step)
                    graphics.DrawRectangle(pen, x, y, step, step);
            }
        }
    }
}
