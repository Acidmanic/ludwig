using Ludwig.Contracts.Di;
using Ludwig.Presentation.Configuration;

namespace Ludwig.Presentation.Administration
{
    public class AdministrationRegistry:RegistryBase
    {
        public AdministrationRegistry()
        {
            AddIssueManager<AdministrationIssueManager>();
            
            Configuration<LudwigConfigurationDescriptor>();
            
            Authenticator<AdministrationAuthenticator>();
        }
    }
}