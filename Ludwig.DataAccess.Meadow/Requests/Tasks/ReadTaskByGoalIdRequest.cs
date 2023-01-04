using Ludwig.DataAccess.Meadow.Models;
using Ludwig.DataAccess.Models;
using Meadow.Requests;

namespace Ludwig.DataAccess.Meadow.Requests.Tasks
{
    public sealed class ReadTaskByGoalIdRequest : MeadowRequest<IdShell<long>, Task>
    {
        public ReadTaskByGoalIdRequest(long goalId) : base(true)
        {
            ToStorage = new IdShell<long>
            {
                Id = goalId
            };
        }

        protected override bool FullTreeReadWrite()
        {
            return true;
        }
    }
}