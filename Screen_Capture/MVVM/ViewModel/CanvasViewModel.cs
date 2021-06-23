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
namespace Screen_Capture.MVVM.ViewModel
{
    public class CanvasViewModel:BaseViewModel
    {
        private Rectangle rect;
        private Point start;
        private Point end;
        private double x;
        private double y;
        public ScreenCapture screen { get; set; }
        private ImageSource _image;
        public ImageSource Image
        {
            get { return _image; }
            set { _image = value; OnPropertyChange(); }
        }
        private bool isClose;
        public bool IsClose
        {
            get { return isClose; }
            set
            {
                isClose = value;
                OnPropertyChange();
            }
        }
    

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


        private void MouseLeftButtonDown(MouseEventArgs e)
        {
            Canvas tmp = e.Source as Canvas;
            if(tmp!=null)
            {
               Point point= e.GetPosition(tmp);
                start = new Point(point.X,point.Y);
                end = new Point(point.X, point.Y);
                rect = new Rectangle();
                rect.Fill = Brushes.Transparent;
                rect.Stroke = Brushes.Red;
                rect.StrokeThickness = 2;
                rect.StrokeDashOffset = 2;
                DoubleCollection c = new DoubleCollection();
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
            Canvas tmp = e.Source as Canvas;
            
            screen.RegionScreenShot((int)x,(int) y,(int) rect.Width, (int)rect.Height);
            screen.SaveImage();
            rect = null;
            IsClose = true;
            
        }


        public CanvasViewModel()
        {
            screen = new ScreenCapture();
        }
    }
}
