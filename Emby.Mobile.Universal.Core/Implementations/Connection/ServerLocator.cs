using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Connectivity;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using MediaBrowser.ApiInteraction;
using MediaBrowser.Model.ApiClient;
using Newtonsoft.Json;

namespace Emby.Mobile.Universal.Core.Implementations.Connection
{
    public class ServerLocator : IServerLocator
    {
        public async Task<List<ServerDiscoveryInfo>> FindServers(int timeoutMs, System.Threading.CancellationToken cancellationToken)
        {
            var result = new List<ServerDiscoveryInfo>();
            var tcs = new TaskCompletionSource<List<ServerDiscoveryInfo>>();

            var ct = new CancellationTokenSource(timeoutMs);
            ct.Token.Register(() =>
            {
                tcs.SetResult(result);
            }, useSynchronizationContext: false);

            var socket = new DatagramSocket();
            socket.MessageReceived += (sender, args) =>
            {
                try
                {
                    var reader = args.GetDataReader();

                    uint stringLength = args.GetDataReader().UnconsumedBufferLength;
                    var rawData = args.GetDataReader().ReadString(stringLength);

                    var server = JsonConvert.DeserializeObject<ServerDiscoveryInfo>(rawData);

                    server.EndpointAddress = string.Format("{0}:{1}", args.RemoteAddress, args.RemotePort);
                    result.Add(server);
                }
                catch
                {
                    tcs.SetResult(result);
                }
            };

            // retrieve the best network adapter to use            
            ConnectionProfile connectionProfile = NetworkInformation.GetInternetConnectionProfile();
            if (connectionProfile != null)
            {
                await socket.BindServiceNameAsync("", connectionProfile.NetworkAdapter);
                IOutputStream o = null;

                try
                {
                    o = await socket.GetOutputStreamAsync(new HostName("255.255.255.255"), "7359");
                    DataWriter writer = new DataWriter(o);
                    writer.WriteString("who is MediaBrowserServer_v2?");
                    await writer.StoreAsync();
                }
                catch
                {
                    tcs.SetResult(result);
                }
            }
            return await tcs.Task;
        }
    }
}