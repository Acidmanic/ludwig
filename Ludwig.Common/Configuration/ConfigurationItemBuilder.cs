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
        private readonly ConfigurationItem _item;

        public ConfigurationItemBuilder()
        {
            _item = new ConfigurationItem();
            _item.ValueType = typeof(string);
            
        }

        public ConfigurationItemBuilder Description(string description)
        {
            _item.Description = description;

            return this;
        }
        
        public ConfigurationItemBuilder DisplayName(string name)
        {
            _item.DisplayName = name;

            return this;
        }
        
        public ConfigurationItemBuilder Key(string key)
        {
            _item.Key = key;

            return this;
        }

        public ConfigurationItemBuilder Type<TProp>()
        {
            _item.ValueType = typeof(TProp);

            return this;
        }

        public ConfigurationItemBuilder FromProperty<TConfig, TProp>(Expression<Func<TConfig, TProp>> selector)
        {
            var key = MemberOwnerUtilities.GetKey(selector);

            _item.Key = key.ToString();

            _item.DisplayName = key.TerminalSegment().Name.TitleCase();

            Type<TProp>();

            return this;
        }

        public ConfigurationItemBuilder TypeString()
        {
            _item.ValueType = typeof(string);
            
            _item.AsString = o => (string)o;

            _item.FromString = s => s;

            return this;
        }
        
        public ConfigurationItemBuilder TypeBoolean()
        {
            _item.ValueType = typeof(bool);
            
            _item.AsString = o =>
            {
                if (o is bool b)
                {
                    return b ? "true" : "false";
                }
                return "false";
            };

            _item.FromString = s =>
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
            _item.ValueType = typeof(int);
            
            _item.AsString = o =>
            {
                if (o is int i)
                {
                    return i.ToString();
                }
                return "0";
            };

            _item.FromString = s =>
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
            
            _item.AsString = o => o.ToString();

            _item.FromString = s => s.CastTo(_item.ValueType);

            return this;
        }

        public ConfigurationItemBuilder ToObject(Func<string, object> convert)
        {
            _item.FromString = convert;

            return this;
        }
        
        public ConfigurationItemBuilder ToString(Func<object,string> convert)
        {
            _item.AsString = convert;

            return this;
        }


        public ConfigurationItem Build()
        {
            return _item;
        }
    }
}