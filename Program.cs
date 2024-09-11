using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.IO;
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

            Console.WriteLine("Подождите");
            var manager=new YandexFileManager(oauthToken);

            await manager.Run();

            Console.WriteLine("Удаление завершено");
        }
    }
}
