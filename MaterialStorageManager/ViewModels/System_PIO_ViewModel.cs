using MaterialStorageManager.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MaterialStorageManager.ViewModels
{
    class System_PIO_ViewModel : Notifier
    {
        Models.MainSequence mainSequence = Models.MainSequence.Inst;
        public ICommand PIOdata { get; set; }
        public System_PIO_ViewModel()
        {
            mainSequence.OnEventViewPIOList += OnEventViewPIOList;
            PIOdata = new Command(PIOdataMethod);
        }

        private void PIOdataMethod(object obj)
        {
            var uid = (PIOUID)Convert.ToInt32(obj);

            switch (uid)
            {
                case PIOUID.PIO_0:
                    mainSequence.PIOData(uid, NInterfaceTimeout);
                    break;
                case PIOUID.PIO_1:
                    mainSequence.PIOData(uid, NDockSenChkTime);
                    break;
                case PIOUID.PIO_2:
                    mainSequence.PIOData(uid, NFeedTimeOut_Start);
                    break;
                case PIOUID.PIO_3:
                    mainSequence.PIOData(uid, NFeedTimeOut_Work);
                    break;
                case PIOUID.PIO_4:
                    mainSequence.PIOData(uid, NFeedTimeOut_End);
                    break;
            }
        }

        private void OnEventViewPIOList(object sender, PIO e)
        {
            NInterfaceTimeout = e.nInterfaceTimeout;
            NDockSenChkTime = e.nDockSenChkTime;
            NFeedTimeOut_Start = e.nFeedTimeOut_Start;
            NFeedTimeOut_Work = e.nFeedTimeOut_Work;
            NFeedTimeOut_End = e.nFeedTimeOut_End;
        }

        int nInterfaceTimeout = 0; // 설비와 도킹 후 PIO 시작 대기시간 (스메마신호 On 확인)
        public int NInterfaceTimeout
        {
            get
            {
                return nInterfaceTimeout;
            }
            set
            {
                nInterfaceTimeout = value;
                OnPropertyChanged();
            }
        }

        int nDockSenChkTime = 0; // 도킹 후 정위치 센서 확인시간
        public int NDockSenChkTime
        {
            get
            {
                return nDockSenChkTime;
            }
            set
            {
                nDockSenChkTime = value;
                OnPropertyChanged();
            }
        }

        int nFeedTimeOut_Start = 0; // 피딩시작 후 입구센서에 트래이 감지 타임아웃
        public int NFeedTimeOut_Start
        {
            get
            {
                return nFeedTimeOut_Start;
            }
            set
            {
                nFeedTimeOut_Start = value;
                OnPropertyChanged();
            }
        }

        int nFeedTimeOut_Work = 0; // 피딩시작 후 작업완료 타임아웃
        public int NFeedTimeOut_Work
        {
            get
            {
                return nFeedTimeOut_Work;
            }
            set
            {
                nFeedTimeOut_Work = value;
                OnPropertyChanged();
            }
        }

        int nFeedTimeOut_End = 0; // 작업종료 후 PIO 종료 (스메마신호 Off 확인)
        public int NFeedTimeOut_End
        {
            get
            {
                return nFeedTimeOut_End;
            }
            set
            {
                nFeedTimeOut_End = value;
                OnPropertyChanged();
            }
        }
    }
}
