using MaterialStorageManager.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace MaterialStorageManager.ViewModels
{
    class System_Goal_ViewModel : Notifier
    {
        Models.MainSequence mainSequence = Models.MainSequence.Inst;
        MsgBox msgBox = MsgBox.Inst;
        public ICommand TreeViewSelectedChanged { get; set; }
        public ICommand AddButtonClick { get; set; }
        public ICommand DelButtonClick { get; set; }
        public ICommand ComboEvent { get; set; }
        public IEnumerable<eLINE> Line { get; set; }

        GOALITEM SelTreeItem = new GOALITEM();

        public System_Goal_ViewModel()
        {
            mainSequence.OnEventViewGoalList += OnEventViewGoalList;
            mainSequence.OnEventViewGoalItemChange += OnEventViewGoalItemChange;
            TreeViewSelectedChanged = new Command(TreeViewChangedMethod);
            AddButtonClick = new Command(AddButtonClickMethod);
            DelButtonClick = new Command(DelButtonClickMethod);
            ComboEvent = new Command(ComboChangedEvent);

            Line = Enum.GetValues(typeof(eLINE)).Cast<eLINE>();
        }

        private void OnEventViewGoalItemChange(object sender, GOALITEM e)
        {
            SelTreeItem = e;
            if (e != null)
            {
                GoalInfo = $"{e.type}, {e.label}";
                LineSel = e.line;
                HostName = e.hostName;
                Label = e.label;
            }
            else
            {
                GoalInfo = string.Empty;
                LineSel = new eLINE();
                HostName = string.Empty;
                Label = string.Empty;
            }
        }
        private void GoalViewInit()
        {
            TreeGoals.Clear();
            GoalInfo = string.Empty;
            LineSel = new eLINE();
            HostName = string.Empty;
            Label = string.Empty;
        }
        private void OnEventViewGoalList(object sender, GOALINFO e)
        {
            GoalViewInit();
            foreach (eGOALTYPE item in Enum.GetValues(typeof(eGOALTYPE)))
            {
                ObservableCollection<TreeData> SubChild1 = new ObservableCollection<TreeData>();
                var lst = e.GetList(item);
                foreach (eLINE line in Enum.GetValues(typeof(eLINE)))
                {
                    if (eLINE.None == line) continue;
                    ObservableCollection<TreeData> SubChild2 = new ObservableCollection<TreeData>();
                    var linegoal = lst.Where(l => l.line == line).ToList();
                    foreach (var goal in linegoal)
                    {
                        SubChild2.Add(new TreeData() { Name = goal.label, Parent = item.ToString() });
                    }
                    SubChild1.Add(new TreeData() { Name = $"Line #{line.ToString().Replace("_", "")}", Children = SubChild2 });
                }

                TreeData treeData = new TreeData() { Name = item.ToString(), Children = SubChild1 };

                TreeGoals.Add(treeData);
            }
        }

        private void TreeViewChangedMethod(object obj)
        {
            if (obj != null)
            {
                TreeData SelTreeData = (TreeData)obj;
                if (SelTreeData.Children.Count == 0)
                    mainSequence.TreeViewData(SelTreeData);
            }
        }

        private void AddButtonClickMethod(object obj)
        {
            if (SelTreeItem == null)
                return;
            var edit = msgBox.ShowInputBoxDlg(MaterialDesignThemes.Wpf.PackIconKind.DatabaseEdit, $"Goal Editor");
            switch (edit.rtn)
            {
                case MsgBox.eBTNTYPE.OK:
                    {
                        var addrtn = mainSequence.GoalAdd(edit.rlt);
                        if (true == addrtn)
                        {
                            mainSequence.ViewDataUpdate(null, "Goal");
                        }
                        else
                        {
                            msgBox.ShowDialog($"{ SelTreeItem.type}-{edit.rlt}의 골 등록에 실패하였습니다.", MsgBox.MsgType.Warn, MsgBox.eBTNSTYLE.OK);
                        }
                        break;
                    }
                default: break;
            }
        }

        private void DelButtonClickMethod(object obj)
        {
            if (SelTreeItem == null)
                return;
            var delrtn = msgBox.ShowDialog($"{SelTreeItem.type}-{SelTreeItem.name}의 골을 제거하시겟습니까?", MsgBox.MsgType.Warn, MsgBox.eBTNSTYLE.OK_CANCEL);
            switch (delrtn)
            {
                case MsgBox.eBTNTYPE.OK:
                    {

                        var chk = mainSequence.GoalDel();
                        if (true == chk)
                        {
                            mainSequence.ViewDataUpdate(null, "Goal");
                        }
                        else
                        {
                            msgBox.ShowDialog($"{ SelTreeItem.type}-{SelTreeItem.name}의 골 제거에 실패하였습니다.", MsgBox.MsgType.Warn, MsgBox.eBTNSTYLE.OK);
                        }
                        break;
                    }
                default: break;
            }
        }

        eLINE comboSelectedValue;
        private void ComboChangedEvent(object obj)
        {
            comboSelectedValue = (eLINE)obj;
        }

        ObservableCollection<TreeData> treegoals = new ObservableCollection<TreeData>();
        public ObservableCollection<TreeData> TreeGoals
        {
            get
            {
                return treegoals;
            }
            set
            {
                treegoals = value;
                OnPropertyChanged();
            }
        }


        string goalinfo = string.Empty;
        public string GoalInfo
        {
            get
            {
                return goalinfo;
            }
            set
            {
                goalinfo = value;
                OnPropertyChanged();
            }
        }

        eLINE lineSel;
        public eLINE LineSel
        {
            get
            {
                return lineSel;
            }
            set
            {
                lineSel = value;
                OnPropertyChanged();
            }
        }

        string hostname = string.Empty;
        public string HostName
        {
            get
            {
                return hostname;
            }
            set
            {
                hostname = value;
                OnPropertyChanged();
            }
        }

        string label = string.Empty;
        public string Label
        {
            get
            {
                return label;
            }
            set
            {
                label = value;
                OnPropertyChanged();
            }
        }
    }
}
