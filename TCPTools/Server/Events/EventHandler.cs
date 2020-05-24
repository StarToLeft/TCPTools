using System;
using TCPTools.Server.Client;
using TCPTools.Structure;

namespace TCPTools.Server.Events
{
    public class EventHandler
    {
        public event EventHandler<SocketConnectedEventArgs> OnSocketConnected;
        public event EventHandler<SocketDisconnectedEventArgs> OnSocketDisconnected;
        public event EventHandler<SocketReceivedDataEventArgs> OnSocketReceivedData;


        public void TriggerSocketConnectedEvent(SocketClient client)
        {
            var handler = OnSocketConnected;
            SocketConnectedEventArgs eventArgs = new SocketConnectedEventArgs(client, new DateTimeOffset());

            handler?.Invoke(this, eventArgs);
        }

        public void TriggerSocketDisconnectedEvent(SocketClient client)
        {
            var handler = OnSocketDisconnected;
            SocketDisconnectedEventArgs eventArgs = new SocketDisconnectedEventArgs(client, new DateTimeOffset());

            handler?.Invoke(this, eventArgs);
        }

        public void TriggerSocketReceivedDataEvent(SocketClient client, SocketData data, string content)
        {
            var handler = OnSocketReceivedData;
            SocketReceivedDataEventArgs eventArgs = new SocketReceivedDataEventArgs(client, data, content);

            handler?.Invoke(this, eventArgs);
        }
    }
}
