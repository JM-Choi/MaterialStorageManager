using MaterialDesignThemes.Wpf;
using MaterialStorageManager.Utils;
using MaterialStorageManager.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MaterialStorageManager.ViewModels
{
    class MainWindowViewModel : Notifier
    {
        public ICommand BtnMenuOpen { get; set; }
        public ICommand BtnMenuClose { get; set; }
        public ICommand BtnMainChange { get; set; }
        public ICommand BtnPopUp { get; set; }
        public ICommand BarMouseDown { get; set; }

        Models.MainSequence mainSequence = Models.MainSequence.Inst;
        MsgBox msgBox = MsgBox.Inst;
        private Logger _log = Logger.Inst;

        public Action DragMoveAction { get; set; }
        public Action CloseMenuAction { get; set; }

        public MainWindowViewModel()
        {
            BtnMenuOpen = new Command(OpenMethod, CanExecuteMethod);
            BtnMenuClose = new Command(CloseMethod, CanExecuteMethod);
            BtnMainChange = new Command(MainChangeMethod, CanExecuteMethod);
            BtnPopUp = new Command(PopUpMethod);
            BarMouseDown = new Command(MouseDownMethod);

            MenuItem = new[]
            {
                new ItemMenu(ePAGE.DashBoard, PackIconKind.ViewDashboard, Visibility.Collapsed, subItem = new[]
                    {
                        new SubItem( eVIWER.Monitor, PackIconKind.Monitor, new UC_DashBoard_Monitor()),
                        new SubItem( eVIWER.Manual, PackIconKind.CarManualTransmission, new UC_DashBoard_Manual())
                    }),
                new ItemMenu(ePAGE.System, PackIconKind.Pencil, Visibility.Collapsed, subItem = new[]
                    {
                        new SubItem( eVIWER.IO, PackIconKind.Toolbox, new UC_System_IO()),
                        new SubItem( eVIWER.TowerLamp, PackIconKind.Lamps, new UC_System_TowerLamp()),
                        new SubItem( eVIWER.Goal, PackIconKind.Target, new UC_System_Goal()),
                        new SubItem( eVIWER.PIO, PackIconKind.NetworkInterfaceCard, new UC_System_PIO()),
                        new SubItem( eVIWER.Option, PackIconKind.Settings, new UC_System_Option())
                    })
            };

            mainSequence.DisignLoadComp();
        }

        private void MouseDownMethod(object obj)
        {
            DragMoveAction();
        }

        frm_User user;
        private void PopUpMethod(object obj)
        {
            eBTN_POPUP btn = (eBTN_POPUP)Convert.ToInt32(obj);
            switch (btn)
            {
                case eBTN_POPUP.LogIn:
                case eBTN_POPUP.Account:
                    user = new frm_User(btn == eBTN_POPUP.LogIn);
                    if (btn == eBTN_POPUP.LogIn)
                    {
                        mainSequence.OnEventUpdateUser += On_UpdateUser;
                    }
                    user.Topmost = true;
                    user.ShowDialog();
                    if (btn == eBTN_POPUP.LogIn)
                    {
                        mainSequence.OnEventUpdateUser -= On_UpdateUser;
                    }
                    break;
                case eBTN_POPUP.LogOut:
                    switch (mainSequence.CurrentView)
                    {
                        case eVIWER.Monitor: case eVIWER.Manual: break;
                        default:
                            if (false == mainSequence._sysStatus.bDebug)
                            {
                                switch (mainSequence._sysStatus._UserGrade)
                                {
                                    case eOPRGRADE.Operator: break;
                                    default: MainChangeMethod(MenuItem[0].SubItems[0].Screen); break;
                                }
                            }
                            else
                            {
                                MainChangeMethod(MenuItem[0].SubItems[0].Screen);
                            }
                            break;
                    }
                    CloseMethod(null);
                    CloseMenuAction();
                    SetLoginUser(new USER(), true);
                    break;
                case eBTN_POPUP.ConfigSave:
                    switch (mainSequence.CurrentView)
                    {
                        case eVIWER.None: // 모델데이터를 저장 시 추가 코딩필요.                            
                            _Data.Inst.MdlSave(mainSequence._sysStatus.currMdlFile);
                            _log.Write(CmdLogType.prdt, $"Model Data를 저장합니다. [{mainSequence._sysStatus.currMdlFile}]");
                            break;
                        default:
                            _Data.Inst.SysSave();
                            _log.Write(CmdLogType.prdt, $"Config Data를 저장합니다. [{_Data.Inst.sys.status.user.grade}/{_Data.Inst.sys.status.user.id}]");
                            break;
                    }
                    break;
                case eBTN_POPUP.Shutdown:
                    Helper.ShowBlurEffectAllWindow();
                    var rtn = msgBox.ShowDialog($"Application을 종료하시겠습니까?", MsgBox.MsgType.Info, MsgBox.eBTNSTYLE.OK_CANCEL);
                    switch (rtn)
                    {
                        case MsgBox.eBTNTYPE.OK:
                            _Finalize();
                            break;
                        default: Helper.StopBlurEffectAllWindow(); break;
                    }
                    break;
            }

        }

        private void _Finalize()
        {
            _log.Write(CmdLogType.prdt, $"Application을 종료합니다. [{mainSequence.Version}]");
            Application.Current.Shutdown();
        }


        private void On_UpdateUser(object sender, USER user)
        {
            SetLoginUser(user);
        }

        public void SetLoginUser(USER user, bool bIsLogOut = false)
        {
            if (false == bIsLogOut)
            {
                mainSequence._sysStatus.User_Set(user);
                LoginGrade = user.grade.ToString();
                LoginID = user.id;
                Logger.Inst.Write(CmdLogType.prdt, $"작업자가 로그인 하였습니다. [{user.grade}/{user.id}]");
            }
            else
            {
                Logger.Inst.Write(CmdLogType.prdt, $"작업자가 로그아웃 하였습니다. [{mainSequence._sysStatus._UserGrade}/{mainSequence._sysStatus.user.id}]");
                mainSequence._sysStatus.User_Set(null, true);
                LoginGrade = LoginID = $"------";
            }
        }

        public SubItem[] subItem { get;}
        ItemMenu[] menuitem;
        public ItemMenu[] MenuItem
        {
            get
            {
                return menuitem;
            }
            set
            {
                menuitem = value;
                OnPropertyChanged();
            }
        }

        UserControl userControl = new UC_DashBoard_Monitor();
        public UserControl UserControl
        {
            get
            {
                return userControl;
            }
            set
            {
                userControl = value;
                OnPropertyChanged();
            }

        }

        UserControl logUserControl = new UC_Logs();
        public UserControl LogUserControl
        {
            get
            {
                return logUserControl;
            }
            set
            {
                logUserControl = value;
                OnPropertyChanged();
            }
        }

        private void OpenMethod(object obj)
        {
            OpenMenuVisibillity = Visibility.Collapsed;
            CloseMenuVisibillity = Visibility.Visible;
            ExpanderMenuVisibillity = Visibility.Visible;
        }

        private void CloseMethod(object obj)
        {
            OpenMenuVisibillity = Visibility.Visible;
            CloseMenuVisibillity = Visibility.Collapsed;
            ExpanderMenuVisibillity = Visibility.Collapsed;
        }

        private void MainChangeMethod(object obj)
        {
            if (UserControl != (UserControl)obj)
            {
                UserControl = (UserControl)obj;
                mainSequence.ViewDataUpdate((UserControl)obj);
            }
        }

        private bool CanExecuteMethod(object arg)
        {
            return true;
        }

        string logingrade = $"------";
        public string LoginGrade
        {
            get
            {
                return logingrade;
            }
            set
            {
                logingrade = value;
                OnPropertyChanged();
            }
        }

        string loginid = $"------";
        public string LoginID
        {
            get
            {
                return loginid;
            }
            set
            {
                loginid = value;
                OnPropertyChanged();
            }
        }

        Visibility openMenuVisibillity = Visibility.Visible;
        public Visibility OpenMenuVisibillity
        {
            get
            {
                return openMenuVisibillity;
            }
            set
            {
                openMenuVisibillity = value;
                OnPropertyChanged();
            }
        }

        Visibility closeMenuVisibillity = Visibility.Collapsed;
        public Visibility CloseMenuVisibillity
        {
            get
            {
                return closeMenuVisibillity;
            }
            set
            {
                closeMenuVisibillity = value;
                OnPropertyChanged();
            }
        }

        Visibility expanderMenuVisibillity = Visibility.Collapsed;
        public Visibility ExpanderMenuVisibillity
        {
            get
            {
                return expanderMenuVisibillity;
            }
            set
            {
                expanderMenuVisibillity = value;
                OnPropertyChanged();
            }
        }
    }
}
