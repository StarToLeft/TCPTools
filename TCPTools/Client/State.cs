
using System;
using System.Collections.Generic;
using System.Text;

namespace TCPTools.Client
{
    class State
    {
        public const int bufferSize = 1024;
        public byte[] buffer = new Byte[bufferSize];

        public StringBuilder sb = new StringBuilder();
    }
}
