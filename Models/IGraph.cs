using System.Collections.Generic;

namespace EastBulgariaPathFinderWPF.Models
{
    public interface IGraph
    {
        void AddCity(ICity city);
        void AddRoad(IRoad road);
        IEnumerable<ICity> GetCities();
        IEnumerable<IRoad> GetRoadsFrom(ICity city);
        ICity GetCityByName(string name);
    }
}
