using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinecraftServerScanner.Server.Models
{
    public class PageOverview<T>
    {
        public Int32 Page { get; set; }
        public Int32 MaxSize { get; set; }
        public Int32 Size { get; set; }
        public List<T> Items { get; set; }

        public PageOverview(Int32 page, Int32 maxSize, List<T> Items)
        {
            this.Page = page;
            this.MaxSize = maxSize;
            this.Items = Items;
            this.Size = this.Items.Count;
        }
    }
}
