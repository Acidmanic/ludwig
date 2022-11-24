using System.IO;
using System.Reflection;
using Ludwig.Common.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Ludwig.Common.Configuration
{
    public class LudwigConfigurationProvider<T>:IConfigurationProvider<T>
        where T : new()
    {
        private static readonly object Locker;
        public static bool ConfigurationRead { get; private set; }
        private static T _configuration;
        private static string _configurationFile;
        private static string _configurationsDirectory;

        public LudwigConfigurationProvider()
        {
        }

        static LudwigConfigurationProvider()
        {
            Locker = new object();
            ConfigurationRead = false;
            _configuration = new T();
        }


        protected void CheckConfigurationsBeRead()
        {
            lock (Locker)
            {
                if (!ConfigurationRead)
                {

                    _configurationsDirectory = this.FilePathInExecutionDirectory("Ludwig.Config");

                    if (!Directory.Exists(_configurationsDirectory))
                    {
                        Directory.CreateDirectory(_configurationsDirectory);
                    }
                    
                    _configurationFile = Path.Join(_configurationsDirectory,typeof(T).FullName + ".json");
                    
                    ConfigurationRead = true;

                    if (!File.Exists(_configurationFile))
                    {
                        var defaultConfigurations = new T();
                        
                        OnFirstWrite(defaultConfigurations);

                        var json = JsonConvert.SerializeObject(defaultConfigurations);

                        File.WriteAllText(_configurationFile, json);
                    }
                    else
                    {
                        var json = File.ReadAllText(_configurationFile);

                        _configuration = JsonConvert.DeserializeObject<T>(json);
                    }
                }
            }
        }

        public T Configuration
        {
            get
            {
                CheckConfigurationsBeRead();

                return _configuration;
            }
        }

        public void SaveConfigurationChanges()
        {
            CheckConfigurationsBeRead();

            var json = JsonConvert.SerializeObject(_configuration);

            if (File.Exists(_configurationFile))
            {
                File.Delete(_configurationFile);
            }

            File.WriteAllText(_configurationFile, json);
        }

        protected virtual void OnFirstWrite(T configuration)
        {
            
        }
    }
}