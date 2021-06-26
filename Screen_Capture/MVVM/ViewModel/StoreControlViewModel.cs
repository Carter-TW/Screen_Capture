using Screen_Capture.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Screen_Capture.MVVM.ViewModel
{
    public class StoreControlViewModel:BaseViewModel
    {
        #region 屬性
        public System.Drawing.Bitmap bitmap { get; set; }
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
        }
        private void SaveOnClipBoard(Window window)
        {
            window.Hide();
            //window.WindowState = WindowState.Minimized;
            System.Windows.Forms.Clipboard.SetImage(bitmap);
        }

        private void SaveFileDialog(Window window)
        {
            window.Hide();
            DateTime now = DateTime.Now;
         
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
                string filename = dlg.FileName;
            }
        }
        #endregion
        public StoreControlViewModel()
        {
            
        }
    }
}
