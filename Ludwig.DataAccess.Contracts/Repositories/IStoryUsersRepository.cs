using System.Collections.Generic;
using EnTier.Repositories;
using Ludwig.DataAccess.Models;

namespace Ludwig.DataAccess.Contracts.Repositories
{
    public interface IStoryUsersRepository:ICrudRepository<StoryUser,long>
    {



        List<StoryUser> ReadStoryUserByNameRequest(string name);
    }
}