using Newtonsoft.Json;
using System.Text;

namespace MVCWebApp.Services
{
    public interface IApiService
    {
        Task<T> CallApiAsync<T>(string url, HttpMethod method, object data = null);
    }

    public class ApiService(IHttpClientFactory httpClientFactory) : IApiService
    {
        public async Task<T> CallApiAsync<T>(string url, HttpMethod method, object data = null)
        {
            var client = httpClientFactory.CreateClient();

            try
            {
                using var request = new HttpRequestMessage(method, url);

                if (data != null)
                {
                    string jsonData = JsonConvert.SerializeObject(data);
                    request.Content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                }

                using var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                string responseContent = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(responseContent);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"API call failed: {ex.Message}");
                return default;
            }
        }
    }
}
