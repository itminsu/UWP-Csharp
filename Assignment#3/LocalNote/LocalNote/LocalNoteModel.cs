using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace LocalNote
{
    class LocalNoteModel
    {
        public int Id { get; }
        public string Title { get; set; }
        public string Content { get; set; }
        public StorageFile File { get; set; }

        public LocalNoteModel(int inID, string inTitle, string inContent)
        {
            Content = inContent;
            Id = inID;
            Title = inTitle;
        }

        //constructor for add
        public LocalNoteModel(int inID, string inTitle)
        {
            Id = inID;
            Title = inTitle;
        }
    }
}
