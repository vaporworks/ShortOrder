using System;
using Symbiote.Core.UnitOfWork;

namespace shortorder.messages
{
    public class OrderRanked : EventBase
    {
        public Guid Id { get; set; }
        public int Rank { get; set; }
    }
}