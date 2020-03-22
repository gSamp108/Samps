using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lameo
{
    public sealed class World
    {
        public Rectangle Bounds;
        public HashSet<Point> Tiles = new HashSet<Point>();
        public Random Rng = new Random();

        public World(int landmass, int addThreshold, int removeThreshold)
        {
            //this.GenerateLandmass(landmass, addThreshold, removeThreshold);
            var wg = new WorldGenerator(this.Rng.Next());
            while (wg.Status!= WorldGenerator.Statuses.Complete)
            {
                wg.Tick();
            }
            foreach(var point in wg.Open)
            {
                this.Tiles.Add(point);
            }
        }

        private void GenerateLandmass(int landmass, int addThreshold, int removeThreshold)
        {
            var open = new HashSet<Point>();
            var closed = new HashSet<Point>();
            var weight = new Dictionary<Point, int>();
            var addQueue = new Queue<Point>();
            var links = new Dictionary<Point, HashSet<Point>>();
            var queued = new HashSet<Point>();

            open.Add(new Point());

            Action<Point> addToClosed = (Point p) =>
            {
                open.Remove(p);
                queued.Remove(p);
                closed.Add(p);

                foreach(var adjacent in p.Adjacent())
                {
                    if (!links.ContainsKey(adjacent)) links.Add(adjacent, new HashSet<Point>());
                    links[adjacent].Add(p);
                    if (!closed.Contains(adjacent)) open.Add(adjacent);
                }
                foreach (var nearby in p.Nearby())
                {
                    if (!weight.ContainsKey(nearby)) weight.Add(nearby, 0);
                    weight[nearby] += 1;
                    if (weight[nearby] > addThreshold && !closed.Contains(nearby) &&  !queued.Contains(nearby) )
                    {
                        addQueue.Enqueue(nearby);
                        queued.Add(nearby);
                    }
                }
            };

            Action<Point> removeFromClosed = (Point p) =>
            {
                closed.Remove(p);
                if (links.ContainsKey(p) && links[p].Count == 0) open.Remove(p);

                foreach (var adjacent in p.Adjacent())
                {
                    if (links.ContainsKey(adjacent))
                    {
                        links[adjacent].Remove(p);
                        if (links[adjacent].Count == 0) open.Remove(adjacent);
                    }
                }
                foreach (var nearby in p.Nearby())
                {
                    if (!closed.Contains(nearby))
                    {
                        if (weight.ContainsKey(nearby)) weight[nearby] -= 1;
                    }
                }
            };

            while (closed.Count < landmass)
            {
                while (closed.Count < landmass)
                {
                    var selectedPoint = open.Random(this.Rng);
                    addQueue.Enqueue(selectedPoint);

                    while (addQueue.Count > 0)
                    {
                        var point = addQueue.Dequeue();
                        addToClosed(point);
                    }
                }

                var hangingPoints = weight.Where(o => o.Value < removeThreshold && closed.Contains(o.Key)).Select(o => o.Key).ToList();
                while (hangingPoints.Count>0)
                {
                    foreach(var point in hangingPoints)
                    {
                        removeFromClosed(point);
                    }
                    hangingPoints = weight.Where(o => o.Value < removeThreshold && closed.Contains(o.Key)).Select(o => o.Key).ToList();
                }
            }

            foreach(var point in closed)
            {
                this.Tiles.Add(point);
            }
        }
    }
}
