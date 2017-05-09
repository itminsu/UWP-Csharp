using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LocalNote
{
    class EditCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private MainPageData mpd;
        public EditCommand(MainPageData inMpd)
        {
            mpd = inMpd;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            //mpd.contentReadOnly = false;
        }

        public void FireCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
