using System.Collections.Generic;
using System.Threading.Tasks;
using Ludwig.Contracts.Models;

namespace Ludwig.Contracts.Authentication
{
    public interface IAuthenticator
    {

        Task<AuthenticationResult> Authenticate(Dictionary<string,string> parameters);

        Task<RequestUpdate> GrantAccess(RequestUpdate requestUpdate);
        
        LoginMethod LoginMethod { get; }
    }
}