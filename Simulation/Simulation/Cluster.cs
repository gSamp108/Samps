using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation
{
    public sealed class Cluster:IdMemeber 
    {
        public Simulation Simulation { get { return this.Universe.Simulation; } }
        public Random Rng { get { return this.Simulation.Rng; } }
        public Universe Universe { get; private set; }

        private List<int> worldsById = new List<int>();

        public Cluster(Universe universe) : base(universe.Clusters)
        {
            this.Universe = universe;
            var worldsToSpawn = this.Rng.Next(this.Simulation.WorldsPerCluster);
            for (int i = 0; i < worldsToSpawn; i++)
            {
                var world = new World(this);
            }
        }

        public override string ToString()
        {
            return "Cluster " + this.Id.ToString();
        }
    }
}
