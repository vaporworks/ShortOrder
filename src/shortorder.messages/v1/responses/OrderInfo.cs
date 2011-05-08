using System;
using System.Collections.Generic;

namespace shortorder.messages
{
    public class OrderInfo
    {
        public Guid Id { get; set; }
        public string Customer { get; set; }
        public List<ItemInfo> Items { get; set; }
    }
}