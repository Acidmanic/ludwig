using System.Collections.Generic;

namespace Ludwig.Contracts.Models
{

    public class RequestUpdate
    {

        public static readonly int RequestUpdateTypeHeader = 0;
        public static readonly int RequestUpdateTypeCookie = 1;
        
        public string Key { get; set; }
        
        public string Value { get; set; }
        
        public int Type { get; set; }

    }


    public static class RequestUpdateExtensions
    {

        public static bool IsHeader(this RequestUpdate update)
        {
            return update.Type == RequestUpdate.RequestUpdateTypeHeader;
        }
        
        public static bool IsCookie(this RequestUpdate update)
        {
            return update.Type == RequestUpdate.RequestUpdateTypeCookie;
        }
    }
}