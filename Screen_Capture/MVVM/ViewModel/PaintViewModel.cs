using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Screen_Capture.Core;
using Xceed.Wpf.Toolkit;

namespace Screen_Capture.MVVM.ViewModel
{
    public class PaintViewModel:BaseViewModel
    {
        #region command function
        public DelegateCommand Copy_Command
        {
            get { return new DelegateCommand(Copy_Object, Can_Copy); }
        }
        public DelegateCommand Pasted_Command
        {
          
            get { return new DelegateCommand(Pasted_Object,Can_Pasted);}
        }
        public DelegateCommand Cut_Command
        {
            get { return new DelegateCommand(Cut_Object, Can_Cut); }
        }
        public DelegateCommand Save_Command
        {
            get { return new DelegateCommand(Save_Object, Can_Save); }
        }
        #endregion
        #region function
        private bool Can_Save()
        {
            if (paints.Count == 0) return false;
            if (paints[SelectedIndex].IsPainted == true) return true;
            else return false;
        }
        private  void Save_Object()
        {
            DateTime now = DateTime.Now;

            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
           
            dlg.FileName = "Image"; // Default file name
            dlg.FileName += now.ToString("yyyy-MM-dd-H-m");
            dlg.DefaultExt = ".png"; // Default file extension
            dlg.Filter = "PNG (.png)|*.png"; // Filter files by extension
            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();
            RenderTargetBitmap bmp = GetRenderTargetBitmap(paints[SelectedIndex].MyCanvas);
          
            // Process save file dialog box results
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
        private bool Can_Cut()
        {
            if (ImageViewModel.Global_Element != null &&  ImageViewModel.Mode == 0) return true;
            else return false;
        }
        private void Cut_Object()
        {
            Clipboard.SetImage(GetRenderTargetBitmap(ImageViewModel.Global_Element));
            paints[SelectedIndex].MyCanvas.Children.Remove(ImageViewModel.Global_Element);
        }
        private bool  Can_Pasted()
        {
            if (Clipboard.ContainsImage())
            {
                return true;
            }
            else return false;
        }
        private void Pasted_Object()
        {
        
                if(paints[SelectedIndex].MyCanvas!=null)
                {
                Rectangle rect = new Rectangle();
                    ImageBrush brush = new ImageBrush();
                    brush.ImageSource= Clipboard.GetImage();
                rect.Width = brush.ImageSource.Width;
                rect.Height = brush.ImageSource.Height;
                    rect.Fill = brush;
                Canvas.SetLeft(rect, 0);
                Canvas.SetTop(rect, 0);
              
                paints[SelectedIndex].MyCanvas.Children.Add(rect);


                }
          
        }
        private bool Can_Copy()
        {
            if (ImageViewModel.Mode == 3 && ImageViewModel.Global_ImageSource != null) return true;
            else  return false;
        }
        private void Copy_Object()
        {
            Clipboard.SetImage((BitmapSource)ImageViewModel.Global_ImageSource.ImageSource);
            
        }
        private RenderTargetBitmap GetRenderTargetBitmap(FrameworkElement element)
        {
            //得到元素的ImageSource
            RenderTargetBitmap bmp = new RenderTargetBitmap((int)element.Width, (int)element.Height, 96, 96, PixelFormats.Pbgra32);

            bmp.Render(element);

            return bmp;
        }
        public void Ltb_Selected(object sender ,RoutedEventArgs e)
        {
            ListBox listBox = sender as ListBox;
    
            Console.WriteLine(listBox.SelectedIndex);
            if(listBox.SelectedIndex!=-1)
                ImageViewModel.Mode = listBox.SelectedIndex;
        
        }

        public void cp_SelectedColorChanged_1(object sender, RoutedPropertyChangedEventArgs<Color?> e) //顏色變換事件
        {
            ColorPicker cp = sender as ColorPicker;
            if (cp.SelectedColor.HasValue)
            {
                Color C = cp.SelectedColor.Value;
                if (cp.Name == "cp1")
                {
                    ImageViewModel.background = new SolidColorBrush(C);
                }
                else if (cp.Name == "cp2")
                {
                    ImageViewModel.borderbackground = new SolidColorBrush(C);
                }
                else if (cp.Name == "cp3")
                {
                    ImageViewModel.forgeground = new SolidColorBrush(C);
                }
   
                Change_Element();
                byte Red = C.R;
                byte Green = C.G;
                byte Blue = C.B;
                long colorVal = Convert.ToInt64(Blue * (Math.Pow(256, 0)) + Green * (Math.Pow(256, 1)) + Red * (Math.Pow(256, 2)));
            }

        }
 
        private void Change_Element()
        {
            FrameworkElement element = ImageViewModel.Global_Element;
            if (element == null) return;
            Rectangle rect = element as Rectangle;
            if(rect!=null)
            {
                rect.Fill = ImageViewModel.background;
                rect.Stroke = ImageViewModel.borderbackground;
    
            }
            else
            {
                Console.WriteLine("test box");
                TextBox box = element as TextBox;
              
                box.Background = ImageViewModel.background;
                box.Foreground = ImageViewModel.borderbackground;
                
            }
        }
      
        public void NewTabItem(double w ,double h,string name)  // 增加新TAB
        {
            paints.Add(new ImageViewModel(w,h,name));
        }
        private void CloseTabItem(object sender ,EventArgs e)// 刪除新TAB
        {

            paints.Remove((ImageViewModel)sender);
        }
        private void CollectionChanged(object sender ,NotifyCollectionChangedEventArgs e)//List 變換處理
        {
            ImageViewModel tmp;
            if(e.Action==NotifyCollectionChangedAction.Add)
            {
                tmp= e.NewItems[0] as ImageViewModel;
                tmp.CloseRequest +=CloseTabItem;
            }
            else
            {
                tmp = e.OldItems[0] as ImageViewModel;
                tmp.CloseRequest -= CloseTabItem;
            }
        }

        public void TabControl_ChangedItem(object sender ,RoutedEventArgs e)
        {
            TabControl tabControl = sender as TabControl;
            if(tabControl.SelectedIndex!=-1)
            {
                SelectedIndex = tabControl.SelectedIndex;
            }
        }
        #endregion
        #region   屬性
        public ObservableCollection<ImageViewModel> paints { get; set; }
      
        public int SelectedIndex { get; set; }
        #endregion
        public PaintViewModel()
        {
            paints = new ObservableCollection<ImageViewModel>();
            paints.CollectionChanged += CollectionChanged;
          //  SelectedIndex = -1;
        


        }
    }
}
