using MinecraftServerScanner.Library.Implementations;
using MinecraftServerScanner.Library.Interfaces;
using MinecraftServerScanner.Library.Json;
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
    public class Pinger : MinecraftServer
    {
        public static TimeSpan Timeout;
        private static Object _lock;

        private TcpClient _client;
        private NetworkStream _stream;

        private HandshakeResponseMessage _response;


        private Pinger(String host, Int16 port)
        {
            this.Host = host;
            this.Port = port;
            this.Online = false;
            _client = new TcpClient();
            _client.ReceiveTimeout = 1000;
            _client.SendTimeout = 1000;

            //_client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            //_client.Client.ExclusiveAddressUse = false;
            //_client.ExclusiveAddressUse = false;

            try
            {
                if (!(!_client.ConnectAsync(host, port).Wait(Timeout)))
                { // Only do anything if a server is found at that ip

                    _stream = _client.GetStream();

                    this.Handshake();
                    this.Request();

                    _response = new HandshakeResponseMessage(_stream);
                    this.Data = _response.Data;
                    this.Online = true;

                    // Close unneeded assets
                    _stream.Close();

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Data {host}:{port}");
                }

            }
            catch (Exception e)
            {
                lock (_lock)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Server {host}:{port} responded with malformed data.");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($"{e.GetType()}:{e.Message}");
                }
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
            message.WriteString(Host);
            message.WriteShort(Port);
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
            Pinger.Timeout = TimeSpan.FromMilliseconds(500);
            _lock = new object();
        }

        public static HandshakeResponse Ping(String host, Int16 port)
        {
            return Pinger.Create(host, port).Data;
        }

        public static Pinger Create(String host, Int16 port)
        {
            return new Pinger(host, port);
        }
    }
}
