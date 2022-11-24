using Ludwig.Common.Configuration;

namespace Ludwig.IssueManager.Gitlab.Configurations
{
    public class GitlabConfigurationProvider:LudwigConfigurationProvider<GitlabConfigurations>
    {
        protected override void OnFirstWrite(GitlabConfigurations configuration)
        {
            configuration.GitlabInstanceBackChannel = "http://litbid.ir";
            configuration.GitlabInstanceFrontChannel = "http://litbid.ir";
        }
    }
}