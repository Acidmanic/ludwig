using System.Collections.Generic;
using Acidmanic.Utilities.Reflection.Attributes;

namespace ApiEmbassy.Models
{
    public class ResponseCarrier
    {
        
        [AutoValuedMember]
        [UniqueMember]
        public long Id { get; set; }
        public Dictionary<string,List<string>> Headers { get; set; }
        
        public int StatusCode { get; set; }
        
        public byte[] Content { get; set; }
        
        public long RequestId { get; set; }
    }
}