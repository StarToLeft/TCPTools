using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace TCPTools.Logging
{
    public static class Logger
    {
        public static void Log<T>(T log, params object[] arg)
        {
            var date = DateTime.Now.ToString("hh:mm:ss");

            Console.Write("["+ date + "] ");
            if (log is String && arg.Length > 0)
                Console.Write(log as string, arg);
            else
                Console.Write(log);
            Console.Write("\n");

            Console.ResetColor();
        }

        public static void Error<T>(T log, params object[] arg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Log(log, arg);
            Console.ResetColor();
        }

        public static void Warn<T>(T log, params object[] arg)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Log(log, arg);
            Console.ResetColor();
        }

        public static void Info<T>(T log, params object[] arg)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Log(log, arg);
            Console.ResetColor();
        }
    }
}
