using System;
using System.Threading;
using TCPTools.Logging;
using TCPTools.Server;
using TCPTools.Server.Events;

namespace ExampleTCP
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Server server = new Server();

                server.eventHandler.OnSocketConnected += SocketConnected;
                server.eventHandler.OnSocketDisconnected += SocketDisconnected;
                server.eventHandler.OnSocketReceivedData += SocketReceivedData;

                server.CreateServer(25565);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        public static void SocketConnected(object sender, EventArgs e)
        {
            SocketConnectedEventArgs eventArgs = (SocketConnectedEventArgs) e;
            Console.WriteLine("Connected a client.");
        }

        public static void SocketDisconnected(object sender, EventArgs e)
        {
            SocketDisconnectedEventArgs eventArgs = (SocketDisconnectedEventArgs)e;
            Console.WriteLine("Disconnected client.");
        }

        public static void SocketReceivedData(object sender, EventArgs e)
        {
            SocketReceivedDataEventArgs eventArgs = (SocketReceivedDataEventArgs)e;
            Console.WriteLine("Received data: " + eventArgs.data);
        }
    }
}
