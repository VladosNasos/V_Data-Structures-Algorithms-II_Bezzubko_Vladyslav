namespace EastBulgariaPathFinderWPF.Models
{
    public interface IPathFinder
    {
        PathResult FindQuickestPath(ICity start, ICity end);
    }
}
