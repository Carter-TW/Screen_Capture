using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Screen_Capture.Core;
using Screen_Capture.MVVM.View;

namespace Screen_Capture.MVVM.ViewModel
{

    public class CanvasViewModel:BaseViewModel
    {
        enum Flag{ region=0,control}
        #region  屬性宣告
      
        private Rectangle rect;  //canvas 上的矩形
        private Point start;
        private Point end;
        private double x;  //設定矩形的左上
        private double y;//設定矩形的左上
        private double rate; // 螢幕寬度/canvas 的寬度=point 座標轉換
        private StoreControlViewModel controlView { get; set; }
       // public double window_width { get; set; } //視窗的寬度
        private ImageSource _image;  
        public ImageSource Image  //螢幕截圖的圖轉換imagesource
        {
            get { return _image; }
            set { _image = value; OnPropertyChange(); }
        }

        private object  _finishview;

        public object finishview
        {
            get { return _finishview; }
            set { _finishview = value; OnPropertyChange(); }
        }

        private double  _UC_x;

        public double UC_x
        {
            get { return  _UC_x; }
            set {  _UC_x = value; OnPropertyChange(); }
        }

        private double _UC_y;

        public double UC_y
        {
            get { return _UC_y; }
            set { _UC_y = value; OnPropertyChange(); }
        }

        private Visibility  _state;

        public Visibility state
        {
            get { return  _state; }
            set {  _state = value; OnPropertyChange(); }
        }

        #endregion
        #region command declare
        public DelegateCommand<Canvas> Bt_ExitRegionShot
        {
            get { return new DelegateCommand<Canvas>(ExitRegionShot); }
        }
        public DelegateCommand<Canvas> Bt_SaveOnClipBoard
        {
            get { return new DelegateCommand<Canvas>(SaveOnClipBoard); }
        }

        public DelegateCommand<Canvas> Bt_SaveFileDialog
        {
            get { return new DelegateCommand<Canvas>(SaveFileDialog); }
        }
   
        #endregion
        #region  command function

        private void SaveFileDialog(Canvas can)
        {
            Application.Current.MainWindow.Hide();
            can.Children.Remove(rect);
            state = Visibility.Hidden;
            DateTime now = DateTime.Now;
            MainViewModel.Global_Screen.RegionScreenShot((int)(x * rate), (int)(y * rate), (int)(rect.Width * rate), (int)(rect.Height * rate));
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
                MainViewModel tmp = (MainViewModel)Application.Current.MainWindow.DataContext;
                tmp.Switch_Command.CanExecute(null);
                tmp.Switch_Command.Execute(SwitchType.Start2Canvas);
            }
        }


        private void SaveOnClipBoard(Canvas can)
        {
            Application.Current.MainWindow.Hide();
            can.Children.Remove(rect);
            state = Visibility.Hidden;
            MainViewModel.Global_Screen.RegionScreenShot((int)(x * rate), (int)(y * rate), (int)(rect.Width * rate), (int)(rect.Height * rate));
            MainViewModel tmp = (MainViewModel)Application.Current.MainWindow.DataContext;
            tmp.Switch_Command.CanExecute(null);
            tmp.Switch_Command.Execute(SwitchType.Start2Canvas);
            //window.WindowState = WindowState.Minimized;
            System.Windows.Forms.Clipboard.SetImage(MainViewModel.Global_Screen.bitmap);
        }

        private void ExitRegionShot(Canvas can)
        {
            state = Visibility.Hidden;
            Application.Current.MainWindow.Hide();
            MainViewModel tmp = (MainViewModel)Application.Current.MainWindow.DataContext;
            tmp.Switch_Command.CanExecute(null);
            tmp.Switch_Command.Execute(SwitchType.Start2Canvas);
            can.Children.Remove(rect);
    
    
        }


        public  void MouseLeftButtonDown(object sender, MouseEventArgs e)
        {
         

            Canvas tmp = sender as Canvas;
            if(tmp!=null)
            {
            
                state = Visibility.Hidden;

                tmp.Children.Remove(rect);
                rect = null;
               Point point= e.GetPosition(tmp);
                start = new Point(point.X,point.Y); 
                end = new Point(point.X, point.Y);
                rect = new Rectangle();
                rect.Fill = Brushes.Transparent;
          //      rect.MouseLeftButtonDown += MouseLeftButtonDown;
                rect.Stroke = Brushes.Red;
                rect.StrokeThickness = 2;
                rect.StrokeDashOffset = 2;
                DoubleCollection c = new DoubleCollection();  //設定Dash的間距
                c.Add(2);
                rect.StrokeDashArray = c;
                Canvas.SetLeft(rect, start.X);
                Canvas.SetTop(rect, start.Y);
                tmp.Children.Add(rect);
            }
        }
         
      public  void MouseMove(object sender, MouseEventArgs e)
        {
         
            Canvas tmp = e.Source as Canvas;
           
                if (e.LeftButton == MouseButtonState.Released || rect == null) return;
                else
                {
             
                //找出最小的X 跟Y 設定給X Y  width 跟height 直接相減取絕對值
                    end = e.GetPosition(tmp);
                    x = start.X ;   
                     y=start.Y;
                    if (end.X < start.X)
                    {
                        x = end.X;
                        
                    

                    }
                    if (end.Y < start.Y)
                    {
                         y = end.Y;
                       
                      
                    }

                    double width = Math.Abs(end.X - start.X);
                    double height = Math.Abs(end.Y - start.Y);
                    rect.Width = width;
                    rect.Height = height;
                    Canvas.SetLeft(rect, x);

                    Canvas.SetTop(rect, y);
                }


               
       
        }
       
        public void MouseLeftButtonUp(object sender, MouseEventArgs e)
        {
            Canvas tmp = sender as Canvas;
 
            if (rect.Width < 10 || rect.Height<10) return;
            var scree_width = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
            rate = scree_width / tmp.ActualWidth;  // 螢幕寬度/canvas 的寬度=point 座標轉換

                 
            
            //    screen.SaveImage();
            UC_x = x;
            if (y - 45 < 0) UC_y = y + rect.Height+5;
            else UC_y = y - 45;
            state = Visibility.Visible;
       
            finishview = controlView;
          

        }
      
     
        #endregion
        #region  construction
        public CanvasViewModel()
        {
          
       
            controlView = new StoreControlViewModel();
            
            finishview = controlView;
            state = Visibility.Hidden;
            UC_x = 20;
            UC_y = 20;
        }
        #endregion
    }
}
