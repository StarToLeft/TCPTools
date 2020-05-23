using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TCPTools.Static
{
    public static class IP
    {
        public static IPAddress GetIp(int port)
        {
            // Get starting ip address and port
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            foreach (var ip in ipHostInfo.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    ipAddress = ip;
                }
            }

            return ipAddress;
        }

        public static IPEndPoint GetLocalEndPoint(int port)
        {
            IPAddress ipAddress = GetIp(port);
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, port);

            return localEndPoint;
        }
    }
}
