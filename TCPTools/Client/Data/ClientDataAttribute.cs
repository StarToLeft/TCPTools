using System;
using System.Collections.Generic;
using System.Text;
using TCPTools.Static;

namespace TCPTools.Client.Data
{
    [System.AttributeUsage(System.AttributeTargets.Method)]
    internal class OnClientDataAttribute : System.Attribute
    {
        public OnClientDataAttribute(Opcode opcode)
        {
            this.opcode = opcode;
        }

        public Opcode opcode;
    }
}
