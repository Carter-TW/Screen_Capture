using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Screen_Capture.Core;
using Screen_Capture.MVVM.View;

namespace Screen_Capture.MVVM.ViewModel
{
  
    public  class StartViewModel:BaseViewModel
    {
        #region Command Declare 
        public DelegateCommand  Option_Command
        {
            get { return new DelegateCommand(Switch_Option); }
        }

        public DelegateCommand New_Command
        {
            get { return new DelegateCommand(Switch_New); }
        }
        public DelegateCommand Start_Command
        {
            get { return new DelegateCommand(Switch_Start); }
        }
        #endregion

        #region command function
       
        private void Switch_Start()
        {
            funcView = screenShotView;
        }
        private void Switch_Option()
        {
            SettingWindow setting= new SettingWindow();
            setting.DataContext = settingmodel;
            setting.ShowDialog();
            
         //   funcView = settingViewModel;
            
        }
        private void Switch_New()
        {
            funcView = newView;
        }
        #endregion
        #region 屬性
        private object  _funcView;

        public object funcView
        {
            get { return  _funcView; }
            set {  _funcView = value; OnPropertyChange(); }
        }
        public ScreenShotViewModel screenShotView { get; set; }
        private SettingViewModel settingmodel;
        private NewViewModel newView;
        #endregion
     
        public StartViewModel()
        {
            screenShotView = new ScreenShotViewModel();
            settingmodel = new SettingViewModel();
            newView = new NewViewModel();
            funcView = screenShotView;
        }
    }
}
