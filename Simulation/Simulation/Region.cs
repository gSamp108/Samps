using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation
{
    public sealed class Region : IdMemeber
    {
        public Simulation Simulation { get { return this.World.Simulation; } }
        public Random Rng { get { return this.Simulation.Rng; } }
        public World World { get; private set; }

        public double Organics { get; private set; }
        public double Minerals { get; private set; }
        public double Radiologicals { get; private set; }

        public Region(World world) : base(world.Regions)
        {
            this.World = world;
            this.Organics = this.Rng.NextDouble() * this.Simulation.OrganicsBonusAtTier[this.World.AtmosphereLevel];
            this.Minerals = this.Rng.NextDouble() * this.Simulation.MineralsBonusAtTier[this.World.AtmosphereLevel];
            this.Radiologicals = this.Rng.NextDouble() * this.Simulation.RadiologicalsBonusAtTier[this.World.AtmosphereLevel];
        }
    }
}
