using System;
using System.Collections.Generic;

namespace shortoder.domain
{
    public class Order
    {
        public Guid Id { get; set; }
        public int Rank { get; set; }
        public string CustomerName { get; set; }
        public IList<int> ItemIds { get; set; }
    }
}
