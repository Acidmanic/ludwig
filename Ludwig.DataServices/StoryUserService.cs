using EnTier;
using EnTier.Services;
using Ludwig.DataAccess.Models;
using Ludwig.DataServices.Contracts;

namespace Ludwig.DataService
{
    public class StoryUserService:CrudService<StoryUser,StoryUser, long,long>, IStoryUserService
    {
        public StoryUserService(EnTierEssence essence) : base(essence)
        {
        }


        public override StoryUser UpdateById(long id, StoryUser value)
        {
            var repository = UnitOfWork.GetCrudRepository<StoryUser, long>();
            
            return repository.Set(value);
        }

        

    }
}