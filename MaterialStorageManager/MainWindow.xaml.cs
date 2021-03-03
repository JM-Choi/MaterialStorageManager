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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MaterialStorageManager
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainWindowViewModel vm = new MainWindowViewModel();
            this.DataContext = vm;
            if (vm.DragMoveAction == null)
                vm.DragMoveAction = new Action(() => DragMove());
            if (vm.CloseMenuAction == null)
            {
                var st = this.Resources["MenuClose"] as Storyboard;
                if (st != null)
                    vm.CloseMenuAction = new Action(() => st.Begin(btn_CloseMenu));
            }
        }
    }
}
