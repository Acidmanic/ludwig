using System.Collections.Generic;

namespace Ludwig.Contracts.Extensions
{
    public static class LoginParameterDictionaryExtensions
    {



        public static string Read(this Dictionary<string, string> parameters,string key)
        {

            if (string.IsNullOrWhiteSpace(key))
            {
                return null;
            }
            key = key.Trim().ToLower();
            
            foreach (var itemKey in parameters.Keys)
            {
                if (itemKey.ToLower().Trim() == key)
                {
                    return parameters[itemKey];
                }
            }

            return null;
        }
    }
}