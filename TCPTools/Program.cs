using System;
using TCPTools.Server.Events;
using TCPTools.Server;
using TCPTools.Logging;
using System.Threading;
using TCPTools.Client;

namespace Example
{
    class Program
    {
        public static Server server;

        static void Main(string[] args)
        {
            Thread server = new Thread(StartServer);
            server.Start();

            Thread.Sleep(2000);

            Thread client = new Thread(StartClient);
            client.Start();
        }

        static public void StartServer()
        {
            // Start the main server
            server = new Server();

            Logger.Info("Starting server");

            server.eventHandler.OnSocketConnected += OnSocketConnected;
            server.eventHandler.OnSocketDisconnected += OnSocketDisconnected;
            server.eventHandler.OnSocketReceviedData += OnSocketRecievedData;

            // Start the server
            server.CreateServer(36674);
        }

        static public void StartClient()
        {
            Client client = new Client(36674);

            client.Connect();
        }

        static public void OnSocketConnected(object sender, EventArgs e)
        {
            SocketConnectedEventArgs eventData = (SocketConnectedEventArgs) e;

            Logger.Log("Recieved a connection with id: " + eventData.client.id);
        }

        static public void OnSocketDisconnected(object sender, EventArgs e) 
        {
            SocketDisconnectedEventArgs eventData = (SocketDisconnectedEventArgs)e;

            Logger.Warn("Disconnected a client with id: " + eventData.client.id);
        }

        static public void OnSocketRecievedData(object sender, EventArgs e)
        {
            SocketReceivedDataEventArgs eventData = (SocketReceivedDataEventArgs)e;

            Logger.Warn("Recieved data: " + eventData.data);
        }
    }
}
