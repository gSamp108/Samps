using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MM3.Simulation
{
    public sealed class Creature : TileEntity
    {
        public sealed class CreatureStats
        {
            public int Strength;
            public int Constitution;
            public int Dexterity;
            public int Intelligence;
            public int Charisma;
            public int Luck;
        }
        public sealed class CreatureSkills
        {

        }

        public CreatureStats BaseStats = new CreatureStats();
        public PointOfInterest HomePOI;
        public bool IsHeroic;
        public bool HasBeenGenerated;
        public Assignment Assignment;

        public Creature(Database database, Tile tile) : base(database, tile) { }

        public void Generate(PointOfInterest homePOI, bool isHeroic)
        {
            if (this.HasBeenGenerated) throw new InvalidOperationException("This creature object has already been generated");
            this.HomePOI = homePOI;
            this.IsHeroic = isHeroic;

            var numberOfStatDice = 3;
            if (this.IsHeroic) numberOfStatDice = 4;

            this.BaseStats.Charisma = this.Dice.Roll(numberOfStatDice, 6);
            this.BaseStats.Constitution = this.Dice.Roll(numberOfStatDice, 6);
            this.BaseStats.Dexterity = this.Dice.Roll(numberOfStatDice, 6);
            this.BaseStats.Intelligence = this.Dice.Roll(numberOfStatDice, 6);
            this.BaseStats.Luck = this.Dice.Roll(numberOfStatDice, 6);
            this.BaseStats.Strength = this.Dice.Roll(numberOfStatDice, 6);
        }
    }
}
