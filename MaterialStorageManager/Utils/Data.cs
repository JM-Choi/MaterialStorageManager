using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MaterialStorageManager.Utils
{
    public class USER
    {
        public eOPRGRADE grade { get; set; }
        public string id { get; set; }
        public string password { get; set; }
        public string msg { get; set; }
        public USER()
        {
            grade = eOPRGRADE.Operator;
            id = password = msg = string.Empty;
        }
    }

    public class USERINFO
    {
        public List<USER> src = new List<USER>();
        public USERINFO()
        {

        }

        public USER LogIn(string id, string password)
        {
            USER user = new USER();
            if (string.Empty != id && string.Empty != password)
            {
                var rtn = src.Any(s => s.id == id && s.password == password);
                if (true == rtn)
                {
                    user = src.Single(s => s.id == id && s.password == password);
                    user.msg = string.Empty;
                }
                else
                {
                    if (src.Any(s => s.id == id))
                    {
                        user.msg = $"The password is wrong.";
                    }
                    else
                    {
                        user.msg = $"{id} is does not exist.";
                    }
                }
            }
            else
            {
                user.msg = $"Please fill in the id and password\r\n[ID:{id}, Passworrd:{password}]\r\nFinally The Rock has come back to Home!!!!";
            }
            return user;
        }

        public bool Add(USER info)
        {
            if (info.id == "Username" && info.password == "Password")
            {
                info.id = string.Empty;
                info.password = string.Empty;
            }
            bool rtn = false;
            if (string.Empty != info.password)
            {
                if (false == src.Where(s => s.id == info.id).Any())
                {
                    rtn = true;
                    src.Add(new USER() { id = info.id, grade = info.grade, password = info.password, msg = string.Empty });
                    info.msg = $"{info.id} Add complete";
                }
                else
                {
                    info.msg = $"{info.id} is already exist.";
                }
            }
            else
            {
                info.msg = $"Please Insert the password.";
            }
            return rtn;
        }

        public bool Remove(USER info)
        {
            bool rtn = false;
            if (false == src.Where(s => s.id == info.id).Any())
            {
                rtn = src.Remove(info);
            }
            else
            {
                info.msg = $"{info.id} is not found.";
            }
            return rtn;
        }

        ~USERINFO()
        {
            src.Clear();
            src = null;
        }
    }

    public class TIMEARG
    {
        public long nStart = 0;
        public long nDelay = 0;
        public long nCurr = 0;

        private void SetCheckTime(ref long nTime)
        {
            nTime = DateTime.Now.Ticks;
        }

        private bool GetCheckTimeOver(long nStartTm, long nDelay, ref long CurrTm)
        {
            bool rtn = false; long nCurrTime = 0; double dCurr = 0.0f;
            nCurrTime = System.DateTime.Now.Ticks;
            dCurr = (nCurrTime - nStartTm) / 10000.0f;
            rtn = (nDelay < dCurr) ? true : false;
            CurrTm = (long)dCurr;
            return rtn;
        }

        private void ChkCurrTime(ref long CurrTm)
        {
            long nCurrTime = 0; double dCurr = 0.0f;
            nCurrTime = System.DateTime.Now.Ticks;
            dCurr = (nCurrTime - this.nStart) / 10000.0f;
            CurrTm = (long)dCurr;
            this.nStart = 0;
        }

        public void Check()
        {
            switch (this.nStart)
            {
                case 0: SetCheckTime(ref this.nStart); break;
                default: ChkCurrTime(ref this.nCurr); break;
            }
        }

        public void Reset()
        {
            this.nStart = 0;
        }

        public bool IsOver(long Delay)
        {
            bool rtn = false;
            switch (this.nStart)
            {
                case 0:
                    SetCheckTime(ref this.nStart);
                    this.nDelay = Delay;
                    break;
                default:
                    rtn = GetCheckTimeOver(this.nStart, this.nDelay, ref this.nCurr);
                    if (true == rtn)
                    {
                        this.nStart = 0;
                        return true;
                    }
                    break;
            }
            return rtn;
        }
    }

    public class CONNECSION
    {
        public eCOMSTATUS mPlus = eCOMSTATUS.DISCONNECTED;
        public eCOMSTATUS io = eCOMSTATUS.DISCONNECTED;
        public eCOMSTATUS Vec = eCOMSTATUS.DISCONNECTED;
    }

    public class IOSRC
    {
        public eIOTYPE Type { get; set; }
        public eSENTYPE SenType { get; set; }
        public int eID { get; set; }        //  도면상 ID        
        public int RealID { get; set; }     // 실제 물리적으로 구성된 ID                
        public string Label { get; set; }

        public IOSRC()
        {
            Type = eIOTYPE.INPUT;
            SenType = eSENTYPE.A;
            eID = 0;
            RealID = 0;
            Label = string.Empty;
        }
    }

    public class IOINFO
    {
        private int nMaxPortNo;
        public int _MaxPort { get { return nMaxPortNo; } set { nMaxPortNo = value; } }
        public List<IOSRC> lst;
        public IOINFO()
        {
            lst = new List<IOSRC>();
            lst.Clear();
        }

        public bool Add(IOSRC info)
        {
            bool rtn = false;
            if (false == lst.Where(s => s.eID == info.eID && s.Type == info.Type).Any())
            {
                rtn = true;
                lst.Add(new IOSRC() { eID = info.eID, Type = info.Type, RealID = info.RealID, SenType = info.SenType, Label = info.Label });
            }
            return rtn;
        }

        public (int chNo, int bitNo) GetInfo(int id)
        {
            return ((int)id / nMaxPortNo, (int)id % nMaxPortNo);
        }
    }

    public class LAMP
    {
        public eEQPSATUS status = eEQPSATUS.Init;
        public TWRLAMP Green { get; set; }
        public TWRLAMP Red { get; set; }
        public TWRLAMP Yellow { get; set; }
        public bool Buzzer { get; set; }

        public LAMP()
        {
            Green = TWRLAMP.OFF;
            Red = TWRLAMP.OFF;
            Yellow = TWRLAMP.OFF;
            Buzzer = false;
        }
    }

    public class LAMPINFO
    {
        public int blinkTime;
        public List<LAMP> lst;
        public LAMPINFO()
        {
            lst = new List<LAMP>();
            lst.Clear();
        }

        public LAMP GetLmp(eEQPSATUS st)
        {
            return (0 < lst.Count) ? lst.Single(s => s.status == st) : new LAMP();
        }
    }

    public sealed class GOALITEM
    {
        public eGOALTYPE type { get; set; }
        public string name { get; set; }
        public eLINE line { get; set; }
        public string hostName { get; set; }
        public string label { get; set; }

        public GOALITEM()
        {
            type = eGOALTYPE.Standby;
            name = string.Empty;
            line = eLINE.None;
            hostName = label = string.Empty;
        }
    }

    public class GOALINFO
    {
        public List<GOALITEM> lst;
        public GOALINFO()
        {
            lst = new List<GOALITEM>();
            lst.Clear();
        }

        public GOALITEM Get(eGOALTYPE Type, string name, bool bUseLbl = false)
        {
            if (false == bUseLbl)
            {
                if (true == lst.Any(g => g.type == Type && g.name == name))
                {
                    var goal = lst.SingleOrDefault(g => g.type == Type && g.name == name);
                    return goal;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                if (true == lst.Any(g => g.type == Type && g.label == name))
                {
                    var goal = lst.SingleOrDefault(g => g.type == Type && g.label == name);
                    return goal;
                }
                else
                {
                    return null;
                }
            }
        }

        public bool Add(GOALITEM item)
        {
            bool rtn = false;
            if (false == lst.Any(g => g.type == item.type && g.name == item.name))
            {
                var goal = lst.SingleOrDefault(g => g.type == item.type && g.name == item.name);
                if (null == goal)
                {
                    lst.Add(item);
                    rtn = true;
                }
            }
            return rtn;
        }

        public bool Remove(GOALITEM item)
        {
            bool rtn = false;
            if (true == lst.Any(g => g.type == item.type && g.name == item.name))
            {
                rtn = true;
                var fndlst = lst.Where(g => g.type == item.type && g.name == item.name).ToList();
                foreach (var g in fndlst)
                {
                    lst.Remove(item);
                }
            }
            return rtn;
        }

        public List<GOALITEM> GetList(eGOALTYPE type)
        {
            var fndLst = lst.Where(s => s.type == type).ToList();
            return (0 < fndLst.Count) ? fndLst : new List<GOALITEM>();
        }
    }

    public class PIO
    {
        public int nInterfaceTimeout { get; set; } // 설비와 도킹 후 PIO 시작 대기시간 (스메마신호 On 확인)
        public int nDockSenChkTime { get; set; } // 도킹 후 정위치 센서 확인시간
        public int nFeedTimeOut_Start { get; set; } // 피딩시작 후 입구센서에 트래이 감지 타임아웃
        public int nFeedTimeOut_Work { get; set; } // 피딩시작 후 작업완료 타임아웃
        public int nFeedTimeOut_End { get; set; } // 작업종료 후 PIO 종료 (스메마신호 Off 확인)
        public PIO()
        {
            nInterfaceTimeout = 0;
            nDockSenChkTime = 0;
            nFeedTimeOut_Start = 0;
            nFeedTimeOut_Work = 0;
            nFeedTimeOut_End = 0;
        }
    }

    public class CONFIG
    {
        public string eqpName { get; set; }
        public eLANGUAGE language { get; set; }
        public string mplusIP { get; set; }
        public int mplusPort { get; set; }
        public string VecIP { get; set; }
        public PIO pio { get; set; }

        public CONFIG()
        {
            eqpName = string.Empty;
            language = eLANGUAGE.kor;
            mplusIP = string.Empty;
            mplusPort = 0;
            VecIP = string.Empty;
            pio = new PIO();
        }
    }

    public class SYS_STATUS
    {
        public bool bLoaded { get; set; }
        public bool bDebug { get; set; }
        public bool bSimul { get; set; }
        public USER user { get; set; }
        public string swVer { get; set; }
        public CONNECSION conection = new CONNECSION();
        public eEQPSATUS eqpState { get; set; }
        public eMANUAL_WORK eManual { get; set; }
        public string currMdlFile { get; set; }
        public SYS_STATUS()
        {
            bLoaded = false;
            eqpState = eEQPSATUS.Init;
            swVer = "";
            bDebug = false;
            user = new USER();
            bSimul = false;
            eManual = eMANUAL_WORK.NONE;
            currMdlFile = "";
        }

        public void User_Set(USER id, bool IsLogOut = false)
        {
            if (false == IsLogOut)
            {
                user.id = id.id;
                user.grade = id.grade;
            }
            else
            {
                user.id = string.Empty;
                user.grade = eOPRGRADE.Operator;
            }
        }
        public eOPRGRADE _UserGrade => user.grade;
    }

    public class UDATESTATUS
    {
        public eUPDATESTATUS id;
        public string dev;
        public string msg;

        public UDATESTATUS()
        {
            id = eUPDATESTATUS.NONE;
            dev = msg = string.Empty;
        }
    }

    [Serializable]
    public class SYS
    {
        public SYS_STATUS status;
        public CONFIG cfg;
        public IOINFO io;
        public GOALINFO goals;
        public LAMPINFO lmp;

        public SYS()
        {
            status = new SYS_STATUS();
            cfg = new CONFIG();
            io = new IOINFO();
            io._MaxPort = 16;
            goals = new GOALINFO();
            lmp = new LAMPINFO();
        }
    }

    public class MDL
    {
        public string name;
        public MDL()
        {
            name = string.Empty;
        }
    }

    class _Data
    {
        private static volatile _Data _inst;
        private static object syncRoot = new Object();

        public USERINFO user = null;
        public SYS sys = null;
        public MDL mdl = null;

        private _Data()
        {
            user = new USERINFO();
            sys = new SYS();
            mdl = new MDL();
        }

        ~_Data()
        {

        }

        public static _Data Inst
        {
            get
            {
                if (_inst == null)
                {
                    lock (syncRoot)
                    {
                        if (_inst == null)
                        {
                            _inst = new _Data();
                            // 기본 폴더 자동생성
                            var path = string.Format($"Data", Environment.CurrentDirectory);
                            var dtif = new DirectoryInfo(path);
                            if (!dtif.Exists)
                            {
                                dtif.Create();
                            }
                            Trace.Write($"path --------------------------------");

                            // 설비 데이터 로딩
                            var rtn = _inst.SysLoad();
                        }
                    }
                }
                return _inst;
            }
        }
        public _Data _data = null;

        public bool UserLoad()
        {
            bool rtn = true;
            string path = $"Data\\user.xml";
            try
            {
                if (System.IO.File.Exists(path))
                {
                    System.IO.FileStream fs4 = new System.IO.FileStream(string.Format(path, Environment.CurrentDirectory), System.IO.FileMode.Open, System.IO.FileAccess.Read);
                    XmlSerializer xs2 = new XmlSerializer(_inst.user.GetType());
                    var temp = (USERINFO)xs2.Deserialize(fs4);
                    fs4.Close();
                    _inst.user = temp;
                }
                else
                {
                    rtn = false;
                }
            }
            catch (Exception e)
            {
                rtn = false;
                Debug.Assert(false, $"{e.ToString()}");
            }
            return rtn;
        }

        public bool UserSave()
        {
            // xml 형태로 파일 기록            
            try
            {
                string path = $"Data\\user.xml";
                System.IO.FileStream fs3 = new System.IO.FileStream(path, System.IO.FileMode.Create, System.IO.FileAccess.Write);
                XmlSerializer xs = new XmlSerializer(_inst.user.GetType());
                xs.Serialize(fs3, _inst.user);
                fs3.Close();
            }
            catch (Exception e)
            {
                Debug.Assert(false, $"{e.ToString()}");
                return false;
            }
            return true;
        }

        public bool StatusLoad()
        {
            bool rtn = true;
            string path = $"Data\\Status.xml";
            try
            {
                if (System.IO.File.Exists(path))
                {
                    System.IO.FileStream fs4 = new System.IO.FileStream(string.Format(path, Environment.CurrentDirectory), System.IO.FileMode.Open, System.IO.FileAccess.Read);
                    XmlSerializer xs2 = new XmlSerializer(_inst.sys.status.GetType());
                    var temp = (SYS_STATUS)xs2.Deserialize(fs4);
                    fs4.Close();
                    _inst.sys.status = temp;
                }
                else
                {
                    rtn = false;
                }
            }
            catch (Exception e)
            {
                rtn = false;
                Debug.Assert(false, $"{e.ToString()}");
            }
            return rtn;
        }

        public bool StatusSave()
        {
            // xml 형태로 파일 기록            
            try
            {
                string path = $"Data\\Status.xml";
                System.IO.FileStream fs3 = new System.IO.FileStream(path, System.IO.FileMode.Create, System.IO.FileAccess.Write);
                XmlSerializer xs = new XmlSerializer(_inst.sys.status.GetType());
                xs.Serialize(fs3, _inst.sys.status);
                fs3.Close();
            }
            catch (Exception e)
            {
                Debug.Assert(false, $"{e.ToString()}");
            }
            return true;
        }

        public bool SysLoad()
        {
            bool rtn = false;
            rtn = _inst.StatusLoad();
            if (false == rtn)
            {
                _inst.sys = new SYS();
                _inst.StatusSave();
            }
            rtn = true;
            string path = $"Data\\syscfg.xml";
            try
            {
                if (System.IO.File.Exists(path))
                {
                    System.IO.FileStream fs4 = new System.IO.FileStream(string.Format(path, Environment.CurrentDirectory), System.IO.FileMode.Open, System.IO.FileAccess.Read);
                    XmlSerializer xs2 = new XmlSerializer(_inst.sys.cfg.GetType());
                    var temp = (CONFIG)xs2.Deserialize(fs4);
                    fs4.Close();
                    _inst.sys.cfg = temp;
                }
                else
                {
                    rtn = false;
                }
            }
            catch (Exception e)
            {
                rtn = false;
                Debug.Assert(false, $"{e.ToString()}");
            }

            if (false == rtn)
            {
                SysSave();
            }
            rtn = IOLoad(_inst.sys.cfg.language);
            if (false == rtn)
            {
                IOSave(_inst.sys.cfg.language);
            }

            rtn = LmpLoad();
            if (false == rtn)
            {
                foreach (eEQPSATUS idx in Enum.GetValues(typeof(eEQPSATUS)))
                {
                    var item = new LAMP() { status = idx, Green = TWRLAMP.OFF, Yellow = TWRLAMP.OFF, Red = TWRLAMP.OFF, Buzzer = false };
                    _inst.sys.lmp.lst.Add(item);
                }
                LmpSave();
            }

            rtn = GoalsLoad(_inst.sys.cfg.language);
            if (false == rtn)
            {
                foreach (eGOALTYPE idx in Enum.GetValues(typeof(eGOALTYPE)))
                {
                    var item = new GOALITEM() { type = idx, name = $"TEST_{idx}" };
                    _inst.sys.goals.Add(item);
                }
                GoalsSave(_inst.sys.cfg.language);
            }
            rtn = UserLoad();

            rtn = MotionLoad();

            _inst.sys.status.bLoaded = false;
            _inst.sys.status.bDebug = true;
            _inst.sys.status.user.id = string.Empty;
            _inst.sys.status.user.grade = eOPRGRADE.Operator;
            return rtn;
        }

        public bool SysSave()
        {
            // xml 형태로 파일 기록            
            try
            {
                string path = $"Data\\syscfg.xml";
                System.IO.FileStream fs3 = new System.IO.FileStream(path, System.IO.FileMode.Create, System.IO.FileAccess.Write);
                XmlSerializer xs = new XmlSerializer(_inst.sys.cfg.GetType());
                xs.Serialize(fs3, _inst.sys.cfg);
                fs3.Close();
                UserSave();
                IOSave(_inst.sys.cfg.language);
                GoalsSave(_inst.sys.cfg.language);
                LmpSave();
                MotionSave();
            }
            catch (Exception e)
            {
                Debug.Assert(false, $"{e.ToString()}");
                return false;
            }
            return true;
        }


        public bool IOLoad(eLANGUAGE lan)
        {
            bool rtn = true;

            string path = $"Data\\IOList_{lan.ToString()}.xml";
            try
            {
                if (System.IO.File.Exists(path))
                {
                    System.IO.FileStream fs4 = new System.IO.FileStream(string.Format(path, Environment.CurrentDirectory), System.IO.FileMode.Open, System.IO.FileAccess.Read);
                    XmlSerializer xs2 = new XmlSerializer(_inst.sys.io.GetType());
                    var temp = (IOINFO)xs2.Deserialize(fs4);
                    fs4.Close();
                    _inst.sys.io = temp;
                }
                else
                {
                    rtn = false;
                }
            }
            catch (Exception e)
            {
                Debug.Assert(false, $"{e.ToString()}");
                return false;
            }
            return rtn;
        }

        public bool IOSave(eLANGUAGE lan)
        {
            // xml 형태로 파일 기록            
            try
            {
                string path = $"Data\\IOList_{lan.ToString()}.xml";
                System.IO.FileStream fs3 = new System.IO.FileStream(path, System.IO.FileMode.Create, System.IO.FileAccess.Write);
                XmlSerializer xs = new XmlSerializer(_inst.sys.io.GetType());
                xs.Serialize(fs3, _inst.sys.io);
                fs3.Close();
            }
            catch (Exception e)
            {
                Debug.Assert(false, $"{e.ToString()}");
                return false;
            }
            return true;
        }

        private bool LmpLoad()
        {
            bool rtn = true;
            string path = $"Data\\Lmp.xml";
            try
            {
                if (System.IO.File.Exists(path))
                {
                    System.IO.FileStream fs4 = new System.IO.FileStream(string.Format(path, Environment.CurrentDirectory), System.IO.FileMode.Open, System.IO.FileAccess.Read);
                    XmlSerializer xs2 = new XmlSerializer(_inst.sys.lmp.GetType());
                    var temp = (LAMPINFO)xs2.Deserialize(fs4);
                    fs4.Close();
                    _inst.sys.lmp = temp;
                    if (0 >= _inst.sys.lmp.lst.Count)
                    {
                        rtn = false;
                    }
                }
                else
                {
                    rtn = false;
                }
            }
            catch (Exception e)
            {
                Debug.Assert(false, $"{e.ToString()}");
                return false;
            }
            return rtn;
        }

        private bool LmpSave()
        {
            // xml 형태로 파일 기록            
            try
            {
                string path = $"Data\\Lmp.xml";
                System.IO.FileStream fs3 = new System.IO.FileStream(path, System.IO.FileMode.Create, System.IO.FileAccess.Write);
                XmlSerializer xs = new XmlSerializer(_inst.sys.lmp.GetType());
                xs.Serialize(fs3, _inst.sys.lmp);
                fs3.Close();
            }
            catch (Exception e)
            {
                Debug.Assert(false, $"{e.ToString()}");
                return false;
            }
            return true;
        }

        private bool GoalsLoad(eLANGUAGE lan)
        {
            bool rtn = true;
            string path = $"Data\\GolaList_{lan.ToString()}.xml";
            if (System.IO.File.Exists(path))
            {
                System.IO.FileStream fs4 = new System.IO.FileStream(string.Format(path, Environment.CurrentDirectory), System.IO.FileMode.Open, System.IO.FileAccess.Read);
                try
                {
                    XmlSerializer xs2 = new XmlSerializer(_inst.sys.goals.GetType());
                    var temp = (GOALINFO)xs2.Deserialize(fs4);
                    _inst.sys.goals = temp;
                }
                catch (Exception e)
                {
                    Debug.Assert(false, $"{e.ToString()}");
                }
                fs4.Close();
                if (0 >= _inst.sys.goals.lst.Count)
                {
                    rtn = false;
                }
            }
            else
            {
                rtn = false;
            }
            return rtn;
        }

        private bool GoalsSave(eLANGUAGE lan)
        {
            // xml 형태로 파일 기록            
            try
            {
                string path = $"Data\\GolaList_{lan}.xml";
                System.IO.FileStream fs3 = new System.IO.FileStream(path, System.IO.FileMode.Create, System.IO.FileAccess.Write);
                XmlSerializer xs = new XmlSerializer(_inst.sys.goals.GetType());
                xs.Serialize(fs3, _inst.sys.goals);
                fs3.Close();
            }
            catch (Exception e)
            {
                Debug.Assert(false, $"{e.ToString()}");
                return false;
            }
            return true;
        }

        public bool MdlLoad(string mdlname)
        {
            bool rtn = false;
            string path = string.Format($"Data\\Model\\{mdlname}.xml", Environment.CurrentDirectory);
            try
            {
                if (System.IO.File.Exists(path))
                {
                    System.IO.FileStream fs4 = new System.IO.FileStream(path, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                    XmlSerializer xs2 = new XmlSerializer(_inst.mdl.GetType());
                    var temp = (MDL)xs2.Deserialize(fs4);
                    _inst.mdl = temp;
                    rtn = true;
                    fs4.Close();
                }
                else
                {
                    rtn = false;
                }
            }
            catch (Exception e)
            {
                Debug.Assert(false, $"{e.ToString()}");
                return false;
            }
            if (false == rtn)
            {
                MdlSave($"DefaultMdl");
                _inst.sys.status.currMdlFile = $"DefaultMdl";
                SysSave();
            }
            return rtn;
        }

        public bool MdlSave(string mdlname)
        {
            string path = string.Format($"Data\\Model\\{mdlname}.xml", Environment.CurrentDirectory);
            // xml 형태로 파일 기록
            try
            {
                System.IO.FileStream fs3 = new System.IO.FileStream(path, System.IO.FileMode.Create, System.IO.FileAccess.Write);
                XmlSerializer xs = new XmlSerializer(_inst.mdl.GetType());
                xs.Serialize(fs3, _inst.mdl);
                fs3.Close();
            }
            catch (Exception e)
            {
                Debug.Assert(false, $"{e.ToString()}");
                return false;
            }
            return true;
        }

        public bool MotionLoad()
        {
            bool rtn = true;
            string path = $"Data\\Motion.xml";
            try
            {
                if (System.IO.File.Exists(path))
                {
                    System.IO.FileStream fs4 = new System.IO.FileStream(string.Format(path, Environment.CurrentDirectory), System.IO.FileMode.Open, System.IO.FileAccess.Read);
                    XmlSerializer xs2 = new XmlSerializer(_inst.sys.status.GetType());
                    var temp = (SYS_STATUS)xs2.Deserialize(fs4);
                    fs4.Close();
                    _inst.sys.status = temp;
                }
                else
                {
                    rtn = false;
                }
            }
            catch (Exception e)
            {
                rtn = false;
                Debug.Assert(false, $"{e.ToString()}");
            }
            return rtn;
        }

        public bool MotionSave()
        {
            // xml 형태로 파일 기록            
            try
            {
                string path = $"Data\\Motion.xml";
                System.IO.FileStream fs3 = new System.IO.FileStream(path, System.IO.FileMode.Create, System.IO.FileAccess.Write);
                XmlSerializer xs = new XmlSerializer(_inst.sys.status.GetType());
                xs.Serialize(fs3, _inst.sys.status);
                fs3.Close();
            }
            catch (Exception e)
            {
                Debug.Assert(false, $"{e.ToString()}");
            }
            return true;
        }
    }
}
