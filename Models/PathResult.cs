using System.Collections.Generic;

namespace EastBulgariaPathFinderWPF.Models
{
    public class PathResult
    {
        public List<ICity> Path { get; set; }
        public double TotalTimeHours { get; set; }
        public double TotalDistanceKm { get; set; }

        public PathResult()
        {
            Path = new List<ICity>();
            TotalTimeHours = 0;
            TotalDistanceKm = 0;
        }
    }
}
