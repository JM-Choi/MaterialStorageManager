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

        Models.MainSequence mainSequence = Models.MainSequence.Inst;

        public MainWindowViewModel()
        {
            BtnMenuOpen = new Command(OpenMethod, CanExecuteMethod);
            BtnMenuClose = new Command(CloseMethod, CanExecuteMethod);
            BtnMainChange = new Command(MainChangeMethod, CanExecuteMethod);
            BtnPopUp = new Command(PopUpMethod);

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
                        new SubItem( eVIWER.TowerLamp, PackIconKind.Lamps, new UC_System_TwrLmp()),
                        new SubItem( eVIWER.Goal, PackIconKind.Target, new UC_System_Goal()),
                        new SubItem( eVIWER.PIO, PackIconKind.NetworkInterfaceCard, new UC_System_PIO()),
                        new SubItem( eVIWER.Option, PackIconKind.Settings, new UC_System_Option())
                    })
            };

        }

        private void PopUpMethod(object obj)
        {
            eBTN_POPUP btn = (eBTN_POPUP)Convert.ToInt32(obj);
            frm_User user;
            switch (btn)
            {
                case eBTN_POPUP.LogIn:
                    user = new frm_User();
                    break;
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
