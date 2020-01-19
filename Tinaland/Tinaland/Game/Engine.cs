using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tinaland.Game
{
    public sealed class Engine
    {
        public Random Rng;

        public Engine()
        {
            this.Rng = new Random();
        }
    }
}
