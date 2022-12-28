using System;
using System.Collections.Generic;
using Acidmanic.Utilities.Results;
using EnTier.Mapper;
using EnTier.UnitOfWork;
using Ludwig.Contracts.Models;
using Ludwig.DataAccess.Contracts.Repositories;
using Ludwig.DataAccess.Models;

namespace Ludwig.Presentation.Authentication
{
    public class AuthenticationStore
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AuthenticationStore(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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

            record = FullTreeStore(record);

            return record;
        }


        public Result<AuthorizationRecord> IsTokenRegistered(string token)
        {
            var repository = _unitOfWork.GetCrudRepository<AuthorizationRecordDal, long>();

            if (repository is IAuthorizationRecordRepository authRecRepo)
            {
                var record = authRecRepo.ReadAuthorizationRecordByToken(token);

                if (record != null)
                {
                    var domain = _mapper.Map<AuthorizationRecord>(record);

                    return new Result<AuthorizationRecord>().Succeed(domain);
                }
            }

            return new Result<AuthorizationRecord>().FailAndDefaultValue();
        }

        public Result<AuthorizationRecord> IsCookieRegistered(string cookie)
        {
            var repository = _unitOfWork.GetCrudRepository<AuthorizationRecordDal, long>();

            if (repository is IAuthorizationRecordRepository authRecRepo)
            {
                var record = authRecRepo.ReadAuthorizationRecordByCookie(cookie);

                if (record != null)
                {
                    var domain = _mapper.Map<AuthorizationRecord>(record);

                    return new Result<AuthorizationRecord>().Succeed(domain);
                }
            }
            
            return new Result<AuthorizationRecord>().FailAndDefaultValue();
        }


        public void RemoveAuthorization(string token)
        {
            var foundRecord = IsTokenRegistered(token);

            if (foundRecord)
            {
                //TODO: delete all updates by recordId
                
                var repository = _unitOfWork.GetCrudRepository<AuthorizationRecordDal, long>();

                repository.Remove(foundRecord.Value.Id);

                _unitOfWork.Complete();
            }
        }


        public AuthorizationRecord FullTreeStore(AuthorizationRecord record)
        {
            var storage = _mapper.Map<AuthorizationRecordDal>(record);

            var repository = _unitOfWork.GetCrudRepository<AuthorizationRecordDal, long>();

            storage = repository.Add(storage);

            if (storage != null)
            {
                var upRepo = _unitOfWork.GetCrudRepository<RequestUpdateDal, long>();

                foreach (var update in record.BackChannelGrantAccessUpdates)
                {
                    var upStorage = _mapper.Map<RequestUpdateDal>(update);

                    upStorage.AuthorizationRecordId = storage.Id;

                    upRepo.Add(upStorage);
                }

                record.Id = storage.Id;
            }

            _unitOfWork.Complete();

            return record;
        }
    }
}