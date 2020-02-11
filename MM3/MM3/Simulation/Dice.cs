using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MM3.Simulation
{
    public sealed class Dice
    {
        public Random Rng;

        public Dice(Random rng)
        {
            this.Rng = rng;
        }

        public int Roll(int amount, int size)
        {
            var result = 0;
            for (int i = 0; i < amount; i++)
            {
                result += 1 + this.Rng.Next(size);
            }
            return result;
        }
    }
}
