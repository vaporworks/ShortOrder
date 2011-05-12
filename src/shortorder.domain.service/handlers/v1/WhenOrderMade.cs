using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using shortoder.domain;
using shortorder.messages;
using Symbiote.Messaging;

namespace shortorder.domain.service.handlers.v1
{
    public class WhenOrderMade : IHandle<Order, OrderMade>
    {
        public Action<IEnvelope> Handle( Order actor, OrderMade message )
        {
            actor.Complete = true;
            return x => x.Acknowledge();
        }
    }
}
