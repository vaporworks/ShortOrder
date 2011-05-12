using System;
using System.Collections.Generic;
using Symbiote.Core.UnitOfWork;

namespace shortorder.messages
{
    public class OrderMade : EventBase
    {
        public Guid Id { get; set; }
    }
}