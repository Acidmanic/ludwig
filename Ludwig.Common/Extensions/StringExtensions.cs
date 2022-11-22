using System;
using System.Collections.Generic;

namespace Ludwig.Common.Extensions
{
    public static class StringExtensions
    {
        public static Dictionary<string, string> LoadHeaders(this string headersString)
        {
            var headers = new Dictionary<string, string>();
            
            if (!string.IsNullOrEmpty(headersString))
            {
                var items = headersString.Split("\n", StringSplitOptions.RemoveEmptyEntries);

                foreach (var item in items)
                {
                    var st = item.IndexOf(":", StringComparison.Ordinal);

                    if (st > -1)
                    {
                        var name = item.Substring(0, st).Trim();

                        var value = item.Substring(st + 1, item.Length - st -1).Trim();

                        if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(value))
                        {
                            headers.Add(name, value);
                        }
                    }
                }
            }

            return headers;
        }


        public static Dictionary<string, string> LoadCookies(this string cookiesString)
        {
            var result = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(cookiesString))
            {
                var items = cookiesString.Split(";", StringSplitOptions.RemoveEmptyEntries);

                foreach (var item in items)
                {
                    var st = item.IndexOf("=", StringComparison.Ordinal);

                    if (st > -1)
                    {
                        var name = item.Substring(0, st).Trim();

                        var value = item.Substring(st + 1, item.Length - st - 1).Trim();

                        if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(value))
                        {
                            result.Add(name, value);
                        }
                    }
                }
            }

            return result;
        }
    }
}