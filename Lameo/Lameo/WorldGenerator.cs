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

        public HashSet<Point> open = new HashSet<Point>();
        public HashSet<Point> closed = new HashSet<Point>();
        public Dictionary<Point, int> weight = new Dictionary<Point, int>();
        public Queue<Point> addQueue = new Queue<Point>();
        public Dictionary<Point, HashSet<Point>> links = new Dictionary<Point, HashSet<Point>>();
        public HashSet<Point> queued = new HashSet<Point>();
        public int AddThreshold = 5;
        public int RemoveThreshold = 3;
        public Statuses Status;
        public int SizeThreshold = 100;
        public List<Point> RemoveList = new List<Point>();
        public Random Rng;

        public WorldGenerator(int seed)
        {
            this.Rng = new Random(seed);
        }

        private void AddToClosed(Point p)
        {
            this.open.Remove(p);
            this.queued.Remove(p);
            this.closed.Add(p);

            foreach (var adjacent in p.Adjacent())
            {
                if (!this.links.ContainsKey(adjacent)) this.links.Add(adjacent, new HashSet<Point>());
                this.links[adjacent].Add(p);
                if (!this.closed.Contains(adjacent)) this.open.Add(adjacent);
            }
            foreach (var nearby in p.Nearby())
            {
                if (!this.weight.ContainsKey(nearby)) this.weight.Add(nearby, 0);
                this.weight[nearby] += 1;
                if (this.weight[nearby] > this.AddThreshold && !this.closed.Contains(nearby) && !this.queued.Contains(nearby))
                {
                    this.addQueue.Enqueue(nearby);
                    this.queued.Add(nearby);
                }
            }
        }

        private void RemoveFromClosed(Point p)
        {
            this.closed.Remove(p);
            if (this.links.ContainsKey(p) && this.links[p].Count == 0) this.open.Remove(p);

            foreach (var adjacent in p.Adjacent())
            {
                if (this.links.ContainsKey(adjacent))
                {
                    this.links[adjacent].Remove(p);
                    if (this.links[adjacent].Count == 0) this.open.Remove(adjacent);
                }
            }
            foreach (var nearby in p.Nearby())
            {
                if (!this.closed.Contains(nearby))
                {
                    if (this.weight.ContainsKey(nearby)) this.weight[nearby] -= 1;
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
                this.RemoveList = this.weight.Where(o => o.Value < this.RemoveThreshold && this.closed.Contains(o.Key)).Select(o => o.Key).ToList();
                if (this.RemoveList.Count == 0)
                {
                    if (this.closed.Count >= this.SizeThreshold) this.Status = Statuses.Complete;
                    else this.Status = Statuses.AddLoop;
                }
            }
            else
            {
                var point = this.RemoveList.Random();
                this.RemoveList.Remove(point);
                this.RemoveFromClosed(point);
            }
        }

        private void TickAddLoop()
        {
            if (this.addQueue.Count > 0)
            {
                var point = this.addQueue.Dequeue();
                this.AddToClosed(point);
            }
            else if (this.closed.Count < this.SizeThreshold)
            {
                var point = this.open.Random(this.Rng);
                this.AddToClosed(point);
            }
            else
            {
                this.Status = Statuses.RemoveLoop;
            }
        }

        private void Initialize()
        {
            this.open.Add(new Point());
            this.Status = Statuses.AddLoop;
        }
    }
}
