
using System.Timers;
using System.ServiceProcess;
using MiniWJA.Data;
using System.Linq;

namespace MiniWJA.Service
{
    public class MiniWjaService : ServiceBase
    {
        private Timer _discoveryTimer;
        private Timer _pollingTimer;

        public MiniWjaService()
        {
            this.ServiceName = "MiniWjaService";
        }

        protected override void OnStart(string[] args)
        {
            _discoveryTimer = new Timer(10 * 60 * 1000);
            _discoveryTimer.Elapsed += (s, e) => RunDiscovery();
            _discoveryTimer.AutoReset = true;
            _discoveryTimer.Start();

            _pollingTimer = new Timer(5 * 60 * 1000);
            _pollingTimer.Elapsed += (s, e) => RunPolling();
            _pollingTimer.AutoReset = true;
            _pollingTimer.Start();
        }

        protected override void OnStop()
        {
            _discoveryTimer?.Stop();
            _pollingTimer?.Stop();
        }

        private void RunDiscovery()
        {
            using (var db = new AppDbContext())
            {
                var ranges = db.DiscoveryRanges.Where(r => r.Enabled).ToList();
                foreach (var range in ranges)
                {
                    foreach (var ip in CidrHelper.Expand(range.Cidr))
                    {
                        var device = SnmpProbe.TryDiscover(ip, range.CommunityString);
                        if (device != null)
                        {
                            var existing = db.Devices.FirstOrDefault(d => d.IpAddress == ip);
                            if (existing == null) db.Devices.Add(device);
                            else
                            {
                                existing.Model = device.Model;
                                existing.LastSeenUtc = device.LastSeenUtc;
                                existing.Status = device.Status;
                            }
                            db.SaveChanges();
                        }
                    }
                }
            }
        }

        private void RunPolling()
        {
            using (var db = new AppDbContext())
            {
                var ranges = db.DiscoveryRanges.ToList();
                var community = ranges.FirstOrDefault()?.CommunityString ?? "public";

                foreach (var dev in db.Devices.ToList())
                {
                    SnmpProbe.TryUpdateStatus(dev, community);
                }
                db.SaveChanges();
            }
        }
    }
}
