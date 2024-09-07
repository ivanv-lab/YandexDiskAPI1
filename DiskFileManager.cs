using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using YandexDisk.Client;
using YandexDisk.Client.Protocol;

namespace YandexDiskAPI1
{
    public class DiskFileManager
    {
        private readonly HttpClient _httpClient;
        private readonly string _accessToken;

        public DiskFileManager(string accessToken)
        {
            _accessToken = accessToken;
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders
                .Authorization =
                new AuthenticationHeaderValue
                ("OAuth", _accessToken);
        }

        public async Task<ListFilesResponse> ListFilesResponse(string path = "")
        {
            var response = await _httpClient.GetAsync
                ($"https://cloud-api.yandex.net/v1" +
                $"/disk/resources" +
                $"?path={Uri.EscapeDataString(path)}");
            response.EnsureSuccessStatusCode();
            var content=await response.Content.ReadAsStringAsync();
            return Newtonsoft.Json.JsonConvert
                .DeserializeObject<ListFilesResponse>
                (content);
        }
    }

    public class ListFilesResponse
    {
        public Resource Embedded { get; set; }
    }
    public class Resource {
    public IEnumerable<Item> Items { get; set; }
    }
    public class Item {
        public string Name { get; set; }    
        public string Path {  get; set; }
    }
    public class  UploadedFileResponse
    {
        public string Path { get; set; }
    }

}
