using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ApiEmbassy.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ApiEmbassy.Services
{
    public class ServerDealer
    {
        private readonly string _remoteAddress;
        private readonly string _localAddress;
        private readonly ILogger _logger;
        private bool _keepRunning = false;

        public ServerDealer(IConfiguration configuration, ILogger logger)
        {
            _logger = logger;
            _remoteAddress = configuration["RemoteAddress"];
            _localAddress = configuration["LocalAddress"];
        }


        public Task StartAsync()
        {
            if (_keepRunning)
            {
                return Task.CompletedTask;
            }

            _logger.LogDebug("Started checking loop");

            _keepRunning = true;

            var runner = new TimeoutRunner();

            return runner.Loop(MainLoop, () => _keepRunning);
        }

        public void Stop()
        {
            _keepRunning = false;
        }



        private static bool _lastNoReceive = false;
        
        private void MainLoop()
        {
            var client = new ApiClient();

            var requests = client.Get<RequestList>(_remoteAddress, "register-configuration/request");

            if (requests)
            {

                if (!requests.Value.Requests.Any())
                {
                    if (!_lastNoReceive)
                    {
                        _logger.LogDebug("Received {Count} Requests", requests.Value.Requests.Count());    

                        _lastNoReceive = true;
                    }
                }
                else
                {
                    _lastNoReceive = false;
                }

                foreach (var request in requests.Value.Requests)
                {
                    _logger.LogDebug("Processing request for [{Uri}]", request.RequestUri);

                    var requestMessage = TransmissionConvert.ToRequestMessage(request);

                    var httpClient = new HttpClient { BaseAddress = new Uri(_localAddress, UriKind.Absolute) };

                    var responseMessage = httpClient.SendAsync(requestMessage).Result;

                    var carrier = TransmissionConvert.ToResponseCarrier(responseMessage).Result;

                    _logger.LogDebug("Got Response {StatusCode}, Content {Content} ",
                        responseMessage.StatusCode,
                        carrier.Content?.Length + "Bytes");

                    carrier.RequestId = request.Id;

                    var posted = client.Post(carrier, _remoteAddress, "register-configuration/request");

                    if (posted)
                    {
                        _logger.LogDebug("Response sent for {Uri}", request.RequestUri);
                    }
                    else
                    {
                        _logger.LogDebug("There was a problem sending response fo {Uri}", request.RequestUri);
                    }
                }
            }
            else
            {
                _logger.LogDebug("unable to receive requests");
            }
        }
    }
}