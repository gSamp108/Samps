using System.Collections.Generic;

namespace MM3.Simulation
{
    public struct Position 
    {
        public int X;
        public int Y;

        public Position(int x,int y)
        {
            this.X = x;
            this.Y = y;
        }

        public override bool Equals(object obj)
        {
            return (obj is Position) && (((Position)obj).X == this.X) && (((Position)obj).Y == this.Y);
        }

        public override int GetHashCode()
        {
            unchecked 
            {
                int hash = 17;
                hash = hash * 23 + this.X.GetHashCode();
                hash = hash * 23 + this.Y.GetHashCode();
                return hash;
            }
        }

        public override string ToString()
        {
            return "(" + this.X.ToString() + ", " + this.Y.ToString() + ")";
        }

        public IEnumerable<Position> Adjacent
        {
            get
            {
                yield return new Position(this.X + 0, this.Y - 1);
                yield return new Position(this.X + 1, this.Y + 0);
                yield return new Position(this.X + 0, this.Y + 1);
                yield return new Position(this.X - 1, this.Y + 0);
            }
        }
        public IEnumerable<Position> Nearby
        {
            get
            {
                yield return new Position(this.X + 0, this.Y - 1);
                yield return new Position(this.X + 1, this.Y - 1);
                yield return new Position(this.X + 1, this.Y + 0);
                yield return new Position(this.X + 1, this.Y + 1);
                yield return new Position(this.X + 0, this.Y + 1);
                yield return new Position(this.X - 1, this.Y + 1);
                yield return new Position(this.X - 1, this.Y + 0);
                yield return new Position(this.X - 1, this.Y - 1);
            }
        }

        public Position Wrap(Bounds bounds)
        {
            var result = new Position(this.X, this.Y);

            while (result.X < 0) { result.X = result.X + bounds.Width; }
            while (result.X >= bounds.Width) { result.X = result.X - bounds.Width; }
            while (result.Y < 0) { result.Y = result.Y + bounds.Height; }
            while (result.Y >= bounds.Height) { result.Y = result.Y - bounds.Height; }

            return result;
        }

    }
}
