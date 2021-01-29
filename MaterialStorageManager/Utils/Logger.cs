using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace MaterialStorageManager.Utils
{
    public enum CmdLogType
    {
        prdt,
        Comm,
        Gem,
        Err,
        Debug
    }

    public enum CommLogType
    {
        None,
        MPlus,
        VEHICLE,
        UR,
        QR,
        LDS,
        Conv,
        IO,
        Servo,
        TactTime
    }

    public class Logger
    {
        private static volatile Logger instance;
        private static object syncRoot = new object();
        private static ExEzLogger PrdtLog;
        public event EventHandler<WriteLogArgs> OnWriteLog;
        public int QueueCnt => PrdtLog.QueueCnt;
        public static Logger Inst
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new Logger();
                    }
                }
                return instance;
            }
        }

        private Logger()
        {

        }

        public void MakeSrcHdl(string MainFoderName)
        {
            PrdtLog = new ExEzLogger("C:\\_Log\\", MainFoderName);
            PrdtLog.LoggerStart();
        }

        public void LoggerClose()
        {
            PrdtLog.LoggerStop();
        }

        public void Write(CmdLogType type, string msg, CommLogType CommType = CommLogType.None)
        {
            if (null == PrdtLog) return;
            string logmsg = string.Empty;
            logmsg = $"{DateTime.Now.ToString("HH:mm:ss.fff")} : {msg}";
            PrdtLog.StrAdd(type, logmsg, CommType);
            Debug.WriteLine($"{type.ToString()} : {msg}");
            OnWriteLog?.Invoke(this, new WriteLogArgs() { name = "", type = type, msg = logmsg, CommType = CommType });
        }

        internal void WriteLog(object fatal, string v)
        {
            throw new NotImplementedException();
        }
    }

    public class ExEzLogger
    {
        private string _PathFodler;
        private eLOGVER _Ver;
        enum eLOGVER
        {
            Old,
            New
        }
        public ExEzLogger(string sPath, string sName)
        {
            _Ver = eLOGVER.New;
            _PathFodler = $"{sPath}\\{sName}";
            DirectoryInfo dtif = new DirectoryInfo(_PathFodler);
            if (!dtif.Exists)
            {
                dtif.Create();
            }
        }

        private void MakeDir(CmdLogType type, CommLogType CommType)
        {
            DateTime Date = DateTime.Now;
            string strY, strM, strD;
            DirectoryInfo dtif = new DirectoryInfo(_PathFodler);
            strY = string.Format("{0:yyyy}", Date);
            strM = string.Format("{0:MM}", Date);
            strD = string.Format("{0:dd}", Date);
            switch (_Ver)
            {
                case eLOGVER.Old:
                    dtif = new DirectoryInfo(string.Format("{0}\\{1}\\{2}", _PathFodler, type.ToString(), strY));
                    if (!dtif.Exists)
                    {
                        dtif.Create();
                    }
                    switch (CommType)
                    {
                        case CommLogType.MPlus:
                        case CommLogType.VEHICLE:
                        case CommLogType.LDS:
                        case CommLogType.QR:
                        case CommLogType.UR:
                        case CommLogType.TactTime:
                            dtif = new DirectoryInfo(string.Format("{0}\\{1}\\{2}\\{3}", _PathFodler, type.ToString(), strY, strM));
                            if (!dtif.Exists)
                            {
                                dtif.Create();
                            }

                            dtif = new DirectoryInfo(string.Format("{0}\\{1}\\{2}\\{3}\\{4}", _PathFodler, type.ToString(), CommType.ToString(), strY, strM));
                            if (!dtif.Exists)
                            {
                                dtif.Create();
                            }
                            break;
                        case CommLogType.None:
                        default:
                            dtif = new DirectoryInfo(string.Format("{0}\\{1}\\{2}\\{3}", _PathFodler, type.ToString(), strY, strM));
                            if (!dtif.Exists)
                            {
                                dtif.Create();
                            }
                            break;
                    }
                    break;
                case eLOGVER.New:
                    dtif = new DirectoryInfo($"{_PathFodler}\\{strY}");
                    if (!dtif.Exists)
                    {
                        dtif.Create();
                    }
                    dtif = new DirectoryInfo($"{_PathFodler}\\{strY}\\{strM}");
                    if (!dtif.Exists)
                    {
                        dtif.Create();
                    }
                    dtif = new DirectoryInfo($"{_PathFodler}\\{strY}\\{strM}\\{strD}");
                    if (!dtif.Exists)
                    {
                        dtif.Create();
                    }
                    switch (type)
                    {
                        case CmdLogType.Comm:
                            dtif = new DirectoryInfo($"{_PathFodler}\\{strY}\\{strM}\\{strD}\\{type.ToString()}");
                            if (!dtif.Exists)
                            {
                                dtif.Create();
                            }
                            break;
                        default: break;
                    }
                    break;
            }
        }

        private string GetFullPath(CmdLogType type, CommLogType CommType)
        {
            string rtn = string.Empty;
            DateTime Date = DateTime.Now;
            string strY, strM, strD, strFileName, strFullPath = "";
            strY = string.Format("{0:yyyy}", Date);
            strM = string.Format("{0:MM}", Date);
            strD = string.Format("{0:dd}", Date);
            switch (_Ver)
            {
                case eLOGVER.Old:
                    strFileName = string.Format("{0}_{1:dd}.txt", type.ToString(), Date);
                    strFullPath = string.Format("{0}\\{1}\\{2}\\{3}\\{4}", _PathFodler, type.ToString(), strY, strM, strFileName);
                    switch (CommType)
                    {
                        case CommLogType.MPlus:
                        case CommLogType.VEHICLE:
                        case CommLogType.LDS:
                        case CommLogType.QR:
                        case CommLogType.UR:
                        case CommLogType.TactTime:
                            rtn = string.Format("{0}\\{1}\\{2}\\{3}\\{4}\\{5}", _PathFodler, type.ToString(), CommType.ToString(), strY, strM, strFileName);
                            break;
                        default:
                        case CommLogType.None:
                            rtn = string.Format("{0}\\{1}\\{2}\\{3}\\{4}", _PathFodler, type.ToString(), strY, strM, strFileName);
                            break;
                    }
                    break;
                case eLOGVER.New:
                    switch (type)
                    {
                        case CmdLogType.Comm:
                            switch (CommType)
                            {
                                case CommLogType.MPlus:
                                case CommLogType.VEHICLE:
                                case CommLogType.UR:
                                case CommLogType.QR:
                                case CommLogType.LDS:
                                case CommLogType.Conv:
                                case CommLogType.IO:
                                case CommLogType.TactTime:
                                    rtn = $"{_PathFodler}\\{strY}\\{strM}\\{strD}\\{type.ToString()}\\{CommType.ToString()}.txt";
                                    break;
                                default: rtn = $"{_PathFodler}\\{strY}\\{strM}\\{strD}\\{type.ToString()}.txt"; break;
                            }
                            break;
                        default: rtn = $"{_PathFodler}\\{strY}\\{strM}\\{strD}\\{type.ToString()}.txt"; break;
                    }
                    break;
            }
            return rtn;
        }

        private string GetTime(bool bNeed2MilSec = true)
        {
            DateTime NowTime = DateTime.Now;
            if (bNeed2MilSec) return NowTime.ToString("HH:mm:ss.") + NowTime.Millisecond.ToString("000");
            else return NowTime.ToString("HH_mm_ss");
        }

        public void Write(CmdLogType type, string LogMsg, CommLogType CommType)
        {
            MakeDir(type, CommType);
            string sFullPath = GetFullPath(type, CommType);
            string log = $"{LogMsg}";
            FileInfo file = new FileInfo(sFullPath);
            try
            {
                if (!file.Exists)
                {
                    using (StreamWriter sw = new StreamWriter(sFullPath))
                    {
                        sw.WriteLine(log);
                        sw.Close();
                    }
                }
                else
                {
                    using (StreamWriter sw = File.AppendText(sFullPath))
                    {
                        sw.WriteLine(log);
                        sw.Close();
                    }
                }
            }
            catch (Exception e)
            {
                StrAdd(CmdLogType.prdt, $"Event Log file write Error.[{log}]\r\n{e.ToString()}", CommLogType.MPlus);
            }
        }

        struct QueueMsg
        {
            public CmdLogType type;
            public CommLogType CommType;
            public string msg;
        }
        private ConcurrentQueue<QueueMsg> messageQueue = new ConcurrentQueue<QueueMsg>();

        public void StrAdd(CmdLogType type, string msg, CommLogType CommType)
        {
            messageQueue.Enqueue(new QueueMsg() { type = type, msg = msg, CommType = CommType });
        }

        private bool loggerRun = true;
        public int QueueCnt => messageQueue.Count;
        public async void LoggerStart()
        {
            await MessageWriter();
        }
        public async void LoggerStop()
        {
            loggerRun = false;
            while (0 < messageQueue.Count)
            {
                await Task.Delay(100);
            }
        }

        private async Task MessageWriter()
        {
            while (true)
            {
                try
                {
                    if (messageQueue.Count > 0)
                    {
                        if (messageQueue.TryDequeue(out QueueMsg res))
                        {
                            Write(res.type, res.msg, res.CommType);
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.Write(false, $"Logger Exception : {e.ToString()}");
                }
                await Task.Delay(1);
                if (false == loggerRun && 0 == messageQueue.Count)
                {
                    break;
                }
            }
        }
    }

    public class WriteLogArgs : EventArgs
    {
        public string name;
        public CmdLogType type;
        public CommLogType CommType;
        public string msg;
    }
}
