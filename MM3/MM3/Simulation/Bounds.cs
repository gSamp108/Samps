using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MM3.Simulation
{
    public struct Bounds
    {
        public int Width;
        public int Height;

        public Bounds(int width, int height)
        {
            this.Width = width;
            this.Height = height;
        }

        public override bool Equals(object obj)
        {
            return (obj is Bounds) && (((Bounds)obj).Width == this.Width) && (((Bounds)obj).Height == this.Height);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + this.Width.GetHashCode();
                hash = hash * 23 + this.Height.GetHashCode();
                return hash;
            }
        }

        public override string ToString()
        {
            return "(" + this.Width.ToString() + ", " + this.Height.ToString() + ")";
        }
    }
}
