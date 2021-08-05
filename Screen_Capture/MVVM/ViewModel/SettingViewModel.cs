using Screen_Capture.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Screen_Capture.MVVM.ViewModel
{
    public class SettingViewModel:BaseViewModel
    {
        #region  屬性
        private HotKeyViewModel hotkey; 
        private object _currentview;

        public object currentview 
        {
            get { return _currentview; }
            set { _currentview = value;  OnPropertyChange(); }
        }

        #endregion

        #region command declare

        public DelegateCommand<Window> CloseWindowCommand
        {
            get { return new DelegateCommand<Window>(CloseWindow); }
        }

        public DelegateCommand<Window> Drag_Command
        {
            get { return new DelegateCommand<Window>(Drag); }
        }
        #endregion
        #region  command function

        private void CloseWindow(Window window)
        {
            if (window != null)
            {

                if (currentview is HotKeyViewModel)
                {
                    HotKeyViewModel hotkey = (HotKeyViewModel)currentview;
                    hotkey.CancelSetting_Command.CanExecute(null);
                    hotkey.CancelSetting_Command.Execute(window);
                }
                else window.Close();
                
            }
        }

        private void Drag( Window window)
        {
            window.DragMove();
        }
        #endregion
        public SettingViewModel()
        {
            hotkey = new HotKeyViewModel();
            
            currentview = hotkey;
        }
    }
}
