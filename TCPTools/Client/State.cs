
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TCPTools.Logging;
using TCPTools.Static;
using TCPTools.Structure;

namespace TCPTools.Client
{
    public class State
    {
        public State(Client client)
        {
            this.client = client;
        }

        public Client client;

        public const int BufferSize = 1024;
        public byte[] buffer = new Byte[BufferSize];

        public int sequence = 0;

        public StringBuilder sb = new StringBuilder();

        private readonly byte[] _endData = Encoding.UTF8.GetBytes("<EOF>");

        public bool SendData(SocketData obj)
        {
            var byteData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(obj));
            SendData(byteData);

            return true;
        }

        public bool SendData(byte[] data)
        {
            try
            {
                if (data == null) return false;

                IEnumerable<byte[]> dataArray = data.Split(1024).ToList();

                var dataArrayChunks = dataArray.ToList();

                for (var i = 0; i < dataArrayChunks.Count; i++)
                {
                    var dataToWrite = dataArrayChunks[i];
                    if (i == dataArrayChunks.Count-1) dataToWrite = ByteHelper.Combine(dataToWrite, _endData);

                    client.stream.BeginWrite(dataToWrite, 0, dataToWrite.Length, SendCallback, client.stream);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                NetworkStream handler = (NetworkStream)ar.AsyncState;
                handler.EndWrite(ar);

                // Trigger success event
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }
    }
}
