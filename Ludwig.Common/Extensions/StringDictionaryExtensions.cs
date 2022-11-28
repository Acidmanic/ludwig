using System.Collections.Generic;

namespace Ludwig.Common.Extensions
{
    public static class StringDictionaryExtensions
    {

        public static bool IsFullyValued(this Dictionary<string, string> dictionary)
        {
            foreach (var value in dictionary.Values)
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    return false;
                }
            }
            return true;
        }
    }
}