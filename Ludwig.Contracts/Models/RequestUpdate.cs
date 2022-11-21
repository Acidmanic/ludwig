using System.Collections.Generic;

namespace Ludwig.Contracts.Models
{
    public class RequestUpdate
    {
        public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();


        public Dictionary<string, string> Cookies { get; set; } = new Dictionary<string, string>();


    }
}