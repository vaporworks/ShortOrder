using System;
using System.Collections.Generic;
using Symbiote.Core.UnitOfWork;

namespace shortorder.messages
{
    public class OrderCreated : EventBase
    {
        public Guid Id { get; set; }
        public string CustomerName { get; set; }
        public IList<OrderItemCreated> Items { get; set; }
        public int Rank { get; set; }
    }

    public class OrderItemCreated
    {
        public int ItemId { get; set; }
        public int Qty { get; set; }
        public string Description { get; set; }
    }
}