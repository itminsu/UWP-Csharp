using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalNote
{
    public class Note
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        //public Note(int id, string title, StorageFile file)
        //{
        //    ID = id;  
        //    Title = title;
        //    File = file;
        //}

        //public int ID { get; }

        //public string Title { get; set; }

        //public string Content { get; set; }

        //public StorageFile File { get; }
    }
}