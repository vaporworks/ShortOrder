using System;
using shortoder.domain;
using shortorder.messages;
using Symbiote.Messaging;

namespace shortorder.domain.service.handlers.v1
{
    public class HandleOrderStatus : IHandle<Order, RequestOrderStatus>
    {
        public Action<IEnvelope> Handle( Order actor, RequestOrderStatus message )
        {
            return x => x.Reply( new OrderRank() { CustomerName = actor.CustomerName, Id = actor.Id, Rank = actor.Rank, Complete = actor.Complete } );
        }
    }
}
