using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalNote
{
    class MainPageData : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public List<LocalNoteModel> Notes { get; set; }
        private LocalNoteModel _SelectedNote;

        public bool enableEditbtn { get; set; }
        //public bool contentReadOnly { get; set; }
        public AddCommand AddNoteCommand { get; }
        public SaveCommand SaveNoteCommand { get; }
        public EditCommand EditNoteCommand { get; }
        public DeleteCommand DeleteNoteCommand { get; }
        //public CheckCommand CheckCommand { get; }

        public LocalNoteModel SelectedNote
        {
            get
            {
                return _SelectedNote;
            }
            set
            {
                _SelectedNote = value;
                //contentReadOnly = true;
                if (value == null)
                {
                    enableEditbtn = false;
                }
                else
                {
                    enableEditbtn = true;
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedNote"));
                //CheckCommand.FireCanExecuteChanged();
            }
        }
        // dummy data
        public MainPageData()
        {
            AddNoteCommand = new AddCommand(this);
            AddNoteCommand.FireCanExecuteChanged();
            Notes = new List<LocalNoteModel>();

            for (int i = 1; i <= 20; i++)
            {
                Notes.Add(new LocalNoteModel(i, "NOTE " + i, "This is NOTE " + i));
            }
        }
    }
}
