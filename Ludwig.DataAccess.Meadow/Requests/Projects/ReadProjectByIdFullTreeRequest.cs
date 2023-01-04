using Ludwig.DataAccess.Meadow.Models;
using Ludwig.DataAccess.Models;
using Meadow.Requests;

namespace Ludwig.DataAccess.Meadow.Requests.Projects
{
    public sealed class ReadProjectByIdFullTreeRequest:MeadowRequest<IdShell<long>,Project>
    {
        public ReadProjectByIdFullTreeRequest(long projectId) : base(true)
        {
            ToStorage = new IdShell<long>
            {
                Id = projectId
            };
        }

        protected override bool FullTreeReadWrite()
        {
            return true;
        }
    }
}