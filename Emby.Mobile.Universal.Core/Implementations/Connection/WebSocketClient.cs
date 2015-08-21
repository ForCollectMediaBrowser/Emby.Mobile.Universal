using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Networking.Sockets;
using MediaBrowser.Model.ApiClient;
using MediaBrowser.Model.Net;

namespace Emby.Mobile.Universal.Core.Implementations.Connection
{
    public class WebSocketClient : IClientWebSocket
    {
        /// <summary>
        ///     The _socket
        /// </summary>
        private MessageWebSocket _socket;

        /// <summary>
        ///     Gets or sets the state.
        /// </summary>
        /// <value>The state.</value>
        public WebSocketState State { get; private set; } = WebSocketState.None;

        /// <summary>
        ///     Gets or sets the receive action.
        /// </summary>
        /// <value>The receive action.</value>
        public Action<byte[]> OnReceiveBytes { get; set; }

        /// <summary>
        ///     Gets or sets the on receive.
        /// </summary>
        /// <value>The on receive.</value>
        public Action<string> OnReceive { get; set; }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (_socket != null)
            {
                _socket.Dispose();
                _socket = null;
                State = WebSocketState.None;
            }
        }

        /// <summary>
        ///     Occurs when [closed].
        /// </summary>
        public event EventHandler Closed;

        /// <summary>
        ///     Connects the async.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Task.</returns>
        public Task ConnectAsync(string url, CancellationToken cancellationToken)
        {
            var taskCompletionSource = new TaskCompletionSource<bool>();

            try
            {
                // Make a local copy to avoid races with the Closed event.
                //MessageWebSocket webSocket = _socket;
                // Have we connected yet?
                if (_socket == null)
                {

                    _socket = new MessageWebSocket();
                    _socket.Closed += SocketClosed;
                    _socket.MessageReceived += SocketMessageReceived;

                    State = WebSocketState.Connecting;

                    return Task.Factory.StartNew(async () =>
                    {
                        await _socket.ConnectAsync(new Uri(url));


                        State = WebSocketState.Open;
                    }, cancellationToken);
                }
            }
            catch (Exception ex)
            {
                _socket = null;
                taskCompletionSource.TrySetException(ex);
            }

            return taskCompletionSource.Task;
        }

        private void SocketClosed(IWebSocket sender, WebSocketClosedEventArgs args)
        {
            Closed?.Invoke(this, EventArgs.Empty);
        }

        private void SocketMessageReceived(MessageWebSocket sender, MessageWebSocketMessageReceivedEventArgs args)
        {
            if (OnReceive != null)
            {
                if (args.MessageType == SocketMessageType.Utf8)
                {
                    try
                    {
                        var reader = args.GetDataReader();
                        var message = reader.ReadString(reader.UnconsumedBufferLength);
                        OnReceive(message);
                    }
                    catch (Exception)
                    { }

                }
            }
        }

        /// <summary>
        ///     Sends the async.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="type">The type.</param>
        /// <param name="endOfMessage">if set to <c>true</c> [end of message].</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Task.</returns>
        public Task SendAsync(byte[] bytes, WebSocketMessageType type, bool endOfMessage, CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew(async () =>
            {
                try
                {
                    var output = _socket.OutputStream;
                    await output.WriteAsync(bytes.AsBuffer());
                }
                catch
                { }
            }, cancellationToken);
        }
    }
}
