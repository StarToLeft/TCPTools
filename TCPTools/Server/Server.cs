using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using TCPTools.Server.Client;
using TCPTools.Logging;
using TCPTools.Server.Events;
using TCPTools.Structure;
using Newtonsoft.Json;
using TCPTools.Client.Data;
using TCPTools.Static;

// REWRITE WITH THIS https://www.codeproject.com/Articles/1243360/TCP-Socket-Off-the-shelf-Revisited-with-Async-Awai

namespace TCPTools.Server
{
    public class Server
    {
        public static ManualResetEvent allDone = new ManualResetEvent(false);

        public int port;
        public Socket listener;
        public IPEndPoint localEndPoint;

        public List<SocketClient> connectedSockets = new List<SocketClient>();

        private static int bufferSize = 1024;

        public Events.EventHandler eventHandler;

        public ServerDataHandler dataHandler = new ServerDataHandler();

        public Server()
        {
            eventHandler = new Events.EventHandler();
        }


        public void CreateServer(int port)
        {
            this.port = port;

            connectedSockets = new List<SocketClient>();

            (Socket socket, IPEndPoint ipEndPoint) = CreateSocket();
            listener = socket;
            localEndPoint = ipEndPoint;

            StartSocketServer();
        }
        public (Socket, IPEndPoint) CreateSocket()
        {
            // Get starting ip address and port
            IPAddress ipAddress = IP.GetIp(port);
            IPEndPoint ipEndPoint = IP.GetLocalEndPoint(port);

            Logger.Info("Server address set to {0}:{1}", ipEndPoint.Address, ipEndPoint.Port);

            // Create the TCP/IP socket.
            Socket socket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            return (socket, ipEndPoint);
        }

        public void StartSocketServer()
        {
            try
            {
                listener.Bind(localEndPoint);
                // Start listening, also assign a backlog (a queue), max 128 people can sit in the queue to be accepted a connection.
                listener.Listen(128);

                while (true)
                {
                    allDone.Reset();

                    Logger.Warn("Listening for new connections");
                    // Start an asynchronous socket to listen for connections.
                    listener.BeginAccept(
                        new AsyncCallback(AcceptCallback),
                        listener);

                    // Wait until a connection is made before continuing.
                    allDone.WaitOne();
                }
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
            }
        }

        public void AcceptCallback(IAsyncResult ar)
        {
            try
            {
                // Resume the main accept connection thread, accept new connections as this one has been accepted.
                allDone.Set();

                // Get the socket that handles the tcpClient request.  
                Socket asyncState = (Socket)ar.AsyncState;
                Socket handler = asyncState.EndAccept(ar);

                // Create a SocketClient object, contains main state for all connected clients.
                SocketClient socketClient = new SocketClient(Guid.NewGuid().ToString("N"), handler);
                connectedSockets.Add(socketClient);

                Logger.Info("Connecting a new tcpClient");
                // Trigger a accepted a new connection event.

                handler.BeginReceive(socketClient.buffer, 0, bufferSize, 0, new AsyncCallback(ReadCallback), socketClient);

                HelloData helloData = new HelloData(socketClient.pingMsClient, socketClient.sequence);
                socketClient.SendData(helloData);

                eventHandler.TriggerSocketConnectedEvent(socketClient);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        public void ReadCallback(IAsyncResult ar)
        {
            try
            {
                SocketClient socketClient = (SocketClient)ar.AsyncState;
                socketClient = connectedSockets.Find(x => x.id == socketClient.id);

                // Read data from the tcpClient socket.
                var bytesRead = socketClient.socket.EndReceive(ar);
                if (bytesRead <= 0) return;

                // Append current data, as more data might be sent
                socketClient.sb.Append(Encoding.UTF8.GetString(socketClient.buffer, 0, bytesRead));

                var content = socketClient.sb.ToString();

                // Check for end of data tag
                if (content.Contains("<EOF>"))
                {
                    // Remove end of line from data
                    content = content.Replace("<EOF>", "");

                    // Reset the data from memory, get ready for new data
                    socketClient.sb = new StringBuilder();
                    socketClient.buffer = new byte[socketClient.buffer.Length];

                    SocketData data = JsonConvert.DeserializeObject<SocketData>(content);

                    dataHandler.ReceivedData(data.O, content, this, socketClient.id);
                    eventHandler.TriggerSocketReceivedDataEvent(socketClient, data, content);
                }

                // Listen for new data
                socketClient.socket.BeginReceive(socketClient.buffer, 0, bufferSize, 0, new AsyncCallback(ReadCallback), socketClient);
            } catch (Exception ex)
            {
                if (!ex.Message.Contains("forcibly closed by the remote host"))
                    Logger.Error(ex);

                // If it fails to read, disconnect the tcpClient (this might be a bottle neck in the future, hmm)
                SocketClient socketClient = (SocketClient)ar.AsyncState;
                connectedSockets.RemoveAt(connectedSockets.FindIndex(x => x.id == socketClient.id));
                socketClient.socket.Shutdown(SocketShutdown.Both);
                socketClient.socket.Close();

                eventHandler.TriggerSocketDisconnectedEvent(socketClient);
            }
        }
    }
}