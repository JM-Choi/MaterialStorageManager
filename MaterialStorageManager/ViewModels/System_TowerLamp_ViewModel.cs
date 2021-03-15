using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MaterialStorageManager.Utils;

namespace MaterialStorageManager.ViewModels
{
    class System_TowerLamp_ViewModel : Notifier
    {
        Models.MainSequence mainSequence = Models.MainSequence.Inst;
        public ICommand ListBox_SelectedItem_Changed { get; set; }
        public ICommand Radiobutton_Changed { get; set; }

        int twrLmp_SelectedItem = 0;

        public System_TowerLamp_ViewModel()
        {
            mainSequence.OnEventViewLampList += OnEventViewLampList;
            ListBox_SelectedItem_Changed = new Command(ExecuteMethod, CanExecuteMethod);
            Radiobutton_Changed = new Command(Radiobutton_Changed_Method);

            SubItems = new[]
            {
                new SubItem(" INITIALIZE", MaterialDesignThemes.Wpf.PackIconKind.LockReset),
                new SubItem(" STOP", MaterialDesignThemes.Wpf.PackIconKind.Stop),
                new SubItem(" STOPPING", MaterialDesignThemes.Wpf.PackIconKind.Stopwatch),
                new SubItem(" IDLE", MaterialDesignThemes.Wpf.PackIconKind.Walk),
                new SubItem(" RUN", MaterialDesignThemes.Wpf.PackIconKind.RunFast),
                new SubItem(" ERROR", MaterialDesignThemes.Wpf.PackIconKind.Error),
                new SubItem(" EMG", MaterialDesignThemes.Wpf.PackIconKind.CarEmergencyBrake)

            };
        }

        private void Radiobutton_Changed_Method(object obj)
        {
            TWRLAMPUID twruid = (TWRLAMPUID)(Convert.ToInt32(obj));
            eEQPSATUS state = (eEQPSATUS)(Convert.ToInt32(twrLmp_SelectedItem));
            mainSequence.TowerLampData(twruid, state);
            
        }

        private bool CanExecuteMethod(object arg)
        {
            return true;
        }

        private void ExecuteMethod(object obj)
        {
            if ((int)obj > -1)
            {
                twrLmp_SelectedItem = (int)obj;
                mainSequence.ViewDataUpdate(null, "TowerLamp");
            }
        }

        private void OnEventViewLampList(object sender, LAMPINFO e)
        {
            switch (e.lst[twrLmp_SelectedItem].Green)
            {
                case TWRLAMP.OFF:
                    Green_Off = true;
                    break;
                case TWRLAMP.ON:
                    Green_On = true;
                    break;
                case TWRLAMP.BLINK:
                    Green_Blink = true;
                    break;
            }
            switch (e.lst[twrLmp_SelectedItem].Yellow)
            {
                case TWRLAMP.OFF:
                    Yellow_Off = true;
                    break;
                case TWRLAMP.ON:
                    Yellow_On = true;
                    break;
                case TWRLAMP.BLINK:
                    Yellow_Blink = true;
                    break;
            }

            switch (e.lst[twrLmp_SelectedItem].Red)
            {
                case TWRLAMP.OFF:
                    Red_Off = true;
                    break;
                case TWRLAMP.ON:
                    Red_On = true;
                    break;
                case TWRLAMP.BLINK:
                    Red_Blink = true;
                    break;
            }

            switch (e.lst[twrLmp_SelectedItem].Buzzer)
            {
                case false: 
                    Buzzer_Off = true;
                    break;
                case true:
                    Buzzer_On = true;
                    break;
            }

            BlinkTime = e.blinkTime;
        }

        SubItem[] subItems;
        public SubItem[] SubItems
        {
            get
            {
                return subItems;
            }
            set
            {
                subItems = value;
                OnPropertyChanged();
            }
        }

        bool green_Off = false;
        public bool Green_Off
        {
            get
            {
                return green_Off;
            }
            set
            {
                green_Off = value;
                OnPropertyChanged();
            }
        }

        bool green_On = false;
        public bool Green_On
        {
            get
            {
                return green_On;
            }
            set
            {
                green_On = value;
                OnPropertyChanged();
            }
        }

        bool green_Blink = false;
        public bool Green_Blink
        {
            get
            {
                return green_Blink;
            }
            set
            {
                green_Blink = value;
                OnPropertyChanged();
            }
        }

        bool yellow_Off = false;
        public bool Yellow_Off
        {
            get
            {
                return yellow_Off;
            }
            set
            {
                yellow_Off = value;
                OnPropertyChanged();
            }
        }

        bool yellow_On = false;
        public bool Yellow_On
        {
            get
            {
                return yellow_On;
            }
            set
            {
                yellow_On = value;
                OnPropertyChanged();
            }
        }

        bool yellow_Blink = false;
        public bool Yellow_Blink
        {
            get
            {
                return yellow_Blink;
            }
            set
            {
                yellow_Blink = value;
                OnPropertyChanged();
            }
        }

        bool red_Off = false;
        public bool Red_Off
        {
            get
            {
                return red_Off;
            }
            set
            {
                red_Off = value;
                OnPropertyChanged();
            }
        }

        bool red_On = false;
        public bool Red_On
        {
            get
            {
                return red_On;
            }
            set
            {
                red_On = value;
                OnPropertyChanged();
            }
        }

        bool red_Blink = false;
        public bool Red_Blink
        {
            get
            {
                return red_Blink;
            }
            set
            {
                red_Blink = value;
                OnPropertyChanged();
            }
        }

        bool buzzer_Off = false;
        public bool Buzzer_Off
        {
            get
            {
                return buzzer_Off;
            }
            set
            {
                buzzer_Off = value;
                OnPropertyChanged();
            }
        }

        bool buzzer_On = false;
        public bool Buzzer_On
        {
            get
            {
                return buzzer_On;
            }
            set
            {
                buzzer_On = value;
                OnPropertyChanged();
            }
        }

        int blinkTime = 0;
        public int BlinkTime
        {
            get
            {
                return blinkTime;
            }
            set
            {
                blinkTime = value;
                OnPropertyChanged();
            }
        }
    }
}
