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

namespace Screen_Capture.MVVM.ViewModel
{
 
    public class MainViewModel:BaseViewModel
    {
        #region  屬性
        private object _currentview;

        public object currentview  //主畫面的選項後出現的view
        {
            get { return _currentview; }
            set { _currentview = value; OnPropertyChange(); }
        }
        private StartViewModel startView { get; set; }
       
        private ImageSource _image;
        public ImageSource Image
        {
            get { return _image; }
            set { _image = value; OnPropertyChange(); }
        }
        #endregion


        public MainViewModel()
        {
            startView = new StartViewModel();
            
            currentview = startView;

        }
    }
}
