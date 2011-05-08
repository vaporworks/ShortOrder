using System;

namespace shortorder.messages
{
    public class OrderRank
    {
        public Guid Id { get; set; }
        public string CustomerName { get; set; }
        public int Rank { get; set; }
    }
}