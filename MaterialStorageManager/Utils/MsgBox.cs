using MaterialDesignThemes.Wpf;
using MaterialStorageManager.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MaterialStorageManager.Utils
{
    public class MsgBox
    {
        #region MsgBox.Inst 싱글톤 객체 생성
        private static volatile MsgBox instance;
        private static object syncObj = new object();
        public static MsgBox Inst
        {
            get
            {
                if (instance == null)
                {
                    lock (syncObj)
                    {
                        if (instance == null)
                            instance = new MsgBox();
                    }
                }
                return instance;
            }
        }
        #endregion

        public event EventHandler<MSGBOXDATA> OnEventMsgBoxData;
        public enum eBTNSTYLE { OK, OK_CANCEL, OK_CANCEL_RETRY, OK_CANCEL_RETRY_IGNORE }
        public enum eBTNTYPE { OK = 1, CANCEL, Retry, Ignore }
        public enum MsgType { Info, Warn, Error, }

        public eBTNTYPE btnRlt { get; set; }

        public eBTNTYPE Show(string msg)
        {
            return Show(msg, MsgType.Info, eBTNSTYLE.OK);
        }

        public eBTNTYPE Show(string msg, MsgType type, eBTNSTYLE btns)
        {
            CloseAllMsgBox();
            frm_Msg msgBox = new frm_Msg();
            MSGBOXDATA msgBoxData = new MSGBOXDATA
            {
                message = msg,
                msgType = type,
                btnStyle = btns
            };

            OnEventMsgBoxData?.Invoke(this, msgBoxData);
            
            msgBox.Show();
            return btnRlt;
        }

        public eBTNTYPE ShowDialog(string msg)
        {
            CloseAllMsgBox();
            return ShowDialog(msg, MsgType.Info, eBTNSTYLE.OK);
        }

        public eBTNTYPE ShowDialog(string msg, MsgType type, eBTNSTYLE btns)
        {
            CloseAllMsgBox();
            frm_Msg msgBox = new frm_Msg();
            MSGBOXDATA msgBoxData = new MSGBOXDATA
            {
                message = msg,
                msgType = type,
                btnStyle = btns
            };

            OnEventMsgBoxData?.Invoke(this, msgBoxData);

            //msgBox.SetTskProc(true);
            msgBox.ShowDialog();
            //msgBox.SetTskProc();
            return btnRlt;
        }

        //public (eBTNTYPE rtn, string rlt) ShowInputBoxDlg(PackIconKind icon, string title)
        //{
        //    frm_InputBox box = new frm_InputBox(icon, title);
        //    box.Topmost = true;
        //    box.ShowDialog();
        //    return (box._BtnRlt, box._Content);
        //}

        public void CloseAllMsgBox()
        {
            foreach (frm_Msg window in Application.Current.Windows.OfType<frm_Msg>())
                window.Close();
            while (Application.Current.Windows.OfType<frm_Msg>().Count() > 0) { }
        }

        public void CloseAllShortNoti()
        {
            foreach (frm_Noti window in Application.Current.Windows.OfType<frm_Noti>())
                //window.FastClose();
            while (Application.Current.Windows.OfType<frm_Noti>().Count() > 0) { }

        }

        public void CloseAllMessage()
        {
            CloseAllShortNoti();
            CloseAllMsgBox();
        }
    }
}
