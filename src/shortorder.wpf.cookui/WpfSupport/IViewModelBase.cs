using System;
using System.ComponentModel;

namespace shortorder.wpf.cookui.WpfSupport
{
    public interface IViewModelBase : INotifyPropertyChanged, IDisposable
    {
        string DisplayName { get; set; }
    }
}