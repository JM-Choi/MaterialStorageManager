using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MaterialStorageManager.Utils
{
    public class SubItem : INotifyPropertyChanged
    {
        public SubItem(eVIWER name, PackIconKind icon, UserControl screen = null)
        {
            Name = name;
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

