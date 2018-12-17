using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MinecraftServerScanner.Library.Json
{
    public class Players
    {
        [JsonProperty(PropertyName = "max")]
        public Int32 Max { get; set; }

        [JsonProperty(PropertyName = "online")]
        public Int32 Online { get; set; }

        [JsonProperty(PropertyName = "sample")]
        public List<Player> Sample { get; set; }

        public Players()
        {
            this.Sample = new List<Player>();
        }
    }
}
