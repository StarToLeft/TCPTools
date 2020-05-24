using System;
using System.Linq;

using Newtonsoft.Json;

using TCPTools.Client.Data;
using TCPTools.Logging;
using TCPTools.Server.Client;
using TCPTools.Static;
using TCPTools.Structure;

namespace TCPTools.Server.Data.DataHandlers
{
    internal class Ping
    {
        [OnServerData(Opcode.Ping)]
        public static void OnData(string content, Server server, string id)
        {
            try
            {
                PingData data = JsonConvert.DeserializeObject<PingData>(content);
                SocketClient socketClient = server.connectedSockets.FirstOrDefault(x => x.id == id);

                if (socketClient == null) return;
                socketClient.lastPing = new DateTimeOffset(DateTime.Now);

                socketClient.SendData(new PongData());
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }
    }
}
