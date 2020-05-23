using System;
using TCPTools.Static;

namespace TCPTools.Structure
{
    class PingDataData
    {
        public int pingMs { get; set; }
    }

    class PingData : SocketData
    {
        public PingData(int _pingMs, int _sendNumber)
        {
            O = Opcode.Hello;
            S = _sendNumber;
            D = new PingDataData() { pingMs = _pingMs };
        }

        public new Opcode O { get; set; }
        public new int S { get; set; }
        public new PingDataData D { get; set; }
        public new string E { get; set; }
    }
}
