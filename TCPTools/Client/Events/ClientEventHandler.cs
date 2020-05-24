using System;
using System.Collections.Generic;
using System.Text;
using TCPTools.Server.Client;
using TCPTools.Structure;

namespace TCPTools.Client.Events
{
    public class ClientEventHandler
    {
        public event EventHandler<ClientSocketConnectedEventArgs> OnSocketConnected;
        public event EventHandler<ClientSocketDisconnectedEventArgs> OnSocketDisconnected;
        public event EventHandler<ClientSocketReceivedDataEventArgs> OnSocketReceivedData;


        public void TriggerSocketConnectedEvent()
        {
            var handler = OnSocketConnected;
            ClientSocketConnectedEventArgs eventArgs = new ClientSocketConnectedEventArgs(DateTimeOffset.Now);

            handler?.Invoke(this, eventArgs);
        }

        public void TriggerSocketDisconnectedEvent()
        {
            var handler = OnSocketDisconnected;
            ClientSocketDisconnectedEventArgs eventArgs = new ClientSocketDisconnectedEventArgs(DateTimeOffset.Now);

            handler?.Invoke(this, eventArgs);
        }

        public void TriggerSocketReceivedDataEvent(SocketData data, string content)
        {
            var handler = OnSocketReceivedData;
            ClientSocketReceivedDataEventArgs eventArgs = new ClientSocketReceivedDataEventArgs(data, content);

            handler?.Invoke(this, eventArgs);
        }
    }
}
