namespace Ludwig.Common.Configuration
{
    public interface IConfigurationProvider<T>
    {
        T GetConfiguration();

        void SaveConfigurationChanges(T config);
    }
}