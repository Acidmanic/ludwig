using Ludwig.DataAccess.Meadow.Requests.Common;
using Ludwig.DataAccess.Models;
using Meadow.Requests;

namespace Ludwig.DataAccess.Meadow.Requests.AuthorizationRecords
{
    public class ReadAuthorizationRecordByIdFullTreeRequest:GetByIdRequest<AuthorizationRecordDal,long>
    {
        public ReadAuthorizationRecordByIdFullTreeRequest(long id) : base(id, true)
        {
        }
    }
}