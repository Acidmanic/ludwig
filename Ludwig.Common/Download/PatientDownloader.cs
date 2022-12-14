using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Ludwig.Presentation.Download;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.LightWeight;
using Newtonsoft.Json;

namespace Ludwig.Common.Download
{
    public class PatientDownloader
    {
        public ILogger Logger { get; set; } = new LoggerAdapter(s => { });
        public string Proxy { get; set; } = null;

        public IRequestCookieCollection Cookies { get; set; } = null;

        public Dictionary<string, string> InDirectCookies { get; set; } = new Dictionary<string, string>();

        public Dictionary<string, string> Headers { get; } = new Dictionary<string, string>();


        private async Task<DownloadResult<T>> DownloadData<T>(string url, int timeout,
            Func<WebClient, string, Task<T>> pickData)
        {
            Logger.LogDebug("Downloading {Url}...", url);

            Exception exception = null;

            using (var net = new TimeoutWebClient(timeout))
            {
                if (Proxy != null)
                {
                    Logger.LogDebug("Using Proxy: {Proxy}", Proxy);

                    net.Proxy = new WebProxy(Proxy);
                }


                //net.Headers.Add("Connection","keep-alive");

                net.Headers.Add("Accept-Encoding", "gzip, deflate");

                net.Headers.Add("Accept-Language", "en-US,en;q=0.5");

                net.Headers.Add("Referer", "http://litbid.ir/");

                net.Headers.Add("User-Agent",
                    " Mozilla/5.0 (X11; Ubuntu; Linux x86_64; rv:105.0) Gecko/20100101 Firefox/105.0");

                net.InDirectCookies = InDirectCookies;

                foreach (var header in Headers)
                {
                    try
                    {
                        net.Headers.Add(header.Key, header.Value);
                    }
                    catch (Exception)
                    {
                    }
                }

                if (Cookies != null)
                {
                    net.Cookies = Cookies;
                }


                try
                {
                    var data = await pickData(net, url);

                    Logger.LogInformation("{Url} Has been downloaded Successfully.", url);

                    var result = new DownloadResult<T>().Succeed(data, new WebHeaderCollection());

                    foreach (var key in net.ResponseHeaders.AllKeys)
                    {
                        var value = net.ResponseHeaders[key];
                        result.ResponseHeaders.Add(key, value);
                    }

                    return result;
                }
                catch (Exception e)
                {
                    exception = e;
                }
            }

            return new DownloadResult<T>().Fail(exception);
        }


        public async Task<DownloadResult<byte[]>> DownloadFile(string url, int timeout)
        {
            return await DownloadData(url, timeout, (client, cUrl) => client.DownloadDataTaskAsync(cUrl));
        }

        public async Task<DownloadResult<string>> DownloadString(string url, int timeout)
        {
            return await DownloadData(url, timeout, (client, cUrl) => client.DownloadStringTaskAsync(cUrl));
        }

        public async Task<DownloadResult<string>> UploadString(string url, string data, int timeout)
        {
            return await DownloadData(url, timeout, (client, cUrl) => client.UploadStringTaskAsync(cUrl, data));
        }

        public async Task<DownloadResult<T>> DownloadObject<T>(string url, int timeout)
        {
            var result = await DownloadString(url, timeout);

            if (!result)
            {
                return new DownloadResult<T>().Fail(result.Exception);
            }

            try
            {
                var downloadedObject = JsonConvert.DeserializeObject<T>(result.Value);

                return new DownloadResult<T>().Succeed(downloadedObject, result.ResponseHeaders);
            }
            catch (Exception e)
            {
                return new DownloadResult<T>().Fail(e);
            }
        }

        private async Task<DownloadResult<T>> Retry<T>(string url, int timeout, int tries,
            Func<string, int, Task<DownloadResult<T>>> method)
        {
            DownloadResult<T> result = new DownloadResult<T>().Fail(new Exception());

            int count = 0;

            while (!result.Success && count < tries)
            {
                result = await method(url, timeout);

                if (result)
                {
                    return result;
                }

                Logger.LogDebug("Retrying {Count}", count);

                count += 1;
            }

            Logger.LogError("Unable to download {Url}, Exception: {Exception}", url, result.Exception);

            return result;
        }


        public async Task<DownloadResult<byte[]>> DownloadFile(string url, int timeout, int retries)
        {
            return await Retry(url, timeout, retries, DownloadFile);
        }

        public async Task<DownloadResult<string>> DownloadString(string url, int timeout, int retries)
        {
            return await Retry(url, timeout, retries, DownloadString);
        }

        public async Task<DownloadResult<T>> DownloadObject<T>(string url, int timeout, int retries)
        {
            return await Retry(url, timeout, retries, DownloadObject<T>);
        }

        public async Task<DownloadResult<string>> UploadString(string url, string data, int timeout, int retries)
        {
            return await Retry(url, timeout, retries, (cUrl, client) => UploadString(cUrl, data, timeout));
        }

        public async Task<DownloadResult<T>> UploadObject<T>(string url, object data, int timeout, int retries)
        {
            var json = JsonConvert.SerializeObject(data);

            var responseJson = await UploadString(url, json, timeout, retries);

            if (responseJson)
            {
                try
                {
                    var response = JsonConvert.DeserializeObject<T>(responseJson.Value);

                    return new DownloadResult<T>
                    {
                        Exception = null,
                        Primary = response,
                        ResponseHeaders = responseJson.ResponseHeaders
                    };
                }
                catch (Exception e)
                {
                    return new DownloadResult<T>()
                    {
                        Primary = default,
                        Secondary = e,
                        Success = false,
                    };
                }
            }
            else
            {
                return new DownloadResult<T>
                {
                    Exception = responseJson.Exception,
                    Primary = default,
                    Success = false,
                    ResponseHeaders = responseJson.ResponseHeaders
                };
            }
        }
    }
}