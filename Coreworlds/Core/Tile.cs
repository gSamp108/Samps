using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coreworlds.Core
{
    public sealed class Tile
    {
        public readonly World World;
        public readonly Position Position;

        public TerrainTypes Terrain;

        public Tile(World world, Position position)
        {
            this.World = world;
            this.Position = position;
        }
    }
}
