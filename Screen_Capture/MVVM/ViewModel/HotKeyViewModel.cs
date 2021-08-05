using Screen_Capture.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
//System.Collections.ObjectModel.ObservableCollection
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Diagnostics;
using Screen_Capture.MVVM.Model;
using Screen_Capture.Services;

namespace Screen_Capture.MVVM.ViewModel
{

    public class itemHotkeys:BaseViewModel
    {
        public itemHotkeys(HotKeys keys)
        {
            Name = keys.Name;
            key= KeyInterop.KeyFromVirtualKey(keys.Button);
            str = key.ToString();
            Alt = keys.Alt;
            Shift = keys.Shift;
            Ctrl = keys.Ctrl;
            
        }
        #region 屬性
        public Key key { get; set; }
        private  string _Name;

        public  string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        private  string  _str;

        public string  str
        {
            get { return _str; }
            set { _str = value; OnPropertyChange(); HotKeyViewModel.IsUpdate = true; }
        }
        private int _Ctrl;

        public int Ctrl
        {
            get { return  _Ctrl; }
            set {
                if (value == 0) _Ctrl = (int)HotkeyHelper.KeyModifiers.None;
                else _Ctrl = (int)HotkeyHelper.KeyModifiers.Ctrl;
                OnPropertyChange(); HotKeyViewModel.IsUpdate = true; 
            
            }
        }
        private int _Shift;

        public int Shift
        {
            get { return _Shift; }
            set {
                if (value == 0) _Shift = (int)HotkeyHelper.KeyModifiers.None;
                else _Shift = (int)HotkeyHelper.KeyModifiers.Shift;
                OnPropertyChange(); HotKeyViewModel.IsUpdate = true; }
        }

        private  int _Alt;

        public int Alt
        {
            get { return _Alt; }
            set {
                if (value == 0) _Alt= (int)HotkeyHelper.KeyModifiers.None;
                else _Alt = (int)HotkeyHelper.KeyModifiers.Alt;
                OnPropertyChange(); HotKeyViewModel.IsUpdate = true; 
            }
        }
        #endregion
        #region function
      
        public void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            HotKeyViewModel.IsUpdate = true;
            TextBox textBox = sender as TextBox;
            textBox.Text = "";
            HotKeyViewModel.IsUpdate = true;

            key =e.Key;
              str = e.Key.ToString();

            

        }
        public void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

            e.Handled = true;
        }
        #endregion
    }


    public  class HotKeyViewModel:BaseViewModel
    {
        #region 屬性
        private IDataService<HotKeys> service;

        private HotkeyHelper[] hotkeyHelpers;
        public static bool IsUpdate { get; set;  }
        private bool _IsRead;

        public bool IsRead 
        {
            get { return _IsRead; }
            set { _IsRead = value;  OnPropertyChange(); }
        }

        private Dictionary<int, HotkeyHelper.KeyModifiers> table=new Dictionary<int, HotkeyHelper.KeyModifiers>()
        {
            { (int)HotkeyHelper.KeyModifiers.Ctrl,HotkeyHelper.KeyModifiers.Ctrl},
            { (int)HotkeyHelper.KeyModifiers.Shift,HotkeyHelper.KeyModifiers.Shift},
            { (int)HotkeyHelper.KeyModifiers.Alt,HotkeyHelper.KeyModifiers.Alt},
            { (int)HotkeyHelper.KeyModifiers.None,HotkeyHelper.KeyModifiers.None},
            { (int)HotkeyHelper.KeyModifiers.Win,HotkeyHelper.KeyModifiers.Win},
            { (int)HotkeyHelper.KeyModifiers.NoRepeat,HotkeyHelper.KeyModifiers.NoRepeat}
        }
            
            ;

        public ObservableCollection<itemHotkeys> hotKeys { get; set; }
        #endregion
        #region command Declare
        public DelegateCommand <Window>StoreSetting_Command
        {
            get { return new DelegateCommand<Window>(StoreSetting, Can_StoreSetting); }
        }

        public DelegateCommand<Window> CancelSetting_Command
        {
            get { return new DelegateCommand<Window>(CancelSetting); }
        }
        #endregion
        #region command function
        private bool Can_StoreSetting(Window window)
        {
            if (HotKeyViewModel.IsUpdate) return true;
            else return false;
        }
        private void StoreSetting(Window window)
        {
            
            Setting_HotKey();
            UnRegisterKey();
            RegisterKey();
            IsUpdate = false;
       
            window.Close();
        }
        private void CancelSetting(Window window)
        {

            Inital_HotKey();
            window.Hide();
        }
        #endregion

        #region  function
        private void Setting_HotKey()
        {
            for(int i=0; i<hotKeys.Count;i++)
            {
                int tmp = KeyInterop.VirtualKeyFromKey(hotKeys[i].key);
                service.Update(i+1, new HotKeys() {Alt=hotKeys[i].Alt,Shift=hotKeys[i].Shift,Ctrl=hotKeys[i].Ctrl,Button=tmp, Name=hotKeys[i].Name });
            }
            
        }
        private void Inital_HotKey()
        {
            hotKeys.Clear();
            List<HotKeys> data = (List<HotKeys>)service.GetAll().Result;
            for (int i = 0; i < data.Count; i++)
            {
               
                hotKeys.Add(new itemHotkeys(data[i]));
            }

            
        }
        private void RegisterKey()
        {
            hotkeyHelpers[0] = new HotkeyHelper(table[hotKeys[0].Alt] | table[hotKeys[0].Shift] | table[hotKeys[0].Ctrl], hotKeys[0].key, RegionWindow_Action);
            hotkeyHelpers[1] = new HotkeyHelper(table[hotKeys[1].Alt] | table[hotKeys[1].Shift] | table[hotKeys[1].Ctrl], hotKeys[1].key, FullWindow_Action);
            hotkeyHelpers[2] = new HotkeyHelper(table[hotKeys[2].Alt] | table[hotKeys[2].Shift] | table[hotKeys[2].Ctrl], hotKeys[2].key, Window_Action);
        }
        public void UnRegisterKey()
        {
            hotkeyHelpers[0].Dispose();
            hotkeyHelpers[1].Dispose();
            hotkeyHelpers[2].Dispose();
        }
        private void RegionWindow_Action(HotkeyHelper helper)
        {
            MainViewModel main = (MainViewModel)Application.Current.MainWindow.DataContext;
            StartViewModel start = main.currentview as StartViewModel;
            ScreenShotViewModel screen = start.screenShotView;
            screen.RegionScreenShot_Command.CanExecute(null);
            screen.RegionScreenShot_Command.Execute(Application.Current.MainWindow);

        }

        private void FullWindow_Action(HotkeyHelper helper)
        {
            MainViewModel main = (MainViewModel)Application.Current.MainWindow.DataContext;
            StartViewModel start = main.currentview as StartViewModel;
            ScreenShotViewModel screen = start.screenShotView;
            screen.FullScreenShot_Command.CanExecute(null);
            screen.FullScreenShot_Command.Execute(Application.Current.MainWindow);

        }
        private void Window_Action(HotkeyHelper helper)
        {
            MainViewModel main = (MainViewModel)Application.Current.MainWindow.DataContext;
            StartViewModel start = main.currentview as StartViewModel;
            ScreenShotViewModel screen = start.screenShotView;
            screen.WindowScreenShot_Command.CanExecute(null);
            screen.WindowScreenShot_Command.Execute(Application.Current.MainWindow);

        }

        #endregion
        public HotKeyViewModel()
        {
         
            service = new GenericService<HotKeys>(new HotKeyDBContextFactory() );

            hotkeyHelpers = new HotkeyHelper[3];
            hotKeys = new ObservableCollection<itemHotkeys>();
            Inital_HotKey();
            RegisterKey();




            IsUpdate = false;
            IsRead = false; 
        }
    }
}
