using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MM3.Simulation
{
    public class TileEntity : Database.Member
    {
        public Tile Tile;
        public World World { get { return this.Tile.World; } }
        public Position Position { get { return this.Tile.Position; } }
        public Random Rng { get { return this.World.Rng; } }
        public Dice Dice { get { return this.World.Dice; } }

        public TileEntity(Database database, Tile tile) : base(database)
        {
            this.Tile = tile;
        }

        public virtual void Tick(Time time)
        {

        }
    }
}
