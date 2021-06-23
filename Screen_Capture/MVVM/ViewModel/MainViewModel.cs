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

        private CanvasWindow  _canvasWindow;

        public CanvasWindow canvasWindow
        {
            get { return  _canvasWindow; }
            set {  _canvasWindow = value;  OnPropertyChange(); }
        }

        private object    _currentview;

        public object    currentview
        {
            get { return _currentview; }
            set { _currentview = value; OnPropertyChange(); }
        }

        private  object _view;

        public object   view
        {
            get { return _view; }
            set { _view = value; OnPropertyChange(); }
        }
        

        private ScreenCapture screen;
     

    
        
        #region  Declare Command
        public DelegateCommand FullScreenShot_Command
        {
            get { return new DelegateCommand(FullScreemShot); }
        }
        public DelegateCommand WindowScreenShot_Command
        {
            get { return new DelegateCommand(WindowScreenShot); }
        }

        public DelegateCommand<Window> RegionScreenShot_Command
        {
            get { return new DelegateCommand<Window>(RegionScreenShot); }
        }

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
        #region  Command function 

        private void FullScreemShot()
        {
          
            screen.FullScreenShot();
            screen.SaveImage();
        }
        private void WindowScreenShot()
        {
            screen.WindowScreennShot();
            screen.SaveImage();
        }
     

        private void RegionScreenShot(Window window)
        {

            //screen.RegionScreenShot(300,300,500,500);
            // screen.SaveImage();

            screen.FullScreenShot();
            canvasWindow = new CanvasWindow();
            CanvasView.Image = screen.GetImageSource();
            CanvasView.screen.FullScreenShot();
            canvasWindow.DataContext = CanvasView;
            canvasWindow.Show();
      
        
            // window.Hide();


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
        
        private StartViewModel startview { get; set; }
        private CanvasViewModel CanvasView { get; set; }
        #endregion
        public MainViewModel()
        {
            screen = new ScreenCapture();
            CanvasView = new CanvasViewModel();
            startview = new StartViewModel();
            currentview = startview;
           
        }
    }
}
