using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MinecraftServerScanner.Library.Json
{
    public class HandshakeResponse
    {
        [JsonProperty(PropertyName = "players")]
        public Players Players { get; set; }

        [JsonProperty(PropertyName = "version")]
        public Version Version { get; set; }

        [JsonProperty(PropertyName = "description")]
        public Chat Description { get; set; }

        [JsonProperty(PropertyName = "modInfo")]
        public ModInfo ModInfo { get; set; }

        [JsonProperty(PropertyName = "favicon")]
        public String Icon { get; set; }
    }
}
