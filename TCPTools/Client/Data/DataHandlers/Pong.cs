using System.Threading;

using Newtonsoft.Json;
using TCPTools.Logging;
using TCPTools.Static;
using TCPTools.Structure;

namespace TCPTools.Client.Data.DataHandlers
{
    class Pong
    {
        [OnClientData(Opcode.Pong)]
        public static void OnData(string content, Client client)
        {

        }
    }
}