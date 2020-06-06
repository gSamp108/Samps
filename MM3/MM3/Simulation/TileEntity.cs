using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MM3.Simulation
{
    public class TileEntity : Database.Member
    {
        private const int CurrentVersion = 1;

        public Position Position;
        public Tile Tile
        {
            get { return this.World.GetTile(this.Position); }
            set { this.Position = value.Position; }
        }

        public World World { get; private set; }
        public Random Rng { get { return this.World.Rng; } }
        public Dice Dice { get { return this.World.Dice; } }

        public TileEntity(Database database, World world, Position position) : base(database)
        {
            this.World = world;
            this.Position = position;
        }

        public virtual void Tick(Time time)
        {

        }

        public override void SaveToStream(BinaryWriter writer)
        {
            base.SaveToStream(writer);
            writer.Write(TileEntity.CurrentVersion);
            writer.Write(this.Position);
        }

        public override void LoadFromStream(BinaryReader reader)
        {
            base.LoadFromStream(reader);
            var version = reader.ReadInt32();
            this.Position = reader.ReadPosition();
        }
    }
}
