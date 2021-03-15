using System;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using MaterialStorageManager.Utils;
using System.Windows.Threading;
using Newtonsoft.Json;
using System.Net.Http;
using System.IO;
using System.Diagnostics;
using System.Windows.Controls;

namespace MaterialStorageManager.Models
{
    public class MainSequence
    {
        #region MainSequence.Inst 싱글톤 객체 생성
        private static volatile MainSequence instance;
        private static object syncObj = new object();
        public static MainSequence Inst
        {
            get
            {
                if (instance == null)
                {
                    lock (syncObj)
                    {
                        if (instance == null)
                            instance = new MainSequence();
                    }
                }
                return instance;
            }
        }

        private _Data _data = _Data.Inst._data;
        private SYS _sys;
        private MDL _mdl;
        public SYS_STATUS _sysStatus;
        private GOALITEM _SelItem = new GOALITEM();

        //public XmlControl xmlControl = new XmlControl();
        DateTime DataTime = new DateTime();
        #endregion

        public event EventHandler<MMSTATUSGROUP> OnEventMMState;
        public event EventHandler<APIRECVDATA> OnEventJobState;
        public event EventHandler<LOGGROUP> OnEventLog;
        public event EventHandler<PROVIDINGDATA> OnEventProvingData;
        public event EventHandler<RECVINGDATA> OnEventRecvingData;
        public event EventHandler<VIEWIOLIST> OnEventViewIOList;
        public event EventHandler<CONFIG> OnEventViewOptionList;
        public event EventHandler<PIO> OnEventViewPIOList;
        public event EventHandler<LAMPINFO> OnEventViewLampList;
        public event EventHandler<GOALINFO> OnEventViewGoalList;
        public event EventHandler<GOALITEM> OnEventViewGoalItemChange;
        public event EventHandler<USER> OnEventUpdateUser;

        //string stateURL = "http://ptsv2.com/t/hvmuv-1604648300/post";
        //string stateURL = "http://ptsv2.com/t/j2glg-1603952070/post";
        //string jobURL = "http://ptsv2.com/t/j2glg-1603952070/post";
        //string jobstateURL = "http://ptsv2.com/t/p1i36-1604014353/post";
        //string operatorURL = "http://ptsv2.com/t/operator/post";
        //string materialURL = "http://ptsv2.com/t/mdi9g-1604014541/post";
        //string alarmURL = "http://ptsv2.com/t/dvxu4-1604014567/post";

        string stateURL = "status";
        string jobURL = string.Empty;
        string jobstateURL = "jobreport";
        string operatorURL = "operator";
        string materialURL = "material";
        string alarmURL = "alarm";

        public CONFIGGROUP configGP = new CONFIGGROUP();
        public MMSTATUSGROUP mmstatusGP = new MMSTATUSGROUP();
        public STATUSGROUP statusGP = new STATUSGROUP();
        public JOBSTATUSGROUP jobstatusGP = new JOBSTATUSGROUP();
        public OPERATORGROUP operatorGP = new OPERATORGROUP();
        public ALARMGROUP alarmGP = new ALARMGROUP();
        public MATERIALGROUP materialGP = new MATERIALGROUP();
        public LOGGROUP logGP = new LOGGROUP();
        public APIRECVDATA recvdataGP = new APIRECVDATA();
        public PROVIDINGDATA providingData = new PROVIDINGDATA();
        public RECVINGDATA recvingData = new RECVINGDATA();
        public VIEWIOLIST viewIOList = new VIEWIOLIST();
        public bool Send = false;
        public bool ThreadFlag = true;
        public eVIWER CurrentView = eVIWER.Manual;
        string[] ran_materialname = { "Alcohol", "Methanol", "HydrogenPeroxide", "Dextrose" };
        string[] ran_materiallot = { "LAC01S", "TUH20HH", "QWE123", "ASX009", "POK39JE" };

        ServerResp serverRESP = new ServerResp();

        public DispatcherOperation SendDispatcher;

        public MainSequence()
        {
            _sys = _Data.Inst.sys;
            _mdl = _Data.Inst.mdl;
            _sysStatus = _Data.Inst.sys.status;
            viewIOList.ioSRC = _sys.io.lst;
            //xmlControl.Load();
            SendDispatcher = Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
            {
                StateSendThread();
            }
                    );

        }


        #region RestAPI
        public async void StateSendThread()
        {
            while (ThreadFlag)
            {
                try
                {
                    if (Send)
                        stateComm();
                }
                catch (Exception ex)
                {

                }
                await Task.Delay(1000);

            }
        }

        public void stateComm()
        {
            DataTime = DateTime.Now;
            StateReport stateRESP = new StateReport
            {
                Id = configGP.TowerID,
                Time = DataTime,
                Mode = statusGP.StrgMode,
                State = statusGP.StrgState,
                AccessState = statusGP.StrgAccState,
                JobId = recvdataGP.JobID,
                Msg = "state Check"
            };

            string JsonSerial = JsonConvert.SerializeObject(stateRESP);

            Textbox_Write(JsonSerial, stateURL);
            if (serverRESP != null && serverRESP.Job != null && recvdataGP.JobID != serverRESP.Job.Id)
            {
                recvdataGP.JobID = serverRESP.Job.Id;
                recvdataGP.UuidID = serverRESP.Job.Uuid;
                recvdataGP.OperName = serverRESP.Job.Oper;
                recvdataGP.ViewEnable = true;
                serverRESP.Job = null;
                OnEventJobState?.Invoke(this, recvdataGP);
            }
        }

        public void jobstateComm()
        {
            DataTime = DateTime.Now;
            JobReport jobREPT = new JobReport
            {
                Id = configGP.TowerID,
                Time = DataTime,
                JobId = recvdataGP.JobID,
                JobState = jobstatusGP.JobState
            };

            string JsonSerial = JsonConvert.SerializeObject(jobREPT);
            Textbox_Write(JsonSerial, jobstateURL);

            if (jobREPT.JobState == JobState.Complete)
            {
                recvdataGP = new APIRECVDATA();
                OnEventJobState?.Invoke(this, recvdataGP);
            }
        }

        public void operatorComm()
        {
            DataTime = DateTime.Now;
            QueryOperator oper = new QueryOperator
            {
                Id = configGP.TowerID,
                Time = DataTime,
                OperId = operatorGP.OperatorNum
            };

            string JsonSerial = JsonConvert.SerializeObject(oper);
            Textbox_Write(JsonSerial, operatorURL);
        }

        public void materialComm()
        {
            DataTime = DateTime.Now;
            Random random = new Random();
            MtrMovedReport mtrmove = new MtrMovedReport
            {
                Id = configGP.TowerID,
                Uuid = string.IsNullOrEmpty(recvdataGP.UuidID) ? "TEST" + DataTime.ToString("yyyyMMddhhmmss") : recvdataGP.UuidID,
                IsProviding = materialGP.Provide == true ? true : false,
                Time = DataTime,
                OperId = string.IsNullOrEmpty(recvdataGP.OperName) ? "NoOperationName" : recvdataGP.OperName,
                Material = new MrtDetail
                {
                    Name = ran_materialname[random.Next(ran_materialname.Length)],
                    LotId = ran_materiallot[random.Next(ran_materiallot.Length)],
                    Expiry = DateTime.Now.AddYears(1),
                    Qty = random.Next(1, 999),
                    TypeName = "",
                }
            };

            string JsonSerial = JsonConvert.SerializeObject(mtrmove);
            Textbox_Write(JsonSerial, materialURL);
        }

        public void alarmComm()
        {
            DataTime = DateTime.Now;
            AlarmReport alarmReport = new AlarmReport
            {
                Id = configGP.TowerID,
                Time = DataTime,
                AlarmCode = alarmGP.AlarmCode,
                IsAlarmSet = alarmGP.AlarmState == true ? true : false
            };

            string JsonSerial = JsonConvert.SerializeObject(alarmReport);
            Textbox_Write(JsonSerial, alarmURL);
        }

        public async void Textbox_Write(string JsonSerial, string url)
        {
            try
            {
                logGP.LogText = $"{JsonSerial}\r\n";
                logGP.LogType = "TX : ";
                OnEventLog?.Invoke(this, logGP);
                string wbPostResult = await callWebPost(JsonSerial, url);
                if (string.IsNullOrEmpty(wbPostResult))
                {
                    logGP.LogText = $"Post Fail\r\n";
                    logGP.LogType = string.Empty;
                    OnEventLog?.Invoke(this, logGP);
                    return;
                }
                logGP.LogText = $"{wbPostResult}\r\n";
                logGP.LogType = string.Empty;
                OnEventLog?.Invoke(this, logGP);

                serverRESP = JsonConvert.DeserializeObject<ServerResp>(wbPostResult);

                logGP.LogText = $"time : {serverRESP.Time}\r\n";
                logGP.LogText += $"state : {serverRESP.State}\r\n";
                logGP.LogText += $"msg : {serverRESP.Msg}\r\n";

                if (serverRESP.Job != null)
                {
                    logGP.LogText += $"Job id : {serverRESP.Job.Id}\r\n";
                    logGP.LogText += $"Job uuid : {serverRESP.Job.Uuid}\r\n";
                    logGP.LogText += $"Job oper : {serverRESP.Job.Oper}\r\n";
                }
                if (url.Contains(operatorURL))
                    recvdataGP.OperName = serverRESP.Msg;
                logGP.LogType = "RX : ";
                OnEventLog?.Invoke(this, logGP);

                mmstatusGP.MMstate = serverRESP.State.ToString();
                OnEventMMState?.Invoke(this, mmstatusGP);

            }
            catch (Exception ex)
            {

            }
        }

        public void EventLogSend(LOGGROUP logGP)
        {
            
        }


        public async Task<string> callWebPost(string JsonSerial, string url)
        {
            string responseText = string.Empty;

            try
            {
                string post_url = configGP.URL + url;
                using (var client = new HttpClient())
                {
                    using (var request = new HttpRequestMessage(HttpMethod.Post, post_url))
                    {
                        if (!string.IsNullOrEmpty(JsonSerial))
                        {
                            request.Content = new StringContent(JsonSerial, Encoding.UTF8, "application/json");
                        }
                        try
                        {
                            using (var response = await client.SendAsync(request))
                            {
                                var stream = await response.Content.ReadAsStreamAsync();
                                var returnStr = await StreamToStringAsync(stream);
                                stream.Close();
                                return returnStr;
                            }
                        }
                        catch (Exception e)
                        {
                            Debug.WriteLine($"서버 응답 없음.[{post_url}]:{e.ToString()}");
                            return string.Empty;
                        }

                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return responseText;
        }

        private async Task<string> StreamToStringAsync(Stream stream)
        {
            string content = null;
            if (stream != null)
            {
                using (var sr = new StreamReader(stream))
                {
                    content = await sr.ReadToEndAsync();
                }
            }
            return content;
        }

        public void ConfigSave()
        {
            //xmlControl.DataUpdate(configGP);
            //xmlControl.Save();
        }
        #endregion

        #region View
        public void ViewDataUpdate(UserControl userControl, string ucname = "")
        {
            string UC_Name = string.Empty;
            if (userControl != null)
                UC_Name = userControl.ToString().Split('.')[2].Split('_')[2];
            else
                UC_Name = ucname;

            switch(UC_Name)
            {
                case "Monitor":
                    OnEventProvingData?.Invoke(this, providingData);
                    OnEventRecvingData?.Invoke(this, recvingData);
                    break;
                case "Manual":
                    break;
                case "IO":
                    OnEventViewIOList?.Invoke(this, viewIOList);
                    break;
                case "TowerLamp":
                    OnEventViewLampList?.Invoke(this, _sys.lmp);
                    break;
                case "Goal":
                    OnEventViewGoalList?.Invoke(this, _sys.goals);
                    break;
                case "PIO":
                    OnEventViewPIOList?.Invoke(this, _sys.cfg.pio);
                    break;
                case "Option":
                    OnEventViewOptionList?.Invoke(this, _sys.cfg);
                    break;
            }
            CurrentView = (eVIWER)Enum.Parse(typeof(eVIWER), UC_Name);
        }

        public void UpdateUserData(USER userData)
        {
            OnEventUpdateUser?.Invoke(this, userData);
        }

        public void TowerLampData(TWRLAMPUID uid, eEQPSATUS state)
        {
            var val = _sys.lmp.GetLmp(state);
            switch (uid)
            {
                case TWRLAMPUID.GREEN_OFF:                    
                case TWRLAMPUID.GREEN_ON:
                case TWRLAMPUID.GREEN_BLINK:
                    val.Green = (TWRLAMP)((int)uid % 10);
                    break;
                case TWRLAMPUID.YELLOW_OFF:
                case TWRLAMPUID.YELLOW_ON:
                case TWRLAMPUID.YELLOW_BLINK:
                    val.Yellow = (TWRLAMP)((int)uid % 10);
                    break;
                case TWRLAMPUID.RED_OFF:
                case TWRLAMPUID.RED_ON:
                case TWRLAMPUID.RED_BLINK:
                    val.Red = (TWRLAMP)((int)uid % 10);
                    break;
                case TWRLAMPUID.BUZZER_OFF:
                case TWRLAMPUID.BUZZER_ON:
                    val.Buzzer = 30 == (int)uid ? false : true;
                    break;
            }
        }

        public void OptionData(OPTIONUID uid, object data)
        {
            var cfg = _sys.cfg;

            switch (uid)
            {
                case OPTIONUID.EQPNAME:
                    cfg.eqpName = data.ToString();
                    break;
                case OPTIONUID.LANGUAGE:
                    cfg.language = (eLANGUAGE)data;
                    break;
                case OPTIONUID.MPLUSIP:
                    cfg.mplusIP = data.ToString();
                    break;
                case OPTIONUID.MPLUSPORT:
                    cfg.mplusPort = Convert.ToInt32(data);
                    break;
                case OPTIONUID.VECIP:
                    cfg.VecIP = data.ToString();
                    break;
            }
        }

        public void PIOData(PIOUID uid, int data)
        {
            var pio = _sys.cfg.pio;

            switch (uid)
            {
                case PIOUID.PIO_0:
                    pio.nInterfaceTimeout = data;
                    break;
                case PIOUID.PIO_1:
                    pio.nDockSenChkTime = data;
                    break;
                case PIOUID.PIO_2:
                    pio.nFeedTimeOut_Start = data;
                    break;
                case PIOUID.PIO_3:
                    pio.nFeedTimeOut_Work = data;
                    break;
                case PIOUID.PIO_4:
                    pio.nFeedTimeOut_End = data;
                    break;
            }
        }

        public void TreeViewData(TreeData SelTreeData)
        {
            var goaltype = SelTreeData.Parent.ToEnum<eGOALTYPE>();
            var goalInfo = _sys.goals;
            _SelItem = goalInfo.Get(goaltype, SelTreeData.Name, true);
            OnEventViewGoalItemChange?.Invoke(this, _SelItem);
        }

        public bool GoalAdd(string MsgBoxRlt)
        {
            var goals = _sys.goals;
            return goals.Add(new GOALITEM() { type = _SelItem.type, name = MsgBoxRlt, line = _SelItem.line, label = MsgBoxRlt });
        }

        public bool GoalDel()
        {
            var goals = _sys.goals;
            return goals.Remove(_SelItem);
        }
        #endregion

    }
}
