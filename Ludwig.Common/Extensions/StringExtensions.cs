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


        public static string Slashend(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return "/";
            }

            if (!value.EndsWith("/"))
            {
                value += "/";
            }

            return value;
        }
        
        public static string UnSlashend(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return "";
            }

            if (value.EndsWith("/"))
            {
                value =value.Substring(0,value.Length-1);
            }

            return value;
        }

        public static bool HasValue(this string first, params string[] others)
        {
            if (string.IsNullOrWhiteSpace(first))
            {
                return false;
            }

            foreach (var other in others)
            {
                if (string.IsNullOrWhiteSpace(other))
                {
                    return false;
                }
            }

            return true;
        }
    }
}