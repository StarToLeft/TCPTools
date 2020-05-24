using System;
using System.Collections.Generic;
using System.Text;
using TCPTools.Static;

namespace TCPTools.Client.Data
{
    [System.AttributeUsage(System.AttributeTargets.Method)]
    class OnServerDataAttribute : System.Attribute
    {
        public OnServerDataAttribute(Opcode opcode)
        {
            this.opcode = opcode;
        }

        public Opcode opcode;
    }
}
