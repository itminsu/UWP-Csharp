using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Popups;

namespace LocalNote
{
    class SaveCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private MainPageData mpd;
        public SaveCommand(MainPageData inMpd)
        {
            mpd = inMpd;
        }

        public bool CanExecute(object parameter)
        {
            return mpd.SelectedNote != null && !mpd.CurrentNoteReadOnly;
        }

        public async void Execute(object parameter)
        {
            // Create the message dialog and set its content
            MessageDialog SavedDialog = new MessageDialog("Saved!");
            // Add commands and set their callbacks; both buttons use the same callback function instead of inline event handlers
            SavedDialog.Commands.Add(new UICommand("OK") { Id = 0 });
            // Set the command that will be invoked by default
            SavedDialog.DefaultCommandIndex = 0;
            // Show the message dialog
            await SavedDialog.ShowAsync();
        }

        public void FireCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public async Task save()
        {
            using (StorageStreamTransaction storageStreamTransaction = await mpd.SelectedNote.File.OpenTransactedWriteAsync())
            {
                using (DataWriter dataWriter = new DataWriter(storageStreamTransaction.Stream))
                {
                    mpd.SelectedNote.Content = mpd.CurrentNoteContent;
                    dataWriter.WriteString(mpd.CurrentNoteContent);
                    storageStreamTransaction.Stream.Size = await dataWriter.StoreAsync();
                    await storageStreamTransaction.CommitAsync();
                }
            }
        }
    }
}
