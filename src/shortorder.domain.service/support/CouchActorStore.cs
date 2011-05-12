using System;
using Relax;
using Relax.Impl;
using shortoder.domain;
using Symbiote.Core;
using Symbiote.Core.Actor;

namespace shortorder.domain.service.support
{
    public class OrderStore : IActorStore<Order>
    {
        public CouchProxy Couch { get; set; }
        public IKeyAccessor KeyAccessor { get; set; }

        IMemento<Order> IActorStore<Order>.Get<TKey>( TKey id )
        {
            return Couch.Get<OrderMemento>( id.ToString() );
        }

        public void Store( IMemento<Order> actor )
        {
            var id = KeyAccessor.GetId( actor.Retrieve() );
            var old = Couch.Get<OrderMemento>( id ) as CouchDocument;
            if( old != null )
                ( actor as CouchDocument ).DocumentRevision = old.DocumentRevision;
            Couch.Persist( id, actor );
        }

        public OrderStore( CouchProxy couch, IKeyAccessor keyAccessor )
        {
            Couch = couch;
            KeyAccessor = keyAccessor;
        }
    }
}
