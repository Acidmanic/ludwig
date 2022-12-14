namespace Ludwig.Contracts.Configurations
{
    public interface IConfigurationProvider
    {
        
        void SaveConfigurationChanges();

        void LoadConfigurations();
        
        TProperty ReadByName<TProperty>(string name, TProperty defaultValue = default);

        void WriteByName<TProperty>(string name, TProperty value);
        

        T GetConfiguration<T>() where T : new();
    }
}