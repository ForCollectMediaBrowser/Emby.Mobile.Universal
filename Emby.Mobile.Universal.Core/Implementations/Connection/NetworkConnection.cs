using System;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Connectivity;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using MediaBrowser.ApiInteraction;
using MediaBrowser.Model.ApiClient;

namespace Emby.Mobile.Universal.Core.Implementations.Connection
{
    public class NetworkConnection : INetworkConnection
    {
        public NetworkConnection()
        {
            System.Net.NetworkInformation.NetworkChange.NetworkAddressChanged += NetworkChange_NetworkAddressChanged;
        }

        void NetworkChange_NetworkAddressChanged(object sender, EventArgs e)
        {
            if (NetworkChanged != null)
                NetworkChanged(this, e);
        }

        public NetworkStatus GetNetworkStatus()
        {
            bool localNetworkDetected = false;
            bool remoteNetworkDetected = false;

            var lanIdentifiers = NetworkInformation.GetConnectionProfiles();
            foreach (var lanIdentifier in lanIdentifiers)
            {
                var level = lanIdentifier.GetNetworkConnectivityLevel();
                if (!remoteNetworkDetected)
                {
                    if (level == NetworkConnectivityLevel.InternetAccess)
                    {
                        remoteNetworkDetected = true;
                        localNetworkDetected = true;
                        break;
                    }
                }

                if (!localNetworkDetected && level == NetworkConnectivityLevel.LocalAccess)
                {
                    localNetworkDetected = true;
                }
            }

            if (!remoteNetworkDetected)
            {
                remoteNetworkDetected = NetworkInformation.GetInternetConnectionProfile() != null;
            }

            var status = new NetworkStatus();
            status.IsLocalNetworkAvailable = localNetworkDetected;
            status.IsNetworkAvailable = remoteNetworkDetected;
            status.IsNetworkAvailable = localNetworkDetected;

            return status;
        }

        public async Task SendWakeOnLan(string macAddress, System.Threading.CancellationToken cancellationToken)
        {
            DatagramSocket socket = new DatagramSocket();

            await socket.BindServiceNameAsync("0");

            IOutputStream o = await socket.GetOutputStreamAsync(new HostName("255.255.255.255"), "7");
            DataWriter writer = new DataWriter(o);
            writer.WriteBytes(GetBuffer(macAddress));
            await writer.StoreAsync();

            socket.Dispose();

        }

        public async Task SendWakeOnLan(string macAddress, int port, System.Threading.CancellationToken cancellationToken)
        {
            DatagramSocket socket = new DatagramSocket();

            await socket.BindServiceNameAsync("0");

            IOutputStream o = await socket.GetOutputStreamAsync(new HostName("255.255.255.255"), port.ToString());
            DataWriter writer = new DataWriter(o);
            writer.WriteBytes(GetBuffer(macAddress));
            await writer.StoreAsync();

            socket.Dispose();
        }

        public async Task SendWakeOnLan(string macAddress, string ipAddress, int port, System.Threading.CancellationToken cancellationToken)
        {
            DatagramSocket socket = new DatagramSocket();

            await socket.BindServiceNameAsync("0");

            IOutputStream o = await socket.GetOutputStreamAsync(new HostName(ipAddress), port.ToString());
            DataWriter writer = new DataWriter(o);
            writer.WriteBytes(GetBuffer(macAddress));
            await writer.StoreAsync();

            socket.Dispose();
        }

        private Byte[] GetBuffer(string macAddress)
        {
            var buf = new char[102];
            Byte[] sendBytes = Encoding.UTF8.GetBytes(buf);

            for (int x = 0; x < 6; x++)
            {
                sendBytes[x] = 0xff;
            }

            string[] macDigits = null;
            if (macAddress.Contains("-"))
            {
                macDigits = macAddress.Split('-');
            }
            else
            {
                macDigits = macAddress.Split(':');
            }

            int start = 6;
            for (int i = 0; i < 16; i++)
            {
                for (int x = 0; x < 6; x++)
                {
                    sendBytes[start + i * 6 + x] = (byte)Convert.ToInt32(macDigits[x], 16);
                }
            }

            return sendBytes;
        }


        public event EventHandler<EventArgs> NetworkChanged;
    }
}