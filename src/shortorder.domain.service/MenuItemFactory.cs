using shortoder.domain;
using Symbiote.Core.Actor;

namespace shortorder.domain.service
{
    public class MenuItemFactory : IActorFactory<MenuItem>
    {
        public MenuItem CreateInstance<TKey>( TKey id )
        {
            return new MenuItem() { Id = int.Parse( id.ToString() ) };
        }
    }
}