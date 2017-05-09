using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LocalNote
{
    class CheckCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private MainPageData mpd;

        //public bool enableEditbtn { get; set; }
        
        public CheckCommand(MainPageData mpd)
        {
            this.mpd = mpd;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            //enableEditbtn = true;
        }

        public void FireCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

    }
}
