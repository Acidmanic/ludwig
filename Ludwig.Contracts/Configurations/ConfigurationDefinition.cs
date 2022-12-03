using System;

namespace Ludwig.Contracts.Configurations
{
    public class ConfigurationDefinition
    {
        public string DisplayName { get; set; }
        
        public string Key { get; set; }
        
        public string Description { get; set; }
        
        public Type ValueType { get; set; }
        
        public Func<object,string> AsString { get; set; }
        
        public Func<string,object> FromString { get; set; }
        
        public Func<string,bool> VerifyStringValue { get; set; }
    }
}