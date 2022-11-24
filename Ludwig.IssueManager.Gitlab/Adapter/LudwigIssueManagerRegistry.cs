using Ludwig.Contracts.Di;
using Ludwig.IssueManager.Gitlab.Configurations;

namespace Ludwig.IssueManager.Gitlab.Adapter
{
    public class LudwigIssueManagerRegistry:RegistryBase
    {

        public LudwigIssueManagerRegistry()
        {
            Transient<GitlabConfigurationProvider>();
            
            Authenticator<UsernamePasswordAuthenticator>();
        }
    }
}