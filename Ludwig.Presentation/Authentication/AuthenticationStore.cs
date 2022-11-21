using System;
using EnTier.DataAccess.InMemory;
using EnTier.Repositories;
using EnTier.Results;
using EnTier.UnitOfWork;
using Ludwig.Contracts.Authentication;
using Ludwig.Contracts.Models;

namespace Ludwig.Presentation.Authentication
{
    public class AuthenticationStore
    {

        private readonly ICrudRepository<AuthenticationRecord,string> _repository;
        private readonly IUnitOfWork _unitOfWork;
        public AuthenticationStore(IUnitOfWork unitOfWork)
        {
            _repository = unitOfWork.GetCrudRepository<AuthenticationRecord,string>();
            _unitOfWork = unitOfWork;
        }


        public AuthenticationRecord GenerateToken(string loginMethodName,AuthenticationResult result)
        {
            var token = Guid.NewGuid().ToString();

            var record = new AuthenticationRecord
            {
                Token = token,
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


        public Result<AuthenticationRecord> FindAuthenticatedLoginMethodName(string token)
        {
            var record = _repository.GetById(token);

            if (record == null)
            {
                return new Result<AuthenticationRecord>().FailAndDefaultValue();
            }

            return new Result<AuthenticationRecord>().Succeed(record);
        }
        
        
        public void RemoveAuthorization(string token)
        {
            var record = _repository.GetById(token);

            if (record != null)
            {
                _repository.Remove(token);
                
                _unitOfWork.Complete();
            }
        }
        
    }
}