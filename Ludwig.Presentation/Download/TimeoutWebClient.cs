using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Http;

namespace Ludwig.Presentation.Download
{
    public class TimeoutWebClient:WebClient
    {
        private int TimeOut { get; }

        public TimeoutWebClient(int timeout)
        {
            TimeOut = timeout;
        }

        public TimeoutWebClient()
        {
            TimeOut = 1000;
        }

        public IRequestCookieCollection Cookies { get; set; } = null;
        
        public Dictionary<string, string> InDirectCookies { get; set; } = new Dictionary<string, string>();
        
        
        protected override WebRequest GetWebRequest(Uri uri)
        {
            WebRequest w = base.GetWebRequest(uri);
            if (w != null)
            {
                w.Timeout = TimeOut;
                
                if(w is HttpWebRequest request)
                {
                    request.AllowAutoRedirect = true;
                    
                    request.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;
                    
                    request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;

                    if (request.CookieContainer == null)
                    {
                        request.CookieContainer = new CookieContainer();
                    }
                    if (Cookies != null)
                    {
                        foreach (var cookie in Cookies)
                        {
                            request.CookieContainer.Add(Cookie( uri,cookie.Key,cookie.Value));
                        }
                    }

                    foreach (var cookie in InDirectCookies)
                    {
                        request.CookieContainer.Add(Cookie(uri,cookie.Key,cookie.Value));
                    }
                }
                return w;
            }

            return null;
        }


        private Cookie Cookie(Uri uri, string name, string value)
        {
            return new Cookie(name, value, uri.LocalPath, uri.Host);

        }
        
    }
}