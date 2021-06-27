using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Screen_Capture.Core;
using Xceed.Wpf.Toolkit;

namespace Screen_Capture.MVVM.ViewModel
{
    public class PaintViewModel:BaseViewModel
    {
        #region command function
        public DelegateCommand<ImageViewModel>Close_Command
        {
            get;
        }
        #endregion
        #region function
        public void cp_SelectedColorChanged_1(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            ColorPicker cp = sender as ColorPicker;
            if (cp.SelectedColor.HasValue)
            {
                Color C = cp.SelectedColor.Value;
                byte Red = C.R;
                byte Green = C.G;
                byte Blue = C.B;
                long colorVal = Convert.ToInt64(Blue * (Math.Pow(256, 0)) + Green * (Math.Pow(256, 1)) + Red * (Math.Pow(256, 2)));
            }

        }
        private void NewTabItem(ImageViewModel imageView)
        {
            test.Add(imageView);
        }
        private void CloseTabItem(object sender ,EventArgs e)
        {
        
            test.Remove((ImageViewModel)sender);
        }
        private void CollectionChanged(object sender ,NotifyCollectionChangedEventArgs e)
        {
            ImageViewModel tmp;
            if(e.Action==NotifyCollectionChangedAction.Add)
            {
                tmp= e.NewItems[0] as ImageViewModel;
                tmp.CloseRequest += CloseTabItem;
            }
            else
            {
                tmp = e.OldItems[0] as ImageViewModel;
                tmp.CloseRequest -= CloseTabItem;
            }
        }
        #endregion
        #region   屬性
        public ObservableCollection<ImageViewModel> test { get; set; }
        #endregion
        public PaintViewModel()
        {
            test = new ObservableCollection<ImageViewModel>();
            test.CollectionChanged += CollectionChanged;
            test.Add(new ImageViewModel());
            test.Add(new ImageViewModel());
            test.Add(new ImageViewModel());
         
        }
    }
}
