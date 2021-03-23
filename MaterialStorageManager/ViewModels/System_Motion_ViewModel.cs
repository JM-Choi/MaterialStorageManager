using MaterialStorageManager.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialStorageManager.ViewModels
{
    class System_Motion_ViewModel : Notifier
    {
        Models.MainSequence mainSequence = Models.MainSequence.Inst;
        public System_Motion_ViewModel()
        {
            mainSequence.OnEventMotionCounts += OnEventMotionCounts;
        }

        private void OnEventMotionCounts(object sender, int e)
        {
            MotionCount = e;
        }

        int motionCount = 0;
        public int MotionCount
        {
            get
            {
                return motionCount;
            }
            set
            {
                motionCount = value;
                OnPropertyChanged();
            }
        }
    }
}
