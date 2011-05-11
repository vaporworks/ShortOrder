using shortorder.wpf.cookui.WpfSupport;

namespace shortorder.wpf.cookui.ViewModel
{
    public interface IOrderItemViewModel : IViewModelBase
    {
        int ItemId { get; set; }
        int Qty { get; set; }
        string Description { get; set; }
        string Label { get; }
    }
}