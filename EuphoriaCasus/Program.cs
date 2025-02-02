using EuphoriaCasus.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace EuphoriaCasus
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var resourceDir = Path.Combine(Directory.GetCurrentDirectory(), "Resources");
            var locationPaths = Directory.GetFiles(resourceDir);
            var locations = new List<Location>();

            foreach (var locationString in locationPaths)
            {
                var serializer = new XmlSerializer(typeof(Location));
                Location location;
                using (XmlReader reader = XmlReader.Create(locationString))
                {
                    location = (Location)serializer.Deserialize(reader);
                }

                locations.Add(location);
            }

            Console.WriteLine("------------Best Route - Offline Data-------------");
            CalculateRoute([.. locations]);

            // Update location distances with live data from OSRM.
            Console.WriteLine("-------------Best Route - Live Data---------------");
            locations = (await LocationUpdater.UpdateDistancesAsync([.. locations]));
            CalculateRoute([.. locations]);
        }

        /// <summary>
        /// Calculate the Best route based on a set of locations.
        /// </summary>
        /// <param name="locations">The list of locations used to calculate the best route.</param>
        private static void CalculateRoute(List<Location> locations)
        {
            var start = locations.Single(l => l.Id == 1);
            locations.Remove(start);

            var bestRoute = RouteCalculator.CalculateBestRoute(start, locations);

            // Output the results
            Console.WriteLine();
            Console.WriteLine("--------------------Best Route--------------------");
            Console.WriteLine();
            Console.WriteLine("Best Route:");
            foreach (var location in bestRoute.Route)
            {
                Console.WriteLine(location.Name);
            }
            Console.WriteLine($"Shortest Distance: {bestRoute.ShortestDistance} km");
            Console.WriteLine($"Fastest Time: {bestRoute.FastestTime} seconds");
            Console.WriteLine();
        }
    }
}