using System;

using TCPTools.Server.Client;
using TCPTools.Structure;

namespace TCPTools.Server.Events
{
    public class SocketConnectedEventArgs : EventArgs
    {
        public SocketConnectedEventArgs(SocketClient _client, DateTimeOffset _eventDate)
        {
            client = _client;
            connectionTimestamp = _eventDate;
        }

        public SocketClient client;
        public DateTimeOffset connectionTimestamp;
    }

    public class SocketDisconnectedEventArgs : EventArgs
    {
        public SocketDisconnectedEventArgs(SocketClient _client, DateTimeOffset _eventDate)
        {
            client = _client;
            eventTimestamp = _eventDate;
        }

        public SocketClient client;
        public DateTimeOffset eventTimestamp;
    }

    public class SocketReceivedDataEventArgs : EventArgs
    {
        public SocketReceivedDataEventArgs(SocketClient _client, SocketData _data)
        {
            client = _client;
            data = _data;
        }

        public SocketClient client;
        public SocketData data { get; set; }
    }
}
