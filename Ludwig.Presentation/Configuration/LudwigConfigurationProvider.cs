using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Acidmanic.Utilities.Results;
using Ludwig.Common.Configuration;
using Ludwig.Common.Extensions;
using Ludwig.Contracts.Configurations;
using Newtonsoft.Json;

namespace Ludwig.Presentation.Configuration
{
    public class LudwigConfigurationProvider : IConfigurationProvider
    {
        private readonly Dictionary<string, string> _configurationData = new Dictionary<string, string>();
        private readonly List<ConfigurationDefinition> _configurationDefinitions = new List<ConfigurationDefinition>();
        private readonly string _configurationsFile;

        private static readonly object Locker = new object();

        public LudwigConfigurationProvider()
        {
            _configurationsFile = new object().FilePathInExecutionDirectory("Ludwig.Config.json");
        }


        public void AddDefinition(ConfigurationDefinition definition)
        {
            lock (Locker)
            {
                _configurationDefinitions.Add(definition);
            }
        }

        public void AddDefinitions(IEnumerable<ConfigurationDefinition> definitions)
        {
            lock (Locker)
            {
                _configurationDefinitions.AddRange(definitions);
            }
        }


        public void SaveConfigurationChanges()
        {
            lock (Locker)
            {
                var json = JsonConvert.SerializeObject(_configurationData);

                if (File.Exists(_configurationsFile))
                {
                    File.Delete(_configurationsFile);
                }

                File.WriteAllText(json, _configurationsFile);
            }
        }

        public void LoadConfigurationChanges()
        {
            lock (Locker)
            {
                var json = File.ReadAllText(_configurationsFile);

                var data = JsonConvert.DeserializeObject<Dictionary<string, String>>(json);

                _configurationData.Clear();
                
                if (data != null)
                {
                    foreach (var keyValuePair in data)
                    {
                        _configurationData.Add(keyValuePair.Key,keyValuePair.Value);
                    }
                }
            }
        }

        public TProperty ReadByName<TProperty>(string name, TProperty defaultValue = default)
        {
            lock (Locker)
            {
                if (_configurationData.ContainsKey(name))
                {
                    var foundDefinition = GetDefinition<TProperty>(name);

                    if (foundDefinition)
                    {
                        var stringValue = _configurationData[name];

                        try
                        {
                            var propertyValue = (TProperty)foundDefinition.Value.FromString(stringValue);

                            return propertyValue;
                        }
                        catch (Exception _)
                        {
                        }
                    }
                }
            }

            return defaultValue;
        }


        public void WriteByName<TProperty>(string name, TProperty value)
        {
            lock (Locker)
            {
                var foundDefinition = GetDefinition<TProperty>(name);

                if (foundDefinition)
                {
                    var stringValue = foundDefinition.Value.AsString(value);

                    if (_configurationData.ContainsKey(name))
                    {
                        _configurationData.Remove(name);
                    }

                    _configurationData.Add(name, stringValue);

                    SaveConfigurationChanges();
                }
            }
        }

        public TConfig GetConfiguration<TConfig>() where TConfig : new()
        {
            var configuration = new TConfig();

            lock (Locker)
            {
                var configurationType = typeof(TConfig);

                var properties = configurationType.GetProperties();

                foreach (var property in properties)
                {
                    if (property.CanRead && property.CanWrite)
                    {
                        var foundDefinition = GetDefinition(property.Name, property.PropertyType);

                        if (foundDefinition.Success && _configurationData.ContainsKey(property.Name))
                        {
                            var stringValue = _configurationData[property.Name];

                            var propertyValue = foundDefinition.Value.FromString(stringValue);

                            property.SetValue(configuration, propertyValue);
                        }
                    }
                }
            }

            return configuration;
        }

        private Result<ConfigurationDefinition> GetDefinition<TProperty>(string name)
        {
            return GetDefinition(name, typeof(TProperty));
        }

        private Result<ConfigurationDefinition> GetDefinition(string name, Type propertyType)
        {
            var foundItem = _configurationDefinitions
                .FirstOrDefault(c => c.Key == name && c.ValueType == propertyType);

            if (foundItem != null)
            {
                return new Result<ConfigurationDefinition>(true, foundItem);
            }

            return new Result<ConfigurationDefinition>().FailAndDefaultValue();
        }
    }
}