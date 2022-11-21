using System.Collections.Generic;

namespace Ludwig.Presentation.Models
{
    public class LoginParameters
    {
        public Dictionary<string,string> Parameters { get; set; }
        
        public string LoginMethodName { get; set; }
    }
}