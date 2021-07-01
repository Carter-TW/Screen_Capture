using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.ComponentModel;

namespace Screen_Capture.Core
{
 
    public class BaseViewModel : INotifyPropertyChanged
    {
        public  event PropertyChangedEventHandler  PropertyChanged;
        public void OnPropertyChange([CallerMemberName] string PropertyName=null)
        {
           
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
       
    }

   
    }
