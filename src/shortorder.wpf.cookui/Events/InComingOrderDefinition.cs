using System;
using System.Collections.Generic;

namespace shortorder.wpf.cookui.Events
{
    public class InComingOrderDefinition
    {
        public Guid Id { get; set; }
        public string CustomerName { get; set; }
        public IList<InComingOrderItemDefinition> Items { get; set; }
        public DateTime TimeReceived { get; set; }
        public int Rank { get; set; }
    }
}