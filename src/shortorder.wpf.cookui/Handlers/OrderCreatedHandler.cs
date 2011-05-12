using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using Microsoft.Practices.Prism.Events;
using shortorder.messages;
using shortorder.wpf.cookui.Events;
using Symbiote.Messaging;

namespace shortorder.wpf.cookui.Handlers
{
    public class OrderCreatedHandler : IHandle<OrderCreated>
    {
        private readonly IEventAggregator _eventAggregator;

        public OrderCreatedHandler(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        public Action<IEnvelope> Handle(OrderCreated message)
        {
            try
            {
                HandleIncomingOrder( message );
                
                return m => m.Acknowledge();
            }
            catch ( Exception ex)
            {
                return m => m.Reject( ex.ToString() );
            }
        }

        private void HandleIncomingOrder(OrderCreated message)
        {
            Execute.OnUIThread( () =>
                {
                    var msg = new InComingOrderDefinition()
                    {
                        CustomerName = message.CustomerName,
                        Id = message.Id,
                        Rank = message.Rank,
                        TimeReceived = DateTime.Now,
                        Items = message.Items.Select(s => new InComingOrderItemDefinition()
                        {
                            Description = s.Description,
                            ItemId = s.ItemId,
                            Qty = s.Qty
                        }).ToList()
                    };

                    _eventAggregator.GetEvent<InComingOrderEvent>().Publish(msg);
                });
        }
    }
}
