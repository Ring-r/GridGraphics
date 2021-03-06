﻿using System;
using System.Drawing;

namespace GridGraphics
{
    class Grid
    {
        public readonly Anchor AnchorX = new Anchor();
        public readonly Anchor AnchorY = new Anchor();

        public readonly AnchorGrid AnchorGridX = new AnchorGrid();
        public readonly AnchorGrid AnchorGridY = new AnchorGrid();

        public Grid()
        {
            AnchorGridX.SetCount(10, true);
            AnchorGridX.Parent = AnchorX;

            AnchorGridY.SetCount(10, true);
            AnchorGridY.Parent = AnchorY;
        }

        public void MoveToCenter()
        {
            AnchorX.Coef = 0.5f;
            AnchorY.Coef = 0.5f;

            AnchorGridX.Coef = 0.5f;
            AnchorGridY.Coef = 0.5f;
        }

        public void ScaleToFit()
        {
            if (AnchorGridX.Count == 0 || AnchorGridY.Count == 0)
                return;

            var step = Math.Min(AnchorX.Size / AnchorGridX.Count, AnchorY.Size / AnchorGridY.Count);
            AnchorGridX.Step = step;
            AnchorGridY.Step = step;
        }

        private readonly Pen pen = Pens.Black;
        private readonly Pen crossPen = Pens.Red;
        private readonly float crossHalfSize = 5f;
        public void Draw(Graphics graphics)
        {
            // TODO: Find correct start value closed to min.

            var minX = AnchorGridX.GlobalShift;
            var maxX = minX + AnchorGridX.Size;

            var minY = AnchorGridY.GlobalShift;
            var maxY = minY + AnchorGridY.Size;

            var stepX = AnchorGridX.Step;
            var stepY = AnchorGridY.Step;
            for (var x = minX; x < maxX - stepX / 2; x += stepX)
            {
                for (var y = minY; y < maxY - stepY / 2; y += stepY)
                    graphics.DrawRectangle(pen, x, y, stepX, stepY);
            }

            var crossX = AnchorX.Shift;
            var crossY = AnchorY.Shift;
            graphics.DrawLine(crossPen, crossX - crossHalfSize, crossY, crossX + crossHalfSize, crossY);
            graphics.DrawLine(crossPen, crossX, crossY - crossHalfSize, crossX, crossY + crossHalfSize);
        }
    }
}
