using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation
{
    public static class Extensions
    {
        public static int Next(this Random rng, MinMax value)
        {
            return rng.Next((value.Max + 1) - value.Min) + value.Min;
        }
        public static bool Chance(this Random rng, double value)
        {
            return rng.NextDouble() < value;
        }
        public static int Dice(this Random rng, int number, int size)
        {
            var result = 0;
            for (int i = 0; i < number; i++)
            {
                result += rng.Next(size) + 1;
            }
            return result;
        }
        public static int Clamp(this int value, int min, int max)
        {
            var result = value;
            if (result < min) result = min;
            if (result > max) result = max;
            return result;
        }
    }
}
