using System;
using System.Collections.Generic;
using static MaterialStorageManager.Utils.MsgBox;

namespace MaterialStorageManager.Utils
{
    #region View 관련
    enum eBTN_POPUP
    {
        LogIn,
        LogOut,
        Account,
        ConfigSave,
        Shutdown,
    }
    public class DEF_CONST
    {
        public const int SEQ_FINISH = 0;
        public const int SEQ_INIT = 1;
        public const int SEQ_STEP_FINISH = 1000;
        public const int SEQ_MAIN_FINISH = 10000;
        public const double ZERO = 0.000000000000010;
        public const double PIE = 3.141592768;
        public const double COMM_ERR = -999;
        public const double MOTION_BAND = 0.015;
        public const double ORGMOTION_BAND = 0.015;
        public const int SENTIME = 200;
        public const int MOTOR_REV = 10000;
        public const double BASE_ANG_REV = 1.6145;
    }

    public enum eLANGUAGE
    {
        kor,
        eng,
        chn
    }

    public enum eEQPSATUS : int
    {
        Init,          // Need 2 SystemOn        
        Stop,
        Stopping,
        Idle,
        Run,
        Error,
        EMG
    }

    public enum eOPRGRADE : int
    {
        Operator,
        Maintenance,
        Maker,
        Master
    }

    public enum eDEV : int
    {
        MPlus,
        IO,
        LD
    }

    public enum ePAGE : int
    {
        DashBoard,
        System
    }

    public enum eVIWER : int
    {
        None,
        Monitor,
        Manual,
        IO,
        TowerLamp,
        Goal,
        PIO,
        Option
    }

    public enum CHANGEMODEBY
    {
        PROCESS,
        EMG,
        SWITCH,
        UIBUTTON,
        MPLUS,
        MANUAL
    }

    public enum eCOMSTATUS
    {
        DISCONNECTED,
        CONNECTED
    }

    public enum eIOTYPE
    {
        INPUT,
        OUTPUT
    }

    public enum eSENTYPE
    {
        A,
        B
    }

    public enum TWRLAMP : int
    {
        OFF,
        ON,
        BLINK
    }

    public enum eDATAEXCHANGE
    {
        Load,
        Save,
        StatusUpdate,
        Data2UI,
        UI2Data
    }

    public enum eDATATYPE
    {
        NONE,
        _bool,
        _int,
        _str,
        _float,
        _double
    }

    public enum eUPDATESTATUS
    {
        NONE,
        CHANGED_EQP,
        CHANGED_PARAM
    }

    public enum eSTATE : int
    {
        None,
        WorkTrg,
        Working,
        Checking,
        Checked,
        Failed,
        Stopping,
        Stopped,
        Error,
        Resume,
        Done,
        Reset
    }

    public enum eSEQLIST : int
    {
        Main,
        MAX_SEQ
    }

    public enum eSUBSEQLIST
    {
        SWITCH,
        MAX_SUB_SEQ
    }

    public enum eMANUAL_WORK : int
    {
        NONE,
        CHARGE
    }

    public enum eINPUT
    {

    }

    public enum eOUTPUT
    {

    }

    public enum eERROR
    {
        None
    }

    public enum ePortName
    {
        COM1,
        COM2,
        COM3,
        COM4,
        COM5,
        COM6,
        COM7,
        COM8,
        COM9,
        COM10,
        COM11
    }

    public enum eGOALTYPE
    {
        Pickup,
        Dropoff,
        Charge,
        Standby
    }

    public enum eLINE
    {
        None,
        _23,
        _24,
        _25,
        Sharing,
    }
    #endregion

    #region API 통신
    public enum ServerRespType
    {
        None,
        Acknowledge,
        NotAcknowledge,
        ServiceIsNotAvailable,
        NotAccessible,
        ProcessDelay,
        FailedToExecute,
    }

    public class ServerResp
    {
        public DateTime Time { get; set; }
        public ServerRespType State { get; set; }
        public string Msg { get; set; }

        public JobDetail Job { get; set; }
    }

    public class JobDetail
    {
        public string Id { get; set; }
        public string Uuid { get; set; }
        public string Oper { get; set; }
    }

    public enum StrgMode
    {
        None,
        Manual,
        Auto,
    }

    public enum StrgState
    {
        None,
        Idle,
        Providing,
        Receiving,
        Error,
    }

    public enum StrgAccState
    {
        None,
        Idle,
        ReadyProv,
        ReadyRecv,
        Error,
    }

    public class StateReport
    {
        public string Id { get; set; }
        public DateTime Time { get; set; }
        public StrgMode Mode { get; set; }
        public StrgState State { get; set; }
        public StrgAccState AccessState { get; set; }
        public string JobId { get; set; }
        public string Msg { get; set; }
    }

    public enum JobState
    {
        None,
        Queued,
        Assigned,
        PreWork,
        Work,
        PostWork,
        Complete,
        Error,
    }

    public class JobReport
    {
        public string Id { get; set; }
        public DateTime Time { get; set; }
        public string JobId { get; set; }
        public JobState JobState { get; set; }
    }

    public class MtrMovedReport
    {
        public string Id { get; set; }
        public DateTime Time { get; set; }
        public string Uuid { get; set; }
        public bool IsProviding { get; set; }
        public string OperId { get; set; }
        public MrtDetail Material { get; set; }
    }

    public class MrtDetail
    {
        public string Name { get; set; }
        public string LotId { get; set; }
        public DateTime Expiry { get; set; }
        public int Qty { get; set; }
        public string TypeName { get; set; }
    }

    public class AlarmReport
    {
        public string Id { get; set; }
        public DateTime Time { get; set; }
        public AlarmCode AlarmCode { get; set; }
        public bool IsAlarmSet { get; set; }
    }

    public class QueryOperator
    {
        public string Id { get; set; }
        public DateTime Time { get; set; }
        public string OperId { get; set; }
    }

    public enum AlarmCode
    {
        None,
        AbortInitialize,
        FailedInitialize,
        MotorCommunicationFailure,
        MobotMoveToHomeTimeout,
        FailedToLoadProgram,
        FailedToPlayProgram,
        DecodeBarcodeFailure,
        DecodeBarcodeIsNotValid,
        DetectedAMaterialOnGripper,
        MotorStateIsNotMatched,
        MaterialTowerIsFull,
        MotorEmergencyStop,
        MotorProtectiveStop,
        MaterialNotRecognized,
        BarcodeScanerNotDetected,
        HumidityLoggerNotResponding,
        SensorErrorEncoder,
        ErrorClampingOnClosing,
        ErrorClampingOnOpening,
        ErrorMotorBlocked,
        MotionBoardNotRecogninzed,
        MaterialDetectedInputLocation,
    }
    #endregion

    #region Model <-> ViewModel 연동
    public class CONFIGGROUP
    {
        public string URL = string.Empty;
        public string TowerID = string.Empty;
    }

    public class MMSTATUSGROUP
    {
        public string MMstate = string.Empty;
    }

    public class STATUSGROUP
    {
        public StrgMode StrgMode = StrgMode.None;
        public StrgState StrgState = StrgState.None;
        public StrgAccState StrgAccState = StrgAccState.None;
    }

    public class JOBSTATUSGROUP
    {
        public JobState JobState = JobState.None;
    }

    public class OPERATORGROUP
    {
        public string OperatorNum = string.Empty;
    }

    public class ALARMGROUP
    {
        public AlarmCode AlarmCode = AlarmCode.None;
        public bool AlarmState = true;
    }

    public class MATERIALGROUP
    {
        public bool Provide = true;
    }

    public class LOGGROUP
    {
        public string LogType = string.Empty;
        public string LogText = string.Empty;
    }

    public class LOGITEMLIST
    {
        public string LogType { get; set; }
        public string Log { get; set; }
    }

    public class APIRECVDATA
    {
        public string JobID = string.Empty;
        public string UuidID = string.Empty;
        public string OperName = string.Empty;
        public bool ViewEnable = false;
    }

    public class PROVIDINGDATA
    {
        public string ProvOperName = "1";
        public string ProvUuid = "2";
        public string ProvMTRName = "3";
        public string ProvLotID = "4";
        public string ProvExpiry = "5";
        public string ProvQTY = "6";
        public string ProvType = "7";

        //public string ProvOperName = string.Empty;
        //public string ProvUuid = string.Empty;
        //public string ProvMTRName = string.Empty;
        //public string ProvLotID = string.Empty;
        //public string ProvExpiry = string.Empty;
        //public string ProvQTY = string.Empty;
        //public string ProvType = string.Empty;
    }
    public class RECVINGDATA
    {
        public string RecvOperName = "8";
        public string RecvUuid = "9";
        public string RecvMTRName = "10";
        public string RecvLotID = "11";
        public string RecvExpiry = "12";
        public string RecvQTY = "13";
        public string RecvType = "14";

        //public string RecvOperName = string.Empty;
        //public string RecvUuid = string.Empty;
        //public string RecvMTRName = string.Empty;
        //public string RecvLotID = string.Empty;
        //public string RecvExpiry = string.Empty;
        //public string RecvQTY = string.Empty;
        //public string RecvType = string.Empty;

    }

    public class VIEWIOLIST
    {
        public List<IOSRC> ioSRC = new List<IOSRC>();
    }

    public class MSGBOXDATA
    {
        public string message = string.Empty;
        public MsgType msgType = MsgType.Info;
        public eBTNSTYLE btnStyle = eBTNSTYLE.OK;
    }
    #endregion
}
