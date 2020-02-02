using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MM3.Simulation
{
    public sealed class Tile
    {
        public World World;
        public Position Position;
        public bool IsLand;

        public Tile(World world, Position position)
        {
            this.World = world;
            this.Position = position;
        }
    }
}
