using System.Collections.Generic;
using Ludwig.Contracts.Models;

namespace Ludwig.Contracts.Authentication
{
    public interface IBackChannelRequestGrant
    {

        List<RequestUpdate> GetGrantRequestUpdates();
    }
}