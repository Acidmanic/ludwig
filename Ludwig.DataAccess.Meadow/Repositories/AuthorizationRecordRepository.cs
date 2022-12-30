using System.Collections.Generic;
using System.Linq;
using EnTier.DataAccess.Meadow;
using Ludwig.DataAccess.Contracts.Repositories;
using Ludwig.DataAccess.Meadow.Requests.AuthorizationRecords;
using Ludwig.DataAccess.Models;
using Meadow.Configuration;
using Meadow.Requests;
using Meadow.Scaffolding.Contracts;
using Microsoft.Extensions.Logging;

namespace Ludwig.DataAccess.Meadow.Repositories
{
    public class AuthorizationRecordRepository : MeadowCrudRepository<AuthorizationRecordDal, long>,
        IAuthorizationRecordRepository
    {
        public AuthorizationRecordRepository(IMeadowConfigurationProvider configurationProvider) : base(
            configurationProvider.GetConfigurations())
        {
        }


        public override AuthorizationRecordDal GetById(long id)
        {
            var request = new ReadAuthorizationRecordByIdFullTreeRequest(id);

            var engine = GetEngine();

            var response = engine.PerformRequest(request);

            ErrorCheck(response);

            return response.FromStorage?.FirstOrDefault();
        }

        public AuthorizationRecordDal ReadAuthorizationRecordByToken(string token)
        {
            var request = new  ReadAuthorizationRecordByTokenFullTreeRequest(token);
            
            var engine = GetEngine();

            var response = engine.PerformRequest(request);

            ErrorCheck(response);

            return response.FromStorage?.FirstOrDefault();
            
        }

        public AuthorizationRecordDal ReadAuthorizationRecordByCookie(string cookie)
        {
            var request = new ReadAuthorizationRecordByCookieFullTreeRequest(cookie);
            
            var engine = GetEngine();

            var response = engine.PerformRequest(request);

            ErrorCheck(response);

            return response.FromStorage?.FirstOrDefault();
        }


        private void ErrorCheck<I, O>(MeadowRequest<I, O> response) where O : class, new()
        {
            if (response.Failed)
            {
                Logger.LogError(response.FailureException,
                    "Unable to perform {Opration} due to an exception: {Exception}",
                    response.RequestText, response.FailureException);
            }
        }
    }
}