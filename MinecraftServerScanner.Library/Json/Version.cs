using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MinecraftServerScanner.Library.Json
{
    public class Version
    {
        [JsonProperty(PropertyName = "name")]
        public String Name { get; set; }

        [JsonProperty(PropertyName = "protocol")]
        public String Protocol { get; set; }
    }
}
