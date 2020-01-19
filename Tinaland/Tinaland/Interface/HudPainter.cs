using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Tinaland.Interface
{
    public sealed class HudPainter
    {
        private Graphics canvas;
        private Font font;
        private int y;

        public HudPainter(Graphics canvas, Font font)
        {
            this.font = font;
            this.Reinitialize(canvas);
        }

        public void DrawLine(string text, Brush brush)
        {
            canvas.DrawString(text, this.font, brush, new Point(10, y));
            this.y += 12;
        }

        public void Reinitialize(Graphics canvas)
        {
            this.canvas = canvas;
            this.y = 10;
        }
    }
}
