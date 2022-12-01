using Ludwig.Contracts.Di;
using Ludwig.IssueManager.Gitlab.Configurations;

namespace Ludwig.IssueManager.Gitlab.Adapter
{
    public class GitlabIssueManagerRegistry:RegistryBase
    {

        public GitlabIssueManagerRegistry()
        {
            
            
            Configuration<GitlabConfigurationDescriptor>();
            
            Authenticator<OpenIdAuthenticatorPlus>();
            
            Authenticator<DirectGitlabCredentials>();
            
            AddIssueManager<GitlabIssueManager>();
            
        }
    }
}