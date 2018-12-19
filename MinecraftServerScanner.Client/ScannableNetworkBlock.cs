using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinecraftServerScanner.Client
{
    public class ScannableNetworkBlock
    {
        public Int32 Id { get; set; }
        public String CIDR { get; set; }
    }
}
