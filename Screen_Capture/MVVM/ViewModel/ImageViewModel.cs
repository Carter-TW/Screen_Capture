using Screen_Capture.Core;
using Screen_Capture.MVVM.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Screen_Capture.MVVM.ViewModel
{
    public class ImageViewModel : BaseViewModel
    {
        #region 屬性
        private string _File_Name;

        public string File_Name
        {
            get { return _File_Name; ; }
            set { _File_Name= value;  OnPropertyChange(); }
        }

        private double _Canvas_Width;

        public double  Canvas_Width
        {
            get { return _Canvas_Width; }
            set { _Canvas_Width = value; OnPropertyChange(); }
        }
        private double _Canvas_Height;

        public double Canvas_Height
        {
            get { return _Canvas_Height; }
            set { _Canvas_Height = value; OnPropertyChange(); }
        }
        private Brush _Canvas_Brush;
        public Brush Canvas_Brush
        {
                get { return _Canvas_Brush; }
            set { _Canvas_Brush = value; OnPropertyChange(); }
        }

        public event EventHandler CloseRequest;
        public static int Mode { get; set; } //paint function
        public static Brush background { get; set; }
        public static Brush borderbackground { get; set; }
        public static Brush forgeground { get; set; }
        public static ImageBrush Global_ImageSource { get; set; }
        public static FrameworkElement Global_Element { get; set; }
        public DelegateCommand<EventHandler> CloseCommand { get; }
        public Canvas MyCanvas { get; set; }
        public bool IsPainted { get; set; }
        private Point origin;
        private Point start;
        private Point end;
        private FrameworkElement control;
        private Rectangle R_rect;
        #endregion

        #region function
        public void Cvs_MouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            IsPainted = true;
            Canvas canvas = sender as Canvas;
            Point point = e.GetPosition(canvas);
            if (Mode == 0)
            {
                MLD_Change_Fun(canvas, point, control);
            }
            if (Mode == 1)
            {
                control = GetHitElement(canvas, point);
                if (control != null)
                {
                    TextBox box = sender as TextBox;
                    if (box != null) Console.WriteLine("box");
                    else Console.WriteLine("box null");
                    control.CaptureMouse();
                    point = e.GetPosition(control);
                    MLD_Move_Fun(canvas, point);
                }

            }
            if (Mode == 2)
            {
                control = GetElement();

                MLD_Rect_Fun(canvas, point);
            }

            if (Mode == 3)
            {
                MLD_Region_Fun(canvas, point, control);
            }


        }

        public void Cvs_MouseMove(object sender, MouseEventArgs e)
        {
            Canvas canvas = sender as Canvas;
            Point point = e.GetPosition(canvas);
            if (Mode == 1)
            {
                if (e.LeftButton == MouseButtonState.Pressed && control != null)
                {
                    MV_Move_Fun(canvas, point);
                }
            }
            if (Mode == 2)
            {
                if (e.LeftButton == MouseButtonState.Released || control == null) return;
                MV_Rect_Fun(canvas, point);
            }

            if (Mode == 3)
            {
                if (e.LeftButton == MouseButtonState.Released) return;
                MV_Region_Fun(canvas, point, control);
            }


        }
        public void Cvs_MouseLeftButtonUp(object sender, MouseEventArgs e)
        {
            Canvas canvas = sender as Canvas;
            Point point = e.GetPosition(canvas);

            if (Mode == 1 && control != null)
            {
                MLU_Move_Fun(canvas, point);
            }
            if (Mode == 2) MLU_Rect_Fun(canvas, point);
            if (Mode == 3)
            {
                MLU_Region_Fun(canvas, point, control);



            }
        }
        private void MLD_Region_Fun(Canvas canvas, Point point, FrameworkElement element)
        {
            ImageBrush imageBrush = new ImageBrush();
            imageBrush.ImageSource = GetRenderTargetBitmap(canvas);
            canvas.Background = imageBrush.Clone();
            canvas.Children.Clear();
            origin = start = end = point;
            R_rect = new Rectangle() { Fill = Brushes.White, Stroke = Brushes.Black };
            DoubleCollection c = new DoubleCollection();  //設定Dash的間距
            c.Add(2);
            R_rect.StrokeDashArray = c;
           
            Canvas.SetLeft(R_rect, start.X);
            Canvas.SetLeft(R_rect, start.Y);
            canvas.Children.Add(R_rect);
        }
        private void MV_Region_Fun(Canvas canvas, Point point, FrameworkElement element)
        {
            end = point;
            if (end.X < start.X) start.X = end.X;
            if (end.Y < start.Y) start.Y = end.Y;
            Size point1 = GetSize(end, origin);
            R_rect.Width = point1.Width;
            R_rect.Height = point1.Height;
            Canvas.SetLeft(R_rect, start.X);
            Canvas.SetTop(R_rect, start.Y);
        }
        private void MLU_Region_Fun(Canvas canvas, Point point, FrameworkElement element)
        {
            ImageBrush originBrush = new ImageBrush();

            originBrush.ImageSource = GetRenderTargetBitmap(canvas); //畫完Region的背景圖
            canvas.Children.Remove(R_rect); //先移除Region 重繪背景之後再加入
            Size tmp = GetSize(end, origin);
            ImageBrush imageBrush = new ImageBrush();

            imageBrush.ImageSource = GetRenderTargetBitmap(canvas);


            //切割canvas 給 Rect的背景 
            BitmapSource bitmap = CutImage((BitmapSource)imageBrush.ImageSource, new Int32Rect((int)start.X, (int)start.Y, (int)tmp.Width, (int)tmp.Height));
            imageBrush.ImageSource = (ImageSource)bitmap;
            Global_ImageSource = imageBrush.Clone();
            R_rect.Fill = imageBrush;



            Canvas.SetLeft(R_rect, start.X);
            Canvas.SetTop(R_rect, start.Y);
            canvas.Children.Add(R_rect); //把重繪的Refgion 重新加入

            canvas.Background = originBrush; //把切割完的canvas畫上去

        }
        #region 移動物件
        private void MLD_Move_Fun(Canvas canvas, Point point)
        {

            start = point;



        }
        private void MV_Move_Fun(Canvas canvas, Point point)
        {

            Canvas.SetLeft(control, point.X - start.X);
            Canvas.SetTop(control, point.Y - start.Y);
        }
        private void MLU_Move_Fun(Canvas canvas, Point point)
        {
            control.ReleaseMouseCapture();
        }

        #endregion
        private void MLD_Change_Fun(Canvas canvas, Point point, FrameworkElement element)
        {

            control = GetHitElement(canvas, point);
            Global_Element = control;



        }

        #region 劃出物件
        private void MLD_Rect_Fun(Canvas canvas, Point point)
        {
            origin = end = start = point;
            Size tmp = GetSize(start, end);


            //    DragElem dragElem = new DragElem(canvas, element);
            control.Width = tmp.Width;
            control.Height = tmp.Height;
            Canvas.SetLeft(control, point.X);
            Canvas.SetTop(control, point.Y);
            canvas.Children.Add(control);

        }
        private void MV_Rect_Fun(Canvas canvas, Point point)
        {
            end = point;
            if (end.X < start.X) start.X = end.X;
            if (end.Y < start.Y) start.Y = end.Y;
            Size tmp = GetSize(origin, end);
            control.Width = tmp.Width;
            control.Height = tmp.Height;
            Canvas.SetLeft(control, start.X);
            Canvas.SetTop(control, start.Y);
        }

        private void MLU_Rect_Fun(Canvas canvas, Point point)
        {
            control = null;
        }
        #endregion

        private Size GetSize(Point first, Point second)
        {
          
                
            double width = Math.Abs(first.X - second.X);
            double height = Math.Abs(first.Y - second.Y);
            return new Size(width, height);
        }
        private RenderTargetBitmap GetRenderTargetBitmap(FrameworkElement element)
        {
            //得到元素的ImageSource
            RenderTargetBitmap bmp = new RenderTargetBitmap((int)element.Width, (int)element.Height, 96, 96, PixelFormats.Pbgra32);

            bmp.Render(element);

            return bmp;
        }
        private BitmapSource CutImage(BitmapSource bitmapSource, Int32Rect cut)
        {
            //切割image 
            var stride = bitmapSource.Format.BitsPerPixel * cut.Width / 8;

            byte[] data = new byte[cut.Height * stride];

            bitmapSource.CopyPixels(cut, data, stride, 0);

            return BitmapSource.Create(cut.Width, cut.Height, 0, 0, PixelFormats.Bgr32, null, data, stride);
        }
        private FrameworkElement GetHitElement(Canvas canvas, Point point)
        {
            FrameworkElement element = canvas.InputHitTest(point) as FrameworkElement;
            if (element == null)
            {
            
                return null;
            }

            else
            {
                Canvas test = element as Canvas;
                if (test == null)
                {
                    
                    return element;
                }
                else
                {

                 
                    return null;
                }
            }
        }
        private FrameworkElement GetElement()
        {
            if (Mode == 2)
            {
                Rectangle rect = new Rectangle() { Fill = background, Stroke = borderbackground, StrokeThickness = 1, MaxHeight = 400, MaxWidth = 400 };
                rect.ClipToBounds = true;
                return rect;
            }

            else return null;
        }
   

 
        #endregion
        public ImageViewModel(double w,double h ,string name,Brush b)
        {
            CloseCommand = new DelegateCommand<EventHandler>(p => CloseRequest?.Invoke(this, EventArgs.Empty));
            forgeground = Brushes.White;
            background = Brushes.Black;
            borderbackground = Brushes.Black;
            Mode = 0;
            control = null;
            IsPainted = false;
            Canvas_Height = h;
            Canvas_Width = w;
            File_Name = name;
        
            if (b == null) Canvas_Brush = Brushes.White;
            else Canvas_Brush = b;
            
            
     
        }
    }
}
