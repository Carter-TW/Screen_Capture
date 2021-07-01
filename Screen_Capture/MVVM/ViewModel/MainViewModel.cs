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
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.IO;
using Gma.System.MouseKeyHook;
using System.Text.RegularExpressions;
namespace Screen_Capture.MVVM.ViewModel
{

    public class MainViewModel : BaseViewModel
    {
        #region  屬性
        private string  _canvas_width;

        public string canvas_width
        {
            get { return _canvas_width; }
            set { _canvas_width = value; OnPropertyChange(); }
        }

        private string _canvas_height;

        public string canvas_height
        {
            get { return _canvas_height; }
            set { _canvas_height = value; OnPropertyChange(); }
        }

        public static ScreenCapture  Global_Screen{get;set ;}
        private object _currentview;
        private IKeyboardMouseEvents m_GlobalHook;
        public object currentview  //主畫面的選項後出現的view
        {
            get { return _currentview; }
            set { _currentview = value; OnPropertyChange(); }
        }
        private StartViewModel startView { get; set; }
       private PaintViewModel paintView { get; set;  }
        private bool[,] Check;
        private ImageSource _image;
        public ImageSource Image
        {
            get { return _image; }
            set { _image = value; OnPropertyChange(); }
        }
        #endregion

        #region Command Declare
        public DelegateCommand<Canvas> Save_Command
        {
            get { return  new DelegateCommand<Canvas>(Save_File);  }
        }
        public DelegateCommand Switch_Command
        {
            get { return new DelegateCommand(Switch_View); }
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
        public DelegateCommand<Window> Drag_Command
        {
            get { return new DelegateCommand<Window>(Window_MouseDown); }
        }

        public DelegateCommand<string> Checked_Command
        {
            get { return new DelegateCommand<string>(Checked_Fun); }
        }
        public DelegateCommand<string> UnChecked_Command
        {
            get { return new DelegateCommand<string>(UnChecked_Fun); }
        }
        public DelegateCommand New_Command
        {
            get { return new DelegateCommand(New_Canvas, Can_New); }
        }
     
        #endregion

        #region command function
       
        private bool Can_New()
        {
            if (double.Parse(canvas_width) > 100 && double.Parse(canvas_height)>100) return true;
            else return false;
        }
        private void New_Canvas()
        {
            DateTime now = DateTime.Now;
            string name = "Image"; // Default file name
           name+= now.ToString("yyyy-MM-dd-H-m");
            Console.WriteLine(canvas_height);
            paintView.NewTabItem(double.Parse(canvas_width),double.Parse( canvas_height), name);
            currentview = paintView;
        }
        public  void NumberValidationTextBox_Width(object sender, TextCompositionEventArgs e)
        {
            //"[^0-9]+"
            Regex regex = new Regex("^[0-9]+([0-9]+)?$");
            e.Handled = !regex.IsMatch(e.Text);
          
        }
        public void NumberValidationTextBox_Height(object sender, TextCompositionEventArgs e)
        {
            //"[^0-9]+"
            Regex regex = new Regex("^[0-9]+([0-9]+)?$");
            e.Handled = !regex.IsMatch(e.Text);
          
        }
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
        private void Switch_View()
        {
            if (currentview == startView) currentview = paintView;
            else currentview = startView;
        }

        private void Save_File(Canvas element)
        {
            RenderTargetBitmap bmp = new RenderTargetBitmap((int)element.ActualWidth, (int)element.ActualHeight, 96, 96, PixelFormats.Pbgra32);

            bmp.Render(element);

            //</ get Screenshot of Element >



            //< create Encoder >

            PngBitmapEncoder encoder = new PngBitmapEncoder();

            encoder.Frames.Add(BitmapFrame.Create(bmp));

            //</ create Encoder >
            DateTime now = DateTime.Now;

            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();

            dlg.FileName = "Image"; // Default file name
            dlg.FileName += now.ToString("yyyy-MM-dd-H-m");
            dlg.DefaultExt = ".png"; // Default file extension
            dlg.Filter = "PNG (.png)|*.png"; // Filter files by extension
            Nullable<bool> result = dlg.ShowDialog();

            //< save >
            if (result == true)
            {
                FileStream fs = new FileStream(dlg.FileName, FileMode.Create);

                encoder.Save(fs);

                fs.Close();
            }
        }
        #endregion

        #region function
        private bool Check_TextNumber(string str)
        {
            int i=int.Parse(str);
            return i >= 100   ;
        }
        public void Subscribe()
        {
            // Note: for the application hook, use the Hook.AppEvents() instead
            m_GlobalHook = Hook.GlobalEvents();

         //   m_GlobalHook.MouseDownExt += GlobalHookMouseDownExt;
            
            m_GlobalHook.KeyDown += GlobalHookKeyPress;
        }

        private void GlobalHookKeyPress(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            //RControlKey  LControlKey
            //RShiftKey  LShiftKey
            //RMenu LMenu
            //PrintScrenn
            //   Console.WriteLine("KeyPress: \t{0}", e.KeyData.ToString());
            ScreenShotViewModel tmp = startView.screenShotView as ScreenShotViewModel;

            if (Check_HotKey(0,e) && e.KeyCode== System.Windows.Forms.Keys.PrintScreen)
            {
                tmp.FullScreenShot_Command.Execute(Application.Current.MainWindow);
            }
           else  if (Check_HotKey(1, e) && e.KeyCode == System.Windows.Forms.Keys.PrintScreen)
            {
                tmp.RegionScreenShot_Command.Execute(Application.Current.MainWindow);
            }
           else  if (Check_HotKey(2, e) && e.KeyCode == System.Windows.Forms.Keys.PrintScreen)
            {
                tmp.WindowScreenShot_Command.Execute(Application.Current.MainWindow);
            }
        }
        private bool Check_HotKey(int index, System.Windows.Forms.KeyEventArgs e)
        {
       
            for(int i=0;i<3;i++)
            {
                if( Check[index,i]==true )
                {
                    switch (i)
                    {
                        case 0:
                            if (!e.Control) return false;
                                break;
                        case 1:
                            if (!e.Shift) return false;
                                break;
                        case 2:
                            if (!e.Alt) return false;
                                break;
                    }

                }
            }
            return true;
        }
     
        public void Unsubscribe()
        {
        
            m_GlobalHook.KeyDown -= GlobalHookKeyPress;

            //It is recommened to dispose it
            m_GlobalHook.Dispose();
        }
        private void Checked_Fun(string num)
        {
            int total = Int32.Parse(num);
            Check[total / 10, total % 10] = true;

        }
        private void UnChecked_Fun(string num)
        {
            int total = Int32.Parse(num);
            Check[total / 10, total % 10] = false;
        }
        #endregion
        public MainViewModel()
        {
            startView = new StartViewModel();
            paintView = new PaintViewModel();
            Subscribe();
            canvas_height = "0";
            canvas_width = "0";
            Global_Screen = new ScreenCapture();
            Check = new bool[3, 3];
            // currentview = paintView;
            currentview = startView;
        }
    }
}
