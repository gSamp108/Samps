using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coreworlds.Core
{
    public sealed class World
    {
        public int TotalChunksLoaded { get { return this.chunks.Count; } }

        private Dictionary<Position, Chunk> chunks = new Dictionary<Position, Chunk>();

        public Chunk GetChunk(Position chunkPosition)
        {
            if (!this.chunks.ContainsKey(chunkPosition))
            {
                var chunk = new Chunk(this, chunkPosition);
                chunk.Load();
                this.chunks.Add(chunkPosition, chunk);                
            }
            return this.chunks[chunkPosition];
        }
        public Tile GetTile(Position position)
        {
            var chunkPosition = this.GetChunkPosition(position);
            var chunk = this.GetChunk(chunkPosition);
            return chunk.GetTile(position);
        }

        public Position GetChunkPosition(Position position)
        {
            return new Position((int)Math.Floor((float)position.X / (float)Chunk.Size), (int)Math.Floor((float)position.Y / (float)Chunk.Size));
        }
    }
}
