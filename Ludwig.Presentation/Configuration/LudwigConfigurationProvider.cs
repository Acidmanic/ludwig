using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Acidmanic.Utilities.Results;
using Ludwig.Common.Extensions;
using Ludwig.Contracts.Configurations;
using Ludwig.Presentation.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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
            
            LoadConfigurations();
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

                File.WriteAllText( _configurationsFile,json);
            }
        }

        public void LoadConfigurations()
        {
            lock (Locker)
            {
                try
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
                catch (Exception _) {}
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
                        var key = property.Name.CamelCase();
                        
                        var foundDefinition = GetDefinition(key, property.PropertyType);

                        if (foundDefinition.Success && _configurationData.ContainsKey(key))
                        {
                            var stringValue = _configurationData[key];

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

        public List<ConfigurationDefinition> Definitions => new List<ConfigurationDefinition>(_configurationDefinitions);

        public List<ConfigurationTransferItem> GetTransferItems()
        {
            var result = new List<ConfigurationTransferItem>();

            lock (Locker)
            {
                result.AddRange(
                    _configurationDefinitions.Select( d => new ConfigurationTransferItem
                    {
                        Description = d.Description,
                        Key = d.Key,
                        DisplayName = d.DisplayName,
                    StringValue = _configurationData.ContainsKey(d.Key) ? _configurationData[d.Key]:null
                    })
                );
            }
            return result;
        }

        public Result<Message> UpdateTransferItems(List<ConfigurationTransferItem> items)
        {

            var result = new Result<Message>().Succeed(new Message());
            
            foreach (var item in items)
            {
                var key = item.Key;

                var definition = _configurationDefinitions.FirstOrDefault(d => d.Key == key);

                if (definition != null)
                {

                    if (definition.VerifyStringValue(item.StringValue))
                    {
                        try
                        {
                            var value = definition.FromString(item.StringValue);

                            _configurationData[key] = item.StringValue;

                        }
                        catch (Exception e)
                        {
                            result.Success = false;
                    
                            result.Value.Lines.Add(e.Message);
                        }
                    }
                    else
                    {
                        result.Success = false;
                        
                        result.Value.Lines.Add($"The value: '{item.StringValue}'," +
                                               $" Is not valid for configuration item: '{definition.DisplayName}'. " +
                                               $"Hence it has not been set.");
                    }
                }
                else
                {
                    result.Value.Lines.Add($"It's rude to tamper with data! There is no such a thing as {key}.");
                }
            }

            SaveConfigurationChanges();
            
            return result;
        }
    }
}