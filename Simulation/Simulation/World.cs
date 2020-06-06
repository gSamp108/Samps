using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation
{
    public sealed class World : IdMemeber
    {
        public Simulation Simulation { get { return this.Cluster.Simulation; } }
        public Random Rng { get { return this.Simulation.Rng; } }
        public Cluster Cluster { get; private set; }
        public IdStore Regions { get; private set; }
        public int AtmosphereLevel;

        public World(Cluster cluster) : base(cluster.Worlds)
        {
            this.Cluster = cluster;
            this.Regions = new IdStore();
            var atmosphere = this.Rng.Dice(3, 6);
            if (atmosphere > this.Simulation.AtmosphereTierChance[0]) this.AtmosphereLevel += 1;
            if (atmosphere > this.Simulation.AtmosphereTierChance[1]) this.AtmosphereLevel += 1;
            if (atmosphere > this.Simulation.AtmosphereTierChance[2]) this.AtmosphereLevel += 1;


            var regionsToSpawn = this.Rng.Next(this.Simulation.RegionsPerWorld);
            for (int i = 0; i < regionsToSpawn; i++)
            {
                var region = new Region(this);
            }
        }
    }
}
