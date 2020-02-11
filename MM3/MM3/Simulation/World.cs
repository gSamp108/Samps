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
        public Time Time = new Time();
        public Dice Dice;

        private Tile[,] tiles;

        public World (int size)
        {
            this.Size = size;
            this.Dice = new Dice(this.Rng);
            var worldGenerator = new WorldGenerator();
            this.tiles = worldGenerator.Generate(this.Rng, this);
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
