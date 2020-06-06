using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation
{
    public struct Position
    {
        public static Position operator +(Position a, Position b)
        {
            return new Position(a.X + b.X, a.Y + b.Y);
        }
        public static Position operator -(Position a, Position b)
        {
            return new Position(a.X - b.X, a.Y - b.Y);
        }
        public static Position operator *(Position a, int b)
        {
            return new Position(a.X * b, a.Y * b);
        }

        public int X { get; set; }
        public int Y { get; set; }

        private const int hashCodePrime = 486187739;

        public Position(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public override bool Equals(object obj)
        {
            return (obj is Position && ((Position)obj).X == this.X && ((Position)obj).Y == this.Y);
        }

        public override int GetHashCode()
        {
            //Source: (https://stackoverflow.com/a/263416)
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                // Suitable nullity checks etc, of course :)
                hash = hash * Position.hashCodePrime + this.X.GetHashCode();
                hash = hash * Position.hashCodePrime + this.Y.GetHashCode();
                return hash;
            }
        }

        public override string ToString()
        {
            return "(" + this.X + ", " + this.Y + ")";
        }
    }
}
