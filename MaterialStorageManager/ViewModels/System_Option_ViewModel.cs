using MaterialStorageManager.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MaterialStorageManager.ViewModels
{
    class System_Option_ViewModel : Notifier
    {
        Models.MainSequence mainSequence = Models.MainSequence.Inst;
        public IEnumerable<eLANGUAGE> Languages { get; set; }
        public ICommand OptionData { get; set; }

        public System_Option_ViewModel()
        {
            mainSequence.OnEventViewOptionList += OnEventViewOptionList;
            Languages = Enum.GetValues(typeof(eLANGUAGE)).Cast<eLANGUAGE>();
            OptionData = new Command(OptionDataMethod);
        }

        private void OptionDataMethod(object obj)
        {
            var uid = (OPTIONUID)Convert.ToInt32(obj);
            switch (uid)
            {
                case OPTIONUID.EQPNAME:
                    mainSequence.OptionData(uid, EqpName);
                    break;
                case OPTIONUID.LANGUAGE:
                    mainSequence.OptionData(uid, Language);
                    break;
                case OPTIONUID.MPLUSIP:
                    mainSequence.OptionData(uid, MplusIP);
                    break;
                case OPTIONUID.MPLUSPORT:
                    mainSequence.OptionData(uid, MplusPort);
                    break;
                case OPTIONUID.VECIP:
                    mainSequence.OptionData(uid, VecIP);
                    break;
            }
        }

        private void OnEventViewOptionList(object sender, CONFIG e)
        {
            EqpName = e.eqpName;
            Language = e.language;
            MplusIP = e.mplusIP;
            MplusPort = e.mplusPort;
            VecIP = e.VecIP;
        }

        string eqpName = string.Empty;
        public string EqpName
        {
            get
            {
                return eqpName;
            }
            set
            {
                eqpName = value;
                OnPropertyChanged();
            }
        }

        eLANGUAGE language = eLANGUAGE.kor;
        public eLANGUAGE Language
        {
            get
            {
                return language;
            }
            set
            {
                language = value;
                OnPropertyChanged();
            }
        }

        string mplusIP = string.Empty;
        public string MplusIP
        {
            get
            {
                return mplusIP;
            }
            set
            {
                mplusIP = value;
                OnPropertyChanged();
            }
        }

        int mplusPort = 0;
        public int MplusPort
        {
            get
            {
                return mplusPort;
            }
            set
            {
                mplusPort = value;
                OnPropertyChanged();
            }
        }

        string vecIP = string.Empty;
        public string VecIP
        {
            get
            {
                return vecIP;
            }
            set
            {
                vecIP = value;
                OnPropertyChanged();
            }
        }
    }
}
