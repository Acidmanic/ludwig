using Ludwig.DataAccess.Meadow.Models;
using Ludwig.DataAccess.Models;
using Meadow.Requests;

namespace Ludwig.DataAccess.Meadow.Requests.Tasks
{
    public sealed class ReadTaskByStepIdRequest : MeadowRequest<IdShell<long>, Task>
    {
        public ReadTaskByStepIdRequest(long stepId) : base(true)
        {
            ToStorage = new IdShell<long>
            {
                Id = stepId
            };
        }

        protected override bool FullTreeReadWrite()
        {
            return true;
        }
    }
}