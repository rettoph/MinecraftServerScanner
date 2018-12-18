using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MinecraftServerScanner.Library.Json
{
    public class Mod
    {
        [JsonProperty(PropertyName = "modid")]
        public String ModId { get; set; }

        [JsonProperty(PropertyName = "version")]
        public String Version { get; set; }
    }
}
