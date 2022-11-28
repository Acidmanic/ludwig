namespace Ludwig.Contracts.Models
{
    public class LoginField
    {
        
        public string Name { get; set; }

        public bool UiProtectedValue { get; set; }
        
        
        public string Description { get; set; }
        
        public string DisplayName { get; set; }
        

        public static LoginField Username { get; } = new LoginField
        {
            Name = "Username", UiProtectedValue = false,
            DisplayName = "Username"
        };
        
        public static LoginField Password { get; } = new LoginField { 
            Name = "Password",
            UiProtectedValue = true ,
            DisplayName = "Password"
        };
        
        public static LoginField Code { get; } = new LoginField {
            Name = "Code",
            UiProtectedValue = false ,
            DisplayName = "Code"
        };
        
        public static LoginField Otp { get; } = new LoginField { Name = "Otp",
            UiProtectedValue = false ,
            DisplayName = "One Time Password."
        };
    }
}