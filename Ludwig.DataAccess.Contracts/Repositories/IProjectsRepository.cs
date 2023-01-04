using EnTier.Repositories;
using Ludwig.DataAccess.Models;

namespace Ludwig.DataAccess.Contracts.Repositories
{
    public interface IProjectsRepository:ICrudRepository<Project,long>
    {
        
    }
}