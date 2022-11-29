using System;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Microsoft.AspNetCore.WebUtilities;

namespace Ludwig.Common.Utilities
{
    public class Pkce
    {
        public string State { get; set; }

        public string Verifier { get; set; }

        public string Challenge { get; set; }


        public static Pkce CreateNew()
        {
            return CreateNew(CreateState(), CreatVerifier());
        }

        public static Pkce CreateNewWithState(string state)
        {
            return CreateNew(state, CreatVerifier());
        }

        public static Pkce CreateNewWithVerifier(string verifier)
        {
            return CreateNew(CreateState(), verifier);
        }

        public static Pkce CreateNew(string state, string verifier)
        {
            var pkce = new Pkce
            {
                State = state,
                Verifier = verifier
            };

            pkce.Challenge = ChallengeOf(pkce.Verifier);

            return pkce;
        }

        private static string CreateState()
        {
            return "Request" + DateTime.Now.ToString("yyyyMMMddhhmm");
        }

        private static string ChallengeOf(string value)
        {
            var sha = SHA256.Create();

            var valueBytes = System.Text.Encoding.UTF8.GetBytes(value);

            var digested = sha.ComputeHash(valueBytes);

            var base64 = Base64UrlTextEncoder.Encode(digested);
            
            return base64;
        }


        private static readonly char[] VerifierChars = CreateVerifierChars();
        private static readonly Random Random = new Random(DateTime.Now.Millisecond);

        private static char[] CreateVerifierChars()
        {
            char[] chars = new char[55];

            var index = 0;

            chars[index] = 'a';

            while (chars[index] != 'z')
            {
                index++;

                chars[index] = (char)(chars[index - 1] + 1);
            }

            index++;

            chars[index] = 'A';

            while (chars[index] != 'Z')
            {
                index++;

                chars[index] = (char)(chars[index - 1] + 1);
            }

            index++;
            chars[index] = '-';
            index++;
            chars[index] = '~';
            index++;
            chars[index] = '.';

            return chars;
        }


        private static string CreatVerifier()
        {
            var maxCount = VerifierChars.Length;
            var count = 64;
            StringBuilder sb = new StringBuilder(count);

            for (int i = 0; i < count; i++)
            {
                var index = Random.Next(0, maxCount);
                sb.Append(VerifierChars[index]);
            }

            return sb.ToString();
        }
    }
}