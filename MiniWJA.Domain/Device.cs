
using System;

namespace MiniWJA.Domain
{
    public class Device
    {
        public int Id { get; set; }
        public string Hostname { get; set; }
        public string IpAddress { get; set; }
        public string Vendor { get; set; } = "HP";
        public string Model { get; set; }
        public string SerialNumber { get; set; }
        public string FirmwareVersion { get; set; }
        public DateTime LastSeenUtc { get; set; }
        public DeviceStatus Status { get; set; } = DeviceStatus.Unknown;
        public virtual SuppliesStatus Supplies { get; set; } = new SuppliesStatus();
        public virtual UsageCounters Counters { get; set; } = new UsageCounters();
    }

    public enum DeviceStatus { Unknown, Online, Offline, Error }

    public class SuppliesStatus
    {
        public int Id { get; set; }
        public int? BlackPercent { get; set; }
        public int? CyanPercent { get; set; }
        public int? MagentaPercent { get; set; }
        public int? YellowPercent { get; set; }
        public int DeviceId { get; set; }
    }

    public class UsageCounters
    {
        public int Id { get; set; }
        public long TotalPages { get; set; }
        public long ColorPages { get; set; }
        public long MonoPages { get; set; }
        public DateTime UpdatedUtc { get; set; }
        public int DeviceId { get; set; }
    }

    public class DiscoveryRange
    {
        public int Id { get; set; }
        public string Cidr { get; set; }
        public string CommunityString { get; set; } = "public";
        public bool Enabled { get; set; } = true;
    }

    public class ConfigTemplate
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool? DefaultDuplexOn { get; set; }
        public bool? DisableColor { get; set; }
        public TimeSpan? SleepAfter { get; set; }
    }
}
