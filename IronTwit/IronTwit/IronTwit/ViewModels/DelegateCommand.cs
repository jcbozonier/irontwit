using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace IronTwit.ViewModels
{
    public class Do<T> : ICommand
    {
        private Action<T> _Action;
        private Predicate<T> _Pred;

        public Do(Action<T> action)
        {
            _Action = action;
        }

        public void If(Predicate<T> pred)
        {
            _Pred = pred;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            if(!(parameter is T)) 
                throw new ArgumentException("parameter doesn't match the type.");
            _Action((T) parameter);
        }

        public bool CanExecute(object parameter)
        {
            if(_Pred == null) 
                throw new NullReferenceException("You must provide a predicate to tell when an action can be carried out.");
            return _Pred((T) parameter);
        }
    }
}
