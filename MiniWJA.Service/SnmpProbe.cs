
using SnmpSharpNet;
using System.Net;
using MiniWJA.Domain;

namespace MiniWJA.Service
{
    public static class SnmpProbe
    {
        public static Device TryDiscover(string ip, string community)
        {
            try
            {
                var target = new UdpTarget(IPAddress.Parse(ip), 161, 2000, 1);
                var param = new OctetString(community);
                var agent = new AgentParameters(param) { Version = SnmpVersion.Ver2 };

                Oid oidSysName  = new Oid("1.3.6.1.2.1.1.5.0");
                Oid oidSysDescr = new Oid("1.3.6.1.2.1.1.1.0");

                var pdu = new Pdu(PduType.Get);
                pdu.VbList.Add(oidSysName);
                pdu.VbList.Add(oidSysDescr);

                var result = (SnmpV2Packet)target.Request(pdu, agent);
                target.Close();

                if (result != null && result.Pdu.ErrorStatus == 0)
                {
                    string hostname = result.Pdu.VbList[0].Value.ToString();
                    string descr    = result.Pdu.VbList[1].Value.ToString();

                    return new Device
                    {
                        Hostname = string.IsNullOrWhiteSpace(hostname) ? ip : hostname,
                        IpAddress = ip,
                        Vendor = descr.Contains("HP") ? "HP" : "Unknown",
                        Model = descr,
                        LastSeenUtc = System.DateTime.UtcNow,
                        Status = DeviceStatus.Online
                    };
                }
            }
            catch { }

            return null;
        }

        public static void TryUpdateStatus(Device device, string community)
        {
            try
            {
                var target = new UdpTarget(IPAddress.Parse(device.IpAddress), 161, 2000, 1);
                var agent = new AgentParameters(new OctetString(community)) { Version = SnmpVersion.Ver2 };
                var pdu = new Pdu(PduType.Get);
                pdu.VbList.Add(new Oid("1.3.6.1.2.1.1.5.0"));
                var result = (SnmpV2Packet)target.Request(pdu, agent);
                target.Close();
                if (result != null && result.Pdu.ErrorStatus == 0)
                {
                    device.LastSeenUtc = System.DateTime.UtcNow;
                    device.Status = DeviceStatus.Online;
                }
                else
                {
                    device.Status = DeviceStatus.Offline;
                }
            }
            catch { device.Status = DeviceStatus.Offline; }
        }
    }
}
