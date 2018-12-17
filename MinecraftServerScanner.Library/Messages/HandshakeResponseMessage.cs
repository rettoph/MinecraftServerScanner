using MinecraftServerScanner.Library.Json;
using MinecraftServerScanner.Library.Json.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace MinecraftServerScanner.Library.Messages
{
    public class HandshakeResponseMessage
    {
        private InboundMessage _im;
        public HandshakeResponse Data { get; private set; }


        public HandshakeResponseMessage(NetworkStream stream)
        {
            _im = new InboundMessage(stream);

            var jsonLength = _im.ReadVarInt();
            var json = _im.ReadString(jsonLength);

            this.Data = JsonConvert.DeserializeObject<HandshakeResponse>(json, new HandshakeResponseConverter());
        }
    }
}
