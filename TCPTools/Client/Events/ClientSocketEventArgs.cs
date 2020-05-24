using System;

using TCPTools.Server.Client;
using TCPTools.Structure;

namespace TCPTools.Client.Events
{
    public class ClientSocketConnectedEventArgs : EventArgs
    {
        public ClientSocketConnectedEventArgs(DateTimeOffset _eventDate)
        {
            connectionTimestamp = _eventDate;
        }

        public DateTimeOffset connectionTimestamp;
    }

    public class ClientSocketDisconnectedEventArgs : EventArgs
    {
        public ClientSocketDisconnectedEventArgs(DateTimeOffset _eventDate)
        {
            eventTimestamp = _eventDate;
        }

        public DateTimeOffset eventTimestamp;
    }

    public class ClientSocketReceivedDataEventArgs : EventArgs
    {
        public ClientSocketReceivedDataEventArgs(SocketData _socketData, string _data)
        {
            socketData = _socketData;
            data = _data;
        }

        public SocketData socketData { get; set; }
        public string data { get; set; }
    }
}
