using MaterialDesignThemes.Wpf;
using MaterialStorageManager.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using static MaterialStorageManager.Utils.MsgBox;

namespace MaterialStorageManager.ViewModels
{
    class Frm_Msg_ViewModel : Notifier
    {
        public ICommand CmdWindowLoaded { get; set; }
        public ICommand CmdButtonClick { get; set; }
        public ICommand CmdBarMouseDown { get; set; }

        MsgBox msgBox = MsgBox.Inst;
        MsgType type;
        public eBTNSTYLE btnStyle { get; set; }

        public Action CloseAction { get; set; }
        public Action DragMoveAction { get; set; }

        DispatcherTimer tmrUpdate, tmrTskProc;
        DateTime createTime;

        public Frm_Msg_ViewModel()
        {
            CmdWindowLoaded = new Command(WindowLoaded);
            CmdButtonClick = new Command(ButtonClick);
            CmdBarMouseDown = new Command(BarMouseDown);
            msgBox.OnEventMsgBoxData += OnEventMsgBoxData;
        }

        private void BarMouseDown(object obj)
        {
            try
            {
                DragMoveAction();
            }
            catch
            {

            }
        }

        private void ButtonClick(object obj)
        {
            msgBox.btnRlt = (eBTNTYPE)Convert.ToInt32(obj);
            Message = string.Empty;
            tmrUpdate.Stop();
            SetTskProc();
            CloseAction();
        }

        private void OnEventMsgBoxData(object sender, MSGBOXDATA e)
        {
            Message = e.message;
            type = e.msgType;
            btnStyle = e.btnStyle;
            buttonSet();
        }

        private void WindowLoaded(object obj)
        {
            string[] split = Message.Split('\r');
            Window_Height = (split.Length * 120) + 80;
        }

        private void buttonSet()
        {
            switch (type)
            {
                case MsgType.Info: Icon_Kind = PackIconKind.Information; break;
                case MsgType.Warn: Icon_Kind = PackIconKind.Warning; break;
                case MsgType.Error: Icon_Kind = PackIconKind.ErrorOutline; break;
            }

            switch (btnStyle)
            {
                case eBTNSTYLE.OK:
                    OK_Visibility = Visibility.Visible;
                    OK_Column = 1;
                    OK_ColumnSpan = 2;
                    OK_Row = 0;
                    break;
                case eBTNSTYLE.OK_CANCEL:
                    OK_Visibility = Visibility.Visible;
                    Cancel_Visibility = Visibility.Visible;

                    OK_Column = 0;
                    OK_ColumnSpan = 2;
                    OK_Row = 0;

                    Cancel_Column = 2;
                    Cancel_ColumnSpan = 2;
                    Cancel_Row = 0;
                    break;
                case eBTNSTYLE.OK_CANCEL_RETRY:
                    OK_Visibility = Visibility.Visible;
                    Cancel_Visibility = Visibility.Visible;
                    Retry_Visibility = Visibility.Visible;

                    OK_Column = 0;
                    OK_ColumnSpan = 2;
                    OK_Row = 0;

                    Cancel_Column = 1;
                    Cancel_ColumnSpan = 2;
                    Cancel_Row = 0;

                    Retry_Column = 2;
                    Retry_ColumnSpan = 2;
                    Retry_Row = 0;
                    break;
                case eBTNSTYLE.OK_CANCEL_RETRY_IGNORE:
                    OK_Visibility = Visibility.Visible;
                    Cancel_Visibility = Visibility.Visible;
                    Retry_Visibility = Visibility.Visible;
                    Ignore_Visibility = Visibility.Visible;

                    OK_Column = 0;
                    OK_Row = 0;

                    Cancel_Column = 1;
                    Cancel_Row = 0;

                    Retry_Column = 2;
                    Retry_Row = 0;

                    Ignore_Column = 3;
                    Ignore_Row = 0;
                    break;
            }
            createTime = DateTime.Now;
            tmrUpdate = new DispatcherTimer();
            tmrUpdate.Interval = TimeSpan.FromMilliseconds(10);    //시간간격 설정
            tmrUpdate.Tick += new EventHandler(Tmr_Tick);           //이벤트 추가    
            tmrUpdate.Start();

            tmrTskProc = new DispatcherTimer();
            tmrUpdate.Interval = TimeSpan.FromMilliseconds(0.1);    //시간간격 설정

            SetTskProc(true);
        }

        bool bTogle = false;
        private void Tmr_Tick(object sender, EventArgs e)
        {
            var time = DateTime.Now - createTime;
            CurrentTime = time.ToString(@"hh\:mm\:ss");
            switch (type)
            {
                case MsgType.Info:
                    MsgBackGround = new SolidColorBrush(Colors.White);
                    MsgForeGround = new SolidColorBrush(Colors.Black);
                    break;
                case MsgType.Warn:
                    MsgBackGround = new SolidColorBrush(Colors.Orange);
                    MsgForeGround = new SolidColorBrush(Colors.WhiteSmoke);
                    break;
                case MsgType.Error:
                    MsgBackGround = bTogle ? new SolidColorBrush(Colors.Red) : new SolidColorBrush(Colors.Yellow);
                    MsgForeGround = bTogle ? new SolidColorBrush(Colors.White) : new SolidColorBrush(Colors.Red);
                    break;
            }

            var state = _Data.Inst.sys.status.eqpState;
            //switch (type)
            //{
            //    case MsgType.Error:
            //        switch (state)
            //        {
            //            case eEQPSATUS.Error: case eEQPSATUS.EMG: break;
            //            default: msgBox.btnRlt = eBTNTYPE.OK; CloseAction(); break;
            //        }
            //        break;
            //    default: break;
            //}
            bTogle ^= true;
        }

        public void SetTskProc(bool bEnb = false)
        {
            if (true == bEnb)
            {
                tmrTskProc.Start();
            }
            else
            {
                tmrTskProc.Stop();
            }
        }

        int window_Height = 200;
        public int Window_Height
        {
            get
            {
                return window_Height;
            }
            set
            {
                window_Height = value;
                OnPropertyChanged();
            }
        }

        string message = "test message";
        public string Message
        {
            get
            {
                return message;
            }
            set
            {
                message = value;
                OnPropertyChanged();
            }
        }

        int ok_column = 0;
        public int OK_Column
        {
            get
            {
                return ok_column;
            }
            set
            {
                ok_column = value;
                OnPropertyChanged();
            }
        }

        int ok_columnspan = 1;
        public int OK_ColumnSpan
        {
            get
            {
                return ok_columnspan;
            }
            set
            {
                ok_columnspan = value;
                OnPropertyChanged();
            }
        }

        int ok_row = 0;
        public int OK_Row
        {
            get
            {
                return ok_row;
            }
            set
            {
                ok_row = value;
                OnPropertyChanged();
            }
        }

        Visibility ok_visibility = Visibility.Hidden;
        public Visibility OK_Visibility
        {
            get
            {
                return ok_visibility;
            }
            set
            {
                ok_visibility = value;
                OnPropertyChanged();
            }
        }

        int cancel_column = 0;
        public int Cancel_Column
        {
            get
            {
                return cancel_column;
            }
            set
            {
                cancel_column = value;
                OnPropertyChanged();
            }
        }

        int cancel_columnspan = 1;
        public int Cancel_ColumnSpan
        {
            get
            {
                return cancel_columnspan;
            }
            set
            {
                cancel_columnspan = value;
                OnPropertyChanged();
            }
        }

        int cancel_row = 0;
        public int Cancel_Row
        {
            get
            {
                return cancel_row;
            }
            set
            {
                cancel_row = value;
                OnPropertyChanged();
            }
        }

        Visibility cancel_visibility = Visibility.Hidden;
        public Visibility Cancel_Visibility
        {
            get
            {
                return cancel_visibility;
            }
            set
            {
                cancel_visibility = value;
                OnPropertyChanged();
            }
        }

        int retry_column = 0;
        public int Retry_Column
        {
            get
            {
                return retry_column;
            }
            set
            {
                retry_column = value;
                OnPropertyChanged();
            }
        }

        int retry_columnspan = 1;
        public int Retry_ColumnSpan
        {
            get
            {
                return retry_columnspan;
            }
            set
            {
                retry_columnspan = value;
                OnPropertyChanged();
            }
        }

        int retry_row = 0;
        public int Retry_Row
        {
            get
            {
                return retry_row;
            }
            set
            {
                retry_row = value;
                OnPropertyChanged();
            }
        }

        Visibility retry_visibility = Visibility.Hidden;
        public Visibility Retry_Visibility
        {
            get
            {
                return retry_visibility;
            }
            set
            {
                retry_visibility = value;
                OnPropertyChanged();
            }
        }

        int ignore_column = 0;
        public int Ignore_Column
        {
            get
            {
                return ignore_column;
            }
            set
            {
                ignore_column = value;
                OnPropertyChanged();
            }
        }

        int ignore_columnspan = 1;
        public int Ignore_ColumnSpan
        {
            get
            {
                return ignore_columnspan;
            }
            set
            {
                ignore_columnspan = value;
                OnPropertyChanged();
            }
        }

        int ignore_row = 0;
        public int Ignore_Row
        {
            get
            {
                return ignore_row;
            }
            set
            {
                ignore_row = value;
                OnPropertyChanged();
            }
        }

        Visibility ignore_visibility = Visibility.Hidden;
        public Visibility Ignore_Visibility
        {
            get
            {
                return ignore_visibility;
            }
            set
            {
                ignore_visibility = value;
                OnPropertyChanged();
            }
        }

        PackIconKind icon_kind;
        public PackIconKind Icon_Kind
        {
            get
            {
                return icon_kind;
            }
            set
            {
                icon_kind = value;
                OnPropertyChanged();
            }
        }

        string currenttime = string.Empty;
        public string CurrentTime
        {
            get
            {
                return currenttime;
            }
            set
            {
                currenttime = value;
                OnPropertyChanged();
            }
        }

        SolidColorBrush msgbackgroud;
        public SolidColorBrush MsgBackGround
        {
            get
            {
                return msgbackgroud;
            }
            set
            {
                msgbackgroud = value;
                OnPropertyChanged();
            }
        }


        SolidColorBrush msgforegroud;
        public SolidColorBrush MsgForeGround
        {
            get
            {
                return msgforegroud;
            }
            set
            {
                msgforegroud = value;
                OnPropertyChanged();
            }
        }
    }
}
