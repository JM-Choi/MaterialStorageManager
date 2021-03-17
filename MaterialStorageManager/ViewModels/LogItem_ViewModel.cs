using MaterialStorageManager.Utils;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace MaterialStorageManager.ViewModels
{
    public class LogItem_ViewModel : Notifier
    {
        Models.MainSequence mainSequence = Models.MainSequence.Inst;
        private CmdLogType logItemType;
        public LogItem_ViewModel(CmdLogType logType)
        {
            mainSequence.OnEventLogs += OnEventLogs;
            logItemType = logType;
        }

        private void OnEventLogs(object sender, LogMsg e)
        {
            if (e.logUsr == logItemType)
            {
                AddMsg(e.msg);
            }
        }

        private const int maxLine = 100;
        public void AddMsg(string msg)
        {
            if (listText.Count() >= maxLine)
                listText.RemoveAt(0);
            listText.Add(msg);

            Text = string.Empty;
            foreach (string val in listText)
                Text += val;
        }

        ObservableCollection<string> listText = new ObservableCollection<string>();
        string text = string.Empty;
        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                text = value;
                OnPropertyChanged();
            }
        }
    }
}
