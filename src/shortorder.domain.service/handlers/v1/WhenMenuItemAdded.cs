using shortorder.messages;
using Symbiote.Core.UnitOfWork;
using Symbiote.Messaging;

namespace shortorder.domain.service.handlers.v1
{
    public class WhenMenuItemAdded : EventListenerBase<MenuItemAdded>
    {
        public IBus Bus { get; set; }

        public override void OnEvent( MenuItemAdded evnt )
        {
            Bus.Publish( "shortorder.events", evnt );
        }

        public WhenMenuItemAdded( IBus bus ) : base( false )
        {
            Bus = bus;
        }
    }
}