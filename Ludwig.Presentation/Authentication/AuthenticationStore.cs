using System;
using System.Collections.Generic;
using System.Linq;
using EnTier.Repositories;
using EnTier.Results;
using EnTier.UnitOfWork;
using Ludwig.Contracts.Models;

namespace Ludwig.Presentation.Authentication
{
    public class AuthenticationStore
    {
        private readonly ICrudRepository<AuthorizationRecord, long> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public AuthenticationStore(IUnitOfWork unitOfWork)
        {
            _repository = unitOfWork.GetCrudRepository<AuthorizationRecord, long>();
            _unitOfWork = unitOfWork;
        }


        public AuthorizationRecord GenerateToken(
            string loginMethodName, 
            AuthenticationResult result,
            string requestOrigin,
            List<RequestUpdate> grantBackAccessUpdates)
        {
            var token = Guid.NewGuid().ToString();
            var cookie = Guid.NewGuid().ToString();
            
            var record = new AuthorizationRecord
            {
                RequestOrigin = requestOrigin,
                Token = token,
                Cookie = cookie,
                ExpirationEpoch = DateTime.Now.AddDays(7).Ticks,
                LoginMethodName = loginMethodName,
                SubjectId = result.SubjectId,
                EmailAddress = result.EmailAddress,
                SubjectWebPage = result.SubjectWebPage,
                BackChannelGrantAccessUpdates = grantBackAccessUpdates,
                IsAdministrator = result.IsAdministrator,
                IsIssueManager = result.IsIssueManager
            };

            record = _repository.Add(record);

            _unitOfWork.Complete();

            return record;
        }


        public Result<AuthorizationRecord> IsTokenRegistered(string token)
        {
            var record = 
                _repository.Find(r => r.Token == token)
                    .FirstOrDefault();

            if (record == null)
            {
                return new Result<AuthorizationRecord>().FailAndDefaultValue();
            }

            return new Result<AuthorizationRecord>().Succeed(record);
        }
        
        public Result<AuthorizationRecord> IsCookieRegistered(string cookie)
        {
            var record = 
                _repository.Find(r => r.Cookie == cookie)
                    .FirstOrDefault();

            if (record == null)
            {
                return new Result<AuthorizationRecord>().FailAndDefaultValue();
            }

            return new Result<AuthorizationRecord>().Succeed(record);
        }


        public void RemoveAuthorization(string token)
        {

            var foundRecord = IsTokenRegistered(token);

            if (foundRecord)
            {
                _repository.Remove(foundRecord.Value.Id);

                _unitOfWork.Complete();
            }
        }
    }
}