using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MM3.Simulation
{
    public class TileEntity
    {
        public Tile Tile;
        public World World { get { return this.Tile.World; } }
        public Position Position { get { return this.Tile.Position; } }

        public TileEntity(Tile tile)
        {
            this.Tile = tile;
        }
    }
}
