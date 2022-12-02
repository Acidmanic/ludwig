using Ludwig.Contracts.Di;
using Ludwig.IssueManager.Gitlab.Configurations;

namespace Ludwig.IssueManager.Gitlab.Adapter
{
    public class GitlabIssueManagerRegistry:RegistryBase
    {

        public GitlabIssueManagerRegistry()
        {
            
            
            Configuration<GitlabConfigurationDescriptor>();
            
            Authenticator<OpenIdAuthenticator>();
            
            Authenticator<DirectGitlabCredentials>();
            
            AddIssueManager<GitlabIssueManager>();
            
        }
    }
}