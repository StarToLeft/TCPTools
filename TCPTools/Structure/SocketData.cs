using System;
using TCPTools.Static;

namespace TCPTools.Structure
{
    public class SocketData
    {
        public Opcode O { get; set; } // Opcode
        public int S { get; set; } // Number
        public object D { get; set; } = new Object(); // Data
        public string E { get; set; } = String.Empty; // EventType
    }
}
