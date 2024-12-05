namespace EastBulgariaPathFinderWPF.Models
{
    public interface IRoad
    {
        ICity From { get; }
        ICity To { get; }
        double DistanceKm { get; }
        double MaxSpeedKmh { get; }
    }
}
