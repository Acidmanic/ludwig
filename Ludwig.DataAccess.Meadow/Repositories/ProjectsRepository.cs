using System.Collections.Generic;
using System.Linq;
using EnTier.DataAccess.Meadow;
using Ludwig.DataAccess.Contracts.Repositories;
using Ludwig.DataAccess.Meadow.Requests.Iterations;
using Ludwig.DataAccess.Meadow.Requests.Projects;
using Ludwig.DataAccess.Meadow.Requests.Tasks;
using Ludwig.DataAccess.Models;
using Meadow.Requests;
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

            ReadFullTreeInto(project);

            return project;
        }

        private void ReadFullTreeInto(Project project)
        {
            if (project != null)
            {
                ReadIterationsInto(project);
                
                var tasks = ReadTasks(new ReadTaskProjectIdRequest(project.Id));
                
                PutTasksInto(project,tasks);
            }
        }

        private void ReadIterationsInto(Project project)
        {
            if (project == null)
            {
                return;
            }

            var request = new ReadIterationsByProjectIdRequest(project.Id);

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

        private void PutTasksInto(Project project, List<Task> tasks)
        {
            foreach (var goal in project.Goals)
            {
                foreach (var step in goal.Steps)
                {
                    step.Tasks = new List<Task>();
                    
                    step.Tasks.AddRange(tasks.Where(t => t.StepId==step.Id));
                }
            }

            foreach (var iteration in project.Iterations)
            {
                iteration.Tasks = new List<Task>();
                
                iteration.Tasks.AddRange(tasks.Where( t=> t.IterationId == iteration.Id));
            }
        }
        

        private void ReadTasksInto(Project project)
        {
            if (project == null)
            {
                return;
            }

            foreach (var projectGoal in project.Goals)
            {
                foreach (var step in projectGoal.Steps)
                {
                    var request = new ReadTaskByStepIdRequest(step.Id);

                    var tasks = ReadTasks(request);

                    step.Tasks = new List<Task>(tasks);
                }
            }

            foreach (var iteration in project.Iterations)
            {
                var request = new ReadTaskByIterationIdRequest(iteration.Id);

                var tasks = ReadTasks(request);

                iteration.Tasks = new List<Task>(tasks);
            }
        }


        private List<Task> ReadTasks<TIn>(MeadowRequest<TIn, Task> request)
        {
            var engine = GetEngine();

            var response = engine.PerformRequest(request);

            if (response.Failed)
            {
                Logger.LogError(response.FailureException,
                    "Unable to perform {Operation} due to an exception: {Exception}",
                    request.RequestText, response.FailureException);
            }

            return response.FromStorage;
        }
    }
}