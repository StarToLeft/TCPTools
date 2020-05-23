using System;
using System.Collections.Generic;
using System.Text;
using TCPTools.Errors;
using TCPTools.Static;

namespace TCPTools.Structure
{
    class ErrorDataData
    {
        public Error Error { get; set; }
    }

    class ErrorData : SocketData
    {
        public ErrorData(Error error)
        {
            D = new ErrorDataData() { Error = error };
        }

        public new Opcode O { get; set; }
        public new int S { get; set; } 
        public new ErrorDataData D { get; set; } 
        public new string E { get; set; }
    }
}
