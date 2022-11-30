using System.Collections.Generic;
using System.Linq;
using Ludwig.Common.Configuration;
using Ludwig.Common.Extensions;
using Ludwig.Contracts.Configurations;
using Ludwig.Contracts.Extensions;
using Ludwig.Contracts.Models;

namespace Ludwig.Common.Utilities
{
    public class ConfigureByLogin<T> where T : new()
    {
        private readonly IConfigurationProvider _configurationProvider;


        public ConfigureByLogin(IConfigurationProvider configurationProvider)
        {
            _configurationProvider = configurationProvider;
        }


        public LoginMethod EquipForUi(LoginMethod method)
        {
            var result = new LoginMethod
            {
                Description = method.Description,
                Fields = new List<LoginField>(method.Fields),
                Link = method.Link,
                Name = method.Name,
                Queries = new List<LoginQuery>(method.Queries),
                ConfigurationRequirements = new List<ConfigurationRequirement>()
            };

            foreach (var requirement in method.ConfigurationRequirements)
            {
                var value = _configurationProvider.ReadByName<string>(requirement.ConfigurationName);

                if (string.IsNullOrWhiteSpace(value))
                {
                    var extraField = new LoginField
                    {
                        Description = requirement.Description,
                        Name = requirement.ConfigurationName.CamelCase(),
                        DisplayName = requirement.DisplayName,
                        UiProtectedValue = false
                    };

                    result.Fields.Add(extraField);
                }
            }

            return result;
        }


        public void HarvestConfigurations(Dictionary<string, string> parameters)
        {
            var configurationType = typeof(T);

            var stringType = typeof(string);

            var conf = _configurationProvider.GetConfiguration<T>();

            var properties = configurationType.GetProperties()
                .Where(p => p.CanRead && p.CanWrite && p.PropertyType == stringType);

            var keysList = parameters.Keys.ToList();

            var saveNeeded = false;

            foreach (var property in properties)
            {
                var name = property.Name.CamelCase();

                if (keysList.Contains(name))
                {
                    var value = parameters[name];

                    property.SetValue(conf, value);

                    saveNeeded = true;
                }
            }

            if (saveNeeded)
            {
                _configurationProvider.SaveConfigurationChanges();
            }
        }


        public string ReadConfigurationFirst(Dictionary<string, string> parameters, string name)
        {
            var value = _configurationProvider.ReadByName<string>(name.PascalCase());

            if (string.IsNullOrWhiteSpace(value))
            {
                value = parameters.Read("applicationId");
            }

            return value;
        }
    }
}