using System.Collections.Generic;

namespace Ludwig.Contracts.Models
{
    public class RequestUpdate
    {
        public Dictionary<string,string> Headers { get; set; }
        
        
        public Dictionary<string,string> Cookies { get; set; }
        
        
    }
}