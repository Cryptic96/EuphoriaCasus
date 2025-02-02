using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace EuphoriaCasus.Services
{
    public class OsrmService
    {
        private readonly HttpClient _httpClient;

        public OsrmService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<(double distance, int duration)> GetRouteAsync(double startLat, double startLon, double endLat, double endLon)
        {
            // Convert latitude and longitude to string with '.' as the decimal separator
            string startLatStr = startLat.ToString("G", System.Globalization.CultureInfo.InvariantCulture);
            string startLonStr = startLon.ToString("G", System.Globalization.CultureInfo.InvariantCulture);
            string endLatStr = endLat.ToString("G", System.Globalization.CultureInfo.InvariantCulture);
            string endLonStr = endLon.ToString("G", System.Globalization.CultureInfo.InvariantCulture);

            // OSRM API endpoint for route calculation
            string url = $"http://router.project-osrm.org/route/v1/driving/{startLonStr},{startLatStr};{endLonStr},{endLatStr}?overview=false";

            // Make the request to the OSRM API
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            // Parse the JSON response
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(jsonResponse);

            // Extract distance and duration
            var route = json["routes"][0];
            double distance = route["distance"].Value<double>(); // Distance in meters
            int duration = route["duration"].Value<int>(); // Duration in seconds

            return (distance, duration);
        }
    }
}