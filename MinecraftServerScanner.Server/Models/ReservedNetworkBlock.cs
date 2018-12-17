using MinecraftServerScanner.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MinecraftServerScanner.Server.Models
{
    public class ReservedNetworkBlock
    {
        public Int32    Id      { get; set; }
        public String   CIDR    { get; set; }
        public DateTime Created { get; set; }

        private IPNetwork _network;
        public IPNetwork Network {
            get
            {
                return _network == null ? (_network = IPNetwork.Parse(this.CIDR)) : _network;
            }
        }

        public ReservedNetworkBlock()
        {
            this.Created = DateTime.Now;
        }
    }
}
