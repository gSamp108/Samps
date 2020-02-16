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
        public HashSet<Creature> NamedPopulation = new HashSet<Creature>();
        public Assignment Leader;
        public Assignment FarmingSupervisor;

        public PointOfInterest(Database database, Tile tile) : base(database, tile)
        {
            this.InitializeAssignments();
            this.GeneralPopulation = new Ranked(this.Dice.Roll(10, 10), 100);
            this.GeneralPopulation.LevelChanged += this.GeneralPopulation_LevelChanged;
            this.SearchGeneralPopulationForHeroicCreatures();
            this.FarmingInfrastructure = new Ranked(this.Dice.Roll(1, 10), 10);
        }

        private void InitializeAssignments()
        {
            this.Leader = new Assignment();
            this.Leader.Type = Assignment.AssignmentTypes.PointOfInterestLeader;
            this.Leader.PointOfInterest = this;
            this.FarmingSupervisor = new Assignment();
            this.FarmingSupervisor.Type = Assignment.AssignmentTypes.FarmingSupervisor;
            this.FarmingSupervisor.PointOfInterest = this;
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
            if (this.Leader.Creature == null) this.FillVacancy(this.Leader);
            if (this.FarmingSupervisor.Creature == null) this.FillVacancy(this.FarmingSupervisor);
        }

        private void FillVacancy(Assignment assignment)
        {
            var selectedCreature = default(Creature);
            var unassignedPopulation = this.NamedPopulation.Where(o => o.Assignment == null).ToList();
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

            if (selectedCreature != null)
            {
                selectedCreature.Assignment = assignment;
                assignment.Creature = selectedCreature;
            }
        }

        private Creature GenerateCreatureFromPopulation(bool isHeroic)
        {
            this.GeneralPopulation.ForceLevelChange(-1);
            var creature = this.World.GenerateCreature(this.Tile);
            creature.Generate(this, isHeroic);
            this.NamedPopulation.Add(creature);
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
