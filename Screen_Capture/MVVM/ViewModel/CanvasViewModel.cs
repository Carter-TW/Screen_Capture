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
        #region  屬性宣告
     
        private Rectangle rect;  //canvas 上的矩形
        private Point start;
        private Point end;
        private double x;  //設定矩形的左上
        private double y;//設定矩形的左上
        private ImageSource _image;   //MainViewModel給的ImageSource

        public Window window { get; set; }
        public ScreenCapture screen { get; set; }  //螢幕截圖的class
        public ImageSource Image
        {
            get { return _image; }
            set { _image = value; OnPropertyChange(); }
        }

        #endregion
        #region command declare
        public DelegateCommand<MouseEventArgs>  Cvs_MouseLeftButtonDown
        {
            get { return new DelegateCommand<MouseEventArgs>(MouseLeftButtonDown); }
        }

        public DelegateCommand<MouseEventArgs> Cvs_MouseLeftButtonUp
        {
            get { return new DelegateCommand<MouseEventArgs>(MouseLeftButtonUp); }
        }
        public DelegateCommand<MouseEventArgs> Cvs_MouseMove
        {
            get { return new DelegateCommand<MouseEventArgs>(MouseMove); }
        }
        #endregion
        #region  command function
        private void MouseLeftButtonDown(MouseEventArgs e)
        {
            Canvas tmp = e.Source as Canvas;
            if(tmp!=null)
            {
              
               Point point= e.GetPosition(tmp);
                start = new Point(point.X,point.Y);
                end = new Point(point.X, point.Y);
             
                rect.Fill = Brushes.Transparent;
                rect.Stroke = Brushes.Red;
                rect.StrokeThickness = 2;
                rect.StrokeDashOffset = 2;
                DoubleCollection c = new DoubleCollection();  //設定Dash的間距
                c.Add(5);
                rect.StrokeDashArray = c;
                Canvas.SetLeft(rect, start.X);
                Canvas.SetTop(rect, start.Y);
                tmp.Children.Add(rect);
            }
        }
        
       private  void MouseMove(MouseEventArgs e)
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
       
        private void MouseLeftButtonUp(MouseEventArgs e)
        {
         
      
               
            
            
            var scree_width = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
            var rate = scree_width / window.ActualWidth; // 螢幕寬度/canvas 的寬度=point 座標轉換
            
            screen.RegionScreenShot((int)(x*rate),(int)(y*rate),(int) (rect.Width*rate), (int)(rect.Height*rate ));
            screen.SaveImage();
           rect = null;
            window.Close();
         
            
        }
        #endregion
        #region  construction
        public CanvasViewModel()
        {
            screen = new ScreenCapture();
            rect = new Rectangle();
        }
        #endregion
    }
}
