using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MinecraftServerScanner.Server.Models;

namespace MinecraftServerScanner.Server
{
    public class MincraftContext : DbContext
    {
        public DbSet<ReservedNetworkBlock> ReservedNetworkBlocks { get; set; }
        public DbSet<ScannableNetworkBlock> ScannableNetworkBlocks { get; set; }

        public MincraftContext (DbContextOptions<MincraftContext> options)
            : base(options)
        {
        }
    }
}
