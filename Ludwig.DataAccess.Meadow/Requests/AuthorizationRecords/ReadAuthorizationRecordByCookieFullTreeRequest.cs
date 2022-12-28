using Ludwig.DataAccess.Meadow.Models;
using Ludwig.DataAccess.Models;
using Meadow.Requests;

namespace Ludwig.DataAccess.Meadow.Requests.AuthorizationRecords
{
    public sealed class ReadAuthorizationRecordByCookieFullTreeRequest:MeadowRequest<CookieShell,AuthorizationRecordDal>
    {
        public ReadAuthorizationRecordByCookieFullTreeRequest(string cookie) : base(true)
        {

            ToStorage = new CookieShell
            {
                Cookie = cookie
            };
        }

        protected override bool FullTreeReadWrite()
        {
            return true;
        }
    }
}