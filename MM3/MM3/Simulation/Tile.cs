using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MM3.Simulation
{
    public sealed class Tile
    {
        private const int CurrentVersion = 1;

        public enum TerrainTypes { Ocean, Shallow, Flats, Forest, Hill, Mountain }
        public enum BiomeTypes { Cold, Temperate, Hot }

        public World World;
        public Position Position;
        public TerrainTypes Terrain;
        public BiomeTypes Biome;

        private int PointOfInterestId;
        public PointOfInterest PointOfInterest
        {
            get
            {
                return this.World.GetPointOfInterest(this.PointOfInterestId);
            }
            set
            {
                if (value == null) this.PointOfInterestId = 0;
                else this.PointOfInterestId = value.DatabaseId;
            }
        }

        public Tile(World world, Position position)
        {
            this.World = world;
            this.Position = position;
        }

        public void SaveToStream(BinaryWriter writer)
        {
            writer.Write(Tile.CurrentVersion);
            writer.Write(this.Position);
            writer.Write((int)Terrain);
            writer.Write((int)Biome);
            writer.Write(this.PointOfInterestId);
        }
        public void LoadFromStream(BinaryReader reader)
        {
            var version = reader.ReadInt32();
            this.Position = reader.ReadPosition();
            this.Terrain = (TerrainTypes)reader.ReadInt32();
            this.Biome = (BiomeTypes)reader.ReadInt32();
            this.PointOfInterestId = reader.ReadInt32();
        }
    }
}
