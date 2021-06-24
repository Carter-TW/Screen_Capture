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
        #region 屬性宣告
        
        private CanvasWindow  _canvasWindow;
        public CanvasWindow canvasWindow  //Region 全螢幕的window
        {
            get { return  _canvasWindow; }
            set {  _canvasWindow = value;  OnPropertyChange(); }
        }

        private object    _currentview;

        public object    currentview  //主畫面的選項後出現的view
        {
            get { return _currentview; }
            set { _currentview = value; OnPropertyChange(); }
        }

        private StartViewModel startview { get; set; } //選項畫面的ViewModel
        private CanvasViewModel CanvasView { get; set; }   //全螢幕畫面的viewModel
        private ScreenCapture screen;  //螢幕截圖class 
        #endregion





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

            

            screen.FullScreenShot(); //先全螢幕截圖
            canvasWindow = new CanvasWindow(); //創造canvas window
            CanvasView.Image = screen.GetImageSource(); // imagesouce 給cvs windwo 的ViewModel
            CanvasView.screen.FullScreenShot(); //cvs也用螢幕截圖，這樣在canvas window 上的區域截圖完成後好做截圖
            CanvasView.window = canvasWindow; //將cvs的window給到cvs的viewModel
            canvasWindow.DataContext = CanvasView; //將MainWindow上的cvs viewModel 給到cvs window 的DataContext 才會更新畫面
            canvasWindow.Show();
             window.Hide();


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
            screen = new ScreenCapture();
            CanvasView = new CanvasViewModel();
            startview = new StartViewModel();
            currentview = startview;
           
        }
    }
}
