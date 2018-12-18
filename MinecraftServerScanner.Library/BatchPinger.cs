using MinecraftServerScanner.Library.Messages;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;

namespace MinecraftServerScanner.Library
{
    public class BatchPinger
    {
        private IPNetwork _network;
        private Int16 _port;
        private IPAddressCollection _ips;
        private Pinger[] Pings;
        private CountdownEvent _complete;

        private BatchPinger(IPNetwork network, Int16 port, Int32 threads)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"Starting batch {network} with port {port}...");

            _network = network;
            _ips = _network.ListIPAddress();
            _port = port;
            _complete = new CountdownEvent((Int32)_ips.Count);
            this.Pings = new Pinger[(Int32)_ips.Count];

            ThreadPool.SetMinThreads(threads, threads);
            ThreadPool.SetMaxThreads(threads, threads);

            // Queue up Pinger creation for each ip in the network
            for (Int32 i= 0; i < _ips.Count; i++)
                ThreadPool.QueueUserWorkItem(new WaitCallback(this.CreatePinger), i);

            // Wait for the countdown
            _complete.Wait();

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Batch complete.");
        }

        private void CreatePinger(object n)
        {
            var i = (Int32)n;
            var ip = _ips[i];

            this.Pings[i] = Pinger.Create(ip.ToString(), _port);
            _complete.Signal();
        }

        public static Pinger[] Ping(IPNetwork network, Int16 port)
        {
            return new BatchPinger(network, port, 255).Pings;
        }

        public static Pinger[] Ping(String network, Int16 port)
        {
            return BatchPinger.Ping(IPNetwork.Parse(network), port);
        }
    }
}
