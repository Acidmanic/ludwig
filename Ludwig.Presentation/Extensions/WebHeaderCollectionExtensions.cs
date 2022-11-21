using System;
using System.Collections.Generic;
using System.Net;

namespace Ludwig.Presentation.Extensions
{
    public static class WebHeaderCollectionExtensions
    {
        

        public static List<string> HeadersByKey(this WebHeaderCollection headers,string key)
        {

            key = key.Trim().ToLower();
            
            var headerValues = new List<string>();
            
            foreach (string headerKey in headers.Keys)
            {
                if (headerKey.ToLower().Trim() == key)
                {
                    headerValues.Add(headers[key]);
                }
            }

            return headerValues;
        }

        public static List<string> RawSentCookies(this WebHeaderCollection headers)
        {
            return headers.HeadersByKey("Set-Cookie");
        }
        
        public static Dictionary<string, string> SentCookies(this WebHeaderCollection headers)
        {

            var cookies = new Dictionary<string, string>();

            var allSetCookieValues = headers.HeadersByKey("Set-Cookie");

            foreach (var setCookieValue in allSetCookieValues)
            {
                var keyValues = setCookieValue.Split(";");

                foreach (var keyValue in keyValues)
                {
                    var st = keyValue.IndexOf("=", StringComparison.Ordinal);
                    // separator exists and both key and value are not empty
                    if (st > 0 && st < keyValue.Length - 2) 
                    {
                        var key = keyValue.Substring(0, st);

                        var value = keyValue.Substring(st + 1, keyValue.Length - st - 1);
                        
                        cookies.Add(key,value);
                    }
                }
            }
            return cookies;
        }

    }
}