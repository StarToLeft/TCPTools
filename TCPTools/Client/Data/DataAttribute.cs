using System;
using System.Collections.Generic;
using System.Text;

namespace TCPController.Client.Data
{
    [System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Method)]
    class DataAttribute : System.Attribute
    {
    }

    [System.AttributeUsage(System.AttributeTargets.Method)]
    class OnDataAttribute : System.Attribute
    {
    }
}
