using System.Windows.Input;

namespace shortorder.wpf.cookui.WpfSupport
{
    public interface ICommandViewModel : IClosableViewModelBase
    {
        ICommand Command { get; }
    }
}