using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MinecraftServerScanner.Library.Json
{
    public class ModInfo
    {
        [JsonProperty(PropertyName = "type")]
        public String Type { get; set; }

        [JsonProperty(PropertyName = "modList")]
        public Mod[] List { get; set; }
    }
}
