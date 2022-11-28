namespace ApiEmbassy.Extensions
{
    public static class StringExtensions
    {
        
        
        public static bool AreEqualAsKeys(this string value, string comparee)
        {

            if (value == null || comparee == null)
            {
                return false;
            }

            return value.ToLower().Trim() == comparee.ToLower().Trim();
        }

        public static string Slashend(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return "/";
            }

            if (value.EndsWith("/"))
            {
                return value;
            }

            return value + "/";
        }

    }
}