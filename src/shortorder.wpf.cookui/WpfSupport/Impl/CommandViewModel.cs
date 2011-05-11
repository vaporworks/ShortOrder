using System;
using System.Windows.Input;

namespace shortorder.wpf.cookui.WpfSupport.Impl
{
    public class CommandViewModel : ClosableViewModelBase, ICommandViewModel
    {
        public CommandViewModel()
        {

        }

        public CommandViewModel(string displayName, ICommand command)
        {
            if (command == null)
                throw new ArgumentNullException("command");

            base.DisplayName = displayName;
            Command = command;
        }

        public virtual ICommand Command { get; protected set; }
    }

    /*
     *  Thanks to Josh Smith for this class
     */
}
