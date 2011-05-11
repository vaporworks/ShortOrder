using System.Collections.ObjectModel;
using shortorder.wpf.cookui.WpfSupport;

namespace shortorder.wpf.cookui.ViewModel
{
    public interface IOrderCollectionViewModel : IClosableViewModelBase
    {
        ObservableCollection<IOrderViewModel> Orders { get; set; }
    }
}