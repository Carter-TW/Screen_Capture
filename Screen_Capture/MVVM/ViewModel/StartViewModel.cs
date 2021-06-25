using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Screen_Capture.Core;
namespace Screen_Capture.MVVM.ViewModel
{
   public  class StartViewModel:BaseViewModel
    {
        #region 屬性
        private object  _funcView;

        public object funcView
        {
            get { return  _funcView; }
            set {  _funcView = value; OnPropertyChange(); }
        }
        private ScreenShotViewModel screenShotView { get; set; }
        #endregion
        #region Command Declare
        public DelegateCommand<Window> MaxWindowCommand
        {
            get { return new DelegateCommand<Window>(MaxWindow); }
        }

        public DelegateCommand<Window> MiniWindowCommand
        {
            get { return new DelegateCommand<Window>(MiniWindow); }
        }
        public DelegateCommand<Window> CloseWindowCommand
        {
            get { return new DelegateCommand<Window>(CloseWindow); }
        }
        #endregion

        #region command function
        private void MiniWindow(Window window)
        {

            if (window != null)
            {
                window.WindowState = WindowState.Minimized;
            }
        }

        private void CloseWindow(Window window)
        {
            if (window != null)
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

        #endregion
        public StartViewModel()
        {
            screenShotView = new ScreenShotViewModel();
            funcView = screenShotView;
        }
    }
}
