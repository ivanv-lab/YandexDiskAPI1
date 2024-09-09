using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace YandexDiskAPI1
{
    public class YandexDiskRequest
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://cloud-api.yandex.net/v1/disk";

        public YandexDiskRequest()
        {
            _httpClient = new HttpClient();
        }

        public async Task GetFiles(string oauthToken)
        {
            var url = $"{BaseUrl}/resources?path=/";

            AddAuthorizationHeader(_httpClient, oauthToken);

            //var content = new StringContent(JsonConvert.SerializeObject(new { path }), System.Text.Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                //return await response.Content.ReadAsStringAsync();
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var diskResponse = JsonConvert
                    .DeserializeObject<DiskResponse>(jsonResponse);
                foreach (var item in diskResponse.Items)
                {
                    string sizeInfo = item.Type == "dir" ? "Папка" : $"{item.Size}";
                    Console.WriteLine($"{item.Name}: {sizeInfo}, " +
                        $"Последнее изменение: {item.Modified:yyyy-MM-dd HH-mm-ss} " +
                        $"Путь: {item.Path}");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Ошибка при получении списка файлов: {ex.Message}");
                throw;
            }
        }

        private void AddAuthorizationHeader(HttpClient client, string oauthToken)
        {
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.
                AuthenticationHeaderValue("OAuth", oauthToken);
        }

        public class DiskFile
        {
            public string Name { get; set; }
            public long Size { get; set; }
            public DateTime Modified { get; set; }
            public string Type {  get; set; }
            public string Path {  get; set; }
        }
        public class DiskResponse
        {
            public List<DiskFile> Items { get; set; }
        }
    }
}
