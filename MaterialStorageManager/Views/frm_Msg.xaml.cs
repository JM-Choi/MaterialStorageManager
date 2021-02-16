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
    /// frm_Msg.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class frm_Msg : Window
    {
        public frm_Msg()
        {
            InitializeComponent();
            Frm_Msg_ViewModel vm = new Frm_Msg_ViewModel();
            this.DataContext = vm;
            if (vm.CloseAction == null)
                vm.CloseAction = new Action(() => this.Close());
        }
    }
}
