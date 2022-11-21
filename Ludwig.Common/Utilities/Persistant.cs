using System.IO;
using System.Reflection;
using Acidmanic.Utilities.Results;
using Newtonsoft.Json;

namespace Ludwig.Common.Utilities
{
    public class Persistant<T>
        where T : new()
    {
        public T Value { get; set; }

        private readonly string _filePath;

        public Persistant()
        {
            _filePath =
                new FileInfo(Assembly.GetEntryAssembly()?.Location ?? "").Directory?.FullName ?? "";

            _filePath = Path.Join(_filePath, $"{typeof(T).FullName}.Json");
        }

        public void Load()
        {
            if (!File.Exists(_filePath))
            {
                Value = new T();

                Save();
            }

            var json = File.ReadAllText(_filePath);

            Value = JsonConvert.DeserializeObject<T>(json);
        }

        public void Save()
        {
            var json = JsonConvert.SerializeObject(Value);

            if (File.Exists(_filePath))
            {
                File.Delete(_filePath);
            }
            File.WriteAllText(_filePath, json);
        }
    }
}