using System;
using System.Linq.Expressions;
using Acidmanic.Utilities.Reflection;
using Acidmanic.Utilities.Reflection.Extensions;
using Ludwig.Common.Extensions;
using Ludwig.Contracts.Configurations;

namespace Ludwig.Common.Configuration
{
    public class Cib : ConfigurationItemBuilder
    {
    }
    public class Cib<TConfiguration> : ConfigurationItemBuilder<TConfiguration>
    {
    }

    public class ConfigurationItemBuilder<TConfiguration> : ConfigurationItemBuilder
    {
        public ConfigurationItemBuilder FromProperty<TProp>(Expression<Func<TConfiguration, TProp>> selector)
        {
            return base.FromProperty<TConfiguration, TProp>(selector);
        }
    }
    
    public class ConfigurationItemBuilder
    {
        private readonly ConfigurationDefinition _definition;

        public ConfigurationItemBuilder()
        {
            _definition = new ConfigurationDefinition();
            _definition.ValueType = typeof(string);
            
        }

        public ConfigurationItemBuilder Description(string description)
        {
            _definition.Description = description;

            return this;
        }
        
        public ConfigurationItemBuilder DisplayName(string name)
        {
            _definition.DisplayName = name;

            return this;
        }
        
        public ConfigurationItemBuilder Key(string key)
        {
            _definition.Key = key;

            return this;
        }

        public ConfigurationItemBuilder Type<TProp>()
        {
            _definition.ValueType = typeof(TProp);

            return this;
        }

        public ConfigurationItemBuilder FromProperty<TConfig, TProp>(Expression<Func<TConfig, TProp>> selector)
        {
            var key = MemberOwnerUtilities.GetKey(selector);

            _definition.Key = key.ToString();

            _definition.DisplayName = key.TerminalSegment().Name.TitleCase();

            Type<TProp>();

            return this;
        }

        public ConfigurationItemBuilder TypeString()
        {
            _definition.ValueType = typeof(string);
            
            _definition.AsString = o => (string)o;

            _definition.FromString = s => s;

            return this;
        }
        
        public ConfigurationItemBuilder TypeBoolean()
        {
            _definition.ValueType = typeof(bool);
            
            _definition.AsString = o =>
            {
                if (o is bool b)
                {
                    return b ? "true" : "false";
                }
                return "false";
            };

            _definition.FromString = s =>
            {
                if (!string.IsNullOrWhiteSpace(s))
                {
                    var normalized = s.Trim().ToLower();
                    return (normalized == "yes" || normalized == "true");
                }

                return false;
            };

            return this;
        }
        
        public ConfigurationItemBuilder TypeInteger()
        {
            _definition.ValueType = typeof(int);
            
            _definition.AsString = o =>
            {
                if (o is int i)
                {
                    return i.ToString();
                }
                return "0";
            };

            _definition.FromString = s =>
            {
                if (!string.IsNullOrWhiteSpace(s))
                {
                    var normalized = s.Trim().ToLower();

                    if (int.TryParse(normalized, out var value))
                    {
                        return value;
                    }
                }

                return 0;
            };

            return this;
        }

        public ConfigurationItemBuilder DirectCast<TProp>()
        {

            Type<TProp>();
            
            _definition.AsString = o => o.ToString();

            _definition.FromString = s => s.CastTo(_definition.ValueType);

            return this;
        }

        public ConfigurationItemBuilder ToObject(Func<string, object> convert)
        {
            _definition.FromString = convert;

            return this;
        }
        
        public ConfigurationItemBuilder ToString(Func<object,string> convert)
        {
            _definition.AsString = convert;

            return this;
        }


        public ConfigurationDefinition Build()
        {
            return _definition;
        }
    }
}