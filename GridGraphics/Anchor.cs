using System;

namespace GridGraphics
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
        public float Size
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
        public void SetSize(float value, bool asCoef)
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException();

            if (size == value)
                return;

            size = value;

            Recalculate(asCoef);
        }
    }
}
