using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MinecraftServerScanner.Library.Utilities
{
    /// <summary>
    /// Simple utility used to help calculate the next
    /// ip range group based on an input
    /// </summary>
    public class CidrMapper
    {
        public static IPNetwork Next(IPNetwork current)
        {
            var curNet = IPNetwork.Parse(current.ToString());
            uint ipAsUint = CidrMapper.IpToInt(curNet.ListIPAddress().Last());
            var nextAddress = BitConverter.GetBytes(ipAsUint + 1);
            return IPNetwork.Parse($"{String.Join(".", nextAddress.Reverse())}/{curNet.Cidr}");
        }

        public static UInt32 IpToInt(IPAddress address)
        {
            byte[] addressBytes = address.GetAddressBytes().Reverse().ToArray();
            return BitConverter.ToUInt32(addressBytes, 0);
        }
    }
}
