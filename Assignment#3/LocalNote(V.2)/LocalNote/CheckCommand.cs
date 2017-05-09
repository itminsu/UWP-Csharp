﻿using System;
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
        private readonly Predicate<object> _CanExecute;
        private readonly Action<object> _Execute;

        public CheckCommand(Action<object> execute, Predicate<object> canExecute)
        {
            _CanExecute = canExecute;
            _Execute = execute;
        }

        public CheckCommand(Action<object> execute) : this(execute, null) { }

        public bool CanExecute(object parameter)
        {
            return _CanExecute == null ? true : _CanExecute(parameter);
        }

        public void Execute(object parameter)
        {
            if (CanExecute(parameter))
            {
                _Execute(parameter);
            }
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, System.EventArgs.Empty);
        }
    }
}