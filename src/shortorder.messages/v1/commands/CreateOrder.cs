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
        public IList<int> ItemIds { get; set; }
    }
}
