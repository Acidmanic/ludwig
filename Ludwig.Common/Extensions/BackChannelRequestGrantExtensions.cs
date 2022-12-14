using Ludwig.Common.Download;
using Ludwig.Common.Rest;
using Ludwig.Contracts.Authentication;
using Ludwig.Contracts.Models;

namespace Ludwig.Common.Extensions
{
    public static class BackChannelRequestGrantExtensions
    {
        public static void GrantDownloaderAccess(this IBackChannelRequestGrant grant,
            PatientDownloader backChannelCarrier)
        {
            var updates = grant.GetGrantRequestUpdates();

            foreach (var update in updates)
            {
                if (update.IsHeader())
                {
                    backChannelCarrier.Headers.Add(update.Key, update.Value);
                }

                if (update.IsCookie())
                {
                    backChannelCarrier.InDirectCookies.Add(update.Key, update.Value);
                }
            }
        }

        public static PatientDownloader CreateGrantedDownloader(this IBackChannelRequestGrant grant)
        {
            var downloader = new PatientDownloader();

            GrantDownloaderAccess(grant, downloader);

            return downloader;
        }
        
        public static RestClient CreateGrantedRestClient(this IBackChannelRequestGrant grant)
        {
            var client = new RestClient();

            var updates = grant.GetGrantRequestUpdates();

            client.DefaultMetadata = new HttpMetadata();
            
            foreach (var update in updates)
            {
                if (update.IsHeader())
                {
                    client.DefaultMetadata.Headers.Add(update.Key, update.Value);
                }

                if (update.IsCookie())
                {
                    client.DefaultMetadata.Cookies.Add(update.Key, update.Value);
                }
            }
            
            return client;
        }
    }
}