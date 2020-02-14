using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MM3
{
    public static class Extenstions
    {
        private static Random rng = new Random();

        public static Point Wrap(this Point @this, Rectangle bounds)
        {
            var result = new Point(@this.X, @this.Y);

            while (result.X < bounds.Left) { result.X += ((-bounds.Left) + bounds.Right); }
            while (result.X >= bounds.Right) { result.X -= (bounds.Right - bounds.Left); }
            while (result.Y < bounds.Top) { result.Y += ((-bounds.Top) + bounds.Bottom); }
            while (result.Y >= bounds.Bottom) { result.Y -= (bounds.Bottom - bounds.Top); }

            return result;
        }

        public static type Random<type>(this HashSet<type> @this) { return @this.Random(Extenstions.rng); }
        public static type Random<type>(this HashSet<type> @this, Random random)
        {
            if (@this.Count < 1) return default(type);
            return @this.ToList().Random(random);
        }

        public static type Random<type>(this List<type> @this) { return @this.Random(Extenstions.rng); }
        public static type Random<type>(this List<type> @this, Random random)
        {
            if (@this.Count < 1) return default(type);
            return @this[random.Next(@this.Count)];
        }

        public static void DrawCorrectRectangle(this Graphics canvas, Pen pen, Rectangle rectangle)
        {
            canvas.DrawRectangle(pen, new Rectangle(rectangle.X, rectangle.Y, rectangle.Width - 1, rectangle.Height - 1));
        }
        public static void DrawBetterString(this Graphics canvas, string text, Font font, Brush forecolor, bool useBackground, Brush backcolor, Point position, bool verticalCenter, bool horizontalCenter)
        {
            var renderPosition = position;

            if (verticalCenter || horizontalCenter || useBackground)
            {
                var renderedTextSizeF = canvas.MeasureString(text, font);
                var renderedTextSize = new Size((int)renderedTextSizeF.Width, (int)renderedTextSizeF.Height);
                if (verticalCenter) renderPosition.Y = renderPosition.Y - (renderedTextSize.Height / 2);
                if (horizontalCenter) renderPosition.X = renderPosition.X - (renderedTextSize.Width / 2);
                if (useBackground) canvas.FillRectangle(backcolor, new Rectangle(renderPosition, renderedTextSize));
            }
            canvas.DrawString(text, font, forecolor, renderPosition);
        }
    }
}
