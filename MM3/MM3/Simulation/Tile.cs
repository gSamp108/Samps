using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MM3.Simulation
{
    public sealed class Tile
    {
        public enum TerrainTypes { Ocean, Shallow, Flats, Forest, Hill, Mountain }
        public enum BiomeTypes { Cold, Temperate, Hot }

        public World World;
        public Position Position;
        public TerrainTypes Terrain;
        public BiomeTypes Biome;
        public PointOfInterest PointOfInterest;

        public Tile(World world, Position position)
        {
            this.World = world;
            this.Position = position;
        }
    }
}
