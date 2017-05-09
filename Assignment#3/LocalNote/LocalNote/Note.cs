using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace LocalNote
{
    public class Note
    {
        public Note(int id, string title, StorageFile file)
        {
            ID = id;
            Title = title;
            File = file;
        }

        public int ID { get; }

        public string Title { get; set; }

        public string Content { get; set; }

        public StorageFile File { get; }
    }
}