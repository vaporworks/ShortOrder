using shortoder.domain;
using Symbiote.Core;

namespace shortorder.domain.service
{
    public class MenuItemKeyAccessor : IKeyAccessor<MenuItem>
    {
        public string GetId( MenuItem actor )
        {
            return actor.Id.ToString();
        }

        public void SetId<TKey>( MenuItem actor, TKey key )
        {
            actor.Id = int.Parse( key.ToString() );
        }
    }
}