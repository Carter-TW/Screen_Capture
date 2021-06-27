using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Screen_Capture.Core;
namespace Screen_Capture.MVVM.ViewModel
{
   public  class StartViewModel:BaseViewModel
    {
        #region 屬性
        private object  _funcView;

        public object funcView
        {
            get { return  _funcView; }
            set {  _funcView = value; OnPropertyChange(); }
        }
        private ScreenShotViewModel screenShotView { get; set; }
        #endregion
     
        public StartViewModel()
        {
            screenShotView = new ScreenShotViewModel();
            funcView = screenShotView;
        }
    }
}
