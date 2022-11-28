namespace Ludwig.Contracts.Models
{
    public class LoginQuery
    {
        public string Name { get; set; }
        
        public string QueryKey { get; set; }
        
        public string ProvidedStateDescription { get; set; }
        
        public string NotProvidedStateDescription { get; set; }
    }
}