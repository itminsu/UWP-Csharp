using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Popups;

namespace LocalNote
{
    class MainPageData : INotifyPropertyChanged
    {
        public MainPageData()
        {
            AddNoteCommand = new AddCommand(this);
            EditNoteCommand = new EditCommand(this);
            DeleteNoteCommand = new DeleteCommand(this);
            SaveNoteCommand = new SaveCommand(this);
            Init();
        }

        public ObservableCollection<LocalNoteModel> Notes { get; set; }
        public ObservableCollection<LocalNoteModel> ShownNotes { get; set; }
        public StorageFolder NotesFolder { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        
        public AddCommand AddNoteCommand { get; }
        public SaveCommand SaveNoteCommand { get; }
        public EditCommand EditNoteCommand { get; }
        public DeleteCommand DeleteNoteCommand { get; }
        //public CheckCommand CheckCommand { get; }
        private LocalNoteModel _selectedNote;
        private bool _currentNoteReadOnly;
        //private bool _enableEditbtn;
        private string _currentNoteContent;
        private string _searchText = "";

        //selected note
        public LocalNoteModel SelectedNote
        {
            get
            {
                return _selectedNote;
            }
            set
            {
                if ((_selectedNote == null || _selectedNote.Content == CurrentNoteContent) && value != null)
                {
                    _selectedNote = value;
                    CurrentNoteContent = _selectedNote.Content;
                    PropertyChanged?.Invoke(this, 
                        new PropertyChangedEventArgs("SelectedNote"));
                    CurrentNoteReadOnly = true;
                    EditNoteCommand.FireCanExecuteChanged();
                    DeleteNoteCommand.FireCanExecuteChanged();
                    SaveNoteCommand.FireCanExecuteChanged();
                }
                else
                {
                    CurrentNoteContent = "";
                    CurrentNoteReadOnly = true;
                    _selectedNote = null;
                    EditNoteCommand.FireCanExecuteChanged();
                    DeleteNoteCommand.FireCanExecuteChanged();
                    SaveNoteCommand.FireCanExecuteChanged();
                }

            }
        }

        public bool CurrentNoteReadOnly
        {
            get
            {
                return _currentNoteReadOnly;
            }
            set
            {
                _currentNoteReadOnly = value;
                PropertyChanged?.Invoke(this, 
                    new PropertyChangedEventArgs("CurrentNoteReadOnly"));
                SaveNoteCommand.FireCanExecuteChanged();
            }
        }

        //public bool EnableEditbtn
        //{
        //    get
        //    {
        //        return _enableEditbtn;
        //    }
        //    set
        //    {
        //        _enableEditbtn = value;
        //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("EndableEditbtn"));
        //        SaveNoteCommand.FireCanExecuteChanged();
        //    }
        //}

        public string CurrentNoteContent
        {
            get
            {
                return _currentNoteContent;
            }
            set
            {
                _currentNoteContent = value;
                PropertyChanged?.Invoke(this, 
                    new PropertyChangedEventArgs("CurrentNoteContent"));
            }
        }

        

        public async void Init()
        {
            Notes = new ObservableCollection<LocalNoteModel>();
            ShownNotes = new ObservableCollection<LocalNoteModel>();
            await PopulateNotes();
            Search();
        }
        
        public async Task PopulateNotes()
        {
            Notes.Clear();//delete notes
            StorageFolder directory = ApplicationData.Current.LocalFolder;
            //Create folder for notes, or get folder for notes.
            if (NotesFolder == null)
            {
                try
                {
                    NotesFolder = await directory.GetFolderAsync("Notes");
                    Search();
                }
                catch
                {
                    NotesFolder = await directory.CreateFolderAsync("Notes");
                }
            }
            IReadOnlyList<StorageFile> files = await NotesFolder.GetFilesAsync();
            int id = 0;
            foreach (StorageFile currentFile in files)
            {
                LocalNoteModel noteToAdd = new LocalNoteModel(id, currentFile.Name.Substring(0, currentFile.Name.LastIndexOf(".")), await FileIO.ReadTextAsync(currentFile));
                noteToAdd.File = currentFile;
                Notes.Add(noteToAdd);
                id++;
            }
        }
        
        public string SearchText
        {
            get
            {
                return _searchText;
            }
            set
            {
                if (value == _searchText)
                {
                    return;
                }
                _searchText = value;
                PropertyChanged?.Invoke(this,
                    new PropertyChangedEventArgs("SearchText"));
                Search();
            }
        }
        public void Search()
        {

            if (_searchText == null)
            {
                _searchText = "";
            }
            var lowerCaseFilter = SearchText.ToLowerInvariant().Trim();
            var result = Notes.Where(
                note => note.Title.ToLowerInvariant()
                        .Contains(lowerCaseFilter)).ToList();
            var toRemove = Notes.Except(result).ToList();
            foreach (var x in toRemove)
            {
                Notes.Remove(x);
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
    }
}
