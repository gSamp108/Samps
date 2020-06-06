using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation
{
    public sealed class Cluster: DataStore.Member
    {
        public Simulation Simulation { get { return this.Universe.Simulation; } }
        public Random Rng { get { return this.Simulation.Rng; } }
        public Universe Universe { get; private set; }

        public IdStore<World> Worlds { get; private set; }

        public Cluster(Universe universe) : base(universe.Clusters)
        {
            this.Universe = universe;
            this.Worlds = new IdStore<World>(this.Universe.Worlds);
            this.Generate();
        }

        private void Generate()
        {
            var worldsToSpawn = this.Rng.Next(this.Simulation.WorldsPerCluster);
            for (int i = 0; i < worldsToSpawn; i++)
            {
                var world = new World(this);
                this.Worlds.Add(world.Id);
            }
        }

        public override string ToString()
        {
            return "Cluster " + this.Id.ToString();
        }
    }
}
