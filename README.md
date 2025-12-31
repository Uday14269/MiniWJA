
# MiniWJA (.NET Framework 4.8) Starter

A minimal, educational scaffold that mimics key HP Web Jetadmin concepts:
- ASP.NET MVC admin console
- Windows Service for discovery & polling (SNMP)
- EF6 data layer and domain models

## Prerequisites
- Visual Studio 2022/2019 with .NET desktop workload
- .NET Framework 4.8 Developer Pack
- SQL Server (localdb or full)

## Restore NuGet packages
Use Package Manager Console:

### Web (MVC)
PM> Install-Package Microsoft.AspNet.Mvc -Version 5.2.9
PM> Install-Package Microsoft.AspNet.Razor -Version 3.2.9
PM> Install-Package Microsoft.AspNet.WebPages -Version 3.2.9
PM> Install-Package Microsoft.AspNet.SignalR -Version 2.4.3  # optional
PM> Install-Package EntityFramework -Version 6.4.4

### Service (Windows Service)
PM> Install-Package SnmpSharpNet
PM> Install-Package Quartz
PM> Install-Package EntityFramework -Version 6.4.4

### Data (EF6)
PM> Install-Package EntityFramework -Version 6.4.4

## Database
Set the connection string in `MiniWJA.Web/Web.config` and `MiniWJA.Service/App.config`:

```
Data Source=localhost;Initial Catalog=MiniWJA;Integrated Security=True;MultipleActiveResultSets=True
```

Enable EF6 migrations (from Package Manager Console):

```
PM> Enable-Migrations -ProjectName MiniWJA.Data -StartUpProjectName MiniWJA.Web
PM> Add-Migration Initial
PM> Update-Database
```

## Run the Web app
Open `MiniWJA.Web/MiniWJA.Web.csproj` in Visual Studio and press F5.

## Install the Windows Service
Build `MiniWJA.Service` → open elevated cmd:

```
sc create MiniWjaService binPath= "C:\Path\To\MiniWJA.Service.exe" start= auto
sc start MiniWjaService
```

The service runs discovery every 10 minutes and polling every 5 minutes.
Default discovery CIDR: set via DB record in `DiscoveryRanges` (e.g., `192.168.1.0/24`, community `public`).

## Notes
- SNMP probing uses `SnmpSharpNet`; extend it with Printer-MIB OIDs for supplies & counters.
- MVC views are basic; customize UI and add Identity/RBAC if needed.
- This starter is educational and not production-hardened.
