using System.Collections.Generic;
using EnTier.Repositories;
using Ludwig.DataAccess.Models;

namespace Ludwig.DataAccess.Contracts.Repositories
{
    public interface IAuthorizationRecordRepository:ICrudRepository<AuthorizationRecordDal,long>
    {
        AuthorizationRecordDal ReadAuthorizationRecordByToken(string token);
        
        AuthorizationRecordDal ReadAuthorizationRecordByCookie(string cookie);
        
        
    }
}