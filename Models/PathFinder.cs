using System;
using System.Collections.Generic;
using System.Linq;

namespace EastBulgariaPathFinderWPF.Models
{
    public class PathFinder : IPathFinder
    {
        private readonly IGraph _graph;

        public PathFinder(IGraph graph)
        {
            _graph = graph ?? throw new ArgumentNullException(nameof(graph));
        }

        public PathResult FindQuickestPath(ICity start, ICity end)
        {
            if (start == null)
                throw new ArgumentNullException(nameof(start));
            if (end == null)
                throw new ArgumentNullException(nameof(end));
            if (start.Equals(end))
                return new PathResult { Path = new List<ICity> { start }, TotalTimeHours = 0, TotalDistanceKm = 0 };

            var times = new Dictionary<ICity, double>();
            var distances = new Dictionary<ICity, double>();
            var previous = new Dictionary<ICity, ICity>();
            var queue = new PriorityQueue<ICity, double>();

            foreach (var city in _graph.GetCities())
            {
                times[city] = double.PositiveInfinity;
                distances[city] = 0;
            }

            times[start] = 0;
            queue.Enqueue(start, 0);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();

                if (current.Equals(end))
                    break;

                foreach (var road in _graph.GetRoadsFrom(current))
                {
                    var neighbor = road.To;
                    var travelTime = road.DistanceKm / road.MaxSpeedKmh;
                    var newTime = times[current] + travelTime;
                    var newDistance = distances[current] + road.DistanceKm;

                    if (newTime < times[neighbor])
                    {
                        times[neighbor] = newTime;
                        distances[neighbor] = newDistance;
                        previous[neighbor] = current;
                        queue.Enqueue(neighbor, newTime);
                    }
                }
            }

            if (!previous.ContainsKey(end) && !start.Equals(end))
                throw new InvalidOperationException($"No path found from {start.Name} to {end.Name}.");

            var path = new List<ICity>();
            var totalTime = times[end];
            var totalDistance = distances[end];

            var currentPath = end;
            while (currentPath != null)
            {
                path.Add(currentPath);
                previous.TryGetValue(currentPath, out currentPath);
            }

            path.Reverse();

            return new PathResult
            {
                Path = path,
                TotalTimeHours = Math.Round(totalTime, 2),
                TotalDistanceKm = Math.Round(totalDistance, 2)
            };
        }
    }
}
