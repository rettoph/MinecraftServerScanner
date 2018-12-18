using MinecraftServerScanner.Library.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MinecraftServerScanner.Library.Interfaces
{
    public interface IMinecraftServer
    {
        String Host { get; set; }
        Int16 Port { get; set; }
        Boolean Online { get; set; }
        HandshakeResponse Data { get; set; }
    }
}
