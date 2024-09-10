using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YandexDiskAPI1
{
    public class AuthManager
    {
        private const string ClientId = "829dcf0d2cc248159d4477d3dcc56f06";
        private const string ClientSecret = "800a745f6f2642b8840492ffad2fe14d";

        public async Task<string> GetToken()
        {
            var authUrl = $"https://oauth.yandex.ru/authorize?response_type=code&client_id=" +
                $"{ClientId}";
            Console.WriteLine($"Перейдите по ссылке и введите код подтверждения:");
            Console.WriteLine(authUrl);

            var code=Console.ReadLine();

            using var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, $"https://oauth.yandex.ru/token")
            {
                Content = new FormUrlEncodedContent(new[]
                {
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
                new KeyValuePair<string, string>("code", code),
                new KeyValuePair<string, string>("client_id", ClientId),
                new KeyValuePair<string, string>("client_secret", ClientSecret)
            })
            };

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var responseData = await response.Content.ReadAsStringAsync();
            dynamic data = Newtonsoft.Json.Linq.JObject.Parse(responseData);

            return data.access_token;
        }
    }
}
