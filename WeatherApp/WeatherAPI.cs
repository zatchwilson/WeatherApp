using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using static System.Net.WebRequestMethods;
using System.Configuration;
using System.Collections.Specialized;

namespace WeatherApp
{
    public class WeatherAPI
    {
        public WeatherData StartClient(string zipCode)
        {
            string apiKey = ConfigurationManager.AppSettings.Get("APIKey");
            string rnUri = "https://weatherapi-com.p.rapidapi.com/current.json?q=" + zipCode;
            WeatherData data = null;
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(rnUri),
                Headers =
                {
                    { "X-RapidAPI-Key", apiKey },
                    { "X-RapidAPI-Host", "weatherapi-com.p.rapidapi.com" },
                },
            };

            data = TestClient(client, request, data).Result;

            return data;
        }

        //Ensures the request (zip code) was valid
        static async Task<WeatherData> TestClient(HttpClient client, HttpRequestMessage request, WeatherData data)
        {
            try
            {
                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadAsStringAsync();
                    data = JsonConvert.DeserializeObject<WeatherData>(body);
                }
            }

            catch
            {
                Console.WriteLine("The information that was input is not valid.");
            }

            return data;
        }

        /// <summary>
        /// Converts the JSON into a WeatherData object
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public WeatherData ParseJson(string json)
        {
            WeatherData data = JsonConvert.DeserializeObject<WeatherData>(json);
            return data;
        }
    }
}


