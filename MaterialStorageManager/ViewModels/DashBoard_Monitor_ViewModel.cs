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
            mainSequence.OnEvnetMonitorBtnChange += OnEvnetMonitorBtnChange;
        }

        private void OnEvnetMonitorBtnChange(object sender, eEQPSATUS e)
        {
            BTN_Status(e);
        }

        private void BTN_Status(eEQPSATUS status)
        {
            BtnStartEnable = false;
            BtnStopEnable = false;
            BtnResetEnable = false;
            BtnDropJobEnable = false;

            switch (status)
            {
                case eEQPSATUS.Init:
                case eEQPSATUS.Stop:
                    BtnStartEnable = true;
                    BtnResetEnable = true;
                    break;
                case eEQPSATUS.Stopping: break;
                case eEQPSATUS.Idle:
                case eEQPSATUS.Run:
                    BtnStopEnable = true;
                    if (eEQPSATUS.Run == status)
                    {
                        BtnDropJobEnable = true;
                    }
                    break;
                case eEQPSATUS.Error:
                case eEQPSATUS.EMG:
                    BtnResetEnable = true;
                    break;
            }
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

        bool btnStartEnable = true;
        public bool BtnStartEnable
        {
            get
            {
                return btnStartEnable;
            }
            set
            {
                btnStartEnable = value;
                OnPropertyChanged();
            }
        }

        bool btnStopEnable = true;
        public bool BtnStopEnable
        {
            get
            {
                return btnStopEnable;
            }
            set
            {
                btnStopEnable = value;
                OnPropertyChanged();
            }
        }

        bool btnResetEnable = true;
        public bool BtnResetEnable
        {
            get
            {
                return btnResetEnable;
            }
            set
            {
                btnResetEnable = value;
                OnPropertyChanged();
            }
        }

        bool btnDropJobEnable = true;
        public bool BtnDropJobEnable
        {
            get
            {
                return btnDropJobEnable;
            }
            set
            {
                btnDropJobEnable = value;
                OnPropertyChanged();
            }
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
