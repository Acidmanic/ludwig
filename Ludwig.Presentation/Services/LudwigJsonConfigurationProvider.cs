using System;
using System.IO;
using EnTier.Results;
using Ludwig.Presentation.Configuration;
using Ludwig.Presentation.Contracts;
using Newtonsoft.Json;

namespace Ludwig.Presentation.Services
{
    public class LudwigJsonConfigurationProvider:ILudwigConfigurationProvider
    {
        


        public LudwigConfiguration Configuration {

            get
            {
                var assembly = this.GetType().Assembly;

                var binDir = new FileInfo(assembly.Location??".").Directory.FullName;

                var configurationFile = Path.Combine(binDir, "Ludwig.Config.json");

                var readConfiguration = ReadFile<LudwigConfiguration>(configurationFile);

                if (readConfiguration)
                {
                    return readConfiguration;
                }

                var defaultConfiguration = new LudwigConfiguration
                {
                    JiraBaseUrl = "http://litbid.ir:8888"
                };

                WriteFile(defaultConfiguration, configurationFile);

                return defaultConfiguration;
            }
        }
        
        
        
        
        
        
        private Result<T> ReadFile<T>(string filePath)
        {

            try
            {
                var content = File.ReadAllText(filePath);

                var value = JsonConvert.DeserializeObject<T>(content);

                return new Result<T>(true, value);
            }
            catch (Exception)
            {
            }

            return new Result<T>().FailAndDefaultValue();
        }
        
        private Result WriteFile(object value, string path)
        {
            try
            {
                var json = JsonConvert.SerializeObject(value);

                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                
                File.WriteAllText(path,json);

                return new Result().Succeed();
            }
            catch (Exception)
            {
            }
            return new Result().Fail();
        }
    }
}