using MaterialDesignThemes.Wpf;
using MaterialStorageManager.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialStorageManager.ViewModels
{
    public class UsCtrl_MenuItem_ViewModel : Notifier
    {
        ItemMenu _itemMenu;
        public UsCtrl_MenuItem_ViewModel(ItemMenu itemMenu)
        {
            _Initialize(itemMenu);
            Header = itemMenu.Header;
            SubItems = itemMenu.SubItems;
            Icon = itemMenu.Icon;
        }

        private void _Initialize(ItemMenu itemMenu)
        {
            //ePAGE header, List< SubItem > subitems, PackIconKind icon
            _itemMenu = itemMenu;
        }

        ePAGE header = ePAGE.DashBoard;
        public ePAGE Header
        {
            get
            {
                return header;
            }
            set
            {
                header = value;
                OnPropertyChanged();
            }
        }

        SubItem[] subItems;
        public SubItem[] SubItems
        {
            get
            {
                return subItems;
            }
            set
            {
                subItems = value;
                OnPropertyChanged();
            }
        }

        PackIconKind icon = PackIconKind.Monitor;
        public PackIconKind Icon
        {
            get
            {
                return icon;
            }
            set
            {
                icon = value;
                OnPropertyChanged();
            }
        }
    }
}
