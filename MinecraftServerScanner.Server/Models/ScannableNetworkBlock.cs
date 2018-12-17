using MinecraftServerScanner.Server.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinecraftServerScanner.Server.Models
{
    public class ScannableNetworkBlock
    {
        public Int32 Id { get; set; }
        public String CIDR { get; set; }
        public DateTime Created { get; set; }

        public ScannableNetworkBlockState State { get; set; }
        public DateTime Updated { get; set; }

        public ScannableNetworkBlock()
        {
            this.Created = DateTime.Now;

            this.State = ScannableNetworkBlockState.Unassigned;

            this.Updated = DateTime.Now;
        }
    }
}
