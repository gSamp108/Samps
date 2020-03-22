using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lameo
{
    public static class Extensions
    {
        private static Random rng = new Random();

        //public static type Random<type>(this HashSet<type> @this)
        //{
        //    return @this.Random(Extensions.rng);
        //}

        public static type Random<type>(this HashSet<type> @this, Random rng)
        {
            return @this.ToList().Random(rng);
        }

        //public static type Random<type>(this List<type> @this)
        //{
        //    return @this.Random(Extensions.rng);
        //}

        public static type Random<type>(this List<type> @this, Random rng)
        {
            if (@this.Count == 0) return default(type);
            return @this[rng.Next(@this.Count)];
        }

        public static IEnumerable<Point> Adjacent(this Point @this)
        {
            yield return new Point(@this.X, @this.Y - 1);
            yield return new Point(@this.X + 1, @this.Y);
            yield return new Point(@this.X, @this.Y + 1);
            yield return new Point(@this.X - 1, @this.Y);
        }

        public static IEnumerable<Point> Nearby(this Point @this)
        {
            yield return new Point(@this.X, @this.Y - 1);
            yield return new Point(@this.X + 1, @this.Y - 1);
            yield return new Point(@this.X + 1, @this.Y);
            yield return new Point(@this.X + 1, @this.Y + 1);
            yield return new Point(@this.X, @this.Y + 1);
            yield return new Point(@this.X - 1, @this.Y + 1);
            yield return new Point(@this.X - 1, @this.Y);
            yield return new Point(@this.X - 1, @this.Y - 1);
        }
    }
}
