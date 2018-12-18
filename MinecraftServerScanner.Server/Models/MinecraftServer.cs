using MinecraftServerScanner.Library.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinecraftServerScanner.Server.Models
{
    public class MinecraftServer : MinecraftServerScanner.Library.Implementations.MinecraftServer
    {
        public Int32 Id { get; set; }

        public DateTime Created { get; set; }
        public DateTime Scanned { get; set; }
        public DateTime LastOnline { get; set; }

        public MinecraftServer()
        {
            this.Created = DateTime.Now;
        }
    }
}
