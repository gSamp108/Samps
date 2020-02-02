using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MM3.Simulation
{
    public sealed class Creature
    {        
        public Tile Tile;
        public World World { get { return this.Tile.World; } }
        public Position Position { get { return this.Tile.Position; } }
        public Random Rng { get { return this.World.Rng; } }

        public int HealthFromConstitution;

        public Stats BaseStats = new Stats();

        public int Constitution { get { return this.BaseStats.Constitution; } }

        public int Health;
        public int MaxHealth { get { return this.Constitution * this.HealthFromConstitution; } }

        public Creature(Tile tile)
        {
            this.Tile = tile;
            this.HealthFromConstitution = 10;
            this.BaseStats.Strength = this.Rng.Next(6) + this.Rng.Next(6) + this.Rng.Next(6) + 3;
            this.BaseStats.Constitution = this.Rng.Next(6) + this.Rng.Next(6) + this.Rng.Next(6) + 3;
            this.Health = this.MaxHealth;
        }
    }
}
