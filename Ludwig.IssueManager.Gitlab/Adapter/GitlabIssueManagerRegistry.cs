using Ludwig.Contracts.Di;
using Ludwig.IssueManager.Gitlab.Configurations;

namespace Ludwig.IssueManager.Gitlab.Adapter
{
    public class GitlabIssueManagerRegistry:RegistryBase
    {

        public GitlabIssueManagerRegistry()
        {
            Transient<GitlabConfigurationProvider>();
            
            Authenticator<OpenIdAuthenticatorPlus>();
            
            Authenticator<UsernamePasswordAuthenticator>();
            
            AddIssueManager<GitlabIssueManager>();
            
        }
    }
}