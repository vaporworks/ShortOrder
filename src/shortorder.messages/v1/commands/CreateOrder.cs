using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace shortorder.messages
{
    public class CreateOrder
    {
        public Guid Id { get; set; }
        public string CustomerName { get; set; }
        public IList<AddOrderItem> AddItems { get; set; }
    }

    public class AddOrderItem
    {
        public Guid OrderId { get; set; }
        public int ItemId { get; set; }
        public int Qty { get; set; }
    }
}
