using MaterialStorageManager.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using static MaterialStorageManager.Utils.MsgBox;

namespace MaterialStorageManager.ViewModels
{
    class Frm_User_ViewModel : Notifier
    {
        public ICommand BtnEvent { get; set; }
        public ICommand ComboEvent { get; set; }
        public IEnumerable<eOPRGRADE> Oprgrade { get; set; }

        public USERINFO userinfo;
        Models.MainSequence mainSequence = Models.MainSequence.Inst;
        MsgBox msgBox = MsgBox.Inst;
        public Action CloseAction { get; set; }

        public Frm_User_ViewModel(bool bCreateAccount)
        {
            userinfo = _Data.Inst.user;
            BtnEvent = new Command(BtnClickEvnet);
            ComboEvent = new Command(ComboChangedEvent);
            if (bCreateAccount)
            {
                LoginGridVisibility = Visibility.Visible;
            }
            else
            {
                RegisterGridVisibility = Visibility.Visible;
                BtnClickEvnet("btn_openregister");
            }
        }

        eOPRGRADE comboSelectedValue;
        private void ComboChangedEvent(object obj)
        {
            comboSelectedValue = (eOPRGRADE)obj;
        }

        private void BtnClickEvnet(object obj)
        {
            switch(obj)
            {
                case "btn_Go":
                    var user = userinfo.LogIn(UserName, PassWord);
                    if (string.IsNullOrEmpty(user.msg))
                    {
                        mainSequence.UpdateUserData(user);
                        BtnClickEvnet("btn_Exit");
                    }
                    else
                    {
                        var rtn = msgBox.ShowDialog(user.msg, MsgType.Warn, eBTNSTYLE.OK);
                    }
                    break;
                case "btn_openregister":
                    RegisterGridVisibility = Visibility.Visible;
                    OpenregisterVisibility = Visibility.Collapsed;
                    ExitForeground = new SolidColorBrush(Colors.WhiteSmoke);
                    MainRectangleFill = new SolidColorBrush(Colors.PowderBlue);
                    Oprgrade = Enum.GetValues(typeof(eOPRGRADE)).Cast<eOPRGRADE>();
                    break;
                case "btn_Edit":
                    var newuser = new USER();
                    newuser.id = NewUserName;
                    newuser.password = NewPassWord;
                    if (!string.IsNullOrEmpty(newuser.id) && !string.IsNullOrEmpty(newuser.password))
                    {
                        newuser.grade = comboSelectedValue;
                        userinfo.Add(newuser);
                        Logger.Inst.Write(CmdLogType.prdt, $"신규 User를 등록하였습니다. [{newuser.grade}/{newuser.id}]");
                        _Data.Inst.UserSave();
                        BtnClickEvnet("btn_Exit");
                    }
                    break;
                case "btn_Exit":
                    CloseAction();
                    break;
                case "btn_Back":
                    RegisterGridVisibility = Visibility.Collapsed;
                    OpenregisterVisibility = Visibility.Visible;
                    ExitForeground = new SolidColorBrush(Color.FromRgb(236, 97, 97));
                    MainRectangleFill = new SolidColorBrush(Color.FromRgb(255, 255, 255));
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

        string newuserName = string.Empty;
        public string NewUserName
        {
            get
            {
                return newuserName;
            }
            set
            {
                newuserName = value;
                OnPropertyChanged();
            }
        }

        string newpassWord = string.Empty;
        public string NewPassWord
        {
            get
            {
                return newpassWord;
            }
            set
            {
                newpassWord = value;
                OnPropertyChanged();
            }
        }

        Visibility openregistervisibility = Visibility.Hidden;
        public Visibility OpenregisterVisibility
        {
            get
            {
                return openregistervisibility;
            }
            set
            {
                openregistervisibility = value;
                OnPropertyChanged();
            }
        }

        Visibility backvisibility = Visibility.Hidden;
        public Visibility BackVisibility
        {
            get
            {
                return backvisibility;
            }
            set
            {
                backvisibility = value;
                OnPropertyChanged();
            }
        }

        Visibility logingridvisibility = Visibility.Hidden;
        public Visibility LoginGridVisibility
        {
            get
            {
                return logingridvisibility;
            }
            set
            {
                logingridvisibility = value;
                OnPropertyChanged();
            }
        }

        Visibility registergridvisibility = Visibility.Hidden;
        public Visibility RegisterGridVisibility
        {
            get
            {
                return registergridvisibility;
            }
            set
            {
                registergridvisibility = value;
                OnPropertyChanged();
            }
        }

        SolidColorBrush exitforeground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF285A8B"));
        public SolidColorBrush ExitForeground
        {
            get
            {
                return exitforeground;
            }
            set
            {
                exitforeground = value;
                OnPropertyChanged();
            }
        }

        SolidColorBrush mainrectanglefill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF4F4F5"));
        public SolidColorBrush MainRectangleFill
        {
            get
            {
                return mainrectanglefill;
            }
            set
            {
                mainrectanglefill = value;
                OnPropertyChanged();
            }
        }
    }
}
