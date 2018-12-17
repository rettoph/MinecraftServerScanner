﻿using Microsoft.AspNetCore.Mvc;
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
        private MincraftContext _db;
        private static Object _lock = new object();

        public ScannableNetworkBlockController(MincraftContext db)
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
        [HttpGet]
        [Route("{id}/complete")]
        public JsonResult Complete(Int32 id)
        {
            var block = _db.ScannableNetworkBlocks.Where(snb => snb.Id == id).FirstOrDefault();

            // Mark the blocks state as complete
            block.State = ScannableNetworkBlockState.Scanned;
            block.Updated = DateTime.Now;

            // save the block
            _db.SaveChanges();

            // Assign a new block to the client
            return this.Assign();

        }

        /// <summary>
        /// Calculate the next block based on the most recently created block
        /// </summary>
        /// <returns></returns>
        private ScannableNetworkBlock CalculateNext()
        {
            lock (_lock)
            {
                var reservedList = _db.ReservedNetworkBlocks.Select(rnb => rnb.Network);
                var nextCidr = IPNetwork.Parse(_db.ScannableNetworkBlocks.LastOrDefault()?.CIDR ?? "0.0.0.0/21");
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
