using shortorder.messages;
using Symbiote.Core.UnitOfWork;
using Symbiote.Messaging;

namespace shortorder.domain.service.handlers.v1
{
    public class WhenOrderCreated : EventListenerBase<OrderCreated>
    {
        public IBus Bus { get; set; }

        public override void OnEvent( OrderCreated evnt )
        {
            Bus.Publish( "shortorder.events", evnt );
        }

        public WhenOrderCreated( IBus bus ) : base( false )
        {
            Bus = bus;
        }
    }
}