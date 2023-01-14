using Ludwig.DataAccess.Meadow.Models;
using Ludwig.DataAccess.Models;
using Meadow.Requests;

namespace Ludwig.DataAccess.Meadow.Requests.Iterations
{
    public sealed class ReadIterationsByProjectIdRequest:MeadowRequest<IdShell<long>,Iteration>
    {
        public ReadIterationsByProjectIdRequest(long projectId) : base(true)
        {
            ToStorage = new IdShell<long> { Id = projectId };
        }
    }
}