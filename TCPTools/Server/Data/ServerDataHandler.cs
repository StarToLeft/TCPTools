using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TCPTools.Logging;
using TCPTools.Static;

namespace TCPTools.Client.Data
{
    public class ServerDataHandler
    {
        // TODO: Implement better caching, store more data in memory than running operations multiple times on the CPU

        public ServerDataHandler()
        {
            _methods = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => x.IsClass)
                .SelectMany(x => x.GetMethods())
                .Where(x => x.GetCustomAttributes(typeof(OnServerDataAttribute), false).FirstOrDefault() != null);
        }

        // TODO: Implement custom type
        private readonly IEnumerable<MethodInfo> _methods;

        public void ReceivedData(Opcode o, string content, Server.Server server, string id)
        {
            foreach (MethodInfo method in _methods)
            {
                try
                {
                    var parameters = method.GetCustomAttributes(typeof(OnServerDataAttribute));
                    
                    Attribute parameter = parameters.FirstOrDefault(attribute => attribute.GetType() == typeof(OnServerDataAttribute));

                    if (!(parameter is OnServerDataAttribute x)) continue;
                    if (x.opcode != o) continue;
                    
                    var parametersArray = new object[] { content, server, id };
                    method.Invoke(null, parametersArray);
                } catch (Exception ex)
                {
                    Logger.Error(ex);
                }
            }
        }
    }
}
