using System.Collections.Generic;

namespace Ludwig.Contracts.Models
{
    public class LoginMethod
    {
        public List<LoginField> Fields { get; set; } = new List<LoginField>();

        public List<LoginQuery> Queries { get; set; } = new List<LoginQuery>();

        public List<ConfigurationRequirement> ConfigurationRequirements { get; set; } =
            new List<ConfigurationRequirement>();

        public string Description { get; set; }

        public UiLink Link { get; set; }

        public string Name { get; set; }


        public static LoginMethod CreateUsernamePasswordLoginMethod(string name)
        {
            return new LoginMethod
            {
                Fields = new List<LoginField>
                {
                    LoginField.Username,
                    LoginField.Password
                },
                Description = $"Please Enter your {name} Username and password to log in.",
                Name = "UsernamePassword",
                Queries = new List<LoginQuery>()
            };
        }
        
        public static LoginMethod CreateIndirectLoginMethod(string name,string loginUrl)
        {
            return new LoginMethod
            {
                Fields = new List<LoginField>
                {
                    LoginField.Username,
                    LoginField.Password
                },
                Description = $"Please Login into your {name} account first, to be able to use the application.",
                Name = "Indirect",
                Link = new UiLink
                {
                    Title = $"{name} Login Page.",
                    Url = loginUrl
                },
                Queries = new List<LoginQuery>()
            };
        }
    }
}