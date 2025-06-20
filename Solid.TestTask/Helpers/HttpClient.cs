namespace Solid.TestTask.Helpers
{
    public static class HttpClient
    {
        public static byte[] DownloadBytes(Uri url)
        {
            using (System.Net.Http.HttpClient client = new System.Net.Http.HttpClient())
            {
                return client.GetByteArrayAsync(url).GetAwaiter().GetResult();
            }
        }
    }
}
