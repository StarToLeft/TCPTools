using System;

namespace TCPTools.Logging
{
    public static class Logger
    {
        public static void Log<T>(T log, params object[] arg)
        {
            var date = DateTime.Now.ToString("hh:mm:ss");

            if (log is String && arg.Length > 0)
                Console.WriteLine("[" + date + "] " + log, arg);
            else
                Console.WriteLine("[" + date + "] {0}", log);

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
