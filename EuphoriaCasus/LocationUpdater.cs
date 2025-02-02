using EuphoriaCasus.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EuphoriaCasus
{
    public static class LocationUpdater
    {
        private static readonly OsrmService _osrmService = new();

        /// <summary>
        /// Update the distance and time needed to travel with the OSRM api.
        /// </summary>
        /// <param name="locations">The list of locations which are updated.</param>
        /// <returns>The updated locations list.</returns>
        public static async Task<List<Location>> UpdateDistancesAsync(List<Location> locations)
        {
            // Clear existing distances
            foreach (var location in locations)
            {
                location.Distances = [];
            }

            // Update distances for each location
            for (int i = 0; i < locations.Count; i++)
            {
                var startLocation = locations[i];

                for (int j = 0; j < locations.Count; j++)
                {
                    if (i == j) continue; // Skip the same location

                    var endLocation = locations[j];

                    // Get the distance and duration from OSRM
                    var (distance, duration) = await _osrmService.GetRouteAsync(
                        startLocation.Latitude,
                        startLocation.Longitude,
                        endLocation.Latitude,
                        endLocation.Longitude
                    );

                    // Create a new Distance object and add it to the start location's distances
                    startLocation.Distances.Add(new Distance
                    {
                        Id = endLocation.Id,
                        Km = distance / 1000, // Convert meters to kilometers
                        Seconds = duration
                    });
                }
            }

            return locations;
        }
    }
}