using System;
using shortoder.domain;
using shortorder.messages;
using Symbiote.Messaging;

namespace shortorder.domain.service.handlers.v1
{
    public class HandleOrderStatusChange : IHandle<Order, OrderRanked>
    {
        public Action<IEnvelope> Handle( Order actor, OrderRanked message )
        {
            actor.Rank = message.Rank;
            return x => x.Acknowledge();
        }
    }
}