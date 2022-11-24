namespace Ludwig.Common.Configuration
{
    public interface IConfigurationProvider<T>
    {
        T Configuration { get; }

        void SaveConfigurationChanges();
    }
}