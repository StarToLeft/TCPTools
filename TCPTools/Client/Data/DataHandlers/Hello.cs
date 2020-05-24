using System.Threading;

using Newtonsoft.Json;
using TCPTools.Logging;
using TCPTools.Static;
using TCPTools.Structure;

namespace TCPTools.Client.Data.DataHandlers
{
    class Hello
    {
        [OnClientData(Opcode.Hello)]
        public static void OnData(string content, Client client)
        {
            client.helloData = JsonConvert.DeserializeObject<HelloData>(content);

            Thread pingThread = new Thread(() =>
            {
                PingThread(client);
            });

            pingThread.Start();
        }

        private static void PingThread(Client client)
        {
            while (client.tcpClient.Connected)
            {
                Thread.Sleep(client.helloData.D.PingMs);
                client.socket.SendData(new PingData());
            }
        }
    }
}
