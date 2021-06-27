using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Screen_Capture.Core;
using System.Windows.Media;
using System.Windows.Input;
using Screen_Capture.MVVM.View;

namespace Screen_Capture.MVVM.ViewModel
{
 
    public class MainViewModel:BaseViewModel
    {
        #region  屬性
        private object _currentview;

        public object currentview  //主畫面的選項後出現的view
        {
            get { return _currentview; }
            set { _currentview = value; OnPropertyChange(); }
        }
        private StartViewModel startView { get; set; }
       private PaintViewModel paintView { get; set;  }
        private ImageSource _image;
        public ImageSource Image
        {
            get { return _image; }
            set { _image = value; OnPropertyChange(); }
        }
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
        public DelegateCommand<Window> Drag_Command
        {
            get { return new DelegateCommand<Window>(Window_MouseDown); }
        }
        #endregion

        #region command function
        public void Window_MouseDown(Window window)
        {


            if (window != null)
            {

                window.DragMove();
            }



        }
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
        public MainViewModel()
        {
            startView = new StartViewModel();
            paintView = new PaintViewModel();
            currentview = paintView;
           // currentview = startView;
        }
    }
}
