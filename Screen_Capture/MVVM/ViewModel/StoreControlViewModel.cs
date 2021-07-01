using Screen_Capture.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace Screen_Capture.MVVM.ViewModel
{
    public class StoreControlViewModel:BaseViewModel
    {
        #region 屬性
        private int x;
        private int y;
        private int width;
        private int height;
        private Canvas canvas;
        private Rectangle rect;
        private Visibility state;
        #endregion
        #region declare command 
        public DelegateCommand <Window>Bt_SaveOnClipBoard
        {
            get { return new DelegateCommand<Window>(SaveOnClipBoard); }
        }
        public DelegateCommand<Window>Bt_SaveFileDialog
        {
            get { return new DelegateCommand<Window>(SaveFileDialog); }
        }

        public DelegateCommand<Window>Bt_ExitRegionShot
        {
            get { return new DelegateCommand<Window>(ExitRegionShot); }
        }
        #endregion
        #region  command function
        private void ExitRegionShot(Window window )
        {
                window.Hide();
            canvas.Children.Remove(rect);
        }
        private void SaveOnClipBoard(Window window)
        {
            window.Hide();
            canvas.Children.Remove(rect);
            state = Visibility.Hidden;
            MainViewModel.Global_Screen.RegionScreenShot(x, y, width, height);
            //window.WindowState = WindowState.Minimized;
            System.Windows.Forms.Clipboard.SetImage(MainViewModel.Global_Screen.bitmap);
        }

        private void SaveFileDialog(Window window)
        {
            window.Hide();
            canvas.Children.Remove(rect);
            state = Visibility.Hidden;
            DateTime now = DateTime.Now;
            MainViewModel.Global_Screen.RegionScreenShot(x, y, width, height);
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "Image"; // Default file name
            dlg.FileName += now.ToString("yyyy-MM-dd-H-m");
            dlg.DefaultExt = ".png"; // Default file extension
            dlg.Filter = "PNG (.png)|*.png"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                MainViewModel.Global_Screen.bitmap.Save(dlg.FileName);
            }
        }
        #endregion
        #region function
        public void Init_Region(int xx ,int yy ,int w ,int h ,Canvas c ,Rectangle r, Visibility  s)
        {
            x = xx;
            y = yy;
            width = w;
            height = h;
            canvas = c;
            rect = r;
           state = s;
        }
        #endregion
        public StoreControlViewModel()
        {
           
        }
    }
}
