using System;
using System.Linq;
using EnTier.Repositories;
using EnTier.Results;
using EnTier.UnitOfWork;
using Ludwig.Contracts.Models;

namespace Ludwig.Presentation.Authentication
{
    public class AuthenticationStore
    {
        private readonly ICrudRepository<AuthenticationRecord, long> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public AuthenticationStore(IUnitOfWork unitOfWork)
        {
            _repository = unitOfWork.GetCrudRepository<AuthenticationRecord, long>();
            _unitOfWork = unitOfWork;
        }


        public AuthenticationRecord GenerateToken(
            string loginMethodName, 
            AuthenticationResult result,
            string requestOrigin)
        {
            var token = Guid.NewGuid().ToString();
            var cookie = Guid.NewGuid().ToString();
            
            var record = new AuthenticationRecord
            {
                RequestOrigin = requestOrigin,
                Token = token,
                Cookie = cookie,
                ExpirationEpoch = DateTime.Now.AddDays(7).Ticks,
                LoginMethodName = loginMethodName,
                SubjectId = result.SubjectId,
                EmailAddress = result.EmailAddress,
                SubjectWebPage = result.SubjectWebPage
            };

            record = _repository.Add(record);

            _unitOfWork.Complete();

            return record;
        }


        public Result<AuthenticationRecord> IsTokenRegistered(string token)
        {
            var record = 
                _repository.Find(r => r.Token == token)
                    .FirstOrDefault();

            if (record == null)
            {
                return new Result<AuthenticationRecord>().FailAndDefaultValue();
            }

            return new Result<AuthenticationRecord>().Succeed(record);
        }
        
        public Result<AuthenticationRecord> IsCookieRegistered(string cookie)
        {
            var record = 
                _repository.Find(r => r.Cookie == cookie)
                    .FirstOrDefault();

            if (record == null)
            {
                return new Result<AuthenticationRecord>().FailAndDefaultValue();
            }

            return new Result<AuthenticationRecord>().Succeed(record);
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