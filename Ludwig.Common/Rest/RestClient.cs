using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using HttpMethod = System.Net.Http.HttpMethod;

namespace Ludwig.Common.Rest
{
    public class RestClient
    {
        public string BaseUrl { get; set; }

        public HttpMetadata DefaultMetadata { get; set; } = HttpMetadata.Empty;

        public async Task<MetaObject<TResponse>> PostAsync<TResponse>(string uri, [MaybeNull] object data,
            HttpMetadata metadata = null)
        {
            if (metadata == null)
            {
                metadata = DefaultMetadata;
            }

            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(uri, UriKind.Relative),
            };

            if (data != null)
            {
                var requestJson = JsonConvert.SerializeObject(data, new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented,
                    ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new CamelCaseNamingStrategy()
                    }
                });

                request.Content = new StringContent(requestJson, Encoding.Default, "application/json");
            }

            WriteMetadata(request, metadata);

            var httpClient = new HttpClient()
            {
                BaseAddress = new Uri(BaseUrl, UriKind.Absolute)
            };

            var response = await httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var responseMetadata = ReadMetadata(response);

                var json = await response.Content.ReadAsStringAsync();

                TResponse responseData = default;
                try
                {
                    responseData = JsonConvert.DeserializeObject<TResponse>(json);
                }
                catch (Exception _)
                {
                }

                return new MetaObject<TResponse>
                {
                    Metadata = responseMetadata,
                    Value = responseData
                };
            }

            return new MetaObject<TResponse>();
        }

        private HttpMetadata ReadMetadata(HttpResponseMessage response)
        {
            var metadata = new HttpMetadata();

            foreach (var header in response.Headers)
            {
                var values = string.Join(';', header.Value);

                metadata.Headers.Add(header.Key, values);
            }

            metadata.MoveCookiesOutOfHeaders();

            return metadata;
        }

        private void WriteMetadata(HttpRequestMessage request, HttpMetadata metadata)
        {
            if (metadata == null)
            {
                return;
            }

            foreach (var header in metadata.Headers)
            {
                if (header.Key.ToLower() != "cookies")
                {
                    WriteHeader(request, header);
                }
            }

            var cookiesValue = metadata.CookiesHeaderString();

            if (!string.IsNullOrWhiteSpace(cookiesValue))
            {
                WriteHeader(request, new KeyValuePair<string, string>("Cookies", cookiesValue));
            }
        }


        private void WriteHeader(HttpRequestMessage request, KeyValuePair<string, string> header)
        {
            try
            {
                request.Headers.Add(header.Key, header.Value);
            }
            catch (Exception e)
            {
                if (request.Content != null && request.Content.Headers != null)
                {
                    try
                    {
                        request.Content.Headers.Add(header.Key, header.Value);
                    }
                    catch (Exception exception)
                    {
                        //Console.WriteLine(exception);
                    }
                }
            }
        }
    }
}