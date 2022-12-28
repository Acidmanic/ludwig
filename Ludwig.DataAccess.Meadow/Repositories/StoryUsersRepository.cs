using System.Threading.Tasks;
using EnTier.DataAccess.Meadow;
using Ludwig.DataAccess.Contracts.Repositories;
using Ludwig.DataAccess.Models;
using Meadow.Scaffolding.Contracts;

namespace Ludwig.DataAccess.Meadow.Repositories
{
    public class StoryUsersRepository:MeadowCrudRepository<StoryUser,long>,IStoryUsersRepository
    {
        public StoryUsersRepository(IMeadowConfigurationProvider configurationProvider) : base(configurationProvider.GetConfigurations())
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
    }
}