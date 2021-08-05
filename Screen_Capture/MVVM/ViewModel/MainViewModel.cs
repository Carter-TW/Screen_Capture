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
using System.Text.RegularExpressions;
using Microsoft.Win32;
using Screen_Capture.MVVM.Model;
using System.Diagnostics;
using Screen_Capture.Services;

namespace Screen_Capture.MVVM.ViewModel
{
    public enum SwitchType { Start2Paint, Start2Canvas }
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
        
        public object currentview  //主畫面的選項後出現的view
        {
            get { return _currentview; }
            set { _currentview = value; OnPropertyChange(); }
        }
        private StartViewModel startView { get; set; }
       private PaintViewModel paintView { get; set;  }
       private CanvasViewModel CanvasView { get; set; }
        private ImageSource _image;

        public ImageSource image
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
        
        public DelegateCommand<SwitchType> Switch_Command
        {
            get { return new DelegateCommand<SwitchType>(Switch_View); }
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


        public DelegateCommand New_Command
        {
            get { return new DelegateCommand(New_Canvas, Can_New); }
        }
        public DelegateCommand Open_Command
        {
            get { return new DelegateCommand(Open_Object); }
        }

     public DelegateCommand SaveObject_Command
        {
            get { return new DelegateCommand(Save_Object, Can_Save); }
        }
        #endregion

        #region command function
        private bool Can_Save()
        {
            if (paintView.paints.Count == 0) return false;
            else return true;
        }
        private void Save_Object()
        {
            for (int i = 0;  i < paintView.paints.Count  ; i++)
             {
                if (!paintView.paints[i].IsPainted) continue;
                Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();

                dlg.FileName = paintView.paints[i].File_Name; // Default file name
                dlg.DefaultExt = ".png"; // Default file extension
                dlg.Filter = "PNG (.png)|*.png"; // Filter files by extension
                                                 // Show save file dialog box
                Nullable<bool> result = dlg.ShowDialog();
                RenderTargetBitmap bmp = GetRenderTargetBitmap(paintView.paints[i].MyCanvas);
                if (result == true)
                {
                    // Save document
                    PngBitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(bmp));
                    //</ create Encoder >
                    //< save >
                    FileStream fs = new FileStream(dlg.FileName, FileMode.Create);
                    encoder.Save(fs);
                    fs.Close();
                }
            }
    

      
         
      

      
       
        }

        private RenderTargetBitmap GetRenderTargetBitmap(FrameworkElement element)
        {
            //得到元素的ImageSource
            RenderTargetBitmap bmp = new RenderTargetBitmap((int)element.Width, (int)element.Height, 96, 96, PixelFormats.Pbgra32);
            
            bmp.Render(element);

            return bmp;
        }
        private void Open_Object()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg)|*.jpg|All Files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
            
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(openFileDialog.FileName);
                bitmap.EndInit();
                Image tmp = new Image();
                tmp.Source = bitmap;
                ImageBrush brush = new ImageBrush();
                brush.ImageSource = tmp.Source;
                string file = openFileDialog.FileName;
                string[] list = file.Split('\\');
                canvas_width = brush.ImageSource.Width.ToString();
                canvas_height = brush.ImageSource.Height.ToString();
                paintView.paints.Add(new ImageViewModel(double.Parse(canvas_width), double.Parse(canvas_height), list[list.Length-1], brush ));
                currentview = paintView;



            }
        }
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
            paintView.paints.Add(new ImageViewModel(double.Parse(canvas_width),double.Parse( canvas_height), name,null));
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
        private void Switch_View(SwitchType switchType)
        {
            switch (switchType)
            {
                case SwitchType.Start2Paint:
                    if (currentview == paintView) currentview = startView;
                    else if (currentview == startView) currentview = paintView;
                    break;
                case SwitchType.Start2Canvas:
                    if (currentview == CanvasView) currentview = startView;
                    else if (currentview == startView) currentview = CanvasView;
                    break;
                default:
                    break;
            
            }

        
         
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
  

        #endregion
        private  void test(HotkeyHelper helper)
        {
            Console.WriteLine("test");
        }
        public MainViewModel()
        {
            startView = new StartViewModel();
            paintView = new PaintViewModel();
            CanvasView = new CanvasViewModel();
            canvas_height = "0";
            canvas_width = "0";
            Global_Screen = new ScreenCapture();
            
            // currentview = paintView;
            currentview = startView;

            HotkeyHelper helper = new HotkeyHelper(HotkeyHelper.KeyModifiers.Alt | HotkeyHelper.KeyModifiers.Ctrl, Key.A,test);
            




        }
    }
}
