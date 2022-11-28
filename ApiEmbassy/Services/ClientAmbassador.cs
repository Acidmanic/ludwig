using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ApiEmbassy.Models;
using EnTier.Repositories;
using EnTier.Results;
using EnTier.UnitOfWork;
using Microsoft.AspNetCore.Http;

namespace ApiEmbassy.Services
{
    public class ClientAmbassador
    {
        private readonly ICrudRepository<RequestRecord, long> _requestsRepository;
        private readonly ICrudRepository<ResponseCarrier, long> _responsesRepository;


        public long Timeout { get; set; } = 10000; 
        
        public ClientAmbassador(IUnitOfWork unitOfWork)
        {
            _requestsRepository = unitOfWork.GetCrudRepository<RequestRecord, long>();
            
            _responsesRepository = unitOfWork.GetCrudRepository<ResponseCarrier, long>();
        }

        public async Task Communicate(HttpContext context)
        {
            var record = await TransmissionConvert.ToRequestRecord(context.Request);

            record = _requestsRepository.Add(record);
            
            var runner = new TimeoutRunner();

            var response = await runner
                .LoopWithTimeout(() => TryFindResponse(record), 30000);

            if (response)
            {
                await TransmissionConvert.IntoHttpContext(context, response);
                
                Console.WriteLine($"Record {record.Id} -> {record.RequestUri} has been Responded with Status {response.Value.StatusCode}" +
                                  $" and {response.Value.Content.Length} data");
            }
            else
            {
                _requestsRepository.Remove(record.Id);

                Console.WriteLine($"Record {record.Id} has been removed due timeout.");
                
                context.Response.StatusCode = StatusCodes.Status504GatewayTimeout;
            }
        }
        
        private Result<ResponseCarrier> TryFindResponse(RequestRecord record)
        {
            var response = _responsesRepository
                .Find(r => r.RequestId == record.Id)
                .FirstOrDefault();

            if (response != null)
            {
                _requestsRepository.Remove(record.Id);
                _responsesRepository.Remove(response.Id);

                return new Result<ResponseCarrier>(true, response);
            }

            return new Result<ResponseCarrier>().FailAndDefaultValue();
        }


        public void ReceiveResponse(ResponseCarrier response)
        {
            _responsesRepository.Add(response);
        }


        public IEnumerable<RequestRecord> GetRequests()
        {
            return _requestsRepository.All();
        }
        
        public RequestRecord RequestById(long id)
        {
            return _requestsRepository.GetById(id);
        }
        
        
        public IEnumerable<RequestRecord> Find(Expression<Func<RequestRecord,bool>> expression)
        {
            return _requestsRepository.Find(expression);
        }
    }
}