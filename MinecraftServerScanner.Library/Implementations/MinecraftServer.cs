using MinecraftServerScanner.Library.Interfaces;
using MinecraftServerScanner.Library.Json;
using MinecraftServerScanner.Library.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace MinecraftServerScanner.Library.Implementations
{
    public class MinecraftServer : IMinecraftServer
    {
        public String Host { get; set; }
        public Int16 Port { get; set; }

        public Boolean Online { get; set; }
        public HandshakeResponse Data { get; set; }
        
    }
}
