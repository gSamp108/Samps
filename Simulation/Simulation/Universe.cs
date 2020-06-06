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
        public DataStore Clusters { get; private set; }
        public DataStore Worlds { get; private set; }
        public DataStore Regions { get; private set; }

        public Universe()
        {
            this.Simulation = new Simulation();
            this.Clusters = new DataStore();
            this.Worlds = new DataStore();
            this.Regions = new DataStore();
            this.Generate();
        }

        private void Generate()
        {
            var clustersToSpawn = this.Rng.Next(this.Simulation.ClustersPerUniverse);
            for (int i = 0; i < clustersToSpawn; i++)
            {
                var cluster = new Cluster(this);
            }
        }
    }
}
