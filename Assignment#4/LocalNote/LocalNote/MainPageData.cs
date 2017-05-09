using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;
using Windows.ApplicationModel.Core;
using Windows.UI.Popups;
using System.Collections.ObjectModel;
using Windows.Storage;
using Windows.Storage.Streams;
using SQLite.Net;
using System.IO;

namespace LocalNote
{
    //https://code.msdn.microsoft.com/windowsapps/Local-Data-Base-SQLite-for-5e6146aa

    public class MainPageData : INotifyPropertyChanged
    {

        private SQLiteConnection _DatabaseConnection;
        
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<Note> Notes { get; set; }
        //private StorageFolder _NotesFolder;
        private List<Note> _AllNotes = new List<Note>();
        private bool _NewNote = false;

        private Note _SelectedNote;
        public Note SelectedNote
        {
            get
            {
                return _SelectedNote;
            }
            set
            {
                _SelectedNote = value;
                if (SelectedNote == null)
                {
                    ActiveContent = "";
                    IsNewNote(true);
                }
                else
                {
                    ActiveContent = _SelectedNote.Content;
                    IsNewNote(false);
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedNote"));
            }
        }
        public ICommand AddNoteCommand { get; set; }
        public ICommand SaveNoteCommand { get; set; }
        public ICommand EditNoteCommand { get; set; }
        public ICommand DeleteNoteCommand { get; set; }
        public ICommand AboutCommand { get; set; }
        public ICommand ExitCommand { get; set; }

        private string _ActiveContent;
        public string ActiveContent
        {
            get
            {
                return _ActiveContent;
            }
            set
            {
                _ActiveContent = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ActiveContent"));
            }
        }

        private bool _AddButtonIsEnabled;
        public bool AddButtonIsEnabled
        {
            get
            {
                return _AddButtonIsEnabled;
            }
            set
            {
                _AddButtonIsEnabled = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AddButtonIsEnabled"));
            }
        }

        private bool _SaveButtonIsEnabled;
        public bool SaveButtonIsEnabled
        {
            get
            {
                return _SaveButtonIsEnabled;
            }
            set
            {
                _SaveButtonIsEnabled = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SaveButtonIsEnabled"));
            }
        }

        private bool _EditButtonIsEnabled;
        public bool EditButtonIsEnabled
        {
            get
            {
                return _EditButtonIsEnabled;
            }
            set
            {
                _EditButtonIsEnabled = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("EditButtonIsEnabled"));
            }
        }

        private bool _DeleteButtonIsEnabled;
        public bool DeleteButtonIsEnabled
        {
            get
            {
                return _DeleteButtonIsEnabled;
            }
            set
            {
                _DeleteButtonIsEnabled = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DeleteButtonIsEnabled"));
            }
        }

        private bool _SearchBoxIsEnabled;
        public bool SearchBoxIsEnabled
        {
            get
            {
                return _SearchBoxIsEnabled;
            }
            set
            {
                _SearchBoxIsEnabled = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SearchBoxIsEnabled"));
            }
        }

        private bool _NoteContentIsReadyOnly;
        public bool NoteContentIsReadOnly
        {
            get
            {
                return _NoteContentIsReadyOnly;
            }
            set
            {
                _NoteContentIsReadyOnly = value;
                SaveButtonIsEnabled = !value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NoteContentIsReadOnly"));
            }
        }
        private string _Filter;
        public string Filter
        {
            get
            {
                return _Filter;
            }
            set
            {
                if (value == _Filter)
                {
                    return;
                }
                _Filter = value;
                PropertyChanged?.Invoke(this,
                    new PropertyChangedEventArgs(nameof(Filter)));
                PerformFiltering();
            }
        }

        private void PerformFiltering()
        {
            if (_Filter == null)
            {
                _Filter = "";
            }
            var lowerCaseFilter = Filter.ToLowerInvariant().Trim();
            var result = _AllNotes.Where(
                note => note.Title.ToLowerInvariant()
                        .Contains(lowerCaseFilter)).ToList();
            var toRemove = Notes.Except(result).ToList();
            foreach (var note in toRemove)
            {
                Notes.Remove(note);
            }
            // Add back in the correct order.
            var resultcount = result.Count;
            for (int i = 0; i < resultcount; i++)
            {
                var resultItem = result[i];
                if (i + 1 > Notes.Count || !Notes[i].Equals(resultItem))
                {
                    Notes.Insert(i, resultItem);
                }
            }
        }
        
        private void RefreshNoteList()
        {
            TableQuery<Note> notes = _DatabaseConnection.Table<Note>();
            foreach (Note databaseNote in notes)
            {
                bool noteFound = false;
                foreach (Note uiNote in _AllNotes)
                {
                    if (databaseNote.ID == uiNote.ID)
                        noteFound = true;
                }
                if (!noteFound)
                    _AllNotes.Add(databaseNote);
            }
            PerformFiltering();
            if (_NewNote)
            {
                SelectedNote = _AllNotes.Last();
                _NewNote = false;
            }
            //StorageFolder appFolder = ApplicationData.Current.LocalFolder;
            //if (_NotesFolder == null)
            //{
            //    try
            //    {
            //        _NotesFolder = await appFolder.GetFolderAsync("Notes");
            //    }
            //    catch
            //    {
            //        _NotesFolder = await appFolder.CreateFolderAsync("Notes");
            //    }
            //}
            //IReadOnlyList<StorageFile> notesList = await _NotesFolder.GetFilesAsync();
            //for (int i = 0; i < notesList.Count; i++)
            //{
            //    StorageFile currentNoteFile = notesList.ElementAt(i);
            //    bool noteExists = false;
            //    foreach (Note note in _AllNotes)
            //    {
            //        if (note.File.Name == currentNoteFile.Name)
            //        {
            //            noteExists = true;
            //            break;
            //        }
            //    }
            //    if (!noteExists)
            //    {
            //        string currentNoteName = currentNoteFile.Name.Substring(0, currentNoteFile.Name.LastIndexOf("."));
            //        Note currentNote = new Note(i + 1, currentNoteName, currentNoteFile);
            //        currentNote.Content = await FileIO.ReadTextAsync(currentNoteFile);
            //        _AllNotes.Add(currentNote);
            //    }
            //}
            //PerformFiltering();
        }

        private void IsNewNote(bool flag)
        {
            DeleteButtonIsEnabled = !flag;
            AddButtonIsEnabled = !flag;
            EditModeIsEnabled(flag);
        }

        public void CreateNewNote(string title, string content)
        {
            Note newNote = new Note()
            {
                Title = title,
                Content = content
            };
            _DatabaseConnection.Insert(newNote);
            _NewNote = true;
            RefreshNoteList();

            //int newNoteId = 1;
            //if (_AllNotes.Count > 0)
            //    newNoteId = _AllNotes.Last().ID + 1;
            //StorageFile newNoteFile = await _NotesFolder.CreateFileAsync(title + ".txt");
            //Note newNote = new Note(newNoteId, title, newNoteFile);
            //await WriteToNote(newNote, content);
            //_AllNotes.Add(newNote);
            //await RefreshNoteList();
            //return newNote;
        }

        public void WriteToNote(Note note, string content)
        {
            note.Content = content;
            _DatabaseConnection.Update(note);

            //using (StorageStreamTransaction storageStreamTransaction = await note.File.OpenTransactedWriteAsync())
            //{
            //    using (DataWriter dataWriter = new DataWriter(storageStreamTransaction.Stream))
            //    {
            //        note.Content = content;
            //        dataWriter.WriteString(content);
            //        storageStreamTransaction.Stream.Size = await dataWriter.StoreAsync();
            //        await storageStreamTransaction.CommitAsync();
            //    }
            //}
        }

        private void TryCreateDb()
        {
            string databasePath = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "LocalNote.sqlite");
            _DatabaseConnection = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), databasePath);
            _DatabaseConnection.CreateTable<Note>();
        }

        public void DeleteNote(Note note)
        {
            _DatabaseConnection.Delete(note);
            _AllNotes.Remove(note);
            RefreshNoteList();

            //StorageFile noteToDeleteFile = await _NotesFolder.GetFileAsync(note.Title + ".txt");
            //await noteToDeleteFile.DeleteAsync();
            //_AllNotes.Remove(note);
            //await RefreshNoteList();
        }

        private void InitializeNotes()
        {
            Notes = new ObservableCollection<Note>();
            SelectedNote = null;
            RefreshNoteList();
            //await RefreshNoteList();
        }

        public MainPageData()
        {
            SearchBoxIsEnabled = true;
            EditModeIsEnabled(true);

            AddNoteCommand = new CheckCommand(ExecuteNewNote, CanExecuteNewNote);
            EditNoteCommand = new CheckCommand(ExecuteEditNote, CanExecuteEditNote);
            SaveNoteCommand = new CheckCommand(ExecuteSaveNote, CanExecuteSaveNote);
            DeleteNoteCommand = new CheckCommand(ExecuteDeleteNote, CanExecuteDeleteNote);
            AboutCommand = new CheckCommand(ExecuteAbout, CanExecuteAbout);
            ExitCommand = new CheckCommand(ExecuteExit, CanExecuteExit);

            TryCreateDb();
            InitializeNotes();
        }

        private bool CanExecuteExit(object obj)
        {
            return true;
        }

        private void ExecuteExit(object obj)
        {
            CoreApplication.Exit();
        }

        private bool CanExecuteAbout(object obj)
        {
            return true;
        }

        private async void ExecuteAbout(object obj)
        {
            MessageDialog aboutDialog = new MessageDialog("Created by Minsu Lee (w0293156)");
            await aboutDialog.ShowAsync();
        }

        private bool CanExecuteDeleteNote(object obj)
        {
            return true;
        }

        private async Task DeleteDialog()
        {
            ContentDialog deleteDialog = new ContentDialog()
            {
                Title = "Delete Note",
                Content = "Would you like to delete this note?",
                PrimaryButtonText = "Yes",
                SecondaryButtonText = "No"
            };
            var result = await deleteDialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                //await DeleteNote(_SelectedNote);
                DeleteNote(_SelectedNote);
                SelectedNote = null;
                NoteContentIsReadOnly = false;
            }
        }

        private async void ExecuteDeleteNote(object obj)
        {
            await DeleteDialog();
        }

        private bool CanExecuteSaveNote(object obj)
        {
            return true;
        }

        private async Task TitleDialog()
        {
            TextBox textBox = new TextBox { Height = 32, AcceptsReturn = false };
            ContentDialog titleDialog = new ContentDialog()
            {
                Title = "Note Title",
                Content = textBox,
                PrimaryButtonText = "Create Note",
                SecondaryButtonText = "Cancel"
            };
            ContentDialogResult result = await titleDialog.ShowAsync();
            bool canCreate = true;
            if (result == ContentDialogResult.Primary)
            {
                string newTitle = textBox.Text;
                for (int i = 0; i < _AllNotes.Count; i++)
                {
                    if (newTitle == _AllNotes.ElementAt(i).Title)
                    {
                        canCreate = false;
                        break;
                    }
                }
                if (canCreate)
                {
                    //SelectedNote = await CreateNewNote(newTitle, _ActiveContent);
                    CreateNewNote(newTitle, _ActiveContent);
                }
                else
                {
                    MessageDialog duplicateNameDialog = new MessageDialog("Please try a different title.");
                    await duplicateNameDialog.ShowAsync();
                    await TitleDialog();
                }
            }
        }

        private async void ExecuteSaveNote(object obj)
        {
            if (_SelectedNote == null)
                await TitleDialog();
            else
                WriteToNote(_SelectedNote, _ActiveContent);
            //await WriteToNote(_SelectedNote, _ActiveContent);
            EditModeIsEnabled(false);
        }

        private bool CanExecuteEditNote(object obj)
        {
            return true;
        }

        private void EditModeIsEnabled(bool flag)
        {
            NoteContentIsReadOnly = !flag;
            EditButtonIsEnabled = !flag;
        }

        private void ExecuteEditNote(object obj)
        {
            EditModeIsEnabled(true);
        }

        private bool CanExecuteNewNote(object obj)
        {
            return true;
        }

        private void ExecuteNewNote(object obj)
        {
            SelectedNote = null;
        }
    }
}