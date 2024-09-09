using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text.Json;

namespace YandexDiskAPI1
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            var oauthToken = "y0_AgAAAAAjNsyhAAxoAwAAAAEQYp3XAAB65Pto3Q9Hrr_OPnsmRHacs_5zug";

            await GetYandexDiskFiles(oauthToken);

            var yandexDiskRequest = new YandexDiskRequest();
            try
            {
                //await yandexDiskRequest.GetFiles(oauthToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
            }
        }

        private static async Task GetYandexDiskFiles(string token)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("OAuth", token);

                var response = await httpClient.GetAsync("https://cloud-api.yandex.net/v1/disk" +
                    "/resources?path=/");

                List<string> allFolders = new List<string>();

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    JObject json = JObject.Parse(content);
                    foreach (var item in json["_embedded"]["items"])
                    {
                        if (item["type"].ToString() == "dir")
                        {
                            string name = item["name"].ToString();
                            Console.WriteLine(name);
                            allFolders.Add(name);
                        }
                    }
                    Console.WriteLine();
                    foreach (var item in json["_embedded"]["items"])
                    {
                        if (item["type"].ToString() == "dir")
                        {
                            string name = item["path"].ToString();
                            Console.WriteLine(name);
                        }
                    }
                }
            }
        }
    }
}
