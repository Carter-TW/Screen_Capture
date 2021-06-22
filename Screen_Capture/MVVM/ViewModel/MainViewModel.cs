using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Screen_Capture.Core;
using Screen_Capture.MVVM.ViewModel;

namespace Screen_Capture.ViewModel
{
    public class MainViewModel:BaseViewModel
    {
        private object _currentView;
     
        public object currentView
        {
            get { return _currentView; }
            set {
                _currentView = value;
                OnPropertyChange(); 
            }
        }
        public StartViewModel StartView { get; set; }
        #region  Declare Command
        public DelegateCommand<Window> MaxWindowCommand
        {
            get { return new DelegateCommand<Window>(MaxWindow); }
        }

        public DelegateCommand<Window>  MiniWindowCommand
        {
            get { return new DelegateCommand<Window>(MiniWindow ) ; }
        }
        public DelegateCommand<Window>CloseWindowCommand
        {
            get { return new DelegateCommand<Window>(CloseWindow); }
        }
        public DelegateCommand<MouseEventArgs> MouseLeftClick
        {
            get { return new DelegateCommand<MouseEventArgs>(GetPosition); }
        }

        #endregion
        #region  Command function 
        private void MiniWindow(Window window)
        {
            
            if (window != null)
            {
                window.WindowState = WindowState.Minimized;
            }
        }

        private void CloseWindow(Window window)
        {
             if(window!=null)
            {
                window.Close();
            }
        }
        private void MaxWindow(Window window)
        {
            if (window != null)
            {
                if (window.WindowState == WindowState.Maximized) window.WindowState = WindowState.Normal;
                else window.WindowState = WindowState.Maximized;
            }
                
        }

        [System.Runtime.InteropServices.DllImport("User32")]
        private extern static bool GetCursorPos(out MousePoint point);
        private  struct MousePoint
        {
            public int x;
            public int y;
        };

        private void GetPosition(MouseEventArgs e)
        {
            

        }
        #endregion
        public MainViewModel()
        {
            StartView = new StartViewModel();
            currentView = StartView;
        }
        


    }
}
