using MaterialDesignThemes.Wpf;
using MaterialStorageManager.Utils;
using MaterialStorageManager.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace MaterialStorageManager.ViewModels
{
    public class Logs_ViewModel : Notifier
    {
        public ICommand ListBoxSelChanged { get; set; }
        public Logs_ViewModel()
        {
            ListBoxSelChanged = new Command(ListBoxSelChangedMethod);
            LogTypeItems = new[]
            {
                new SubItem("Production", CmdLogType.prdt, PackIconKind.Reproduction, new UC_LogItem(CmdLogType.prdt)),
                new SubItem("Communication", CmdLogType.Comm, PackIconKind.NearFieldCommunication, new UC_LogItem(CmdLogType.Comm)),
                new SubItem("SECSGEM", CmdLogType.Gem, PackIconKind.TableNetwork, new UC_LogItem(CmdLogType.Gem)),
                new SubItem("ERROR", CmdLogType.Err, PackIconKind.Error, new UC_LogItem(CmdLogType.Err)),
                new SubItem("Debug", CmdLogType.Debug, PackIconKind.DebugStepInto, new UC_LogItem(CmdLogType.Debug))
            };
            UserControl = LogTypeItems[0].Screen;
        }

        private void ListBoxSelChangedMethod(object obj)
        {
            if (obj != null && (int)obj >= 0)
            {
                if (UserControl != LogTypeItems[(int)obj].Screen)
                {
                    UserControl = LogTypeItems[(int)obj].Screen;
                }
            }
        }

        SubItem[] logTypeItems;
        public SubItem[] LogTypeItems
        {
            get
            {
                return logTypeItems;
            }
            set
            {
                logTypeItems = value;
                OnPropertyChanged();
            }
        }

        UserControl userControl;
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
    }
}
