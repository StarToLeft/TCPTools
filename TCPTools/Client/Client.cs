using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using TCPController.Client.Data;
using TCPTools.Logging;
using TCPTools.Static;
using TCPTools.Structure;

namespace TCPTools.Client
{
    class Client
    {
        public Client(int port)
        {
            this.port = port;
        }

        public int port;

        public TcpClient client;
        public NetworkStream stream;

        public State state = new State();

        public DataHandler dataHandler = new DataHandler();

        public void Connect(IPEndPoint ipEndPoint = null)
        {
            if (ipEndPoint == null)
            {
                ipEndPoint = IP.GetLocalEndPoint(port);
            }

            client = new TcpClient(ipEndPoint.Address.ToString(), ipEndPoint.Port);
            stream = client.GetStream();

            client.SendBufferSize = State.bufferSize;
            client.ReceiveBufferSize = State.bufferSize;

            Logger.Info("Client: Connecting to server");

            stream.BeginRead(state.buffer, 0, client.SendBufferSize, new AsyncCallback(BeginReceive), state);
        }

        private void BeginReceive(IAsyncResult ar)
        {
            try
            {
                int bytesRead = stream.EndRead(ar);

                if (bytesRead > 0)
                {
                    String content = String.Empty;

                    state.sb.Append(Encoding.UTF8.GetString(state.buffer));
                    content = state.sb.ToString();

                    Logger.Log("Client: Found data");

                    if (content.Contains("<EOF>"))
                    {
                        content = content.Replace("<EOF>", "");

                        state.buffer = new Byte[State.bufferSize];
                        state.sb = new StringBuilder();

                        SocketData socketData = JsonConvert.DeserializeObject<SocketData>(content);

                        ReceivedData(socketData.O, content);
                    }
                }

                stream.BeginRead(state.buffer, 0, client.SendBufferSize, new AsyncCallback(BeginReceive), state);
            } catch
            {
                client.Close();
            }
        }

        private void ReceivedData(Opcode o, string content)
        {
            dataHandler.ReceivedData(o, content);
        }
    }
}
