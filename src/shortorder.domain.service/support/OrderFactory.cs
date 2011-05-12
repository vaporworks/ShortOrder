using System;
using shortoder.domain;
using Symbiote.Core.Actor;

namespace shortorder.domain.service
{
    public class OrderFactory : IActorFactory<Order>
    {
        public Order CreateInstance<TKey>( TKey id )
        {
            return new Order()
                       {
                           Id = Guid.Parse( id.ToString() )
                       };
        }
    }
}