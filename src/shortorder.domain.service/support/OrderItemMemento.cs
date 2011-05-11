using shortoder.domain;
using Symbiote.Core;

namespace shortorder.domain.service
{
    public class OrderItemMemento : IMemento<OrderItem>
    {
        public int ItemId { get; set; }
        public int Qty { get; set; }

        public void Capture( OrderItem instance )
        {
            ItemId = instance.ItemId;
            Qty = instance.Qty;
        }

        public void Reset( OrderItem instance )
        {
            instance.ItemId = ItemId;
            instance.Qty = ItemId;
        }

        public OrderItem Retrieve()
        {
            return new OrderItem()
            {
                ItemId = ItemId,
                Qty = Qty
            };
        }
    }
}