using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

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

                        var value = item.Substring(st + 1, item.Length - st - 1).Trim();

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
                value = value.Substring(0, value.Length - 1);
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

        public static string CamelCase(this string value)
        {
            return Case(value, false);
        }

        public static string PascalCase(this string value)
        {
            return Case(value, true);
        }
        public static string TitleCase(this string name)
        {
            var nameChars = name.ToCharArray();

            var sb = new StringBuilder();

            bool lastUpper = false;

            foreach (var c in nameChars)
            {
                var upper = char.IsUpper(c);

                if (upper)
                {
                    if (lastUpper)
                    {
                        sb.Append(char.ToLower(c));
                    }
                    else
                    {
                        sb.Append(' ').Append(c);
                    }
                }
                else
                {
                    sb.Append(c);
                }

                lastUpper = upper;
            }

            return sb.ToString().Trim();
        }

        private static string Case(string value, bool upper)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return value;
            }

            var first = value.Substring(0, 1);
            var rest = value.Substring(1, value.Length - 1);
            first = upper ? first.ToUpper() : first.ToLower();
            return first + rest;
        }


        public static string ToSh256(this string value)
        {
            var sha = SHA256.Create();

            var valueBytes = System.Text.Encoding.UTF8.GetBytes(value);

            var digested = sha.ComputeHash(valueBytes);

            var hashed = Convert.ToBase64String(digested);

            return hashed;
        }
    }
}