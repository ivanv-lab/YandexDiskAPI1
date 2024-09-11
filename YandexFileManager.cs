using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace YandexDiskAPI1
{
    public class YandexFileManager
    {
        public string token { get; set; }
        private List<string> foldersToDelete=new List<string>();
        public YandexFileManager(string token)
        {
            this.token = token;
        }
        public async Task Run()
        {
            foldersToDelete.Clear();
            await GetYandexDiskFiles(token);
            if (foldersToDelete.Count == 0)
            {
                return;
            }
            await DeleteFolders(token);
            if (foldersToDelete.Count > 0)
                await Run();
        }
        private async Task GetYandexDiskFiles(string token, string path = "/")
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
                }
            }
        }
        private async Task DeleteFolders(string token)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("OAuth", token);

                string url = "https://cloud-api.yandex.net/v1/disk" +
                    "/resources?path=";

                var tasks = foldersToDelete.Select(async p =>
                await httpClient.DeleteAsync(url + p)).ToList();
                await Task.WhenAll(tasks);
                //foreach (string path in foldersToDelete)
                //{
                //    string urlToFolder = url + path;
                //    var response = await httpClient.DeleteAsync(urlToFolder);
                //    if (response.IsSuccessStatusCode)
                //    {
                //        Console.WriteLine($"{path} успешно удален");
                //    }
                //}
            }
        }
    }
}
