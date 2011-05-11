using shortorder.wpf.cookui.WpfSupport.Impl;

namespace shortorder.wpf.cookui.ViewModel.Impl
{
    public class OrderItemViewModel : ViewModelBase, IOrderItemViewModel
    {
        private int _itemId;
        private int _qty;
        private string _description;

        public int ItemId
        {
            get { return _itemId; }
            set {
                _itemId = value;
                OnPropertyChanged( "ItemId" );
            }
        }

        public int Qty
        {
            get { return _qty; }
            set
            {
                _qty = value;
                OnPropertyChanged("Qty");
                OnPropertyChanged("Label");
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged("Description");
                OnPropertyChanged("Label");
            }
        }

        public string Label
        {
            get
            {
                var msg = string.Format( "{0} {1}{2}", Qty, Description, Qty > 1 ? "s" : "" );
                return msg;
            }
        }
    }
}