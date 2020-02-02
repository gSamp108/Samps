using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MM3.Simulation
{
    public sealed class World
    {
        public Random Rng = new Random();
        public int Size;

        private Tile[,] tiles;

        public World (int size)
        {
            this.Size = size;
            this.tiles = new Tile[this.Size, this.Size];
            this.TileGeneration();
        }

        private void TileGeneration()
        {
            for (int x = 0; x < this.Size; x++)
            {
                for (int y = 0; y < this.Size; y++)
                {
                    this.tiles[x, y] = new Tile(this, new Position(x, y));
                    if (this.Rng.Next(5) > 0) this.tiles[x, y].IsLand = true;
                }
            }
        }

        public void Tick()
        {

        }

        public Position WrapPosition(Position position)
        {
            var result = new Position(position.X, position.Y);

            while (result.X < 0) { result.X = result.X + this.Size; }
            while (result.X >= this.Size) { result.X = result.X - this.Size; }
            while (result.Y < 0) { result.Y = result.Y + this.Size; }
            while (result.Y >= this.Size) { result.Y = result.Y - this.Size; }

            return result;
        }

        public Tile GetTile(Position position)
        {
            var fixedPosition = this.WrapPosition(position);
            return this.tiles[fixedPosition.X, fixedPosition.Y];
        }
    }
}
