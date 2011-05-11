using System;
using System.Collections.Generic;
using shortorder.wpf.cookui.ViewModel;

namespace shortorder.wpf.cookui.Factory
{
    public interface IOrderViewModelFactory
    {
        IOrderViewModel GetOrderViewModelFromValues(Guid id, string customerName, DateTime received, int rank);

        IOrderItemViewModel GetOrderItemViewModelFromValue( string description, int itemId, int qty );
    }
}