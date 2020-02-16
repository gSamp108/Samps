using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MM3.Simulation
{
    public sealed class Time
    {
        private const int MinutesPerHour = 60;
        private const int HoursPerDay = 24;
        private const int DaysPerMonth = 30;
        private const int MonthsPerYear = 10;

        public int Year { get; private set; }
        public int Month { get; private set; }
        public int Day { get; private set; }
        public int Hour { get; private set; }
        public int Minute { get; private set; }

        public event Action MinutePassed = delegate { };
        public event Action HourPassed = delegate { };
        public event Action DayPassed = delegate { };
        public event Action MonthPassed = delegate { };
        public event Action YearPassed = delegate { };

        public Time(int year, int month, int day, int hour, int minute)
        {
            this.Year = year;
            this.Month = month;
            this.Day = day;
            this.Hour = hour;
            this.Minute = minute;
        }

        public override string ToString()
        {
            var yearMonthDay = "(" + this.Year.ToString("0000") + "." + this.Month.ToString("00") + "." + this.Day.ToString("00") + ")";
            var hoursMinutes = "[" + this.Hour.ToString("00") + "." + this.Minute.ToString("00") + "]";
            return yearMonthDay + hoursMinutes;
        }

        public void Advance(int minute, int hour, int day, int month, int year)
        {
            for (int tick = 0; tick < minute; tick++)
            {
                this.Minute += 1;
                this.Rebalance();
                this.MinutePassed();
            }
            for (int tick = 0; tick < hour; tick++)
            {
                this.Hour += 1;
                this.Rebalance();
                this.HourPassed();
            }
            for (int tick = 0; tick < day; tick++)
            {
                this.Day += 1;
                this.Rebalance();
                this.DayPassed();
            }
            for (int tick = 0; tick < month; tick++)
            {
                this.Month += 1;
                this.Rebalance();
                this.MonthPassed();
            }
            for (int tick = 0; tick < year; tick++)
            {
                this.Year += 1;
                this.Rebalance();
                this.YearPassed();
            }
        }

        private void Rebalance()
        {
            var hourPassed = false;
            var dayPassed = false;
            var monthPassed = false;
            var yearPassed = false;

            if (this.Minute == Time.MinutesPerHour)
            {
                hourPassed = true;
                this.Minute = 0;
                this.Hour += 1; 
            }
            if (this.Hour == Time.HoursPerDay)
            {
                dayPassed = true;
                this.Hour = 0;
                this.Day += 1;
            }
            if (this.Day > Time.DaysPerMonth)
            {
                monthPassed = true;
                this.Day = 1;
                this.Month += 1;
            }
            if (this.Month > Time.MonthsPerYear)
            {
                yearPassed = true;
                this.Month = 1;
                this.Year += 1;
            }

            if (hourPassed) this.HourPassed();
            if (dayPassed) this.DayPassed();
            if (monthPassed) this.MonthPassed();
            if (yearPassed) this.YearPassed();
        }
    }
}
