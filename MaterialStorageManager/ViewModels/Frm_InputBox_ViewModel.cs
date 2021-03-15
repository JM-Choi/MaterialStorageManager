using MaterialDesignThemes.Wpf;
using MaterialStorageManager.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MaterialStorageManager.ViewModels
{
    class Frm_InputBox_ViewModel : Notifier
    {
        MsgBox msgBox = MsgBox.Inst;

        public Action CloseAction { get; set; }
        public ICommand ButtonEvent { get; set; }
        public Frm_InputBox_ViewModel()
        {
            msgBox.OnEventInputBoxData += OnEventInputBoxData;
            ButtonEvent = new Command(ButtonEventMethod);
        }

        private void ButtonEventMethod(object obj)
        {
            msgBox.btnRlt = obj.ToString().ToEnum<MsgBox.eBTNTYPE>();
            msgBox.content = MsgBox.eBTNTYPE.CANCEL == msgBox.btnRlt ? null : TxtContent;

            if (string.IsNullOrEmpty(msgBox.content))
            {
                msgBox.btnRlt = MsgBox.eBTNTYPE.CANCEL;
                msgBox.content = null;
            }

            CloseAction();
        }

        private void OnEventInputBoxData(object sender, INPUTBOXDATA e)
        {
            IconKind = e.packIcon;
            Title = e.title;
        }

        PackIconKind iconKind;
        public PackIconKind IconKind
        {
            get
            {
                return iconKind;
            }
            set
            {
                iconKind = value;
                OnPropertyChanged();
            }
        }
        string title = string.Empty;
        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
                OnPropertyChanged();
            }
        }

        string txtContent = string.Empty;
        public string TxtContent
        {
            get
            {
                return txtContent;
            }
            set
            {
                txtContent = value;
                OnPropertyChanged();
            }
        }
    }
}
