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
    class DataHandler
    {
        public DataHandler()
        {
            methods = AppDomain.CurrentDomain.GetAssemblies() // Returns all currenlty loaded assemblies
                .SelectMany(x => x.GetTypes()) // returns all types defined in this assemblies
                .Where(x => x.IsClass) // only yields classes
                .SelectMany(x => x.GetMethods()) // returns all methods defined in those classes
                .Where(x => x.GetCustomAttributes(typeof(OnDataAttribute), false).FirstOrDefault() != null); // returns only methods that have the InvokeAttribute
        }

        private IEnumerable<MethodInfo> methods;

        public void ReceivedData(Opcode o, string content)
        {
            foreach (var method in methods) // iterate through all found methods
            {
                try
                {
                    object[] parametersArray = new object[] { content };
                    method.Invoke(null, parametersArray);
                } catch (Exception ex)
                {
                    Logger.Error(ex);
                }
            }
        }
    }
}
