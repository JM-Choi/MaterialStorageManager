using MaterialStorageManager.Utils;
using MaterialStorageManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MaterialStorageManager.Views
{
    /// <summary>
    /// UC_LogItem.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class UC_LogItem : UserControl
    {
        public UC_LogItem(CmdLogType logType)
        {
            InitializeComponent();

            LogItem_ViewModel vm = new LogItem_ViewModel(logType);

            this.DataContext = vm;
        }
    }
}
