using System.Collections.ObjectModel;
using System.Windows.Threading;
using shortorder.wpf.cookui.WpfSupport;

namespace shortorder.wpf.cookui.ViewModel
{
    public interface IOrderCollectionViewModel : IClosableViewModelBase
    {
        ObservableCollection<IOrderViewModel> Orders { get; set; }
        Dispatcher Dispatcher { get; set; }
    }
}