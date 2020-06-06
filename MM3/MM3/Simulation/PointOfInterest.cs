using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MM3.Simulation
{
    public class PointOfInterest : TileEntity
    {
        private const int CurrentVersion = 1;

        public string Name { get; private set; }
        public Ranked GeneralPopulation;
        public Ranked FarmingInfrastructure;

        private HashSet<int> VisitorIds = new HashSet<int>();
        public IEnumerable<Creature> Visitors
        {
            get
            {
                foreach(var id in this.VisitorIds)
                {
                    yield return this.World.GetCreature(id);
                }
            }
        }


        public PointOfInterest(Database database, World world, Position position) : base(database, world, position)
        {
            this.Name = this.World.NameGenerator.GenerateName(this.Rng);
            this.GeneralPopulation = new Ranked(this.Dice.Roll(10, 10), 100);
            this.GeneralPopulation.LevelChanged += this.GeneralPopulation_LevelChanged;
            this.SearchGeneralPopulationForHeroicCreatures();
            this.FarmingInfrastructure = new Ranked(this.Dice.Roll(1, 10), 10);
        }

        private void SearchGeneralPopulationForHeroicCreatures()
        {
            var peopleToSearch = this.GeneralPopulation.Level;
            for (int i = 0; i < peopleToSearch; i++)
            {
                this.RollForHeroicCreature();
            }
        }

        private void GeneralPopulation_LevelChanged(int direction)
        {
            if (direction == 1) this.RollForHeroicCreature();                
        }

        private void RollForHeroicCreature()
        {
            var heroicRoll = this.Rng.NextDouble();
            if (heroicRoll < this.World.Settings.SpecialCreatureChance)
            {
                var creature = this.GenerateCreatureFromPopulation(true);
            }
        }

        public override void Tick(Time time)
        {
            base.Tick(time);
            this.TickPopulationGrowth();
        }

        private Creature GenerateCreatureFromPopulation(bool isHeroic)
        {
            this.GeneralPopulation.ForceLevelChange(-1);
            var creature = this.World.GenerateCreature(this.Tile);
            creature.Generate(this, isHeroic);
            this.AddVisitor(creature);
            return creature;
        }

        public void AddVisitor(Creature creature)
        {
            this.VisitorIds.Add(creature.DatabaseId);
        }

        private void TickPopulationGrowth()
        {
            var baseGrowthFactor = this.FarmingInfrastructure.Level;
            var randomness = this.Dice.Roll(1, 6);
            if (this.Rng.Next(3) == 0) randomness = -randomness;
            var growthFactor = baseGrowthFactor + randomness;
            this.GeneralPopulation.ChangeXp(growthFactor);
        }

        public override void SaveToStream(BinaryWriter writer)
        {
            base.SaveToStream(writer);
            writer.Write(PointOfInterest.CurrentVersion);
            writer.Write(this.Name);
            writer.Write(this.GeneralPopulation);
            writer.Write(this.FarmingInfrastructure);
            writer.Write(this.VisitorIds.Count);
            foreach(var visitorId in this.VisitorIds)
            {
                writer.Write(visitorId);
            }
        }
        public override void LoadFromStream(BinaryReader reader)
        {
            base.LoadFromStream(reader);
            var version = reader.ReadInt32();
            this.Name = reader.ReadString();
            this.GeneralPopulation = reader.ReadRanked();
            this.FarmingInfrastructure = reader.ReadRanked();
            var visitorCount = reader.ReadInt32();
            for (int i = 0; i < visitorCount; i++)
            {
                this.VisitorIds.Add(reader.ReadInt32());
            }
        }
    }
}
