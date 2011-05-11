using System;

namespace shortorder.wpf.cookui.Events
{
    public class OrderMadeDefinition
    {
        public Guid Id { get; set; }
        public string CustomerName { get; set; }
        public DateTime TimeMade { get; set; }
    }
}