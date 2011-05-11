using System;
using System.Collections.Generic;
using System.Linq;
using shortorder.wpf.cookui.Events;
using shortorder.wpf.cookui.ViewModel;
using shortorder.wpf.cookui.ViewModel.Impl;
using Symbiote.Core;

namespace shortorder.wpf.cookui.Factory
{
    public class OrderViewModelFactory : IOrderViewModelFactory
    {
        public IOrderViewModel GetOrderViewModelFromValues(Guid id, string customerName, DateTime received, int rank)
        {
            var order = Assimilate.GetInstanceOf<IOrderViewModel>();
            order.CustomerName = customerName;
            order.Id = id;
            order.Rank = rank;
            order.TimeReceived = received;
            return order;
        }

        public IOrderItemViewModel GetOrderItemViewModelFromValue(string description, int itemId, int qty)
        {
            var orderItem = Assimilate.GetInstanceOf<IOrderItemViewModel>();
            orderItem.Description = description;
            orderItem.ItemId = itemId;
            orderItem.Qty = qty;
            return orderItem;
        }
    }
}