
using System.ServiceProcess;

namespace MiniWJA.Service
{
    static class Program
    {
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] { new MiniWjaService() };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
