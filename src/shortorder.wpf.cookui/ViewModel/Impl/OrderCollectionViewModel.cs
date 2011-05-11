using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Threading;
using Microsoft.Practices.Prism.Events;
using shortorder.wpf.cookui.Events;
using shortorder.wpf.cookui.Factory;
using shortorder.wpf.cookui.WpfSupport.Impl;
using Symbiote.Messaging;
using Symbiote.Rabbit;

namespace shortorder.wpf.cookui.ViewModel.Impl
{
    public class OrderCollectionViewModel : ClosableViewModelBase, IOrderCollectionViewModel
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IOrderViewModelFactory _orderViewModelFactory;
        private IBus _bus;

        public ObservableCollection<IOrderViewModel> Orders { get; set; }

        public int OrderCount
        {
            get { return Orders.Count; }
        }

        public OrderCollectionViewModel(IEventAggregator eventAggregator, IOrderViewModelFactory orderViewModelFactory, IBus bus)
        {
            _eventAggregator = eventAggregator;
            _orderViewModelFactory = orderViewModelFactory;
            Orders = new ObservableCollection<IOrderViewModel>();
            _bus = bus;
            WireUpOrderEvents();
            WireUpMessageBus();
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
                    _eventAggregator.GetEvent<OrderMadeEvent>().Publish( new OrderMadeDefinition()
                                                                             {
                                                                                 CustomerName = order.CustomerName,
                                                                                 Id = order.Id,
                                                                                 TimeMade = DateTime.Now
                                                                             });
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
            if(!Dispatcher.CurrentDispatcher.CheckAccess())
            {
                Dispatcher.CurrentDispatcher.BeginInvoke( DispatcherPriority.Normal, new Action(() => HandleIncomingOrder( orderDefinition )));
            }
            else
            {
                var order = _orderViewModelFactory.GetOrderViewModelFromValues(( orderDefinition.Id),
                                                                          orderDefinition.CustomerName,
                                                                          orderDefinition.TimeReceived,
                                                                          orderDefinition.Rank );
                order.Items = orderDefinition.Items.Select(s => _orderViewModelFactory.GetOrderItemViewModelFromValue(s.Description, s.ItemId, s.Qty)).ToList();
            }
        }
    }
}