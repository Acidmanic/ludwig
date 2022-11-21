using System.Threading.Tasks;
using Ludwig.Contracts.Models;

namespace Ludwig.Contracts.Authentication
{
    public interface IAuthenticator
    {

        Task<AuthenticationResult> Authenticate();

        Task<RequestUpdate> GrantAccess(RequestUpdate requestUpdate);
        
    }
}