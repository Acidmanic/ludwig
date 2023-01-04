using Ludwig.DataAccess.Meadow.Models;
using Ludwig.DataAccess.Models;
using Meadow.Requests;

namespace Ludwig.DataAccess.Meadow.Requests.Tasks
{
    public sealed class ReadTaskByIterationIdRequest : MeadowRequest<IdShell<long>, Task>
    {
        public ReadTaskByIterationIdRequest(long iterationId) : base(true)
        {
            ToStorage = new IdShell<long>
            {
                Id = iterationId
            };
        }

        protected override bool FullTreeReadWrite()
        {
            return true;
        }
    }
}