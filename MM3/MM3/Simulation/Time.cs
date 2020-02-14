using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MM3.Simulation
{
    public sealed class Time
    {
        public int Year { get; private set; }
        public int Month { get; private set; }
        public int Day { get; private set; }

        public Time() : this(1000, 1, 1) { }
        public Time(int year, int month, int day)
        {
            this.Year = year;
            this.Month = month;
            this.Day = day;
        }

        public override string ToString()
        {
            return "(" + this.Year.ToString("0000") + "." + this.Month.ToString("00") + "." + this.Day.ToString("00") + ")";
        }

        public void Tick()
        {
            this.Day += 1;
            if (this.Day > 10)
            {
                this.Day = 1;
                this.Month += 1;
                if (this.Month > 10)
                {
                    this.Month = 1;
                    this.Year += 1;
                }
            }
        }
    }
}
