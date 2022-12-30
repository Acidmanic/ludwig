using System.Collections.Generic;
using System.Threading.Tasks;
using EnTier.DataAccess.Meadow;
using Ludwig.DataAccess.Contracts.Repositories;
using Ludwig.DataAccess.Meadow.Requests.StoryUsers;
using Ludwig.DataAccess.Models;
using Meadow.Requests;
using Meadow.Scaffolding.Contracts;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Ocsp;

namespace Ludwig.DataAccess.Meadow.Repositories
{
    public class StoryUsersRepository : MeadowCrudRepository<StoryUser, long>, IStoryUsersRepository
    {
        public StoryUsersRepository(IMeadowConfigurationProvider configurationProvider) : base(configurationProvider
            .GetConfigurations())
        {
        }

        public override StoryUser Update(StoryUser value)
        {
            return Set(value);
        }

        public override Task<StoryUser> UpdateAsync(StoryUser value)
        {
            return SetAsync(value);
        }

        public List<StoryUser> ReadStoryUserByNameRequest(string name)
        {
            var request = new ReadStoryUserByNameRequest(name);

            var engine = GetEngine();

            var response = engine.PerformRequest(request);

            ErrorCheck(response);

            return response.FromStorage ?? new List<StoryUser>();
        }


        private void ErrorCheck<TIn, TOut>(MeadowRequest<TIn, TOut> response) where TOut : class, new()
        {
            if (response.Failed)
            {
                Logger.LogError(response.FailureException,
                    "Unable to perform {Operation} due to exception: {Exception}",
                    response.RequestText, response.FailureException);
            }
        }
    }
}