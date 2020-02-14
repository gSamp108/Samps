using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MM3.Simulation
{
    public sealed class Assignment
    {
        public enum AssignmentTypes { None, PointOfInterestLeader, FarmingSupervisor }

        public AssignmentTypes Type;
        public PointOfInterest PointOfInterest;
        public Creature Creature;

    }
}
