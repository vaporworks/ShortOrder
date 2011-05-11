using System;
using System.Windows.Input;

namespace shortorder.wpf.cookui.WpfSupport.Impl
{
    /// <summary>
    /// This ViewModelBase subclass requests to be removed 
    /// from the UI when its CloseCommand executes.
    /// This class is abstract.
    /// </summary>
    public abstract class ClosableViewModelBase : ViewModelBase, IClosableViewModelBase
    {
        #region Fields

        RelayCommand _closeCommand;

        #endregion // Fields

        #region CloseCommand

        /// <summary>
        /// Returns the command that, when invoked, attempts
        /// to remove this workspace from the user interface.
        /// </summary>
        public virtual ICommand CloseCommand
        {
            get { return _closeCommand ?? ( _closeCommand = new RelayCommand( param => OnRequestClose() ) ); }
        }

        #endregion // CloseCommand

        #region RequestClose [event]

        /// <summary>
        /// Raised when this workspace should be removed from the UI.
        /// </summary>
        public event EventHandler RequestClose;

        protected virtual void OnRequestClose()
        {
            if (RequestClose != null)
            {
                RequestClose( this, new ClosableViewModelEventArgs()
                                        {
                                            ViewModel = this
                                        } );
            }
        }

        #endregion // RequestClose [event]
    }
}