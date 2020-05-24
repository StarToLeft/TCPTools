using System;
using TCPTools.Static;

namespace TCPTools.Structure
{
    public class HelloDataData
    {
        public int PingMs { get; set; }
    }

    public class HelloData : SocketData
    {
        public HelloData(int pingMs, int sequence)
        {
            O = Opcode.Hello;
            S = sequence;
            D = new HelloDataData() { PingMs = pingMs };
        }

        public new Opcode O { get; set; }
        public new int S { get; set; }
        public new HelloDataData D { get; set; }
        public new string E { get; set; }
    }
}
