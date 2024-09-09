using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;

namespace YandexDiskAPI1
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            var ouathToken = "y0_AgAAAAAjNsyhAAxoAwAAAAEQYp3XAAB65Pto3Q9Hrr_OPnsmRHacs_5zug";

            await GetYandexDiskFiles(ouathToken);
        }

        private static async Task GetYandexDiskFiles(string token)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("OAuth", token);

                var response = await httpClient.GetAsync("https://cloud-api.yandex.net/v1/disk/resources/files");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    JObject json = JObject.Parse(content);
                    foreach (var item in json["items"])
                    {
                        string name = item["name"].ToString();
                        Console.WriteLine(name);
                    }
                }
                else
                {
                    Console.WriteLine($"Ошибка: {response.StatusCode}");
                    string errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(errorContent);
                }
            }
        }
    }
}
