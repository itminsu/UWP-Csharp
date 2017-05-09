using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

namespace LocalNote
{
    class AddCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private MainPageData mpd;

        public AddCommand(MainPageData inMpd)
        {
            mpd = inMpd;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            TextBox titleInput = new TextBox { Height = 32, AcceptsReturn = false };
            ContentDialog titleDialog = new ContentDialog()
            {
                Title = "Note Title",
                Content = titleInput,
                PrimaryButtonText = "Create",
                SecondaryButtonText = "Cancel"
            };
            ContentDialogResult result = await titleDialog.ShowAsync();
            bool nameMatched = false;
            if (result == ContentDialogResult.Primary)
            {
                foreach (LocalNoteModel currentNote in mpd.Notes)
                {
                    if (titleInput.Text == currentNote.Title)
                    {
                        nameMatched = true;
                        break;
                    }
                }
                if (!nameMatched)
                {
                    await add(titleInput.Text);
                }
                else
                {
                    MessageDialog duplicateNameDialog = new MessageDialog("Already exists the note with that title");
                    await duplicateNameDialog.ShowAsync();
                    Execute(parameter);
                }
            }

        }

        public async Task add(string inTitle)
        {
            LocalNoteModel createdNote = new LocalNoteModel(mpd.Notes.Count, inTitle);
            StorageFile newNoteFile = await mpd.NotesFolder.CreateFileAsync(createdNote.Title + ".txt");
            createdNote.File = newNoteFile;
            mpd.Notes.Add(createdNote);
            mpd.SelectedNote = createdNote;
            mpd.Search();
        }

        public void FireCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
