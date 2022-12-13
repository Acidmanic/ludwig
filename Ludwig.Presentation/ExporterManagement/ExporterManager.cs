using System.Collections.Generic;
using System.Linq;
using Ludwig.Contracts;
using Ludwig.Contracts.Authentication;
using Ludwig.Presentation.Extensions;
using Microsoft.AspNetCore.Http;

namespace Ludwig.Presentation.ExporterManagement
{
    public class ExporterManager
    {
        private readonly IHttpContextAccessor _httpContextAccessor;


        private readonly Dictionary<string, List<IExporter>> _exporterSetByLoginMethodName =
            new Dictionary<string, List<IExporter>>();

        public ExporterManager(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }


        public void Register(IAuthenticator authenticator, IExporter exporter)
        {
            var loginMethodName = authenticator.LoginMethod.Name;

            if (!_exporterSetByLoginMethodName.ContainsKey(loginMethodName))
            {
                _exporterSetByLoginMethodName.Add(loginMethodName, new List<IExporter>());
            }

            var set = _exporterSetByLoginMethodName[loginMethodName];

            if (set.All(e => e.Id.UniqueKey != exporter.Id.UniqueKey))
            {
                set.Add(exporter);
            }
            else
            {
                //TODO: log
            }
        }


        public List<IExporter> Exporters
        {
            get
            {
                var loginMethodName = _httpContextAccessor.GetLoginMethodNameClaim();

                if (!string.IsNullOrWhiteSpace(loginMethodName))
                {
                    if (_exporterSetByLoginMethodName.ContainsKey(loginMethodName))
                    {
                        return _exporterSetByLoginMethodName[loginMethodName];
                    }
                }

                return new List<IExporter>();
            }
        }
    }
}