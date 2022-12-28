using Ludwig.DataAccess.Meadow.Models;
using Ludwig.DataAccess.Models;
using Meadow.Requests;

namespace Ludwig.DataAccess.Meadow.Requests.AuthorizationRecords
{
    public sealed class ReadAuthorizationRecordByTokenFullTreeRequest:MeadowRequest<TokenShell<string>,AuthorizationRecordDal>
    {
        public ReadAuthorizationRecordByTokenFullTreeRequest(string token) : base(true)
        {
            ToStorage = new TokenShell<string>
            {
                Token = token
            };
        }


        protected override bool FullTreeReadWrite()
        {
            return true;
        }
    }
}