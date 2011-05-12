using Relax;
using Relax.Impl;
using Symbiote.Core;
using Symbiote.Core.Actor;

namespace shortorder.domain.service.support
{
    public class CouchActorStore<T> : IActorStore<T>
        where T : class
    {
        public CouchProxy Couch { get; set; }
        public IKeyAccessor KeyAccessor { get; set; }

        public IMemento<T> Get<TKey>( TKey id )
        {
            return Couch.Get<IMemento<T>>( id.ToString() );
        }

        public void Store( IMemento<T> actor )
        {
            var id = KeyAccessor.GetId( actor.Retrieve() );
            var old = Couch.Get<IMemento<T>>( id ) as CouchDocument;
            if( old != null )
                ( actor as CouchDocument ).DocumentRevision = old.DocumentRevision;
            Couch.Persist( id, actor );
        }

        public CouchActorStore( CouchProxy couch, IKeyAccessor keyAccessor )
        {
            Couch = couch;
            KeyAccessor = keyAccessor;
        }
    }
}
