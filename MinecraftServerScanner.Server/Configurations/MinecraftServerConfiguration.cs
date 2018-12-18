using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MinecraftServerScanner.Library.Json;
using MinecraftServerScanner.Library.Json.Converters;
using MinecraftServerScanner.Server.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinecraftServerScanner.Server.Configurations
{
    public class MinecraftServerConfiguration : IEntityTypeConfiguration<MinecraftServer>
    {
        private HandshakeResponseConverter _converter;
        public MinecraftServerConfiguration()
        {
            _converter = new HandshakeResponseConverter();
        }

        public void Configure(EntityTypeBuilder<MinecraftServer> builder)
        {
            builder.Property(s => s.Data).HasConversion(
                v => JsonConvert.SerializeObject(v),
                v => JsonConvert.DeserializeObject<HandshakeResponse>(v, _converter));
        }
    }
}
