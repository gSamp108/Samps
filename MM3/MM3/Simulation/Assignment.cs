using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MM3.Simulation
{
    public sealed class Assignment
    {
        public enum AssignmentTypes { POILeader, POIWatchLeader, POIWatchman }

        public AssignmentTypes Type;
        public PointOfInterest PointOfInterest;
        public HashSet<Creature> Creatures = new HashSet<Creature>();
        public Creature Creature;

        public Assignment(AssignmentTypes type, PointOfInterest pointOfInterest = null)
        {
            this.Type = type;
            this.PointOfInterest = pointOfInterest;
        }

        public void Assign(Creature creature)
        {
            if (creature.Assignment != null) creature.Assignment.Unassign(creature);
            creature.Assignment = this;
            if (this.Type == AssignmentTypes.POILeader) this.Creature = creature;
            else if (this.Type == AssignmentTypes.POIWatchLeader) this.Creature = creature;
            else if (this.Type == AssignmentTypes.POIWatchman) this.Creatures.Add(creature);
            else throw new InvalidOperationException("Assignment type not handled");
        }

        public void Unassign(Creature creature)
        {
            if (this.Type == AssignmentTypes.POILeader) this.Creature = null;
            else if (this.Type == AssignmentTypes.POIWatchLeader) this.Creature = null;
            else if (this.Type == AssignmentTypes.POIWatchman) this.Creatures.Remove(creature);
            else throw new InvalidOperationException("Assignment type not handled");
            this.Creature.Assignment = null;
        }
    }
}
