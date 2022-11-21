using System.Collections.Generic;

namespace Ludwig.Presentation.Download
{
    public class Response<T>
    {
        public T Data { get; set; }
        
        public Dictionary<string,string> Headers { get; set; } 
        public Dictionary<string,string> Cookies { get; set; } 

    }
}