﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MM3.Simulation
{
    public sealed class City : PointOfInterest
    {
        public City(Database database, Tile tile) : base(database, tile)
        {
        }
    }
}