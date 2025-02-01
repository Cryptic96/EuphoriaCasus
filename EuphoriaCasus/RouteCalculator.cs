using EuphoriaCasus.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EuphoriaCasus
{
    public class RouteCalculator
    {
        public static BestRoute CalculateBestRoute(Location startLocation, List<Location> locations)
        {
            var otherLocations = locations.ToList();
            var bestRoute = new List<Location>();
            double shortestDistance = double.MaxValue;
            int fastestTime = int.MaxValue;

            // Generate all permutations of the other locations
            var permutations = GetPermutations(otherLocations, otherLocations.Count);

            foreach (var perm in permutations)
            {
                // Calculate the total distance and time for this route
                double totalDistance = 0;
                int totalTime = 0;

                // Start from the start location
                var currentLocation = startLocation;

                // Visit each location in the permutation
                foreach (var location in perm)
                {
                    var distanceInfo = currentLocation.Distances.First(d => d.Id == location.Id);
                    totalDistance += distanceInfo.Km;
                    totalTime += distanceInfo.Seconds;
                    currentLocation = location;
                }

                // Return to the start location
                var returnDistanceInfo = currentLocation.Distances.First(d => d.Id == startLocation.Id);
                totalDistance += returnDistanceInfo.Km;
                totalTime += returnDistanceInfo.Seconds;

                // Check if this route is better than the best found so far
                if (totalDistance < shortestDistance || (totalDistance == shortestDistance && totalTime < fastestTime))
                {
                    shortestDistance = totalDistance;
                    fastestTime = totalTime;
                    bestRoute = [startLocation, .. perm, startLocation];
                }
            }

            return new(bestRoute, shortestDistance, fastestTime);
        }

        /// <summary>
        /// The total number of routes can be calculated as:
        /// [ {Total Routes} = (n - 1)! ]
        /// Where(n ) is the total number of locations. For 10 locations:
        /// [ {Total Routes} = (10 - 1)! = 9! = 362880 ]
        /// </summary>
        private static List<List<T>> GetPermutations<T>(List<T> list, int length)
        {
            if (length == 1)
            {
                return list.Select(t => new List<T> { t }).ToList();
            }

            return GetPermutations(list, length - 1)
                .SelectMany(t => list.Where(e => !t.Contains(e)),
                    (t1, t2) =>
                    {
                        var newList = new List<T>(t1) { t2 }; // Create a new list and add the new element
                        return newList;
                    })
                .ToList();
        }
    }
}