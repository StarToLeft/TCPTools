using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using TCPTools.Client.Data;
using TCPTools.Client.Events;
using TCPTools.Logging;
using TCPTools.Static;
using TCPTools.Structure;

namespace TCPTools.Client
{
    public class Client
    {
        public Client(int port)
        {
            this.port = port;
        }

        public int port;
        public bool isConnected;

        public HelloData helloData;

        // Connection
        public TcpClient tcpClient;
        public NetworkStream stream;

        // Data-handler, handles what data should be triggered to events 
        public ClientDataHandler dataHandler = new ClientDataHandler();
        public ClientEventHandler eventHandler = new ClientEventHandler();

        // The current socket state
        public State socket;

        private int _amountOfRetries = 0;

        public void Connect(IPEndPoint ipEndPoint = null, int maxRetries = 10)
        {
            var notConnected = true;

            while (notConnected)
            {
                if (_amountOfRetries >= maxRetries) return;

                _amountOfRetries++;

                try
                {
                    if (ipEndPoint == null)
                    {
                        ipEndPoint = IP.GetLocalEndPoint(port);
                    }

                    tcpClient = new TcpClient(ipEndPoint.Address.ToString(), ipEndPoint.Port);
                    stream = tcpClient.GetStream();

                    socket = new State(this);

                    tcpClient.SendBufferSize = State.BufferSize;
                    tcpClient.ReceiveBufferSize = State.BufferSize;

                    stream.BeginRead(socket.buffer, 0, tcpClient.SendBufferSize, BeginReceive, socket);
                    eventHandler.TriggerSocketConnectedEvent();
                    notConnected = false;
                    isConnected = true;
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                    stream = null;
                    tcpClient = null;
                    socket = null;
                }
            }

            isConnected = false;
        }

        private void BeginReceive(IAsyncResult ar)
        {
            try
            {
                var bytesRead = stream.EndRead(ar);

                if (bytesRead > 0)
                {
                    socket.sb.Append(Encoding.UTF8.GetString(socket.buffer));
                    var content = socket.sb.ToString();

                    if (content.Contains("<EOF>"))
                    {
                        content = content.Replace("<EOF>", "");

                        socket.buffer = new byte[State.BufferSize];
                        socket.sb = new StringBuilder();

                        SocketData socketData = JsonConvert.DeserializeObject<SocketData>(content);
                        if (socketData.S > socket.sequence)
                            socket.sequence = socketData.S;

                        ReceivedData(socketData.O, content);
                        eventHandler.TriggerSocketReceivedDataEvent(socketData, content);
                    }
                }

                stream.BeginRead(socket.buffer, 0, tcpClient.SendBufferSize, BeginReceive, socket);
            } catch (Exception ex)
            {
                Logger.Error("Begin receive error: " + ex);
                tcpClient.Close();
                eventHandler.TriggerSocketDisconnectedEvent();
            }
        }

        private void ReceivedData(Opcode o, string content)
        {
            dataHandler.ReceivedData(o, content, this);
        }
    }
}
