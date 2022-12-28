using Acidmanic.Utilities.Reflection.Attributes;

namespace Ludwig.DataAccess.Models
{
    [OwnerName("RequestUpdates")]
    public class RequestUpdateDal
    {
        [AutoValuedMember]
        [UniqueMember]
        public long Id { get; set; }
        
        public string Key { get; set; }
        
        public string Value { get; set; }
        
        public int Type { get; set; }
        
        public long AuthorizationRecordId { get; set; }
    }
}