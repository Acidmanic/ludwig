using CoreCommandLine;
using CoreCommandLine.Attributes;
using Ludwig.Development.Tool.Commands;

namespace Ludwig.Development.Tool
{
    [Subcommands(
        typeof(Reset),
        typeof(SeedBinary),
        typeof(View)
    )]
    public class LdtApplication : CommandLineApplication
    {
        protected override void OnAfterExecution(Context context, string[] args, ICommand command)
        {
            Output("-------------------------------------------------");
        }
    }
}