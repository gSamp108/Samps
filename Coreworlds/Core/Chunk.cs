using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coreworlds.Core
{
    public sealed class Chunk
    {
        public const int Size = 16;

        public readonly World World;
        public readonly Position Position;

        private Tile[,] tiles;
        private Random rng = new Random();

        public Chunk(World world, Position position)
        {
            this.World = world;
            this.Position = position;
        }

        public void Load()
        {
            this.tiles = new Tile[Chunk.Size, Chunk.Size];

            for (var x = 0; x < Chunk.Size; x++)
            {
                for (var y = 0; y < Chunk.Size; y++)
                {
                    var tilePosition = new Position((this.Position.X * Chunk.Size) + x, (this.Position.Y * Chunk.Size) + y);
                    var perlinValue = (Noise.CalcPixel2D(x, y, 100) / 255f);
                    var tile = new Tile(this.World, tilePosition);

                    if (perlinValue < 0.2)
                    {
                        tile.Terrain = TerrainTypes.Water;
                    }
                    else if (perlinValue >= 0.2 && perlinValue < 0.3)
                    {
                        tile.Terrain = TerrainTypes.Sand;
                    }
                    else if (perlinValue >= 0.3)
                    {
                        tile.Terrain = TerrainTypes.Grass;
                    }

                    if (false)
                    {
                        var terrain = this.rng.Next(4);
                        if (terrain == 0) tile.Terrain = TerrainTypes.DeepWater;
                        if (terrain == 1) tile.Terrain = TerrainTypes.Water;
                        if (terrain == 2) tile.Terrain = TerrainTypes.Sand;
                        if (terrain == 3) tile.Terrain = TerrainTypes.Grass;
                    }

                    this.tiles[x, y] = tile;
                }
            }
        }

        public Tile GetTile(Position position)
        {
            var tilePositionInChunk = position - (this.Position * Chunk.Size);
            return this.tiles[tilePositionInChunk.X, tilePositionInChunk.Y];
        }
    }
}
