using MinecraftServerScanner.Library.Messages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftServerScanner.Library
{
    /// <summary>
    /// Simple class that will automatically ping
    /// an input host & port, and return the
    /// minecraft response body
    /// </summary>
    public class Pinger
    {
        public static TimeSpan Timeout;

        private TcpClient _client;
        private NetworkStream _stream;
        private String _host;
        private Int16 _port;

        public HandshakeResponseMessage Response { get; private set; }

        private Pinger(String host, Int16 port)
        {
            Console.WriteLine($"Pinging {host}:{port}");

            _host = host;
            _port = port;
            _client = new TcpClient();

            var res = _client.BeginConnect(host, port, null, null);
            var suc = res.AsyncWaitHandle.WaitOne(Pinger.Timeout);


            if (suc)
            { // Only do anything if a server is found at that ip
                _stream = _client.GetStream();

                this.Handshake();
                this.Request();

                this.Response = new HandshakeResponseMessage(_stream);

                // Close unneeded assets
                _stream.Close();
            }

            // Close unneeded assets
            _client.Close();
        }

        /// <summary>
        /// Send a "Handshake" packet
        /// http://wiki.vg/Server_List_Ping#Ping_Process
        /// </summary>
        private void Handshake()
        {
            var message = new OutboundMessage(0x00);
            message.WriteVarInt(1);
            message.WriteString(_host);
            message.WriteShort(_port);
            message.WriteVarInt(1);
            this.SendMessage(message);
        }

        /// <summary>
        /// Send a "Status Request" packet
        /// http://wiki.vg/Server_List_Ping#Ping_Process
        /// </summary>
        private void Request()
        {
            var message = new OutboundMessage(0x00);
            this.SendMessage(message);
        }

        private void SendMessage(OutboundMessage message)
        {
            var data = message.GetData();

            //_stream.Write(length, 0, length.Length);
            _stream.Write(data, 0, data.Length);
        }

        static Pinger()
        {
            Pinger.Timeout = TimeSpan.FromMilliseconds(75);
        }

        public static HandshakeResponseMessage Ping(String host, Int16 port)
        {
            return Pinger.Create(host, port).Response;
        }

        public static Pinger Create(String host, Int16 port)
        {
            return new Pinger(host, port);
        }
    }
}
