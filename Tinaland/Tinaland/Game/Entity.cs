using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tinaland.Game
{
    public sealed class Entity
    {
        public World World;
        public Chunk Chunk
        {
            get
            {
                return this.World.GetChunk(this.ChunkPosition);
            }
        }
        public Tile Tile
        {
            get
            {
                return this.Chunk.GetTile(TilePosition);
            }
        }

        public Position WorldPosition;
        public Position ChunkPosition
        {
            get
            {
                var x = (int)Math.Floor((float)this.WorldPosition.X / (float)(Chunk.Size * Tile.Size));
                var y = (int)Math.Floor((float)this.WorldPosition.Y / (float)(Chunk.Size * Tile.Size));
                return new Position(x, y);
            }
        }
        public Position ChunkLocalPosition
        {
            get
            {
                var x = this.WorldPosition.X - (this.ChunkPosition.X * (Chunk.Size * Tile.Size));
                var y = this.WorldPosition.Y - (this.ChunkPosition.Y * (Chunk.Size * Tile.Size));
                return new Position(x, y);
            }
        }
        public Position TilePosition
        {
            get
            {
                var x = (int)Math.Floor((float)this.ChunkLocalPosition.X / (float)Tile.Size);
                var y = (int)Math.Floor((float)this.ChunkLocalPosition.Y / (float)Tile.Size);
                return new Position(x, y);
            }
        }
        public Position TileLocalPosition
        {
            get
            {
                var x = this.ChunkLocalPosition.X - (this.TilePosition.X * Tile.Size);
                var y = this.ChunkLocalPosition.Y - (this.TilePosition.Y * Tile.Size);
                return new Position(x, y);
            }
        }

        public Entity(World world)
        {
            this.World = world;
        }
    }
}
