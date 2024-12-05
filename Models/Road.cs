using System;

namespace EastBulgariaPathFinderWPF.Models
{
    public class Road : IRoad
    {
        public ICity From { get; private set; }
        public ICity To { get; private set; }
        public double DistanceKm { get; private set; }
        public double MaxSpeedKmh { get; private set; }

        public Road(ICity from, ICity to, double distanceKm, double maxSpeedKmh)
        {
            From = from ?? throw new ArgumentNullException(nameof(from));
            To = to ?? throw new ArgumentNullException(nameof(to));

            if (distanceKm <= 0)
                throw new ArgumentException("Distance must be positive.", nameof(distanceKm));
            if (maxSpeedKmh <= 0)
                throw new ArgumentException("Max speed must be positive.", nameof(maxSpeedKmh));

            DistanceKm = distanceKm;
            MaxSpeedKmh = maxSpeedKmh;
        }
    }
}
