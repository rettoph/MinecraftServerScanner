using MinecraftServerScanner.Library;
using MinecraftServerScanner.Library.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftServerScanner.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            var c = "0.0.0.0/21";
            var i = 0;
            while(true)
            {
                Console.WriteLine($"{i} - {c}");
                c = CidrMapper.Next(c);
                i++;
            }
            */
            

            Stopwatch _sw = new Stopwatch();
            _sw.Start();
            var test = BatchPinger.Ping("127.0.0.0/21", 25565);
            _sw.Stop();

            Console.WriteLine(_sw.Elapsed);

            Console.ReadLine();
        }
    }
}
