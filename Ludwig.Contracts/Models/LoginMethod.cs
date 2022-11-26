using System.Collections.Generic;

namespace Ludwig.Contracts.Models
{
    public class LoginMethod
    {
        public List<LoginField> TextInputFields { get; set; }
        
        public List<LoginField> QueryFields { get; set; }

        public string Description { get; set; }

        public UiLink Link { get; set; }

        public string Name { get; set; }


        public static LoginMethod CreateUsernamePasswordLoginMethod(string name)
        {
            return new LoginMethod
            {
                TextInputFields = new List<LoginField>
                {
                    LoginField.Username,
                    LoginField.Password
                },
                Description = $"Please Enter your {name} Username and password to log in.",
                Name = "UsernamePassword",
                QueryFields = new List<LoginField>()
            };
        }
        
        public static LoginMethod CreateIndirectLoginMethod(string name,string loginUrl)
        {
            return new LoginMethod
            {
                TextInputFields = new List<LoginField>
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
                QueryFields = new List<LoginField>()
            };
        }
    }
}