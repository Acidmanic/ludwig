using Meadow.Configuration;
using Meadow.Scaffolding.Contracts;

namespace Ludwig.DataAccess.Meadow
{
    public class MeadowConfigurationProvider : IMeadowConfigurationProvider
    {
        public MeadowConfiguration GetConfigurations()
        {
            return new MeadowConfiguration()
            {
                BuildupScriptDirectory = "Scripts",
                DatabaseFieldNameDelimiter = '_',
                ConnectionString = $"Allow User Variables=True;Server=localhost;Database=LudwigDb;Uid=sa;Pwd='never54aga.1n';"
            };
        }
    }
}