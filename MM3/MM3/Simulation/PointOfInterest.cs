using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MM3.Simulation
{
    public class PointOfInterest : TileEntity
    {
        public Ranked GeneralPopulation;
        public Ranked FarmingInfrastructure;
        public HashSet<Creature> Visitors = new HashSet<Creature>();
        public Assignment Leader;
        public Assignment WatchLeader;
        public Assignment Watchman;

        public PointOfInterest(Database database, Tile tile) : base(database, tile)
        {
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
            this.FillVacancies();
            this.TickPopulationGrowth();
        }

        private void FillVacancies()
        {
            if (this.Leader == null) this.Leader = new Assignment(Assignment.AssignmentTypes.POILeader, this);
            if (this.WatchLeader == null) this.WatchLeader = new Assignment(Assignment.AssignmentTypes.POIWatchLeader, this);
            if (this.Watchman == null) this.Watchman = new Assignment(Assignment.AssignmentTypes.POIWatchman, this);

            if (this.Leader.Creature == null)
            {
                if (this.WatchLeader.Creature == null) this.FillVacancy(this.Leader);
                else this.Leader.Assign(this.WatchLeader.Creature);
            }

            if (this.WatchLeader.Creature == null)
            {
                if (this.Watchman.Creatures.Count > 0)
                {
                    var creature = this.Watchman.Creatures.Random();
                    this.WatchLeader.Assign(creature);
                }
                else this.FillVacancy(this.WatchLeader);
            }

            var watchCount = (this.GeneralPopulation.Level / 10);
            var watchmanSelectionFailed = false;
            while (this.Watchman.Creatures.Count < watchCount && !watchmanSelectionFailed)
            {
                var selectedCreature = this.GenerateCreatureFromPopulation(false);
                if (selectedCreature == null) watchmanSelectionFailed = true;
                else this.Watchman.Assign(selectedCreature);
            }
        }

        private void FillVacancy(Assignment assignment)
        {
            var selectedCreature = default(Creature);
            var unassignedPopulation = this.Visitors.Where(o => o.Assignment == null).ToList();
            if (unassignedPopulation.Count > 0)
            {
                foreach (var unassignedCreature in unassignedPopulation)
                {
                    if (unassignedCreature.OfferAssignment(assignment))
                    {
                        selectedCreature = unassignedCreature;
                        break;
                    }
                }
            }
            if (selectedCreature == null) selectedCreature = this.GenerateCreatureFromPopulation(false);
            if (selectedCreature != null) assignment.Assign(selectedCreature);
        }

        private Creature GenerateCreatureFromPopulation(bool isHeroic)
        {
            this.GeneralPopulation.ForceLevelChange(-1);
            var creature = this.World.GenerateCreature(this.Tile);
            creature.Generate(this, isHeroic);
            this.Visitors.Add(creature);
            return creature;
        }

        private void TickPopulationGrowth()
        {
            var baseGrowthFactor = this.FarmingInfrastructure.Level;
            var randomness = this.Dice.Roll(1, 6);
            if (this.Rng.Next(3) == 0) randomness = -randomness;
            var growthFactor = baseGrowthFactor + randomness;
            this.GeneralPopulation.ChangeXp(growthFactor);
        }
    }
}
