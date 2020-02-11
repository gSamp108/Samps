using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MM3.Simulation
{
    public sealed class Time
    {
        public int Year;
        public int Month;
        public int Day;

        public Time() : this(1000, 1, 1) { }
        public Time(int year,int month,int day)
        {
            this.Year = year;
            this.Month = month;
            this.Day = day;
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
