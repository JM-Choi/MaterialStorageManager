using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MaterialStorageManager.Utils
{
    public class ItemMenu
    {
        public ItemMenu(ePAGE header, PackIconKind icon, Visibility visibility, SubItem[] subitems)
        {
            Header = header;
            Icon = icon;
            Visibility = visibility.ToString();
            SubItems = subitems;
        }

        public ePAGE Header { get; private set; }
        public PackIconKind Icon { get; private set; }
        public string Visibility { get; set; }
        public SubItem[] SubItems { get; private set; }
    }
}
