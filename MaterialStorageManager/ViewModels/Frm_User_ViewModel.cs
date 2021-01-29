using MaterialStorageManager.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static MaterialStorageManager.Utils.MsgBox;

namespace MaterialStorageManager.ViewModels
{
    class Frm_User_ViewModel : Notifier
    {
        public ICommand BtnEvent { get; set; }

        public USERINFO userinfo;
        Models.MainSequence mainSequence = Models.MainSequence.Inst;
        public Frm_User_ViewModel()
        {
            userinfo = _Data.Inst.user;
            BtnEvent = new Command(BtnClickEvnet);
        }

        private void BtnClickEvnet(object obj)
        {
            switch(obj)
            {
                case "btn_Go":
                    var user = userinfo.LogIn(userName, passWord);
                    if (string.IsNullOrEmpty(user.msg))
                    {
                        //Evt_UpdateUser?.Invoke(this, user);
                        BtnClickEvnet("btn_Exit");
                    }
                    else
                    {
                        var rtn = MsgBox.ShowDialog(user.msg, MsgType.Warn, eBTNSTYLE.OK);
                    }
                    break;
                case "btn_Exit":
                    break;
            }
        }

        string userName = string.Empty;
        public string UserName
        {
            get
            {
                return userName;
            }
            set
            {
                userName = value;
                OnPropertyChanged();
            }
        }

        string passWord = string.Empty;
        public string PassWord
        {
            get
            {
                return passWord;
            }
            set
            {
                passWord = value;
                OnPropertyChanged();
            }
        }
    }
}
