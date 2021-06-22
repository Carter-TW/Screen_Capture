using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
namespace Screen_Capture.Core
{
    public class DelegateCommand:DelegateCommand<Object>
    {
        
        public DelegateCommand(Action action):base(o=>action() )
        { }
        public DelegateCommand(Action action , Func<bool> func):base(o=>action() ,o=>func())
        {

        }
    }

    public class DelegateCommand<T> : ICommand
    {
        private readonly Action<T> execute;
        private readonly  Func<T, bool> canexecute;

        public DelegateCommand(Action<T> e , Func<T,bool>f)
        {
                execute=e ;
                canexecute=f ;
          }
         public  DelegateCommand(Action<T> e):this(e,null)
        {

        }

        public    event EventHandler  CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }

            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

       public  bool  CanExecute(object parameter)
        {

            if ( canexecute == null)
            {
            
                return true;
            }
            else return canexecute((T)parameter);
            
            
          
         
        }

        public    void  Execute(object parameter)
        {
            execute((T)parameter);
        }

    }
}
