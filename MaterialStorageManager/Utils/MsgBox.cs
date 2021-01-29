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
        public enum eBTNSTYLE { OK, OK_CANCEL, OK_CANCEL_RETRY, OK_CANCEL_RETRY_IGNORE }
        public enum eBTNTYPE { OK = 1, CANCEL, Retry, Ignore }
        public enum MsgType { Info, Warn, Error, }
        public static eBTNTYPE Show(string msg)
        {
            return Show(msg, MsgType.Info, eBTNSTYLE.OK);
        }

        public static eBTNTYPE Show(string msg, MsgType type, eBTNSTYLE btns)
        {
            CloseAllMsgBox();
            frm_Msg msgBox = new frm_Msg(msg, type, btns);
            msgBox.SetTskProc();
            msgBox.Show();
            return msgBox._btnRlt;
        }

        public static eBTNTYPE ShowDialog(string msg)
        {
            CloseAllMsgBox();
            return ShowDialog(msg, MsgType.Info, eBTNSTYLE.OK);
        }

        public static eBTNTYPE ShowDialog(string msg, MsgType type, eBTNSTYLE btns)
        {
            CloseAllMsgBox();
            frm_Msg msgBox = new frm_Msg(msg, type, btns);
            msgBox.SetTskProc(true);
            msgBox.ShowDialog();
            msgBox.SetTskProc();
            return msgBox._btnRlt;
        }

        public static (eBTNTYPE rtn, string rlt) ShowInputBoxDlg(PackIconKind icon, string title)
        {
            frm_InputBox box = new frm_InputBox(icon, title);
            box.Topmost = true;
            box.ShowDialog();
            return (box._BtnRlt, box._Content);
        }

        public static void CloseAllMsgBox()
        {
            foreach (frm_Msg window in Application.Current.Windows.OfType<frm_Msg>())
                window.Close();
            while (Application.Current.Windows.OfType<frm_Msg>().Count() > 0) { }
        }

        public static void CloseAllShortNoti()
        {
            foreach (frm_Noti window in Application.Current.Windows.OfType<frm_Noti>())
                window.FastClose();
            while (Application.Current.Windows.OfType<frm_Noti>().Count() > 0) { }

        }

        public static void CloseAllMessage()
        {
            CloseAllShortNoti();
            CloseAllMsgBox();
        }
    }
}
