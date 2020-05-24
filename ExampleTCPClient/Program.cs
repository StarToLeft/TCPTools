using System;
using TCPTools.Client;
using TCPTools.Client.Events;

namespace ExampleTCPClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Client client = new Client(25565);

            client.eventHandler.OnSocketConnected += SocketConnected;
            client.eventHandler.OnSocketDisconnected += SocketDisconnected;
            client.eventHandler.OnSocketReceivedData += SocketReceivedData;

            client.Connect();
        }

        public static void SocketConnected(object sender, EventArgs e)
        {
            ClientSocketConnectedEventArgs eventArgs = (ClientSocketConnectedEventArgs)e;
            Console.WriteLine("Connected a client.");
        }

        public static void SocketDisconnected(object sender, EventArgs e)
        {
            ClientSocketDisconnectedEventArgs eventArgs = (ClientSocketDisconnectedEventArgs)e;
            Console.WriteLine("Disconnected a client.");
        }

        public static void SocketReceivedData(object sender, EventArgs e)
        {
            ClientSocketReceivedDataEventArgs eventArgs = (ClientSocketReceivedDataEventArgs)e;
            Console.WriteLine("Received data " + eventArgs.data);
        }
    }
}
