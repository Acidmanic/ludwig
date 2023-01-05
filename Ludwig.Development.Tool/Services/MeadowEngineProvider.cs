using Meadow;
using Meadow.Extensions;
using Meadow.MySql;
using Meadow.Scaffolding.Contracts;
using Microsoft.Extensions.Logging;

namespace Ludwig.Development.Tool.Services
{
    public class MeadowEngineProvider
    {
        private static readonly object Locker = new object();
        private static MeadowEngine _engine = null;
        
        
        private readonly IMeadowConfigurationProvider _meadowConfigurationProvider;
        private readonly ILogger _logger;
        
        public MeadowEngineProvider(IMeadowConfigurationProvider meadowConfigurationProvider, ILogger logger)
        {
            _meadowConfigurationProvider = meadowConfigurationProvider;
            _logger = logger;
        }


        public MeadowEngine ProvideEngine()
        {
            lock (Locker)
            {
                if (_engine == null)
                {
                    var assembly = _meadowConfigurationProvider.GetType().Assembly;
                
                    _engine = new MeadowEngine(_meadowConfigurationProvider.GetConfigurations(),assembly);

                    _logger.UseForMeadow();

                    _engine.UseMySql();
                }

                return _engine;
            }
        }
    }
}