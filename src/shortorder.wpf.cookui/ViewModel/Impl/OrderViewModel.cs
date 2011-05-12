using System;
using System.Collections.Generic;
using System.Windows.Input;
using shortorder.wpf.cookui.WpfSupport.Impl;

namespace shortorder.wpf.cookui.ViewModel.Impl
{
    public class OrderViewModel : ClosableViewModelBase, IOrderViewModel
    {
        private Guid _id;
        private int _rank;
        private string _customerName;
        private IList<IOrderItemViewModel> _items;
        private DateTime _timeReceived;

        public OrderViewModel()
        {
            _items = new List<IOrderItemViewModel>();
        }

        public Guid Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }

        public int Rank
        {
            get { return _rank; }
            set
            {
                _rank = value;
                OnPropertyChanged("Rank");
            }
        }

        public string CustomerName
        {
            get { return _customerName; }
            set
            {
                _customerName = value;
                OnPropertyChanged("CustomerName");
            }
        }

        public IList<IOrderItemViewModel> Items
        {
            get { return _items; }
            set
            {
                _items = value;
                OnPropertyChanged("Items");
            }
        }

        public DateTime TimeReceived
        {
            get { return _timeReceived; }
            set
            {
                _timeReceived = value;
                OnPropertyChanged( "TimeReceived" );
            }
        }
    }
}
