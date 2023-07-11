using System.Net;
using System.Net.Sockets;

namespace WeatherApp;

public partial class MainPage : ContentPage
{
    WeatherData data = null;
    WeatherAPI api = new WeatherAPI();
    bool zipEntered = false;  

    public MainPage()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Runs when Retrieve button is clicked
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnRetrieveClicked(object sender, EventArgs e)
    {
        zipEntered = true;
        Task.Run(GetData);
    }

    /// <summary>
    /// Runs if the current location button was pressed
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnCurrentClicked(object sender, EventArgs e)
    {
        zipEntered = false;
        Task.Run(GetData);
    }


    /// <summary>
    /// Gets the current temperature from the Weather Data
    /// </summary>
    /// <returns></returns>
    private Task GetData()
    {
        //If using the zip code button, use that
        if (zipEntered)
        {
            data = api.StartClient(ZipEntry.Text);
        }
        //If using 'current location', retrieve their IP and use that for the weather data
        else
        {
            var externalIP = GetExternalIpAddress();
            GetExternalIpAddress().Wait();
            data = api.StartClient(externalIP.Result.ToString());
        }

        //Checks if an invalid ZipCode was entered
        if (data == null)
        {
            Application.Current.MainPage.Dispatcher.Dispatch(() => WeatherText.Text = "Invalid Zip Code Entered.");
            return null;
        }

        //Sets the UI with the temperature, location, condition, and sets the icon
        Application.Current.MainPage.Dispatcher.Dispatch(() => WeatherText.Text = "The Current Temperature is " + data.current.Temp_f + " degrees.");
        Application.Current.MainPage.Dispatcher.Dispatch(() => Location.Text = data.location.Name);
        Application.Current.MainPage.Dispatcher.Dispatch(() => WeatherCondition.Text = data.current.Condition.Text);
        setIcon(data.current.Condition.Code);
        
        return Task.CompletedTask;
    }

    /// <summary>
    /// Sets the current Icon for the display 
    /// </summary>
    /// <param name="icon"></param>
    private void setIcon(int icon)
    {
        if (icon == 1000)
        {
            Application.Current.MainPage.Dispatcher.Dispatch(() => WeatherIcon.Source = "sunny.png");
        }
        else if (icon == 1003)
        {
            Application.Current.MainPage.Dispatcher.Dispatch(() => WeatherIcon.Source = "partly_cloudy.png");
        }
        else if (icon == 1006 || icon == 1009)
        {
            Application.Current.MainPage.Dispatcher.Dispatch(() => WeatherIcon.Source = "cloudy.png");
        }
        else if (icon == 1030 || icon == 1135 || icon == 1147)
        {
            Application.Current.MainPage.Dispatcher.Dispatch(() => WeatherIcon.Source = "foggy.png");
        }
        else if (icon == 1063 || icon == 1150 || icon == 1153 || icon == 1180 || icon == 1183 || icon == 1186 || icon == 1189 || icon == 1192 || icon == 1195 || icon == 1240 || icon == 1243 || icon == 1246)
        {
            Application.Current.MainPage.Dispatcher.Dispatch(() => WeatherIcon.Source = "rainy.png");
        }
        else if (icon == 1066 || icon == 1069 || icon == 1072 || icon == 1114 || icon == 1117 || icon == 1168 || icon == 1171 || icon == 1198 || icon == 1201 || icon == 1204 || icon == 1210 || icon == 1213 || icon == 1216 || icon == 1219 || icon == 1222 || icon == 1225 || icon == 1237 || icon == 1249 || icon == 1252 || icon == 1255 || icon == 1258 || icon == 1261)
        {
            Application.Current.MainPage.Dispatcher.Dispatch(() => WeatherIcon.Source = "snowing.png");
        }
        else if (icon == 1087 || icon == 1273 || icon == 1276 || icon == 1279 || icon == 1282)
        {
            Application.Current.MainPage.Dispatcher.Dispatch(() => WeatherIcon.Source = "stormy.png");
        }
    }

    /// <summary>
    /// Retreives the user's public facing IP from "icanhazip.com"
    /// </summary>
    /// <returns></returns>
    public static async Task<IPAddress?> GetExternalIpAddress()
    {
        var externalIpString = (await new HttpClient().GetStringAsync("http://icanhazip.com"))
            .Replace("\\r\\n", "").Replace("\\n", "").Trim();
        if (!IPAddress.TryParse(externalIpString, out var ipAddress)) return null;
        return ipAddress;
    }
}

