using Ludwig.DataAccess.Meadow.Models;
using Ludwig.DataAccess.Models;
using Meadow.Requests;

namespace Ludwig.DataAccess.Meadow.Requests.Tasks
{
    public sealed class ReadTaskProjectIdRequest:MeadowRequest<IdShell<long>,Task>
    {
        public ReadTaskProjectIdRequest(long projectId) : base(true)
        {
            ToStorage = new IdShell<long> { Id = projectId };
        }

        protected override bool FullTreeReadWrite()
        {
            return true;
        }
    }
}