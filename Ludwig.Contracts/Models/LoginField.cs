namespace Ludwig.Contracts.Models
{
    public class LoginField
    {
        public string Name { get; set; }

        public bool UiProtectedValue { get; set; }

        public static LoginField Username { get; } = new LoginField { Name = "Username", UiProtectedValue = false };
        
        public static LoginField Password { get; } = new LoginField { Name = "Password", UiProtectedValue = true };
        
        public static LoginField Code { get; } = new LoginField { Name = "Code", UiProtectedValue = false };
        
        public static LoginField Otp { get; } = new LoginField { Name = "Otp", UiProtectedValue = false };
    }
}