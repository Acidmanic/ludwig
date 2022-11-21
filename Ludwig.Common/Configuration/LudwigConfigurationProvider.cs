using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Ludwig.Common.Configuration
{
    public class LudwigConfigurationProvider<T>:IConfigurationProvider<T>
        where T : new()
    {
        private static readonly object Locker;
        public static bool ConfigurationRead { get; private set; }
        private static JObject _configuration;
        private static string _configurationFile;

        public LudwigConfigurationProvider()
        {
        }

        static LudwigConfigurationProvider()
        {
            Locker = new object();
            ConfigurationRead = false;
            _configuration = new JObject();
        }


        protected void CheckConfigurationsBeRead()
        {
            lock (Locker)
            {
                if (!ConfigurationRead)
                {
                    _configurationFile =
                        new FileInfo(Assembly.GetEntryAssembly()?.Location ?? "").Directory?.FullName ?? "";

                    _configurationFile = Path.Join(_configurationFile, "Ludwig.Config.Json");


                    ConfigurationRead = true;

                    if (!File.Exists(_configurationFile))
                    {
                        var defaultConfigurations = new T();

                        var json = JsonConvert.SerializeObject(defaultConfigurations);

                        File.WriteAllText(_configurationFile, json);
                    }
                    else
                    {
                        var json = File.ReadAllText(_configurationFile);

                        _configuration = JsonConvert.DeserializeObject<JObject>(json);
                    }
                }
            }
        }

        public T GetConfiguration()
        {
            CheckConfigurationsBeRead();

            return _configuration.ToObject<T>();
        }

        public void SaveConfigurationChanges(T config)
        {
            CheckConfigurationsBeRead();

            _configuration.Merge(config);

            var json = JsonConvert.SerializeObject(_configuration);

            if (File.Exists(_configurationFile))
            {
                File.Delete(_configurationFile);
            }

            File.WriteAllText(_configurationFile, json);
        }
    }
}