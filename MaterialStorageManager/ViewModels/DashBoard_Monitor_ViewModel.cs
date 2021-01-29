using MaterialStorageManager.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialStorageManager.ViewModels
{
    public class DashBoard_Monitor_ViewModel : Notifier
    {
        Models.MainSequence mainSequence = Models.MainSequence.Inst;
        public DashBoard_Monitor_ViewModel()
        {
            mainSequence.OnEventJobState += OnEventJobState;
            mainSequence.OnEventMMState += OnEventMMState;
            mainSequence.OnEventProvingData += OnEvnetProvingData;
            mainSequence.OnEventRecvingData += OnEvnetRecvingData;
        }

        private void OnEventJobState(object sender, APIRECVDATA e)
        {
            JobID = e.JobID;
        }

        private void OnEventMMState(object sender, MMSTATUSGROUP e)
        {
            MMState = e.MMstate;
        }

        private void OnEvnetProvingData(object sender, PROVIDINGDATA e)
        {
            ProvOperName = e.ProvOperName;
            ProvUuid = e.ProvUuid;
            ProvMTRName = e.ProvMTRName;
            ProvLotID = e.ProvLotID;
            ProvExpiry = e.ProvExpiry;
            ProvQTY = e.ProvQTY;
            ProvType = e.ProvType;
        }

        private void OnEvnetRecvingData(object sender, RECVINGDATA e)
        {
            RecvOperName = e.RecvOperName;
            RecvUuid = e.RecvUuid;
            RecvMTRName = e.RecvMTRName;
            RecvLotID = e.RecvLotID;
            RecvExpiry = e.RecvExpiry;
            RecvQTY = e.RecvQTY;
            RecvType = e.RecvType;
        }

        string jobID = string.Empty;
        public string JobID
        {
            get
            {
                return jobID;
            }
            set
            {
                jobID = value;
                OnPropertyChanged();
            }
        }
        string jobState = string.Empty;
        public string JobState
        {
            get
            {
                return jobState;
            }
            set
            {
                jobState = value;
                OnPropertyChanged();
            }
        }
        string state = string.Empty;
        public string State
        {
            get
            {
                return state;
            }
            set
            {
                state = value;
                OnPropertyChanged();
            }
        }

        string accState = string.Empty;
        public string AccState
        {
            get
            {
                return accState;
            }
            set
            {
                accState = value;
                OnPropertyChanged();
            }
        }

        string mmState = string.Empty;
        public string MMState
        {
            get
            {
                return mmState;
            }
            set
            {
                mmState = value;
                OnPropertyChanged();
            }
        }

        string msmState = string.Empty;
        public string MSMState
        {
            get
            {
                return msmState;
            }
            set
            {
                msmState = value;
                OnPropertyChanged();
            }
        }

        string provOperName = string.Empty;
        public string ProvOperName
        {
            get
            {
                return provOperName;
            }
            set
            {
                provOperName = value;
                OnPropertyChanged(); ;
            }
        }

        string provUuid = string.Empty;
        public string ProvUuid
        {
            get
            {
                return provUuid;
            }
            set
            {
                provUuid = value;
                OnPropertyChanged(); ;
            }
        }

        string provMTRName = string.Empty;
        public string ProvMTRName
        {
            get
            {
                return provMTRName;
            }
            set
            {
                provMTRName = value;
                OnPropertyChanged(); ;
            }
        }

        string provLotID = string.Empty;
        public string ProvLotID
        {
            get
            {
                return provLotID;
            }
            set
            {
                provLotID = value;
                OnPropertyChanged(); ;
            }
        }

        string provExpiry = string.Empty;
        public string ProvExpiry
        {
            get
            {
                return provExpiry;
            }
            set
            {
                provExpiry = value;
                OnPropertyChanged(); ;
            }
        }

        string provQTY = string.Empty;
        public string ProvQTY
        {
            get
            {
                return provQTY;
            }
            set
            {
                provQTY = value;
                OnPropertyChanged(); ;
            }
        }

        string provType = string.Empty;
        public string ProvType
        {
            get
            {
                return provType;
            }
            set
            {
                provType = value;
                OnPropertyChanged(); ;
            }
        }

        string recvOperName = string.Empty;
        public string RecvOperName
        {
            get
            {
                return recvOperName;
            }
            set
            {
                recvOperName = value;
                OnPropertyChanged(); ;
            }
        }

        string recvUuid = string.Empty;
        public string RecvUuid
        {
            get
            {
                return recvUuid;
            }
            set
            {
                recvUuid = value;
                OnPropertyChanged(); ;
            }
        }

        string recvMTRName = string.Empty;
        public string RecvMTRName
        {
            get
            {
                return recvMTRName;
            }
            set
            {
                recvMTRName = value;
                OnPropertyChanged(); ;
            }
        }

        string recvLotID = string.Empty;
        public string RecvLotID
        {
            get
            {
                return recvLotID;
            }
            set
            {
                recvLotID = value;
                OnPropertyChanged(); ;
            }
        }

        string recvExpiry = string.Empty;
        public string RecvExpiry
        {
            get
            {
                return recvExpiry;
            }
            set
            {
                recvExpiry = value;
                OnPropertyChanged(); ;
            }
        }

        string recvQTY = string.Empty;
        public string RecvQTY
        {
            get
            {
                return recvQTY;
            }
            set
            {
                recvQTY = value;
                OnPropertyChanged(); ;
            }
        }

        string recvType = string.Empty;
        public string RecvType
        {
            get
            {
                return recvType;
            }
            set
            {
                recvType = value;
                OnPropertyChanged(); ;
            }
        }
    }
}
