using Ludwig.DataAccess.Meadow.Models;
using Ludwig.DataAccess.Models;
using Meadow.Requests;

namespace Ludwig.DataAccess.Meadow.Requests.Iterations
{
    public sealed class ReadIterationByProjectFullTreeRequest : MeadowRequest<IdShell<long>, Iteration>
    {
        public ReadIterationByProjectFullTreeRequest(long projectId) : base(true)
        {
            ToStorage = new IdShell<long> { Id = projectId };
        }

        protected override bool FullTreeReadWrite()
        {
            return true;
        }
    }
}