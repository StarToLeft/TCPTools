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
        public SocketReceivedDataEventArgs(SocketClient _client, SocketData _socketData, string _data)
        {
            client = _client;
            socketData = _socketData;
            data = _data;
        }

        public SocketClient client { get; set; }
        public SocketData socketData { get; set; }
        public string data { get; set; }
    }
}
