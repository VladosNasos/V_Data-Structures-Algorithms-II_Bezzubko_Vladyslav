using System;

namespace EastBulgariaPathFinderWPF.Models
{
    public class City : ICity
    {
        public string Name { get; private set; }

        public City(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("City name cannot be null or empty.", nameof(name));

            Name = name.Trim();
        }

        public override bool Equals(object obj)
        {
            if (obj is ICity city)
            {
                return Name.Equals(city.Name, StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }

        public override int GetHashCode() => Name.ToLower().GetHashCode();
    }
}
