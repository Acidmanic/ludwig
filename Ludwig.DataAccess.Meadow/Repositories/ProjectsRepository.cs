using System.Linq;
using EnTier.DataAccess.Meadow;
using Ludwig.DataAccess.Contracts.Repositories;
using Ludwig.DataAccess.Meadow.Requests.Iterations;
using Ludwig.DataAccess.Meadow.Requests.Projects;
using Ludwig.DataAccess.Models;
using Meadow.Scaffolding.Contracts;
using Microsoft.Extensions.Logging;

namespace Ludwig.DataAccess.Meadow.Repositories
{
    public class ProjectsRepository : MeadowCrudRepository<Project, long>, IProjectsRepository
    {
        public ProjectsRepository(IMeadowConfigurationProvider provider) : base(provider.GetConfigurations())
        {
        }

        public override Project GetById(long id)
        {
            var request = new ReadProjectByIdFullTreeRequest(id);

            var engine = GetEngine();

            var response = engine.PerformRequest(request);

            if (response.Failed)
            {
                Logger.LogError(response.FailureException,
                    "Unable to perform {Operation} due to an exception: {Exception}",
                    request.RequestText, response.FailureException);
            }

            var project = response.FromStorage.FirstOrDefault();

            if (project != null)
            {
                ReadIterationsInto(project);
                
            }

            return project;
        }

        private void ReadIterationsInto(Project project)
        {
            if (project == null)
            {
                return;
            }

            var request = new ReadIterationByProjectFullTreeRequest(project.Id);

            var engine = GetEngine();

            var response = engine.PerformRequest(request);

            if (response.Failed)
            {
                Logger.LogError(response.FailureException,
                    "Unable to perform {Operation} due to an exception: {Exception}",
                    request.RequestText, response.FailureException);
            }

            project.Iterations = response.FromStorage;
        }

    }
}