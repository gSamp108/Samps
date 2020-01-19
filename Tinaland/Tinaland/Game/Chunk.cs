using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tinaland.Game
{
    public sealed class Chunk
    {
        public const int Size = 8;
        public Position Position;
        public World World;

        private Tile[,] tiles;

        public Chunk(World world, Position position)
        {
            this.World = world;
            this.Position = position;
            this.tiles = new Tile[Chunk.Size, Chunk.Size];
            for (int x = 0; x < Chunk.Size; x++)
            {
                for (int y = 0; y < Chunk.Size; y++)
                {
                    this.tiles[x, y] = new Tile(this, new Position(x, y));
                }
            }
        }

        public Tile GetTile(Position position)
        {
            var wrapped = false;
            var tileWrap = new Position();
            var chunkWrap = new Position();

            if (position.X < 0)
            {
                wrapped = true;
                tileWrap.X += Chunk.Size;
                chunkWrap.X -= 1;
            }
            if (position.X >= Chunk.Size)
            {
                wrapped = true;
                tileWrap.X -= Chunk.Size;
                chunkWrap.X += 1;
            }
            if (position.Y < 0)
            {
                wrapped = true;
                tileWrap.Y += Chunk.Size;
                chunkWrap.Y -= 1;
            }
            if (position.Y >= Chunk.Size)
            {
                wrapped = true;
                tileWrap.Y -= Chunk.Size;
                chunkWrap.Y += 1;
            }

            if (wrapped)
            {
                var wrappedTilePosition = new Position(position.X + tileWrap.X, position.Y + tileWrap.Y);
                var wrappedChunk = this.World.GetChunk(new Position(this.Position.X - chunkWrap.X, this.Position.Y - chunkWrap.Y));
                return wrappedChunk.GetTile(wrappedTilePosition);
            }
            else
            {
                return this.tiles[position.X, position.Y];
            }
        }
    }
}
