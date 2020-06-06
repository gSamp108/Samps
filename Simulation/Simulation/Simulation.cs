using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation
{
    public sealed class Simulation
    {
        public Random Rng { get; private set; }
        public MinMax ClustersPerUniverse = new MinMax(12, 50);
        public MinMax WorldsPerCluster = new MinMax(1, 9);
        public MinMax RegionsPerWorld = new MinMax(8, 16);
        public int[] AtmosphereTierChance = { 7, 11, 15 };
        public double[] OrganicsBonusAtTier = { 0.2d, 0.8d, 1d, 2d };
        public double[] MineralsBonusAtTier = { 1.5d, 1.2d, 1d, 0.8d };
        public double[] RadiologicalsBonusAtTier = { 2d, 1.2d, 1d, 0.5d };

        public Simulation()
        {
            this.Rng = new Random();
        }
    }
}
