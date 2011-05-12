using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Threading;
using Microsoft.Practices.Prism.Events;
using shortorder.messages;
using shortorder.wpf.cookui.Events;
using shortorder.wpf.cookui.Factory;
using shortorder.wpf.cookui.WpfSupport.Impl;
using Symbiote.Messaging;
using Symbiote.Messaging.Impl.Mesh;
using Symbiote.Rabbit;

namespace shortorder.wpf.cookui.ViewModel.Impl
{
    public class OrderCollectionViewModel : ClosableViewModelBase, IOrderCollectionViewModel
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IOrderViewModelFactory _orderViewModelFactory;
        private readonly IBus _bus;
        private readonly INode _node;

        public ObservableCollection<IOrderViewModel> Orders { get; set; }

        public int OrderCount
        {
            get { return Orders.Count; }
        }

        public OrderCollectionViewModel(IEventAggregator eventAggregator, IOrderViewModelFactory orderViewModelFactory, IBus bus, INode node)
        {
            _eventAggregator = eventAggregator;
            _orderViewModelFactory = orderViewModelFactory;
            Orders = new ObservableCollection<IOrderViewModel>();
            _bus = bus;
            _node = node;
            WireUpOrderEvents();
            WireUpMessageBus();
            Orders.Add(new OrderViewModel()
                           {
                               CustomerName = "Jim",
                               Id = Guid.NewGuid(),
                               Items = new List<IOrderItemViewModel>()
                                           {
                                               new OrderItemViewModel()
                                                   {
                                                       ItemId = 1,
                                                       Description = "Burgerz",
                                                       Qty = 2
                                                   },
                                               new OrderItemViewModel()
                                                   {
                                                       ItemId = 21,
                                                       Description = "Drinx",
                                                       Qty = 2
                                                   }
                                           }
                           });
            Orders.Add( new OrderViewModel()
                            {
                                CustomerName = "Alex",
                                Id = Guid.NewGuid(),
                                Items = new List<IOrderItemViewModel>()
                                            {
                                                new OrderItemViewModel()
                                                    {
                                                        ItemId = 1,
                                                        Description = "Burgerz",
                                                        Qty = 2
                                                    },
                                                new OrderItemViewModel()
                                                    {
                                                        ItemId = 21,
                                                        Description = "Drinx",
                                                        Qty = 2
                                                    },
                                                new OrderItemViewModel()
                                                    {
                                                        ItemId = 13,
                                                        Description = "Shaix",
                                                        Qty = 45
                                                    }
                                            }
                            } );
        }

        private void WireUpMessageBus()
        {
            _bus.AddRabbitChannel(x => x.Fanout("shortorder.events").Durable().PersistentDelivery());
            _bus.AddRabbitQueue(x => x.QueueName("shortorder.events")
                                      .ExchangeName("shortorder.events")
                                      .NoAck()
                                      .Durable()
                                      .StartSubscription());
        }

        private void WireUpOrderEvents()
        {
            Orders.CollectionChanged += HandleOrdersCollectionChanged;
            _eventAggregator.GetEvent<InComingOrderEvent>().Subscribe( HandleIncomingOrder );
        }

        private void HandleOrdersCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            if(args.Action == NotifyCollectionChangedAction.Remove)
            {
                HandleRemoval( args );
            }
            else if(args.Action == NotifyCollectionChangedAction.Add)
            {
                HandleAdditions( args );
            }
        }

        private void HandleAdditions( NotifyCollectionChangedEventArgs args )
        {
            foreach(var item in args.NewItems)
            {
                var order = item as IOrderViewModel;
                if(order != null)
                {
                    order.RequestClose += HandleOrderRequestClose;
                    var idx = Orders.IndexOf(order);
                    _bus.Publish( "shortorder.events", new OrderRanked()
                                                           {
                                                               Id = order.Id,
                                                               Rank = idx
                                                           } );
                }
            }
            OnPropertyChanged( "OrderCount" );
        }

        private void HandleRemoval( NotifyCollectionChangedEventArgs args )
        {
            foreach(var item in args.OldItems)
            {
                var order = item as IOrderViewModel;
                if(order != null)
                {
                    _node.Publish( new OrderMade()
                                       {
                                           Id = order.Id
                                       } );
                }
            }
            OnPropertyChanged("OrderCount");
        }

        private void HandleOrderRequestClose(object sender, EventArgs e)
        {
            var orderViewModel = sender as IOrderViewModel;
            if(orderViewModel != null)
            {
                Orders.Remove( orderViewModel );
            }
        }

        private void HandleIncomingOrder(InComingOrderDefinition orderDefinition)
        {
            var order = _orderViewModelFactory.GetOrderViewModelFromValues(( orderDefinition.Id),
                                                                          orderDefinition.CustomerName,
                                                                          orderDefinition.TimeReceived,
                                                                          orderDefinition.Rank );
            order.Items = orderDefinition.Items.Select(s => _orderViewModelFactory.GetOrderItemViewModelFromValue(s.Description, s.ItemId, s.Qty)).ToList();
            Orders.Add( order );

            //Execute.OnUIThread( () =>
            //{
                
            //} );
        }
    }
}