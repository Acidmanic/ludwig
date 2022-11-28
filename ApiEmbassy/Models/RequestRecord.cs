using System;
using System.Collections.Generic;
using System.Net.Http;
using Acidmanic.Utilities.Reflection.Attributes;

namespace ApiEmbassy.Models
{
    public class RequestRecord
    {
        [AutoValuedMember]
        [UniqueMember]
        public long Id { get; set; }
        
        public HttpMethod Method { get; set; }
        
        public string RequestUri { get; set; }
        
        public byte[] Content { get; set; }
        
        public Dictionary<string,List<string>> Headers { get; set; }
        
        public Dictionary<string,List<string>> Queries { get; set; }

        public string EmbassyId { get; set; }
        
    }
}

