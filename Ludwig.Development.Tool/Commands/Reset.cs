using System;
using System.ComponentModel;
using CoreCommandLine;
using CoreCommandLine.Attributes;
using Ludwig.DataAccess.Meadow;
using Ludwig.Development.Tool.Services;
using Meadow;
using Meadow.Extensions;
using Meadow.Scaffolding.Contracts;
using Microsoft.Extensions.Logging;

namespace Ludwig.Development.Tool.Commands
{
    [CommandName("reset")]
    public class Reset : CommandBase
    {
        private readonly MeadowEngineProvider _engineProvider;

        public Reset(MeadowEngineProvider engineProvider)
        {
            _engineProvider = engineProvider;
        }

        public override bool Execute(Context context, string[] args)
        {
            if (AmIPresent(args))
            {
                var engine = _engineProvider.ProvideEngine();

                try
                {
                    if (engine.DatabaseExists())
                    {
                        engine.DropDatabase();
                    }
                    
                    engine.CreateDatabase();

                    engine.BuildUpDatabase();
                }
                catch (Exception e)
                {
                    Logger.LogError(e, "Exception while performing {CommandName}: {Exception}", Name, e);
                }

                return true;
            }

            return false;
        }

        public override string Description => "Will Drop Database and build it up from scratch";
    }
}