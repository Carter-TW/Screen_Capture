using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;

namespace Screen_Capture.Core
{
   public  class ScreenCapture
    {
        private   const int DWMWA_EXTENDED_FRAME_BOUNDS = 9;
        #region  Construction
        public ScreenCapture()
        {
            count = 1;
            FileName = "Image";
            FileName += count.ToString()+"-";
        }
        #endregion
        
        #region  Wind32 Function Declare 

        [System.Runtime.InteropServices.DllImport("user32.dll")]   
        private  static extern Boolean GetWindowRect(IntPtr hWnd, ref Rectangle bounds);  //  得到視窗大小

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, ExactSpelling = true)]
        private static extern IntPtr GetForegroundWindow();//  得到最上層視窗

        [System.Runtime.InteropServices.DllImport("dwmapi.dll")]
        // 去掉視窗所擁有的陰影面積
        private static extern int DwmGetWindowAttribute(IntPtr hwnd, int dwAttribute, out Rectangle pvAttribute, int cbAttribute);

        [System.Runtime.InteropServices.DllImport("User32.dll")]
        private static extern IntPtr GetDC(IntPtr Hwnd); //其在MSDN中原型為HDC GetDC(HWND hWnd),HDC和HWND都是驅動器句柄（長指標），在C#中只能用IntPtr代替了
        
        [System.Runtime.InteropServices.DllImport("User32.dll")]
        private static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);


        [System.Runtime.InteropServices.DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public static extern bool DeleteObject( IntPtr hObject);

        #endregion
        #region  Property
        public  IntPtr hwnd;
        private Rectangle bounds;
        private Bitmap bitmap;
        private string FileName;
        private int count;
        #endregion

        #region  private  Function
        private System.Windows.Media.ImageSource ConvertBitmapToImageSource(Bitmap bmp)
        {
            var handle = bmp.GetHbitmap();
            try
            {
                return Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            finally { DeleteObject(handle); }
        }

        private   void  ScreenShot()
        {
           
            using (Graphics gfx= Graphics.FromImage(bitmap))
            {
                gfx.CopyFromScreen(bounds.X, bounds.Y, 0, 0, bounds.Size, CopyPixelOperation.SourceCopy);
             
                DateTime now = DateTime.Now;
                FileName += now.ToString("yyyy-MM-dd-H-m");
                  count++;
                bitmap.Save(@"C:\Users\88690\Pictures\"+FileName+".png");
                FileName = "Image" + count.ToString()+"-";
            }
          
            //bitmap.Dispose();
        }
        #endregion
        #region  public Function
        public void RegionScreenShot(int x,int y,int width,int height)
        {
            hwnd = GetDC(IntPtr.Zero);
            bounds = new Rectangle(x, y, width, height);
            using (Graphics gfx=Graphics.FromHdc(hwnd))
            {
              
                Pen dash_pen = new Pen(Color.Red, 4);
                dash_pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                gfx.DrawRectangle(dash_pen, bounds);
            }
            ReleaseDC(IntPtr.Zero, hwnd);
        }
        public void WindowScreennShot()
        {
            hwnd = GetForegroundWindow();
            if (DwmGetWindowAttribute(hwnd, DWMWA_EXTENDED_FRAME_BOUNDS, out bounds, System.Runtime.InteropServices.Marshal.SizeOf(typeof(Rectangle))) != 0)
            {
                GetWindowRect(hwnd, ref bounds);
            }
            bounds.Width = bounds.Width - bounds.X;
            bounds.Height = bounds.Height - bounds.Y;
            bitmap = new Bitmap(bounds.Width, bounds.Height);
            ScreenShot();
        }
        public void FullScreenShot()
        {

            bounds = new Rectangle((int)Screen.PrimaryScreen.Bounds.X, (int)Screen.PrimaryScreen.Bounds.Y, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            bitmap = new Bitmap((int)bounds.Width,(int)bounds.Height);
            ScreenShot();
            ConvertBitmapToImageSource(bitmap);
        }

        #endregion

    }
}
