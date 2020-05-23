using System;
using System.Collections.Generic;
using System.Text;

namespace TCPTools.Static
{
    public enum Opcode
    {
        None = 0,
        Hello = 1,
        Ping = 2,
        Data = 3,
    }
}
