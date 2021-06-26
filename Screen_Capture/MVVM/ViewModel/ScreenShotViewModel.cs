using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Screen_Capture.Core;
namespace Screen_Capture.MVVM.ViewModel
{
    
   
    public class ScreenShotViewModel :BaseViewModel
    {
        #region 屬性
        private ScreenCapture screen;
   
        private CanvasViewModel CanvasView { get; set;  }
       

        #endregion
        #region  Declare Command
        public DelegateCommand<Window> FullScreenShot_Command
        {
            get { return new DelegateCommand<Window>(FullScreemShot); }
        }
        public DelegateCommand<Window> WindowScreenShot_Command
        {
            get { return new DelegateCommand<Window>(WindowScreenShot); }
        }

        public DelegateCommand<Window> RegionScreenShot_Command
        {
            get { return new DelegateCommand<Window>(RegionScreenShot); }
        }


        #endregion

        #region command function 
        private void FullScreemShot(Window window)
        {
            window.Hide(); //隱藏應用程式
            screen.FullScreenShot();
            screen.SaveImage();
        }
        private void WindowScreenShot(Window window)
        {
            window.Hide(); //隱藏應用程式
            screen.WindowScreennShot();
            screen.SaveImage();
        }


        private void RegionScreenShot(Window window)
        {
            window.Hide(); //隱藏應用程式
            screen.FullScreenShot(); //先全螢幕截圖
            MainViewModel tmp = (MainViewModel)window.DataContext;
            tmp.Image=screen.GetImageSource(); 
            CanvasView.screen = screen; //ScreenShotViewModel的screen傳給CanvasViewModel
          
            window.WindowState = WindowState.Maximized; //將放置螢幕截圖的canvas最大化
            tmp.currentview = CanvasView;
            window.DataContext = tmp;
            window.Show();

            /*

            screen.FullScreenShot(); //先全螢幕截圖
            canvasWindow = new CanvasWindow(); //創造canvas window
            CanvasView.Image = screen.GetImageSource(); // imagesouce 給cvs windwo 的ViewModel
            CanvasView.screen.FullScreenShot(); //cvs也用螢幕截圖，這樣在canvas window 上的區域截圖完成後好做截圖
            CanvasView.window = canvasWindow; //將cvs的window給到cvs的viewModel
            canvasWindow.DataContext = CanvasView; //將MainWindow上的cvs viewModel 給到cvs window 的DataContext 才會更新畫面
            canvasWindow.Show();
            window.Hide();
            */

        }
        #endregion
        public ScreenShotViewModel()
        {
            screen = new ScreenCapture();
            CanvasView = new CanvasViewModel();
        }
    }
}
