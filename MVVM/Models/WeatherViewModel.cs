using PropertyChanged;
using System.Text.Json;
using System.Windows.Input;

namespace Weather.MVVM.Models;

[AddINotifyPropertyChangedInterface]
public class WeatherViewModel
{
    public WeatherData WeatherData { get; set; }
    public string PlaceName { get; set; }
    public DateTime dateTime { get; set; } = DateTime.Now;
    private HttpClient client;
    public WeatherViewModel()
    {
        client = new HttpClient();
    }
    public ICommand SearchCommand => new Command(async (searchText) =>
    {
        PlaceName = searchText.ToString();
        var location = await GetCoOrdinatesAsync(searchText.ToString());
        await GetWeather(location);
    });
    private async Task<Location> GetCoOrdinatesAsync(string address)
    {
        IEnumerable<Location> locations = await Geocoding.Default.GetLocationsAsync(address);
        Location location = locations?.FirstOrDefault();
        if (location == null)
        {
            return null;
        }
        return location;
    }

    private async Task GetWeather(Location location)
    {
        var url = $"https://api.open-meteo.com/v1/forecast?latitude={location.Latitude}&longitude={location.Longitude}&hourly=temperature_2m&daily=weathercode,temperature_2m_max,temperature_2m_min&current_weather=true&timezone=America%2FAnchorage";
        var response = await client.GetAsync(url);
        try
        {
            if (response.IsSuccessStatusCode)
            {
                using (var resposeStream = await response.Content.ReadAsStreamAsync())
                {
                    var data = await JsonSerializer.DeserializeAsync<WeatherData>(resposeStream);
                    WeatherData = data;
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.InnerException);
            throw;
        }

    }
}
