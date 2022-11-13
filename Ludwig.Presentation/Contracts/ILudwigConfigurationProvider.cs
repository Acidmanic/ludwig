using Ludwig.Presentation.Configuration;

namespace Ludwig.Presentation.Contracts
{
    public interface ILudwigConfigurationProvider
    {
        LudwigConfiguration Configuration { get; }
    }
}