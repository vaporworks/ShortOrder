using System;
using System.Collections.Generic;

namespace shortoder.domain
{
    public class Order
    {
        public Guid Id { get; set; }
        public int Rank { get; set; }
        public bool Complete { get; set; }
        public string CustomerName { get; set; }
        public IList<OrderItem> ItemIds { get; set; }

        public Order() 
        {
            ItemIds = new List<OrderItem>();
        }
    }

    public class OrderItem
    {
        public int ItemId { get; set; }
        public int Qty { get; set; }
    }
}
