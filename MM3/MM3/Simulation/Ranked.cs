using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MM3.Simulation
{
    public class Ranked
    {
        private sealed class DefaultSettings
        {
            public int DifficultyFactor = 10;
            public RankedTypes Type = RankedTypes.Linear;
            public int Level = 0;
            public bool UsePassionSystem = false;
            public int Passion = 0;
            public int Xp = 0;
        }
        private static DefaultSettings Default = new DefaultSettings();

        public enum RankedTypes { Flat, Linear, Exponential }

        public int DifficultyFactor { get; protected set; }
        public int Level { get; protected set; }
        public RankedTypes Type { get; protected set; }
        public float Xp { get; protected set; }
        public bool UsePassionSystem { get; private set; }
        public int Passion { get; private set; }

        public float XpToNextLevel => this.CalculateXpRequiredForLevel(this.Level + 1);

        public event Action<int> LevelChanged = delegate (int direction) { };

        public Ranked(RankedTypes type, int level, int xp, int difficultyFactor, bool usePassionSystem, int passion)
        {
            this.Type = type;
            this.Level = level;
            this.Xp = xp;
            this.DifficultyFactor = difficultyFactor;
            this.UsePassionSystem = usePassionSystem;
            this.Passion = passion;
        }
        public Ranked(int level) : this(Default.Type, level, Default.Xp, Default.DifficultyFactor, Default.UsePassionSystem, Default.Passion) { }
        public Ranked(int level, int difficultyFactor) : this(Default.Type, level, Default.Xp, difficultyFactor, Default.UsePassionSystem, Default.Passion) { }
        public Ranked() : this(Default.Type, Default.Level, Default.Xp, Default.DifficultyFactor, Default.UsePassionSystem, Default.Passion) { }

        public virtual void ChangeXp(float amount)
        {
            if (!this.UsePassionSystem) this.Xp += amount;
            else if (this.Passion == 0) this.Xp += (amount * 0.25f);
            else this.Xp += (amount * ((float)this.Passion));

            while (this.Xp < 0 && this.Level > 0)
            {
                this.Xp += this.CalculateXpRequiredForLevel(this.Level);
                this.Level -= 1;
                this.LevelChanged(-1);
            }
            if (this.Xp < 0) this.Xp = 0;
            while (this.Xp > this.XpToNextLevel)
            {
                this.Xp -= this.XpToNextLevel;
                this.Level += 1;
                this.LevelChanged(1);
            }
        }

        public void ForceLevelChange(int amount)
        {
            this.Level += amount;
            this.ChangeXp(0);
        }

        protected float CalculateXpRequiredForLevel(int level)
        {
            if (this.Type == RankedTypes.Flat) return this.DifficultyFactor;
            else if (this.Type == RankedTypes.Linear) return (this.DifficultyFactor * level);
            else if (this.Type == RankedTypes.Exponential) return (this.DifficultyFactor ^ level);
            else return 1;
        }

        public override string ToString()
        {
            if (this.UsePassionSystem) return this.Level.ToString() + this.GetPassionString() + " (" + this.Xp.ToString("0") + "/" + this.XpToNextLevel.ToString("0") + ")";
            else return this.Level.ToString() + " (" + this.Xp.ToString("0") + "/" + this.XpToNextLevel.ToString("0") + ")";
        }

        private string GetPassionString()
        {
            var result = string.Empty;
            for(int i=0;i<this.Passion;i++)
            {
                result += "*";
            }
            if (result.Length > 0) result = " " + result;
            return result;
        }
    }
}
