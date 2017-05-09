using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Popups;

namespace LocalNote
{
    class DeleteCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private MainPageData mpd;
        public DeleteCommand(MainPageData inMpd)
        {
            mpd = inMpd;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            // Create the message dialog and set its content
            var DeleteDialog = new MessageDialog("Delete note?");

            // Add commands and set their callbacks; both buttons use the same callback function instead of inline event handlers
            DeleteDialog.Commands.Add(new UICommand(
                "Yes",
                new UICommandInvokedHandler(this.CommandInvokedHandler)));
            DeleteDialog.Commands.Add(new UICommand(
                "No",
                new UICommandInvokedHandler(this.CommandInvokedHandler)));

            // Set the command that will be invoked by default
            DeleteDialog.DefaultCommandIndex = 0;

            // Set the command to be invoked when escape is pressed
            DeleteDialog.CancelCommandIndex = 1;

            // Show the message dialog
            await DeleteDialog.ShowAsync();
        }

        public void FireCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        private void CommandInvokedHandler(IUICommand command)
        {
            // Display message showing the label of the command that was invoked
        }

    }
}
