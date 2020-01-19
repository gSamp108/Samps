using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tinaland.Game
{
    public sealed class World
    {
        public int Size;
        public Engine Engine;
        private Chunk[,] chunks;

        public World(Engine engine, int size)
        {
            this.Engine = engine;
            this.Size = size;
            this.chunks = new Chunk[this.Size, this.Size];
            for (int x = 0; x < this.Size; x++)
            {
                for (int y = 0; y < this.Size; y++)
                {
                    this.chunks[x, y] = new Chunk(this, new Position(x, y));
                }
            }
        }

        public Chunk GetChunk(Position position)
        {
            var wrapped = new Position(position.X, position.Y);

            while (wrapped.X < 0) { wrapped.X += this.Size; }
            while (wrapped.X >= this.Size) { wrapped.X -= this.Size; }
            while (wrapped.Y < 0) { wrapped.Y += this.Size; }
            while (wrapped.Y >= this.Size) { wrapped.Y -= this.Size; }

            return this.chunks[wrapped.X, wrapped.Y];
        }

        public void Tick()
        {
        }
    }
}
