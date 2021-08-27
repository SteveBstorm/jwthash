using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Consommation
{
    public class BaseRequester
    {
        private string BaseUrl { get; set; }

        public  string Token { get; set; }

        private HttpClient _client { get; set; }

        public BaseRequester(string baseAdress)
        {
            BaseUrl = baseAdress;

            _client = new HttpClient();
            _client.BaseAddress = new Uri(BaseUrl);
        }

        public void Login(string Email, string password)
        {
            string jsonBody = JsonConvert.SerializeObject(new { email = Email, password = password });
            HttpContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            HttpResponseMessage response = _client.PostAsync("user/login", content).Result;

            response.EnsureSuccessStatusCode();
            string jsonResponse = response.Content.ReadAsStringAsync().Result;
            Token = jsonResponse;
        }

        public TResult Get<TResult>(string url)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            using(HttpResponseMessage response = _client.GetAsync(url).Result)
            {
                if(response.StatusCode == HttpStatusCode.OK)
                {
                    string json = response.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<TResult>(json);
                }
                else
                {
                    throw new Exception("Requête invalide");
                }
            }
        }

        public void Post<T>(string url, T Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            string jsonBody = JsonConvert.SerializeObject(Entity);
            HttpContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            using(HttpResponseMessage response = _client.PostAsync(url, content).Result)
            {
                if(response.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception("Requête foireuse");
                }
            }
        }

        public void Delete(string url, Guid Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            using (HttpResponseMessage response = _client.DeleteAsync(url+"/"+Id.ToString()).Result)
            {
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception("Requête foireuse");
                }
            }
        }

        public void Update<T>(string url, T Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            string jsonBody = JsonConvert.SerializeObject(Entity);
            HttpContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            using (HttpResponseMessage response = _client.PutAsync(url, content).Result)
            {
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception("Requête foireuse");
                }
            }
        }

    }
}
