namespace Ludwig.Presentation.Authentication
{
    public class LudwigClaims
    {
        public const string LoginMethod = "LoginMethod";
        
        public const string UserScheme = "UserScheme";

        public class UserSchemes
        {
            public const string IssueManager = "IssueManager";
            public const string Administrator = "Administrator";
        }
    }
}