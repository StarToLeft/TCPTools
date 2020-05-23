using System;
using System.Collections.Generic;
using System.Text;
using TCPTools.Static;

namespace TCPController.Client.Data.DataHandlers
{
    [DataAttribute]
    class DataHello
    {
        public static Opcode Opcode { get; }

        [OnDataAttribute]
        public static void OnData(string content)
        {
            Console.WriteLine("Yupp, this works!");
        }
    }
}
