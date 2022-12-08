using System;
using System.Collections.Generic;
using System.Linq;
using Ludwig.Common.Extensions;
using Ludwig.Contracts.Configurations;
using Ludwig.Contracts.Models;
using Ludwig.Presentation.Configuration;
using Ludwig.Presentation.Models;

namespace Ludwig.Presentation.Services
{
    public class LoginMethodFilterService
    {
        private readonly LudwigConfigurationProvider _configurationProvider;
        private readonly List<string> _satisfiedConfigurations;
        
        public LoginMethodFilterService(LudwigConfigurationProvider configurationProvider)
        {
            _configurationProvider = configurationProvider;
            
            _configurationProvider.LoadConfigurations();

            _satisfiedConfigurations = new List<string>();
            
            var definitions = _configurationProvider.Definitions;
            
            foreach (var definition in definitions)
            {


                var stringValue = _configurationProvider.ReadStringValueByName(definition.Key);
                
                if (!string.IsNullOrWhiteSpace(stringValue))
                {
                    _satisfiedConfigurations.Add(definition.Key);
                }
            }
            
        }

        public List<LoginMethod> FilterByConfiguration(IEnumerable<LoginMethod> methods)
        {
            var transferItems = _configurationProvider.GetTransferItems();

            
            
            var usableConfigurations = new List<LoginMethod>();

            foreach (var method in methods)
            {
                if (Satisfied(method))
                {
                    usableConfigurations.Add(method);
                }
            }

            return usableConfigurations;
        }


        private bool Satisfied(LoginMethod method)
        {
            
            foreach (var requirement in method.ConfigurationRequirements)
            {
                var key = requirement.ConfigurationName.CamelCase();

                if (!_satisfiedConfigurations.Contains(key))
                {
                    return false;
                }
            }

            return true;
        }
    }
}