using System;
using System.Collections.Generic;
using System.Linq;
using Relax;
using shortoder.domain;
using Symbiote.Core;

namespace shortorder.domain.service
{
    public class OrderMemento : 
        CouchDocument,
        IMemento<Order>
    {
        public Guid Id { get; set; }
        public int Rank { get; set; }
        public string CustomerName { get; set; }
        public bool Complete { get; set; }
        public List<OrderItemMemento> OrderItemMementos { get; set; }

        public void Capture( Order instance )
        {
            DocumentId = instance.Id.ToString();
            Id = instance.Id;
            Rank = instance.Rank;
            CustomerName = instance.CustomerName;
            Complete = instance.Complete;
            OrderItemMementos.Clear();
            foreach(var item in instance.ItemIds)
            {
                var memento = new OrderItemMemento();
                memento.Capture( item );
                OrderItemMementos.Add( memento );
            }
        }

        public void Reset( Order instance )
        {
            instance.Id = Id;
            instance.Rank = Rank;
            instance.CustomerName = CustomerName;
            instance.Complete = Complete;
            instance.ItemIds = GetOrderItems();
        }

        public Order Retrieve()
        {
            return new Order()
                       {
                           Id = Id,
                           CustomerName = CustomerName,
                           Rank = Rank,
                           Complete = Complete,
                           ItemIds = GetOrderItems()
                       };
        }

        private IList<OrderItem> GetOrderItems()
        {
            return OrderItemMementos.Select( s => new OrderItem()
                                                      {
                                                          ItemId = s.ItemId,
                                                          Qty = s.Qty
                                                      } ).ToList();
        }

        public OrderMemento() 
        {
            OrderItemMementos = new List<OrderItemMemento>();
        }
    }
}