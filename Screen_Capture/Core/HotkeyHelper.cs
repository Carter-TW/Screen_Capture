using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Interop;

namespace Screen_Capture.Core
{
    public class HotkeyHelper : IDisposable
    {
        public HotkeyHelper(KeyModifiers modifiers ,Key  k, Action<HotkeyHelper> a)
        {
            keyModifiers = modifiers;
            key = k;
            action = a;
            Register();
        }
         
        #region 屬性
        private bool _disposed=false ;
        private int ID;
        private  static Dictionary<int, HotkeyHelper> _dicHotKeyToClBacKProc;
        private Action<HotkeyHelper> action;
        public  KeyModifiers keyModifiers { get; private set; }
        public Key key { get; private set; }
        public const int WmHotKey = 0x0312; //判斷是否式屬於hot key 訊息
        #endregion



        #region Win32 function
        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hwnd, int id, uint Modifiers, uint key);
        [DllImport("user32.dll")]
        public static extern   bool UnregisterHotKey(IntPtr hwnd, int id);
        #endregion

        #region function
        private  bool  Register()
        {
            int virtualkeycod = KeyInterop.VirtualKeyFromKey(key);
            ID = virtualkeycod + ((int)keyModifiers * 0x0001);
            bool result = RegisterHotKey(IntPtr.Zero,ID,(uint)keyModifiers ,(uint)virtualkeycod);
            
            if (_dicHotKeyToClBacKProc == null)
            {
                ComponentDispatcher.ThreadFilterMessage += new ThreadMessageEventHandler(ThreadMessageEvnetFilter);
                _dicHotKeyToClBacKProc = new Dictionary<int, HotkeyHelper>();
                _dicHotKeyToClBacKProc.Add(ID, this);
            }
            else _dicHotKeyToClBacKProc.Add(ID, this);
            return result;
        }
        private void Unregister()
        {
            HotkeyHelper helper;
            if(_dicHotKeyToClBacKProc.TryGetValue(ID,out helper))
            {
                UnregisterHotKey(IntPtr.Zero, ID);
                _dicHotKeyToClBacKProc.Remove(ID);
                
            }
            
            

        }

        private static void  ThreadMessageEvnetFilter(ref MSG msg ,ref bool handled)
        {
            if(!handled)
            {
                if (msg.message == WmHotKey)
                {
                    HotkeyHelper helper;
                    if(_dicHotKeyToClBacKProc.TryGetValue((int)msg.wParam,out helper))
                    {
                        if(helper.action!=null)
                        {
                            helper.action.Invoke(helper);
                        }
                    }
                }


            }
        }

        #endregion


        #region Dispose function
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this._disposed)
            {
                // If disposing equals true, dispose all managed
                // and unmanaged resources.
                if (disposing)
                {
                    // Dispose managed resources.
                   
                }
                Unregister();
                // Note disposing has been done.
                _disposed = true;
            }
        }
        ~HotkeyHelper()
        {
            
            Dispose(false);
        }
        #endregion

        [Flags]
        public enum KeyModifiers {
            None = 0x0000,
            Alt = 0x0001,
            Ctrl = 0x0002,
            NoRepeat = 0x4000,
            Shift = 0x0004,
            Win = 0x0008
        }

    }
}
