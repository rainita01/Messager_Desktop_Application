using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demo_158.Base
{
    public class SharingDataViewModel : ViewModelBase
    {
        private object _currentView;
        private static SharingDataViewModel? instance;
        private SharingDataViewModel()
        {

        }

        public  EventHandler CurrenViewChanged;
        public EventHandler CurrentViewErrorChanged;    
        public object CurrentView
        {
            get => _currentView;
            set => SetField(ref _currentView, value);
        }
     

        public static SharingDataViewModel Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SharingDataViewModel();
                }
                return instance;
            }
          
        }

     
           
       
    }
}
