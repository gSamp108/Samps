using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MM3.Simulation
{
    public sealed class WorldSettings
    {
        private const int CurrentVersion = 1;

        public float SpecialCreatureChance = 0.01f;

        public void SaveToStream(BinaryWriter writer)
        {
            writer.Write(WorldSettings.CurrentVersion);
            writer.Write(this.SpecialCreatureChance);
        }

        public void LoadFromStream(BinaryReader reader)
        {
            var version = reader.ReadInt32();
            if (version == 1)
            {
                this.SpecialCreatureChance = reader.ReadSingle();
            }
        }
    }
}
