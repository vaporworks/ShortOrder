using System;
using System.Collections.Generic;
using System.Linq;
using shortoder.domain;
using Symbiote.Core;

namespace shortorder.domain.service
{
    public class OrderMemento : IMemento<Order>
    {
        public Guid Id { get; set; }
        public int Rank { get; set; }
        public string CustomerName { get; set; }
        public List<int> ItemIds { get; set; }

        public void Capture( Order instance )
        {
            Id = instance.Id;
            Rank = instance.Rank;
            ItemIds = instance.ItemIds.ToList();
            CustomerName = instance.CustomerName;
        }

        public void Reset( Order instance )
        {
            instance.Id = Id;
            instance.Rank = Rank;
            ItemIds = instance.ItemIds.ToList();
            instance.CustomerName = CustomerName;
        }

        public Order Retrieve()
        {
            return new Order() 
            { 
                Id = Id, 
                CustomerName = CustomerName, 
                Rank = Rank, 
                ItemIds = ItemIds };
        }
    }
}