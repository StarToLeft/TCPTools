using System;
using System.Collections.Generic;
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
using TCPTools.Static;

// REWRITE WITH THIS https://www.codeproject.com/Articles/1243360/TCP-Socket-Off-the-shelf-Revisited-with-Async-Awai

namespace TCPTools.Server
{
    public class Server
    {
        public static ManualResetEvent allDone = new ManualResetEvent(false);

        // Options
        public int port;
        public Socket listener;
        public IPEndPoint localEndPoint;

        public List<SocketClient> connectedSockets = new List<SocketClient>();

        private static int bufferSize = 1024;

        public Events.EventHandler eventHandler;

        public Server()
        {
            eventHandler = new Events.EventHandler();
        }

        public void CreateServer(int port)
        {
            this.port = port;

            connectedSockets = new List<SocketClient>();

            (Socket, IPEndPoint) socketData = CreateSocket();
            listener = socketData.Item1;
            localEndPoint = socketData.Item2;

            StartSocketServer();
        }

        public (Socket, IPEndPoint) CreateSocket()
        {
            // Get starting ip address and port
            IPAddress ipAddress = IP.GetIp(port);
            IPEndPoint localEndPoint = IP.GetLocalEndPoint(port);

            Logger.Info("Server adress set to {0}:{1}", localEndPoint.Address, localEndPoint.Port);

            // Create the TCP/IP socket.  
            Socket listener = new Socket(ipAddress.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);

            return (listener, localEndPoint);
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

                    // Start an asynchronous socket to listen for connections.
                    Logger.Info("Waiting for a connection...");
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
            // Unpause the main accept connection thread, accept new connections as this one has been accepted.
            allDone.Set();

            // Get the socket that handles the client request.  
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);

            // Create a SocketClient objject, contains main state for all connected clients.
            SocketClient socketClient = new SocketClient(Guid.NewGuid().ToString("N"), handler);
            connectedSockets.Add(socketClient);

            Logger.Info("Connecting a new client");
            // Trigger a accepted a new connection event.
            eventHandler.TriggerSocketConnectedEvent(socketClient);

            PingData pingData = new PingData(5000, 0);
            socketClient.SendData(pingData);

            handler.BeginReceive(socketClient.buffer, 0, bufferSize, 0, new AsyncCallback(ReadCallback), socketClient);
        }

        public void ReadCallback(IAsyncResult ar)
        {
            try
            {
                String content = String.Empty;

                SocketClient socketClient = (SocketClient)ar.AsyncState;
                socketClient = connectedSockets.Find(x => x.id == socketClient.id);

                // Read data from the client socket.
                int bytesRead = socketClient.socket.EndReceive(ar);
                if (bytesRead > 0)
                {
                    // Append current data, as more data might be sent
                    socketClient.sb.Append(Encoding.ASCII.GetString(socketClient.buffer, 0, bytesRead));

                    content = socketClient.sb.ToString();

                    // Check for end of data tag
                    if (content.EndsWith("<EOF>"))
                    {
                        // Remove end of line from data
                        content = content.Replace("<EOF>", "");

                        // Reset the data from memory, get ready for new data
                        socketClient.sb = new StringBuilder();
                        socketClient.buffer = new Byte[socketClient.buffer.Length];

                        Console.WriteLine(content + " with EOF");

                        SocketData data = JsonConvert.DeserializeObject<SocketData>(content);

                        eventHandler.TriggerSocketReceivedDataEvent(socketClient, data);

                        // Start listening for new data to come
                        socketClient.socket.BeginReceive(socketClient.buffer, 0, bufferSize, 0, new AsyncCallback(ReadCallback), socketClient);
                    }
                    else
                    {
                        // Request more data if End Of Line was not found <EOF>
                        socketClient.socket.BeginReceive(socketClient.buffer, 0, bufferSize, 0, new AsyncCallback(ReadCallback), socketClient);
                    }
                }
            } catch (Exception ex)
            {
                Logger.Error(ex);

                // If it fails to read, disconnect the client (this might be a bottle neck in the future, hmm)
                SocketClient socketClient = (SocketClient)ar.AsyncState;
                connectedSockets.RemoveAt(connectedSockets.FindIndex(x => x.id == socketClient.id));
                socketClient.socket.Shutdown(SocketShutdown.Both);
                socketClient.socket.Close();

                eventHandler.TriggerSocketDisconnectedEvent(socketClient);
            }
        }
    }
}