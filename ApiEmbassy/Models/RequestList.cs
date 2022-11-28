using System.Collections.Generic;

namespace ApiEmbassy.Models
{
    public class RequestList
    {
        public IEnumerable<RequestRecord> Requests { get; set; }
    }
}