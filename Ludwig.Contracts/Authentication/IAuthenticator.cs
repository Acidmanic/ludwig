using System.Collections.Generic;
using System.Threading.Tasks;
using Ludwig.Contracts.Models;

namespace Ludwig.Contracts.Authentication
{
    public interface IAuthenticator
    {

        Task<AuthenticationResult> Authenticate(Dictionary<string,string> parameters);

        Task<List<RequestUpdate>> GrantAccess();
        
        LoginMethod LoginMethod { get; }
    }
}