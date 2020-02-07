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
    }
}
