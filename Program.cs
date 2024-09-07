namespace YandexDiskAPI1
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            var authManaher=new AuthManager();
            var accessToken=await authManaher.GetAccessTokenAsync();

            var diskApi=new DiskFileManager(accessToken);

            var files = await diskApi.ListFilesResponse("");
            foreach (var file in files.Embedded.Items)
            {
                Console.WriteLine(file.Name);
            }
        }
    }
}
