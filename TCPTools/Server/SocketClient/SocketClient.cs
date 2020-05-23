using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Linq;

using TCPTools.Logging;
using TCPTools.Static;

using TCPTools.Structure;
using Newtonsoft.Json;
using TCPTools.Errors;

namespace TCPTools.Server.Client
{
    public class SocketClient
    {
        public SocketClient(string id, Socket socket)
        {
            this.id = id;
            this.socket = socket;
        }

        public string id;
        public Socket socket;

        public const int BufferSize = 1024;
        public byte[] buffer = new byte[BufferSize];

        public StringBuilder sb = new StringBuilder();

        public DateTimeOffset lastPing;

        public int pingMs = 5000;
        public int sendNumber = 0; // Number of sent and recieved data

        private byte[] endData = Encoding.UTF8.GetBytes("<EOF>");

        public bool SendData(SocketData obj)
        {
            byte[] byteData;

            byteData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(obj)); 

            IEnumerable<byte[]> dataArray = ByteHelper.Split(byteData, 1024).ToList();

            Console.WriteLine(Encoding.UTF8.GetString(byteData));

            dataArray.ToList().ForEach(x =>
            {
                SendData(x);
            });

            return true;
        }

        public bool SendData(byte[] data)
        {
            sendNumber++;

            try
            {
                if (data == null) return false;

                byte[] newData = ByteHelper.Combine(data, endData);

                socket.BeginSend(newData, 0, newData.Length, 0, new AsyncCallback(SendCallback), socket);

                Logger.Warn("Debugging SendData: Sending data");

                return true;
            } catch
            {
                sendNumber--;
                return false;
            }
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                Socket handler = (Socket)ar.AsyncState;
                int bytesSent = handler.EndSend(ar);

                // Trigger success event
                Logger.Warn("Debugging SendData: Sent data");
            }
            catch
            {
                sendNumber--;
                Logger.Error("Failed to send data");
            }
        }

        public void ReceivedHeartBeat(SocketData data)
        {
            var pingData = data as PingData;

            if (pingData.S != sendNumber)
            {
                SendData(new ErrorData(Error.IncorrectSendNumber));
            }
        }
    }
}
