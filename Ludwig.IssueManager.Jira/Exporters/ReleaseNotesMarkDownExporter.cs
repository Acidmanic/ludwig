using System.Threading.Tasks;
using Ludwig.Contracts;
using Ludwig.Contracts.Configurations;
using Ludwig.Contracts.Models;
using Ludwig.IssueManager.Jira.Configuration;
using Ludwig.IssueManager.Jira.Services;

namespace Ludwig.IssueManager.Jira.Exporters
{
    internal class ReleaseNotesMarkDownExporter : IExporter
    {
        private readonly Services.Jira.Jira _jira;
        private readonly IConfigurationProvider _configurationProvider;

        public ReleaseNotesMarkDownExporter(Services.Jira.Jira jira, IConfigurationProvider configurationProvider)
        {
            _jira = jira;
            _configurationProvider = configurationProvider;
        }

        public ExportInformation Id { get; } = new ExportInformation
        {
            DisplayName = "Jira - Release notes",
            UniqueKey = "a5a83d8e-7a63-11ed-be31-4707aa0203de"
        };

        public async Task<ExportData> ProvideExport()
        {
            _configurationProvider.LoadConfigurations();

            var config = _configurationProvider.GetConfiguration<JiraConfiguration>();
            var projectId = config.JiraProject;
            var jiraUrl = config.JiraFrontChannelUrl;


            if (!string.IsNullOrEmpty(projectId))
            {
                var issues = await _jira.IssuesByProject(projectId);

                var doc = new ReleaseNoteDocument(issues, jiraUrl);

                var mdString = doc.ToString();

                var bytes = System.Text.Encoding.UTF8.GetBytes(mdString);

                return new ExportData
                {
                    Data = bytes,
                    FileName = "Jira - Release Notes.md",
                    MediaType = "text/markdown"
                };
            }

            return null;
        }
    }
}