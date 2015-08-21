using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cimbalino.Toolkit.Services;
using MediaBrowser.ApiInteraction;
using MediaBrowser.Model.ApiClient;
using MediaBrowser.Model.Logging;
using Newtonsoft.Json;

namespace Emby.Mobile.Universal.Core.Implementations.Connection
{
    public class CredentialProvider : ICredentialProvider
    {
        private readonly ILogger _log;
        private const string ServerCredentialSettings = "ServerCredentialSettings.json";

        private readonly IStorageServiceHandler _storageService = new StorageService().Local;

        public CredentialProvider(ILogger log)
        {
            _log = log;
        }

        public async Task<ServerCredentials> GetServerCredentials()
        {
            if (!await _storageService.FileExistsAsync(ServerCredentialSettings))
            {
                return new ServerCredentials();
            }

            var json = await _storageService.ReadAllTextAsync(ServerCredentialSettings);

            var credentials = JsonConvert.DeserializeObject<ServerCredentials>(json);

            Debug.WriteLine("GetCreds, Server count: " + (credentials != null && credentials.Servers != null ? credentials.Servers.Count : 0));

            return credentials ?? new ServerCredentials();
        }

        private static readonly SemaphoreSlim Lock = new SemaphoreSlim(1);

        public async Task SaveServerCredentials(ServerCredentials configuration)
        {
            await SaveServerCreds(configuration);
        }

        private async Task<bool> SaveServerCreds(ServerCredentials configuration)
        {
            var tsc = new TaskCompletionSource<bool>();
            //Deployment.Current.Dispatcher.BeginInvoke(async () =>
            //{
                await Lock.WaitAsync();

                try
                {
                    var json = JsonConvert.SerializeObject(configuration);

                    await _storageService.WriteAllTextAsync(ServerCredentialSettings, json).ConfigureAwait(false);
                }
                finally
                {
                    Lock.Release();
                }

                Debug.WriteLine("SaveCreds, Server count: " + (configuration != null && configuration.Servers != null ? configuration.Servers.Count : 0));

                tsc.SetResult(true);
            //});

            return await tsc.Task;
        }

        public async Task RemoveServer(ServerInfo server)
        {
            var creds = await GetServerCredentials();

            var serverExists = creds.Servers.FirstOrDefault(x => x.Id == server.Id);
            if (serverExists != null)
            {
                creds.Servers.Remove(serverExists);
                await SaveServerCredentials(creds);
            }
        }
    }
}