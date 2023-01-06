using CoreCommandLine;
using CoreCommandLine.Attributes;
using EnTier.UnitOfWork;
using Ludwig.DataAccess.Models;
using Ludwig.Development.Tool.InMemory;

namespace Ludwig.Development.Tool.Commands
{
    [CommandName("seed-binary","sb")]
    public class SeedBinary:CommandBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public SeedBinary(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public override bool Execute(Context context, string[] args)
        {
            if (AmIPresent(args))
            {

                var binarySeed = new BinaryProjectSeed();

                Seed(binarySeed);
                
                return true;
            }

            return false;
        }

        private void Seed(BinaryProjectSeed binarySeed)
        {
            var projectRepo = _unitOfWork.GetCrudRepository<Project, long>();

            var insertedProject = projectRepo.Add(binarySeed.Project);

            binarySeed.Project.Id = insertedProject.Id;

            var iterationRepo = _unitOfWork.GetCrudRepository<Iteration, long>();

            foreach (var iteration in binarySeed.Project.Iterations)
            {
                iteration.ProjectId = insertedProject.Id;
                
                var insertedIteration = iterationRepo.Add(iteration);

                iteration.Id = insertedIteration.Id;
            }

            var goalRepo = _unitOfWork.GetCrudRepository<Goal, long>();
            var stepRepo = _unitOfWork.GetCrudRepository<Step, long>();
            var taskRepo = _unitOfWork.GetCrudRepository<Task, long>();
            
            foreach (var goal in binarySeed.Project.Goals)
            {
                goal.ProjectId = insertedProject.Id;

                var insertedGoal = goalRepo.Add(goal);

                goal.Id = insertedGoal.Id;

                foreach (var step in goal.Steps)
                {
                    step.ProjectId = insertedProject.Id;
                    
                    step.GoalId = insertedGoal.Id;

                    var insertedStep = stepRepo.Add(step);

                    step.Id = insertedStep.Id;


                    for (int i = 0; i < step.Tasks.Count; i++)
                    {
                        var task = step.Tasks[i];
                        task.StepId = insertedStep.Id;
                        task.IterationId = binarySeed.Project.Iterations[i].Id;
                        task.GoalId = insertedGoal.Id;
                        task.ProjectId = insertedProject.Id;

                        var insertedTask = taskRepo.Add(task);

                        task.Id = insertedTask.Id;

                    }
                }
            }
        }
    }
}