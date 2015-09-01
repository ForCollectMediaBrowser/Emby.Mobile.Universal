using System;
using System.Threading.Tasks;
using MediaBrowser.ApiInteraction;
using MediaBrowser.Model.ApiClient;

namespace Emby.Mobile.Universal.Core.NullServices
{
    public class NullCredentialProvider : ICredentialProvider
    {
        public Task<ServerCredentials> GetServerCredentials()
        {
            throw new NotImplementedException();
        }

        public Task SaveServerCredentials(ServerCredentials configuration)
        {
            throw new NotImplementedException();
        }
    }
}
