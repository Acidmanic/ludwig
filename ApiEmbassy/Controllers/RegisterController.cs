using ApiEmbassy.Extensions;
using ApiEmbassy.Models;
using ApiEmbassy.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiEmbassy.Controllers
{
    [ApiController]
    [Route("register-configuration")]
    public class RegisterController : ControllerBase
    {
        private readonly ClientAmbassador _ambassador;

        public RegisterController(Embassy embassy)
        {
            _ambassador = embassy.GetAmbassador();
        }

        [HttpGet]
        [Route("request/{embassyId}")]
        public IActionResult GetRequestsById(string embassyId)
        {
            return Ok(new RequestList
            {
                Requests = _ambassador
                    .Find(r => r.EmbassyId.AreEqualAsKeys(embassyId))
            });
        }

        [HttpGet]
        [Route("request")]
        public IActionResult GetAllRequests()
        {
            return Ok(new RequestList()
            {
                Requests = _ambassador.GetRequests()
            });
        }

        [HttpPost]
        [Route("request")]
        public IActionResult ApplyResponse(ResponseCarrier carrier)
        {
            _ambassador.ReceiveResponse(carrier);

            return Ok();
        }
    }
}