using Ludwig.DataAccess.Meadow.Models;
using Ludwig.DataAccess.Models;
using Meadow.Requests;

namespace Ludwig.DataAccess.Meadow.Requests.StoryUsers
{
    public sealed class ReadStoryUserByNameRequest:MeadowRequest<NameShell,StoryUser>
    {
        public ReadStoryUserByNameRequest(string name) : base(true)
        {
            ToStorage = new NameShell
            {
                Name = name
            };
        }
    }
}