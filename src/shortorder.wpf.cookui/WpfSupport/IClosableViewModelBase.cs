using System;
using System.Windows.Input;
using shortorder.wpf.cookui.WpfSupport.Impl;

namespace shortorder.wpf.cookui.WpfSupport
{
    public interface IClosableViewModelBase : IViewModelBase
    {
        /// <summary>
        /// Returns the command that, when invoked, attempts
        /// to remove this workspace from the user interface.
        /// </summary>
        ICommand CloseCommand { get; }

        /// <summary>
        /// Raised when this workspace should be removed from the UI.
        /// </summary>
        event EventHandler RequestClose;
    }
}