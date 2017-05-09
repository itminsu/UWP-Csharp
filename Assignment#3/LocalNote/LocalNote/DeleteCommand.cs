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
            return mpd.SelectedNote != null;
        }

        public async void Execute(object parameter)
        {
            // Create the message dialog and set its content
            MessageDialog DelDialog = new MessageDialog("Delete this note? ");

            // Add commands and set their callbacks; both buttons use the same callback function instead of inline event handlers
            DelDialog.Commands.Add(new UICommand("Yes") { Id = 0 });
            DelDialog.Commands.Add(new UICommand("No") { Id = 1 });

            // Set the command that will be invoked by default
            DelDialog.DefaultCommandIndex = 1;

            // Set the command to be invoked when escape is pressed
            DelDialog.CancelCommandIndex = 1;

            // Show the message dialog
            var result = await DelDialog.ShowAsync();
            if ((int)result.Id == 0)
            {
                RemoveNote();
            }
        }

        public void FireCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public async void RemoveNote()
        {
            if (mpd.SelectedNote.File != null)
            {
                await mpd.SelectedNote.File.DeleteAsync();
            }
            mpd.Notes.Remove(mpd.SelectedNote);
            mpd.SelectedNote = null;
            mpd.Search();
        }

    }
}
