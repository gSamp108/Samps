using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation
{
    public sealed class GraphicsTextHelper
    {
        private Graphics canvas;
        private Font font;
        private Brush brush;
        private Point origin;
        private Point shift;
        private int shiftAmount;

        public GraphicsTextHelper(Graphics canvas, Font font, Brush brush, Point origin, int shiftAmount)
        {
            this.canvas = canvas;
            this.font = font;
            this.brush = brush;
            this.origin = origin;
            this.shift = this.origin;
            this.shiftAmount = shiftAmount;
        }
        public void Draw(string text)
        {
            this.Draw(text, this.brush);
        }
        public void Draw(string text, Brush brush)
        {
            canvas.DrawString(text, this.font, brush, new Point(this.origin.X + this.shift.X, this.origin.Y + this.shift.Y));
            this.shift.Y += this.shiftAmount;
        }
    }
}
