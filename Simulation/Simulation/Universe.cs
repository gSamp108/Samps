using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation
{
    public sealed class Universe 
    {
        public Simulation Simulation { get; private set; }
        public Random Rng { get { return this.Simulation.Rng; } }
        public IdStore Clusters { get; private set; }
        public IdStore Worlds { get; private set; }
        public IdStore Regions { get; private set; }

        public Universe()
        {
            this.Simulation = new Simulation();
            this.Clusters = new IdStore();
            var clustersToSpawn = this.Rng.Next(this.Simulation.ClustersPerUniverse);
            for (int i = 0; i < clustersToSpawn; i++)
            {
                var cluster = new Cluster(this);
            }
        }
    }
}
