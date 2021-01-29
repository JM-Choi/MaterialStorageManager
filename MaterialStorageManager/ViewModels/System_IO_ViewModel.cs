using MaterialStorageManager.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialStorageManager.ViewModels
{
    public class System_IO_ViewModel : Notifier
    {
        Models.MainSequence mainSequence = Models.MainSequence.Inst;
        public System_IO_ViewModel()
        {
            mainSequence.OnEventViewIOList += OnEventViewIOList;
        }

        private void OnEventViewIOList(object sender, VIEWIOLIST e)
        {
            InputList = e.ioSRC.Where(p=> p.Type == eIOTYPE.INPUT).ToList();
            OutputList = e.ioSRC.Where(p => p.Type == eIOTYPE.OUTPUT).ToList();
        }

        List<IOSRC> inputList = new List<IOSRC>();
        public List<IOSRC> InputList
        {
            get
            {
                return inputList;
            }
            set
            {
                inputList = value;
                OnPropertyChanged();
            }
        }

        List<IOSRC> outputList = new List<IOSRC>();
        public List<IOSRC> OutputList
        {
            get
            {
                return outputList;
            }
            set
            {
                outputList = value;
                OnPropertyChanged();
            }
        }
    }
}
