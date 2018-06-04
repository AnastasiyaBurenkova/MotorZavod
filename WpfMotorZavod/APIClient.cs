using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace WpfMotorZavod
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

        private static async Task<HttpResponseMessage> GetRequest(string requestUrl)
        {
            return await client.GetAsync(requestUrl);
        }

        private static async Task<HttpResponseMessage> PostRequest<T>(string requestUrl, T model)
        {
            return await client.PostAsJsonAsync(requestUrl, model);
        }

        public static async Task<T> GetRequestData<T>(string requestUrl)
        {
            HttpResponseMessage response = Task.Run(() => GetRequest(requestUrl)).Result;
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<T>();
            }
            else
            {
                string error = response.Content.ReadAsStringAsync().Result;
                throw new Exception(error);
            }
        }

        public static void PostRequestData<T>(string requestUrl, T model)
        {
            HttpResponseMessage response = Task.Run(() => PostRequest(requestUrl, model)).Result;
            if (!response.IsSuccessStatusCode)
            {
                string error = response.Content.ReadAsStringAsync().Result;
                var errorMessage = JsonConvert.DeserializeObject<HttpErrorMessage>(error);
                throw new Exception(errorMessage.Message + " " + (errorMessage.MessageDetail ?? "") +
                    " " + (errorMessage.ExceptionMessage ?? ""));
            }
        }

        public static async Task<U> PostRequestData<T, U>(string requestUrl, T model)
        {
            HttpResponseMessage response = Task.Run(() => PostRequest(requestUrl, model)).Result;
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<U>();
            }
            else
            {
                string error = response.Content.ReadAsStringAsync().Result;
                var errorMessage = JsonConvert.DeserializeObject<HttpErrorMessage>(error);
                throw new Exception(errorMessage.Message + " " + errorMessage.MessageDetail ?? "" +
                    " " + errorMessage.ExceptionMessage ?? "");
            }
        }
    }
}