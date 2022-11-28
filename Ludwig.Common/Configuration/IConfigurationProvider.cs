namespace Ludwig.Common.Configuration
{
    public interface IConfigurationProvider<T>
    {
        T Configuration { get; }

        void SaveConfigurationChanges();

        TProperty ReadByName<TProperty>(string name, TProperty defaultValue = default);

        void WriteByName(string name, object value);
    }
}