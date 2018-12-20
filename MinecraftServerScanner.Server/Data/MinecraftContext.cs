using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MinecraftServerScanner.Server.Configurations;
using MinecraftServerScanner.Server.Models;

namespace MinecraftServerScanner.Server
{
    public class MinecraftContext : DbContext
    {
        public DbSet<ReservedNetworkBlock> ReservedNetworkBlocks { get; set; }
        public DbSet<ScannableNetworkBlock> ScannableNetworkBlocks { get; set; }
        public DbSet<MinecraftServer> MinecraftServers { get; set; }

        public MinecraftContext (DbContextOptions<MinecraftContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new MinecraftServerConfiguration());
        }
    }
}
