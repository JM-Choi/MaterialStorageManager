using MaterialStorageManager.Utils;
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
    /// UsCtrl_MenuItem.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class UsCtrl_MenuItem : UserControl
    {
        public UsCtrl_MenuItem(ItemMenu itemMenu)
        {
            InitializeComponent();
            DataContext = new ViewModels.UsCtrl_MenuItem_ViewModel(itemMenu);
        }
    }
}
