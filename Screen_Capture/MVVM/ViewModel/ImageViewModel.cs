using Screen_Capture.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen_Capture.MVVM.ViewModel
{
    public class ImageViewModel:BaseViewModel
    {
        public event EventHandler CloseRequest;
        public DelegateCommand <EventHandler>CloseCommand { get; }
        public ImageViewModel()
        {
            CloseCommand = new DelegateCommand<EventHandler>(p => CloseRequest?.Invoke(this, EventArgs.Empty));
        }
    }
}
