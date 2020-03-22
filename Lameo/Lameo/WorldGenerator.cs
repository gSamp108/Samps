using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lameo
{
    public sealed class WorldGenerator
    {
        public enum Statuses { Uninitialized, AddLoop, RemoveLoop, Complete }

        public HashSet<Point> Open = new HashSet<Point>();
        public HashSet<Point> Closed = new HashSet<Point>();
        public Dictionary<Point, int> Weight = new Dictionary<Point, int>();
        public Queue<Point> AddQueue = new Queue<Point>();
        public Dictionary<Point, HashSet<Point>> Links = new Dictionary<Point, HashSet<Point>>();
        public HashSet<Point> Queued = new HashSet<Point>();
        public int AddThreshold = 5;
        public int RemoveThreshold = 3;
        public Statuses Status;
        public int SizeThreshold = 100;
        public List<Point> RemoveList = new List<Point>();
        public Random Rng;
        public int Seed;

        public WorldGenerator(int seed)
        {
            this.Seed = seed;
            this.Rng = new Random(this.Seed);
        }

        private void AddToClosed(Point p)
        {
            this.Open.Remove(p);
            this.Queued.Remove(p);
            this.Closed.Add(p);

            foreach (var adjacent in p.Adjacent())
            {
                if (!this.Links.ContainsKey(adjacent)) this.Links.Add(adjacent, new HashSet<Point>());
                this.Links[adjacent].Add(p);
                if (!this.Closed.Contains(adjacent)) this.Open.Add(adjacent);
            }
            foreach (var nearby in p.Nearby())
            {
                if (!this.Weight.ContainsKey(nearby)) this.Weight.Add(nearby, 0);
                this.Weight[nearby] += 1;
                if (this.Weight[nearby] > this.AddThreshold && !this.Closed.Contains(nearby) && !this.Queued.Contains(nearby))
                {
                    this.AddQueue.Enqueue(nearby);
                    this.Queued.Add(nearby);
                }
            }
        }

        private void RemoveFromClosed(Point p)
        {
            this.Closed.Remove(p);
            if (this.Links.ContainsKey(p) && this.Links[p].Count == 0) this.Open.Remove(p);

            foreach (var adjacent in p.Adjacent())
            {
                if (this.Links.ContainsKey(adjacent))
                {
                    this.Links[adjacent].Remove(p);
                    if (this.Links[adjacent].Count == 0) this.Open.Remove(adjacent);
                }
            }
            foreach (var nearby in p.Nearby())
            {
                if (!this.Closed.Contains(nearby))
                {
                    if (this.Weight.ContainsKey(nearby)) this.Weight[nearby] -= 1;
                }
            }
        }

        public void Tick()
        {
            if (this.Status == Statuses.Uninitialized) this.Initialize();
            else if (this.Status == Statuses.AddLoop) this.TickAddLoop();
            else if (this.Status == Statuses.RemoveLoop) this.TickRemoveLoop();
        }

        private void TickRemoveLoop()
        {
            if (this.RemoveList.Count == 0)
            {
                this.RemoveList = this.Weight.Where(o => o.Value < this.RemoveThreshold && this.Closed.Contains(o.Key)).Select(o => o.Key).ToList();
                if (this.RemoveList.Count == 0)
                {
                    if (this.Closed.Count >= this.SizeThreshold) this.Status = Statuses.Complete;
                    else this.Status = Statuses.AddLoop;
                }
            }
            else
            {
                var point = this.RemoveList.Random(this.Rng);
                this.RemoveList.Remove(point);
                this.RemoveFromClosed(point);
            }
        }

        private void TickAddLoop()
        {
            if (this.AddQueue.Count > 0)
            {
                var point = this.AddQueue.Dequeue();
                this.AddToClosed(point);
            }
            else if (this.Closed.Count < this.SizeThreshold)
            {
                var point = this.Open.Random(this.Rng);
                this.AddToClosed(point);
            }
            else
            {
                this.Status = Statuses.RemoveLoop;
            }
        }

        private void Initialize()
        {
            this.Open.Add(new Point());
            this.Status = Statuses.AddLoop;
        }
    }
}
