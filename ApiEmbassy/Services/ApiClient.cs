using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using EnTier.Results;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;

namespace ApiEmbassy.Services
{
    public class ApiClient
    {


        public Result<T> Get<T>(string baseAddress,string uri)
        {

            HttpClient client;
            string response;
            try
            {
                 client = new HttpClient { BaseAddress = new Uri(baseAddress, UriKind.Absolute) };
                 
                 response = client.GetStringAsync(uri).Result;
            }
            catch (Exception e)
            {
                return new Result<T>().FailAndDefaultValue();
            }

            

            if (!string.IsNullOrWhiteSpace(response))
            {
                try
                {
                    var value = JsonConvert.DeserializeObject<T>(response);

                    return new Result<T>(true, value);
                }
                catch (Exception _) { }
            }

            return new Result<T>().FailAndDefaultValue();
        }
        
        public Result Post(object payload,string baseAddress,string uri)
        {
            var client = new HttpClient { BaseAddress = new Uri(baseAddress, UriKind.Absolute) };

            var json = JsonConvert.SerializeObject(payload);

            var content = new StringContent(json,Encoding.Default,"application/json");

            try
            {
                var response = client.PostAsync(uri, content).Result;

                return new Result{Success = response.IsSuccessStatusCode};
            }
            catch (Exception _)
            {
                return new Result { Success = false };
            }
        }
    }
}

