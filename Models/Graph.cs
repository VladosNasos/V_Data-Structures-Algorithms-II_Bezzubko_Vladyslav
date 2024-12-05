using System;
using System.Collections.Generic;

namespace EastBulgariaPathFinderWPF.Models
{
    public class Graph : IGraph
    {
        private readonly Dictionary<string, ICity> _cities;
        private readonly Dictionary<ICity, List<IRoad>> _adjacencyList;

        public Graph()
        {
            _cities = new Dictionary<string, ICity>(StringComparer.OrdinalIgnoreCase);
            _adjacencyList = new Dictionary<ICity, List<IRoad>>();
        }

        public void AddCity(ICity city)
        {
            if (city == null)
                throw new ArgumentNullException(nameof(city));

            if (!_cities.ContainsKey(city.Name))
            {
                _cities[city.Name] = city;
                _adjacencyList[city] = new List<IRoad>();
            }
        }

        public void AddRoad(IRoad road)
        {
            if (road == null)
                throw new ArgumentNullException(nameof(road));

            AddCity(road.From);
            AddCity(road.To);

            _adjacencyList[road.From].Add(road);
            // Assuming roads are bidirectional
            var reverseRoad = new Road(road.To, road.From, road.DistanceKm, road.MaxSpeedKmh);
            _adjacencyList[road.To].Add(reverseRoad);
        }

        public IEnumerable<ICity> GetCities() => _cities.Values;

        public IEnumerable<IRoad> GetRoadsFrom(ICity city)
        {
            if (city == null)
                throw new ArgumentNullException(nameof(city));

            if (!_adjacencyList.ContainsKey(city))
                throw new ArgumentException($"City '{city.Name}' does not exist in the graph.");

            return _adjacencyList[city];
        }

        public ICity GetCityByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("City name cannot be null or empty.", nameof(name));

            if (_cities.TryGetValue(name.Trim(), out var city))
                return city;

            throw new KeyNotFoundException($"City '{name}' not found.");
        }
    }
}
