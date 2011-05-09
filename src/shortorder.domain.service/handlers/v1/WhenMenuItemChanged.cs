using shortorder.messages;
using Symbiote.Core.UnitOfWork;
using Symbiote.Messaging;

namespace shortorder.domain.service.handlers.v1
{
    public class WhenMenuItemChanged : EventListenerBase<MenuItemChanged>
    {
        public IBus Bus { get; set; }

        public override void OnEvent( MenuItemChanged evnt )
        {
            Bus.Publish( "shortorder.events", evnt );
        }

        public WhenMenuItemChanged( IBus bus ) : base( false )
        {
            Bus = bus;
        }
    }
}