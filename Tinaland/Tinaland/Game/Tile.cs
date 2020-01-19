using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tinaland.Game
{
    public sealed class Tile
    {
        public const int Size = 16;

        public Chunk Chunk;
        public Position ChunkPosition;
        public Position WorldPosition;
        public int Number;

        public Tile(Chunk chunk, Position position)
        {
            this.Chunk = chunk;
            this.ChunkPosition = position;
            this.WorldPosition = new Position(this.ChunkPosition.X + (this.Chunk.Position.X * Chunk.Size), this.ChunkPosition.Y + (this.Chunk.Position.Y * Chunk.Size));
        }
    }
}
