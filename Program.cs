using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Net.Http.Headers;

namespace YandexDiskAPI1
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            var authManager = new AuthManager();
            var oauthToken = await authManager.GetToken();

            // oauthToken = "y0_AgAAAAAjNsyhAAxoAwAAAAEQYp3XAAB65Pto3Q9Hrr_OPnsmRHacs_5zug";
            var stopwatch = Stopwatch.StartNew();
            Console.WriteLine("Подождите");
            await GetYandexDiskFiles(oauthToken);

            Console.WriteLine("ПАПКИ НА УДАЛЕНИЕ::::::::::::::::::::::::::::::::::");
            foreach(var path in foldersToDelete)
            {
                Console.WriteLine(path);
            }
            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedMilliseconds);
        }
        public static List<string>foldersToDelete = new List<string>();
        private static async Task GetYandexDiskFiles(string token,string path="/")
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("OAuth", token);

                var response = await httpClient.GetAsync("https://cloud-api.yandex.net/v1/disk" +
                    $"/resources?path={path}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    JObject json = JObject.Parse(content);

                    List<string> paths = new List<string>();

                    foreach (var item in json["_embedded"]["items"])
                    {
                        if (item["type"].ToString() == "dir")
                        {
                            paths.Add(item["path"].ToString());
                        }
                    }
                    if (json["_embedded"]["items"].Count() == 0)
                    {
                        foldersToDelete.Add(path);
                        return;
                    }
                    var tasks = paths.Select(async p => await
                    GetYandexDiskFiles(token, p)).ToList();
                    await Task.WhenAll(tasks);
                    //foreach (var pat in paths)
                    //{
                        //await GetYandexDiskFiles(token, pat);

                        //Parallel.Invoke(async () =>
                        //await GetYandexDiskFiles(token, pat));
                    //}
                }
            }
        }
    }
}
