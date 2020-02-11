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
        public Dice Dice { get { return this.World.Dice; } }

        public int HealthFromConstitution;

        public Stats BaseStats = new Stats();

        public int Constitution { get { return this.BaseStats.Constitution; } }

        public int Health;
        public int MaxHealth { get { return this.Constitution * this.HealthFromConstitution; } }

        public Creature(Tile tile)
        {
            this.Tile = tile;
            this.HealthFromConstitution = 10;
            this.BaseStats.Charisma = this.Dice.Roll(3, 6);
            this.BaseStats.Constitution = this.Dice.Roll(3, 6);
            this.BaseStats.Dexterity = this.Dice.Roll(3, 6);
            this.BaseStats.Intelligence = this.Dice.Roll(3, 6);
            this.BaseStats.Luck = this.Dice.Roll(3, 6);
            this.BaseStats.Strength = this.Dice.Roll(3, 6);
            this.Health = this.MaxHealth;
        }
    }
}
