using System.Collections.Generic;

namespace EuphoriaCasus.Models
{
    public class BestRoute(IList<Location> route, double shortestDistance, int fastestTime)
    {
        public IList<Location> Route { get; private set; } = route;

        public double ShortestDistance { get; private set; } = shortestDistance;

        public int FastestTime { get; private set; } = fastestTime;
    }
}