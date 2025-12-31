
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace MiniWJA.Service
{
    public static class CidrHelper
    {
        public static IEnumerable<string> Expand(string cidr)
        {
            var parts = cidr.Split('/');
            if (parts.Length != 2) yield break;
            var baseIp = parts[0];
            int prefix;
            if (!int.TryParse(parts[1], out prefix)) yield break;

            var ipBytes = IPAddress.Parse(baseIp).GetAddressBytes().Reverse().ToArray();
            uint ip = System.BitConverter.ToUInt32(ipBytes, 0);
            uint mask = prefix == 0 ? 0 : uint.MaxValue << (32 - prefix);
            uint network = ip & mask;
            uint broadcast = network | ~mask;
            for (uint addr = network + 1; addr < broadcast; addr++)
            {
                var b = System.BitConverter.GetBytes(addr).Reverse().ToArray();
                yield return new IPAddress(b).ToString();
            }
        }
    }
}
