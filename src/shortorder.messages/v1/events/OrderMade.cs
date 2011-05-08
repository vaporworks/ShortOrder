using System;
using System.Collections.Generic;

namespace shortorder.messages
{
    public class OrderMade
    {
        public Guid Id { get; set; }
        public string CustomerName { get; set; }
        public IList<int> ItemIds { get; set; }
    }
}