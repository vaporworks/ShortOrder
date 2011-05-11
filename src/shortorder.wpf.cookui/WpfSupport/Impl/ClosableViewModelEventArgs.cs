using System;

namespace shortorder.wpf.cookui.WpfSupport.Impl
{
    public class ClosableViewModelEventArgs : EventArgs
    {
        public IClosableViewModelBase ViewModel { get; set; }
    }
}