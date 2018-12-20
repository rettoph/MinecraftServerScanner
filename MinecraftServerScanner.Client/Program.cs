using MinecraftServerScanner.Library;
using MinecraftServerScanner.Library.Implementations;
using MinecraftServerScanner.Library.Interfaces;
using MinecraftServerScanner.Library.Json;
using MinecraftServerScanner.Library.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MinecraftServerScanner.Client
{
    class Program
    {
        private static String _baseUri;
        #region HTTP response structures
        private struct NetworkBlock
        {
            public Int32 Id { get; set; }
            public String CIDR { get; set; }
        }
        #endregion

        static void Main(string[] args)
        {
            for(short i = 25800; i< 32767; i++)
                BatchPinger.Ping("23.235.255/24", i);
            Console.ReadLine();

            _baseUri = "http://rettoph.io/api/v1";
            BatchPinger.Threads = args.Length == 0 ? 512 : Int32.Parse(args[0]);

            Stopwatch _sw = new Stopwatch();

            _sw.Start();

            NetworkBlock _block;

            while (true)
            {
                _sw.Start();
                _block = NextBlock();
                var pings = BatchPinger.Ping(_block.CIDR, 25565);
                Complete(_block, pings.Where(p => p.Online));

                _sw.Stop();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(_sw.Elapsed);
                _sw.Reset();
            }

            Console.ReadLine();
        }

        private static NetworkBlock Complete(NetworkBlock block, IEnumerable<IMinecraftServer> servers)
        {
            return JsonConvert.DeserializeObject<NetworkBlock>(
                Post($"/scannable-network-blocks/{block.Id}/complete", servers));
        }

        /// <summary>
        /// Request another network block from the server
        /// </summary>
        /// <param name="completed"></param>
        /// <returns></returns>
        private static NetworkBlock NextBlock()
        {
            return JsonConvert.DeserializeObject<NetworkBlock>(
                    Get("/scannable-network-blocks/assign"));
        }

        /// <summary>
        /// General HTTP Get/POST
        /// https://stackoverflow.com/questions/27108264/c-sharp-how-to-properly-make-a-http-web-get-request
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        private static String Get(String uri)
        {
            var request = WebRequest.Create(_baseUri + uri);
            request.Method = "GET";
            request.Timeout = 600000;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                    using (StreamReader reader = new StreamReader(stream))
                        return reader.ReadToEnd();
        }

        /// <summary>
        /// General HTTP Get/POST
        /// https://stackoverflow.com/questions/27108264/c-sharp-how-to-properly-make-a-http-web-get-request
        /// 
        /// Converting NameValueCollection to String
        /// https://stackoverflow.com/questions/7023046/sending-namevaluecollection-to-http-request-c-sharp
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private static String Post(String uri, Object data)
        {
            var request = WebRequest.Create(_baseUri + uri);
            //request = WebRequest.Create("http://localhost/data.php");

            request.Method = "POST";
            request.ContentType = "application/json";
            request.Timeout = 600000;

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(JsonConvert.SerializeObject(data));
                streamWriter.Flush();
                streamWriter.Close();
            }

            String test;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                    using (StreamReader reader = new StreamReader(stream))
                        test =  reader.ReadToEnd();

            return test;
        }
    }
}
