using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

    public class SubItem : INotifyPropertyChanged
    {
        public SubItem(eVIWER name, PackIconKind icon, UserControl screen = null)
        {
            Name = name;
            Icon = icon;
            Screen = screen;

        }

        public SubItem(string text, CmdLogType logtype, PackIconKind icon, UserControl screen = null)
        {
            Text = text;
            LogType = logtype;
            Icon = icon;
            Screen = screen;

        }

        public SubItem(string text, PackIconKind icon)
        {
            Text = text;
            Icon = icon;
        }

        public string Text { get; set; }
        public eVIWER Name { get; set; }
        public PackIconKind Icon { get; set; }
        public CmdLogType LogType { get; set; }
        UserControl _screen;
        public UserControl Screen
        {
            get { return _screen; }
            set { this.MutateVerbose(ref _screen, value, RaisePropertyChanged()); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private Action<PropertyChangedEventArgs> RaisePropertyChanged()
        {
            return args => PropertyChanged?.Invoke(this, args);
        }
    }
}
