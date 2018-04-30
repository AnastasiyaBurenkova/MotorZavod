

using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AbstractShopView
{
    public static class APIClient
    {
        private static HttpClient client = new HttpClient();

        public static void Connect()
        {
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["IPAddress"]);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public static Task<HttpResponseMessage> GetRequest(string requestUrl)
        {
            return client.GetAsync(requestUrl);
        }

        public static Task<HttpResponseMessage> PostRequest<T>(string requestUrl, T model)
        {
            return client.PostAsJsonAsync(requestUrl, model);
        }

        public static T GetElement<T>(Task<HttpResponseMessage> response)
        {
            return response.Result.Content.ReadAsAsync<T>().Result;
        }

        public static string GetError(Task<HttpResponseMessage> response)
        {
            return response.Result.Content.ReadAsStringAsync().Result;
        }
    }
}