
using System.ComponentModel;
using System.ServiceProcess;
using System.Configuration.Install;

namespace MiniWJA.Service
{
    [RunInstaller(true)]
    public class ProjectInstaller : Installer
    {
        public ProjectInstaller()
        {
            var processInstaller = new ServiceProcessInstaller
            {
                Account = ServiceAccount.LocalSystem
            };
            var serviceInstaller = new ServiceInstaller
            {
                ServiceName = "MiniWjaService",
                StartType = ServiceStartMode.Automatic
            };
            Installers.Add(processInstaller);
            Installers.Add(serviceInstaller);
        }
    }
}
