using MinecraftServerScanner.Library;
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

        public String Icon { get; set; }
        public String Version { get; set; }

        public MinecraftServer()
        {
            this.Created = DateTime.Now;
        }

        /// <summary>
        /// Rescan the current server (Does not update the database)
        /// </summary>
        public void Scan()
        {
            if (this.Scanned <= DateTime.Now.AddMinutes(-5))
            { // Ensure we only scan a server at most once every 5 minutes
                var ping = Pinger.Create(this.Host, this.Port);

                this.Scanned = DateTime.Now;
                this.Online = ping.Online;

                if (ping.Online)
                {
                    this.LastOnline = DateTime.Now;
                    this.Data = ping.Data;
                    this.Icon = ping.Data.Icon;
                    this.Version = ping.Data.Version.Name;
                }
            }
        }
    }
}
