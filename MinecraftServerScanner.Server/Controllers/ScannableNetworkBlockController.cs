using Microsoft.AspNetCore.Mvc;
using MinecraftServerScanner.Library.Implementations;
using MinecraftServerScanner.Library.Interfaces;
using MinecraftServerScanner.Library.Json;
using MinecraftServerScanner.Library.Utilities;
using MinecraftServerScanner.Server.Enums;
using MinecraftServerScanner.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MinecraftServerScanner.Server.Controllers
{
    [Route("api/v1/scannable-network-blocks")]
    public class ScannableNetworkBlockController : Controller
    {
        private MinecraftContext _db;
        private static Object _lock = new object();

        public ScannableNetworkBlockController(MinecraftContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Create a new block (or select an unfinished block),
        /// mark is as assigned, and return its data so the client
        /// may begin scanning.
        /// 
        /// Ensure that the input block is not within a reserved range
        /// </summary>
        [HttpGet]
        [Route("assign")]
        public JsonResult Assign()
        {
            lock(_lock)
            {
                ScannableNetworkBlock block;

                // First check if any assigned blocks havent been completed in over 5 minutes

                var unfinished = _db.ScannableNetworkBlocks
                    .Where(snb => snb.State != ScannableNetworkBlockState.Scanned && snb.Updated < DateTime.Now.AddMinutes(-5));

                if(unfinished.Count() > 0)
                { // If theres an unnassigned or forgotten block, reassign it to the newest user
                    block = unfinished.First();
                }
                else
                { // Otherwise generate the next block to be scanned
                    block = this.CalculateNext();
                }

                // Update the blocks status
                block.State = ScannableNetworkBlockState.Assigned;
                block.Updated = DateTime.Now;
                _db.SaveChanges();

                // Return the block data
                return Json(block);
            }
        }

        /// <summary>
        /// Mark a specific scannable network block as copmlete
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("{id}/complete")]
        public JsonResult Complete(Int32 id, [FromBody] Library.Implementations.MinecraftServer[] servers)
        {
            // Store each input server
            foreach(Library.Implementations.MinecraftServer server in servers)
            {
                _db.MinecraftServers.Add(new Models.MinecraftServer()
                {
                    Host = server.Host,
                    Port = server.Port,
                    Data = server.Data,
                    Online = server.Online,
                    LastOnline = (server.Online ? DateTime.Now : DateTime.MinValue),
                    Scanned = DateTime.Now,
                    Version = server.Data?.Version.Name,
                    Icon = server.Data?.Icon
                });
            }

            var block = _db.ScannableNetworkBlocks.Where(snb => snb.Id == id).FirstOrDefault();

            // Mark the blocks state as complete
            block.State = ScannableNetworkBlockState.Scanned;
            block.Updated = DateTime.Now;

            // save the block
            _db.SaveChanges();

            // Tell the client the block is not complete
            return Json(block);
        }

        /// <summary>
        /// Calculate the next block based on the most recently created block
        /// </summary>
        /// <returns></returns>
        private ScannableNetworkBlock CalculateNext()
        {
            lock (_lock)
            {
                var reservedList = _db.ReservedNetworkBlocks.Select(rnb => rnb.Network).ToArray();
                var nextCidr = IPNetwork.Parse(_db.ScannableNetworkBlocks.LastOrDefault()?.CIDR ?? "0.0.0.0/20");
                bool good;

                do
                {
                    nextCidr = CidrMapper.Next(nextCidr);
                    good = true;
                    foreach (IPNetwork reserved in reservedList)
                    { // Check for ip overlap within the reserved lists
                        if (CidrMapper.IpToInt(reserved.FirstUsable) <= CidrMapper.IpToInt(nextCidr.FirstUsable) && CidrMapper.IpToInt(reserved.LastUsable) >= CidrMapper.IpToInt(nextCidr.LastUsable))
                        {
                            good = false;
                            break;
                        }
                    }
                } while (!good);


                // Create a new block in the database
                var nextBlock = _db.ScannableNetworkBlocks.Add(new ScannableNetworkBlock()
                {
                    CIDR = nextCidr.ToString()
                });
                _db.SaveChanges();

                // Return the data
                return nextBlock.Entity;
            }
        }
    }
}
