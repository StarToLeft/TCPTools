using System;
using TCPTools.Server.Client;
using TCPTools.Structure;

namespace TCPTools.Server.Events
{
    public class EventHandler
    {
        public EventHandler()
        {

        }

        public event EventHandler<SocketConnectedEventArgs> OnSocketConnected;
        public event EventHandler<SocketDisconnectedEventArgs> OnSocketDisconnected;
        public event EventHandler<SocketReceivedDataEventArgs> OnSocketReceviedData;


        public void TriggerSocketConnectedEvent(SocketClient client)
        {
            EventHandler<SocketConnectedEventArgs> handler = OnSocketConnected;
            SocketConnectedEventArgs eventArgs = new SocketConnectedEventArgs(client, new DateTimeOffset());

            if (handler != null)
            {
                handler.Invoke(this, eventArgs);
            }
        }

        public void TriggerSocketDisconnectedEvent(SocketClient client)
        {
            EventHandler<SocketDisconnectedEventArgs> handler = OnSocketDisconnected;
            SocketDisconnectedEventArgs eventArgs = new SocketDisconnectedEventArgs(client, new DateTimeOffset());
            
            if (handler != null)
            {
                handler.Invoke(this, eventArgs);
            }
        }

        public void TriggerSocketReceivedDataEvent(SocketClient client, SocketData data)
        {
            EventHandler<SocketReceivedDataEventArgs> handler = OnSocketReceviedData;
            SocketReceivedDataEventArgs eventArgs = new SocketReceivedDataEventArgs(client, data);

            if (data.O == Static.Opcode.Ping)
            {
                client.ReceivedHeartBeat(data);
            }
            
            if (handler != null)
            {
                handler.Invoke(this, eventArgs);
            }
        }
    }
}
