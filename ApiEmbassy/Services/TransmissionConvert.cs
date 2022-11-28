using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ApiEmbassy.Extensions;
using ApiEmbassy.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace ApiEmbassy.Services
{
    public static class TransmissionConvert
    {
        public static HttpRequestMessage ToRequestMessage(RequestRecord record)
        {
            
            var query = record.Queries.ToQueryString();
            
            var request = new HttpRequestMessage()
            {
                Method = record.Method,
                RequestUri = new Uri(record.RequestUri + query, UriKind.Relative),
            };

            request.Content = new StreamContent(new MemoryStream(record.Content));

            foreach (var header in record.Headers)
            {
                try
                {
                    request.Headers.Add(header.Key, header.Value);
                }
                catch (Exception e)
                {
                    try
                    {
                        request.Content.Headers.Add(header.Key, header.Value);
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception);
                    }
                }
            }

            return request;
        }


        public static Task<RequestRecord> ToRequestRecord(HttpContext context)
        {
            return ToRequestRecord(context.Request);
        }

        public static async Task<RequestRecord> ToRequestRecord(HttpRequest httpRequest)
        {
            var request = new RequestRecord
            {
                Method = httpRequest.Method.ToHttpMethod(),
                Content = await httpRequest.Body.ReadAllBytes(),
                Headers = new Dictionary<string, List<string>>(),
                RequestUri = httpRequest.Path.ToString(),
                Queries = new Dictionary<string, List<string>>()
            };

            foreach (var header in httpRequest.Headers)
            {
                request.Headers.Add(header.Key, new List<string>(header.Value));
            }

            foreach (var query in httpRequest.Query)
            {
                request.Queries.Add(query.Key,new List<string>(query.Value));
            }

            return request;
        }


        public static async Task IntoHttpContext(HttpContext context, ResponseCarrier response)
        {
            foreach (var header in response.Headers)
            {
                try
                {
                    context.Response.Headers.Add(header.Key, new StringValues(header.Value.ToString()));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            context.Response.StatusCode = response.StatusCode;

            await context.Response.Body.WriteAsync(response.Content);
        }

        public static async Task IntoHttpContext(HttpContext context, HttpResponseMessage response)
        {
            foreach (var header in response.Headers)
            {
                try
                {
                    context.Response.Headers.Add(header.Key, new StringValues(header.Value.ToString()));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            var responseContent = await response.Content.ReadAsByteArrayAsync();

            await context.Response.Body.WriteAsync(responseContent);
        }

        public static async Task<ResponseCarrier> ToResponseCarrier(HttpResponseMessage responseMessage)
        {
            var response = new ResponseCarrier
            {
                StatusCode = (int)responseMessage.StatusCode,
                Content = await responseMessage.Content.ReadAsByteArrayAsync(),
                Headers = new Dictionary<string, List<string>>()
            };

            foreach (var header in responseMessage.Headers)
            {
                var values = new List<string>();

                values.AddRange(header.Value);

                response.Headers.Add(header.Key, values);
            }

            return response;
        }
    }
}