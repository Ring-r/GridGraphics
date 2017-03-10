using System;

namespace GridGraphics
{
    public class AnchorGrid : Anchor
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

        public void ShiftCells(int di)
        {
            Shift += di * Step;
        }
    }
}
