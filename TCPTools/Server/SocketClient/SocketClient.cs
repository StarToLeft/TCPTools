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
        public int pingMsClient = 2000;
        public int pingMsServer = 3000;
        
        public int sequence;

        // To append on last line
        private readonly byte[] _endData = Encoding.UTF8.GetBytes("<EOF>");

        public void SendData(SocketData obj)
        {
            obj.S = sequence++;

            var byteData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(obj));
            SendData(byteData, true);
        }

        public void SendData(byte[] data, bool didUpdateSequence = false)
        {
            try
            {
                IEnumerable<byte[]> dataArray = data.Split(1024).ToList();

                var dataArrayChunks = dataArray.ToList();

                for (var i = 0; i < dataArrayChunks.Count; i++)
                {
                    try
                    {
                        var dataToWrite = dataArrayChunks[i];
                        if (i == dataArrayChunks.Count - 1) dataToWrite = ByteHelper.Combine(dataToWrite, _endData);

                        if (socket.Connected)
                            socket.BeginSend(dataToWrite, 0, dataToWrite.Length, 0, SendCallback, socket);
                        else throw new Exception("Not connected");
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ex);

                        if (didUpdateSequence)
                            sequence--;
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);

                if (didUpdateSequence)
                    sequence--;
            }
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                Socket handler = (Socket)ar.AsyncState;
                var bytesSent = handler.EndSend(ar);

                // Trigger success event
            }
            catch (Exception ex)
            {
                sequence--;
                Logger.Error(ex);
            }
        }
    }
}
