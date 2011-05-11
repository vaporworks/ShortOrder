using System;
using System.Collections.Generic;
using shortorder.wpf.cookui.ViewModel.Impl;
using shortorder.wpf.cookui.WpfSupport;

namespace shortorder.wpf.cookui.ViewModel
{
    public interface IOrderViewModel : IClosableViewModelBase
    {
        Guid Id { get; set; }
        int Rank { get; set; }
        string CustomerName { get; set; }
        IList<IOrderItemViewModel> Items { get; set; }
        DateTime TimeReceived { get; set; }
    }
}