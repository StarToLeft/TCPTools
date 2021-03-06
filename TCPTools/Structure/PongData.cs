﻿using System;
using TCPTools.Static;

namespace TCPTools.Structure
{
    class PongData : SocketData
    {
        public new Opcode O { get; set; } = Opcode.Pong;
        public new int S { get; set; }
        public new object D { get; set; }
        public new string E { get; set; }
    }
}