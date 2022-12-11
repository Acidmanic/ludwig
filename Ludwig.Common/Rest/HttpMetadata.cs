using System;
using System.Collections.Generic;
using System.Text;
using Acidmanic.Utilities.Results;

namespace Ludwig.Common.Rest
{
    public class HttpMetadata
    {
        public static readonly HttpMetadata Empty = new HttpMetadata();

        public Dictionary<string, string> Cookies { get; set; } = new Dictionary<string, string>();

        public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();


        public string CookiesHeaderString()
        {
            var sb = new StringBuilder();

            var sep = "";

            foreach (var cookie in Cookies)
            {
                sb.Append(sep).Append(cookie.Key).Append("=").Append(cookie.Value);
                sep = ";";
            }

            var foundCookie = FindIgnoreCase(Headers, "cookies");

            if (foundCookie)
            {
                sb.Append(sep).Append(foundCookie.Value.Key).Append("=").Append(foundCookie.Value.Value);
            }
            
            return sb.ToString();
        }

        internal void MoveCookiesOutOfHeaders()
        {
            var foundCookie = FindIgnoreCase(Headers, "cookies");

            if (foundCookie)
            {
                AddCookies(foundCookie.Value.Value);

                Headers.Remove(foundCookie.Value.Key);
            }
        }

        public void AddCookies(string cookiesHeaderValue)
        {
            var cookies = cookiesHeaderValue.Split(";", StringSplitOptions.RemoveEmptyEntries);

            foreach (var cookie in cookies)
            {
                var st = cookie.IndexOf("=", StringComparison.Ordinal);

                if (st > -1)
                {
                    var key = cookie.Substring(0, st).Trim();

                    var value = cookie.Substring(st + 1, cookie.Length - st - 1);

                    if (Cookies.ContainsKey(key))
                    {
                        Cookies.Remove(key);
                    }
                    
                    Cookies.Add(key,value);
                }
            }
        }

        private Result<KeyValuePair<string, string>> FindIgnoreCase(Dictionary<string, string> collection, string key)
        {
            var lowKey = key.ToLower();
            
            foreach (var item in collection)
            {
                if (item.Key.ToLower() == lowKey)
                {
                    return item;
                }
            }
            
            return new Result<KeyValuePair<string, string>>(false,default);
        }
    }
}