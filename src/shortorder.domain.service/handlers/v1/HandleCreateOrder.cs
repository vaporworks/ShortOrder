using System;
using System.Linq;
using Relax;
using shortoder.domain;
using shortorder.messages;
using Symbiote.Core.Actor;
using Symbiote.Core.Memento;
using Symbiote.Core.UnitOfWork;
using Symbiote.Messaging;

namespace shortorder.domain.service.handlers.v1
{
    public class HandleCreateOrder : IHandle<CreateOrder>
    {
        public IDocumentRepository Couch { get; set; }
        public IMemoizer MementoProvider { get; set; }
        public IAgent<Order> OrderAgent { get; set; }

        public Action<IEnvelope> Handle( CreateOrder message )
        {
            try 
            {
                var order = new Order();
                using( var context = Context.CreateFor( order ) )
                {
                    order.Id = message.Id;
                    order.CustomerName = message.CustomerName;
                    order.ItemIds = message.Items.Select( s => new OrderItem() {ItemId = s.ItemId, Qty = s.Qty} ).ToList();

                    Couch.Save( order.Id, MementoProvider.GetMemento( order ) );

                    OrderAgent.RegisterActor( order.Id, order );

                    context.PublishOnCommit<OrderCreated>( x => 
                    {
                        x.ActorId = message.Id.ToString();
                        x.CustomerName = message.CustomerName;
                        x.Items = message.Items.Select( s => new OrderItemCreated()
                                                                    {
                                                                        ItemId = s.ItemId,
                                                                        Qty = s.Qty
                                                                    } ).ToList();
                    } );

                    return x => x.Acknowledge();
                }
            }
            catch (Exception ex)
            {
                return x => x.Reject( ex.ToString() );
            }
        }

        public HandleCreateOrder( IDocumentRepository couch, IMemoizer mementoProvider, IAgent<Order> orderAgent )
        {
            Couch = couch;
            MementoProvider = mementoProvider;
            OrderAgent = orderAgent;
        }
    }
}