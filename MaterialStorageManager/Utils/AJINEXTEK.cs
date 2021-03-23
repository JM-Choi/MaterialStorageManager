using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MaterialStorageManager.Utils
{
    #region AJINEXTEK DIO Board
    public class AJINDIO
    {
        //========== 보드 및 모듈 정보 =================================================================================

        // DIO 모듈이 있는지 확인
        [DllImport("AXL.dll")] public static extern uint AxdInfoIsDIOModule(ref uint upStatus);

        // DIO 모듈 No 확인
        [DllImport("AXL.dll")] public static extern uint AxdInfoGetModuleNo(int lBoardNo, int lModulePos, ref int lpModuleNo);

        // DIO 입출력 모듈의 개수 확인
        [DllImport("AXL.dll")] public static extern uint AxdInfoGetModuleCount(ref int lpModuleCount);

        // 지정한 모듈의 입력 접점 개수 확인
        [DllImport("AXL.dll")] public static extern uint AxdInfoGetInputCount(int lModuleNo, ref int lpCount);

        // 지정한 모듈의 출력 접점 개수 확인
        [DllImport("AXL.dll")] public static extern uint AxdInfoGetOutputCount(int lModuleNo, ref int lpCount);

        // 지정한 모듈 번호로 베이스 보드 번호, 모듈 위치, 모듈 ID 확인
        [DllImport("AXL.dll")] public static extern uint AxdInfoGetModule(int lModuleNo, ref int lpBoardNo, ref int lpModulePos, ref uint upModuleID);

        // 해당 모듈이 제어가 가능한 상태인지 반환한다.
        [DllImport("AXL.dll")] public static extern uint AxdInfoGetModuleStatus(int lModuleNo);

        //========== 인터럽트 설정 확인 =================================================================================

        // 지정한 모듈에 인터럽트 메시지를 받아오기 위하여 윈도우 메시지, 콜백 함수 또는 이벤트 방식을 사용
        //========= 인터럽트 관련 함수 ======================================================================================
        // 콜백 함수 방식은 이벤트 발생 시점에 즉시 콜백 함수가 호출 됨으로 가장 빠르게 이벤트를 통지받을 수 있는 장점이 있으나
        // 콜백 함수가 완전히 종료 될 때까지 메인 프로세스가 정체되어 있게 된다.
        // 즉, 콜백 함수 내에 부하가 걸리는 작업이 있을 경우에는 사용에 주의를 요한다. 
        // 이벤트 방식은 쓰레드등을 이용하여 인터럽트 발생여부를 지속적으로 감시하고 있다가 인터럽트가 발생하면 
        // 처리해주는 방법으로, 쓰레드 등으로 인해 시스템 자원을 점유하고 있는 단점이 있지만
        // 가장 빠르게 인터럽트를 검출하고 처리해줄 수 있는 장점이 있다.
        // 일반적으로는 많이 쓰이지 않지만, 인터럽트의 빠른처리가 주요 관심사인 경우에 사용된다. 
        // 이벤트 방식은 이벤트의 발생 여부를 감시하는 특정 쓰레드를 사용하여 메인 프로세스와 별개로 동작되므로
        // MultiProcessor 시스템등에서 자원을 가장 효율적으로 사용할 수 있게 되어 특히 권장하는 방식이다.
        // 인터럽트 메시지를 받아오기 위하여 윈도우 메시지 또는 콜백 함수를 사용한다.
        // (메시지 핸들, 메시지 ID, 콜백함수, 인터럽트 이벤트)
        //    hWnd            : 윈도우 핸들, 윈도우 메세지를 받을때 사용. 사용하지 않으면 NULL을 입력.
        //    uMessage        : 윈도우 핸들의 메세지, 사용하지 않거나 디폴트값을 사용하려면 0을 입력.
        //    proc            : 인터럽트 발생시 호출될 함수의 포인터, 사용하지 않으면 NULL을 입력.
        //    pEvent          : 이벤트 방법사용시 이벤트 핸들
        [DllImport("AXL.dll")] public static extern uint AxdiInterruptSetModule(int lModuleNo, IntPtr hWnd, uint uMessage, CAXHS.AXT_INTERRUPT_PROC pProc, ref uint pEvent);

        // 지정한 모듈의 인터럽트 사용 유무 설정
        //======================================================//
        // uUse        : DISABLE(0)    // 인터럽트 해제
        //             : ENABLE(1)     // 인터럽트 설정
        //======================================================//
        [DllImport("AXL.dll")] public static extern uint AxdiInterruptSetModuleEnable(int lModuleNo, uint uUse);

        // 지정한 모듈의 인터럽트 사용 유무 확인
        //======================================================//
        // *upUse      : DISABLE(0)    // 인터럽트 해제
        //             : ENABLE(1)     // 인터럽트 설정
        //======================================================//
        [DllImport("AXL.dll")] public static extern uint AxdiInterruptGetModuleEnable(int lModuleNo, ref uint upUse);

        // 인터럽트 발생 위치 확인
        [DllImport("AXL.dll")] public static extern uint AxdiInterruptRead(ref int lpModuleNo, ref uint upFlag);

        //========== 인터럽트 상승 / 하강 에지 설정 확인 =================================================================================
        // 지정한 입력 접점 모듈, Interrupt Rising / Falling Edge register의 Offset 위치에서 bit 단위로 상승 또는 하강 에지 값을 설정
        //===============================================================================================//
        // lModuleNo    : 모듈 번호
        // lOffset      : 입력 접점에 대한 Offset 위치
        // uMode        : DOWN_EDGE(0)
        //              : UP_EDGE(1)
        // uValue       : DISABLE(0)
        //              : ENABLE(1)
        //===============================================================================================//
        [DllImport("AXL.dll")] public static extern uint AxdiInterruptEdgeSetBit(int lModuleNo, int lOffset, uint uMode, uint uValue);

        // 지정한 입력 접점 모듈, Interrupt Rising / Falling Edge register의 Offset 위치에서 byte 단위로 상승 또는 하강 에지 값을 설정
        //===============================================================================================//
        // lModuleNo    : 모듈 번호
        // lOffset      : 입력 접점에 대한 Offset 위치
        // uMode        : DOWN_EDGE(0)
        //              : UP_EDGE(1)
        // uValue       : 0x00 ~ 0x0FF ('1'로 Setting 된 부분 인터럽트 설정)
        //===============================================================================================//
        [DllImport("AXL.dll")] public static extern uint AxdiInterruptEdgeSetByte(int lModuleNo, int lOffset, uint uMode, uint uValue);

        // 지정한 입력 접점 모듈, Interrupt Rising / Falling Edge register의 Offset 위치에서 word 단위로 상승 또는 하강 에지 값을 설정
        //===============================================================================================//
        // lModuleNo    : 모듈 번호
        // lOffset      : 입력 접점에 대한 Offset 위치
        // uMode        : DOWN_EDGE(0)
        //              : UP_EDGE(1)
        // uValue       : 0x00 ~ 0x0FFFF ('1'로 Setting 된 부분 인터럽트 설정)
        //===============================================================================================//
        [DllImport("AXL.dll")] public static extern uint AxdiInterruptEdgeSetWord(int lModuleNo, int lOffset, uint uMode, uint uValue);

        // 지정한 입력 접점 모듈, Interrupt Rising / Falling Edge register의 Offset 위치에서 double word 단위로 상승 또는 하강 에지 값을 설정
        //===============================================================================================//
        // lModuleNo    : 모듈 번호
        // lOffset      : 입력 접점에 대한 Offset 위치
        // uMode        : DOWN_EDGE(0)
        //              : UP_EDGE(1)
        // uValue       : 0x00 ~ 0x0FFFFFFFF ('1'로 Setting 된 부분 인터럽트 설정)
        //===============================================================================================//
        [DllImport("AXL.dll")] public static extern uint AxdiInterruptEdgeSetDword(int lModuleNo, int lOffset, uint uMode, uint uValue);

        // 지정한 입력 접점 모듈, Interrupt Rising / Falling Edge register의 Offset 위치에서 bit 단위로 상승 또는 하강 에지 값을 확인
        //===============================================================================================//
        // lModuleNo    : 모듈 번호
        // lOffset      : 입력 접점에 대한 Offset 위치
        // uMode        : DOWN_EDGE(0)
        //              : UP_EDGE(1)
        // *upValue     : 0x00 ~ 0x0FF ('1'로 Setting 된 부분 인터럽트 설정)
        //===============================================================================================//
        [DllImport("AXL.dll")] public static extern uint AxdiInterruptEdgeGetBit(int lModuleNo, int lOffset, uint uMode, ref uint upValue);

        // 지정한 입력 접점 모듈, Interrupt Rising / Falling Edge register의 Offset 위치에서 byte 단위로 상승 또는 하강 에지 값을 확인
        //===============================================================================================//
        // lModuleNo    : 모듈 번호
        // lOffset      : 입력 접점에 대한 Offset 위치
        // uMode        : DOWN_EDGE(0)
        //              : UP_EDGE(1)
        // *upValue     : 0x00 ~ 0x0FF ('1'로 Setting 된 부분 인터럽트 설정)
        //===============================================================================================//
        [DllImport("AXL.dll")] public static extern uint AxdiInterruptEdgeGetByte(int lModuleNo, int lOffset, uint uMode, ref uint upValue);

        // 지정한 입력 접점 모듈, Interrupt Rising / Falling Edge register의 Offset 위치에서 word 단위로 상승 또는 하강 에지 값을 확인
        //===============================================================================================//
        // lModuleNo    : 모듈 번호
        // lOffset      : 입력 접점에 대한 Offset 위치
        // uMode        : DOWN_EDGE(0)
        //              : UP_EDGE(1)
        // *upValue     : 0x00 ~ 0x0FFFFFFFF ('1'로 Setting 된 부분 인터럽트 설정)
        //===============================================================================================//
        [DllImport("AXL.dll")] public static extern uint AxdiInterruptEdgeGetWord(int lModuleNo, int lOffset, uint uMode, ref uint upValue);

        // 지정한 입력 접점 모듈, Interrupt Rising / Falling Edge register의 Offset 위치에서 double word 단위로 상승 또는 하강 에지 값을 확인
        //===============================================================================================//
        // lModuleNo    : 모듈 번호
        // lOffset      : 입력 접점에 대한 Offset 위치
        // uMode        : DOWN_EDGE(0)
        //              : UP_EDGE(1)
        // *upValue     : 0x00 ~ 0x0FFFFFFFF ('1'로 Setting 된 부분 인터럽트 설정)
        //===============================================================================================//
        [DllImport("AXL.dll")] public static extern uint AxdiInterruptEdgeGetDword(int lModuleNo, int lOffset, uint uMode, ref uint upValue);

        // 전체 입력 접점 모듈, Interrupt Rising / Falling Edge register의 Offset 위치에서 bit 단위로 상승 또는 하강 에지 값을 설정
        //===============================================================================================//
        // lOffset      : 입력 접점에 대한 Offset 위치
        // uMode        : DOWN_EDGE(0)
        //              : UP_EDGE(1)
        // uValue       : DISABLE(0)
        //              : ENABLE(1)
        //===============================================================================================//
        [DllImport("AXL.dll")] public static extern uint AxdiInterruptEdgeSet(int lOffset, uint uMode, uint uValue);

        // 전체 입력 접점 모듈, Interrupt Rising / Falling Edge register의 Offset 위정에서 bit 단위로 상승 또는 하강 에지 값을 확인
        //===============================================================================================//
        // lOffset      : 입력 접점에 대한 Offset 위치
        // uMode        : DOWN_EDGE(0)
        //              : UP_EDGE(1)
        // *upValue     : DISABLE(0)
        //              : ENABLE(1)
        //===============================================================================================//
        [DllImport("AXL.dll")] public static extern uint AxdiInterruptEdgeGet(int lOffset, uint uMode, ref uint upValue);

        //========== 입출력 레벨 설정 확인 =================================================================================
        //==입력 레벨 설정 확인
        // 지정한 입력 접점 모듈의 Offset 위치에서 bit 단위로 데이터 레벨을 설정
        //===============================================================================================//
        // lModuleNo    : 모듈 번호
        // lOffset      : 입력 접점에 대한 Offset 위치
        // uLevel       : LOW(0)
        //              : HIGH(1)
        //===============================================================================================//
        [DllImport("AXL.dll")] public static extern uint AxdiLevelSetInportBit(int lModuleNo, int lOffset, uint uLevel);

        // 지정한 입력 접점 모듈의 Offset 위치에서 byte 단위로 데이터 레벨을 설정
        //===============================================================================================//
        // lModuleNo    : 모듈 번호
        // lOffset      : 입력 접점에 대한 Offset 위치
        // uLevel       : 0x00 ~ 0x0FF('1'로 설정 된 비트는 HIGH, '0'으로 설정 된 비트는 LOW)
        //===============================================================================================//
        [DllImport("AXL.dll")] public static extern uint AxdiLevelSetInportByte(int lModuleNo, int lOffset, uint uLevel);

        // 지정한 입력 접점 모듈의 Offset 위치에서 word 단위로 데이터 레벨을 설정
        //===============================================================================================//
        // lModuleNo    : 모듈 번호
        // lOffset      : 입력 접점에 대한 Offset 위치
        // uLevel       : 0x00 ~ 0x0FFFF('1'로 설정 된 비트는 HIGH, '0'으로 설정 된 비트는 LOW)
        //===============================================================================================//
        [DllImport("AXL.dll")] public static extern uint AxdiLevelSetInportWord(int lModuleNo, int lOffset, uint uLevel);

        // 지정한 입력 접점 모듈의 Offset 위치에서 double word 단위로 데이터 레벨을 설정
        //===============================================================================================//
        // lModuleNo    : 모듈 번호
        // lOffset      : 입력 접점에 대한 Offset 위치
        // uLevel       : 0x00 ~ 0x0FFFFFFFF('1'로 설정 된 비트는 HIGH, '0'으로 설정 된 비트는 LOW)
        //===============================================================================================//
        [DllImport("AXL.dll")] public static extern uint AxdiLevelSetInportDword(int lModuleNo, int lOffset, uint uLevel);

        // 지정한 입력 접점 모듈의 Offset 위치에서 bit 단위로 데이터 레벨을 확인
        //===============================================================================================//
        // lModuleNo    : 모듈 번호
        // lOffset      : 입력 접점에 대한 Offset 위치
        // *upLevel     : LOW(0)
        //              : HIGH(1)
        //===============================================================================================//
        [DllImport("AXL.dll")] public static extern uint AxdiLevelGetInportBit(int lModuleNo, int lOffset, ref uint upLevel);

        // 지정한 입력 접점 모듈의 Offset 위치에서 byte 단위로 데이터 레벨을 확인
        //===============================================================================================//
        // lModuleNo    : 모듈 번호
        // lOffset      : 입력 접점에 대한 Offset 위치
        // *upLevel     : 0x00 ~ 0x0FF('1'로 읽힌 비트는 HIGH, '0'으로 읽힌 비트는 LOW)
        //===============================================================================================//
        [DllImport("AXL.dll")] public static extern uint AxdiLevelGetInportByte(int lModuleNo, int lOffset, ref uint upLevel);

        // 지정한 입력 접점 모듈의 Offset 위치에서 word 단위로 데이터 레벨을 확인
        //===============================================================================================//
        // lModuleNo    : 모듈 번호
        // lOffset      : 입력 접점에 대한 Offset 위치
        // *upLevel     : 0x00 ~ 0x0FFFF('1'로 읽힌 비트는 HIGH, '0'으로 읽힌 비트는 LOW)
        //===============================================================================================//
        [DllImport("AXL.dll")] public static extern uint AxdiLevelGetInportWord(int lModuleNo, int lOffset, ref uint upLevel);

        // 지정한 입력 접점 모듈의 Offset 위치에서 double word 단위로 데이터 레벨을 확인
        //===============================================================================================//
        // lModuleNo    : 모듈 번호
        // lOffset      : 입력 접점에 대한 Offset 위치
        // *upLevel     : 0x00 ~ 0x0FFFFFFFF('1'로 읽힌 비트는 HIGH, '0'으로 읽힌 비트는 LOW)
        //===============================================================================================//
        [DllImport("AXL.dll")] public static extern uint AxdiLevelGetInportDword(int lModuleNo, int lOffset, ref uint upLevel);

        // 전체 입력 접점 모듈의 Offset 위치에서 bit 단위로 데이터 레벨을 설정
        //===============================================================================================//
        // lOffset      : 입력 접점에 대한 Offset 위치
        // uLevel       : LOW(0)
        //              : HIGH(1)
        //===============================================================================================//
        [DllImport("AXL.dll")] public static extern uint AxdiLevelSetInport(int lOffset, uint uLevel);

        // 전체 입력 접점 모듈의 Offset 위치에서 bit 단위로 데이터 레벨을 확인
        //===============================================================================================//
        // lModuleNo    : 모듈 번호
        // lOffset      : 입력 접점에 대한 Offset 위치
        // *upLevel     : LOW(0)
        //              : HIGH(1)
        //===============================================================================================//
        [DllImport("AXL.dll")] public static extern uint AxdiLevelGetInport(int lOffset, ref uint upLevel);

        //==출력 레벨 설정 확인
        // 지정한 출력 접점 모듈의 Offset 위치에서 bit 단위로 데이터 레벨을 설정
        //===============================================================================================//
        // lModuleNo    : 모듈 번호
        // lOffset      : 출력 접점에 대한 Offset 위치
        // uLevel       : LOW(0)
        //              : HIGH(1)
        //===============================================================================================//
        [DllImport("AXL.dll")] public static extern uint AxdoLevelSetOutportBit(int lModuleNo, int lOffset, uint uLevel);

        // 지정한 출력 접점 모듈의 Offset 위치에서 byte 단위로 데이터 레벨을 설정
        //===============================================================================================//
        // lModuleNo    : 모듈 번호
        // lOffset      : 출력 접점에 대한 Offset 위치
        // uLevel       : 0x00 ~ 0x0FF('1'로 설정 된 비트는 HIGH, '0'으로 설정 된 비트는 LOW)
        //===============================================================================================//
        [DllImport("AXL.dll")] public static extern uint AxdoLevelSetOutportByte(int lModuleNo, int lOffset, uint uLevel);

        // 지정한 출력 접점 모듈의 Offset 위치에서 word 단위로 데이터 레벨을 설정
        //===============================================================================================//
        // lModuleNo    : 모듈 번호
        // lOffset      : 출력 접점에 대한 Offset 위치
        // uLevel       : 0x00 ~ 0x0FFFF('1'로 설정 된 비트는 HIGH, '0'으로 설정 된 비트는 LOW)
        //===============================================================================================//
        [DllImport("AXL.dll")] public static extern uint AxdoLevelSetOutportWord(int lModuleNo, int lOffset, uint uLevel);

        // 지정한 출력 접점 모듈의 Offset 위치에서 double word 단위로 데이터 레벨을 설정
        //===============================================================================================//
        // lModuleNo    : 모듈 번호
        // lOffset      : 출력 접점에 대한 Offset 위치
        // uLevel       : 0x00 ~ 0x0FFFFFFFF('1'로 설정 된 비트는 HIGH, '0'으로 설정 된 비트는 LOW)
        //===============================================================================================//
        [DllImport("AXL.dll")] public static extern uint AxdoLevelSetOutportDword(int lModuleNo, int lOffset, uint uLevel);

        // 지정한 출력 접점 모듈의 Offset 위치에서 bit 단위로 데이터 레벨을 확인
        //===============================================================================================//
        // lModuleNo    : 모듈 번호
        // lOffset      : 출력 접점에 대한 Offset 위치
        // *upLevel     : LOW(0)
        //              : HIGH(1)
        //===============================================================================================//
        [DllImport("AXL.dll")] public static extern uint AxdoLevelGetOutportBit(int lModuleNo, int lOffset, ref uint upLevel);

        // 지정한 출력 접점 모듈의 Offset 위치에서 byte 단위로 데이터 레벨을 확인
        //===============================================================================================//
        // lModuleNo    : 모듈 번호
        // lOffset      : 출력 접점에 대한 Offset 위치
        // uLevel       : 0x00 ~ 0x0FF('1'로 읽힌 비트는 HIGH, '0'으로 읽힌 비트는 LOW)
        //===============================================================================================//
        [DllImport("AXL.dll")] public static extern uint AxdoLevelGetOutportByte(int lModuleNo, int lOffset, ref uint upLevel);

        // 지정한 출력 접점 모듈의 Offset 위치에서 word 단위로 데이터 레벨을 확인
        //===============================================================================================//
        // lModuleNo    : 모듈 번호
        // lOffset      : 출력 접점에 대한 Offset 위치
        // uLevel       : 0x00 ~ 0x0FFFF('1'로 읽힌 비트는 HIGH, '0'으로 읽힌 비트는 LOW)
        //===============================================================================================//
        [DllImport("AXL.dll")] public static extern uint AxdoLevelGetOutportWord(int lModuleNo, int lOffset, ref uint upLevel);

        // 지정한 출력 접점 모듈의 Offset 위치에서 double word 단위로 데이터 레벨을 확인
        //===============================================================================================//
        // lModuleNo    : 모듈 번호
        // lOffset      : 출력 접점에 대한 Offset 위치
        // uLevel       : 0x00 ~ 0x0FFFFFFFF('1'로 읽힌 비트는 HIGH, '0'으로 읽힌 비트는 LOW)
        //===============================================================================================//
        [DllImport("AXL.dll")] public static extern uint AxdoLevelGetOutportDword(int lModuleNo, int lOffset, ref uint upLevel);

        // 전체 출력 접점 모듈의 Offset 위치에서 bit 단위로 데이터 레벨을 설정
        //===============================================================================================//
        // lOffset      : 출력 접점에 대한 Offset 위치
        // uLevel       : LOW(0)
        //              : HIGH(1)
        //===============================================================================================//
        [DllImport("AXL.dll")] public static extern uint AxdoLevelSetOutport(int lOffset, uint uLevel);

        // 전체 출력 접점 모듈의 Offset 위치에서 bit 단위로 데이터 레벨을 확인
        //===============================================================================================//
        // lOffset      : 출력 접점에 대한 Offset 위치
        // *upLevel     : LOW(0)
        //              : HIGH(1)
        //===============================================================================================//
        [DllImport("AXL.dll")] public static extern uint AxdoLevelGetOutport(int lOffset, ref uint upLevel);

        //========== 입출력 포트 쓰기 읽기 =================================================================================
        //==출력 포트 쓰기
        // 전체 출력 접점 모듈의 Offset 위치에서 bit 단위로 데이터를 출력
        //===============================================================================================//
        // lOffset      : 출력 접점에 대한 Offset 위치
        // uLevel       : LOW(0)
        //              : HIGH(1)
        //===============================================================================================//
        [DllImport("AXL.dll")] public static extern uint AxdoWriteOutport(int lOffset, uint uValue);

        // 지정한 출력 접점 모듈의 Offset 위치에서 bit 단위로 데이터를 출력
        //===============================================================================================//
        // lModuleNo    : 모듈 번호
        // lOffset      : 출력 접점에 대한 Offset 위치
        // uLevel       : LOW(0)
        //              : HIGH(1)
        //===============================================================================================//
        [DllImport("AXL.dll")] public static extern uint AxdoWriteOutportBit(int lModuleNo, int lOffset, uint uValue);

        // 지정한 출력 접점 모듈의 Offset 위치에서 byte 단위로 데이터를 출력
        //===============================================================================================//
        // lModuleNo    : 모듈 번호
        // lOffset      : 출력 접점에 대한 Offset 위치
        // uValue       : 0x00 ~ 0x0FF('1'로 설정 된 비트는 HIGH, '0'으로 설정 된 비트는 LOW)
        //===============================================================================================//
        [DllImport("AXL.dll")] public static extern uint AxdoWriteOutportByte(int lModuleNo, int lOffset, uint uValue);

        // 지정한 출력 접점 모듈의 Offset 위치에서 word 단위로 데이터를 출력
        //===============================================================================================//
        // lModuleNo    : 모듈 번호
        // lOffset      : 출력 접점에 대한 Offset 위치
        // uValue       : 0x00 ~ 0x0FFFF('1'로 설정 된 비트는 HIGH, '0'으로 설정 된 비트는 LOW)
        //===============================================================================================//
        [DllImport("AXL.dll")] public static extern uint AxdoWriteOutportWord(int lModuleNo, int lOffset, uint uValue);

        // 지정한 출력 접점 모듈의 Offset 위치에서 double word 단위로 데이터를 출력
        //===============================================================================================//
        // lModuleNo    : 모듈 번호
        // lOffset      : 출력 접점에 대한 Offset 위치
        // uValue       : 0x00 ~ 0x0FFFFFFFF('1'로 설정 된 비트는 HIGH, '0'으로 설정 된 비트는 LOW)
        //===============================================================================================//
        [DllImport("AXL.dll")] public static extern uint AxdoWriteOutportDword(int lModuleNo, int lOffset, uint uValue);

        //==출력 포트 읽기    
        // 전체 출력 접점 모듈의 Offset 위치에서 bit 단위로 데이터를 읽기
        //===============================================================================================//
        // lOffset      : 출력 접점에 대한 Offset 위치
        // *upLevel     : LOW(0)
        //              : HIGH(1)
        //===============================================================================================//
        [DllImport("AXL.dll")] public static extern uint AxdoReadOutport(int lOffset, ref uint upValue);

        // 지정한 출력 접점 모듈의 Offset 위치에서 bit 단위로 데이터를 읽기
        //===============================================================================================//
        // lModuleNo    : 모듈 번호
        // lOffset      : 출력 접점에 대한 Offset 위치
        // *upLevel     : LOW(0)
        //              : HIGH(1)
        //===============================================================================================//
        [DllImport("AXL.dll")] public static extern uint AxdoReadOutportBit(int lModuleNo, int lOffset, ref uint upValue);

        // 지정한 출력 접점 모듈의 Offset 위치에서 byte 단위로 데이터를 읽기
        //===============================================================================================//
        // lModuleNo    : 모듈 번호
        // lOffset      : 출력 접점에 대한 Offset 위치
        // *upValue     : 0x00 ~ 0x0FF('1'로 읽힌 비트는 HIGH, '0'으로 읽힌 비트는 LOW)
        //===============================================================================================//
        [DllImport("AXL.dll")] public static extern uint AxdoReadOutportByte(int lModuleNo, int lOffset, ref uint upValue);

        // 지정한 출력 접점 모듈의 Offset 위치에서 word 단위로 데이터를 읽기
        //===============================================================================================//
        // lModuleNo    : 모듈 번호
        // lOffset      : 출력 접점에 대한 Offset 위치
        // *upValue     : 0x00 ~ 0x0FFFF('1'로 읽힌 비트는 HIGH, '0'으로 읽힌 비트는 LOW)
        //===============================================================================================//
        [DllImport("AXL.dll")] public static extern uint AxdoReadOutportWord(int lModuleNo, int lOffset, ref uint upValue);

        // 지정한 출력 접점 모듈의 Offset 위치에서 double word 단위로 데이터를 읽기
        //===============================================================================================//
        // lModuleNo    : 모듈 번호
        // lOffset      : 출력 접점에 대한 Offset 위치
        // *upValue     : 0x00 ~ 0x0FFFFFFFF('1'로 읽힌 비트는 HIGH, '0'으로 읽힌 비트는 LOW)
        //===============================================================================================//
        [DllImport("AXL.dll")] public static extern uint AxdoReadOutportDword(int lModuleNo, int lOffset, ref uint upValue);

        //==입력 포트 일기    
        // 전체 입력 접점 모듈의 Offset 위치에서 bit 단위로 데이터를 읽기
        //===============================================================================================//
        // lOffset      : 입력 접점에 대한 Offset 위치
        // *upValue     : LOW(0)
        //              : HIGH(1)
        //===============================================================================================//
        [DllImport("AXL.dll")] public static extern uint AxdiReadInport(int lOffset, ref uint upValue);

        // 지정한 입력 접점 모듈의 Offset 위치에서 bit 단위로 데이터를 읽기
        //===============================================================================================//
        // lModuleNo    : 모듈 번호
        // lOffset      : 입력 접점에 대한 Offset 위치
        // *upValue     : LOW(0)
        //              : HIGH(1)
        //===============================================================================================//
        [DllImport("AXL.dll")] public static extern uint AxdiReadInportBit(int lModuleNo, int lOffset, ref uint upValue);

        // 지정한 입력 접점 모듈의 Offset 위치에서 byte 단위로 데이터를 읽기
        //===============================================================================================//
        // lModuleNo    : 모듈 번호
        // lOffset      : 입력 접점에 대한 Offset 위치
        // *upValue     : 0x00 ~ 0x0FF('1'로 읽힌 비트는 HIGH, '0'으로 읽힌 비트는 LOW)
        //===============================================================================================//
        [DllImport("AXL.dll")] public static extern uint AxdiReadInportByte(int lModuleNo, int lOffset, ref uint upValue);

        // 지정한 입력 접점 모듈의 Offset 위치에서 word 단위로 데이터를 읽기
        //===============================================================================================//
        // lModuleNo    : 모듈 번호
        // lOffset      : 입력 접점에 대한 Offset 위치
        // *upValue     : 0x00 ~ 0x0FFFF('1'로 읽힌 비트는 HIGH, '0'으로 읽힌 비트는 LOW)
        //===============================================================================================//
        [DllImport("AXL.dll")] public static extern uint AxdiReadInportWord(int lModuleNo, int lOffset, ref uint upValue);

        // 지정한 입력 접점 모듈의 Offset 위치에서 double word 단위로 데이터를 읽기
        //===============================================================================================//
        // lModuleNo    : 모듈 번호
        // lOffset      : 입력 접점에 대한 Offset 위치
        // *upValue     : 0x00 ~ 0x0FFFFFFFF('1'로 읽힌 비트는 HIGH, '0'으로 읽힌 비트는 LOW)
        //===============================================================================================//
        [DllImport("AXL.dll")] public static extern uint AxdiReadInportDword(int lModuleNo, int lOffset, ref uint upValue);

        //== MLII 용 M-Systems DIO(R7 series) 전용 함수.
        // 지정한 모듈에 장착된 입력 접점용 확장 기능 모듈의 Offset 위치에서 bit 단위로 데이터를 읽기
        //===============================================================================================//
        // lModuleNo   : 모듈 번호
        // lOffset     : 입력 접점에 대한 Offset 위치(0~15)
        // *upValue    : LOW(0)
        //             : HIGH(1)
        //===============================================================================================//
        [DllImport("AXL.dll")] public static extern uint AxdReadExtInportBit(int lModuleNo, int lOffset, ref uint upValue);

        // 지정한 모듈에 장착된 입력 접점용 확장 기능 모듈의 Offset 위치에서 byte 단위로 데이터를 읽기
        //===============================================================================================//
        // lModuleNo   : 모듈 번호
        // lOffset     : 입력 접점에 대한 Offset 위치(0~1)
        // *upValue    : 0x00 ~ 0x0FF('1'로 읽힌 비트는 HIGH, '0'으로 읽힌 비트는 LOW)
        //===============================================================================================//
        [DllImport("AXL.dll")] public static extern uint AxdReadExtInportByte(int lModuleNo, int lOffset, ref uint upValue);

        // 지정한 모듈에 장착된 입력 접점용 확장 기능 모듈의 Offset 위치에서 word 단위로 데이터를 읽기
        //===============================================================================================//
        // lModuleNo   : 모듈 번호
        // lOffset     : 입력 접점에 대한 Offset 위치(0)
        // *upValue    : 0x00 ~ 0x0FFFF('1'로 읽힌 비트는 HIGH, '0'으로 읽힌 비트는 LOW)
        //===============================================================================================//
        [DllImport("AXL.dll")] public static extern uint AxdReadExtInportWord(int lModuleNo, int lOffset, ref uint upValue);

        // 지정한 모듈에 장착된 입력 접점용 확장 기능 모듈의 Offset 위치에서 dword 단위로 데이터를 읽기
        //===============================================================================================//
        // lModuleNo   : 모듈 번호
        // lOffset     : 입력 접점에 대한 Offset 위치(0)
        // *upValue    : 0x00 ~ 0x00000FFFF('1'로 읽힌 비트는 HIGH, '0'으로 읽힌 비트는 LOW)
        //===============================================================================================//
        [DllImport("AXL.dll")] public static extern uint AxdReadExtInportDword(int lModuleNo, int lOffset, ref uint upValue);

        // 지정한 모듈에 장착된 출력 접점용 확장 기능 모듈의 Offset 위치에서 bit 단위로 데이터를 읽기
        //===============================================================================================//
        // lModuleNo   : 모듈 번호
        // lOffset     : 출력 접점에 대한 Offset 위치(0~15)
        // *upValue    : LOW(0)
        //             : HIGH(1)
        //===============================================================================================//
        [DllImport("AXL.dll")] public static extern uint AxdReadExtOutportBit(int lModuleNo, int lOffset, ref uint upValue);

        // 지정한 모듈에 장착된 출력 접점용 확장 기능 모듈의 Offset 위치에서 byte 단위로 데이터를 읽기
        //===============================================================================================//
        // lModuleNo   : 모듈 번호
        // lOffset     : 출력 접점에 대한 Offset 위치(0~1)
        // *upValue    : 0x00 ~ 0x0FF('1'로 읽힌 비트는 HIGH, '0'으로 읽힌 비트는 LOW)
        //===============================================================================================//
        [DllImport("AXL.dll")] public static extern uint AxdReadExtOutportByte(int lModuleNo, int lOffset, ref uint upValue);

        // 지정한 모듈에 장착된 출력 접점용 확장 기능 모듈의 Offset 위치에서 word 단위로 데이터를 읽기
        //===============================================================================================//
        // lModuleNo   : 모듈 번호
        // lOffset     : 출력 접점에 대한 Offset 위치(0)
        // *upValue    : 0x00 ~ 0x0FFFF('1'로 읽힌 비트는 HIGH, '0'으로 읽힌 비트는 LOW)
        //===============================================================================================//
        [DllImport("AXL.dll")] public static extern uint AxdReadExtOutportWord(int lModuleNo, int lOffset, ref uint upValue);

        // 지정한 모듈에 장착된 출력 접점용 확장 기능 모듈의 Offset 위치에서 dword 단위로 데이터를 읽기
        //===============================================================================================//
        // lModuleNo   : 모듈 번호
        // lOffset     : 출력 접점에 대한 Offset 위치(0)
        // *upValue    : 0x00 ~ 0x00000FFFF('1'로 읽힌 비트는 HIGH, '0'으로 읽힌 비트는 LOW)
        //===============================================================================================//
        [DllImport("AXL.dll")] public static extern uint AxdReadExtOutportDword(int lModuleNo, int lOffset, ref uint upValue);

        // 지정한 모듈에 장착된 출력 접점용 확장 기능 모듈의 Offset 위치에서 bit 단위로 데이터 출력
        //===============================================================================================//
        // lModuleNo   : 모듈 번호
        // lOffset     : 출력 접점에 대한 Offset 위치
        // uValue      : LOW(0)
        //             : HIGH(1)
        //===============================================================================================//
        [DllImport("AXL.dll")] public static extern uint AxdWriteExtOutportBit(int lModuleNo, int lOffset, uint uValue);

        // 지정한 모듈에 장착된 출력 접점용 확장 기능 모듈의 Offset 위치에서 byte 단위로 데이터 출력
        //===============================================================================================//
        // lModuleNo   : 모듈 번호
        // lOffset     : 출력 접점에 대한 Offset 위치(0~1)
        // uValue      : 0x00 ~ 0x0FF('1'로 읽힌 비트는 HIGH, '0'으로 읽힌 비트는 LOW)
        //===============================================================================================//
        [DllImport("AXL.dll")] public static extern uint AxdWriteExtOutportByte(int lModuleNo, int lOffset, uint uValue);

        // 지정한 모듈에 장착된 출력 접점용 확장 기능 모듈의 Offset 위치에서 word 단위로 데이터 출력
        //===============================================================================================//
        // lModuleNo   : 모듈 번호
        // lOffset     : 출력 접점에 대한 Offset 위치(0)
        // uValue    : 0x00 ~ 0x0FFFF('1'로 읽힌 비트는 HIGH, '0'으로 읽힌 비트는 LOW)
        //===============================================================================================//
        [DllImport("AXL.dll")] public static extern uint AxdWriteExtOutportWord(int lModuleNo, int lOffset, uint uValue);

        // 지정한 모듈에 장착된 출력 접점용 확장 기능 모듈의 Offset 위치에서 dword 단위로 데이터 출력
        //===============================================================================================//
        // lModuleNo   : 모듈 번호
        // lOffset     : 출력 접점에 대한 Offset 위치(0)
        // uValue    : 0x00 ~ 0x00000FFFF('1'로 읽힌 비트는 HIGH, '0'으로 읽힌 비트는 LOW)
        //===============================================================================================//
        [DllImport("AXL.dll")] public static extern uint AxdWriteExtOutportDword(int lModuleNo, int lOffset, uint uValue);

        // 지정한 모듈에 장착된 입/출력 접점용 확장 기능 모듈의 Offset 위치에서 bit 단위로 데이터 레벨을 설정
        //===============================================================================================//
        // lModuleNo   : 모듈 번호
        // lOffset     : 입력 접점에 대한 Offset 위치(0~15)
        // uLevel      : LOW(0)
        //             : HIGH(1)
        //===============================================================================================//
        [DllImport("AXL.dll")] public static extern uint AxdLevelSetExtportBit(int lModuleNo, int lOffset, uint uLevel);

        // 지정한 모듈에 장착된 입/출력 접점용 확장 기능 모듈의 Offset 위치에서 byte 단위로 데이터 레벨을 설정
        //===============================================================================================//
        // lModuleNo   : 모듈 번호
        // lOffset     : 입력 접점에 대한 Offset 위치(0~1)
        // uLevel      : 0x00 ~ 0xFF('1'로 설정 된 비트는 HIGH, '0'으로 설정 된 비트는 LOW)
        //===============================================================================================//
        [DllImport("AXL.dll")] public static extern uint AxdLevelSetExtportByte(int lModuleNo, int lOffset, uint uLevel);

        // 지정한 모듈에 장착된 입/출력 접점용 확장 기능 모듈의 Offset 위치에서 word 단위로 데이터 레벨을 설정
        //===============================================================================================//
        // lModuleNo   : 모듈 번호
        // lOffset     : 입력 접점에 대한 Offset 위치(0)
        // uLevel      : 0x00 ~ 0xFFFF('1'로 설정 된 비트는 HIGH, '0'으로 설정 된 비트는 LOW)
        //===============================================================================================//
        [DllImport("AXL.dll")] public static extern uint AxdLevelSetExtportWord(int lModuleNo, int lOffset, uint uLevel);

        // 지정한 모듈에 장착된 입/출력 접점용 확장 기능 모듈의 Offset 위치에서 dword 단위로 데이터 레벨을 설정
        //===============================================================================================//
        // lModuleNo   : 모듈 번호
        // lOffset     : 입력 접점에 대한 Offset 위치(0)
        // uLevel      : 0x00 ~ 0x0000FFFF('1'로 설정 된 비트는 HIGH, '0'으로 설정 된 비트는 LOW)
        //===============================================================================================//
        [DllImport("AXL.dll")] public static extern uint AxdLevelSetExtportDword(int lModuleNo, int lOffset, uint uLevel);

        // 지정한 모듈에 장착된 입/출력 접점용 확장 기능 모듈의 Offset 위치에서 bit 단위로 데이터 레벨 확인
        //===============================================================================================//
        // lModuleNo   : 모듈 번호
        // lOffset     : 입력 접점에 대한 Offset 위치(0~15)
        // *upLevel      : LOW(0)
        //             : HIGH(1)
        //===============================================================================================//
        [DllImport("AXL.dll")] public static extern uint AxdLevelGetExtportBit(int lModuleNo, int lOffset, ref uint upLevel);

        // 지정한 모듈에 장착된 입/출력 접점용 확장 기능 모듈의 Offset 위치에서 byte 단위로 데이터 레벨 확인
        //===============================================================================================//
        // lModuleNo   : 모듈 번호
        // lOffset     : 입력 접점에 대한 Offset 위치(0~1)
        // *upLevel      : 0x00 ~ 0xFF('1'로 설정 된 비트는 HIGH, '0'으로 설정 된 비트는 LOW)
        //===============================================================================================//
        [DllImport("AXL.dll")] public static extern uint AxdLevelGetExtportByte(int lModuleNo, int lOffset, ref uint upLevel);

        // 지정한 모듈에 장착된 입/출력 접점용 확장 기능 모듈의 Offset 위치에서 word 단위로 데이터 레벨 확인
        //===============================================================================================//
        // lModuleNo   : 모듈 번호
        // lOffset     : 입력 접점에 대한 Offset 위치(0)
        // *upLevel      : 0x00 ~ 0xFFFF('1'로 설정 된 비트는 HIGH, '0'으로 설정 된 비트는 LOW)
        //===============================================================================================//
        [DllImport("AXL.dll")] public static extern uint AxdLevelGetExtportWord(int lModuleNo, int lOffset, ref uint upLevel);

        // 지정한 모듈에 장착된 입/출력 접점용 확장 기능 모듈의 Offset 위치에서 dword 단위로 데이터 레벨 확인
        //===============================================================================================//
        // lModuleNo   : 모듈 번호
        // lOffset     : 입력 접점에 대한 Offset 위치(0)
        // *upLevel      : 0x00 ~ 0x0000FFFF('1'로 설정 된 비트는 HIGH, '0'으로 설정 된 비트는 LOW)
        //===============================================================================================//
        [DllImport("AXL.dll")] public static extern uint AxdLevelGetExtportDword(int lModuleNo, int lOffset, ref uint upLevel);

        //========== 고급 함수 =================================================================================
        // 지정한 입력 접점 모듈의 Offset 위치에서 신호가 Off에서 On으로 바뀌었는지 확인
        //===============================================================================================//
        // lModuleNo    : 모듈 번호
        // lOffset      : 입력 접점에 대한 Offset 위치
        // *upValue     : FALSE(0)
        //              : TRUE(1)
        //===============================================================================================//
        [DllImport("AXL.dll")] public static extern uint AxdiIsPulseOn(int lModuleNo, int lOffset, ref uint upValue);

        // 지정한 입력 접점 모듈의 Offset 위치에서 신호가 On에서 Off으로 바뀌었는지 확인
        //===============================================================================================//
        // lModuleNo    : 모듈 번호
        // lOffset      : 입력 접점에 대한 Offset 위치
        // *upValue     : FALSE(0)
        //              : TRUE(1)
        //===============================================================================================//
        [DllImport("AXL.dll")] public static extern uint AxdiIsPulseOff(int lModuleNo, int lOffset, ref uint upValue);

        // 지정한 입력 접점 모듈의 Offset 위치에서 신호가 count 만큼 호출될 동안 On 상태로 유지하는지 확인
        //===============================================================================================//
        // lModuleNo    : 모듈 번호
        // lOffset      : 입력 접점에 대한 Offset 위치
        // lCount       : 0 ~ 0x7FFFFFFF(2147483647)
        // *upValue     : FALSE(0)
        //              : TRUE(1)
        // lStart       : 1(최초 호출)
        //              : 0(반복 호출)
        //===============================================================================================//
        [DllImport("AXL.dll")] public static extern uint AxdiIsOn(int lModuleNo, int lOffset, int lCount, ref uint upValue, int lStart);

        // 지정한 입력 접점 모듈의 Offset 위치에서 신호가 count 만큼 호출될 동안 Off 상태로 유지하는지 확인
        //===============================================================================================//
        // lModuleNo    : 모듈 번호
        // lOffset      : 출력 접점에 대한 Offset 위치
        // lCount       : 0 ~ 0x7FFFFFFF(2147483647)
        // *upValue     : FALSE(0)
        //              : TRUE(1)
        // lStart       : 1(최초 호출)
        //              : 0(반복 호출)
        //===============================================================================================//
        [DllImport("AXL.dll")] public static extern uint AxdiIsOff(int lModuleNo, int lOffset, int lCount, ref uint upValue, int lStart);

        // 지정한 출력 접점 모듈의 Offset 위치에서 설정한 mSec동안 On을 유지하다가 Off 시킴
        //===============================================================================================//
        // lModuleNo    : 모듈 번호
        // lOffset      : 출력 접점에 대한 Offset 위치
        // lCount       : 0 ~ 0x7FFFFFFF(2147483647)
        // lmSec        : 1 ~ 30000
        //===============================================================================================//
        [DllImport("AXL.dll")] public static extern uint AxdoOutPulseOn(int lModuleNo, int lOffset, int lmSec);

        // 지정한 출력 접점 모듈의 Offset 위치에서 설정한 mSec동안 Off를 유지하다가 On 시킴
        //===============================================================================================//
        // lModuleNo    : 모듈 번호
        // lOffset      : 출력 접점에 대한 Offset 위치
        // lCount       : 0 ~ 0x7FFFFFFF(2147483647)
        // lmSec        : 1 ~ 30000
        //===============================================================================================//
        [DllImport("AXL.dll")] public static extern uint AxdoOutPulseOff(int lModuleNo, int lOffset, int lmSec);

        // 지정한 출력 접점 모듈의 Offset 위치에서 설정한 횟수, 설정한 간격으로 토글한 후 원래의 출력상태를 유지함
        //===============================================================================================//
        // lModuleNo    : 모듈 번호
        // lOffset      : 출력 접점에 대한 Offset 위치
        // lInitState   : Off(0)
        //              : On(1)
        // lmSecOn      : 1 ~ 30000
        // lmSecOff     : 1 ~ 30000
        // lCount       : 1 ~ 0x7FFFFFFF(2147483647)
        //              : -1 무한 토글
        //===============================================================================================//
        [DllImport("AXL.dll")] public static extern uint AxdoToggleStart(int lModuleNo, int lOffset, int lInitState, int lmSecOn, int lmSecOff, int lCount);

        // 지정한 출력 접점 모듈의 Offset 위치에서 토글중인 출력을 설정한 신호 상태로 정지 시킴
        //===============================================================================================//
        // lModuleNo    : 모듈 번호
        // lOffset      : 출력 접점에 대한 Offset 위치
        // uOnOff       : Off(0)
        //              : On(1)
        //===============================================================================================//
        [DllImport("AXL.dll")] public static extern uint AxdoToggleStop(int lModuleNo, int lOffset, uint uOnOff);

        // 지정한 출력 모듈의 Network이 끊어 졌을 경우 출력 상태를 출력 Byte 단위로 설정한다.
        //===============================================================================================//
        // lModuleNo   : 모듈 번호
        // dwSize      : 설정 할 Byte 수(ex. RTEX-DB32 : 2, RTEX-DO32 : 4)
        // dwaSetValue : 설정 할 변수 값(Default는 Network 끊어 지기 전 상태 유지)
        //             : 0 --> Network 끊어 지기 전 상태 유지
        //             : 1 --> On
        //             : 2 --> Off
        //===============================================================================================//
        [DllImport("AXL.dll")] public static extern uint AxdoSetNetworkErrorAct(int lModuleNo, uint dwSize, ref uint dwaSetValue);

        [DllImport("AXL.dll")] public static extern uint AxdSetContactNum(int lModuleNo, uint dwInputNum, uint dwOutputNum);

        [DllImport("AXL.dll")] public static extern uint AxdGetContactNum(int lModuleNo, ref uint dwpInputNum, ref uint dwpOutputNum);
    }
    #endregion

    #region AJINEXTEK MOTION Board
    public class AJINMOTION
    {
        //========== 보드 및 모듈 확인함수(Info) - Infomation ===============================================================
        // 해당 축의 보드번호, 모듈 위치, 모듈 아이디를 반환한다.
        [DllImport("AXL.dll")] public static extern uint AxmInfoGetAxis(int nAxisNo, ref int npBoardNo, ref int npModulePos, ref uint upModuleID);
        // 모션 모듈이 존재하는지 반환한다.
        [DllImport("AXL.dll")] public static extern uint AxmInfoIsMotionModule(ref uint upStatus);
        // 해당 축이 유효한지 반환한다.
        [DllImport("AXL.dll")] public static extern uint AxmInfoIsInvalidAxisNo(int nAxisNo);
        // 해당 축이 제어가 가능한 상태인지 반환한다.
        [DllImport("AXL.dll")] public static extern uint AxmInfoGetAxisStatus(int nAxisNo);
        // 축 개수, 시스템에 장착된 유효한 모션 축수를 반환한다.
        [DllImport("AXL.dll")] public static extern uint AxmInfoGetAxisCount(ref int npAxisCount);
        // 해당 보드/모듈의 첫번째 축번호를 반환한다.
        [DllImport("AXL.dll")] public static extern uint AxmInfoGetFirstAxisNo(int nBoardNo, int nModulePos, ref int npAxisNo);
        // 해당 보드의 첫번째 축번호를 반환한다.
        [DllImport("AXL.dll")] public static extern uint AxmInfoGetBoardFirstAxisNo(int lBoardNo, int lModulePos, ref int lpAxisNo);

        //========= 가상 축 함수 ============================================================================================
        // 초기 상태에서 AXM 모든 함수의 축번호 설정은 0 ~ (실제 시스템에 장착된 축수 - 1) 범위에서 유효하지만
        // 이 함수를 사용하여 실제 장착된 축번호 대신 임의의 축번호로 바꿀 수 있다.
        // 이 함수는 제어 시스템의 H/W 변경사항 발생시 기존 프로그램에 할당된 축번호를 그대로 유지하고 실제 제어 축의 
        // 물리적인 위치를 변경하여 사용을 위해 만들어진 함수이다.
        // 주의사항 : 여러 개의 실제 축번호에 대하여 같은 번호로 가상 축을 중복해서 맵핑할 경우 
        //            실제 축번호가 낮은 축만 가상 축번호로 제어 할 수 있으며, 
        //            나머지 같은 가상축 번호로 맵핑된 축은 제어가 불가능한 경우가 발생 할 수 있다.

        // 가상축을 설정한다.
        [DllImport("AXL.dll")] public static extern uint AxmVirtualSetAxisNoMap(int nRealAxisNo, int nVirtualAxisNo);
        // 설정한 가상채널(축) 번호를 반환한다.
        [DllImport("AXL.dll")] public static extern uint AxmVirtualGetAxisNoMap(int nRealAxisNo, ref int npVirtualAxisNo);
        // 멀티 가상축을 설정한다.
        [DllImport("AXL.dll")] public static extern uint AxmVirtualSetMultiAxisNoMap(int nSize, int[] npRealAxesNo, int[] npVirtualAxesNo);
        // 설정한 멀티 가상채널(축) 번호를 반환한다.
        [DllImport("AXL.dll")] public static extern uint AxmVirtualGetMultiAxisNoMap(int nSize, ref int npRealAxesNo, ref int npVirtualAxesNo);
        // 가상축 설정을 해지한다.
        [DllImport("AXL.dll")] public static extern uint AxmVirtualResetAxisMap();

        //========= 인터럽트 관련 함수 ======================================================================================
        // 콜백 함수 방식은 이벤트 발생 시점에 즉시 콜백 함수가 호출 됨으로 가장 빠르게 이벤트를 통지받을 수 있는 장점이 있으나
        // 콜백 함수가 완전히 종료 될 때까지 메인 프로세스가 정체되어 있게 된다.
        // 즉, 콜백 함수 내에 부하가 걸리는 작업이 있을 경우에는 사용에 주의를 요한다. 
        // 이벤트 방식은 쓰레드등을 이용하여 인터럽트 발생여부를 지속적으로 감시하고 있다가 인터럽트가 발생하면 
        // 처리해주는 방법으로, 쓰레드 등으로 인해 시스템 자원을 점유하고 있는 단점이 있지만
        // 가장 빠르게 인터럽트를 검출하고 처리해줄 수 있는 장점이 있다.
        // 일반적으로는 많이 쓰이지 않지만, 인터럽트의 빠른처리가 주요 관심사인 경우에 사용된다. 
        // 이벤트 방식은 이벤트의 발생 여부를 감시하는 특정 쓰레드를 사용하여 메인 프로세스와 별개로 동작되므로
        // MultiProcessor 시스템등에서 자원을 가장 효율적으로 사용할 수 있게 되어 특히 권장하는 방식이다.
        // 인터럽트 메시지를 받아오기 위하여 윈도우 메시지 또는 콜백 함수를 사용한다.
        // (메시지 핸들, 메시지 ID, 콜백함수, 인터럽트 이벤트)
        //    hWnd    : 윈도우 핸들, 윈도우 메세지를 받을때 사용. 사용하지 않으면 NULL을 입력.
        //    wMsg    : 윈도우 핸들의 메세지, 사용하지 않거나 디폴트값을 사용하려면 0을 입력.
        //    proc    : 인터럽트 발생시 호출될 함수의 포인터, 사용하지 않으면 NULL을 입력.
        //    pEvent  : 이벤트 방법사용시 이벤트 핸들
        [DllImport("AXL.dll")] public static extern uint AxmInterruptSetAxis(int nAxisNo, uint hWnd, uint uMessage, CAXHS.AXT_INTERRUPT_PROC pProc, ref uint pEvent);

        // 설정 축의 인터럽트 사용 여부를 설정한다
        // 해당 축에 인터럽트 설정 / 확인
        // uUse : 사용 유무 => DISABLE(0), ENABLE(1)
        [DllImport("AXL.dll")] public static extern uint AxmInterruptSetAxisEnable(int nAxisNo, uint uUse);
        // 설정 축의 인터럽트 사용 여부를 반환한다
        [DllImport("AXL.dll")] public static extern uint AxmInterruptGetAxisEnable(int nAxisNo, ref uint upUse);

        //인터럽트를 이벤트 방식으로 사용할 경우 해당 인터럽트 정보 읽는다.
        [DllImport("AXL.dll")] public static extern uint AxmInterruptRead(ref int npAxisNo, ref uint upFlag);

        // 해당 축의 인터럽트 플래그 값을 반환한다.
        [DllImport("AXL.dll")] public static extern uint AxmInterruptReadAxisFlag(int nAxisNo, int nBank, ref uint upFlag);

        // 지정 축의 사용자가 설정한 인터럽트 발생 여부를 설정한다.
        // lBank         : 인터럽트 뱅크 번호 (0 - 1) 설정가능.
        // uInterruptNum : 인터럽트 번호 설정 비트번호로 설정 hex값 혹은 define된값을 설정
        // AXHS.h파일에 INTERRUPT_BANK1, 2 DEF를 확인한다.
        [DllImport("AXL.dll")] public static extern uint AxmInterruptSetUserEnable(int nAxisNo, int lBank, uint uInterruptNum);

        // 지정 축의 사용자가 설정한 인터럽트 발생 여부를 확인한다.
        [DllImport("AXL.dll")] public static extern uint AxmInterruptGetUserEnable(int nAxisNo, int lBank, ref uint upInterruptNum);

        // 카운터 비교기 이벤트를 사용하기 위해 비교기에 값을 설정한다.
        // lComparatorNo :	0(CNTC1 : Command)
        //					1(CNTC2 : Actual)
        //					2 ~ 4(CNTC3 ~ CNTC5)
        // dPosition : 비교기 위치 값
        [DllImport("AXL.dll")] public static extern uint AxmInterruptSetCNTComparator(int nAxisNo, int nComparatorNo, double dPosition);

        // 카운터 비교기에 설정된 위치값을 확인한다.
        // lComparatorNo :	0(CNTC1 : Command)
        //					1(CNTC2 : Actual)
        //					2 ~ 4(CNTC3 ~ CNTC5)
        // dpPosition : 비교기 위치 값
        [DllImport("AXL.dll")] public static extern uint AxmInterruptGetCNTComparator(int nAxisNo, int nComparatorNo, ref double dpPosition);

        //======== 모션 파라메타 설정 =======================================================================================
        // AxmMotLoadParaAll로 파일을 Load 시키지 않으면 초기 파라메타 설정시 기본 파라메타 설정. 
        // 현재 PC에 사용되는 모든축에 똑같이 적용된다. 기본파라메타는 아래와 같다. 
        // 00:AXIS_NO.             =0          01:PULSE_OUT_METHOD.    =4         02:ENC_INPUT_METHOD.    =3     03:INPOSITION.          =2
        // 04:ALARM.               =1          05:NEG_END_LIMIT.       =1         06:POS_END_LIMIT.       =1     07:MIN_VELOCITY.        =1
        // 08:MAX_VELOCITY.        =700000     09:HOME_SIGNAL.         =4         10:HOME_LEVEL.          =1     11:HOME_DIR.            =0
        // 12:ZPHASE_LEVEL.        =1          13:ZPHASE_USE.          =0         14:STOP_SIGNAL_MODE.    =0     15:STOP_SIGNAL_LEVEL.   =1
        // 16:HOME_FIRST_VELOCITY. =100        17:HOME_SECOND_VELOCITY.=100       18:HOME_THIRD_VELOCITY. =20    19:HOME_LAST_VELOCITY.  =1
        // 20:HOME_FIRST_ACCEL.    =400        21:HOME_SECOND_ACCEL.   =400       22:HOME_END_CLEAR_TIME. =1000  23:HOME_END_OFFSET.     =0
        // 24:NEG_SOFT_LIMIT.      =-134217728 25:POS_SOFT_LIMIT.      =134217727 26:MOVE_PULSE.          =1     27:MOVE_UNIT.           =1
        // 28:INIT_POSITION.       =1000       29:INIT_VELOCITY.       =200       30:INIT_ACCEL.          =400   31:INIT_DECEL.          =400
        // 32:INIT_ABSRELMODE.     =0          33:INIT_PROFILEMODE.    =4         34:SVON_LEVEL.          =1     35:ALARM_RESET_LEVEL.   =1
        // 36:ENCODER_TYPE.        =1          37:SOFT_LIMIT_SEL.      =0         38:SOFT_LIMIT_STOP_MODE.=0     39:SOFT_LIMIT_ENABLE.   =0

        // 00=[AXIS_NO             ]: 축 (0축 부터 시작함)
        // 01=[PULSE_OUT_METHOD    ]: Pulse out method TwocwccwHigh = 6
        // 02=[ENC_INPUT_METHOD    ]: disable = 0, 1체배 = 1, 2체배 = 2, 4체배 = 3, 결선 관련방향 교체시(-).1체배 = 11  2체배 = 12  4체배 = 13
        // 03=[INPOSITION          ], 04=[ALARM     ], 05,06 =[END_LIMIT   ]  : 0 = B접점, 1= A접점, 2 = 사용안함, 3 = 기존상태 유지
        // 07=[MIN_VELOCITY        ]: 시작 속도(START VELOCITY)
        // 08=[MAX_VELOCITY        ]: 드라이버가 지령을 받아들일수 있는 지령 속도. 보통 일반 Servo는 700k
        // Ex> screw : 20mm pitch drive: 10000 pulse 모터: 400w
        // 09=[HOME_SIGNAL         ]: 4 - Home in0 , 0 :PosEndLimit , 1 : NegEndLimit // _HOME_SIGNAL참조.
        // 10=[HOME_LEVEL          ]: 0 = B접점, 1 = A접점, 2 = 사용안함, 3 = 기존상태 유지
        // 11=[HOME_DIR            ]: 홈 방향(HOME DIRECTION) 1:+방향, 0:-방향
        // 12=[ZPHASE_LEVEL        ]: 0 = B접점, 1 = B접점, 2 = 사용안함, 3 = 기존상태 유지
        // 13=[ZPHASE_USE          ]: Z상사용여부. 0: 사용안함 , 1: +방향, 2: -방향 
        // 14=[STOP_SIGNAL_MODE    ]: ESTOP, SSTOP 사용시 모드 0:감속정지, 1:급정지 
        // 15=[STOP_SIGNAL_LEVEL   ]: ESTOP, SSTOP 사용 레벨.  0 = B접점, 1 = A접점, 2 = 사용안함, 3 = 기존상태 유지 
        // 16=[HOME_FIRST_VELOCITY ]: 1차구동속도 
        // 17=[HOME_SECOND_VELOCITY]: 검출후속도 
        // 18=[HOME_THIRD_VELOCITY ]: 마지막 속도 
        // 19=[HOME_LAST_VELOCITY  ]: index검색및 정밀하게 검색하기위한 속도. 
        // 20=[HOME_FIRST_ACCEL    ]: 1차 가속도 , 21=[HOME_SECOND_ACCEL   ] : 2차 가속도 
        // 22=[HOME_END_CLEAR_TIME ]: 원점 검색 Enc 값 Set하기 위한 대기시간,  23=[HOME_END_OFFSET] : 원점검출후 Offset만큼 이동.
        // 24=[NEG_SOFT_LIMIT      ]: - SoftWare Limit 같게 설정하면 사용안함, 25=[POS_SOFT_LIMIT ]: + SoftWare Limit 같게 설정하면 사용안함.
        // 26=[MOVE_PULSE          ]: 드라이버의 1회전당 펄스량              , 27=[MOVE_UNIT  ]: 드라이버 1회전당 이동량 즉:스크류 Pitch
        // 28=[INIT_POSITION       ]: 에이젼트 사용시 초기위치  , 사용자가 임의로 사용가능
        // 29=[INIT_VELOCITY       ]: 에이젼트 사용시 초기속도  , 사용자가 임의로 사용가능
        // 30=[INIT_ACCEL          ]: 에이젼트 사용시 초기가속도, 사용자가 임의로 사용가능
        // 31=[INIT_DECEL          ]: 에이젼트 사용시 초기감속도, 사용자가 임의로 사용가능
        // 32=[INIT_ABSRELMODE     ]: 절대(0)/상대(1) 위치 설정
        // 33=[INIT_PROFILEMODE    ]: 프로파일모드(0 - 4) 까지 설정
        //                            '0': 대칭 Trapezode, '1': 비대칭 Trapezode, '2': 대칭 Quasi-S Curve, '3':대칭 S Curve, '4':비대칭 S Curve
        // 34=[SVON_LEVEL          ]: 0 = B접점, 1 = A접점
        // 35=[ALARM_RESET_LEVEL   ]: 0 = B접점, 1 = A접점
        // 36=[ENCODER_TYPE        ]: 0 = TYPE_INCREMENTAL, 1 = TYPE_ABSOLUTE
        // 37=[SOFT_LIMIT_SEL      ]: 0 = COMMAND, 1 = ACTUAL
        // 38=[SOFT_LIMIT_STOP_MODE]: 0 = EMERGENCY_STOP, 1 = SLOWDOWN_STOP
        // 39=[SOFT_LIMIT_ENABLE   ]: 0 = DISABLE, 1 = ENABLE

        // AxmMotSaveParaAll로 저장 되어진 .mot파일을 불러온다. 해당 파일은 사용자가 Edit 하여 사용 가능하다.
        [DllImport("AXL.dll")] public static extern uint AxmMotLoadParaAll(string szFilePath);
        // 모든축에 대한 모든 파라메타를 축별로 저장한다. .mot파일로 저장한다.
        [DllImport("AXL.dll")] public static extern uint AxmMotSaveParaAll(string szFilePath);

        // 파라메타 28 - 31번까지 사용자가 프로그램내에서  이 함수를 이용해 설정 한다
        [DllImport("AXL.dll")] public static extern uint AxmMotSetParaLoad(int nAxisNo, double InitPos, double InitVel, double InitAccel, double InitDecel);
        // 파라메타 28 - 31번까지 사용자가 프로그램내에서  이 함수를 이용해 확인 한다.
        [DllImport("AXL.dll")] public static extern uint AxmMotGetParaLoad(int nAxisNo, ref double InitPos, ref double InitVel, ref double InitAccel, ref double InitDecel);

        // 지정 축의 펄스 출력 방식을 설정한다.
        // uMethod  0 :OneHighLowHigh, 1 :OneHighHighLow, 2 :OneLowLowHigh, 3 :OneLowHighLow, 4 :TwoCcwCwHigh
        //          5 :TwoCcwCwLow, 6 :TwoCwCcwHigh, 7 :TwoCwCcwLow, 8 :TwoPhase, 9 :TwoPhaseReverse
        //        OneHighLowHigh                = 0x0,                    // 1펄스 방식, PULSE(Active High), 정방향(DIR=Low)  / 역방향(DIR=High)
        //        OneHighHighLow                = 0x1,                    // 1펄스 방식, PULSE(Active High), 정방향(DIR=High) / 역방향(DIR=Low)
        //        OneLowLowHigh                 = 0x2,                    // 1펄스 방식, PULSE(Active Low),  정방향(DIR=Low)  / 역방향(DIR=High)
        //        OneLowHighLow                 = 0x3,                    // 1펄스 방식, PULSE(Active Low),  정방향(DIR=High) / 역방향(DIR=Low)
        //        TwoCcwCwHigh                  = 0x4,                    // 2펄스 방식, PULSE(CCW:역방향),  DIR(CW:정방향),  Active High
        //        TwoCcwCwLow                   = 0x5,                    // 2펄스 방식, PULSE(CCW:역방향),  DIR(CW:정방향),  Active Low
        //        TwoCwCcwHigh                  = 0x6,                    // 2펄스 방식, PULSE(CW:정방향),   DIR(CCW:역방향), Active High
        //        TwoCwCcwLow                   = 0x7,                    // 2펄스 방식, PULSE(CW:정방향),   DIR(CCW:역방향), Active Low
        //        TwoPhase                      = 0x8,                    // 2상(90' 위상차),  PULSE lead DIR(CW: 정방향), PULSE lag DIR(CCW:역방향)
        //        TwoPhaseReverse               = 0x9                     // 2상(90' 위상차),  PULSE lead DIR(CCW: 정방향), PULSE lag DIR(CW:역방향)
        [DllImport("AXL.dll")] public static extern uint AxmMotSetPulseOutMethod(int nAxisNo, uint uMethod);
        // 지정 축의 펄스 출력 방식 설정을 반환한다,
        [DllImport("AXL.dll")] public static extern uint AxmMotGetPulseOutMethod(int nAxisNo, ref uint upMethod);

        // 지정 축의 외부(Actual) 카운트의 증가 방향 설정을 포함하여 지정 축의 Encoder 입력 방식을 설정한다.
        // uMethod : 0 - 7 설정.
        //        ObverseUpDownMode             = 0x0,                     // 정방향 Up/Down
        //        ObverseSqr1Mode               = 0x1,                     // 정방향 1체배
        //        ObverseSqr2Mode               = 0x2,                     // 정방향 2체배
        //        ObverseSqr4Mode               = 0x3,                     // 정방향 4체배
        //        ReverseUpDownMode             = 0x4,                     // 역방향 Up/Down
        //        ReverseSqr1Mode               = 0x5,                     // 역방향 1체배
        //        ReverseSqr2Mode               = 0x6,                     // 역방향 2체배
        //        ReverseSqr4Mode               = 0x7                      // 역방향 4체배
        [DllImport("AXL.dll")] public static extern uint AxmMotSetEncInputMethod(int nAxisNo, uint uMethod);
        // 지정 축의 외부(Actual) 카운트의 증가 방향 설정을 포함하여 지정 축의 Encoder 입력 방식을 반환한다.
        [DllImport("AXL.dll")] public static extern uint AxmMotGetEncInputMethod(int nAxisNo, ref uint upMethod);

        // 설정 속도 단위가 RPM(Revolution Per Minute)으로 맞추고 싶다면.
        // ex>        rpm 계산:
        // 4500 rpm ?
        // unit/ pulse = 1 : 1이면      pulse/ sec 초당 펄스수가 되는데
        // 4500 rpm에 맞추고 싶다면     4500 / 60 초 : 75회전/ 1초
        // 모터가 1회전에 몇 펄스인지 알아야 된다. 이것은 Encoder에 Z상을 검색해보면 알수있다.
        // 1회전:1800 펄스라면 75 x 1800 = 135000 펄스가 필요하게 된다.
        // AxmMotSetMoveUnitPerPulse에 Unit = 1, Pulse = 1800 넣어 동작시킨다.
        // 주의할점 : rpm으로 제어하게 된다면 속도와 가속도 도 rpm단위로 바뀌게 된다.

        // 지정 축의 펄스 당 움직이는 거리를 설정한다.
        [DllImport("AXL.dll")] public static extern uint AxmMotSetMoveUnitPerPulse(int nAxisNo, double dUnit, int nPulse);
        // 지정 축의 펄스 당 움직이는 거리를 반환한다.
        [DllImport("AXL.dll")] public static extern uint AxmMotGetMoveUnitPerPulse(int nAxisNo, ref double dpUnit, ref int npPulse);

        // 지정 축에 감속 시작 포인트 검출 방식을 설정한다.
        // uMethod : 0 -1 설정
        // AutoDetect = 0x0 : 자동 가감속.
        // RestPulse  = 0x1 : 수동 가감속.
        [DllImport("AXL.dll")] public static extern uint AxmMotSetDecelMode(int nAxisNo, uint uMethod);
        // 지정 축의 감속 시작 포인트 검출 방식을 반환한다        
        [DllImport("AXL.dll")] public static extern uint AxmMotGetDecelMode(int nAxisNo, ref uint upMethod);

        // 지정 축에 수동 감속 모드에서 잔량 펄스를 설정한다.
        // 사용방법: 만약 AxmMotSetRemainPulse를 500 펄스를 설정
        //           AxmMoveStartPos를 위치 10000을 보냈을경우에 9500펄스부터 
        //           남은 펄스 500은  AxmMotSetMinVel로 설정한 속도로 유지하면서 감속 된다.
        [DllImport("AXL.dll")] public static extern uint AxmMotSetRemainPulse(int nAxisNo, uint uData);
        // 지정 축의 수동 감속 모드에서 잔량 펄스를 반환한다.        
        [DllImport("AXL.dll")] public static extern uint AxmMotGetRemainPulse(int nAxisNo, ref uint upData);

        // 지정 축에 등속도 구동 함수에서의 최고 속도를 설정한다.
        // 주의사항 : 입력 최대 속도 값이 PPS가 아니라 UNIT 이다.
        // ex) 최대 출력 주파수(PCI-N804/404 : 10 MPPS)
        // ex) 최대 출력 Unit/Sec(PCI-N804/404 : 10MPPS * Unit/Pulse)
        [DllImport("AXL.dll")] public static extern uint AxmMotSetMaxVel(int nAxisNo, double dVel);
        // 지정 축의 등속도 구동 함수에서의 최고 속도를 반환한다.
        [DllImport("AXL.dll")] public static extern uint AxmMotGetMaxVel(int nAxisNo, ref double dpVel);

        // 지정 축의 이동 거리 계산 모드를 설정한다.
        // uAbsRelMode : POS_ABS_MODE '0' - 절대 좌표계
        //               POS_REL_MODE '1' - 상대 좌표계
        [DllImport("AXL.dll")] public static extern uint AxmMotSetAbsRelMode(int nAxisNo, uint uAbsRelMode);
        // 지정 축의 설정된 이동 거리 계산 모드를 반환한다
        [DllImport("AXL.dll")] public static extern uint AxmMotGetAbsRelMode(int nAxisNo, ref uint upAbsRelMode);

        // 지정 축의 구동 속도 프로파일 모드를 설정한다.
        // ProfileMode : SYM_TRAPEZOIDE_MODE    '0' - 대칭 Trapezode
        //               ASYM_TRAPEZOIDE_MODE   '1' - 비대칭 Trapezode
        //               QUASI_S_CURVE_MODE     '2' - 대칭 Quasi-S Curve
        //               SYM_S_CURVE_MODE       '3' - 대칭 S Curve
        //               ASYM_S_CURVE_MODE      '4' - 비대칭 S Curve
        //               SYM_TRAP_M3_SW_MODE    '5' - 대칭 Trapezode : MLIII 내부 S/W Profile
        //               ASYM_TRAP_M3_SW_MODE   '6' - 비대칭 Trapezode : MLIII 내부 S/W Profile
        //               SYM_S_M3_SW_MODE       '7' - 대칭 S Curve : MLIII 내부 S/W Profile
        //               ASYM_S_M3_SW_MODE      '8' - asymmetric S Curve : MLIII 내부 S/W Profile
        [DllImport("AXL.dll")] public static extern uint AxmMotSetProfileMode(int lAxisNo, uint uProfileMode);
        // 지정 축의 설정한 구동 속도 프로파일 모드를 반환한다.
        [DllImport("AXL.dll")] public static extern uint AxmMotGetProfileMode(int nAxisNo, ref uint upProfileMode);

        // 지정 축의 가속도 단위를 설정한다.
        // AccelUnit : UNIT_SEC2   '0' - 가감속 단위를 unit/sec2 사용
        //             SEC         '1' - 가감속 단위를 sec 사용
        [DllImport("AXL.dll")] public static extern uint AxmMotSetAccelUnit(int nAxisNo, uint uAccelUnit);
        // 지정 축의 설정된 가속도단위를 반환한다.
        [DllImport("AXL.dll")] public static extern uint AxmMotGetAccelUnit(int nAxisNo, ref uint upAccelUnit);

        // 주의사항: 최소속도를 UNIT/PULSE 보다 작게할 경우 최소단위가 UNIT/PULSE로 맞추어지기때문에 최소 속도가 UNIT/PULSE 가 된다.
        // 지정 축에 초기 속도를 설정한다.
        [DllImport("AXL.dll")] public static extern uint AxmMotSetMinVel(int nAxisNo, double dMinVelocity);
        // 지정 축의 초기 속도를 반환한다
        [DllImport("AXL.dll")] public static extern uint AxmMotGetMinVel(int nAxisNo, ref double dpMinVelocity);

        // 지정 축의 가속 저크값을 설정한다.[%].
        [DllImport("AXL.dll")] public static extern uint AxmMotSetAccelJerk(int nAxisNo, double dAccelJerk);
        // 지정 축의 설정된 가속 저크값을 반환한다.
        [DllImport("AXL.dll")] public static extern uint AxmMotGetAccelJerk(int nAxisNo, ref double dpAccelJerk);

        // 지정 축의 감속 저크값을 설정한다.[%].
        [DllImport("AXL.dll")] public static extern uint AxmMotSetDecelJerk(int nAxisNo, double dDecelJerk);
        // 지정 축의 설정된 감속 저크값을 반환한다.
        [DllImport("AXL.dll")] public static extern uint AxmMotGetDecelJerk(int nAxisNo, ref double dpDecelJerk);

        // 지정 축의 속도 Profile결정시 우선순위(속도 Or 가속도)를 설정한다.
        // Priority : PRIORITY_VELOCITY   '0' - 속도 Profile결정시 지정한 속도값에 가깝도록 계산함(일반장비 및 Spinner에 사용).
        //            PRIORITY_ACCELTIME  '1' - 속도 Profile결정시 지정한 가감속시간에 가깝도록 계산함(고속 장비에 사용).
        [DllImport("AXL.dll")] public static extern uint AxmMotSetProfilePriority(int nAxisNo, uint uPriority);
        // 지정 축의 속도 Profile결정시 우선순위(속도 Or 가속도)를 반환한다.
        [DllImport("AXL.dll")] public static extern uint AxmMotGetProfilePriority(int nAxisNo, ref uint upPriority);

        //=========== 입출력 신호 관련 설정함수 =============================================================================
        // 지정 축의 Z 상 Level을 설정한다.
        // uLevel : LOW(0), HIGH(1)
        [DllImport("AXL.dll")] public static extern uint AxmSignalSetZphaseLevel(int nAxisNo, uint uLevel);
        // 지정 축의 Z 상 Level을 반환한다.
        [DllImport("AXL.dll")] public static extern uint AxmSignalGetZphaseLevel(int nAxisNo, ref uint upLevel);

        // 지정 축의 Servo-On신호의 출력 레벨을 설정한다.
        // uLevel : LOW(0), HIGH(1)
        [DllImport("AXL.dll")] public static extern uint AxmSignalSetServoOnLevel(int nAxisNo, uint uLevel);
        // 지정 축의 Servo-On신호의 출력 레벨 설정을 반환한다.        
        [DllImport("AXL.dll")] public static extern uint AxmSignalGetServoOnLevel(int nAxisNo, ref uint upLevel);

        // 지정 축의 Servo-Alarm Reset 신호의 출력 레벨을 설정한다.
        // uLevel : LOW(0), HIGH(1)
        [DllImport("AXL.dll")] public static extern uint AxmSignalSetServoAlarmResetLevel(int nAxisNo, uint uLevel);
        // 지정 축의 Servo-Alarm Reset 신호의 출력 레벨을 설정을 반환한다.
        [DllImport("AXL.dll")] public static extern uint AxmSignalGetServoAlarmResetLevel(int nAxisNo, ref uint upLevel);

        // 지정 축의 Inpositon 신호 사용 여부 및 신호 입력 레벨을 설정한다
        // uLevel : LOW(0), HIGH(1), UNUSED(2), USED(3)        
        [DllImport("AXL.dll")] public static extern uint AxmSignalSetInpos(int nAxisNo, uint uUse);
        // 지정 축의 Inpositon 신호 사용 여부 및 신호 입력 레벨을 반환한다.        
        [DllImport("AXL.dll")] public static extern uint AxmSignalGetInpos(int nAxisNo, ref uint upUse);
        // 지정 축의 Inpositon 신호 입력 상태를 반환한다.
        [DllImport("AXL.dll")] public static extern uint AxmSignalReadInpos(int nAxisNo, ref uint upStatus);

        // 지정 축의 알람 신호 입력 시 비상 정지의 사용 여부 및 신호 입력 레벨을 설정한다.
        // uLevel : LOW(0), HIGH(1), UNUSED(2), USED(3)
        [DllImport("AXL.dll")] public static extern uint AxmSignalSetServoAlarm(int nAxisNo, uint uUse);
        // 지정 축의 알람 신호 입력 시 비상 정지의 사용 여부 및 신호 입력 레벨을 반환한다.
        [DllImport("AXL.dll")] public static extern uint AxmSignalGetServoAlarm(int nAxisNo, ref uint upUse);
        // 지정 축의 알람 신호의 입력 레벨을 반환한다.
        [DllImport("AXL.dll")] public static extern uint AxmSignalReadServoAlarm(int nAxisNo, ref uint upStatus);

        // 지정 축의 end limit sensor의 사용 유무 및 신호의 입력 레벨을 설정한다. 
        // end limit sensor 신호 입력 시 감속정지 또는 급정지에 대한 설정도 가능하다.
        // uStopMode: EMERGENCY_STOP(0), SLOWDOWN_STOP(1)
        // uPositiveLevel, uNegativeLevel : LOW(0), HIGH(1), UNUSED(2), USED(3)
        [DllImport("AXL.dll")] public static extern uint AxmSignalSetLimit(int nAxisNo, uint uStopMode, uint uPositiveLevel, uint uNegativeLevel);
        // 지정 축의 end limit sensor의 사용 유무 및 신호의 입력 레벨, 신호 입력 시 정지모드를 반환한다
        [DllImport("AXL.dll")] public static extern uint AxmSignalGetLimit(int nAxisNo, ref uint upStopMode, ref uint upPositiveLevel, ref uint upNegativeLevel);
        // 지정축의 end limit sensor의 입력 상태를 반환한다.
        [DllImport("AXL.dll")] public static extern uint AxmSignalReadLimit(int nAxisNo, ref uint upPositiveStatus, ref uint upNegativeStatus);

        // 지정 축의 Software limit의 사용 유무, 사용할 카운트, 그리고 정지 방법을 설정한다
        // uUse       : DISABLE(0), ENABLE(1)
        // uStopMode  : EMERGENCY_STOP(0), SLOWDOWN_STOP(1)
        // uSelection : COMMAND(0), ACTUAL(1)
        // 주의사항: 원점검색시 위 함수를 이용하여 소프트웨어 리밋을 미리 설정해서 구동시 원점검색시 원점검색을 도중에 멈추어졌을경우에도  Enable된다. 
        [DllImport("AXL.dll")] public static extern uint AxmSignalSetSoftLimit(int nAxisNo, uint uUse, uint uStopMode, uint uSelection, double dPositivePos, double dNegativePos);
        // 지정 축의 Software limit의 사용 유무, 사용할 카운트, 그리고 정지 방법을 반환한다
        [DllImport("AXL.dll")] public static extern uint AxmSignalGetSoftLimit(int nAxisNo, ref uint upUse, ref uint upStopMode, ref uint upSelection, ref double dpPositivePos, ref double dpNegativePos);
        // 지정 축의 Software limit의 현재 상태를 반환한다.
        [DllImport("AXL.dll")] public static extern uint AxmSignalReadSoftLimit(int nAxisNo, ref uint upPositiveStatus, ref uint upNegativeStatus);

        // 비상 정지 신호의 정지 방법 (급정지/감속정지) 또는 사용 유무를 설정한다.
        // uStopMode  : EMERGENCY_STOP(0), SLOWDOWN_STOP(1)
        // uLevel : LOW(0), HIGH(1), UNUSED(2), USED(3)
        [DllImport("AXL.dll")] public static extern uint AxmSignalSetStop(int nAxisNo, uint uStopMode, uint uLevel);
        // 비상 정지 신호의 정지 방법 (급정지/감속정지) 또는 사용 유무를 반환한다.
        [DllImport("AXL.dll")] public static extern uint AxmSignalGetStop(int nAxisNo, ref uint upStopMode, ref uint upLevel);
        // 비상 정지 신호의 입력 상태를 반환한다.        
        [DllImport("AXL.dll")] public static extern uint AxmSignalReadStop(int nAxisNo, ref uint upStatus);

        // 지정 축의 Servo-On 신호를 출력한다.
        // uOnOff : FALSE(0), TRUE(1) ( 범용 0출력에 해당됨)
        [DllImport("AXL.dll")] public static extern uint AxmSignalServoOn(int nAxisNo, uint uOnOff);
        // 지정 축의 Servo-On 신호의 출력 상태를 반환한다.
        [DllImport("AXL.dll")] public static extern uint AxmSignalIsServoOn(int nAxisNo, ref uint upOnOff);

        // 지정 축의 Servo-Alarm Reset 신호를 출력한다.
        // uOnOff : FALSE(0), TRUE(1) ( 범용 1출력에 해당됨)
        [DllImport("AXL.dll")] public static extern uint AxmSignalServoAlarmReset(int nAxisNo, uint uOnOff);

        // 범용 출력값을 설정한다.
        // uValue : Hex Value 0x00
        [DllImport("AXL.dll")] public static extern uint AxmSignalWriteOutput(int nAxisNo, uint uValue);
        // 범용 출력값을 반환한다.
        [DllImport("AXL.dll")] public static extern uint AxmSignalReadOutput(int nAxisNo, ref uint upValue);

        // lBitNo : Bit Number(0 - 4)
        // uOnOff : FALSE(0), TRUE(1)
        // 범용 출력값을 비트별로 설정한다.
        [DllImport("AXL.dll")] public static extern uint AxmSignalWriteOutputBit(int nAxisNo, int nBitNo, uint uOn);
        // 범용 출력값을 비트별로 반환한다.
        [DllImport("AXL.dll")] public static extern uint AxmSignalReadOutputBit(int nAxisNo, int nBitNo, ref uint upOn);

        // 범용 입력값을 Hex값으로 반환한다.
        [DllImport("AXL.dll")] public static extern uint AxmSignalReadInput(int nAxisNo, ref uint upValue);

        // lBitNo : Bit Number(0 - 4)
        // 범용 입력값을 비트별로 반환한다.
        [DllImport("AXL.dll")] public static extern uint AxmSignalReadInputBit(int nAxisNo, int nBitNo, ref uint upOn);

        // 입력신호들의 디지털 필터계수를 설정한다.
        // uSignal: END_LIMIT(0), INP_ALARM(1), UIN_00_01(2), UIN_02_04(3)
        // dBandwidthUsec: 0.2uSec~26666usec
        [DllImport("AXL.dll")] public static extern uint AxmSignalSetFilterBandwidth(int nAxisNo, uint uSignal, double dBandwidthUsec);

        // Universal Output을 mSec 동안 On 유지하다가 Off 시킨다
        // nArraySize : 동작시킬 OutputBit배열의 수
        // nmSec : 0 ~ 30000
        [DllImport("AXL.dll")] public static extern uint AxmSignalOutputOn(int nAxisNo, int nArraySize, uint[] upBitNo, uint umSec);

        // Universal Output을 mSec 동안 Off 유지하다가 On 시킨다
        // nArraySize : 동작시킬 OutputBit배열의 수
        // nmSec : 0 ~ 30000
        [DllImport("AXL.dll")] public static extern uint AxmSignalOutputOff(int nAxisNo, int nArraySize, int[] npBitNo, int nmSec);

        //========== 모션 구동중 및 구동후에 상태 확인하는 함수======================================================
        // (구동상태)모션 구동 중인가를 확인
        [DllImport("AXL.dll")] public static extern uint AxmStatusReadInMotion(int nAxisNo, ref uint upStatus);

        //  (펄스 카운트 값)구동시작 이후 출력된 펄스 카운터 값을 확인
        [DllImport("AXL.dll")] public static extern uint AxmStatusReadDrivePulseCount(int nAxisNo, ref int npPulse);

        // DriveStatus 레지스터를 확인
        // 주의사항 : 각 제품별로 하드웨어적인 신호가 다르기때문에 매뉴얼 및 AXHS.xxx 파일을 참고해야한다.
        [DllImport("AXL.dll")] public static extern uint AxmStatusReadMotion(int nAxisNo, ref uint upStatus);

        // EndStatus 레지스터를 확인
        // 주의사항 : 각 제품별로 하드웨어적인 신호가 다르기때문에 매뉴얼 및 AXHS.xxx 파일을 참고해야한다.
        [DllImport("AXL.dll")] public static extern uint AxmStatusReadStop(int nAxisNo, ref uint upStatus);

        // Mechanical 레지스터를 확인
        // 주의사항 : 각 제품별로 하드웨어적인 신호가 다르기때문에 매뉴얼 및 AXHS.xxx 파일을 참고해야한다.
        [DllImport("AXL.dll")] public static extern uint AxmStatusReadMechanical(int nAxisNo, ref uint upStatus);

        // 현재 속도를 읽어 온다
        [DllImport("AXL.dll")] public static extern uint AxmStatusReadVel(int nAxisNo, ref double dpVelocity);

        // Command Pos과 Actual Pos의 차를 확인
        [DllImport("AXL.dll")] public static extern uint AxmStatusReadPosError(int nAxisNo, ref double dpError);

        // 최후 드라이브의 이동 거리를 확인
        [DllImport("AXL.dll")] public static extern uint AxmStatusReadDriveDistance(int nAxisNo, ref double dpUnit);

        // 지정 축의 위치 정보 사용 방법에 대하여 설정한다.
        // uPosType  : Actual position 과 Command position 의 표시 방법
        //    POSITION_LIMIT '0' - 기본 동작, 전체 범위 내에서 동작
        //    POSITION_BOUND '1' - 위치 범위 주기형, dNegativePos ~ dPositivePos 범위로 동작
        // 주의사항(PCI-Nx04해당)
        // - BOUNT설정시 카운트 값이 Max값을 초과 할 때 Min값이되며 반대로 Min값을 초과 할 때 Max값이 된다.
        // - 다시말해 현재 위치값이 설정한 값 밖에서 카운트 될 때는 위의 Min, Max값이 적용되지 않는다.
        [DllImport("AXL.dll")] public static extern uint AxmStatusSetPosType(int nAxisNo, uint uPosType, double dPositivePos, double dNegativePos);
        // 지정 축의 위치 정보 사용 방법에 대하여 반환한다.
        [DllImport("AXL.dll")] public static extern uint AxmStatusGetPosType(int nAxisNo, ref uint upPosType, ref double dpPositivePos, ref double dpNegativePos);
        // 지정 축의 절대치 엔코더 원점 Offset 위치를 설정한다.[PCI-R1604-MLII 전용]
        [DllImport("AXL.dll")] public static extern uint AxmStatusSetAbsOrgOffset(int nAxisNo, double dOrgOffsetPos);

        // 지정 축의 Actual 위치를 설정한다.
        [DllImport("AXL.dll")] public static extern uint AxmStatusSetActPos(int nAxisNo, double dPos);
        // 지정 축의 Actual 위치를 반환한다.
        [DllImport("AXL.dll")] public static extern uint AxmStatusGetActPos(int nAxisNo, ref double dpPos);

        // 지정 축의 Command 위치를 설정한다.
        [DllImport("AXL.dll")] public static extern uint AxmStatusSetCmdPos(int nAxisNo, double dPos);
        // 지정 축의 Command 위치를 반환한다.
        [DllImport("AXL.dll")] public static extern uint AxmStatusGetCmdPos(int nAxisNo, ref double dpPos);
        // 지정 축의 Command 위치와 Actual 위치를 dPos 값으로 일치 시킨다.
        [DllImport("AXL.dll")] public static extern uint AxmStatusSetPosMatch(int nAxisNo, double dPos);

        // 지정 축의 모션 상태(Cmd, Act, Driver Status, Mechanical Signal, Universal Signal)를 한번에 확인 할 수 있다.
        // MOTION_INFO 구조체의 uMask 설정으로 모션 상태 정보를 지정한다.
        // uMask : 모션 상태 표시(6bit) - ex) uMask = 0x1F 설정 시 모든 상태를 표시함.
        // 사용자가 설정한 Level(In/Out)은 반영되지 않음.
        //    [0]        |    Command Position Read
        //    [1]        |    Actual Position Read
        //    [2]        |    Mechanical Signal Read
        //    [3]        |    Driver Status Read
        //    [4]        |    Universal Signal Input Read
        //               |    Universal Signal Output Read
        [DllImport("AXL.dll")] public static extern uint AxmStatusReadMotionInfo(int nAxisNo, ref MOTION_INFO MI);

        // Network 제품 전용함수.
        // 지정한 축의 서보팩에 AlarmCode를 읽어오도록 명령하는 함수.
        [DllImport("AXL.dll")] public static extern uint AxmStatusRequestServoAlarm(int nAxisNo);
        // 지정한 축의 서보팩 AlarmCode를 읽어오는 함수.
        // upAlarmCode      : 해당 서보팩의 Alarm Code참조
        // MR_J4_xxB  : 상위 16Bit : 알람코드 2 digit의 10진수 값, 하위 16Bit : 알람 상세 코드 1 digit 10진수 값
        // uReturnMode      : 함수의 반환 동작조건을 설정[SIIIH(MR-J4-xxB)는 사용하지 않음]
        // [0-Immediate]    : 함수 실행 후 바로 반환
        // [1-Blocking]     : 서보팩으로 부터 알람 코드를 읽을 대 까지 반환하지않음
        // [2-Non Blocking] : 서보팩으로 부터 알람 코드를 읽을 대 까지 반환하지않으나 프로그램 Blocking되지않음
        [DllImport("AXL.dll")] public static extern uint AxmStatusReadServoAlarm(int nAxisNo, uint uReturnMode, ref uint upAlarmCode);
        // 지정한 에러코드에 해당하는 Alarm String을 받아오는 함수
        [DllImport("AXL.dll")] public static extern uint AxmStatusGetServoAlarmString(int nAxisNo, uint uAlarmCode, int nAlarmStringSize, byte[] szAlarmString);

        // 지정한 축의 서보팩에 Alarm History를 읽어오도록 명령하는 함수
        [DllImport("AXL.dll")] public static extern uint AxmStatusRequestServoAlarmHistory(int nAxisNo);
        // 지정한 축의 서보팩 Alarm History를 읽어오는 함수.
        // lpCount          : 읽은 Alarm History 개수 
        // upAlarmCode      : Alarm History를 반환할 배열
        // uReturnMode      : 함수의 반환 동작조건을 설정
        // [0-Immediate]    : 함수 실행 후 바로 반환
        // [1-Blocking]     : 서보팩으로 부터 알람 코드를 읽을 때 까지 제어권 반환하지않음
        // [2-Non Blocking] : 서보팩으로 부터 알람 코드를 읽을 때 까지 제어권 반환하지않으나 프로그램 Blocking되지않음(윈도우 메세지 내부에서 처리함)
        [DllImport("AXL.dll")] public static extern uint AxmStatusReadServoAlarmHistory(int nAxisNo, uint uReturnMode, ref int npCount, ref uint upAlarmCode);
        // 지정한 축의 서보팩 Alarm History를 Clear한다.
        [DllImport("AXL.dll")] public static extern uint AxmStatusClearServoAlarmHistory(int nAxisNo);

        //======== 홈관련 함수===============================================================================================
        // 지정 축의 Home 센서 Level 을 설정한다.
        // uLevel : LOW(0), HIGH(1)
        [DllImport("AXL.dll")] public static extern uint AxmHomeSetSignalLevel(int nAxisNo, uint uLevel);
        // 지정 축의 Home 센서 Level 을 반환한다.
        [DllImport("AXL.dll")] public static extern uint AxmHomeGetSignalLevel(int nAxisNo, ref uint upLevel);
        // 현재 홈 신호 입력상태를 확인한다. 홈신호는 사용자가 임의로 AxmHomeSetMethod 함수를 이용하여 설정할수있다.
        // 일반적으로 홈신호는 범용 입력 0를 사용하고있지만 AxmHomeSetMethod 이용해서 바꾸면 + , - Limit를 사용할수도있다.
        // upStatus : OFF(0), ON(1)
        [DllImport("AXL.dll")] public static extern uint AxmHomeReadSignal(int nAxisNo, ref uint upStatus);

        // 해당 축의 원점검색을 수행하기 위해서는 반드시 원점 검색관련 파라메타들이 설정되어 있어야 됩니다. 
        // 만약 MotionPara설정 파일을 이용해 초기화가 정상적으로 수행됐다면 별도의 설정은 필요하지 않다. 
        // 원점검색 방법 설정에는 검색 진행방향, 원점으로 사용할 신호, 원점센서 Active Level, 엔코더 Z상 검출 여부 등을 설정 한다.
        // 주의사항 : 레벨을 잘못 설정시 -방향으로 설정해도  +방향으로 동작할수 있으며, 홈을 찾는데 있어 문제가 될수있다.
        // (자세한 내용은 AxmMotSaveParaAll 설명 부분 참조)
        // 홈레벨은 AxmSignalSetHomeLevel 사용한다.
        // HClrTim : HomeClear Time : 원점 검색 Encoder 값 Set하기 위한 대기시간 
        // HmDir(홈 방향): DIR_CCW (0) -방향 , DIR_CW(1) +방향
        // HOffset - 원점검출후 이동거리.
        // uZphas: 1차 원점검색 완료 후 엔코더 Z상 검출 유무 설정  0: 사용안함 , 1: Hmdir과 반대 방향, 2: Hmdir과 같은 방향
        // HmSig : PosEndLimit(0) -> +Limit
        //         NegEndLimit(1) -> -Limit
        //         HomeSensor (4) -> 원점센서(범용 입력 0)
        [DllImport("AXL.dll")] public static extern uint AxmHomeSetMethod(int nAxisNo, int nHmDir, uint uHomeSignal, uint uZphas, double dHomeClrTime, double dHomeOffset);
        // 설정되어있는 홈 관련 파라메타들을 반환한다.
        [DllImport("AXL.dll")] public static extern uint AxmHomeGetMethod(int nAxisNo, ref int nHmDir, ref uint uHomeSignal, ref uint uZphas, ref double dHomeClrTime, ref double dHomeOffset);

        // 원점검색 방법의 미세조정을 하는 함수(기본적으로 설정하지 않아도됨).
        // dHomeDogDistance[500 pulse]: 첫번째 Step에서 HomeDog가 센서를 지나쳤는지 확인하기위한 Dog길이를 입력.(단위는 AxmMotSetMoveUnitPerPulse함수로 설정한 단위)
        // lLevelScanTime[100msec]: 2번째 Step(원점센서를 빠져나가는 동작)에서 Level상태를 확인할 Scan시간을 설정(단위는 msec[1~1000]).
        // uFineSearchUse[USE]: 기본 원점검색시 5 Step를 사용하는데 3 Step만 사용하도록 변경할때 0으로 설정.
        // uHomeClrUse[USE]: 원점검색 후 지령값과 Encoder값을 0으로 자동 설정여부를 설정.
        [DllImport("AXL.dll")] public static extern uint AxmHomeSetFineAdjust(int nAxisNo, double dHomeDogLength, uint lLevelScanTime, uint uFineSearchUse, uint uHomeClrUse);
        // 설정되어있는 홈 관련 미세조정 파라메타들을 반환한다.
        [DllImport("AXL.dll")] public static extern uint AxmHomeGetFineAdjust(int nAxisNo, ref double dpHomeDogLength, ref uint lpLevelScanTime, ref uint upFineSearchUse, ref uint upHomeClrUse);

        // 원점을 빠르고 정밀하게 검색하기 위해 여러 단계의 스탭으로 검출한다. 이때 각 스탭에 사용 될 속도를 설정한다. 
        // 이 속도들의 설정값에 따라 원점검색 시간과, 원점검색 정밀도가 결정된다. 
        // 각 스탭별 속도들을 적절히 바꿔가면서 각 축의 원점검색 속도를 설정하면 된다. 
        // (자세한 내용은 AxmMotSaveParaAll 설명 부분 참조)
        // 원점검색시 사용될 속도를 설정하는 함수
        // [dVelFirst]- 1차구동속도   [dVelSecond]-검출후속도   [dVelThird]- 마지막 속도  [dvelLast]- index검색및 정밀하게 검색하기위해. 
        // [dAccFirst]- 1차구동가속도 [dAccSecond]-검출후가속도 
        [DllImport("AXL.dll")] public static extern uint AxmHomeSetVel(int nAxisNo, double dVelFirst, double dVelSecond, double dVelThird, double dvelLast, double dAccFirst, double dAccSecond);
        // 설정되어있는 원점검색시 사용될 속도를 반환한다.
        [DllImport("AXL.dll")] public static extern uint AxmHomeGetVel(int nAxisNo, ref double dVelFirst, ref double dVelSecond, ref double dVelThird, ref double dvelLast, ref double dAccFirst, ref double dAccSecond);

        // 원점검색을 시작한다.
        // 원점검색 시작함수를 실행하면 라이브러리 내부에서 해당축의 원점검색을 수행 할 쓰레드가 자동 생성되어 원점검색을 순차적으로 수행한 후 자동 종료된다.
        // 주의사항 : 진행방향과 반대방향의 리미트 센서가 들어와도 진행방향의 센서가 ACTIVE되지않으면 동작한다.
        //            원점 검색이 시작되어 진행방향이 리밋트 센서가 들어오면 리밋트 센서가 감지되었다고 생각하고 다음단계로 진행된다.
        [DllImport("AXL.dll")] public static extern uint AxmHomeSetStart(int nAxisNo);
        // 원점검색 결과를 사용자가 임의로 설정한다.
        // 원점검색 함수를 이용해 성공적으로 원점검색이 수행되고나면 검색 결과가 HOME_SUCCESS로 설정됩니다.
        // 이 함수는 사용자가 원점검색을 수행하지않고 결과를 임의로 설정할 수 있다. 
        // uHomeResult 설정
        // HOME_SUCCESS                             = 0x01        // 홈 완료
        // HOME_SEARCHING                           = 0x02        // 홈검색중
        // HOME_ERR_GNT_RANGE                       = 0x10        // 홈 검색 범위를 벗어났을경우
        // HOME_ERR_USER_BREAK                      = 0x11        // 속도 유저가 임의로 정지명령을 내렸을경우
        // HOME_ERR_VELOCITY                        = 0x12        // 속도 설정 잘못했을경우
        // HOME_ERR_AMP_FAULT                       = 0x13        // 서보팩 알람 발생 에러
        // HOME_ERR_NEG_LIMIT                       = 0x14        // (-)방향 구동중 (+)리미트 센서 감지 에러
        // HOME_ERR_POS_LIMIT                       = 0x15        // (+)방향 구동중 (-)리미트 센서 감지 에러
        // HOME_ERR_NOT_DETECT                      = 0x16        // 지정한 신호 검출하지 못 할 경우 에러
        // HOME_ERR_UNKNOWN                         = 0xFF        
        [DllImport("AXL.dll")] public static extern uint AxmHomeSetResult(int nAxisNo, uint uHomeResult);
        // 원점검색 결과를 반환한다.
        // 원점검색 함수의 검색 결과를 확인한다. 원점검색이 시작되면 HOME_SEARCHING으로 설정되며 원점검색에 실패하면 실패원인이 설정된다. 실패 원인을 제거한 후 다시 원점검색을 진행하면 된다.
        [DllImport("AXL.dll")] public static extern uint AxmHomeGetResult(int nAxisNo, ref uint upHomeResult);

        // 원점검색 진행률을 반환한다.
        // 원점검색 시작되면 진행율을 확인할 수 있다. 원점검색이 완료되면 성공여부와 관계없이 100을 반환하게 된다. 원점검색 성공여부는 GetHome Result함수를 이용해 확인할 수 있다.
        // upHomeMainStepNumber : Main Step 진행율이다. 
        // 겐트리 FALSE일 경우upHomeMainStepNumber : 0 일때면 선택한 축만 진행사항이고 홈 진행율은 upHomeStepNumber 표시한다.
        // 겐트리 TRUE일 경우 upHomeMainStepNumber : 0 일때면 마스터 홈을 진행사항이고 마스터 홈 진행율은 upHomeStepNumber 표시한다.
        // 겐트리 TRUE일 경우 upHomeMainStepNumber : 10 일때면 슬레이브 홈을 진행사항이고 마스터 홈 진행율은 upHomeStepNumber 표시한다.
        // upHomeStepNumber     : 선택한 축에대한 진행율을 표시한다. 
        // 겐트리 FALSE일 경우  : 선택한 축만 진행율을 표시한다.
        // 겐트리 TRUE일 경우 마스터축, 슬레이브축 순서로 진행율을 표시된다.
        [DllImport("AXL.dll")] public static extern uint AxmHomeGetRate(int nAxisNo, ref uint upHomeMainStepNumber, ref uint upHomeStepNumber);

        //========= 위치 구동함수 ===========================================================================================
        // 주의사항: 위치를 설정할경우 반드시 UNIT/PULSE의 맞추어서 설정한다.
        //           위치를 UNIT/PULSE 보다 작게할 경우 최소단위가 UNIT/PULSE로 맞추어지기때문에 그위치까지 구동이 될수없다.

        // 설정 속도 단위가 RPM(Revolution Per Minute)으로 맞추고 싶다면.
        // ex>        rpm 계산:
        // 4500 rpm ?
        // unit/ pulse = 1 : 1이면      pulse/ sec 초당 펄스수가 되는데
        // 4500 rpm에 맞추고 싶다면     4500 / 60 초 : 75회전/ 1초
        // 모터가 1회전에 몇 펄스인지 알아야 된다. 이것은 Encoder에 Z상을 검색해보면 알수있다.
        // 1회전:1800 펄스라면 75 x 1800 = 135000 펄스가 필요하게 된다.
        // AxmMotSetMoveUnitPerPulse에 Unit = 1, Pulse = 1800 넣어 동작시킨다. 

        // 설정한 거리만큼 또는 위치까지 이동한다.
        // 지정 축의 절대 좌표/ 상대좌표 로 설정된 위치까지 설정된 속도와 가속율로 구동을 한다.
        // 속도 프로파일은 AxmMotSetProfileMode 함수에서 설정한다.
        // 펄스가 출력되는 시점에서 함수를 벗어난다.
        // AxmMotSetAccelUnit(lAxisNo, 1) 일경우 dAccel -> dAccelTime , dDecel -> dDecelTime 으로 바뀐다.
        [DllImport("AXL.dll")] public static extern uint AxmMoveStartPos(int nAxisNo, double dPos, double dVel, double dAccel, double dDecel);

        // 설정한 거리만큼 또는 위치까지 이동한다.
        // 지정 축의 절대 좌표/상대좌표로 설정된 위치까지 설정된 속도와 가속율로 구동을 한다.
        // 속도 프로파일은 AxmMotSetProfileMode 함수에서 설정한다. 
        // 펄스 출력이 종료되는 시점에서 함수를 벗어난다
        [DllImport("AXL.dll")] public static extern uint AxmMovePos(int nAxisNo, double dPos, double dVel, double dAccel, double dDecel);

        // 설정한 속도로 구동한다.
        // 지정 축에 대하여 설정된 속도와 가속율로 지속적으로 속도 모드 구동을 한다. 
        // 펄스 출력이 시작되는 시점에서 함수를 벗어난다.
        // Vel값이 양수이면 CW, 음수이면 CCW 방향으로 구동.
        [DllImport("AXL.dll")] public static extern uint AxmMoveVel(int nAxisNo, double dVel, double dAccel, double dDecel);

        // 지정된 다축에 대하여 설정된 속도와 가속율로 지속적으로 속도 모드 구동을 한다.
        // 펄스 출력이 시작되는 시점에서 함수를 벗어난다.
        // Vel값이 양수이면 CW, 음수이면 CCW 방향으로 구동.
        [DllImport("AXL.dll")] public static extern uint AxmMoveStartMultiVel(int lArraySize, int[] lpAxesNo, double[] dVel, double[] dAccel, double[] dDecel);

        // 지정된 다축에 대하여 설정된 속도와 가속율, SyncMode에 따라 지속적으로 속도 모드 구동을 한다.
        // 펄스 출력이 시작되는 시점에서 함수를 벗어난다.
        // Vel값이 양수이면 CW, 음수이면 CCW 방향으로 구동.
        // uSyncMode    : 동기정지기능 사용안함(0), 동기정지 기능만 사용(1), 알람에 대해서도 동기 정기기능 사용(2)
        [DllImport("AXL.dll")] public static extern uint AxmMoveStartMultiVelEx(int lArraySize, int[] lpAxesNo, double[] dpVel, double[] dpAccel, double[] dpDecel, uint uSyncMode);

        // 지정된 다축에 대하여 설정된 속도와 가속율로 지속적으로 속도 모드 구동을 한다.
        // 펄스 출력이 시작되는 시점에서 함수를 벗어나며 Master축은(Distance가 가장 큰) dVel속도로 움직이며, 나머지 축들의 Distance비율로 움직인다. 
        // 속도는 해당 Chip중 축 번호가 가장 낮은 축의 속도만 읽힘
        [DllImport("AXL.dll")] public static extern uint AxmMoveStartLineVel(int lArraySize, int[] lpAxesNo, double[] dpDis, double dVel, double dAccel, double dDecel);

        // 특정 Input 신호의 Edge를 검출하여 즉정지 또는 감속정지하는 함수.
        // lDetect Signal : edge 검출할 입력 신호 선택.
        // lDetectSignal  : PosEndLimit(0), NegEndLimit(1), HomeSensor(4), EncodZPhase(5), UniInput02(6), UniInput03(7)
        // Signal Edge    : 선택한 입력 신호의 edge 방향 선택 (rising or falling edge).
        //                  SIGNAL_DOWN_EDGE(0), SIGNAL_UP_EDGE(1)
        // 구동방향      : Vel값이 양수이면 CW, 음수이면 CCW.
        // SignalMethod  : 급정지 EMERGENCY_STOP(0), 감속정지 SLOWDOWN_STOP(1)
        // 주의사항: SignalMethod를 EMERGENCY_STOP(0)로 사용할경우 가감속이 무시되며 지정된 속도로 가속 급정지하게된다.
        //          PCI-Nx04를 사용할 경우 lDetectSignal이 PosEndLimit , NegEndLimit(0,1) 을 찾을경우 신호의레벨 Active 상태를 검출하게된다.
        [DllImport("AXL.dll")] public static extern uint AxmMoveSignalSearch(int nAxisNo, double dVel, double dAccel, int nDetectSignal, int nSignalEdge, int nSignalMethod);

        // 특정 Input 신호의 Edge를 검출하여 사용자가 지정함 위치 값만큼 이동하는 함수.(MLIII : Sigma-5/7 전용)
        // dVel           : 구동 속도 설정, 양수이면 CW, 음수이면 CCW.
        // dAccel         : 구동 가속도 설정
        // dDecel         : 구동 감속도 설정, 일반적으로 dAccel의 50배로 설정함.
        // lDetectSignal  : HomeSensor(4)
        // dDis           : 입력 신호의 검출 위치를 기준으로 사용자가 지정한 위치만큼 상대 구동됨.
        // 주의사항:        
        //          - 구동방향과 반대 방향으로 dDis 값 입력시 역방향으로 구동 될 수 있음.
        //          - 속도가 빠르고, dDis 값이 작은 경우 모터가 신호 감지해서 정지한 이후에 최종 위치로 가기 위해서 역방향으로 구동될 수 있음
        //          - 해당 함수를 사용하기 전에 원점 센서는 반드시 LOW 또는 HIGH로 설정되어 있어야함.
        [DllImport("AXL.dll")] public static extern uint AxmMoveSignalSearchAtDis(int nAxisNo, double dVel, double dAccel, double dDecel, int nDetectSignal, double dDis);

        // 지정 축에서 설정된 신호를 검출하고 그 위치를 저장하기 위해 이동하는 함수이다.
        // 원하는 신호를 골라 찾아 움직이는 함수 찾을 경우 그 위치를 저장시켜놓고 AxmGetCapturePos사용하여 그값을 읽는다.
        // Signal Edge   : 선택한 입력 신호의 edge 방향 선택 (rising or falling edge).
        //                 SIGNAL_DOWN_EDGE(0), SIGNAL_UP_EDGE(1)
        // 구동방향      : Vel값이 양수이면 CW, 음수이면 CCW.
        // SignalMethod  : 급정지 EMERGENCY_STOP(0), 감속정지 SLOWDOWN_STOP(1)
        // lDetect Signal: edge 검출할 입력 신호 선택.SIGNAL_DOWN_EDGE(0), SIGNAL_UP_EDGE(1)
        //                 상위 8bit에 대하여 기본 구동(0), Software 구동(1) 을 선택할 수 있다. SMP Board(PCIe-Rxx05-MLIII) 전용
        // lDetectSignal : PosEndLimit(0), NegEndLimit(1), HomeSensor(4), EncodZPhase(5), UniInput02(6), UniInput03(7)
        // lTarget       : COMMAND(0), ACTUAL(1)
        // 주의사항: SignalMethod를 EMERGENCY_STOP(0)로 사용할경우 가감속이 무시되며 지정된 속도로 가속 급정지하게된다.
        //           PCI-Nx04를 사용할 경우 lDetectSignal이 PosEndLimit , NegEndLimit(0,1) 을 찾을경우 신호의레벨 Active 상태를 검출하게된다.
        [DllImport("AXL.dll")] public static extern uint AxmMoveSignalCapture(int nAxisNo, double dVel, double dAccel, int nDetectSignal, int nSignalEdge, int nTarget, int nSignalMethod);
        // 'AxmMoveSignalCapture' 함수에서 저장된 위치값을 확인하는 함수이다.
        // 주의사항: 함수 실행 결과가 "AXT_RT_SUCCESS"일때 저장된 위치가 유효하며, 이 함수를 한번 실행하면 저장 위치값이 초기화된다.
        [DllImport("AXL.dll")] public static extern uint AxmMoveGetCapturePos(int nAxisNo, ref double dpCapPos);

        // "설정한 거리만큼 또는 위치까지 이동하는 함수.
        // 함수를 실행하면 해당 Motion 동작을 시작한 후 Motion 이 완료될때까지 기다리지 않고 바로 함수를 빠져나간다."
        [DllImport("AXL.dll")] public static extern uint AxmMoveStartMultiPos(int nArraySize, int[] nAxisNo, double[] dPos, double[] dVel, double[] dAccel, double[] dDecel);

        // 다축을 설정한 거리만큼 또는 위치까지 이동한다.
        // 지정 축들의 절대 좌표로 설정된 위치까지 설정된 속도와 가속율로 구동을 한다.
        [DllImport("AXL.dll")] public static extern uint AxmMoveMultiPos(int nArraySize, int[] nAxisNo, double[] dPos, double[] dVel, double[] dAccel, double[] dDecel);

        // 설정한 토크 및 속도 값으로 모터를 구동한다.(PCIR-1604-MLII/SIIIH, PCIe-Rxx04-SIIIH  전용 함수)
        // dTroque        : 최대 출력 토크에 대한 %값.     
        // 구동방향       : dTroque값이 양수이면 CW, 음수이면 CCW.
        // dVel           : 최대 모터 구동 속도에 대한 %값.
        // uAccFilterSel  : LINEAR_ACCDCEL(0), EXPO_ACCELDCEL(1), SCURVE_ACCELDECEL(2)
        // uGainSel       : GAIN_1ST(0), GAIN_2ND(1)
        // uSpdLoopSel    : PI_LOOP(0), P_LOOP(1)

        // PCIe-Rxx05-MLIII
        // dTorque        : 최대 출력 토크에 대한 %값 (단위: %)
        //                  dTorque 값이 양수지면 CW, 음수이면 CCW 방향으로 구동
        // dVel           : 구동 속도 (단위: pps)
        // dwAccFilterSel : 사용하지 않음
        // dwGainSel      : 사용하지 않음
        // dwSpdLoopSel   : 사용하지 않음
        [DllImport("AXL.dll")] public static extern uint AxmMoveStartTorque(int lAxisNo, double dTorque, double dVel, uint uAccFilterSel, uint uGainSel, uint uSpdLoopSel);

        // 지정 축의 토크 구동을 정지 한다.
        // AxmMoveStartTorque후 반드시 AxmMoveTorqueStop를 실행하여야 한다.
        [DllImport("AXL.dll")] public static extern uint AxmMoveTorqueStop(int lAxisNo, uint uMethod);

        // 설정한 거리만큼 또는 위치까지 이동한다.
        // 지정 축의 절대 좌표/상대좌표로 설정된 위치까지 설정된 속도/가속율로 구동을 한다.
        // 속도 프로파일은 비대칭 사다리꼴로 고정됩니다.
        // 가감속도 설정 단위는 기울기로 고정됩니다.
        // dAccel != 0.0 이고 dDecel == 0.0 일 경우 이전 속도에서 감속 없이 지정 속도까지 가속.
        // dAccel != 0.0 이고 dDecel != 0.0 일 경우 이전 속도에서 지정 속도까지 가속후 등속 이후 감속.
        // dAccel == 0.0 이고 dDecel != 0.0 일 경우 이전 속도에서 다음 속도까지 감속.

        // 다음의 조건을 만족하여야 합니다.
        // dVel[1] == dVel[3]을 반드시 만족하여야 한다.
        // dVel[2]로 정속 구동 구간이 발생할 수 있도록 dPosition이 충분히 큰값이어야 한다.
        // Ex) dPosition = 10000;
        // dVel[0] = 300., dAccel[0] = 200., dDecel[0] = 0.;    <== 가속
        // dVel[1] = 500., dAccel[1] = 100., dDecel[1] = 0.;    <== 가속
        // dVel[2] = 700., dAccel[2] = 200., dDecel[2] = 250.;  <== 가속, 등속, 감속
        // dVel[3] = 500., dAccel[3] = 0.,   dDecel[3] = 150.;  <== 감속
        // dVel[4] = 200., dAccel[4] = 0.,   dDecel[4] = 350.;  <== 감속
        // 펄스 출력이 종료되는 시점에서 함수를 벗어난다
        [DllImport("AXL.dll")] public static extern uint AxmMoveStartPosWithList(int lAxisNo, double dPosition, double[] dpVel, double[] dpAccel, double[] dpDecel, int lListNum);

        // 설정한 거리만큼 또는 위치까지 대상 축의 위치가 증감할 때 이동을 시작한다.
        // lEvnetAxisNo    : 시작 조건 발생 축
        // dComparePosition: 시작 조건 발생 축의 조건 발생 위치.
        // uPositionSource : 시작 조건 발생 축의 조건 발생 위치 기준 선택 => COMMAND(0), ACTUAL(1)
        // 예약 후 취소는 AxmMoveStop, AxmMoveEStop, AxmMoveSStop를 사용
        // 이동 축과 시작 조건 발생 축은 4축 단위 하나의 그룹(2V04의 경우 같은 모듈)에 존재하여야 합니다.
        [DllImport("AXL.dll")] public static extern uint AxmMoveStartPosWithPosEvent(int lAxisNo, double dPos, double dVel, double dAccel, double dDecel, int lEventAxisNo, double dComparePosition, uint uPositionSource);

        // 지정 축을 설정한 감속도로 감속 정지 한다.
        // dDecel : 정지 시 감속율값
        [DllImport("AXL.dll")] public static extern uint AxmMoveStop(int nAxisNo, double dDecel);
        // 지정 축을 설정한 감속도로 감속 정지 한다.(PCI-Nx04 전용)
        // 현재 가감속 상태와 관계없이 즉시 감속 가능 함수이며 제한된 구동에 대하여 사용 가능하다.
        // -- 사용 가능 구동 : AxmMoveStartPos, AxmMoveVel, AxmLineMoveEx2.
        // dDecel : 정지 시 감속율값
        // 주의 : 감속율값은 최초 설정 감속율보다 크거나 같아야 한다.
        // 주의 : 감속 설정을 시간으로 하였을 경우 최초 설정 감속 시간보다 작거나 같아야 한다.
        [DllImport("AXL.dll")] public static extern uint AxmMoveStopEx(int lAxisNo, double dDecel);
        // 지정 축을 급 정지 한다.
        [DllImport("AXL.dll")] public static extern uint AxmMoveEStop(int nAxisNo);
        // 지정 축을 감속 정지한다.
        [DllImport("AXL.dll")] public static extern uint AxmMoveSStop(int nAxisNo);

        //========= 오버라이드 함수 =========================================================================================
        // 위치 오버라이드 한다.
        // 지정 축의 구동이 종료되기 전 지정된 출력 펄스 수를 조정한다.
        // 주의사항: 오버라이드할 위치를 넣을때는 구동 시점의 위치를 기준으로한 Relative 형태의 위치값으로 넣어준다.
        //           구동시작후 같은방향의 경우 오버라이드를 계속할수있지만 반대방향으로 오버라이드할경우에는 오버라이드를 계속할수없다.
        [DllImport("AXL.dll")] public static extern uint AxmOverridePos(int nAxisNo, double dOverridePos);

        // 지정 축의 속도오버라이드 하기전에 오버라이드할 최고속도를 설정한다.
        // 주의점 : 속도오버라이드를 5번한다면 그중에 최고 속도를 설정해야된다. 
        [DllImport("AXL.dll")] public static extern uint AxmOverrideSetMaxVel(int nAxisNo, double dOverrideMaxVel);
        // 속도 오버라이드 한다.
        // 지정 축의 구동 중에 속도를 가변 설정한다. (반드시 모션 중에 가변 설정한다.)
        // 주의점: AxmOverrideVel 함수를 사용하기전에. AxmOverrideMaxVel 최고로 설정할수있는 속도를 설정해놓는다.
        // EX> 속도오버라이드를 두번한다면 
        // 1. 두개중에 높은 속도를 AxmOverrideMaxVel 설정 최고 속도값 설정.
        // 2. AxmMoveStartPos 실행 지정 축의 구동 중(Move함수 모두 포함)에 속도를 첫번째 속도로 AxmOverrideVel 가변 설정한다.
        // 3. 지정 축의 구동 중(Move함수 모두 포함)에 속도를 두번째 속도로 AxmOverrideVel 가변 설정한다.
        [DllImport("AXL.dll")] public static extern uint AxmOverrideVel(int nAxisNo, double dOverrideVelocity);
        // 가속도, 속도, 감속도를  오버라이드 한다.
        // 지정 축의 구동 중에 가속도, 속도, 감속도를 가변 설정한다. (반드시 모션 중에 가변 설정한다.)
        // 주의점: AxmOverrideAccelVelDecel 함수를 사용하기전에. AxmOverrideMaxVel 최고로 설정할수있는 속도를 설정해놓는다.
        // EX> 속도오버라이드를 두번한다면 
        // 1. 두개중에 높은 속도를 AxmOverrideMaxVel 설정 최고 속도값 설정.
        // 2. AxmMoveStartPos 실행 지정 축의 구동 중(Move함수 모두 포함)에 가속도, 속도, 감속도를 첫번째 속도로 AxmOverrideAccelVelDecel 가변 설정한다.
        // 3. 지정 축의 구동 중(Move함수 모두 포함)에 가속도, 속도, 감속도를 두번째 속도로 AxmOverrideAccelVelDecel 가변 설정한다.
        [DllImport("AXL.dll")] public static extern uint AxmOverrideAccelVelDecel(int nAxisNo, double dOverrideVelocity, double dMaxAccel, double dMaxDecel);
        // 어느 시점에서 속도 오버라이드 한다.
        // 어느 위치 지점과 오버라이드할 속도를 입력시켜 그위치에서 속도오버라이드 되는 함수
        // lTarget : COMMAND(0), ACTUAL(1)
        // 주의점: AxmOverrideVelAtPos 함수를 사용하기전에. AxmOverrideMaxVel 최고로 설정할수있는 속도를 설정해놓는다.
        [DllImport("AXL.dll")] public static extern uint AxmOverrideVelAtPos(int nAxisNo, double dPos, double dVel, double dAccel, double dDecel, double dOverridePos, double dOverrideVelocity, int nTarget);
        // 지정한 시점들에서 지정한 속도로 오버라이드 한다.
        // lArraySize     : 오버라이드 할 위치의 개수를 설정.
        // *dpOverridePos : 오버라이드 할 위치의 배열(lArraySize에서 설정한 개수보다 같거나 크게 선언해야됨)
        // *dpOverrideVel : 오버라이드 할 위치에서 변경 될 속도 배열(lArraySize에서 설정한 개수보다 같거나 크게 선언해야됨)
        // lTarget        : COMMAND(0), ACTUAL(1) 
        // uOverrideMode : 오버라이드 시작 방법을 지정함.
        //                : OVERRIDE_POS_START(0) 지정한 위치에서 지정한 속도로 오버라이드 시작함        
        //                : OVERRIDE_POS_END(1) 지정한 위치에서 지정한 속도가 되도록 미리 오버라이드 시작함
        [DllImport("AXL.dll")] public static extern uint AxmOverrideVelAtMultiPos(int nAxisNo, double dPos, double dVel, double dAccel, double dDecel, int nArraySize, double[] dpOverridePos, double[] dpOverrideVel, int nTarget, uint uOverrideMode);

        // 지정한 시점들에서 지정한 속도/가감속도로 오버라이드 한다.(MLII 전용)
        // lArraySize     : 오버라이드 할 위치의 개수를 설정(최대 5).
        // *dpOverridePos : 오버라이드 할 위치의 배열(lArraySize에서 설정한 개수보다 같거나 크게 선언해야됨)
        // *dpOverrideVel : 오버라이드 할 위치에서 변경 될 속도 배열(lArraySize에서 설정한 개수보다 같거나 크게 선언해야됨)
        // *dpOverrideAccelDecel : 오버라이드 할 위치에서 변경 될 가감속도 배열(lArraySize에서 설정한 개수보다 같거나 크게 선언해야됨)
        // lTarget        : COMMAND(0), ACTUAL(1) 
        // dwOverrideMode : 오버라이드 시작 방법을 지정함.
        //                : OVERRIDE_POS_START(0) 지정한 위치에서 지정한 속도로 오버라이드 시작함  
        //                : OVERRIDE_POS_END(1) 지정한 위치에서 지정한 속도가 되도록 미리 오버라이드 시작함
        [DllImport("AXL.dll")] public static extern uint AxmOverrideVelAtMultiPos2(int lAxisNo, double dPos, double dVel, double dAccel, double dDecel, int lArraySize, double[] dpOverridePos, double[] dpOverrideVel, double[] dpOverrideAccelDecel, int lTarget, uint dwOverrideMode);

        // 다축을 동시에 속도 오버라이드 한다.
        // 주의점: 함수를 사용하기전에. AxmOverrideMaxVel 최고로 설정할수있는 속도를 설정해놓는다.
        // lArraySzie     : 오버라이드 할 축의 개수
        // lpAxisNo       : 오버라이드 할 축의 배열
        // dpOveerrideVel : 오버라이드 할 속도 배열
        [DllImport("AXL.dll")] public static extern uint AxmOverrideMultiVel(int lArraySize, long[] lpAxisNo, double[] dpOverrideVel);

        //========= 마스터, 슬레이브  기어비로 구동 함수 ====================================================================
        // Electric Gear 모드에서 Master 축과 Slave 축과의 기어비를 설정한다.
        // dSlaveRatio : 마스터축에 대한 슬레이브의 기어비( 0 : 0% , 0.5 : 50%, 1 : 100%)
        [DllImport("AXL.dll")] public static extern uint AxmLinkSetMode(int nMasterAxisNo, int nSlaveAxisNo, double dSlaveRatio);
        // Electric Gear 모드에서 설정된 Master 축과 Slave 축과의 기어비를 반환한다.
        [DllImport("AXL.dll")] public static extern uint AxmLinkGetMode(int nMasterAxisNo, ref uint nSlaveAxisNo, ref double dpGearRatio);
        // Master 축과 Slave축간의 전자기어비를 설정 해제 한다.
        [DllImport("AXL.dll")] public static extern uint AxmLinkResetMode(int nMasterAxisNo);

        //======== 겐트리 관련 함수==========================================================================================
        // 모션모듈은 두 축이 기구적으로 Link되어있는 겐트리 구동시스템 제어를 지원한다. 
        // 이 함수를 이용해 Master축을 겐트리 제어로 설정하면 해당 Slave축은 Master축과 동기되어 구동됩니다. 
        // 만약 겐트리 설정 이후 Slave축에 구동명령이나 정지 명령등을 내려도 모두 무시됩니다.
        // uSlHomeUse     : 슬레이축 홈사용 우뮤 ( 0 - 2)
        //             (0 : 슬레이브축 홈을 사용안하고 마스터축을 홈을 찾는다.)
        //             (1 : 마스터축 , 슬레이브축 홈을 찾는다. 슬레이브 dSlOffset 값 적용해서 보정함.)
        //             (2 : 마스터축 , 슬레이브축 홈을 찾는다. 슬레이브 dSlOffset 값 적용해서 보정안함.)
        // dSlOffset      : 슬레이브축 옵셋값
        // dSlOffsetRange : 슬레이브축 옵셋값 레인지 설정
        // 주의사항       : 갠트리 ENABLE시 슬레이브축은 모션중 AxmStatusReadMotion 함수로 확인하면 True(Motion 구동 중)로 확인되야 정상동작이다. 
        //                  슬레이브축에 AxmStatusReadMotion로 확인했을때 InMotion 이 False이면 Gantry Enable이 안된것이므로 알람 혹은 리밋트 센서 등을 확인한다.
        [DllImport("AXL.dll")] public static extern uint AxmGantrySetEnable(int nMasterAxisNo, int nSlaveAxisNo, uint uSlHomeUse, double dSlOffset, double dSlOffsetRange);

        // Slave축의 Offset값을 알아내는방법.
        // A. 마스터, 슬레이브를 모두 서보온을 시킨다.                 
        // B. AxmGantrySetEnable함수에서 uSlHomeUse = 2로 설정후 AxmHomeSetStart함수를 이용해서 홈을 찾는다. 
        // C. 홈을 찾고 나면 마스터축의 Command값을 읽어보면 마스터축과 슬레이브축의 틀어진 Offset값을 볼수있다.
        // D. Offset값을 읽어서 AxmGantrySetEnable함수의 dSlOffset인자에 넣어준다. 
        // E. dSlOffset값을 넣어줄때 마스터축에 대한 슬레이브 축 값이기때문에 부호를 반대로 -dSlOffset 넣어준다.
        // F. dSIOffsetRange 는 Slave Offset의 Range 범위를 말하는데 Range의 한계를 지정하여 한계를 벗어나면 에러를 발생시킬때 사용한다.
        // G. AxmGantrySetEnable함수에 Offset값을 넣어줬으면  AxmGantrySetEnable함수에서 uSlHomeUse = 1로 설정후 AxmHomeSetStart함수를 이용해서 홈을 찾는다.

        // 겐트리 구동에 있어 사용자가 설정한 파라메타를 반환한다.
        [DllImport("AXL.dll")] public static extern uint AxmGantryGetEnable(int nMasterAxisNo, ref uint upSlHomeUse, ref double dpSlOffset, ref double dSlORange, ref uint uGatryOn);
        // 모션 모듈은 두 축이 기구적으로 Link되어있는 겐트리 구동시스템 제어를 해제한다.
        [DllImport("AXL.dll")] public static extern uint AxmGantrySetDisable(int nMasterAxisNo, int nSlaveAxisNo);

        // PCI-Rxx04-MLII 전용.
        // 모션 모듈은 두 축이 기구적으로 Link되어있는 겐트리 구동시스템 제어 중 동기 보상 기능을 설정한다.
        // lMasterGain, lSlaveGain : 두 축간 위치 편차에 대한 보상 값 반영 비율을 % 값으로 입력한다.
        // lMasterGain, lSlaveGain : 0을 입력하면 두 축간 위치 편차 보상 기능을 사용하지 않음. 기본값 : 0%
        [DllImport("AXL.dll")] public static extern uint AxmGantrySetCompensationGain(int lMasterAxisNo, int lMasterGain, int lSlaveGain);
        // 모션 모듈은 두 축이 기구적으로 Link되어있는 겐트리 구동시스템 제어 중 동기 보상 기능을 설정을 확인한다.
        [DllImport("AXL.dll")] public static extern uint AxmGantryGetCompensationGain(int lMasterAxisNo, ref int lMasterGain, ref int lSlaveGain);

        // Master 와 Slave 간 오차 범위를 설정 하고 오차범위 이상이면 Read 함수의 Status에 TRUE를 반환 한다.
        // PCI-R1604 / PCI-R3200-MLIII 전용 함수
        // lMasterAxisNo : Gantry Master Axis No
        // dErrorRange : 오차 범위 설정 값 0~2147483647 (양수만 입력 음수 입력시 1170 Error Code Return)
        // uUse : 모드 설정
        //      ( 0 : Disable)
        //      ( 1 : User 감시 모드)
        //      ( 2 : Flag Latch 모드)
        //      ( 3 : Flag Latch 모드 + Error 발생시 SSTOP)
        //      ( 4 : Flag Latch 모드 + Error 발생시 ESTOP)
        [DllImport("AXL.dll")] public static extern uint AxmGantrySetErrorRange(int lMasterAxisNo, double dErrorRange, uint uUse);
        // Master 와 Slave 간의 오차 범위 설정값을 반환한다.
        [DllImport("AXL.dll")] public static extern uint AxmGantryGetErrorRange(int lMasterAxisNo, ref double dpErrorRange, ref uint upUse);
        // Master 와 Slave 간의 오차값 비교 결과를 반환 한다.
        // dwpStatus : FALSE(0) -> Master 와 Slave 사이의 오차범위가 설정한 오차범위 보다 작다. (정상상태)
        //             TRUE(1) -> Master 와 Slave 사이의 오차범위가 설정한 오차범위 보다 크다. (비정상상태)
        // AxmGantryReadErrorRangeStatus 함수의 경우 InMotion && Gantry Enable && Master/Slave Servo On 상태를 만족 할 때만
        // AXT_RT_SUCCESS를 Return 하며 위의 상태를 만족하지 않으면 Error Code를 Return 한다.
        [DllImport("AXL.dll")] public static extern uint AxmGantryReadErrorRangeStatus(int lMasterAxisNo, ref uint dwpStatus);
        // Master 와 Slave 간의 오차값을 반환 한다.
        // Flag Latch 모드 일때 Latch 된 값을 사용자가 읽어 간 후 다음 Error가 발생 되기 전까지 Error가 발생 했을 때의 값을 유지 하게 됩니다.
        [DllImport("AXL.dll")] public static extern uint AxmGantryReadErrorRangeComparePos(int lMasterAxisNo, ref double dpComparePos);

        //====일반 보간함수 =================================================================================================
        // 주의사항1: AxmContiSetAxisMap함수를 이용하여 축맵핑후에 낮은순서축부터 맵핑을 하면서 사용해야된다.
        //           원호보간의 경우에는 반드시 낮은순서축부터 축배열에 넣어야 동작 가능하다.

        // 주의사항2: 위치를 설정할경우 반드시 마스터축과 슬레이브 축의 UNIT/PULSE의 맞추어서 설정한다.
        //           위치를 UNIT/PULSE 보다 작게 설정할 경우 최소단위가 UNIT/PULSE로 맞추어지기때문에 그위치까지 구동이 될수없다.

        // 주의사항3: 원호 보간을 할경우 반드시 한칩내에서 구동이 될수있으므로 
        //            4축내에서만 선택해서 사용해야된다.

        // 주의사항4: 보간 구동 시작/중에 비정상 정지 조건(+- Limit신호, 서보 알람, 비상정지 등)이 발생하면 
        //            구동 방향에 상관없이 구동을 시작하지 않거나 정지 된다.

        // 직선 보간 한다.
        // 시작점과 종료점을 지정하여 다축 직선 보간 구동하는 함수이다. 구동 시작 후 함수를 벗어난다.
        // AxmContiBeginNode, AxmContiEndNode와 같이사용시 지정된 좌표계에 시작점과 종료점을 지정하여 직선 보간 구동하는 Queue에 저장함수가된다. 
        // 직선 프로파일 연속 보간 구동을 위해 내부 Queue에 저장하여 AxmContiStart함수를 사용해서 시작한다.
        [DllImport("AXL.dll")] public static extern uint AxmLineMove(int lCoord, double[] dPos, double dVel, double dAccel, double dDecel);

        // 2축 단위 직선 보간 한다.(Software 방식)
        // 시작점과 종료점을 지정하여 다축 직선 보간 구동하는 함수이다. 구동 시작 후 함수를 벗어난다.
        [DllImport("AXL.dll")] public static extern uint AxmLineMoveEx2(int lCoord, double[] dpEndPos, double dVel, double dAccel, double dDecel);

        // 2축 원호보간 한다.
        // 시작점, 종료점과 중심점을 지정하여 원호 보간 구동하는 함수이다. 구동 시작 후 함수를 벗어난다.
        // AxmContiBeginNode, AxmContiEndNode, 와 같이사용시 지정된 좌표계에 시작점, 종료점과 중심점을 지정하여 구동하는 원호 보간 Queue에 저장함수가된다.
        // 프로파일 원호 연속 보간 구동을 위해 내부 Queue에 저장하여 AxmContiStart함수를 사용해서 시작한다.
        // lAxisNo = 두축 배열 , dCenterPos = 중심점 X,Y 배열 , dEndPos = 종료점 X,Y 배열.
        // uCWDir   DIR_CCW(0): 반시계방향, DIR_CW(1) 시계방향
        [DllImport("AXL.dll")] public static extern uint AxmCircleCenterMove(int lCoord, int[] lAxisNo, double[] dCenterPos, double[] dEndPos, double dVel, double dAccel, double dDecel, uint uCWDir);

        // 중간점, 종료점을 지정하여 원호 보간 구동하는 함수이다. 구동 시작 후 함수를 벗어난다.
        // AxmContiBeginNode, AxmContiEndNode와 같이사용시 지정된 좌표계에 중간점, 종료점을 지정하여 구동하는 원호 보간 Queue에 저장함수가된다.
        // 프로파일 원호 연속 보간 구동을 위해 내부 Queue에 저장하여 AxmContiStart함수를 사용해서 시작한다.
        // lAxisNo = 두축 배열 , dMidPos = 중간점 X,Y 배열 , dEndPos = 종료점 X,Y 배열, lArcCircle = 아크(0), 원(1)
        [DllImport("AXL.dll")] public static extern uint AxmCirclePointMove(int lCoord, int[] lAxisNo, double[] dMidPos, double[] dEndPos, double dVel, double dAccel, double dDecel, int lArcCircle);

        // 시작점, 종료점과 반지름을 지정하여 원호 보간 구동하는 함수이다. 구동 시작 후 함수를 벗어난다.
        // AxmContiBeginNode, AxmContiEndNode와 같이사용시 지정된 좌표계에 시작점, 종료점과 반지름을 지정하여 원호 보간 구동하는 Queue에 저장함수가된다.
        // 프로파일 원호 연속 보간 구동을 위해 내부 Queue에 저장하여 AxmContiStart함수를 사용해서 시작한다.
        // lAxisNo = 두축 배열 , dRadius = 반지름, dEndPos = 종료점 X,Y 배열 , uShortDistance = 작은원(0), 큰원(1)
        // uCWDir   DIR_CCW(0): 반시계방향, DIR_CW(1) 시계방향
        [DllImport("AXL.dll")] public static extern uint AxmCircleRadiusMove(int lCoord, int[] lAxisNo, double dRadius, double[] dEndPos, double dVel, double dAccel, double dDecel, uint uCWDir, uint uShortDistance);

        // 시작점, 회전각도와 반지름을 지정하여 원호 보간 구동하는 함수이다. 구동 시작 후 함수를 벗어난다.
        // AxmContiBeginNode, AxmContiEndNode와 같이사용시 지정된 좌표계에 시작점, 회전각도와 반지름을 지정하여 원호 보간 구동하는 Queue에 저장함수가된다.
        // 프로파일 원호 연속 보간 구동을 위해 내부 Queue에 저장하여 AxmContiStart함수를 사용해서 시작한다.
        // lAxisNo = 두축 배열 , dCenterPos = 중심점 X,Y 배열 , dAngle = 각도.
        // uCWDir   DIR_CCW(0): 반시계방향, DIR_CW(1) 시계방향
        [DllImport("AXL.dll")] public static extern uint AxmCircleAngleMove(int lCoord, int[] lAxisNo, double[] dCenterPos, double dAngle, double dVel, double dAccel, double dDecel, uint uCWDir);

        //====연속 보간 함수 ================================================================================================
        // 지정된 좌표계에 연속보간 축 맵핑을 설정한다.
        // (축맵핑 번호는 0 부터 시작))
        // 주의점: 축맵핑할때는 반드시 실제 축번호가 작은 숫자부터 큰숫자를 넣는다.
        //         가상축 맵핑 함수를 사용하였을 때 가상축번호를 실제 축번호가 작은 값 부터 lpAxesNo의 낮은 인텍스에 입력하여야 한다.
        //         가상축 맵핑 함수를 사용하였을 때 가상축번호에 해당하는 실제 축번호가 다른 값이라야 한다.
        //         같은 축을 다른 Coordinate에 중복 맵핑하지 말아야 한다.
        [DllImport("AXL.dll")] public static extern uint AxmContiSetAxisMap(int lCoord, uint lSize, int[] lpRealAxesNo);
        //지정된 좌표계에 연속보간 축 맵핑을 반환한다.
        [DllImport("AXL.dll")] public static extern uint AxmContiGetAxisMap(int lCoord, ref uint lSize, ref int lpRealAxesNo);

        // 지정된 좌표계에 연속보간 축 절대/상대 모드를 설정한다.
        // (주의점 : 반드시 축맵핑 하고 사용가능)
        // 지정 축의 이동 거리 계산 모드를 설정한다.
        // uAbsRelMode : POS_ABS_MODE '0' - 절대 좌표계
        //               POS_REL_MODE '1' - 상대 좌표계
        [DllImport("AXL.dll")] public static extern uint AxmContiSetAbsRelMode(int lCoord, uint uAbsRelMode);
        // 지정된 좌표계에 연속보간 축 절대/상대 모드를 반환한다.
        [DllImport("AXL.dll")] public static extern uint AxmContiGetAbsRelMode(int lCoord, ref uint upAbsRelMode);

        // 지정된 좌표계에 보간 구동을 위한 내부 Queue가 비어 있는지 확인하는 함수이다.
        [DllImport("AXL.dll")] public static extern uint AxmContiReadFree(int lCoord, ref uint upQueueFree);
        // 지정된 좌표계에 보간 구동을 위한 내부 Queue에 저장되어 있는 보간 구동 개수를 확인하는 함수이다.
        [DllImport("AXL.dll")] public static extern uint AxmContiReadIndex(int lCoord, ref int npQueueIndex);

        // 지정된 좌표계에 연속 보간 구동을 위해 저장된 내부 Queue를 모두 삭제하는 함수이다.
        [DllImport("AXL.dll")] public static extern uint AxmContiWriteClear(int lCoord);

        // 지정된 좌표계에 연속보간에서 수행할 작업들의 등록을 시작한다. 이함수를 호출한후,
        // AxmContiEndNode함수가 호출되기 전까지 수행되는 모든 모션작업은 실제 모션을 수행하는 것이 아니라 연속보간 모션으로 등록 되는 것이며,
        // AxmContiStart 함수가 호출될 때 비로소 등록된모션이 실제로 수행된다.
        [DllImport("AXL.dll")] public static extern uint AxmContiBeginNode(int lCoord);
        // 지정된 좌표계에서 연속보간을 수행할 작업들의 등록을 종료한다.
        [DllImport("AXL.dll")] public static extern uint AxmContiEndNode(int lCoord);

        // 연속 보간 시작 한다.
        // uProfileset(CONTI_NODE_VELOCITY(0) : 연속 보간 사용, CONTI_NODE_MANUAL(1) : 프로파일 보간 사용, CONTI_NODE_AUTO(2) : 자동 프로파일 보간, 3 : 속도보상 모드 사용) 
        [DllImport("AXL.dll")] public static extern uint AxmContiStart(int lCoord, uint uProfileset, int lAngle);
        // 지정된 좌표계에 연속 보간 구동 중인지 확인하는 함수이다.
        [DllImport("AXL.dll")] public static extern uint AxmContiIsMotion(int lCoord, ref uint upInMotion);

        // 지정된 좌표계에 연속 보간 구동 중 현재 구동중인 연속 보간 인덱스 번호를 확인하는 함수이다.
        [DllImport("AXL.dll")] public static extern uint AxmContiGetNodeNum(int lCoord, ref int npNodeNum);
        // 지정된 좌표계에 설정한 연속 보간 구동 총 인덱스 갯수를 확인하는 함수이다.
        [DllImport("AXL.dll")] public static extern uint AxmContiGetTotalNodeNum(int lCoord, ref int npNodeNum);

        [DllImport("AXL.dll")] public static extern uint AxmContiSetConnectionRadius(int lCoord, double dRadius);

        //====================트리거 함수 ===================================================================================
        // 주의사항: 트리거 위치를 설정할경우 반드시 UNIT/PULSE의 맞추어서 설정한다.
        //           위치를 UNIT/PULSE 보다 작게할 경우 최소단위가 UNIT/PULSE로 맞추어지기때문에 그위치에 출력할수없다.

        // 지정 축에 트리거 기능의 사용 여부, 출력 레벨, 위치 비교기, 트리거 신호 지속 시간 및 트리거 출력 모드를 설정한다.
        // 트리거 기능 사용을 위해서는 먼저  AxmTriggerSetTimeLevel 를 사용하여 관련 기능 설정을 먼저 하여야 한다.
        // dTrigTime : 트리거 출력 시간, 1usec - 최대 50msec ( 1 - 50000 까지 설정)
        // upTriggerLevel  : 트리거 출력 레벨 유무 => LOW(0), HIGH(1)
        // uSelect         : 사용할 기준 위치      => COMMAND(0), ACTUAL(1)
        // uInterrupt      : 인터럽트 설정         => DISABLE(0), ENABLE(1)

        // 지정 축에 트리거 신호 지속 시간 및 트리거 출력 레벨, 트리거 출력방법을 설정한다.
        [DllImport("AXL.dll")] public static extern uint AxmTriggerSetTimeLevel(int lAxisNo, double dTrigTime, uint uTriggerLevel, uint uSelect, uint uInterrupt);
        // 지정 축에 트리거 신호 지속 시간 및 트리거 출력 레벨, 트리거 출력방법을 반환한다.
        [DllImport("AXL.dll")] public static extern uint AxmTriggerGetTimeLevel(int lAxisNo, ref double dTrigTime, ref uint uTriggerLevel, ref uint uSelect, ref uint uInterrupt);

        // 지정 축의 트리거 출력 기능을 설정한다.
        // uMethod : PERIOD_MODE  0x0 : 현재 위치를 기준으로 dPos를 위치 주기로 사용한 주기 트리거 방식
        //           ABS_POS_MODE 0x1 : 트리거 절대 위치에서 트리거 발생, 절대 위치 방식
        // dPos : 주기 선택시 : 위치마다위치마다 출력하기때문에 그 위치
        //        절대 선택시 : 출력할 그 위치, 이 위치와같으면 무조건 출력이 나간다. 
        // 주의사항: AxmTriggerSetAbsPeriod의 주기모드로 설정할경우 처음 그위치가 범위 안에 있으므로 트리거 출력이 한번 발생한다.
        [DllImport("AXL.dll")] public static extern uint AxmTriggerSetAbsPeriod(int nAxisNo, uint uMethod, double dPos);
        // 지정 축에 트리거 기능의 사용 여부, 출력 레벨, 위치 비교기, 트리거 신호 지속 시간 및 트리거 출력 모드를 반환한다.
        [DllImport("AXL.dll")] public static extern uint AxmTriggerGetAbsPeriod(int nAxisNo, ref uint upMethod, ref double dpPos);

        // 사용자가 지정한 시작위치부터 종료위치까지 일정구간마다 트리거를 출력 한다.
        [DllImport("AXL.dll")] public static extern uint AxmTriggerSetBlock(int nAxisNo, double dStartPos, double dEndPos, double dPeriodPos);
        // 'AxmTriggerSetBlock' 함수의 트리거 설정한 값을 읽는다..
        [DllImport("AXL.dll")] public static extern uint AxmTriggerGetBlock(int nAxisNo, ref double dpStartPos, ref double dpEndPos, ref double dpPeriodPos);

        // 사용자가 한 개의 트리거 펄스를 출력한다.
        [DllImport("AXL.dll")] public static extern uint AxmTriggerOneShot(int nAxisNo);
        // 사용자가 한 개의 트리거 펄스를 몇초후에 출력한다.
        [DllImport("AXL.dll")] public static extern uint AxmTriggerSetTimerOneshot(int nAxisNo, int mSec);
        // 절대위치 트리거 무한대 절대위치 출력한다.
        [DllImport("AXL.dll")] public static extern uint AxmTriggerOnlyAbs(int nAxisNo, int nTrigNum, double[] dTrigPos);
        // 트리거 설정을 리셋한다.
        [DllImport("AXL.dll")] public static extern uint AxmTriggerSetReset(int nAxisNo);

        // 지정한 위치에서 트리거 신호 출력을 시작/종료한다.(반복사용 시 함수 재호출 필요)
        // AxmTriggerSetTimeLevel 함수로 설정된 uTriggerLevel, uSelect 값을 기준으로 동작(dTrigTime 및 uInterrupt 값은 사용되지 않음)
        // dStartpos		: 트리거 출력을 시작하는 위치
        // dEndPos			: 트리거 출력을 종료하는 위치
        [DllImport("AXL.dll")] public static extern uint AxmTriggerSetPoint(int nAxisNo, double dStartPos, double dEndPos);

        // AxmTriggerSetPoint 함수로 설정한 값을 확인한다.
        // dStartpos		: 트리거 출력을 시작하는 위치
        // dEndPos			: 트리거 출력을 종료하는 위치
        [DllImport("AXL.dll")] public static extern uint AxmTriggerGetPoint(int nAxisNo, ref double dpStartPos, ref double dpEndPos);

        // AxmTriggerSetPoint 함수로 설정한 위치를 초기화한다.
        // 트리거 출력 도중 함수를 호출한 경우 트리거 출력을 종료한다.
        [DllImport("AXL.dll")] public static extern uint AxmTriggerSetPointClear(int nAxisNo);

        //======== CRC( 잔여 펄스 클리어 함수)===============================================================================
        // Level   : LOW(0), HIGH(1), UNUSED(2), USED(3)
        // uMethod : 잔여펄스 제거 출력 신호 펄스 폭 0 - 7까지 설정가능.
        //           0 : 30 uSec , 1 : 100 uSec, 2: 500 uSec, 3:1 mSec, 4:10 mSec, 5:50 mSec, 6:100 mSec
        //지정 축에 CRC 신호 사용 여부 및 출력 레벨을 설정한다.
        [DllImport("AXL.dll")] public static extern uint AxmCrcSetMaskLevel(int nAxisNo, uint uLevel, uint uMethod);
        // 지정 축의 CRC 신호 사용 여부 및 출력 레벨을 반환한다.
        [DllImport("AXL.dll")] public static extern uint AxmCrcGetMaskLevel(int nAxisNo, ref uint upLevel, ref uint upMethod);

        //uOnOff  : CRC 신호를 Program으로 발생 여부  (FALSE(0),TRUE(1))
        // 지정 축에 CRC 신호를 강제로 발생 시킨다.
        [DllImport("AXL.dll")] public static extern uint AxmCrcSetOutput(int nAxisNo, uint uOnOff);
        // 지정 축의 CRC 신호를 강제로 발생 여부를 반환한다.
        [DllImport("AXL.dll")] public static extern uint AxmCrcGetOutput(int nAxisNo, ref uint upOnOff);

        //======MPG(Manual Pulse Generation) 함수============================================================================
        // lInputMethod  : 0-3 까지 설정가능. 0:OnePhase, 1:사용 안함, 2:TwoPhase2, 3:TwoPhase4
        // lDriveMode    : 0만 설정가능 (0 :MPG 연속모드)
        // MPGPos        : MPG 입력신호마다 이동하는 거리
        // MPGdenominator: MPG(수동 펄스 발생 장치 입력)구동 시 나누기 값
        // dMPGnumerator : MPG(수동 펄스 발생 장치 입력)구동 시 곱하기 값
        // uNumerator    : 최대(1 에서    64) 까지 설정 가능
        // uDenominator  : 최대(1 에서  4096) 까지 설정 가능
        // dMPGdenominator = 4096, MPGnumerator=1 가 의미하는 것은 
        // MPG 한바퀴에 200펄스면 그대로 1:1로 1펄스씩 출력을 의미한다. 
        // 만약 dMPGdenominator = 4096, MPGnumerator=2 로 했을경우는 1:2로 2펄스씩 출력을 내보낸다는의미이다. 
        // 여기에 MPG PULSE = ((Numerator) * (Denominator)/ 4096 ) 칩내부에 출력나가는 계산식이다.
        // 주의사항     : AxmStatusReadInMotion 함수 실행 결과에 유의한다.  (AxmMPGReset 하기 전까지 정상 상태에서는 모션 구동 중 상태.)

        // 지정 축에 MPG 입력방식, 드라이브 구동 모드, 이동 거리, MPG 속도 등을 설정한다.
        [DllImport("AXL.dll")] public static extern uint AxmMPGSetEnable(int nAxisNo, int nInputMethod, int nDriveMode, double dMPGPos, double dVel, double dAccel);
        // 지정 축에 MPG 입력방식, 드라이브 구동 모드, 이동 거리, MPG 속도 등을 반환한다.
        [DllImport("AXL.dll")] public static extern uint AxmMPGGetEnable(int nAxisNo, ref int npInputMethod, ref int npDriveMode, ref double dpMPGPos, ref double dpVel);

        // PCI-Nx04 함수 전용.
        // 지정 축에 MPG 드라이브 구동 모드에서 한펄스당 이동할 펄스 비율을 설정한다.
        [DllImport("AXL.dll")] public static extern uint AxmMPGSetRatio(int nAxisNo, double dMPGnumerator, double dMPGdenominator);
        // 지정 축에 MPG 드라이브 구동 모드에서 한펄스당 이동할 펄스 비율을 반환한다.
        [DllImport("AXL.dll")] public static extern uint AxmMPGGetRatio(int nAxisNo, ref double dMPGnumerator, ref double dMPGdenominator);

        // 지정 축에 MPG 드라이브 설정을 해지한다.
        [DllImport("AXL.dll")] public static extern uint AxmMPGReset(int nAxisNo);

        //======= 헬리컬 이동 ===============================================================================================
        // 주의사항 : Helix를 연속보간 사용시 Spline, 직선보간과 원호보간을 같이 사용할수없다.

        // 지정된 좌표계에 시작점, 종료점과 중심점을 지정하여 헬리컬 보간 구동하는 함수이다.
        // AxmContiBeginNode, AxmContiEndNode와 같이사용시 지정된 좌표계에 시작점, 종료점과 중심점을 지정하여 헬리컬 연속보간 구동하는 함수이다. 
        // 원호 연속 보간 구동을 위해 내부 Queue에 저장하는 함수이다. AxmContiStart함수를 사용해서 시작한다. (연속보간 함수와 같이 이용한다)
        // dCenterPos = 중심점 X,Y  , dEndPos = 종료점 X,Y
        // uCWDir   DIR_CCW(0): 반시계방향, DIR_CW(1) 시계방향
        [DllImport("AXL.dll")] public static extern uint AxmHelixCenterMove(int lCoord, double dCenterXPos, double dCenterYPos, double dEndXPos, double dEndYPos, double dZPos, double dVel, double dAccel, double dDecel, uint uCWDir);

        // 지정된 좌표계에 시작점, 종료점과 반지름을 지정하여 헬리컬 보간 구동하는 함수이다. 
        // AxmContiBeginNode, AxmContiEndNode와 같이사용시 지정된 좌표계에 중간점, 종료점을 지정하여 헬리컬연속 보간 구동하는 함수이다. 
        // 원호 연속 보간 구동을 위해 내부 Queue에 저장하는 함수이다. AxmContiStart함수를 사용해서 시작한다. (연속보간 함수와 같이 이용한다.)
        // dMidPos = 중간점 X,Y  , dEndPos = 종료점 X,Y
        [DllImport("AXL.dll")] public static extern uint AxmHelixPointMove(int lCoord, double dMidXPos, double dMidYPos, double dEndXPos, double dEndYPos, double dZPos, double dVel, double dAccel, double dDecel);

        // 지정된 좌표계에 시작점, 종료점과 반지름을 지정하여 헬리컬 보간 구동하는 함수이다.
        // AxmContiBeginNode, AxmContiEndNode와 같이사용시 지정된 좌표계에 시작점, 종료점과 반지름을 지정하여 헬리컬연속 보간 구동하는 함수이다. 
        // 원호 연속 보간 구동을 위해 내부 Queue에 저장하는 함수이다. AxmContiStart함수를 사용해서 시작한다. (연속보간 함수와 같이 이용한다.)
        // dRadius = 반지름, dEndPos = 종료점 X,Y  , uShortDistance = 작은원(0), 큰원(1)
        // uCWDir   DIR_CCW(0): 반시계방향, DIR_CW(1) 시계방향
        [DllImport("AXL.dll")] public static extern uint AxmHelixRadiusMove(int lCoord, double dRadius, double dEndXPos, double dEndYPos, double dZPos, double dVel, double dAccel, double dDecel, uint uCWDir, uint uShortDistance);

        // 지정된 좌표계에 시작점, 회전각도와 반지름을 지정하여 헬리컬 보간 구동하는 함수이다
        // AxmContiBeginNode, AxmContiEndNode와 같이사용시 지정된 좌표계에 시작점, 회전각도와 반지름을 지정하여 헬리컬연속 보간 구동하는 함수이다. 
        // 원호 연속 보간 구동을 위해 내부 Queue에 저장하는 함수이다. AxmContiStart함수를 사용해서 시작한다. (연속보간 함수와 같이 이용한다.)
        //dCenterPos = 중심점 X,Y  , dAngle = 각도.
        // uCWDir   DIR_CCW(0): 반시계방향, DIR_CW(1) 시계방향
        [DllImport("AXL.dll")] public static extern uint AxmHelixAngleMove(int lCoord, double dCenterXPos, double dCenterYPos, double dAngle, double dZPos, double dVel, double dAccel, double dDecel, uint uCWDir);

        //======== 스플라인 이동 ============================================================================================
        // 주의사항 : Spline를 연속보간 사용시 Helix , 직선보간과 원호보간을 같이 사용할수없다.

        // AxmContiBeginNode, AxmContiEndNode와 같이사용안함. 
        // 스플라인 연속 보간 구동하는 함수이다. 원호 연속 보간 구동을 위해 내부 Queue에 저장하는 함수이다.
        // AxmContiStart함수를 사용해서 시작한다. (연속보간 함수와 같이 이용한다.)
        // lPosSize : 최소 3개 이상.
        // 2축으로 사용시 dPoZ값을 0으로 넣어주면 됨.
        // 3축으로 사용시 축맵핑을 3개및 dPosZ 값을 넣어준다.
        [DllImport("AXL.dll")] public static extern uint AxmSplineWrite(int lCoord, int lPosSize, double[] dPosX, double[] dPosY, double dVel, double dAccel, double dDecel, double dPosZ, int lPointFactor);

        //======== PCI-R1604-MLII/SIIIH, PCIe-Rxx04-SIIIH 전용 함수 ================================================================================== 
        // 위치 보정 테이블 기능에 필요한 내용을 설정한다.
        [DllImport("AXL.dll")] public static extern uint AxmCompensationSet(int nAxisNo, int nNumEntry, double dStartPos, double[] dpPosition, double[] dpCorrection, uint uRollOver);
        // 위치 보정 테이블 기능 설정 내용을 반환한다.
        [DllImport("AXL.dll")] public static extern uint AxmCompensationGet(int nAxisNo, ref int npNumEntry, ref double dpStartPos, ref double dpPosition, ref double dpCorrection, ref uint upRollOver);

        // 위치 보정 테이블 기능의 사용유부를 설정한다.
        [DllImport("AXL.dll")] public static extern uint AxmCompensationEnable(int nAxisNo, uint uEnable);
        // 위치 보정 테이블 기능의 사용유무에 대한 설정 상태를 반환한다.
        [DllImport("AXL.dll")] public static extern uint AxmCompensationIsEnable(int nAxisNo, ref uint upEnable);
        // 현재 지령 위치에서의 보정값을 반환한다.
        [DllImport("AXL.dll")] public static extern uint AxmCompensationGetCorrection(int lAxisNo, ref double dpCorrection);


        // Backlash에 관련된 설정을하는 함수
        // > lBacklashDir: Backlash 보상을 적용할 구동 방향을 설정 (원점검색 방향과 동일하게 설정함)  
        //   - [0] -> Command Position값이 (+)방향으로 구동할 때 지정한 backlash를 적용함 
        //   - [1] -> Command Position값이 (-)방향으로 구동할 때 지정한 backlash를 적용함
        //   - Ex1) lBacklashDir이 0, backlash가 0.01일 때 0.0 -> 100.0으로 위치이동 할 때 실제 이동하는 위치는 100.01이됨
        //   - Ex2) lBacklashDir이 0, backlash가 0.01일 때 0.0 -> -100.0으로 위치이동 할 때 실제 이동하는 위치는 -100.0이됨
        //   ※ NOTANDUM 
        //   - 정확한 Backlash보상을 위해서는 원점검색시 마지막에 Backlash양 만큼 (+)Or(-)방향으로 이동 한 후 원점을 완료하고
        //     Backlash보정을 사용한다. 이 때 Backlash양 만큼 (+)구동을 했다면 backlash_dir을 [1](-)로, (-)구동을 했다면
        //     backlash_dir을 [0](+)로 설정하면 된다.
        // > dBacklash: 기구부에서 진행 방향과 반대반향으로 방향전환시 발생되는 Backlash양을 설정함
        // { RETURN VALUE } 
        //   - [0] -> Backlash 설정이 성공했을 때
        [DllImport("AXL.dll")] public static extern uint AxmCompensationSetBacklash(int lAxisNo, int lBacklashDir, double dBacklash);
        // Backlash에 관련된 설정 내용을 반환한다.
        [DllImport("AXL.dll")] public static extern uint AxmCompensationGetBacklash(int lAxisNo, ref int lpBacklashDir, ref double dpBacklash);
        // Backlash사용유무를 설정/확인하는 함수
        // > dwEnable: Backlash보정 사용유무를 지정
        //   - [0]DISABLE -> Backlash보정을 사용안함    
        //   - [1]ENABLE  -> Backlash보정을 사용함
        // { RETURN VALUE } 
        //   - [0] -> Backlash 설정반환이 성공했을 때
        //   - [4303] -> Backlash 보정기능이 설정되어있지않을 때
        [DllImport("AXL.dll")] public static extern uint AxmCompensationEnableBacklash(int lAxisNo, uint dwEnable);
        [DllImport("AXL.dll")] public static extern uint AxmCompensationIsEnableBacklash(int lAxisNo, ref uint dwpEnable);
        // Backlash보정기능을 사용할 때 Backlash양 만큼 좌우로 이동하여 기구물의 위치를 자동 정렬함(서보 온 동작 이후 한번 사용함)
        // > dVel: 이동 속도[unit / sec]
        // > dAccel: 이동가속도[unit / sec^2]
        // > dAccel: 이동감속도[unit / sec^2]
        // > dWaitTime: Backlash 양만큼 구동 후 원래의 위치로 되돌아올기 까지의 대기시간[msec]
        // { RETURN VALUE } 
        //   - [0]    -> Backlash 보정을 위한 위치설정이 성공했을 때
        //   - [4303] -> Backlash 보정기능이 설정되어있지않을 때
        [DllImport("AXL.dll")] public static extern uint AxmCompensationSetLocating(int lAxisNo, double dVel, double dAccel, double dDecel, double dWaitTime);

        // ECAM 기능에 필요한 내용을 설정한다.
        [DllImport("AXL.dll")] public static extern uint AxmEcamSet(int nAxisNo, int nMasterAxisNo, int nNumEntry, double dMasterStartPos, ref double dpMasterPos, ref double dpSlavePos);
        // ECAM 기능에 필요한 내용을 CMD/ACT Source와 함께 설정한다. (PCIe-Rxx04-SIIIH 전용 함수)
        [DllImport("AXL.dll")] public static extern uint AxmEcamSetWithSource(int lAxisNo, int lMasterAxis, int lNumEntry, double dMasterStartPos, ref double dpMasterPos, ref double dpSlavePos, uint dwSource);
        // ECAM 기능 설정 내용을 반환한다.
        [DllImport("AXL.dll")] public static extern uint AxmEcamGet(int nAxisNo, ref int npMasterAxisNo, ref int npNumEntry, ref double dpMasterStartPos, ref double dpMasterPos, ref double dpSlavePos);
        // ECAM 기능 설정 내용을 CMD/ACT Source와 함께 반환한다. (PCIe-Rxx04-SIIIH 전용 함수)
        [DllImport("AXL.dll")] public static extern uint AxmEcamGetWithSource(int lAxisNo, ref int lpMasterAxis, ref int lpNumEntry, ref double dpMasterStartPos, ref double dpMasterPos, ref double dpSlavePos, ref uint dwpSource);

        // ECAM 기능의 사용 유무를 설정한다.
        [DllImport("AXL.dll")] public static extern uint AxmEcamEnableBySlave(int nAxisNo, uint uEnable);
        // ECAM 기능의 사용 유무를 지정한 Master 축에 연결된 모든 Slave 축에 대하여 설정한다.
        [DllImport("AXL.dll")] public static extern uint AxmEcamEnableByMaster(int nAxisNo, uint uEnable);
        // ECAM 기능의 사용 유무에 대한 설정 상태를 반환한다.
        [DllImport("AXL.dll")] public static extern uint AxmEcamIsSlaveEnable(int nAxisNo, ref uint upEnable);

        //======== Servo Status Monitor =====================================================================================
        // 지정 축의 예외 처리 기능에 대해 설정한다.(MLII : Sigma-5, SIIIH : MR_J4_xxB 전용)
        // uSelMon(0~3) : 감시 정보 선택.
        //          [0] : Torque 
        //          [1] : Velocity of motor
        //          [2] : Accel. of motor
        //          [3] : Decel. of motor
        //          [4] : Position error between Cmd. position and Act. position.
        // uActionValue : 이상 동작 판정 기준 값 설정. 각 정보에 따라 설정 값의 의미가 다음.
        //          [0] : uSelMon에서 선택한 감시 정보에 대하여 예외 처리 하지 않음.
        //         [>0] : uSelMon에서 선택한 감시 정보에 대하여 예외 처리 기능 적용.
        // uAction(0~3) : uActionValue 이상으로 감시 정보가 확인되었을때 예외처리 방법 설정.
        //          [0] : Warning(setting flag only) 
        //          [1] : Warning(setting flag) + Slow-down stop
        //          [2] : Warning(setting flag) + Emergency stop
        //          [3] : Warning(setting flag) + Emergency stop + Servo-Off
        // ※ 주의: 5개의 SelMon 정보에 대해 각각 예외처리 설정이 가능하며, 사용중 예외처리를 원하지않을 경우
        //          반드시 해당 SelMon 정보의 ActionValue값을 0으로 설정해 감시기능을 Disable 해됨.
        [DllImport("AXL.dll")] public static extern uint AxmStatusSetServoMonitor(int nAxisNo, uint uSelMon, double dActionValue, uint uAction);
        // 지정 축의 예외 처리 기능에 대한 설정 상태를 반환한다.(MLII : Sigma-5, SIIIH : MR_J4_xxB 전용)
        [DllImport("AXL.dll")] public static extern uint AxmStatusGetServoMonitor(int nAxisNo, uint uSelMon, ref double dpActionValue, ref uint upAction);
        // 지정 축의 예외 처리 기능에 대한 사용 유무를 설정한다.(MLII : Sigma-5, SIIIH : MR_J4_xxB 전용)
        [DllImport("AXL.dll")] public static extern uint AxmStatusSetServoMonitorEnable(int nAxisNo, uint uEnable);
        // 지정 축의 예외 처리 기능에 대한 사용 유무를 반환한다.(MLII : Sigma-5, SIIIH : MR_J4_xxB 전용)
        [DllImport("AXL.dll")] public static extern uint AxmStatusGetServoMonitorEnable(int nAxisNo, ref uint upEnable);

        // 지정 축의 예외 처리 기능 실행 결과 플래그 값을 반환한다. 함수 실행 후 자동 초기화.(MLII : Sigma-5, SIIIH : MR_J4_xxB 전용)
        [DllImport("AXL.dll")] public static extern uint AxmStatusReadServoMonitorFlag(int nAxisNo, uint uSelMon, ref uint upMonitorFlag, ref double dpMonitorValue);
        // 지정 축의 예외 처리 기능을 위한 감시 정보를 반환한다.(MLII : Sigma-5, SIIIH : MR_J4_xxB 전용)
        [DllImport("AXL.dll")] public static extern uint AxmStatusReadServoMonitorValue(int nAxisNo, uint uSelMon, ref double dpMonitorValue);
        // 지정 축의 부하율을 읽을 수 있도록 설정 합니다.(MLII : Sigma-5, SIIIH : MR_J4_xxB 전용)
        // (MLII, Sigma-5, dwSelMon : 0 ~ 2) ==
        //     [0] : Accumulated load ratio
        //     [1] : Regenerative load ratio
        //     [2] : Reference Torque load ratio
        // (SIIIH, MR_J4_xxB, dwSelMon : 0 ~ 4) ==
        //     [0] : Assumed load inertia ratio(0.1times)
        //     [1] : Regeneration load factor(%)
        //     [2] : Effective load factor(%)
        //     [3] : Peak load factor(%)
        //     [4] : Current feedback(0.1%)	
        [DllImport("AXL.dll")] public static extern uint AxmStatusSetReadServoLoadRatio(int lAxisNo, uint dwSelMon);
        // 지정 축의 부하율을 반환한다.(MLII : Sigma-5, SIIIH : MR_J4_xxB 전용)
        [DllImport("AXL.dll")] public static extern uint AxmStatusReadServoLoadRatio(int lAxisNo, ref double dpMonitorValue);

        //======== PCI-R1604-RTEX 전용 함수==================================================================================
        // RTEX A4Nx 관련 Scale Coefficient를 설정한다.(RTEX, A4Nx 전용)
        [DllImport("AXL.dll")] public static extern uint AxmMotSetScaleCoeff(int nAxisNo, int lScaleCoeff);
        // RTEX A4Nx 관련 Scale Coefficient 를 확인한다.(RTEX, A4Nx 전용)
        [DllImport("AXL.dll")] public static extern uint AxmMotGetScaleCoeff(int nAxisNo, ref int lpScaleCoeff);
        // 특정 Input 신호의 Edge를 검출하여 즉정지 또는 감속정지하는 함수.
        // lDetect Signal : edge 검출할 입력 신호 선택.
        // lDetectSignal  : PosEndLimit(0), NegEndLimit(1), HomeSensor(4), EncodZPhase(5), UniInput02(6), UniInput03(7)
        // Signal Edge    : 선택한 입력 신호의 edge 방향 선택 (rising or falling edge).
        //                  SIGNAL_DOWN_EDGE(0), SIGNAL_UP_EDGE(1)
        // 구동방향      : Vel값이 양수이면 CW, 음수이면 CCW.
        // SignalMethod  : 급정지 EMERGENCY_STOP(0), 감속정지 SLOWDOWN_STOP(1)
        // 주의사항: SignalMethod를 EMERGENCY_STOP(0)로 사용할경우 가감속이 무시되며 지정된 속도로 가속 급정지하게된다.
        //          PCI-Nx04를 사용할 경우 lDetectSignal이 PosEndLimit , NegEndLimit(0,1) 을 찾을경우 신호의레벨 Active 상태를 검출하게된다.
        [DllImport("AXL.dll")] public static extern uint AxmMoveSignalSearchEx(int nAxisNo, double dVel, double dAccel, int nDetectSignal, int nSignalEdge, int nSignalMethod);
        //-------------------------------------------------------------------------------------------------------------------

        //======== PCI-R1604-MLII/SIIIH, PCIe-Rxx04-SIIIH 전용 함수 ==================================================================================
        // 설정한 절대 위치로 이동한다.
        // 속도 프로파일은 사라디꼴 전용으로 구동한다.
        // 펄스가 출력되는 시점에서 함수를 벗어난다.
        // 항상 위치 및 속도, 가감속도를 변경 가능하며, 반대방향 위치 변경 기능을 포함한다.
        [DllImport("AXL.dll")] public static extern uint AxmMoveToAbsPos(int lAxisNo, double dPos, double dVel, double dAccel, double dDecel);
        // 지정 축의 현재 구동 속도를 읽어온다.
        [DllImport("AXL.dll")] public static extern uint AxmStatusReadVelEx(int lAxisNo, ref double dpVel);
        //-------------------------------------------------------------------------------------------------------------------

        //========  PCI-R1604-SIIIH, PCIe-Rxx04-SIIIH 전용 함수 ==================================================================================
        // 지정 축의 전자 기어비를 설정한다. 설정 후 비 휘발성 메모리에 기억됩니다.
        // 초기 값(lNumerator : 4194304(2^22), lDenominator : 10000)
        // MR-J4-B는 전자 기어비를 설정할 수 없으며, 상위 제어기에서 아래의 함수를 사용하여 설정하여야 합니다.
        // 기존 펄스 입력 방식 서보 드라이버(MR-J4-A)의 파라미터 No.PA06, No.PA07에 해당.
        // ex1) 1 um를 제어 단위로 가정. 감속기 비율 : 1/1. Rotary motor를 장착한 Linear stage.
        // Encoder resulotion = 2^22
        // Ball screw pitch : 6 mm
        // ==> lNumerator = 2^22, lDenominator = 6000(6/0.001)
        // AxmMotSetMoveUnitPerPulse에서 Unit/Pulse = 1/1로 설정하였다면, 모든 함수의 위치 단위 : um, 속도 단위 : um/sec, 가감속도 단뒤 : um/sec^2이 된다.
        // AxmMotSetMoveUnitPerPulse에서 Unit/Pulse = 1/1000로 설정하였다면, 모든 함수의 위치 단위 : mm, 속도 단위 : mm/sec, 가감속도 단뒤 : mm/sec^2이 된다.
        // ex2) 0.01도 회전을 제어 단위로 가정. 감속기 비율 : 1/1. Rotary motor를 장착한 회전체 구조물.
        // Encoder resulotion = 2^22
        // 1 회전 : 360
        // ==> lNumerator = 2^22, lDenominator = 36000(360 / 0.01)
        // AxmMotSetMoveUnitPerPulse에서 Unit/Pulse = 1/1로 설정하였다면, 모든 함수의 위치 단위 : 0.01도, 속도 단위 : 0.01도/sec, 가감속도 단뒤 : 0.01도/sec^2이 된다.
        // AxmMotSetMoveUnitPerPulse에서 Unit/Pulse = 1/100로 설정하였다면, 모든 함수의 위치 단위 : 1도, 속도 단위 : 1도/sec, 가감속도 단뒤 : 1도/sec^2이 된다.
        [DllImport("AXL.dll")] public static extern uint AxmMotSetElectricGearRatio(int lAxisNo, int lNumerator, int lDenominator);
        // 지정 축의 전자 기어비 설정을 확인한다.
        [DllImport("AXL.dll")] public static extern uint AxmMotGetElectricGearRatio(int lAxisNo, ref int lpNumerator, ref int lpDenominator);

        // 지정 축의 토크 리미트 값을 설정 합니다.
        // 정방향, 역방향 구동시의 토크 값을 제한하는 함수. 
        // 설정 값은 1 ~ 1000까지 설정
        // 최대 토크의 0.1% 단위로 제어 함.
        [DllImport("AXL.dll")] public static extern uint AxmMotSetTorqueLimit(int lAxisNo, double dbPluseDirTorqueLimit, double dbMinusDirTorqueLimit);

        // 지정 축의 토크 리미트 값을 확인 합니다.
        // 정방향, 역방향 구동시의 토크 값을 읽어 오는 함수.
        // 설정 값은 1 ~ 1000까지 설정
        // 최대 토크의 0.1% 단위로 제어 함.
        [DllImport("AXL.dll")] public static extern uint AxmMotGetTorqueLimit(int lAxisNo, ref double dbpPluseDirTorqueLimit, ref double dbpMinusDirTorqueLimit);

        // 지정 축의 AxmOverridePos에 대한 특수 기능 사용 유무를 설정한다.
        // dwUsage        : AxmOverridPos 적용 가능 위치 판단 기능 사용 유무.
        //                  DISABLE(0) : 특수 기능 사용하지 않음.
        //                  ENABLE(1) : AxmMoveStartPos 설정한 구동 중 위치 변경 가능 위치를 감속 거리의 lDecelPosRatio(%)을 기준으로 판단한다.
        // lDecelPosRatio : 감속 거리에 대한 %값.
        // dReserved      : 사용하지 않음.
        [DllImport("AXL.dll")] public static extern uint AxmOverridePosSetFunction(int lAxisNo, uint dwUsage, int lDecelPosRatio, double dReserved);
        // 지정 축의 AxmOverridePos에 대한 특수 기능 사용 유무를 확인 한다.
        [DllImport("AXL.dll")] public static extern uint AxmOverridePosGetFunction(int lAxisNo, ref uint dwpUsage, ref int lpDecelPosRatio, ref double dpReserved);

        //-------------------------------------------------------------------------------------------------------------------

        //======== PCI-R3200-MLIII 전용 함수==================================================================================
        // 잔류 진동 억제(VST) 특수 함수    
        // 사용전에 반드시 코디에 대해서 축을 할당을 해야하며, 코디 한개에 1개의 축만 맵핑을 해야한다.
        // 아래 함수 실행전에 반드시 Servo ON 상태에서 사용한다.
        // lCoordnate        : 입력 성형 적용 코디 번호를 입력한다. 각 보드별 첫번째 부터 10번째의 코디에 축을 할당해서 사용해야 한다.
        //                     MLIII 마스터 보드는 보드 번호를 기준으로 16 ~ 31까지 보드 별로 순차적으로 16씩 증가된다.
        //                     MLIII B/D 0 : 16 ~ 31
        //                     MLIII B/D 1 : 31 ~ 47
        // cISTSize          : 입력 성형 사용 주파수 개수에 대해서 입력한다. 1로 값을 고정해서 사용한다.
        // dbpFrequency,	 : 10H ~ 500Hz
        //                     1차 주파수 부터 순서데로 입력한다.(저주파부터 고주파).
        // dbpDampingRatio   : 0.001 ~ 0.9
        // dwpImpulseCount   : 2 ~ 5
        [DllImport("AXL.dll")] public static extern uint AxmAdvVSTSetParameter(int lCoordinate, uint dwISTSize, double[] dbpFrequency, double[] dbpDampingRatio, ref uint dwpImpulseCount);
        [DllImport("AXL.dll")] public static extern uint AxmAdvVSTGetParameter(int lCoordinate, ref uint dwpISTSize, double[] dbpFrequency, double[] dbpDampingRatio, ref uint dwpImpulseCount);
        // lCoordnate        : 입력 성형 코디 번호를 입력한다.
        // dwISTEnable       : 입력 성형 사용 유무를 결정한다.
        [DllImport("AXL.dll")] public static extern uint AxmAdvVSTSetEnabele(int lCoordinate, uint dwISTEnable);
        [DllImport("AXL.dll")] public static extern uint AxmAdvVSTGetEnabele(int lCoordinate, ref uint dwISTEnable);

        //====일반 보간함수 =================================================================================================	
        // 직선 보간 한다.
        // 시작점과 종료점을 지정하여 다축 직선 보간 구동하는 함수이다. 구동 시작 후 함수를 벗어난다.
        // AxmAdvContiBeginNode, AxmAdvContiEndNode와 같이사용시 지정된 좌표계에 시작점과 종료점을 지정하여 직선 보간 구동하는 Queue에 저장함수가된다. 
        // 직선 프로파일 연속 보간 구동을 위해 내부 Queue에 저장하여 AxmAdvContiStart함수를 사용해서 시작한다.
        [DllImport("AXL.dll")] public static extern uint AxmAdvLineMove(int lCoordinate, double[] dPosition, double dMaxVelocity, double dStartVel, double dStopVel, double dMaxAccel, double dMaxDecel);
        // 지정된 좌표계에 시작점과 종료점을 지정하는 다축 직선 보간 오버라이드하는 함수이다.
        // 현재 진행중인 보간구동을 지정된 속도 및 위치로 직선 보간 오버라이드 하며, 다음 노드에 대한 직선 보간 구동 예약도 가능한다.
        // IOverrideMode = 0일 경우, 구동중인 보간이 직선, 원호 보간에 관계없이 현재 구동 노드에서 직선 보간으로 즉시 오버라이드 되고, 
        // IOverrideMode = 1이면 현재 구동 노드 다음의 노드부터 직선보간이 차례로 예약된다.
        // IOverrideMode = 1로 본 함수를 호출할때마다 최소 1개에서 최대 8개까지 오버라이드 큐에 증가되면서 자동적으로 예약이 되며, 예약 후 마지막에 IOverrideMode = 0으로 본 함수가 호출되면
        // 내부적으로 오버라이드 큐에 있는 예약 보간들이 연속보간 큐로 저장되고, 직선 오버라이드 구동과 이후의 예약된 연속보간이 순차적으로 실행된다.
        [DllImport("AXL.dll")] public static extern uint AxmAdvOvrLineMove(int lCoordinate, double[] dPosition, double dMaxVelocity, double dStartVel, double dStopVel, double dMaxAccel, double dMaxDecel, int lOverrideMode);
        // 2축 원호보간 한다.
        // 시작점, 종료점과 중심점을 지정하여 원호 보간 구동하는 함수이다. 구동 시작 후 함수를 벗어난다.
        // AxmAdvContiBeginNode, AxmAdvContiEndNode, 와 같이사용시 지정된 좌표계에 시작점, 종료점과 중심점을 지정하여 구동하는 원호 보간 Queue에 저장함수가된다.
        // 프로파일 원호 연속 보간 구동을 위해 내부 Queue에 저장하여 AxmAdvContiStart함수를 사용해서 시작한다.
        // lAxisNo = 두축 배열 , dCenterPos = 중심점 X,Y 배열 , dEndPos = 종료점 X,Y 배열.
        // uCWDir   DIR_CCW(0): 반시계방향, DIR_CW(1) 시계방향
        [DllImport("AXL.dll")] public static extern uint AxmAdvCircleCenterMove(int lCoord, int[] lAxisNo, double[] dCenterPos, double[] dEndPos, double dVel, double dStartVel, double dStopVel, double dAccel, double dDecel, uint uCWDir);
        // 중간점, 종료점을 지정하여 원호 보간 구동하는 함수이다. 구동 시작 후 함수를 벗어난다.
        // AxmAdvContiBeginNode, AxmAdvContiEndNode와 같이사용시 지정된 좌표계에 중간점, 종료점을 지정하여 구동하는 원호 보간 Queue에 저장함수가된다.
        // 프로파일 원호 연속 보간 구동을 위해 내부 Queue에 저장하여 AxmAdvContiStart함수를 사용해서 시작한다.
        // lAxisNo = 두축 배열 , dMidPos = 중간점 X,Y 배열 , dEndPos = 종료점 X,Y 배열, lArcCircle = 아크(0), 원(1)
        [DllImport("AXL.dll")] public static extern uint AxmAdvCirclePointMove(int lCoord, int[] lAxisNo, double[] dMidPos, double[] dEndPos, double dVel, double dStartVel, double dStopVel, double dAccel, double dDecel, int lArcCircle);
        // 시작점, 회전각도와 반지름을 지정하여 원호 보간 구동하는 함수이다. 구동 시작 후 함수를 벗어난다.
        // AxmAdvContiBeginNode, AxmAdvContiEndNode와 같이사용시 지정된 좌표계에 시작점, 회전각도와 반지름을 지정하여 원호 보간 구동하는 Queue에 저장함수가된다.
        // 프로파일 원호 연속 보간 구동을 위해 내부 Queue에 저장하여 AxmAdvContiStart함수를 사용해서 시작한다.
        // lAxisNo = 두축 배열 , dCenterPos = 중심점 X,Y 배열 , dAngle = 각도.
        // uCWDir   DIR_CCW(0): 반시계방향, DIR_CW(1) 시계방향
        [DllImport("AXL.dll")] public static extern uint AxmAdvCircleAngleMove(int lCoord, int[] lAxisNo, double[] dCenterPos, double dAngle, double dVel, double dStartVel, double dStopVel, double dAccel, double dDecel, uint uCWDir);
        // 시작점, 종료점과 반지름을 지정하여 원호 보간 구동하는 함수이다. 구동 시작 후 함수를 벗어난다.
        // AxmAdvContiBeginNode, AxmAdvContiEndNode와 같이사용시 지정된 좌표계에 시작점, 종료점과 반지름을 지정하여 원호 보간 구동하는 Queue에 저장함수가된다.
        // 프로파일 원호 연속 보간 구동을 위해 내부 Queue에 저장하여 AxmAdvContiStart함수를 사용해서 시작한다.
        // lAxisNo = 두축 배열 , dRadius = 반지름, dEndPos = 종료점 X,Y 배열 , uShortDistance = 작은원(0), 큰원(1)
        // uCWDir   DIR_CCW(0): 반시계방향, DIR_CW(1) 시계방향
        [DllImport("AXL.dll")] public static extern uint AxmAdvCircleRadiusMove(int lCoord, int[] lAxisNo, double dRadius, double[] dEndPos, double dVel, double dStartVel, double dStopVel, double dAccel, double dDecel, uint uCWDir, uint uShortDistance);
        // 지정된 좌표계에 시작점과 종료점을 지정하여 2축 원호 보간 오버라이드 구동한다.
        // 현재 진행중인 보간구동을 지정된 속도 및 위치로 원호 보간 오버라이드 하며, 다음 노드에 대한 원호 보간 구동 예약도 가능한다.
        // IOverrideMode = 0일 경우, 구동중인 보간이 직선, 원호 보간에 관계없이 현재 구동 노드에서 원호 보간으로 즉시 오버라이드 되고, 
        // IOverrideMode = 1이면 현재 구동 노드 다음의 노드부터 원호보간이 차례로 예약된다.
        // IOverrideMode = 1로 본 함수를 호출할때마다 최소 1개에서 최대 8개까지 오버라이드 큐에 증가되면서 자동적으로 예약이 되며, 예약 후 마지막에 IOverrideMode = 0으로 본 함수가 호출되면
        // 내부적으로 오버라이드 큐에 있는 예약 보간들이 연속보간 큐로 저장되고, 원호 오버라이드 구동과 이후의 예약된 연속보간이 순차적으로 실행된다.
        [DllImport("AXL.dll")] public static extern uint AxmAdvOvrCircleRadiusMove(int lCoord, int[] lAxisNo, double dRadius, double[] dEndPos, double dVel, double dStartVel, double dStopVel, double dAccel, double dDecel, uint uCWDir, uint uShortDistance, int lOverrideMode);

        //======= 헬리컬 이동 ===============================================================================================
        // 주의사항 : Helix를 연속보간 사용시 Spline, 직선보간과 원호보간을 같이 사용할수없다.

        // 지정된 좌표계에 시작점, 종료점과 중심점을 지정하여 헬리컬 보간 구동하는 함수이다.
        // AxmAdvContiBeginNode, AxmAdvContiEndNode와 같이사용시 지정된 좌표계에 시작점, 종료점과 중심점을 지정하여 헬리컬 연속보간 구동하는 함수이다. 
        // 원호 연속 보간 구동을 위해 내부 Queue에 저장하는 함수이다. AxmAdvContiStart함수를 사용해서 시작한다. (연속보간 함수와 같이 이용한다)
        // dCenterPos = 중심점 X,Y  , dEndPos = 종료점 X,Y .
        // uCWDir DIR_CCW(0): 반시계방향, DIR_CW(1) 시계방향    	
        [DllImport("AXL.dll")] public static extern uint AxmAdvHelixCenterMove(int lCoord, double dCenterXPos, double dCenterYPos, double dEndXPos, double dEndYPos, double dZPos, double dVel, double dStartVel, double dStopVel, double dAccel, double dDecel, uint uCWDir);
        // 지정된 좌표계에 시작점, 종료점과 반지름을 지정하여 헬리컬 보간 구동하는 함수이다. 
        // AxmAdvContiBeginNode, AxmAdvContiEndNode와 같이사용시 지정된 좌표계에 중간점, 종료점을 지정하여 헬리컬연속 보간 구동하는 함수이다. 
        // 원호 연속 보간 구동을 위해 내부 Queue에 저장하는 함수이다. AxmAdvContiStart함수를 사용해서 시작한다. (연속보간 함수와 같이 이용한다.)
        // dMidPos = 중간점 X,Y  , dEndPos = 종료점 X,Y 
        [DllImport("AXL.dll")] public static extern uint AxmAdvHelixPointMove(int lCoord, double dMidXPos, double dMidYPos, double dEndXPos, double dEndYPos, double dZPos, double dVel, double dStartVel, double dStopVel, double dAccel, double dDecel);
        // 지정된 좌표계에 시작점, 회전각도와 반지름을 지정하여 헬리컬 보간 구동하는 함수이다
        // AxmAdvContiBeginNode, AxmAdvContiEndNode와 같이사용시 지정된 좌표계에 시작점, 회전각도와 반지름을 지정하여 헬리컬연속 보간 구동하는 함수이다. 
        // 원호 연속 보간 구동을 위해 내부 Queue에 저장하는 함수이다. AxmAdvContiStart함수를 사용해서 시작한다. (연속보간 함수와 같이 이용한다.)
        //dCenterPos = 중심점 X,Y  , dAngle = 각도.
        // uCWDir   DIR_CCW(0): 반시계방향, DIR_CW(1) 시계방향
        [DllImport("AXL.dll")] public static extern uint AxmAdvHelixAngleMove(int lCoord, double dCenterXPos, double dCenterYPos, double dAngle, double dZPos, double dVel, double dStartVel, double dStopVel, double dAccel, double dDecel, uint uCWDir);
        // 지정된 좌표계에 시작점, 종료점과 반지름을 지정하여 헬리컬 보간 구동하는 함수이다.
        // AxmAdvContiBeginNode, AxmAdvContiEndNode와 같이사용시 지정된 좌표계에 시작점, 종료점과 반지름을 지정하여 헬리컬연속 보간 구동하는 함수이다. 
        // 원호 연속 보간 구동을 위해 내부 Queue에 저장하는 함수이다. AxmAdvContiStart함수를 사용해서 시작한다. (연속보간 함수와 같이 이용한다.)
        // dRadius = 반지름, dEndPos = 종료점 X,Y  , uShortDistance = 작은원(0), 큰원(1)
        // uCWDir   DIR_CCW(0): 반시계방향, DIR_CW(1) 시계방향   
        [DllImport("AXL.dll")] public static extern uint AxmAdvHelixRadiusMove(int lCoord, double dRadius, double dEndXPos, double dEndYPos, double dZPos, double dVel, double dStartVel, double dStopVel, double dAccel, double dDecel, uint uCWDir, uint uShortDistance);
        // 지정된 좌표계에 시작점과 종료점을 지정하여 3축 헬리컬 보간 오버라이드 구동한다.
        // 현재 진행중인 보간구동을 지정된 속도 및 위치로 헬리컬 보간 오버라이드 하며, 다음 노드에 대한 헬리컬 보간 구동 예약도 가능한다.
        // IOverrideMode = 0일 경우, 구동중인 보간이 직선, 원호 보간에 관계없이 현재 구동 노드에서 헬리컬 보간으로 즉시 오버라이드 되고, 
        // IOverrideMode = 1이면 현재 구동 노드 다음의 노드부터 헬리컬 보간이 차례로 예약된다.
        // IOverrideMode = 1로 본 함수를 호출할때마다 최소 1개에서 최대 8개까지 오버라이드 큐에 증가되면서 자동적으로 예약이 되며, 예약 후 마지막에 IOverrideMode = 0으로 본 함수가 호출되면
        // 내부적으로 오버라이드 큐에 있는 예약 보간들이 연속보간 큐로 저장되고, 헬리컬 오버라이드 구동과 이후의 예약된 연속보간이 순차적으로 실행된다.
        [DllImport("AXL.dll")] public static extern uint AxmAdvOvrHelixRadiusMove(int lCoord, double dRadius, double dEndXPos, double dEndYPos, double dZPos, double dVel, double dStartVel, double dStopVel, double dAccel, double dDecel, uint uCWDir, uint uShortDistance, int lOverrideMode);

        //====일반 보간함수 =================================================================================================
        // 직선 보간을 예약 구동한다.
        // 시작점과 종료점을 지정하여 다축 직선 보간을 예약 구동하는 함수이다. 구동 시작 후 함수를 벗어난다.
        // AxmAdvContiBeginNode, AxmAdvContiEndNode와 같이사용시 지정된 좌표계에 시작점과 종료점을 지정하여 직선 보간 구동하는 Queue에 저장함수가된다. 
        // 직선 프로파일 연속 보간 구동을 위해 내부 Queue에 저장하여 AxmAdvContiStart함수를 사용해서 시작한다.
        [DllImport("AXL.dll")] public static extern uint AxmAdvScriptLineMove(int lCoordinate, double[] dPosition, double dMaxVelocity, double dStartVel, double dStopVel, double dMaxAccel, double dMaxDecel, uint dwScript, int lScirptAxisNo, double dScriptPos);
        // 지정된 좌표계에 시작점과 종료점을 지정하는 다축 직선 보간 오버라이드를 예약 구동하는 함수이다.
        // 현재 진행중인 보간구동을 지정된 속도 및 위치로 직선 보간 오버라이드를 예약 구동 하며, 다음 노드에 대한 직선 보간 구동 예약이 가능한다.
        // IOverrideMode = 0일 경우, 구동중인 보간이 직선, 원호 보간에 관계없이 현재 구동 노드에서 직선 보간으로 즉시 오버라이드 되고, 
        // IOverrideMode = 1이면 현재 구동 노드 다음의 노드부터 직선보간이 차례로 예약된다.
        // IOverrideMode = 1로 본 함수를 호출할때마다 최소 1개에서 최대 8개까지 오버라이드 큐에 증가되면서 자동적으로 예약이 되며, 예약 후 마지막에 IOverrideMode = 0으로 본 함수가 호출되면
        // 내부적으로 오버라이드 큐에 있는 예약 보간들이 연속보간 큐로 저장되고, 직선 오버라이드 구동과 이후의 예약된 연속보간이 순차적으로 실행된다.
        [DllImport("AXL.dll")] public static extern uint AxmAdvScriptOvrLineMove(int lCoordinate, double[] dPosition, double dMaxVelocity, double dStartVel, double dStopVel, double dMaxAccel, double dMaxDecel, int lOverrideMode, uint dwScript, int lScirptAxisNo, double dScriptPos);
        // 2축 원호보간을 예약 구동한다.
        // 시작점, 종료점과 중심점을 지정하여 원호 보간을 예약 구동하는 함수이다. 구동 시작 후 함수를 벗어난다.
        // AxmAdvContiBeginNode, AxmAdvContiEndNode, 와 같이사용시 지정된 좌표계에 시작점, 종료점과 중심점을 지정하여 구동하는 원호 보간 Queue에 저장함수가된다.
        // 프로파일 원호 연속 보간 구동을 위해 내부 Queue에 저장하여 AxmAdvContiStart함수를 사용해서 시작한다.
        // lAxisNo = 두축 배열 , dCenterPos = 중심점 X,Y 배열 , dEndPos = 종료점 X,Y 배열.
        // uCWDir   DIR_CCW(0): 반시계방향, DIR_CW(1) 시계방향
        [DllImport("AXL.dll")] public static extern uint AxmAdvScriptCircleCenterMove(int lCoord, int[] lAxisNo, double[] dCenterPos, double[] dEndPos, double dVel, double dStartVel, double dStopVel, double dAccel, double dDecel, uint uCWDir, uint dwScript, int lScirptAxisNo, double dScriptPos);
        // 중간점, 종료점을 지정하여 원호 보간을 예약 구동하는 함수이다. 구동 시작 후 함수를 벗어난다.
        // AxmAdvContiBeginNode, AxmAdvContiEndNode와 같이사용시 지정된 좌표계에 중간점, 종료점을 지정하여 구동하는 원호 보간 Queue에 저장함수가된다.
        // 프로파일 원호 연속 보간 구동을 위해 내부 Queue에 저장하여 AxmAdvContiStart함수를 사용해서 시작한다.
        // lAxisNo = 두축 배열 , dMidPos = 중간점 X,Y 배열 , dEndPos = 종료점 X,Y 배열, lArcCircle = 아크(0), 원(1)
        [DllImport("AXL.dll")] public static extern uint AxmAdvScriptCirclePointMove(int lCoord, int[] lAxisNo, double[] dMidPos, double[] dEndPos, double dVel, double dStartVel, double dStopVel, double dAccel, double dDecel, int lArcCircle, uint dwScript, int lScirptAxisNo, double dScriptPos);
        // 시작점, 회전각도와 반지름을 지정하여 원호 보간을 예약 구동하는 함수이다. 구동 시작 후 함수를 벗어난다.
        // AxmAdvContiBeginNode, AxmAdvContiEndNode와 같이사용시 지정된 좌표계에 시작점, 회전각도와 반지름을 지정하여 원호 보간 구동하는 Queue에 저장함수가된다.
        // 프로파일 원호 연속 보간 구동을 위해 내부 Queue에 저장하여 AxmAdvContiStart함수를 사용해서 시작한다.
        // lAxisNo = 두축 배열 , dCenterPos = 중심점 X,Y 배열 , dAngle = 각도.
        // uCWDir   DIR_CCW(0): 반시계방향, DIR_CW(1) 시계방향
        [DllImport("AXL.dll")] public static extern uint AxmAdvScriptCircleAngleMove(int lCoord, int[] lAxisNo, double[] dCenterPos, double dAngle, double dVel, double dStartVel, double dStopVel, double dAccel, double dDecel, uint uCWDir, uint dwScript, int lScirptAxisNo, double dScriptPos);
        // 시작점, 종료점과 반지름을 지정하여 원호 보간을 예약 구동하는 함수이다. 구동 시작 후 함수를 벗어난다.
        // AxmAdvContiBeginNode, AxmAdvContiEndNode와 같이사용시 지정된 좌표계에 시작점, 종료점과 반지름을 지정하여 원호 보간 구동하는 Queue에 저장함수가된다.
        // 프로파일 원호 연속 보간 구동을 위해 내부 Queue에 저장하여 AxmAdvContiStart함수를 사용해서 시작한다.
        // lAxisNo = 두축 배열 , dRadius = 반지름, dEndPos = 종료점 X,Y 배열 , uShortDistance = 작은원(0), 큰원(1)
        // uCWDir   DIR_CCW(0): 반시계방향, DIR_CW(1) 시계방향
        [DllImport("AXL.dll")] public static extern uint AxmAdvScriptCircleRadiusMove(int lCoord, int[] lAxisNo, double dRadius, double[] dEndPos, double dVel, double dStartVel, double dStopVel, double dAccel, double dDecel, uint uCWDir, uint uShortDistance, uint dwScript, int lScirptAxisNo, double dScriptPos);
        // 지정된 좌표계에 시작점과 종료점을 지정하여 2축 원호 보간 오버라이드를 예약 구동한다.
        // 현재 진행중인 보간구동을 지정된 속도 및 위치로 원호 보간 오버라이드을 예약 구동하며, 다음 노드에 대한 원호 보간 구동 예약이 가능한다.
        // IOverrideMode = 0일 경우, 구동중인 보간이 직선, 원호 보간에 관계없이 현재 구동 노드에서 원호 보간으로 즉시 오버라이드 되고, 
        // IOverrideMode = 1이면 현재 구동 노드 다음의 노드부터 원호보간이 차례로 예약된다.
        // IOverrideMode = 1로 본 함수를 호출할때마다 최소 1개에서 최대 8개까지 오버라이드 큐에 증가되면서 자동적으로 예약이 되며, 예약 후 마지막에 IOverrideMode = 0으로 본 함수가 호출되면
        // 내부적으로 오버라이드 큐에 있는 예약 보간들이 연속보간 큐로 저장되고, 원호 오버라이드 구동과 이후의 예약된 연속보간이 순차적으로 실행된다.
        [DllImport("AXL.dll")] public static extern uint AxmAdvScriptOvrCircleRadiusMove(int lCoord, int[] lAxisNo, double dRadius, double[] dEndPos, double dVel, double dStartVel, double dStopVel, double dAccel, double dDecel, uint uCWDir, uint uShortDistance, int lOverrideMode, uint dwScript, int lScirptAxisNo, double dScriptPos);

        //======= 헬리컬 이동 ===============================================================================================
        // 주의사항 : Helix를 연속보간 사용시 Spline, 직선보간과 원호보간을 같이 사용할수없다.

        // 지정된 좌표계에 시작점, 종료점과 중심점을 지정하여 헬리컬 보간을 예약 구동하는 함수이다.
        // AxmAdvContiBeginNode, AxmAdvContiEndNode와 같이사용시 지정된 좌표계에 시작점, 종료점과 중심점을 지정하여 헬리컬 연속보간을 예약 구동하는 함수이다. 
        // 원호 연속 보간 구동을 위해 내부 Queue에 저장하는 함수이다. AxmAdvContiStart함수를 사용해서 시작한다. (연속보간 함수와 같이 이용한다)
        // dCenterPos = 중심점 X,Y  , dEndPos = 종료점 X,Y .
        // uCWDir DIR_CCW(0): 반시계방향, DIR_CW(1) 시계방향	
        [DllImport("AXL.dll")] public static extern uint AxmAdvScriptHelixCenterMove(int lCoord, double dCenterXPos, double dCenterYPos, double dEndXPos, double dEndYPos, double dZPos, double dVel, double dStartVel, double dStopVel, double dAccel, double dDecel, uint uCWDir, uint dwScript, int lScirptAxisNo, double dScriptPos);
        // 지정된 좌표계에 시작점, 종료점과 반지름을 지정하여 헬리컬 보간을 예약 구동하는 함수이다. 
        // AxmAdvContiBeginNode, AxmAdvContiEndNode와 같이사용시 지정된 좌표계에 중간점, 종료점을 지정하여 헬리컬연속 보간을 예약 구동하는 함수이다. 
        // 원호 연속 보간 구동을 위해 내부 Queue에 저장하는 함수이다. AxmAdvContiStart함수를 사용해서 시작한다. (연속보간 함수와 같이 이용한다.)
        // dMidPos = 중간점 X,Y  , dEndPos = 종료점 X,Y
        [DllImport("AXL.dll")] public static extern uint AxmAdvScriptHelixPointMove(int lCoord, double dMidXPos, double dMidYPos, double dEndXPos, double dEndYPos, double dZPos, double dVel, double dStartVel, double dStopVel, double dAccel, double dDecel, uint dwScript, int lScirptAxisNo, double dScriptPos);
        // 지정된 좌표계에 시작점, 회전각도와 반지름을 지정하여 헬리컬 보간을 예약 구동하는 함수이다
        // AxmAdvContiBeginNode, AxmAdvContiEndNode와 같이사용시 지정된 좌표계에 시작점, 회전각도와 반지름을 지정하여 헬리컬연속 보간을 예약 구동하는 함수이다. 
        // 원호 연속 보간 구동을 위해 내부 Queue에 저장하는 함수이다. AxmAdvContiStart함수를 사용해서 시작한다. (연속보간 함수와 같이 이용한다.)
        //dCenterPos = 중심점 X,Y  , dAngle = 각도.
        // uCWDir   DIR_CCW(0): 반시계방향, DIR_CW(1) 시계방향
        [DllImport("AXL.dll")] public static extern uint AxmAdvScriptHelixAngleMove(int lCoord, double dCenterXPos, double dCenterYPos, double dAngle, double dZPos, double dVel, double dStartVel, double dStopVel, double dAccel, double dDecel, uint uCWDir, uint dwScript, int lScirptAxisNo, double dScriptPos);
        // 지정된 좌표계에 시작점, 종료점과 반지름을 지정하여 헬리컬 보간을 예약 구동하는 함수이다.
        // AxmAdvContiBeginNode, AxmAdvContiEndNode와 같이사용시 지정된 좌표계에 시작점, 종료점과 반지름을 지정하여 헬리컬연속 보간을 예약 구동하는 함수이다. 
        // 원호 연속 보간 구동을 위해 내부 Queue에 저장하는 함수이다. AxmAdvContiStart함수를 사용해서 시작한다. (연속보간 함수와 같이 이용한다.)
        // dRadius = 반지름, dEndPos = 종료점 X,Y  , uShortDistance = 작은원(0), 큰원(1)
        // uCWDir   DIR_CCW(0): 반시계방향, DIR_CW(1) 시계방향  
        [DllImport("AXL.dll")] public static extern uint AxmAdvScriptHelixRadiusMove(int lCoord, double dRadius, double dEndXPos, double dEndYPos, double dZPos, double dVel, double dStartVel, double dStopVel, double dAccel, double dDecel, uint uCWDir, uint uShortDistance, uint dwScript, int lScirptAxisNo, double dScriptPos);
        // 지정된 좌표계에 시작점과 종료점을 지정하여 3축 헬리컬 보간 오버라이드를 예약 구동한다.
        // 현재 진행중인 보간구동을 지정된 속도 및 위치로 헬리컬 보간 오버라이드를 예약 구동한다.
        // IOverrideMode = 0일 경우, 구동중인 보간이 직선, 원호 보간에 관계없이 현재 구동 노드에서 헬리컬 보간으로 즉시 오버라이드 되고, 
        // IOverrideMode = 1이면 현재 구동 노드 다음의 노드부터 헬리컬 보간이 차례로 예약된다.
        // IOverrideMode = 1로 본 함수를 호출할때마다 최소 1개에서 최대 8개까지 오버라이드 큐에 증가되면서 자동적으로 예약이 되며, 예약 후 마지막에 IOverrideMode = 0으로 본 함수가 호출되면
        // 내부적으로 오버라이드 큐에 있는 예약 보간들이 연속보간 큐로 저장되고, 헬리컬 오버라이드 구동과 이후의 예약된 연속보간이 순차적으로 실행된다.
        [DllImport("AXL.dll")] public static extern uint AxmAdvScriptOvrHelixRadiusMove(int lCoord, double dRadius, double dEndXPos, double dEndYPos, double dZPos, double dVel, double dStartVel, double dStopVel, double dAccel, double dDecel, uint uCWDir, uint uShortDistance, int lOverrideMode, uint dwScript, int lScirptAxisNo, double dScriptPos);

        //====연속 보간 함수 ================================================================================================
        // 지정된 좌표계에 연속 보간 구동 중 현재 구동중인 연속 보간 인덱스 번호를 확인하는 함수이다.
        [DllImport("AXL.dll")] public static extern uint AxmAdvContiGetNodeNum(int lCoordinate, ref int lpNodeNum);
        // 지정된 좌표계에 설정한 연속 보간 구동 총 인덱스 갯수를 확인하는 함수이다.
        [DllImport("AXL.dll")] public static extern uint AxmAdvContiGetTotalNodeNum(int lCoordinate, ref int lpNodeNum);
        // 지정된 좌표계에 보간 구동을 위한 내부 Queue에 저장되어 있는 보간 구동 개수를 확인하는 함수이다.
        [DllImport("AXL.dll")] public static extern uint AxmAdvContiReadIndex(int lCoordinate, ref int lpQueueIndex);
        // 지정된 좌표계에 보간 구동을 위한 내부 Queue가 비어 있는지 확인하는 함수이다.
        [DllImport("AXL.dll")] public static extern uint AxmAdvContiReadFree(int lCoordinate, ref uint upQueueFree);
        // 지정된 좌표계에 연속 보간 구동을 위해 저장된 내부 Queue를 모두 삭제하는 함수이다.
        [DllImport("AXL.dll")] public static extern uint AxmAdvContiWriteClear(int lCoordinate);
        // 지정된 좌표계에 연속 보간 오버라이드 구동을 위해 예약된 오버라이드용 큐를 모두 삭제하는 함수이다.
        [DllImport("AXL.dll")] public static extern uint AxmAdvOvrContiWriteClear(int lCoordinate);
        // 연속 보간 시작 한다.
        [DllImport("AXL.dll")] public static extern uint AxmAdvContiStart(int lCoord, uint dwProfileset, int lAngle);
        // 연속 보간 정지 한다.
        [DllImport("AXL.dll")] public static extern uint AxmAdvContiStop(int lCoordinate, double dDecel);
        //지정된 좌표계에 연속보간 축 맵핑을 설정한다.
        //(축맵핑 번호는 0 부터 시작))
        // 주의점:  축맵핑할때는 반드시 실제 축번호가 작은 숫자부터 큰숫자를 넣는다.
        //          가상축 맵핑 함수를 사용하였을 때 가상축번호를 실제 축번호가 작은 값 부터 lpAxesNo의 낮은 인텍스에 입력하여야 한다.
        //          가상축 맵핑 함수를 사용하였을 때 가상축번호에 해당하는 실제 축번호가 다른 값이라야 한다.
        //          같은 축을 다른 Coordinate에 중복 맵핑하지 말아야 한다.
        [DllImport("AXL.dll")] public static extern uint AxmAdvContiSetAxisMap(int lCoord, int lSize, int[] lpAxesNo);
        //지정된 좌표계에 연속보간 축 맵핑을 반환한다.
        [DllImport("AXL.dll")] public static extern uint AxmAdvContiGetAxisMap(int lCoord, ref int lpSize, ref int lpAxesNo);
        // 지정된 좌표계에 연속보간 축 절대/상대 모드를 설정한다.
        // (주의점 : 반드시 축맵핑 하고 사용가능)
        // 지정 축의 이동 거리 계산 모드를 설정한다.
        //uAbsRelMode : POS_ABS_MODE '0' - 절대 좌표계
        //              POS_REL_MODE '1' - 상대 좌표계
        [DllImport("AXL.dll")] public static extern uint AxmAdvContiSetAbsRelMode(int lCoord, uint uAbsRelMode);
        // 지정된 좌표계에 연속보간 축 절대/상대 모드를 반환한다.
        [DllImport("AXL.dll")] public static extern uint AxmAdvContiGetAbsRelMode(int lCoord, ref uint uAbsRelMode);
        // 지정된 좌표계에 연속 보간 구동 중인지 확인하는 함수이다.
        [DllImport("AXL.dll")] public static extern uint AxmAdvContiIsMotion(int lCoordinate, ref uint upInMotion);
        // 지정된 좌표계에 연속보간에서 수행할 작업들의 등록을 시작한다. 이함수를 호출한후,
        // AxmAdvContiEndNode함수가 호출되기 전까지 수행되는 모든 모션작업은 실제 모션을 수행하는 것이 아니라 연속보간 모션으로 등록 되는 것이며,
        // AxmAdvContiStart 함수가 호출될 때 비로소 등록된모션이 실제로 수행된다.
        [DllImport("AXL.dll")] public static extern uint AxmAdvContiBeginNode(int lCoord);
        // 지정된 좌표계에서 연속보간을 수행할 작업들의 등록을 종료한다.
        [DllImport("AXL.dll")] public static extern uint AxmAdvContiEndNode(int lCoord);

        // 지정한 다축을 설정한 감속도로 동기 감속 정지한다.
        [DllImport("AXL.dll")] public static extern uint AxmMoveMultiStop(int lArraySize, int[] lpAxesNo, double[] dMaxDecel);
        // 지정한 다축을 동기 급 정지한다.
        [DllImport("AXL.dll")] public static extern uint AxmMoveMultiEStop(int lArraySize, int[] lpAxesNo);
        // 지정한 다축을 동기 감속 정지한다.
        [DllImport("AXL.dll")] public static extern uint AxmMoveMultiSStop(int lArraySize, int[] lpAxesNo);

        // 지정한 축의 실제 구동 속도를 읽어온다.
        [DllImport("AXL.dll")] public static extern uint AxmStatusReadActVel(int lAxisNo, ref double dpVel);          //2010.10.11
                                                                                                                      // 서보 타입 슬레이브 기기의 SVCMD_STAT 커맨드 값을 읽는다.
        [DllImport("AXL.dll")] public static extern uint AxmStatusReadServoCmdStat(int lAxisNo, ref uint upStatus);
        // 서보 타입 슬레이브 기기의 SVCMD_CTRL 커맨드 값을 읽는다.
        [DllImport("AXL.dll")] public static extern uint AxmStatusReadServoCmdCtrl(int lAxisNo, ref uint upStatus);

        // 겐트리 구동시 마스터 축과 슬레이브 축 간의 위치 차에 대한 설정된 오차 한계값을 반환한다.    
        [DllImport("AXL.dll")] public static extern uint AxmGantryGetMstToSlvOverDist(int lAxisNo, ref double dpPosition);
        // 겐트리 구동시 마스터 축과 슬레이브 축 간의 위치 차에 대한 오차 한계값을 설정한다.
        [DllImport("AXL.dll")] public static extern uint AxmGantrySetMstToSlvOverDist(int lAxisNo, double dPosition);

        // 지정 축의 알람 신호의 코드 상태를 반환한다.
        [DllImport("AXL.dll")] public static extern uint AxmSignalReadServoAlarmCode(int lAxisNo, ref ushort upCodeStatus);

        // 서보 타입 슬레이브 기기의 좌표계 설정을 실시한다. (MLIII 전용)
        [DllImport("AXL.dll")] public static extern uint AxmM3ServoCoordinatesSet(int lAxisNo, uint dwPosData, uint dwPos_sel, uint dwRefe);
        // 서보 타입 슬레이브 기기의 브레이크 작동 신호를 출력한다. (MLIII 전용)
        [DllImport("AXL.dll")] public static extern uint AxmM3ServoBreakOn(int lAxisNo);
        // 서보 타입 슬레이브 기기의 브레이크 작동 신호를 해제한다. (MLIII 전용)
        [DllImport("AXL.dll")] public static extern uint AxmM3ServoBreakOff(int lAxisNo);
        // 서보 타입 슬레이브 기기의 
        [DllImport("AXL.dll")] public static extern uint AxmM3ServoConfig(int lAxisNo, uint dwCfMode);
        // 서보 타입 슬레이브 기기의 센서 종보 초기화를 요구한다.
        [DllImport("AXL.dll")] public static extern uint AxmM3ServoSensOn(int lAxisNo);
        // 서보 타입 슬레이브 기기의 센서전원 OFF를 요구한다.
        [DllImport("AXL.dll")] public static extern uint AxmM3ServoSensOff(int lAxisNo);
        // 서보 타입 슬레이브 기기의 SMON 커맨드를 실행한다.
        [DllImport("AXL.dll")] public static extern uint AxmM3ServoSmon(int lAxisNo);
        // 서보 타입 슬레이브 기기의 모니터 정보나 입출력 신호의 상태를 읽는다.
        [DllImport("AXL.dll")] public static extern uint AxmM3ServoGetSmon(int lAxisNo, ref uint pbParam);
        // 서보 타입 슬레이브 기기에 서보 ON을 요구한다.
        [DllImport("AXL.dll")] public static extern uint AxmM3ServoSvOn(int lAxisNo);
        // 서보 타입 슬레이브 기기에 서보 OFF를 요구한다.
        [DllImport("AXL.dll")] public static extern uint AxmM3ServoSvOff(int lAxisNo);
        // 서보 타입 슬레이브 기기가 지정된 보간 위치로 보간이동을 실시한다.
        [DllImport("AXL.dll")] public static extern uint AxmM3ServoInterpolate(int lAxisNo, uint dwTPOS, uint dwVFF, uint dwTFF, uint dwTLIM);
        // 서보 타입 슬레이브 기기가 지정한 위치로 위치이송을 실시한다.
        [DllImport("AXL.dll")] public static extern uint AxmM3ServoPosing(int lAxisNo, uint dwTPOS, uint dwSPD, uint dwACCR, uint dwDECR, uint dwTLIM);
        // 서보 타입 슬레이브 기기가 지정된 이동속도로 정속이송을 실시한다.
        [DllImport("AXL.dll")] public static extern uint AxmM3ServoFeed(int lAxisNo, uint dwSPD, uint dwACCR, uint dwDECR, uint dwTLIM);
        // 서보 타입 슬레이브 기기가 이송중 외부 위치결정 신호의 입력에 의해 위치이송을 실시한다.
        [DllImport("AXL.dll")] public static extern uint AxmM3ServoExFeed(int lAxisNo, uint dwSPD, uint dwACCR, uint dwDECR, uint dwTLIM, uint dwExSig1, uint dwExSig2);
        // 서보 타입 슬레이브 기기가 외부 위치결정 신호 입력에 의해 위치이송을 실시한다.
        [DllImport("AXL.dll")] public static extern uint AxmM3ServoExPosing(int lAxisNo, uint dwTPOS, uint dwSPD, uint dwACCR, uint dwDECR, uint dwTLIM, uint dwExSig1, uint dwExSig2);
        // 서보 타입 슬레이브 기기가 원점 복귀를 실시한다.
        [DllImport("AXL.dll")] public static extern uint AxmM3ServoZret(int lAxisNo, uint dwSPD, uint dwACCR, uint dwDECR, uint dwTLIM, uint dwExSig1, uint dwExSig2, uint bHomeDir, uint bHomeType);
        // 서보 타입 슬레이브 기기가 속도제어를 실시한다.
        [DllImport("AXL.dll")] public static extern uint AxmM3ServoVelctrl(int lAxisNo, uint dwTFF, uint dwVREF, uint dwACCR, uint dwDECR, uint dwTLIM);
        // 서보 타입 슬레이브 기기가 토크제어를 실시한다.    
        [DllImport("AXL.dll")] public static extern uint AxmM3ServoTrqctrl(int lAxisNo, uint dwVLIM, int lTQREF);
        // bmode 0x00 : common parameters ram
        // bmode 0x01 : common parameters flash
        // bmode 0x10 : device parameters ram
        // bmode 0x11 : device parameters flash
        // 서보 타입 슬레이브 기기의 서보팩 특정 파라미터 설정값을 반환한다.
        [DllImport("AXL.dll")] public static extern uint AxmM3ServoGetParameter(int lAxisNo, uint wNo, uint bSize, uint bMode, ref uint pbParam);
        // 서보 타입 슬레이브 기기의 서보팩 특정 파라미터 값을 설정한다.
        [DllImport("AXL.dll")] public static extern uint AxmM3ServoSetParameter(int lAxisNo, uint wNo, uint bSize, uint bMode, ref uint pbParam);

        // M3 서보팩에 Command Execution을 실행한다
        [DllImport("AXL.dll")] public static extern uint AxmServoCmdExecution(int lAxisNo, uint dwCommand, uint dwSize, ref uint pdwExcData);
        // 서보 타입 슬레이브 기기의 설정된 토크 제한 값을 반환한다.
        [DllImport("AXL.dll")] public static extern uint AxmM3ServoGetTorqLimit(int lAxisNo, ref uint dwpTorqLimit);
        // 서보 타입 슬레이브 기기에 토크 제한 값을 설정한다.
        [DllImport("AXL.dll")] public static extern uint AxmM3ServoSetTorqLimit(int lAxisNo, uint dwTorqLimit);

        // 서보 타입 슬레이브 기기에 설정된 SVCMD_IO 커맨드 값을 반환한다.
        [DllImport("AXL.dll")] public static extern uint AxmM3ServoGetSendSvCmdIOOutput(int lAxisNo, ref uint dwData);
        // 서보 타입 슬레이브 기기에 SVCMD_IO 커맨드 값을 설정한다.
        [DllImport("AXL.dll")] public static extern uint AxmM3ServoSetSendSvCmdIOOutput(int lAxisNo, uint dwData);

        // 서보 타입 슬레이브 기기에 설정된 SVCMD_CTRL 커맨드 값을 반환한다.
        [DllImport("AXL.dll")] public static extern uint AxmM3ServoGetSvCmdCtrl(int lAxisNo, ref uint dwData);
        // 서보 타입 슬레이브 기기에 SVCMD_CTRL 커맨드 값을 설정한다.
        [DllImport("AXL.dll")] public static extern uint AxmM3ServoSetSvCmdCtrl(int lAxisNo, uint dwData);

        // MLIII adjustment operation을 설정 한다.
        // dwReqCode == 0x1005 : parameter initialization : 20sec
        // dwReqCode == 0x1008 : absolute encoder reset   : 5sec
        // dwReqCode == 0x100E : automatic offset adjustment of motor current detection signals  : 5sec
        // dwReqCode == 0x1013 : Multiturn limit setting  : 5sec
        [DllImport("AXL.dll")] public static extern uint AxmM3AdjustmentOperation(int lAxisNo, uint dwReqCode);
        // 서보 축 추가 모니터링 채널별 선택 값을 설정한다.
        [DllImport("AXL.dll")] public static extern uint AxmM3ServoSetMonSel(int lAxisNo, uint dwMon0, uint dwMon1, uint dwMon2);
        // 서보 축 추가 모니터링 채널별 설정 값을 반환한다.
        [DllImport("AXL.dll")] public static extern uint AxmM3ServoGetMonSel(int lAxisNo, ref uint upMon0, ref uint upMon1, ref uint upMon2);
        // 서보 축 추가 모니터링 채널별 설정 값을 기준으로 현재 상태를 반환한다.
        [DllImport("AXL.dll")] public static extern uint AxmM3ServoReadMonData(int lAxisNo, uint dwMonSel, ref uint dwpMonData);
        // 제어할 토크 축 설정
        [DllImport("AXL.dll")] public static extern uint AxmAdvTorqueContiSetAxisMap(int lCoord, int lSize, int[] lpAxesNo, uint dwTLIM, uint dwConMode);
        // 2014.04.28
        // 토크 프로파일 설정 파라미터
        [DllImport("AXL.dll")] public static extern uint AxmM3ServoSetTorqProfile(int lCoord, int lAxisNo, int TorqueSign, uint dwVLIM, uint dwProfileMode, uint dwStdTorq, uint dwStopTorq);
        // 토크 프로파일 확인 파라미터
        [DllImport("AXL.dll")] public static extern uint AxmM3ServoGetTorqProfile(int lCoord, int lAxisNo, ref int lpTorqueSign, ref uint updwVLIM, ref uint upProfileMode, ref uint upStdTorq, ref uint upStopTorq);
        //-------------------------------------------------------------------------------------------------------------------
        //======== SMP 전용 함수 =======================================================================================
        // Inposition 신호의 Range를 설정한다. (dInposRange > 0)
        [DllImport("AXL.dll")] public static extern uint AxmSignalSetInposRange(int lAxisNo, double dInposRange);
        // Inposition 신호의 Range를 반환한다.
        [DllImport("AXL.dll")] public static extern uint AxmSignalGetInposRange(int lAxisNo, ref double dpInposRange);

        // 오버라이드할때 위치속성(절대/상대)을 설정한다.
        [DllImport("AXL.dll")] public static extern uint AxmMotSetOverridePosMode(int lAxisNo, uint dwAbsRelMode);
        // 오버라이드할때 위치속성(절대/상대)을 읽어온다.
        [DllImport("AXL.dll")] public static extern uint AxmMotGetOverridePosMode(int lAxisNo, ref uint dwpAbsRelMode);
        // LineMove 오버라이드할때 위치속성(절대/상대)을 설정한다.
        [DllImport("AXL.dll")] public static extern uint AxmMotSetOverrideLinePosMode(int lCoordNo, uint dwAbsRelMode);
        // LineMove 오버라이드할때 위치속성(절대/상대)을 읽어온다.
        [DllImport("AXL.dll")] public static extern uint AxmMotGetOverrideLinePosMode(int lCoordNo, ref uint dwpAbsRelMode);

        // AxmMoveStartPos와 동일하며 종료속도(EndVel)가 추가되었다.
        [DllImport("AXL.dll")] public static extern uint AxmMoveStartPosEx(int lAxisNo, double dPos, double dVel, double dAccel, double dDecel, double dEndVel);
        // AxmMovePos와 동일하며 종료속도(EndVel)가 추가되었다.
        [DllImport("AXL.dll")] public static extern uint AxmMovePosEx(int lAxisNo, double dPos, double dVel, double dAccel, double dDecel, double dEndVel);

        // Coordinate Motion을 경로상에서 감속정지(dDecel) 시킨다.
        [DllImport("AXL.dll")] public static extern uint AxmMoveCoordStop(int lCoordNo, double dDecel);
        // Coordinate Motion을 급정지 시킨다.
        [DllImport("AXL.dll")] public static extern uint AxmMoveCoordEStop(int lCoordNo);
        // Coordinate Motion을 경로상에서 감속정지 시킨다.
        [DllImport("AXL.dll")] public static extern uint AxmMoveCoordSStop(int lCoordNo);

        // AxmLineMove모션의 위치를 오버라이드 시킨다.
        [DllImport("AXL.dll")] public static extern uint AxmOverrideLinePos(int lCoordNo, ref double dpOverridePos);
        // AxmLineMove모션의 속도를 오버라이드 시킨다. 각축들의 속도비율은 dpDistance로 결정된다.
        [DllImport("AXL.dll")] public static extern uint AxmOverrideLineVel(int lCoordNo, double dOverrideVel, ref double dpDistance);

        // AxmLineMove모션의 속도를 오버라이드 시킨다.
        // dMaxAccel  : 오버라이드 가속도
        // dMaxDecel  : 오버라이드 감속도
        // dpDistance : 각축의 속도 비율
        [DllImport("AXL.dll")] public static extern uint AxmOverrideLineAccelVelDecel(int lCoordNo, double dOverrideVelocity, double dMaxAccel, double dMaxDecel, ref double dpDistance);
        // AxmOverrideVelAtPos에 추가적으로 AccelDecel을 오버라이드 시킨다.
        [DllImport("AXL.dll")] public static extern uint AxmOverrideAccelVelDecelAtPos(int lAxisNo, double dPos, double dVel, double dAccel, double dDecel, double dOverridePos, double dOverrideVel, double dOverrideAccel, double dOverrideDecel, int lTarget);

        // 하나의 마스터축에 다수의 슬레이브축들을 연결한다(Electronic Gearing).
        // lMasterAxisNo : 마스터축
        // lSize : 슬레이브축 개수(최대 8)
        // lpSlaveAxisNo : 슬레이브축 번호
        // dpGearRatio : 마스터축을 기준으로하는 슬레이브축 비율(0은 제외, 1 = 100%)
        [DllImport("AXL.dll")] public static extern uint AxmEGearSet(int lMasterAxisNo, int lSize, ref int lpSlaveAxisNo, ref double dpGearRatio);
        // Electronic Gearing정보를 읽어온다.
        [DllImport("AXL.dll")] public static extern uint AxmEGearGet(int lMasterAxisNo, ref int lpSize, ref int lpSlaveAxisNo, ref double dpGearRatio);
        // Electronic Gearing을 해제한다.
        [DllImport("AXL.dll")] public static extern uint AxmEGearReset(int lMasterAxisNo);
        // Electronic Gearing을 활성/비활성한다.
        [DllImport("AXL.dll")] public static extern uint AxmEGearEnable(int lMasterAxisNo, uint dwEnable);
        // Electronic Gearing을 활성/비활성상태를 읽어온다.
        [DllImport("AXL.dll")] public static extern uint AxmEGearIsEnable(int lMasterAxisNo, ref uint dwpEnable);

        // 주의사항: 입력한 종료속도가 '0'미만이면 '0'으로, 'AxmMotSetMaxVel'로 설정한 최대속도를 초과하면 'MaxVel'로 재설정된다. 
        // 지정 축에 종료속도를 설정한다.
        [DllImport("AXL.dll")] public static extern uint AxmMotSetEndVel(int lAxisNo, double dEndVelocity);
        // 지정 축의 종료속도를 반환한다.
        [DllImport("AXL.dll")] public static extern uint AxmMotGetEndVel(int lAxisNo, ref double dpEndVelocity);

        [DllImport("AXL.dll")] public static extern uint AxmFilletMove(int lCoord, double[] dPos, double[] dFVector, double[] dSVector, double dVel, double dAccel, double dDecel, double dRadius);

        // 단축 PVT 구동을 한다.
        // 사용자가 Position, Velocity, Time Table을 이용하여 생성한 프로파일로 구동한다.
        // AxmSyncBegin, AxmSyncEnd API와 함께 사용시 여러 축의 PVT 구동을 예약한다.
        // 예약된 PVT 구동 프로파일은 AxmSyncStart 명령을 받게되면 동시에 시작한다.
        // lAxisNo : 구동 축
        // dwArraySize : PVT Table size
        // pdPos : Position 배열
        // pdVel : Velocity 배열
        // pdwUsec : Time 배열(Usec 단위, 단 Cycle의 배수여야만 한다. ex 1sec = 1,000,000)
        [DllImport("AXL.dll")] public static extern uint AxmMovePVT(int lAxisNo, uint dwArraySize, double[] pdPos, double[] pdVel, uint[] pdwUsec);

        //====Sync 함수 ================================================================================================
        //지정된 Sync No.에서 사용할 축을 맵핑한다.
        //(맵핑 번호는 0 부터 시작))
        // SyncSetAxisMap의 경우 Sync 구동에서 사용되는 유효축을 설정하는 함수이다.
        // SyncBegin과 SyncEnd 사이에서 사용되는 PVT Motion의 지정 축이 맵핑되지 않은 축일 경우
        // 예약되지 않고 즉시 구동한다. 즉 맵핑된 축만이 Begin과 End사이에서 구동 예약이 되며
        // SyncStart API를 호출하면 지정된 Sync Index에서 예약된 구동이 동시에 시작한다.

        // Sync 구동에서 사용될 유효 축을 지정한다.
        // lSyncNo : Sync Index
        // lSize : 맵핑할 축 갯수
        // lpAxesNo : 맵핑 축 배열
        [DllImport("AXL.dll")] public static extern uint AxmSyncSetAxisMap(int lSyncNo, int lSize, int[] lpAxesNo);

        // 지정된 Sync Index에 할당된 축 맵핑과 예약 프로파일을 리셋한다.
        [DllImport("AXL.dll")] public static extern uint AxmSyncClear(int lSyncNo);

        // 지정된 Sync Index에 수행할 구동 예약을 시작한다.
        // 이 함수를 호출한 후, AxmSyncEnd 함수가 호출되기 전까지 실행되는
        // 유효 축의 PVT 구동은 실제 구동을 즉시 수행하는 것이 아니라 구동 예약이 되며
        // AxmSyncStart 함수가 호출될 때 비로소 예약된 구동이 수행된다.
        [DllImport("AXL.dll")] public static extern uint AxmSyncBegin(int lSyncNo);

        // 지정된 Sync Index에서 수행할 구동 예약을 종료한다.
        [DllImport("AXL.dll")] public static extern uint AxmSyncEnd(int lSyncNo);

        // 지정된 Sync Index에서 예약된 구동을 시작한다.
        [DllImport("AXL.dll")] public static extern uint AxmSyncStart(int lSyncNo);

        // 지정된 축의 Profile Queue에 여유 Count를 확인한다.
        [DllImport("AXL.dll")] public static extern uint AxmStatusReadRemainQueueCount(int lAxisNo, ref uint pdwRemainQueueCount);
    }
    #endregion

    #region AJINEXTEK LIBRARY
    public class AJINLIBRARY
    {
        //========== 라이브러리 초기화 ========================================================================

        // 라이브러리 초기화
        [DllImport("AXL.dll")] public static extern uint AxlOpen(int lIrqNo);
        // 라이브러리 초기화시 하드웨어 칩에 리셋을 하지 않음.
        [DllImport("AXL.dll")] public static extern uint AxlOpenNoReset(uint lIrqNo);
        // 라이브러리 사용을 종료
        [DllImport("AXL.dll")] public static extern int AxlClose();
        // 라이브러리가 초기화 되어 있는 지 확인
        [DllImport("AXL.dll")] public static extern int AxlIsOpened();

        // 인터럽트를 사용한다.
        [DllImport("AXL.dll")] public static extern uint AxlInterruptEnable();
        // 인터럽트를 사용안한다.
        [DllImport("AXL.dll")] public static extern uint AxlInterruptDisable();

        //========== 라이브러리 및 베이스 보드 정보 ===========================================================

        // 등록된 베이스 보드의 개수 확인
        [DllImport("AXL.dll")] public static extern uint AxlGetBoardCount(ref int lpBoardCount);
        // 라이브러리 버전 확인
        [DllImport("AXL.dll")] public static extern uint AxlGetLibVersion(ref byte szVersion);
        // Network제품의 각 모듈별 연결상태를 확인하는 함수
        [DllImport("AXL.dll")] public static extern uint AxlGetModuleNodeStatus(int nBoardNo, int nModulePos);
        // 해당 보드가 제어 가능한 상태인지 반환한다.
        [DllImport("AXL.dll")] public static extern uint AxlGetBoardStatus(int nBoardNo);
        // Network 제품의 Configuration Lock 상태를 반환한다.
        // *wpLockMode  : DISABLE(0), ENABLE(1)
        [DllImport("AXL.dll")] public static extern uint AxlGetLockMode(int nBoardNo, ref uint upLockMode);

        [DllImport("AXL.dll")]
        public static extern uint AxlSetLockMode(int nBoardNo, uint upLockMode);

        // Network 제품의 ScanTime 상태를 설정한다.
        [DllImport("AXL.dll")]
        public static extern uint AxlSetNetComTime(int nBoardNo, byte szNetComTime);

        // Network 제품의 ScanTime 상태를 반환한다.
        [DllImport("AXL.dll")]
        public static extern uint AxlGetNetComTime(int nBoardNo, ref byte szNetComTime);
        //========= 로그 레벨 =================================================================================

        // EzSpy에 출력할 메시지 레벨 설정
        // uLevel : 0 - 3 설정
        // LEVEL_NONE(0)    : 모든 메시지를 출력하지 않는다.
        // LEVEL_ERROR(1)   : 에러가 발생한 메시지만 출력한다.
        // LEVEL_RUNSTOP(2) : 모션에서 Run / Stop 관련 메시지를 출력한다.
        // LEVEL_FUNCTION(3): 모든 메시지를 출력한다.
        [DllImport("AXL.dll")] public static extern uint AxlSetLogLevel(uint uLevel);
        // EzSpy에 출력할 메시지 레벨 확인
        [DllImport("AXL.dll")] public static extern uint AxlGetLogLevel(ref uint upLevel);

        //========== MLIII =================================================================================
        // Network제품의 각 모듈을 검색을 시작하는 함수
        [DllImport("AXL.dll")] public static extern uint AxlScanStart(int lBoardNo, long lNet);
        // Network제품 각 보드의 모든 모듈을 connect하는 함수
        [DllImport("AXL.dll")] public static extern uint AxlBoardConnect(int lBoardNo, long lNet);
        // Network제품 각 보드의 모든 모듈을 Disconnect하는 함수
        [DllImport("AXL.dll")] public static extern uint AxlBoardDisconnect(int lBoardNo, long lNet);

        //========== SIIIH =================================================================================
        // SIIIH 마스터 보드에 연결된 모듈에 대한 검색을 시작하는 함수(SIIIH 마스터 보드 전용)
        [DllImport("AXL.dll")] public static extern uint AxlScanStartSIIIH(ref _SCAN_RESULT pScanResult);
    }
    #endregion

    #region AJINEXTEK REFENUM
    public enum AXT_BASE_BOARD : uint
    {
        AXT_UNKNOWN = 0x00,    // Unknown Baseboard
        AXT_BIHR = 0x01,    // ISA bus, Half size
        AXT_BIFR = 0x02,    // ISA bus, Full size
        AXT_BPHR = 0x03,    // PCI bus, Half size
        AXT_BPFR = 0x04,    // PCI bus, Full size
        AXT_BV3R = 0x05,    // VME bus, 3U size
        AXT_BV6R = 0x06,    // VME bus, 6U size
        AXT_BC3R = 0x07,    // cPCI bus, 3U size
        AXT_BC6R = 0x08,    // cPCI bus, 6U size
        AXT_BEHR = 0x09,    // PCIe bus, Half size
        AXT_BEFR = 0x0A,    // PCIe bus, Full size
        AXT_BEHD = 0x0B,    // PCIe bus, Half size, DB-32T
        AXT_FMNSH4D = 0x52,    // ISA bus, Full size, DB-32T, SIO-2V03 * 2
        AXT_PCI_DI64R = 0x43,    // PCI bus, Digital IN 64점
        AXT_PCIE_DI64R = 0x44,    // PCIe bus, Digital IN 64점
        AXT_PCI_DO64R = 0x53,    // PCI bus, Digital OUT 64점
        AXT_PCIE_DO64R = 0x54,    // PCIe bus, Digital OUT 64점
        AXT_PCI_DB64R = 0x63,    // PCI bus, Digital IN 32점, OUT 32점
        AXT_PCIE_DB64R = 0x64,    // PCIe bus, Digital IN 32점, OUT 32점
        AXT_BPFR_COM = 0x70,    // PCI bus, Full size, For serial function modules.
        AXT_PCIN204 = 0x82,    // PCI bus, Half size On-Board 2 Axis controller.
        AXT_BPHD = 0x83,    // PCI bus, Half size, DB-32T
        AXT_PCIN404 = 0x84,    // PCI bus, Half size On-Board 4 Axis controller.    
        AXT_PCIN804 = 0x85,    // PCI bus, Half size On-Board 8 Axis controller.
        AXT_PCIEN804 = 0x86,    // PCIe bus, Half size On-Board 8 Axis controller.
        AXT_PCIEN404 = 0x87,    // PCIe bus, Half size On-Board 4 Axis controller.
        AXT_PCI_AIO1602HR = 0x93,    // PCI bus, Half size, AI-16ch, AO-2ch AI16HR
        AXT_PCI_R1604 = 0xC1,    // PCI bus[PCI9030], Half size, RTEX based 16 axis controller
        AXT_PCI_R3204 = 0xC2,    // PCI bus[PCI9030], Half size, RTEX based 32 axis controller
        AXT_PCI_R32IO = 0xC3,    // PCI bus[PCI9030], Half size, RTEX based IO only.
        AXT_PCI_REV2 = 0xC4,    // Reserved.
        AXT_PCI_R1604MLII = 0xC5,    // PCI bus[PCI9030], Half size, Mechatrolink-II 16/32 axis controller.
        AXT_PCI_R0804MLII = 0xC6,    // PCI bus[PCI9030], Half size, Mechatrolink-II 08 axis controller.
        AXT_PCI_Rxx00MLIII = 0xC7,    // Master PCI Board, Mechatrolink III AXT, PCI Bus[PCI9030], Half size, Max.32 Slave module support
        AXT_PCIE_Rxx00MLIII = 0xC8,    // Master PCI Express Board, Mechatrolink III AXT, PCI Bus[], Half size, Max.32 Slave module support
        AXT_PCP2_Rxx00MLIII = 0xC9,    // Master PCI/104-Plus Board, Mechatrolink III AXT, PCI Bus[], Half size, Max.32 Slave module support
        AXT_PCI_R1604SIIIH = 0xCA,    // PCI bus[PCI9030], Half size, SSCNET III / H 16/32 axis controller.
        AXT_PCI_R32IOEV = 0xCB,    // PCI bus[PCI9030], Half size, RTEX based IO only. Economic version.
        AXT_PCIE_R0804RTEX = 0xCC,    // PCIe bus, Half size, Half size, RTEX based 08 axis controller.
        AXT_PCIE_R1604RTEX = 0xCD,    // PCIe bus, Half size, Half size, RTEX based 16 axis controller.
        AXT_PCIE_R2404RTEX = 0xCE,    // PCIe bus, Half size, Half size, RTEX based 24 axis controller.
        AXT_PCIE_R3204RTEX = 0xCF,    // PCIe bus, Half size, Half size, RTEX based 32 axis controller.
        AXT_PCIE_Rxx04SIIIH = 0xD0,    // PCIe bus, Half size, SSCNET III / H 16(or 32)-axis(CAMC-QI based) controller.
        AXT_PCIE_Rxx00SIIIH = 0xD1,    // PCIe bus, Half size, SSCNET III / H Max. 32-axis(DSP Based) controller.
        AXT_ETHERCAT_RTOSV5 = 0xD2,    // EtherCAT, RTOS(On Time), Version 5.29
        AXT_PCI_Nx04_A = 0xD3,     // PCI bus, Half size On-Board x Axis controller For Rtos.
        AXT_PCI_R3200MLIII_IO = 0xD4,    // PCI Board, Mechatrolink III AXT, PCI Bus[PCI9030], Half size, Max.32 IO only	controller
        AXT_PCIE_Rxx05MLIII = 0xD5,    // PCIe bus, Half size, Mechatrolink III / H Max. 32-axis(DSP Based) controller.
        AXT_PCIE_Rxx05SIIIH = 0xD6,    // PCIe bus, Half size, Sscnet III / H  32 axis(DSP Based) controller.
        AXT_PCIE_Rxx05RTEX = 0xD7,    // PCIe bus, Half size, RTEX 32 axis(DSP Based) controller.
        AXT_PCIE_Rxx05ECAT = 0xD8,    // PCIe bus, Half size, ECAT(DSP Based) controller.
        AXT_PCI_Rxx05MLIII = 0xD9,    // PCI bus, Half size, Mechatrolink III 32 axis(DSP Based) controller.
        AXT_PCI_Rxx05SIIIH = 0xDA,    // PCI bus, Half size, Sscnet III / H  32 axis(DSP Based) controller.
        AXT_PCI_Rxx05RTEX = 0xDB,    // PCI bus, Half size, RTEX 32 axis(DSP Based) controller.
        AXT_PCI_Rxx05ECAT = 0xDC,    // PCI bus, Half size, ECAT(DSP Based) controller.
        AXT_VIRTUAL_BOARD = 0xDD,    // Virtual XX axis controller
        AXT_PCIE_Rxx05ECAT_E = 0xDE,    // PCIe bus, Half size, ECAT(DSP Based) controller. (Max.64 Axes)
        AXT_EXDEV_BOARD = 0xDF     // AXTEXDEV , 3rd party external device
    }

    // 모듈 정의                                    
    public enum AXT_MODULE : uint
    {
        AXT_UNKNOWN = 0x00,    // Unknown Baseboard                                            
        AXT_SMC_2V01 = 0x01,    // CAMC-5M, 2 Axis
        AXT_SMC_2V02 = 0x02,    // CAMC-FS, 2 Axis
        AXT_SMC_1V01 = 0x03,    // CAMC-5M, 1 Axis
        AXT_SMC_1V02 = 0x04,    // CAMC-FS, 1 Axis
        AXT_SMC_2V03 = 0x05,    // CAMC-IP, 2 Axis
        AXT_SMC_4V04 = 0x06,    // CAMC-QI, 4 Axis
        AXT_SMC_R1V04A4 = 0x07,    // CAMC-QI, 1 Axis, For RTEX A4 slave only
        AXT_SMC_1V03 = 0x08,    // CAMC-IP, 1 Axis
        AXT_SMC_R1V04 = 0x09,    // CAMC-QI, 1 Axis, For RTEX SLAVE only(Pulse Output Module)
        AXT_SMC_R1V04MLIISV = 0x0A,    // CAMC-QI, 1 Axis, For Sigma-X series.
        AXT_SMC_R1V04MLIIPM = 0x0B,    // 2 Axis, For Pulse output series(JEPMC-PL2910).
        AXT_SMC_2V04 = 0x0C,    // CAMC-QI, 2 Axis
        AXT_SMC_R1V04A5 = 0x0D,    // CAMC-QI, 1 Axis, For RTEX A5N slave only
        AXT_SMC_R1V04MLIICL = 0x0E,    // CAMC-QI, 1 Axis, For MLII Convex Linear only
        AXT_SMC_R1V04MLIICR = 0x0F,    // CAMC-QI, 1 Axis, For MLII Convex Rotary only
        AXT_SMC_R1V04PM2Q = 0x10,    // CAMC-QI, 2 Axis, For RTEX SLAVE only(Pulse Output Module)
        AXT_SMC_R1V04PM2QE = 0x11,    // CAMC-QI, 4 Axis, For RTEX SLAVE only(Pulse Output Module)
        AXT_SMC_R1V04MLIIORI = 0x12,    // CAMC-QI, 1 Axis, For MLII Oriental Step Driver only
        AXT_SMC_R1V04A6 = 0x13,    // CAMC-QI, 1 Axis, For RTEX A5N slave only
        AXT_SMC_R1V04SIIIHMIV = 0x14,    // CAMC-QI, 1 Axis, For SSCNETIII/H MRJ4
        AXT_SMC_R1V04SIIIHMIV_R = 0x15,    // CAMC-QI, 1 Axis, For SSCNETIII/H MRJ4
        AXT_SMC_R1V04SIIIHME = 0x16,    // CAMC-QI, 1 Axis, For SSCNETIII/H MRJE
        AXT_SMC_R1V04SIIIHME_R = 0x17,    // CAMC-QI, 1 Axis, For SSCNETIII/H MRJE
        AXT_SMC_R1V04MLIIS7 = 0x1D,    // CAMC-QI, 1 Axis, For ML-II/YASKWA Sigma7(SGD7S)
        AXT_SMC_R1V04MLIIISV = 0x20,    // DSP, 1 Axis, For ML-3 SigmaV/YASKAWA only 
        AXT_SMC_R1V04MLIIIPM = 0x21,    // DSP, 1 Axis, For ML-3 SLAVE/AJINEXTEK only(Pulse Output Module)
        AXT_SMC_R1V04MLIIISV_MD = 0x22,    // DSP, 1 Axis, For ML-3 SigmaV-MD/YASKAWA only (Multi-Axis Control amp)
        AXT_SMC_R1V04MLIIIS7S = 0x23,    // DSP, 1 Axis, For ML-3 Sigma7S/YASKAWA only
        AXT_SMC_R1V04MLIIIS7W = 0x24,    // DSP, 2 Axis, For ML-3 Sigma7W/YASKAWA only(Dual-Axis control amp)
        AXT_SMC_R1V04MLIIIRS2 = 0x25,    // DSP, 1 Axis, For ML-3 RS2A/SANYO DENKY
        AXT_SMC_R1V04MLIIIS7_MD = 0x26,    // DSP, 1 Axis, For ML-3 Sigma7-MD/YASKAWA only (Multi-Axis Control amp)
        AXT_SMC_R1V04MLIIIAZ = 0x27,    // DSP, 4 Axis, For ML-3 AZD/ORIENTAL only (Multi-Axis Control amp)
        AXT_SMC_R1V04MLIIIPCON = 0x28,    // DSP, 1 Axis, For ML-3 PCON/IAI only
        AXT_SMC_R1V04PM2QSIIIH = 0x60,    // CAMC-QI, 2Axis For SSCNETIII/H SLAVE only(Pulse Output Module)
        AXT_SMC_R1V04PM4QSIIIH = 0x61,    // CAMC-QI, 4Axis For SSCNETIII/H SLAVE only(Pulse Output Module)
        AXT_SIO_RCNT2SIIIH = 0x62,    // Counter slave module, Reversible counter, 2 channels, For SSCNETIII/H Only
        AXT_SIO_RDI32SIIIH = 0x63,    // DI slave module, SSCNETIII AXT, IN 32-Channel
        AXT_SIO_RDO32SIIIH = 0x64,    // DO slave module, SSCNETIII AXT, OUT 32-Channel
        AXT_SIO_RDB32SIIIH = 0x65,    // DB slave module, SSCNETIII AXT, IN 16-Channel, OUT 16-Channel
        AXT_SIO_RAI16SIIIH = 0x66,    // AI slave module, SSCNETIII AXT, Analog IN 16ch, 16 bit
        AXT_SIO_RAO08SIIIH = 0x67,    // A0 slave module, SSCNETIII AXT, Analog OUT 8ch, 16 bit
        AXT_SMC_R1V04PM2QSIIIH_R = 0x68,    // CAMC-QI, 2Axis For SSCNETIII/H SLAVE only(Pulse Output Module) 
        AXT_SMC_R1V04PM4QSIIIH_R = 0x69,    // CAMC-QI, 4Axis For SSCNETIII/H SLAVE only(Pulse Output Module) 
        AXT_SIO_RCNT2SIIIH_R = 0x6A,    // Counter slave module, Reversible counter, 2 channels, For SSCNETIII/H Only 
        AXT_SIO_RDI32SIIIH_R = 0x6B,    // DI slave module, SSCNETIII AXT, IN 32-Channel 
        AXT_SIO_RDO32SIIIH_R = 0x6C,    // DO slave module, SSCNETIII AXT, OUT 32-Channel 
        AXT_SIO_RDB32SIIIH_R = 0x6D,    // DB slave module, SSCNETIII AXT, IN 16-Channel, OUT 16-Channel 
        AXT_SIO_RAI16SIIIH_R = 0x6E,    // AI slave module, SSCNETIII AXT, Analog IN 16ch, 16 bit 
        AXT_SIO_RAO08SIIIH_R = 0x6F,    // A0 slave module, SSCNETIII AXT, Analog OUT 8ch, 16 bit 
        AXT_SIO_RDI32MLIII = 0x70,    // DI slave module, MechatroLink III AXT, IN 32-Channel NPN
        AXT_SIO_RDO32MLIII = 0x71,    // DO slave module, MechatroLink III AXT, OUT 32-Channel  NPN
        AXT_SIO_RDB32MLIII = 0x72,    // DB slave module, MechatroLink III AXT, IN 16-Channel, OUT 16-Channel  NPN
        AXT_SIO_RDI32MSMLIII = 0x73,    // DI slave module, MechatroLink III M-SYSTEM, IN 32-Channel
        AXT_SIO_RDO32AMSMLIII = 0x74,    // DO slave module, MechatroLink III M-SYSTEM, OUT 32-Channel
        AXT_SIO_RDI32PMLIII = 0x75,    // DI slave module, MechatroLink III AXT, IN 32-Channel PNP
        AXT_SIO_RDO32PMLIII = 0x76,    // DO slave module, MechatroLink III AXT, OUT 32-Channel  PNP
        AXT_SIO_RDB32PMLIII = 0x77,    // DB slave module, MechatroLink III AXT, IN 16-Channel, OUT 16-Channel  PNP
        AXT_SIO_RDI16MLIII = 0x78,    // DI slave module, MechatroLink III M-SYSTEM, IN 16-Channel
        AXT_SIO_UNDEFINEMLIII = 0x79,    // IO slave module, MechatroLink III Any, Max. IN 480-Channel, Max. OUT 480-Channel
        AXT_SIO_RDI32MLIIISFA = 0x7A,    // DI slave module, MechatroLink III AXT(SFA), IN 32-Channel NPN
        AXT_SIO_RDO32MLIIISFA = 0x7B,    // DO slave module, MechatroLink III AXT(SFA), OUT 32-Channel  NPN
        AXT_SIO_RDB32MLIIISFA = 0x7C,    // DB slave module, MechatroLink III AXT(SFA), IN 16-Channel, OUT 16-Channel  NPN
        AXT_SIO_RDB128MLIIIAI = 0x7D,    // Intelligent I/O (Product by IAI), For MLII only
        AXT_SIO_RSIMPLEIOMLII = 0x7E,    // Digital IN/Out xx점, Simple I/O series, MLII 전용.
        AXT_SIO_RDO16AMLIII = 0x7F,    // DO slave module, MechatroLink III M-SYSTEM, OUT 16-Channel, NPN
        AXT_SIO_RDI16MLII = 0x80,    // DISCRETE INPUT MODULE, 16 points (Product by M-SYSTEM), For MLII only
        AXT_SIO_RDO16AMLII = 0x81,    // NPN TRANSISTOR OUTPUT MODULE, 16 points (Product by M-SYSTEM), For MLII only
        AXT_SIO_RDO16BMLII = 0x82,    // PNP TRANSISTOR OUTPUT MODULE, 16 points (Product by M-SYSTEM), For MLII only 
        AXT_SIO_RDB96MLII = 0x83,    // Digital IN/OUT(Selectable), MAX 96 points, For MLII only
        AXT_SIO_RDO32RTEX = 0x84,    // Digital OUT  32점
        AXT_SIO_RDI32RTEX = 0x85,    // Digital IN  32점
        AXT_SIO_RDB32RTEX = 0x86,    // Digital IN/OUT  32점
        AXT_SIO_RDO32RTEXNT1_D1 = 0x87,    // Digital OUT 32점 IntekPlus 전용
        AXT_SIO_RDI32RTEXNT1_D1 = 0x88,    // Digital IN 32점 IntekPlus 전용
        AXT_SIO_RDB32RTEXNT1_D1 = 0x89,    // Digital IN/OUT 32점 IntekPlus 전용
        AXT_SIO_RDO16BMLIII = 0x8A,    // DO slave module, MechatroLink III M-SYSTEM, OUT 16-Channel, PNP
        AXT_SIO_RDB32ML2NT1 = 0x8B,    // DB slave module, MechatroLink2 AJINEXTEK NT1 Series
        AXT_SIO_RDB128YSMLIII = 0x8C,    // IO slave module, MechatroLink III Any, Max. IN 480-Channel, Max. OUT 480-Channel
        AXT_SIO_DI32_P = 0x92,    // Digital IN  32점, PNP type(source type)
        AXT_SIO_DO32T_P = 0x93,    // Digital OUT 32점, Power TR, PNT type(source type)
        AXT_SIO_RDB128MLII = 0x94,    // Digital IN 64점 / OUT 64점, MLII 전용(JEPMC-IO2310)
        AXT_SIO_RDI32 = 0x95,    // Digital IN  32점, For RTEX only
        AXT_SIO_RDO32 = 0x96,    // Digital OUT 32점, For RTEX only
        AXT_SIO_DI32 = 0x97,    // Digital IN  32점
        AXT_SIO_DO32P = 0x98,    // Digital OUT 32점
        AXT_SIO_DB32P = 0x99,    // Digital IN 16점 / OUT 16점
        AXT_SIO_RDB32T = 0x9A,    // Digital IN 16점 / OUT 16점, For RTEX only
        AXT_SIO_DO32T = 0x9E,    // Digital OUT 16점, Power TR OUTPUT
        AXT_SIO_DB32T = 0x9F,    // Digital IN 16점 / OUT 16점, Power TR OUTPUT
        AXT_SIO_RAI16RB = 0xA0,    // A0h(160) : AI 16Ch, 16 bit, For RTEX only
        AXT_SIO_AI4RB = 0xA1,    // A1h(161) : AI 4Ch, 12 bit
        AXT_SIO_AO4RB = 0xA2,    // A2h(162) : AO 4Ch, 12 bit
        AXT_SIO_AI16H = 0xA3,    // A3h(163) : AI 4Ch, 16 bit
        AXT_SIO_AO8H = 0xA4,    // A4h(164) : AO 4Ch, 16 bit
        AXT_SIO_AI16HB = 0xA5,    // A5h(165) : AI 16Ch, 16 bit (SIO-AI16HR(input module))
        AXT_SIO_AO2HB = 0xA6,    // A6h(166) : AO 2Ch, 16 bit  (SIO-AI16HR(output module))
        AXT_SIO_RAI8RB = 0xA7,    // A7h(167) : AI 8Ch, 16 bit, For RTEX only        
        AXT_SIO_RAO4RB = 0xA8,    // A8h(168) : AO 4Ch, 16 bit, For RTEX only
        AXT_SIO_RAI4MLII = 0xA9,    // A9h(169) : AI 4Ch, 16 bit, For MLII only
        AXT_SIO_RAO2MLII = 0xAA,    // AAh(170) : AO 2Ch, 16 bit, For MLII only
        AXT_SIO_RAVCI4MLII = 0xAB,    // DC VOLTAGE/CURRENT INPUT MODULE, 4 points (Product by M-SYSTEM), For MLII only
        AXT_SIO_RAVO2MLII = 0xAC,    // DC VOLTAGE OUTPUT MODULE, 2 points (Product by M-SYSTEM), For MLII only
        AXT_SIO_RACO2MLII = 0xAD,    // DC CURRENT OUTPUT MODULE, 2 points (Product by M-SYSTEM), For MLII only
        AXT_SIO_RATI4MLII = 0xAE,    // THERMOCOUPLE INPUT MODULE, 4 points (Product by M-SYSTEM), For MLII only
        AXT_SIO_RARTDI4MLII = 0xAF,    // RTD INPUT MODULE, 4 points (Product by M-SYSTEM), For MLII only
        AXT_SIO_RCNT2MLII = 0xB0,    // Counter slave module, Reversible counter, 2 channels (Product by YASKWA), For MLII only
        AXT_SIO_CN2CH = 0xB1,    // Counter Module, 2 channels, Remapped ID, Actual ID is (0xA8)
        AXT_SIO_RCNT2RTEX = 0xB2,    // Counter slave module, Reversible counter, 2 channels, For RTEX Only
        AXT_SIO_RCNT2MLIII = 0xB3,    // Counter slave module, MechatroLink III AXT, 2 ch, Trigger per channel
        AXT_SIO_RHPC4MLIII = 0xB4,    // Counter slave module, MechatroLink III AXT, 4 ch
        AXT_SIO_RAI16RTEX = 0xC0,    // ANALOG VOLTAGE INPUT(+- 10V) 16 Channel RTEX 
        AXT_SIO_RAO08RTEX = 0xC1,    // ANALOG VOLTAGE OUTPUT(+- 10V) 08 Channel RTEX
        AXT_SIO_RAI8MLIII = 0xC2,    // AI slave module, MechatroLink III AXT, Analog IN 8ch, 16 bit
        AXT_SIO_RAI16MLIII = 0xC3,    // AI slave module, MechatroLink III AXT, Analog IN 16ch, 16 bit
        AXT_SIO_RAO4MLIII = 0xC4,    // A0 slave module, MechatroLink III AXT, Analog OUT 4ch, 16 bit
        AXT_SIO_RAO8MLIII = 0xC5,    // A0 slave module, MechatroLink III AXT, Analog OUT 8ch, 16 bit
        AXT_SIO_RAVO4MLII = 0xC6,    // DC VOLTAGE OUTPUT MODULE, 4 points (Product by M-SYSTEM), For MLII only
        AXT_SIO_RAV04MLIII = 0xC7,    // AO Slave module, MechatroLink III M-SYSTEM Voltage output module
        AXT_SIO_RAVI4MLIII = 0xC8,    // AI Slave module, MechatroLink III M-SYSTEM Voltage/Current input module
        AXT_SIO_RAI16MLIIISFA = 0xC9,    // AI slave module, MechatroLink III AXT(SFA), Analog IN 16ch, 16 bit
        AXT_SIO_RDB32MSMLIII = 0xCA,    // DIO slave module, MechatroLink III M-SYSTEM, IN 16-Channel, OUT 16-Channel
        AXT_SIO_RAVI4MLII = 0xCB,    // DC VOLTAGE/CURRENT INPUT MODULE, 4 points (Product by M-SYSTEM), For MLII only
        AXT_SIO_RMEMORY_MLIII = 0xCC,    // Memory Access type module, MechatroLink III
        AXT_SIO_RAVCI8MLII = 0xCE,    // DC VOLTAGE/CURRENT INPUT MODULE, 8 points (Product by M-SYSTEM), For MLII only
        AXT_SIO_RDB64MSMLIII = 0xD0,       // DIO slave module, MechatroLink III M-SYSTEM, IN 32-Channel, OUT 32-Channel
        AXT_SIO_RDI16MSMLIII = 0xD1,       // DI slave module, MechatroLink III M-SYSTEM, IN 16-Channel
        AXT_COM_234R = 0xD3,    // COM-234R
        AXT_COM_484R = 0xD4,    // COM-484R
        AXT_COM_234IDS = 0xD5,    // COM-234IDS
        AXT_COM_484IDS = 0xD6,    // COM-484IDS
        AXT_SIO_AO4F = 0xD7,    // AO 4Ch, 16 bit
        AXT_SIO_AI8F = 0xD8,    // AI 8Ch, 16 bit
        AXT_SIO_AI8AO4F = 0xD9,    // AI 8Ch, AO 4Ch, 16 bit
        AXT_SIO_HPC4 = 0xDA,    // External Encoder module for 4Channel with Trigger function.
        AXT_ECAT_MOTION = 0xE1,    // EtherCAT Motion Module
        AXT_ECAT_DIO = 0xE2,    // EtherCAT DIO Module 
        AXT_ECAT_AIO = 0xE3,    // EtherCAT AIO Module
        AXT_ECAT_COM = 0xE4,    // EtherCAT Serial COM(RS232C) Module
        AXT_ECAT_COUPLER = 0xE5,    // EtherCAT Coupler Module
        AXT_ECAT_CNT = 0xE6,    // EtherCAT Count Module        
        AXT_ECAT_UNKNOWN = 0xE7,    // EtherCAT Unknown Module
        AXT_SMC_4V04_A = 0xEA,    // Nx04_A
        AXT_VIRTUAL_MOTION = 0xEB,    // Virtual Motion Module
        AXT_VIRTUAL_DIO = 0xEC,    // Virtual DIO Module
        AXT_VIRTUAL_AIO = 0xED,    // Virtual AIO Module
        AXT_EXDEV_DIO = 0xF5,    // AXTEXDEV	3rd party External device DIO
        AXT_EXDEV_AIO = 0xF6     // AXTEXDEV	3rd party External device AIO
    }

    public enum AXT_FUNC_RESULT : uint
    {
        AXT_RT_SUCCESS = 0,        // API 함수 수행 성공
        AXT_RT_OPEN_ERROR = 1001,     // 라이브러리 오픈 되지않음
        AXT_RT_OPEN_ALREADY = 1002,     // 라이브러리 오픈 되어있고 사용 중임
        AXT_RT_NOT_OPEN = 1053,     // 라이브러리 초기화 실패
        AXT_RT_NOT_SUPPORT_VERSION = 1054,     // 지원하지않는 하드웨어
        AXT_RT_LOCK_FILE_MISMATCH = 1055,     // Lock파일과 현재 Scan정보가 일치하지 않음

        AXT_RT_RESCAN_NOT_EXIST_BOARD = 1060,    // 보드가 존재하지 않음
        AXT_RT_RESCAN_TIMEOUT = 1061,    // Rescan 명령 후 대기 시간 초과

        AXT_RT_BAD_PARAMETER = 1070,       // 사용자가 입력한 파라미터가 적절하지 않음

        AXT_RT_INVALID_BOARD_NO = 1101,     // 유효하지 않는 보드 번호
        AXT_RT_INVALID_MODULE_POS = 1102,     // 유효하지 않는 모듈 위치
        AXT_RT_INVALID_LEVEL = 1103,     // 유효하지 않는 레벨
        AXT_RT_INVALID_VARIABLE = 1104,     // 유효하지 않는 변수
        AXT_RT_INVALID_MODULE_NO = 1105,     // 유효하지 않는 모듈
        AXT_RT_ERROR_VERSION_READ = 1151,     // 라이브러리 버전을 읽을수 없음
        AXT_RT_NETWORK_ERROR = 1152,     // 하드웨어 네트워크 에러
        AXT_RT_NETWORK_LOCK_MISMATCH = 1153,     // 보드 Lock정보와 현재 Scan정보가 일치하지 않음

        AXT_RT_1ST_BELOW_MIN_VALUE = 1160,     // 첫번째 인자값이 최소값보다 더 작음
        AXT_RT_1ST_ABOVE_MAX_VALUE = 1161,     // 첫번째 인자값이 최대값보다 더 큼
        AXT_RT_2ND_BELOW_MIN_VALUE = 1170,     // 두번째 인자값이 최소값보다 더 작음
        AXT_RT_2ND_ABOVE_MAX_VALUE = 1171,     // 두번째 인자값이 최대값보다 더 큼
        AXT_RT_3RD_BELOW_MIN_VALUE = 1180,     // 세번째 인자값이 최소값보다 더 작음
        AXT_RT_3RD_ABOVE_MAX_VALUE = 1181,     // 세번째 인자값이 최대값보다 더 큼
        AXT_RT_4TH_BELOW_MIN_VALUE = 1190,     // 네번째 인자값이 최소값보다 더 작음
        AXT_RT_4TH_ABOVE_MAX_VALUE = 1191,     // 네번째 인자값이 최대값보다 더 큼
        AXT_RT_5TH_BELOW_MIN_VALUE = 1200,     // 다섯번째 인자값이 최소값보다 더 작음
        AXT_RT_5TH_ABOVE_MAX_VALUE = 1201,     // 다섯번째 인자값이 최대값보다 더 큼
        AXT_RT_6TH_BELOW_MIN_VALUE = 1210,     // 여섯번째 인자값이 최소값보다 더 작음 
        AXT_RT_6TH_ABOVE_MAX_VALUE = 1211,     // 여섯번째 인자값이 최대값보다 더 큼
        AXT_RT_7TH_BELOW_MIN_VALUE = 1220,     // 일곱번째 인자값이 최소값보다 더 작음
        AXT_RT_7TH_ABOVE_MAX_VALUE = 1221,     // 일곱번째 인자값이 최대값보다 더 큼
        AXT_RT_8TH_BELOW_MIN_VALUE = 1230,     // 여덟번째 인자값이 최소값보다 더 작음
        AXT_RT_8TH_ABOVE_MAX_VALUE = 1231,     // 여덟번째 인자값이 최대값보다 더 큼
        AXT_RT_9TH_BELOW_MIN_VALUE = 1240,     // 아홉번째 인자값이 최소값보다 더 작음
        AXT_RT_9TH_ABOVE_MAX_VALUE = 1241,     // 아홉번째 인자값이 최대값보다 더 큼
        AXT_RT_10TH_BELOW_MIN_VALUE = 1250,     // 열번째 인자값이 최소값보다 더 작음
        AXT_RT_10TH_ABOVE_MAX_VALUE = 1251,     // 열번째 인자값이 최대값보다 더 큼
        AXT_RT_11TH_BELOW_MIN_VALUE = 1252,     // 열한번째 인자값이 최소값보다 더 작음
        AXT_RT_11TH_ABOVE_MAX_VALUE = 1253,     // 열한번째 인자값이 최대값보다 더 큼

        AXT_RT_AIO_OPEN_ERROR = 2001,     // AIO 모듈 오픈실패
        AXT_RT_AIO_NOT_MODULE = 2051,     // AIO 모듈 없음
        AXT_RT_AIO_NOT_EVENT = 2052,     // AIO 이벤트 읽지 못함
        AXT_RT_AIO_INVALID_MODULE_NO = 2101,     // 유효하지않은 AIO모듈
        AXT_RT_AIO_INVALID_CHANNEL_NO = 2102,     // 유효하지않은 AIO채널번호
        AXT_RT_AIO_INVALID_USE = 2106,     // AIO 함수 사용못함
        AXT_RT_AIO_INVALID_TRIGGER_MODE = 2107,     // 유효하지않는 트리거 모드
        AXT_RT_AIO_EXTERNAL_DATA_EMPTY = 2108,     // 외부 데이터 값이 없을 경우
        AXT_RT_AIO_INVALID_VALUE = 2109,     // 유효하지않는 값 설정
        AXT_RT_AIO_UPG_ALEADY_ENABLED = 2110,     // AO UPG 기능 사용중 설정됨

        AXT_RT_DIO_OPEN_ERROR = 3001,     // DIO 모듈 오픈실패
        AXT_RT_DIO_NOT_MODULE = 3051,     // DIO 모듈 없음
        AXT_RT_DIO_NOT_INTERRUPT = 3052,     // DIO 인터럽트 설정안됨
        AXT_RT_DIO_INVALID_MODULE_NO = 3101,     // 유효하지않는 DIO 모듈 번호
        AXT_RT_DIO_INVALID_OFFSET_NO = 3102,     // 유효하지않는 DIO OFFSET 번호
        AXT_RT_DIO_INVALID_LEVEL = 3103,     // 유효하지않는 DIO 레벨
        AXT_RT_DIO_INVALID_MODE = 3104,     // 유효하지않는 DIO 모드
        AXT_RT_DIO_INVALID_VALUE = 3105,     // 유효하지않는 값 설정
        AXT_RT_DIO_INVALID_USE = 3106,     // DIO 함수 사용못함      

        AXT_RT_CNT_OPEN_ERROR = 3201,     // CNT 모듈 오픈실패
        AXT_RT_CNT_NOT_MODULE = 3251,     // CNT 모듈 없음
        AXT_RT_CNT_NOT_INTERRUPT = 3252,     // CNT 인터럽트 설정안됨
        AXT_RT_CNT_NOT_TRIGGER_ENABLE = 3253,     // CNT Trigger 출력 기능이 활성화되어 있지 않음
        AXT_RT_CNT_INVALID_MODULE_NO = 3301,     // 유효하지않는 CNT 모듈 번호
        AXT_RT_CNT_INVALID_CHANNEL_NO = 3302,     // 유효하지않는 채널 번호
        AXT_RT_CNT_INVALID_OFFSET_NO = 3303,     // 유효하지않는 CNT OFFSET 번호
        AXT_RT_CNT_INVALID_LEVEL = 3304,     // 유효하지않는 CNT 레벨
        AXT_RT_CNT_INVALID_MODE = 3305,     // 유효하지않는 CNT 모드
        AXT_RT_CNT_INVALID_VALUE = 3306,     // 유효하지않는 값 설정
        AXT_RT_CNT_INVALID_USE = 3307,     // CNT 함수 사용못함
        AXT_RT_CNT_CMD_EXE_TIMEOUT = 3308,     // CNT 모듈 데이터입력 시간초과 했을때
        AXT_RT_CNT_INVALID_VELOCITY = 3309,     // 유효하지않는 CNT 속도
        AXT_RT_PROTECTED_DURING_PWMENABLE = 3310,     // PWM Enable 되어 있는 상태에서 사용 못 함
        AXT_RT_CNT_INVALID_TABLEPOS = 3311,     // 유효하지 않은 CNT TABLE 번호 
        AXT_RT_CNT_DIMENSION_ERROR = 3312,     // 해당 Dimension 설정 상태에서는 사용할 수 없음

        AXT_RT_MOTION_OPEN_ERROR = 4001,     // 모션 라이브러리 Open 실패
        AXT_RT_MOTION_NOT_MODULE = 4051,     // 시스템에 장착된 모션 모듈이 없음
        AXT_RT_MOTION_NOT_INTERRUPT = 4052,     // 인터럽트 결과 읽기 실패
        AXT_RT_MOTION_NOT_INITIAL_AXIS_NO = 4053,     // 해당 축 모션 초기화 실패
        AXT_RT_MOTION_NOT_IN_CONT_INTERPOL = 4054,     // 연속 보간 구동 중이 아닌 상태에서 연속보간 중지 명령을 수행 하였음
        AXT_RT_MOTION_NOT_PARA_READ = 4055,     // 원점 구동 설정 파라미터 로드 실패
        AXT_RT_MOTION_INVALID_AXIS_NO = 4101,     // 해당 축이 존재하지 않음
        AXT_RT_MOTION_INVALID_METHOD = 4102,     // 해당 축 구동에 필요한 설정이 잘못됨
        AXT_RT_MOTION_INVALID_USE = 4103,     // 'uUse' 인자값이 잘못 설정됨
        AXT_RT_MOTION_INVALID_LEVEL = 4104,     // 'uLevel' 인자값이 잘못 설정됨
        AXT_RT_MOTION_INVALID_BIT_NO = 4105,     // 범용 입출력 해당 비트가 잘못 설정됨
        AXT_RT_MOTION_INVALID_STOP_MODE = 4106,     // 모션 정지 모드 설정값이 잘못됨
        AXT_RT_MOTION_INVALID_TRIGGER_MODE = 4107,     // 트리거 설정 모드가 잘못 설정됨
        AXT_RT_MOTION_INVALID_TRIGGER_LEVEL = 4108,     // 트리거 출력 레벨 설정이 잘못됨
        AXT_RT_MOTION_INVALID_SELECTION = 4109,     // 'uSelection' 인자가 COMMAND 또는 ACTUAL 이외의 값으로 설정되어 있음
        AXT_RT_MOTION_INVALID_TIME = 4110,     // Trigger 출력 시간값이 잘못 설정되어 있음
        AXT_RT_MOTION_INVALID_FILE_LOAD = 4111,     // 모션 설정값이 저장된 파일이 로드가 안됨
        AXT_RT_MOTION_INVALID_FILE_SAVE = 4112,     // 모션 설정값을 저장하는 파일 저장에 실패함
        AXT_RT_MOTION_INVALID_VELOCITY = 4113,     // 모션 구동 속도값이 0으로 설정되어 모션 에러 발생
        AXT_RT_MOTION_INVALID_ACCELTIME = 4114,     // 모션 구동 가속 시간값이 0으로 설정되어 모션 에러 발생
        AXT_RT_MOTION_INVALID_PULSE_VALUE = 4115,     // 모션 단위 설정 시 입력 펄스값이 0보다 작은값으로 설정됨
        AXT_RT_MOTION_INVALID_NODE_NUMBER = 4116,     // 위치나 속도 오버라이드 함수가 모션 정지 중에 실햄됨
        AXT_RT_MOTION_INVALID_TARGET = 4117,     // 다축 모션 정지 원인에 관한 플래그를 반환한다.

        AXT_RT_MOTION_ERROR_IN_NONMOTION = 4151,     // 모션 구동중이어야 되는데 모션 구동중이 아닐 때
        AXT_RT_MOTION_ERROR_IN_MOTION = 4152,     // 모션 구동 중에 다른 모션 구동 함수를 실행함
        AXT_RT_MOTION_ERROR = 4153,     // 다축 구동 정지 함수 실행 중 에러 발생함
        AXT_RT_MOTION_ERROR_GANTRY_ENABLE = 4154,     // 겐트리 enable이 되어있을 때
        AXT_RT_MOTION_ERROR_GANTRY_AXIS = 4155,     // 겐트리 축이 마스터채널(축) 번호(0 ~ (최대축수 - 1))가 잘못 들어갔을 때
        AXT_RT_MOTION_ERROR_MASTER_SERVOON = 4156,     // 마스터 축 서보온이 안되어있을 때
        AXT_RT_MOTION_ERROR_SLAVE_SERVOON = 4157,     // 슬레이브 축 서보온이 안되어있을 때
        AXT_RT_MOTION_INVALID_POSITION = 4158,     // 유효한 위치에 없을 때          
        AXT_RT_ERROR_NOT_SAME_MODULE = 4159,     // 똑 같은 모듈내에 있지 않을경우
        AXT_RT_ERROR_NOT_SAME_BOARD = 4160,     // 똑 같은 보드내에 있지 아닐경우
        AXT_RT_ERROR_NOT_SAME_PRODUCT = 4161,     // 제품이 서로 다를경우
        AXT_RT_NOT_CAPTURED = 4162,     // 위치가 저장되지 않을 때
        AXT_RT_ERROR_NOT_SAME_IC = 4163,     // 같은 칩내에 존재하지않을 때
        AXT_RT_ERROR_NOT_GEARMODE = 4164,     // 기어모드로 변환이 안될 때
        AXT_ERROR_CONTI_INVALID_AXIS_NO = 4165,     // 연속보간 축맵핑 시 유효한 축이 아닐 때
        AXT_ERROR_CONTI_INVALID_MAP_NO = 4166,     // 연속보간 맵핑 시 유효한 맵핑 번호가 아닐 때
        AXT_ERROR_CONTI_EMPTY_MAP_NO = 4167,     // 연속보간 맵핑 번호가 비워 있을 때
        AXT_RT_MOTION_ERROR_CACULATION = 4168,     // 계산상의 오차가 발생했을 때
        AXT_RT_ERROR_MOVE_SENSOR_CHECK = 4169,     // 연속보간 구동전 에러센서가(Alarm, EMG, Limit등) 감지된경우 

        AXT_ERROR_HELICAL_INVALID_AXIS_NO = 4170,     // 헬리컬 축 맵핑 시 유효한 축이 아닐 때
        AXT_ERROR_HELICAL_INVALID_MAP_NO = 4171,     // 헬리컬 맵핑 시 유효한 맵핑 번호가 아닐  때 
        AXT_ERROR_HELICAL_EMPTY_MAP_NO = 4172,     // 헬리컬 멥핑 번호가 비워 있을 때
        AXT_ERROR_HELICAL_ZPOS_DISTANCE_ZERO = 4173,     // 헬리컬 맵핑된 Z축의 이동량이 0일 때

        AXT_ERROR_SPLINE_INVALID_AXIS_NO = 4180,     // 스플라인 축 맵핑 시 유효한 축이 아닐 때
        AXT_ERROR_SPLINE_INVALID_MAP_NO = 4181,     // 스플라인 맵핑 시 유효한 맵핑 번호가 아닐 때
        AXT_ERROR_SPLINE_EMPTY_MAP_NO = 4182,     // 스플라인 맵핑 번호가 비워있을 때
        AXT_ERROR_SPLINE_NUM_ERROR = 4183,     // 스플라인 점숫자가 부적당할 때
        AXT_RT_MOTION_INTERPOL_VALUE = 4184,     // 보간할 때 입력 값이 잘못넣어졌을 때
        AXT_RT_ERROR_NOT_CONTIBEGIN = 4185,     // 연속보간 할 때 CONTIBEGIN함수를 호출하지 않을 때
        AXT_RT_ERROR_NOT_CONTIEND = 4186,     // 연속보간 할 때 CONTIEND함수를 호출하지 않을 때

        AXT_RT_MOTION_HOME_SEARCHING = 4201,     // 홈을 찾고 있는 중일 때 다른 모션 함수들을 사용할 때
        AXT_RT_MOTION_HOME_ERROR_SEARCHING = 4202,     // 홈을 찾고 있는 중일 때 외부에서 사용자나 혹은 어떤것에 의한  강제로 정지당할 때
        AXT_RT_MOTION_HOME_ERROR_START = 4203,     // 초기화 문제로 홈시작 불가할 때
        AXT_RT_MOTION_HOME_ERROR_GANTRY = 4204,     // 홈을 찾고 있는 중일 때 겐트리 enable 불가할 때

        AXT_RT_MOTION_READ_ALARM_WAITING = 4210,     // 서보팩으로부터 알람코드 결과를 기다리는 중
        AXT_RT_MOTION_READ_ALARM_NO_REQUEST = 4211,     // 서보팩에 알람코드 반환 명령이 내려지지않았을 때
        AXT_RT_MOTION_READ_ALARM_TIMEOUT = 4212,     // 서보팩 알람읽기 시간초과 했을때(1sec이상)
        AXT_RT_MOTION_READ_ALARM_FAILED = 4213,     // 서보팩 알람읽기에 실패 했을 때
        AXT_RT_MOTION_READ_ALARM_UNKNOWN = 4220,     // 알람코드가 알수없는 코드일 때
        AXT_RT_MOTION_READ_ALARM_FILES = 4221,     // 알람정보 파일이 정해진위치에 존재하지 않을 때 

        AXT_RT_MOTION_POSITION_OUTOFBOUND = 4251,     // 설정한 위치값이 설정 최대값보다 크거나 최소값보다 작은값임 
        AXT_RT_MOTION_PROFILE_INVALID = 4252,     // 구동 속도 프로파일 설정이 잘못됨
        AXT_RT_MOTION_VELOCITY_OUTOFBOUND = 4253,     // 구동 속도값이 최대값보다 크게 설정됨
        AXT_RT_MOTION_MOVE_UNIT_IS_ZERO = 4254,     // 구동 단위값이 0으로 설정됨
        AXT_RT_MOTION_SETTING_ERROR = 4255,     // 속도, 가속도, 저크, 프로파일 설정이 잘못됨
        AXT_RT_MOTION_IN_CONT_INTERPOL = 4256,     // 연속 보간 구동 중 구동 시작 또는 재시작 함수를 실행하였음
        AXT_RT_MOTION_DISABLE_TRIGGER = 4257,     // 트리거 출력이 Disable 상태임 
        AXT_RT_MOTION_INVALID_CONT_INDEX = 4258,     // 연속 보간 Index값 설정이 잘못됨
        AXT_RT_MOTION_CONT_QUEUE_FULL = 4259,     // 모션 칩의 연속 보간 큐가 Full 상태임
        AXT_RT_PROTECTED_DURING_SERVOON = 4260,     // 서보 온 되어 있는 상태에서 사용 못 함
        AXT_RT_HW_ACCESS_ERROR = 4261,     // 메모리 Read / Write 실패 

        AXT_RT_HW_DPRAM_CMD_WRITE_ERROR_LV1 = 4262,     // DPRAM Comamnd Write 실패 Level1
        AXT_RT_HW_DPRAM_CMD_WRITE_ERROR_LV2 = 4263,     // DPRAM Comamnd Write 실패 Level2
        AXT_RT_HW_DPRAM_CMD_WRITE_ERROR_LV3 = 4264,     // DPRAM Comamnd Write 실패 Level3
        AXT_RT_HW_DPRAM_CMD_READ_ERROR_LV1 = 4265,     // DPRAM Comamnd Read 실패 Level1
        AXT_RT_HW_DPRAM_CMD_READ_ERROR_LV2 = 4266,     // DPRAM Comamnd Read 실패 Level2
        AXT_RT_HW_DPRAM_CMD_READ_ERROR_LV3 = 4267,     // DPRAM Comamnd Read 실패 Level3

        AXT_RT_COMPENSATION_SET_PARAM_FIRST = 4300,     // 보정 파라미터 중 첫번째 값이 잘못 설정되었음
        AXT_RT_COMPENSATION_NOT_INIT = 4301,     // 보정테이블 기능 초기화 되지않음
        AXT_RT_COMPENSATION_POS_OUTOFBOUND = 4302,     // 위치 값이 범위내에 존재하지 않음
        AXT_RT_COMPENSATION_BACKLASH_NOT_INIT = 4303,     // 백래쉬 보정기능 초기화 되지않음
        AXT_RT_COMPENSATION_INVALID_ENTRY = 4304,     //

        AXT_RT_SEQ_NOT_IN_SERVICE = 4400,     // 순차 구동 함수 실행 중 자원 할당 실패
        AXT_ERROR_SEQ_INVALID_MAP_NO = 4401,     // 순차 구동 함수 실행 중 맵핑 번호 이상.
        AXT_ERROR_INVALID_AXIS_NO = 4402,     // 함수 설정 인자중 축번호 이상.
        AXT_RT_ERROR_NOT_SEQ_NODE_BEGIN = 4403,     // 순차 구동 노드 입력 시작 함수를 호출하지 않음.
        AXT_RT_ERROR_NOT_SEQ_NODE_END = 4404,     // 순차 구동 노드 입력 종료 함수를 호출하지 않음.
        AXT_RT_ERROR_NO_NODE = 4405,     // 순차 구동 노드 입력이 없음.
        AXT_RT_ERROR_SEQ_STOP_TIMEOUT = 4406,

        AXT_RT_ERROR_RING_COUNTER_ENABLE = 4420,    // Ring Counter 기능이 사용 중
        AXT_RT_ERROR_RING_COUNTER_OUT_OF_RANGE = 4421,    // Ring Counter 사용 중 범위 밖 명령 위치 호출(POS_ABS_LONG_MODE or POS_ABS_SHORT_MODE 일 경우)
        AXT_RT_ERROR_SOFT_LIMIT_ENABLE = 4430,    // Software Limit 기능이 사용 중
        AXT_RT_ERROR_SOFT_LIMIT_NEGATIVE = 4431,    // 이동할 위치가 Negative Software Limit을 벗어남
        AXT_RT_ERROR_SOFT_LIMIT_POSITIVE = 4432,    // 이동할 위치가 Positive Software Limit을 벗어남

        AXT_RT_M3_COMMUNICATION_FAILED = 4500,    // ML3 통신 기준, 통신 실패
        AXT_RT_MOTION_ONE_OF_AXES_IS_NOT_M3 = 4501,    // ML3 통신 기준, 구성된 ML3 노드 중에서 모션 노드 없음 
        AXT_RT_MOTION_BIGGER_VEL_THEN_MAX_VEL = 4502,    // ML3 통신 기준, 지정된 축의 설정된 최대 속도보다 큼
        AXT_RT_MOTION_SMALLER_VEL_THEN_MAX_VEL = 4503,    // ML3 통신 기준, 지정된 축의 설정된 최대 속도보다 작음
        AXT_RT_MOTION_ACCEL_MUST_BIGGER_THEN_ZERO = 4504,    // ML3 통신 기준, 지정된 축의 설정된 가속도가 0보다 큼
        AXT_RT_MOTION_SMALL_ACCEL_WITH_UNIT_PULSE = 4505,    // ML3 통신 기준, UnitPulse가 적용된 가속도가 0보다 큼
        AXT_RT_MOTION_INVALID_INPUT_ACCEL = 4506,    // ML3 통신 기준, 지정된 축의 가속도 입력이 잘못됨
        AXT_RT_MOTION_SMALL_DECEL_WITH_UNIT_PULSE = 4507,    // ML3 통신 기준, UnitPulse가 적용된 감속도가 0보다 큼
        AXT_RT_MOTION_INVALID_INPUT_DECEL = 4508,    // ML3 통신 기준, 지정된 축의 감속도 입력이 잘못됨
        AXT_RT_MOTION_SAME_START_AND_CENTER_POS = 4509,    // ML3 통신 기준, 원호보간의 시작점과 중심점이 같음
        AXT_RT_MOTION_INVALID_JERK = 4510,    // ML3 통신 기준, 지정된 축의 저크 입력이 잘못됨
        AXT_RT_MOTION_INVALID_INPUT_VALUE = 4511,    // ML3 통신 기준, 지정된 축의 입력값이 잘못됨
        AXT_RT_MOTION_NOT_SUPPORT_PROFILE = 4512,    // ML3 통신 기준, 제공되지 않는 속도 프로파일임
        AXT_RT_MOTION_INPOS_UNUSED = 4513,    // ML3 통신 기준, 인포지션 사용하지 않음
        AXT_RT_MOTION_AXIS_IN_SLAVE_STATE = 4514,    // ML3 통신 기준, 지정된 축이 슬레이브 상태가 아님
        AXT_RT_MOTION_AXES_ARE_NOT_SAME_BOARD = 4515,    // ML3 통신 기준, 지정된 축들이 같은 보드 내에 있지 않음
        AXT_RT_MOTION_ERROR_IN_ALARM = 4516,    // ML3 통신 기준, 지정된 축이 알람 상태임
        AXT_RT_MOTION_ERROR_IN_EMGN = 4517,    // ML3 통신 기준, 지정된 축이 비상정지 상태임
        AXT_RT_MOTION_CAN_NOT_CHANGE_COORD_NO = 4518,    // ML3 통신 기준, 코디네이터 넘버 변환 불가임
        AXT_RT_MOTION_INVALID_INTERNAL_RADIOUS = 4519,    // ML3 통신 기준, 원호보간의 X, Y축 반지름 불일치
        AXT_RT_MOTION_CONTI_QUEUE_FULL = 4521,    // ML3 통신 기준, 보간의 큐가 가득 참
        AXT_RT_MOTION_SAME_START_AND_END_POSITION = 4522,    // ML3 통신 기준, 원호보간의 시작점과 종료점이 같음
        AXT_RT_MOTION_INVALID_ANGLE = 4523,    // ML3 통신 기준, 원호보간의 각도가 360도 초과됨
        AXT_RT_MOTION_CONTI_QUEUE_EMPTY = 4524,    // ML3 통신 기준, 보간의 큐가 비어있음
        AXT_RT_MOTION_ERROR_GEAR_ENABLE = 4525,    // ML3 통신 기준, 지정된 축이 이미 링크 설정 상태임
        AXT_RT_MOTION_ERROR_GEAR_AXIS = 4526,    // ML3 통신 기준, 지정된 축이 링크축이 아님
        AXT_RT_MOTION_ERROR_NO_GANTRY_ENABLE = 4527,    // ML3 통신 기준, 지정된 축이 겐트리 설정 상태가 아님
        AXT_RT_MOTION_ERROR_NO_GEAR_ENABLE = 4528,    // ML3 통신 기준, 지정된 축이 링크 설정 상태가 아님
        AXT_RT_MOTION_ERROR_GANTRY_ENABLE_FULL = 4529,    // ML3 통신 기준, 겐트리 설정 가득참
        AXT_RT_MOTION_ERROR_GEAR_ENABLE_FULL = 4530,    // ML3 통신 기준, 링크 설정 가득참
        AXT_RT_MOTION_ERROR_NO_GANTRY_SLAVE = 4531,    // ML3 통신 기준, 지정된 축이 겐트리 슬레이브 설정상태가 아님
        AXT_RT_MOTION_ERROR_NO_GEAR_SLAVE = 4532,    // ML3 통신 기준, 지정된 축이 링크 슬레이브 설정상태가 아님
        AXT_RT_MOTION_ERROR_MASTER_SLAVE_SAME = 4533,    // ML3 통신 기준, 마스터축과 슬레이브 축이 동일함
        AXT_RT_MOTION_NOT_SUPPORT_HOMESIGNAL = 4534,    // ML3 통신 기준, 지정된 축의 홈신호는 지원되지 않음
        AXT_RT_MOTION_ERROR_NOT_SYNC_CONNECT = 4535,    // ML3 통신 기준, 지정된 축이 싱크 연결 상태가 아님
        AXT_RT_MOTION_OVERFLOW_POSITION = 4536,    // ML3 통신 기준, 지정된 축에 대한 구동 위치값이 오버플로우임
        AXT_RT_MOTION_ERROR_INVALID_CONTIMAPAXIS = 4537,    // ML3 통신 기준, 보간작업을 위한 지정된 좌표계 축맵핑이 없음
        AXT_RT_MOTION_ERROR_INVALID_CONTIMAPSIZE = 4538,    // ML3 통신 기준, 보간작업을 ㅜ이한 지정된 좌표계 축맵핑 축사이즈가 잘못됨
        AXT_RT_MOTION_ERROR_IN_SERVO_OFF = 4539,    // ML3 통신 기준, 지정된 축이 서보 OFF되어 있음
        AXT_RT_MOTION_ERROR_POSITIVE_LIMIT = 4540,    // ML3 통신 기준, 지정된 축이 (+)리밋 ON되어 있음
        AXT_RT_MOTION_ERROR_NEGATIVE_LIMIT = 4541,    // ML3 통신 기준, 지정된 축이 (-)리밋 ON되어 있음
        AXT_RT_MOTION_ERROR_OVERFLOW_SWPROFILE_NUM = 4542,    // ML3 통신 기준, 지정된 축들에 대한 지원 프로파일 개수가 오버플로우됨
        AXT_RT_PROTECTED_DURING_INMOTION = 4543,    // in_motion 되어 있는 상태에서 사용 못 함

        AXT_ERROR_SYNC_INVALID_AXIS_NO = 4600,    // Sync 축맵핑 시 유효한 축이 아닐 때
        AXT_ERROR_SYNC_INVALID_MAP_NO = 4601,    // Sync 맵핑 시 유효한 맵핑 번호가 아닐 때
        AXT_ERROR_SYNC_DUPLICATED_TIME = 4602,    // Time table이 중복되었을 때

        AXT_RT_DATA_FLASH_NOT_EXIST = 5000,    // 플래시 메모리가 존재하지 않음
        AXT_RT_DATA_FLASH_BUSY = 5001,    // 플래시 메모리가 사용 중

        AXT_RT_QUEUE_CMD_ERROR = 5010,
        AXT_RT_QUEUE_CMD_WAIT_ERROR = 5011,
        AXT_RT_QUEUE_CMD_WAIT_TIMEOUT = 5012,

        AXT_RT_QUEUE_RSP_ERROR = 5015,
        AXT_RT_QUEUE_RSP_WAIT_ERROR = 5016,
        AXT_RT_QUEUE_RSP_WAIT_TIMEOUT = 5017,

        AXT_RT_MOTION_STILL_CONTI_MOTION = 5018,        // 연속보간 구동 중에 WriteClear나 SetAxisMap 등의 함수를 호출하였음.

        AXT_RT_MOTION_INVALD_SET = 6000,
        AXT_RT_MOTION_INVALD_RESET = 6001,
        AXT_RT_MOTION_INVALD_ENABLE = 6002,

        AXT_RT_LICENSE_INVALID = 6500,        // 유효하지않은 License

        AXT_RT_MONITOR_IN_OPERATION = 6600,        // 현재 Monitor 기능이 동작중에 있음
        AXT_RT_MONITOR_NOT_OPERATION = 6601,        // 현재 Monitor 기능이 동작중이지 않음
        AXT_RT_MONITOR_EMPTY_QUEUE = 6602,        // Monitor data queue가 비어있음
        AXT_RT_MONITOR_INVALID_TRIGGER_OPTION = 6603,        // 트리거 설정이 유효하지 않음
        AXT_RT_MONITOR_EMPTY_ITEM = 6604,        // Item이 비어 있음

        AXT_RT_MACRO_INVALID_MACRO_NO = 6700,
        AXT_RT_MACRO_INVALID_NODE_NO = 6701,
        AXT_RT_MACRO_INVALID_STOP_MODE = 6702,
        AXT_RT_MACRO_MEMORY_MISMATCH = 6703,
        AXT_RT_MACRO_CONTROL_LOCKED = 6704,
        AXT_RT_MACRO_INVALID_STATUS = 6705,
        AXT_RT_MACRO_INVALID_ARGUMENT = 6706,

        AXT_RT_MACRO_NOT_NODE_BEGIN = 6710,
        AXT_RT_MACRO_NOT_NODE_END = 6711,
        AXT_RT_MACRO_ALREADY_BEGIN = 6712,
        AXT_RT_MACRO_NODE_EMPTY = 6713,
        AXT_RT_MACRO_IN_OPERATION = 6714,
        AXT_RT_MACRO_NOT_OPERATION = 6715,
        AXT_RT_MACRO_NOT_SUPPORT_FUNCTION = 6716,
        AXT_RT_MACRO_NODE_FULL = 6717,
        AXT_RT_MACRO_NODE_CHECK_ERROR = 6720,
        AXT_RT_MACRO_NOT_CHECKED = 6721,
        AXT_RT_MACRO_NOT_PAUSED = 6722,         // Macro가 Pause 상태가 아닐때	  


        AXT_MK_RT_INVALID_AXIS = 7100,
        AXT_MK_RT_INVALID_AXIS_SIZE = 7101,
        AXT_MK_RT_INVALID_COORD = 7102,
        AXT_MK_RT_INVALID_COORD_SIZE = 7103,
        AXT_MK_RT_INVALID_AXIS_MAP = 7104,
        AXT_MK_RT_INVALID_AXIS_MAP_SIZE = 7105,
        AXT_MK_RT_INVALID_VEL = 7106,
        AXT_MK_RT_INVALID_END_VEL = 7107,
        AXT_MK_RT_INVALID_ACCEL = 7108,
        AXT_MK_RT_INVALID_DECEL = 7109,
        AXT_MK_RT_INVALID_ABS_REL = 7110,
        AXT_MK_RT_INVALID_PROFILE = 7111,
        AXT_MK_RT_INVALID_STOP_DECEL = 7112,
        AXT_MK_RT_INVALID_STOP_TIME = 7113,
        AXT_MK_RT_INVALID_ACCEL_JERK_RATE = 7114,
        AXT_MK_RT_INVALID_DECEL_JERK_RATE = 7115,
        AXT_MK_RT_INVALID_ACCEL_UNIT = 7116,
        AXT_MK_RT_INVALID_DISTANCE = 7117,
        AXT_MK_RT_INVALID_ANGLE = 7118,
        AXT_MK_RT_INVALID_BIT = 7119,
        AXT_MK_RT_INVALID_PORT = 7120,
        AXT_MK_RT_INVALID_SPLINE_INDEX = 7121,
        AXT_MK_RT_INVALID_THREAD = 7122,
        AXT_MK_RT_INVALID_TIMER = 7123,
        AXT_MK_RT_INVALID_SEGMENT_COUNT = 7124,
        AXT_MK_RT_INVALID_SEGMENT_NO = 7125,
        AXT_MK_RT_INVALID_NODE_NO = 7126,
        AXT_MK_RT_INVALID_HWQ_COUNT = 7127,
        AXT_MK_RT_INVALID_NODE_SIZE = 7128,
        AXT_MK_RT_INVALID_STOP_NODE_SIZE = 7129,
        AXT_MK_RT_INVALID_SPLINE_SIZE = 7130,
        AXT_MK_RT_INVALID_LINE_LINE_FILLET = 7131,
        AXT_MK_RT_INVALID_LINE_ARC_FILLET = 7132,
        AXT_MK_RT_INVALID_ARC_LINE_FILLET = 7133,
        AXT_MK_RT_INVALID_ARC_ARC_FILLET = 7134,
        AXT_MK_RT_INVALID_RESET_FILLET = 7135,
        AXT_MK_RT_INVALID_TASK = 7136,
        AXT_MK_RT_INVALID_ROUND_INDEX = 7137,
        AXT_MK_RT_INVALID_LOG_DATA = 7138,
        AXT_MK_RT_INVALID_LOG10_DATA = 7139,
        AXT_MK_RT_INVALID_PORT_NO = 7140,
        AXT_MK_RT_INVALID_BAUD_RATE = 7141,
        AXT_MK_RT_INVALID_STOP_BIT = 7142,
        AXT_MK_RT_INVALID_PARITY = 7143,
        AXT_MK_RT_INVALID_EDGE = 7144,
        AXT_MK_RT_INVALID_STOP_MODE = 7145,
        AXT_MK_RT_INVALID_TRIGGER_TIME = 7146,
        AXT_MK_RT_INVALID_TRIGGER_LEVEL = 7147,
        AXT_MK_RT_INVALID_TRIGGER_SELECT = 7148,
        AXT_MK_RT_INVALID_TRIGGER_INTERRUPT = 7149,
        AXT_MK_RT_INVALID_TRIGGER_METHOD = 7150,
        AXT_MK_RT_INVALID_TRIGGER_POSITION = 7151,
        AXT_MK_RT_INVALID_TRIGGER_INDEX = 7152,
        AXT_MK_RT_INVALID_ECAM_DATA = 7153,
        AXT_MK_RT_INVALID_ECAM_POSITION = 7154,
        AXT_MK_RT_INVALID_EGEAR_SIZE = 7155,
        AXT_MK_RT_INVALID_INDEX = 7156,
        AXT_MK_RT_INVALID_MOTION_MODE = 7157,
        AXT_MK_RT_INVALID_SIGNAL = 7158,
        AXT_MK_RT_INVALID_STOP_DISTANCE = 7159,
        AXT_MK_RT_INVALID_DIRECTION = 7160,
        AXT_MK_RT_INVALID_ZERO_VELOCITY = 7161,
        AXT_MK_RT_INVALID_COORDINATE_INMOTION = 7162,
        AXT_MK_RT_INVALID_COORDINATE_MOTIONDONE = 7163,
        AXT_MK_RT_INVALID_BLENDING_MODE = 7164,
        AXT_MK_RT_INVALID_BLENDING_VALUE = 7165,
        AXT_MK_RT_INVALID_BLENDING_RATIO = 7166,
        AXT_MK_RT_INVALID_EGEAR_RATIO = 7167,
        AXT_MK_RT_INVALID_SLAVE_AXIS = 7168,
        AXT_MK_RT_INVALID_OPERATION_MODE = 7169,

        AXT_MK_RT_INVALID_CONTIQ_DISABLE = 7170,
        AXT_MK_RT_INVALID_CONTIQ_MODE = 7171,
        AXT_MK_RT_INVALID_CONTIQ_ANGLE = 7172,
        AXT_MK_RT_INVALID_CONTIQ_VELRATE = 7173,
        AXT_MK_RT_INVALID_CONTIQ_FILLET = 7174,

        AXT_MK_RT_CONTIQ_AUTO_VEL = 7175,
        AXT_MK_RT_CONTIQ_AUTO_ARC = 7176,
        AXT_MK_RT_CONTIQ_LINE = 7177,
        AXT_MK_RT_CONTIQ_CIRCLE = 7178,
        AXT_MK_RT_CONTIQ_ARC = 7179,
        AXT_MK_RT_CONTIQ_SYNC_SLAVE = 7180,
        AXT_MK_RT_INVALID_SEGMENT_OUTPUT_SIZE = 7181,
        AXT_MK_RT_INVALID_SEGMENT_NUMBER = 7182,
        AXT_MK_RT_INVALID_TRIG_COUNT = 7183,
        AXT_MK_RT_INVALID_TRIG_TIME = 7184,
        AXT_MK_RT_NOT_CAPTURED = 7185,
        AXT_MK_RT_INVALID_CAPTURE_INDEX = 7186,
        AXT_MK_RT_INVALID_LEVEL = 7187,
        AXT_MK_RT_INVALID_SEGMENT_OUTPUT_MODE = 7188,
        AXT_MK_RT_INVALID_SEGMENT_OUTPUT_VALUE = 7189,
        AXT_MK_RT_INVALID_SEGMENT_OUTPUT_RATIO = 7190,

        AXT_MK_RT_INVALID_SPLINE_POINT_SIZE = 7191,
        AXT_MK_RT_INVALID_INMOTION = 7192,

        AXT_MK_RT_ENABLE_CONTIQ = 7200,
        AXT_MK_RT_DISABLE_CONTIQ = 7201,
        AXT_MK_RT_ENABLE_CONTIQ_SYNC = 7202,
        AXT_MK_RT_DISABLE_CONTIQ_SYNC = 7203,
        AXT_MK_RT_ENABLE_CONTI = 7204,
        AXT_MK_RT_DISABLE_CONTI = 7205,
        AXT_MK_RT_ENABLE_EGEAR = 7206,
        AXT_MK_RT_DISABLE_EGEAR = 7207,
        AXT_MK_RT_ENABLE_TASK = 7208,
        AXT_MK_RT_DISABLE_TASK = 7209,
        AXT_MK_RT_DISABLE_PORT_NO = 7210,

        AXT_MK_RT_ALREADY_OPEN = 7300,
        AXT_MK_RT_ALREADY_CLOSE = 7301,

        AXT_MK_RT_ERROR_HOME = 7400,
        AXT_MK_RT_ERROR_MOTION = 7401,
        AXT_MK_RT_ERROR_INSTOPPING = 7402,
        AXT_MK_RT_ERROR_TIME_OUT = 7403,
        AXT_MK_RT_ERROR_BUFFER_FULL = 7404,
        AXT_MK_RT_ERROR_DATA_CREATE = 7405,
        AXT_MK_RT_ERROR_CALCULATION = 7406,

        AXT_MK_RT_ERROR_QUEUE_FULL = 7500,
        AXT_MK_RT_ERROR_QUEUE_NULL = 7501,

        AXT_MK_RT_ERROR_ECAM_TABLE = 7502,

        AXT_MK_RT_ERROR_SPLINE_POSITION = 7503,

        AXT_MK_RT_INVALID_CIRCULAR_POINT = 7510,
        AXT_MK_RT_INVALID_POINT = 7511,
        AXT_MK_RT_INVALID_QUEUE_SIZE = 7512,
        AXT_MK_RT_INVALID_POSITION = 7513,
        AXT_MK_RT_INVALID_ROTATION = 7514,

        AXT_MK_RT_INVALID_TABLE = 7600,
        AXT_MK_RT_INVALID_TABLE_NO = 7601,
        AXT_MK_RT_INVALID_TABLE_DATA = 7602,
        AXT_MK_RT_INVALID_POSITION_SIZE = 7603,

        AXT_MK_RT_INVALID_TABLE_ENABLED = 7604,
        AXT_MK_RT_INVALID_TABLE_NOT_ENABLED = 7605,
        AXT_MK_RT_INVALID_TABLE_NONE = 7606,
        AXT_MK_RT_INVALID_GET_TABLE = 7607,
        AXT_MK_RT_INVALID_ENABLE_TABLE = 7608,
        AXT_MK_RT_INVALID_DISABLE_TABLE = 7609,

        AXT_MK_RT_INVALID_SET = 7610,
        AXT_MK_RT_INVALID_RESET = 7611,
        AXT_MK_RT_INVALID_ENABLE = 7612,

        AXT_MK_RT_NOT_SUPPORT = 7900,
        AXT_MK_RT_ERROR = 7901,
        AXT_MK_RT_INVLID_FUNCTION_TYPE = 7902,

        AXT_MK_RT_INVALID_ROBOT_SIZE = 7700,
        AXT_MK_RT_INVALID_ROBOT_AXIS_SIZE = 7701,
        AXT_MK_RT_INVALID_ROBOT_COORD_SIZE = 7702,
        AXT_MK_RT_INVALID_ROBOT_NO = 7703,
        AXT_MK_RT_INVALID_ROBOT_COORD = 7704,
        AXT_MK_RT_INVALID_ROBOT_LIMIT = 7705,
        AXT_MK_RT_INVALID_ROBOT_POS_LIMIT = 7706,
        AXT_MK_RT_INVALID_ROBOT_NEG_LIMIT = 7707,
        AXT_MK_RT_INVALID_ROBOT_VEL_LIMIT = 7708,
        AXT_MK_RT_INVALID_ROBOT_ACCEL_LIMIT = 7709,
        AXT_MK_RT_INVALID_ROBOT_DECEL_LIMIT = 7710,
        AXT_MK_RT_ERROR_ROBOT_CALCULATION = 7711,
        AXT_MK_RT_INVALID_FRAME = 7712,
        AXT_MK_RT_INVALID_FRAME_NO = 7713,
        AXT_MK_RT_INVALID_FRAME_TYPE = 7714,
        AXT_MK_RT_INVALID_OBJECT_NO = 7715,
        AXT_MK_RT_INVALID_ROBOT_SYNC = 7716,
        AXT_MK_RT_INVALID_ROBOT_SYNC_MOTION = 7717,
        AXT_MK_RT_INVALID_ROBOT_SYNC_ENABLE = 7718,
        AXT_MK_RT_INVALID_ROBOT_SYNC_DISABLE = 7719,
        AXT_MK_RT_INVALID_ROBOT_SYNC_MOTION_MODE = 7720,
        AXT_MK_RT_INVALID_ROBOT_SYNC_WORK_COORD = 7721,
        AXT_MK_RT_INVALID_CAPTURE_POS_NO = 7722,

        AXT_MK_RT_INVALID_ROBOT_CAPTURE_POS = 7730,
        AXT_MK_RT_INVALID_ROBOT_AXIS = 7731,
        AXT_MK_RT_INVALID_WORK_NEGATIVE_LIMIT = 7732,
        AXT_MK_RT_INVALID_WORK_POSITIVE_LIMIT = 7733,

        AXT_MK_RT_INVALID_TOOL_NO = 7740,

        AXT_MK_RT_INVALID_FREQUENCY_SIZE = 7800,
        AXT_MK_RT_INVALID_IMPULSE_COUNT = 7801,
        AXT_MK_RT_INVALID_AMPLITUDE = 7802,

        AXT_MK_RT_INVALID_INPUT_SHAPER__NONE = 7803,
        AXT_MK_RT_INVALID_INPUT_SHAPER_ENABLED = 7804,

        AXT_MK_RT_INVALID_ARRAY_SIZE = 7805
    }

    public enum AXT_BOOLEAN : uint
    {
        FALSE,
        TRUE
    }

    public enum AXT_LOG_LEVEL : uint
    {
        LEVEL_NONE,
        LEVEL_ERROR,
        LEVEL_RUNSTOP,
        LEVEL_FUNCTION
    }

    public enum AXT_EXISTENCE : uint
    {
        STATUS_NOTEXIST,
        STATUS_EXIST
    }

    public enum AXT_USE : uint
    {
        DISABLE,
        ENABLE
    }

    public enum AXT_AIO_TRIGGER_MODE : uint
    {
        DISABLE_MODE = 0,
        NORMAL_MODE = 1,
        TIMER_MODE = 2,
        EXTERNAL_MODE = 3
    }

    public enum AXT_AIO_FULL_MODE : uint
    {
        NEW_DATA_KEEP,
        CURR_DATA_KEEP
    }

    public enum AXT_AIO_EVENT_MASK : uint
    {
        DATA_EMPTY = 0x01,
        DATA_MANY = 0x02,
        DATA_SMAL = 0x04,
        DATA_FULL = 0x08
    }

    public enum AXT_AIO_INTERRUPT_MASK : uint
    {
        ADC_DONE = 0x01,
        SCAN_END = 0x02,
        FIFO_HALF_FULL = 0x02,
        NO_SIGNAL = 0x03
    }

    public enum AXT_AIO_EVENT_MODE : uint
    {
        AIO_EVENT_DATA_RESET = 0x00,
        AIO_EVENT_DATA_UPPER = 0x01,
        AIO_EVENT_DATA_LOWER = 0x02,
        AIO_EVENT_DATA_FULL = 0x03,
        AIO_EVENT_DATA_EMPTY = 0x04
    }

    public enum AXT_AIO_FIFO_STATUS : uint
    {
        FIFO_DATA_EXIST = 0,
        FIFO_DATA_EMPTY = 1,
        FIFO_DATA_HALF = 2,
        FIFO_DATA_FULL = 6
    }

    public enum AXT_AIO_EXTERNAL_STATUS : uint
    {
        EXTERNAL_DATA_DONE = 0,
        EXTERNAL_DATA_FINE = 1,
        EXTERNAL_DATA_HALF = 2,
        EXTERNAL_DATA_FULL = 3,
        EXTERNAL_COMPLETE = 4
    }

    public enum AXT_DIO_EDGE : uint
    {
        DOWN_EDGE,
        UP_EDGE
    }

    public enum AXT_DIO_STATE : uint
    {
        OFF_STATE,
        ON_STATE
    }

    public enum AXT_MOTION_STOPMODE : uint
    {
        EMERGENCY_STOP,
        SLOWDOWN_STOP
    }

    public enum AXT_MOTION_EDGE : uint
    {
        SIGNAL_DOWN_EDGE,
        SIGNAL_UP_EDGE,
        SIGNAL_LOW_LEVEL,
        SIGNAL_HIGH_LEVEL
    }

    public enum AXT_MOTION_SELECTION : uint
    {
        COMMAND,
        ACTUAL
    }

    public enum AXT_MOTION_TRIGGER_MODE : uint
    {
        PERIOD_MODE,
        ABS_POS_MODE
    }

    public enum AXT_MOTION_LEVEL_MODE : uint
    {
        LOW,
        HIGH,
        UNUSED,
        USED
    }

    public enum AXT_MOTION_ABSREL : uint
    {
        POS_ABS_MODE,
        POS_REL_MODE,
        POS_ABS_LONG_MODE
    }

    public enum AXT_MOTION_PROFILE_MODE : uint
    {
        SYM_TRAPEZOIDE_MODE,
        ASYM_TRAPEZOIDE_MODE,
        QUASI_S_CURVE_MODE,
        SYM_S_CURVE_MODE,
        ASYM_S_CURVE_MODE
    }

    public enum AXT_MOTION_SIGNAL_LEVEL : uint
    {
        INACTIVE,
        ACTIVE
    }

    public enum AXT_MOTION_HOME_RESULT : uint
    {
        HOME_RESERVED = 0x00,    // ML3
        HOME_SUCCESS = 0x01,    // 원점 검색 완료
        HOME_SEARCHING = 0x02,    // 원점 검색 중
        HOME_ERR_GNT_RANGE = 0x10,    // 갠트리 원점 검색 기준, 두 축 사이의 설정이상 오차 발생
        HOME_ERR_USER_BREAK = 0x11,    // 원점 검색 사용자 중지시
        HOME_ERR_VELOCITY = 0x12,    // 원점 검색 속도 이상 에러 발생
        HOME_ERR_AMP_FAULT = 0x13,    // 서보팩 알람 발생 에러
        HOME_ERR_NEG_LIMIT = 0x14,    // (-)방향 구동중 (+)리미트 센서 감지 에러
        HOME_ERR_POS_LIMIT = 0x15,    // (+)방향 구동중 (-)리미트 센서 감지 에러
        HOME_ERR_NOT_DETECT = 0x16,    // 지정한 신호 검출하지 못 할 경우 에러
        HOME_ERR_SETTING = 0x17,    // 사용자 설정 파라미터가 구동시 제약 조건 발생시
        HOME_ERR_SERVO_OFF = 0x18,    // 서보 Off일경우
        HOME_ERR_TIMEOUT = 0x20,    // 지정된 시간 초과 발생으로 오류 발생 
        HOME_ERR_FUNCALL = 0x30,    // 함수 호출 실패
        HOME_ERR_COUPLING = 0x40,    // Gantry Master to Slave Over Distance protection
        HOME_ERR_UNKNOWN = 0xFF     // 미지정 에러
    }

    public enum AXT_MOTION_UNIV_INPUT : uint
    {
        UIO_INP0,
        UIO_INP1,
        UIO_INP2,
        UIO_INP3,
        UIO_INP4,
        UIO_INP5
    }

    public enum AXT_MOTION_UNIV_OUTPUT : uint
    {
        UIO_OUT0,
        UIO_OUT1,
        UIO_OUT2,
        UIO_OUT3,
        UIO_OUT4,
        UIO_OUT5
    }

    public enum AXT_MOTION_DETECT_DOWN_START_POINT : uint
    {
        AutoDetect,
        RestPulse
    }

    public enum AXT_MOTION_PULSE_OUTPUT : uint
    {
        OneHighLowHigh,                // 1펄스 방식, PULSE(Active High), 정방향(DIR=Low)  / 역방향(DIR=High)
        OneHighHighLow,                 // 1펄스 방식, PULSE(Active High), 정방향(DIR=High) / 역방향(DIR=Low)
        OneLowLowHigh,                  // 1펄스 방식, PULSE(Active Low),  정방향(DIR=Low)  / 역방향(DIR=High)
        OneLowHighLow,                  // 1펄스 방식, PULSE(Active Low),  정방향(DIR=High) / 역방향(DIR=Low)
        TwoCcwCwHigh,                   // 2펄스 방식, PULSE(CCW:역방향),  DIR(CW:정방향),  Active High     
        TwoCcwCwLow,                    // 2펄스 방식, PULSE(CCW:역방향),  DIR(CW:정방향),  Active Low     
        TwoCwCcwHigh,                   // 2펄스 방식, PULSE(CW:정방향),   DIR(CCW:역방향), Active High
        TwoCwCcwLow,                    // 2펄스 방식, PULSE(CW:정방향),   DIR(CCW:역방향), Active Low
        TwoPhase,                       // 2상(90' 위상차),  PULSE lead DIR(CW: 정방향), PULSE lag DIR(CCW:역방향)
        TwoPhaseReverse                 // 2상(90' 위상차),  PULSE lead DIR(CCW: 정방향), PULSE lag DIR(CW:역방향)
    }

    public enum AXT_MOTION_EXTERNAL_COUNTER_INPUT : uint
    {
        ObverseUpDownMode,              // 정방향 Up/Down
        ObverseSqr1Mode,                // 정방향 1체배
        ObverseSqr2Mode,                // 정방향 2체배
        ObverseSqr4Mode,                // 정방향 4체배
        ReverseUpDownMode,              // 역방향 Up/Down
        ReverseSqr1Mode,                // 역방향 1체배
        ReverseSqr2Mode,                // 역방향 2체배
        ReverseSqr4Mode                 // 역방향 4체배
    }

    public enum AXT_MOTION_ACC_UNIT : uint
    {
        UNIT_SEC2 = 0x0,                // unit/sec2
        SEC = 0x1                 // sec
    }

    public enum AXT_MOTION_MOVE_DIR : uint
    {
        DIR_CCW = 0x0,                // 반시계방향
        DIR_CW = 0x1                 // 시계방향
    }

    public enum AXT_MOTION_RADIUS_DISTANCE : uint
    {
        SHORT_DISTANCE = 0x0,          // 짧은 거리의 원호 이동 
        LONG_DISTANCE = 0x1           // 긴 거리의 원호 이동 
    }

    public enum AXT_MOTION_POS_TYPE : uint
    {
        POSITION_LIMIT = 0x0,         // 전체 영역사용
        POSITION_BOUND = 0x1          // Pos 지정 사용
    }

    public enum AXT_MOTION_INTERPOLATION_AXIS : uint
    {
        INTERPOLATION_AXIS2 = 0x2,    // 2축을 보간으로 사용할 때
        INTERPOLATION_AXIS3 = 0x3,    // 3축을 보간으로 사용할 때
        INTERPOLATION_AXIS4 = 0x4     // 4축을 보간으로 사용할 때
    }

    public enum AXT_MOTION_CONTISTART_NODE : uint
    {
        CONTI_NODE_VELOCITY = 0x0,           // 속도 지정 보간 모드
        CONTI_NODE_MANUAL = 0x1,           // 노드 가감속 보간 모드
        CONTI_NODE_AUTO = 0x2            // 자동 가감속 보간 모드
    }

    public enum AXT_MOTION_HOME_DETECT : uint
    {
        PosEndLimit = 0x0,           // +Elm(End limit) +방향 리미트 센서 신호
        NegEndLimit = 0x1,           // -Elm(End limit) -방향 리미트 센서 신호
        PosSloLimit = 0x2,           // +Slm(Slow Down limit) 신호 - 사용하지 않음
        NegSloLimit = 0x3,           // -Slm(Slow Down limit) 신호 - 사용하지 않음
        HomeSensor = 0x4,           // IN0(ORG)  원점 센서 신호
        EncodZPhase = 0x5,           // IN1(Z상)  Encoder Z상 신호
        UniInput02 = 0x6,           // IN2(범용) 범용 입력 2번 신호
        UniInput03 = 0x7,           // IN3(범용) 범용 입력 3번 신호
        UniInput04 = 0x8,           // IN4(범용) 범용 입력 4번 신호
        UniInput05 = 0x9            // IN5(범용) 범용 입력 5번 신호
    }

    public enum AXT_MOTION_INPUT_FILTER_SIGNAL_DEF : uint
    {
        END_LIMIT = 0x10,          // 위치클리어 사용않함, 잔여펄스 클리어 사용 안함
        INP_ALARM = 0x11,          // 위치클리어 사용함, 잔여펄스 클리어 사용 안함
        UIN_00_01 = 0x12,          // 위치클리어 사용안함, 잔여펄스 클리어 사용함
        UIN_02_04 = 0x13           // 위치클리어 사용함, 잔여펄스 클리어 사용함
    }

    public enum AXT_MOTION_MPG_INPUT_METHOD : uint
    {
        MPG_DIFF_ONE_PHASE = 0x0,           // MPG 입력 방식 One Phase
        MPG_DIFF_TWO_PHASE_1X = 0x1,           // MPG 입력 방식 TwoPhase1
        MPG_DIFF_TWO_PHASE_2X = 0x2,           // MPG 입력 방식 TwoPhase2
        MPG_DIFF_TWO_PHASE_4X = 0x3,           // MPG 입력 방식 TwoPhase4
        MPG_LEVEL_ONE_PHASE = 0x4,           // MPG 입력 방식 Level One Phase
        MPG_LEVEL_TWO_PHASE_1X = 0x5,           // MPG 입력 방식 Level Two Phase1
        MPG_LEVEL_TWO_PHASE_2X = 0x6,           // MPG 입력 방식 Level Two Phase2
        MPG_LEVEL_TWO_PHASE_4X = 0x7            // MPG 입력 방식 Level Two Phase4
    }

    public enum AXT_MOTION_SENSOR_INPUT_METHOD : uint
    {
        SENSOR_METHOD1 = 0x0,           // 일반 구동
        SENSOR_METHOD2 = 0x1,           // 센서 신호 검출 전은 저속 구동. 신호 검출 후 일반 구동
        SENSOR_METHOD3 = 0x2            // 저속 구동
    }

    public enum AXT_MOTION_HOME_CRC_SELECT : uint
    {
        CRC_SELECT1 = 0x0,           // 위치클리어 사용않함, 잔여펄스 클리어 사용 안함
        CRC_SELECT2 = 0x1,           // 위치클리어 사용함, 잔여펄스 클리어 사용 안함
        CRC_SELECT3 = 0x2,           // 위치클리어 사용안함, 잔여펄스 클리어 사용함
        CRC_SELECT4 = 0x3            // 위치클리어 사용함, 잔여펄스 클리어 사용함
    }

    public enum AXT_MOTION_IPDETECT_DESTINATION_SIGNAL : uint
    {
        PElmNegativeEdge = 0x0,           // +Elm(End limit) 하강 edge
        NElmNegativeEdge = 0x1,           // -Elm(End limit) 하강 edge
        PSlmNegativeEdge = 0x2,           // +Slm(Slowdown limit) 하강 edge
        NSlmNegativeEdge = 0x3,           // -Slm(Slowdown limit) 하강 edge
        In0DownEdge = 0x4,           // IN0(ORG) 하강 edge
        In1DownEdge = 0x5,           // IN1(Z상) 하강 edge
        In2DownEdge = 0x6,           // IN2(범용) 하강 edge
        In3DownEdge = 0x7,           // IN3(범용) 하강 edge
        PElmPositiveEdge = 0x8,           // +Elm(End limit) 상승 edge
        NElmPositiveEdge = 0x9,           // -Elm(End limit) 상승 edge
        PSlmPositiveEdge = 0xa,           // +Slm(Slowdown limit) 상승 edge
        NSlmPositiveEdge = 0xb,           // -Slm(Slowdown limit) 상승 edge
        In0UpEdge = 0xc,           // IN0(ORG) 상승 edge
        In1UpEdge = 0xd,           // IN1(Z상) 상승 edge
        In2UpEdge = 0xe,           // IN2(범용) 상승 edge
        In3UpEdge = 0xf            // IN3(범용) 상승 edge
    }

    public enum AXT_MOTION_IPEND_STATUS : uint
    {
        IPEND_STATUS_SLM = 0x0001,        // Bit 0, limit 감속정지 신호 입력에 의한 종료
        IPEND_STATUS_ELM = 0x0002,        // Bit 1, limit 급정지 신호 입력에 의한 종료
        IPEND_STATUS_SSTOP_SIGNAL = 0x0004,        // Bit 2, 감속 정지 신호 입력에 의한 종료
        IPEND_STATUS_ESTOP_SIGANL = 0x0008,        // Bit 3, 급정지 신호 입력에 의한 종료
        IPEND_STATUS_SSTOP_COMMAND = 0x0010,        // Bit 4, 감속 정지 명령에 의한 종료
        IPEND_STATUS_ESTOP_COMMAND = 0x0020,        // Bit 5, 급정지 정지 명령에 의한 종료
        IPEND_STATUS_ALARM_SIGNAL = 0x0040,        // Bit 6, Alarm 신호 입력에 희한 종료
        IPEND_STATUS_DATA_ERROR = 0x0080,        // Bit 7, 데이터 설정 에러에 의한 종료
        IPEND_STATUS_DEVIATION_ERROR = 0x0100,        // Bit 8, 탈조 에러에 의한 종료
        IPEND_STATUS_ORIGIN_DETECT = 0x0200,        // Bit 9, 원점 검출에 의한 종료
        IPEND_STATUS_SIGNAL_DETECT = 0x0400,        // Bit 10, 신호 검출에 의한 종료(Signal search-1/2 drive 종료)
        IPEND_STATUS_PRESET_PULSE_DRIVE = 0x0800,        // Bit 11, Preset pulse drive 종료
        IPEND_STATUS_SENSOR_PULSE_DRIVE = 0x1000,        // Bit 12, Sensor pulse drive 종료
        IPEND_STATUS_LIMIT = 0x2000,        // Bit 13, Limit 완전정지에 의한 종료
        IPEND_STATUS_SOFTLIMIT = 0x4000,        // Bit 14, Soft limit에 의한 종료
        IPEND_STATUS_INTERPOLATION_DRIVE = 0x8000         // Bit 15, Soft limit에 의한 종료
    }

    public enum AXT_MOTION_IPDRIVE_STATUS : uint
    {
        IPDRIVE_STATUS_BUSY = 0x00001,       // Bit 0, BUSY(드라이브 구동 중)
        IPDRIVE_STATUS_DOWN = 0x00002,       // Bit 1, DOWN(감속 중)
        IPDRIVE_STATUS_CONST = 0x00004,       // Bit 2, CONST(등속 중)
        IPDRIVE_STATUS_UP = 0x00008,       // Bit 3, UP(가속 중)
        IPDRIVE_STATUS_ICL = 0x00010,       // Bit 4, ICL(내부 위치 카운터 < 내부 위치 카운터 비교값)
        IPDRIVE_STATUS_ICG = 0x00020,       // Bit 5, ICG(내부 위치 카운터 > 내부 위치 카운터 비교값)
        IPDRIVE_STATUS_ECL = 0x00040,       // Bit 6, ECL(외부 위치 카운터 < 외부 위치 카운터 비교값)
        IPDRIVE_STATUS_ECG = 0x00080,       // Bit 7, ECG(외부 위치 카운터 > 외부 위치 카운터 비교값)
        IPDRIVE_STATUS_DRIVE_DIRECTION = 0x00100,       // Bit 8, 드라이브 방향 신호(0=CW/1=CCW)
        IPDRIVE_STATUS_COMMAND_BUSY = 0x00200,       // Bit 9, 명령어 수행중
        IPDRIVE_STATUS_PRESET_DRIVING = 0x00400,       // Bit 10, Preset pulse drive 중
        IPDRIVE_STATUS_CONTINUOUS_DRIVING = 0x00800,       // Bit 11, Continuouse speed drive 중
        IPDRIVE_STATUS_SIGNAL_SEARCH_DRIVING = 0x01000,       // Bit 12, Signal search-1/2 drive 중
        IPDRIVE_STATUS_ORG_SEARCH_DRIVING = 0x02000,       // Bit 13, 원점 검출 drive 중
        IPDRIVE_STATUS_MPG_DRIVING = 0x04000,       // Bit 14, MPG drive 중
        IPDRIVE_STATUS_SENSOR_DRIVING = 0x08000,       // Bit 15, Sensor positioning drive 중
        IPDRIVE_STATUS_L_C_INTERPOLATION = 0x10000,       // Bit 16, 직선/원호 보간 중
        IPDRIVE_STATUS_PATTERN_INTERPOLATION = 0x20000,       // Bit 17, 비트 패턴 보간 중
        IPDRIVE_STATUS_INTERRUPT_BANK1 = 0x40000,       // Bit 18, 인터럽트 bank1에서 발생
        IPDRIVE_STATUS_INTERRUPT_BANK2 = 0x80000        // Bit 19, 인터럽트 bank2에서 발생
    }

    public enum AXT_MOTION_IPINTERRUPT_BANK1 : uint
    {
        IPINTBANK1_DONTUSE = 0x00000000,    // INTERRUT DISABLED.
        IPINTBANK1_DRIVE_END = 0x00000001,    // Bit 0, Drive end(default value : 1).
        IPINTBANK1_ICG = 0x00000002,    // Bit 1, INCNT is greater than INCNTCMP.
        IPINTBANK1_ICE = 0x00000004,    // Bit 2, INCNT is equal with INCNTCMP.
        IPINTBANK1_ICL = 0x00000008,    // Bit 3, INCNT is less than INCNTCMP.
        IPINTBANK1_ECG = 0x00000010,    // Bit 4, EXCNT is greater than EXCNTCMP.
        IPINTBANK1_ECE = 0x00000020,    // Bit 5, EXCNT is equal with EXCNTCMP.
        IPINTBANK1_ECL = 0x00000040,    // Bit 6, EXCNT is less than EXCNTCMP.
        IPINTBANK1_SCRQEMPTY = 0x00000080,    // Bit 7, Script control queue is empty.
        IPINTBANK1_CAPRQEMPTY = 0x00000100,    // Bit 8, Caption result data queue is empty.
        IPINTBANK1_SCRREG1EXE = 0x00000200,    // Bit 9, Script control register-1 command is executed.
        IPINTBANK1_SCRREG2EXE = 0x00000400,    // Bit 10, Script control register-2 command is executed.
        IPINTBANK1_SCRREG3EXE = 0x00000800,    // Bit 11, Script control register-3 command is executed.
        IPINTBANK1_CAPREG1EXE = 0x00001000,    // Bit 12, Caption control register-1 command is executed.
        IPINTBANK1_CAPREG2EXE = 0x00002000,    // Bit 13, Caption control register-2 command is executed.
        IPINTBANK1_CAPREG3EXE = 0x00004000,    // Bit 14, Caption control register-3 command is executed.
        IPINTBANK1_INTGGENCMD = 0x00008000,    // Bit 15, Interrupt generation command is executed(0xFF)
        IPINTBANK1_DOWN = 0x00010000,    // Bit 16, At starting point for deceleration drive.
        IPINTBANK1_CONT = 0x00020000,    // Bit 17, At starting point for constant speed drive.
        IPINTBANK1_UP = 0x00040000,    // Bit 18, At starting point for acceleration drive.
        IPINTBANK1_SIGNALDETECTED = 0x00080000,    // Bit 19, Signal assigned in MODE1 is detected.
        IPINTBANK1_SP23E = 0x00100000,    // Bit 20, Current speed is equal with rate change point RCP23.
        IPINTBANK1_SP12E = 0x00200000,    // Bit 21, Current speed is equal with rate change point RCP12.
        IPINTBANK1_SPE = 0x00400000,    // Bit 22, Current speed is equal with speed comparison data(SPDCMP).
        IPINTBANK1_INCEICM = 0x00800000,    // Bit 23, INTCNT(1'st counter) is equal with ICM(1'st count minus limit data)
        IPINTBANK1_SCRQEXE = 0x01000000,    // Bit 24, Script queue command is executed When SCRCONQ's 30 bit is '1'.
        IPINTBANK1_CAPQEXE = 0x02000000,    // Bit 25, Caption queue command is executed When CAPCONQ's 30 bit is '1'.
        IPINTBANK1_SLM = 0x04000000,    // Bit 26, NSLM/PSLM input signal is activated.
        IPINTBANK1_ELM = 0x08000000,    // Bit 27, NELM/PELM input signal is activated.
        IPINTBANK1_USERDEFINE1 = 0x10000000,    // Bit 28, Selectable interrupt source 0(refer "0xFE" command).
        IPINTBANK1_USERDEFINE2 = 0x20000000,    // Bit 29, Selectable interrupt source 1(refer "0xFE" command).
        IPINTBANK1_USERDEFINE3 = 0x40000000,    // Bit 30, Selectable interrupt source 2(refer "0xFE" command).
        IPINTBANK1_USERDEFINE4 = 0x80000000     // Bit 31, Selectable interrupt source 3(refer "0xFE" command).
    }

    public enum AXT_MOTION_IPINTERRUPT_BANK2 : uint
    {
        IPINTBANK2_DONTUSE = 0x00000000,    // INTERRUT DISABLED.
        IPINTBANK2_L_C_INP_Q_EMPTY = 0x00000001,    // Bit 0, Linear/Circular interpolation parameter queue is empty.
        IPINTBANK2_P_INP_Q_EMPTY = 0x00000002,    // Bit 1, Bit pattern interpolation queue is empty.
        IPINTBANK2_ALARM_ERROR = 0x00000004,    // Bit 2, Alarm input signal is activated.
        IPINTBANK2_INPOSITION = 0x00000008,    // Bit 3, Inposition input signal is activated.
        IPINTBANK2_MARK_SIGNAL_HIGH = 0x00000010,    // Bit 4, Mark input signal is activated.
        IPINTBANK2_SSTOP_SIGNAL = 0x00000020,    // Bit 5, SSTOP input signal is activated.
        IPINTBANK2_ESTOP_SIGNAL = 0x00000040,    // Bit 6, ESTOP input signal is activated.
        IPINTBANK2_SYNC_ACTIVATED = 0x00000080,    // Bit 7, SYNC input signal is activated.
        IPINTBANK2_TRIGGER_ENABLE = 0x00000100,    // Bit 8, Trigger output is activated.
        IPINTBANK2_EXCNTCLR = 0x00000200,    // Bit 9, External(2'nd) counter is cleard by EXCNTCLR setting.
        IPINTBANK2_FSTCOMPARE_RESULT_BIT0 = 0x00000400,    // Bit 10, ALU1's compare result bit 0 is activated.
        IPINTBANK2_FSTCOMPARE_RESULT_BIT1 = 0x00000800,    // Bit 11, ALU1's compare result bit 1 is activated.
        IPINTBANK2_FSTCOMPARE_RESULT_BIT2 = 0x00001000,    // Bit 12, ALU1's compare result bit 2 is activated.
        IPINTBANK2_FSTCOMPARE_RESULT_BIT3 = 0x00002000,    // Bit 13, ALU1's compare result bit 3 is activated.
        IPINTBANK2_FSTCOMPARE_RESULT_BIT4 = 0x00004000,    // Bit 14, ALU1's compare result bit 4 is activated.
        IPINTBANK2_SNDCOMPARE_RESULT_BIT0 = 0x00008000,    // Bit 15, ALU2's compare result bit 0 is activated.
        IPINTBANK2_SNDCOMPARE_RESULT_BIT1 = 0x00010000,    // Bit 16, ALU2's compare result bit 1 is activated.
        IPINTBANK2_SNDCOMPARE_RESULT_BIT2 = 0x00020000,    // Bit 17, ALU2's compare result bit 2 is activated.
        IPINTBANK2_SNDCOMPARE_RESULT_BIT3 = 0x00040000,    // Bit 18, ALU2's compare result bit 3 is activated.
        IPINTBANK2_SNDCOMPARE_RESULT_BIT4 = 0x00080000,    // Bit 19, ALU2's compare result bit 4 is activated.
        IPINTBANK2_L_C_INP_Q_LESS_4 = 0x00100000,    // Bit 20, Linear/Circular interpolation parameter queue is less than 4.
        IPINTBANK2_P_INP_Q_LESS_4 = 0x00200000,    // Bit 21, Pattern interpolation parameter queue is less than 4.
        IPINTBANK2_XSYNC_ACTIVATED = 0x00400000,    // Bit 22, X axis sync input signal is activated.
        IPINTBANK2_YSYNC_ACTIVATED = 0x00800000,    // Bit 23, Y axis sync input siangl is activated.
        IPINTBANK2_P_INP_END_BY_END_PATTERN = 0x01000000     // Bit 24, Bit pattern interpolation is terminated by end pattern.
                                                             //IPINTBANK2_                          = 0x02000000,    // Bit 25, Don't care.
                                                             //IPINTBANK2_                          = 0x04000000,    // Bit 26, Don't care.
                                                             //IPINTBANK2_                          = 0x08000000,    // Bit 27, Don't care.
                                                             //IPINTBANK2_                          = 0x10000000,    // Bit 28, Don't care.
                                                             //IPINTBANK2_                          = 0x20000000,    // Bit 29, Don't care.
                                                             //IPINTBANK2_                          = 0x40000000,    // Bit 30, Don't care.
                                                             //IPINTBANK2_                          = 0x80000000     // Bit 31, Don't care.
    }

    public enum AXT_MOTION_IPMECHANICAL_SIGNAL : uint
    {
        IPMECHANICAL_PELM_LEVEL = 0x0001,        // Bit 0, +Limit 급정지 신호가 액티브 됨
        IPMECHANICAL_NELM_LEVEL = 0x0002,        // Bit 1, -Limit 급정지 신호 액티브 됨
        IPMECHANICAL_PSLM_LEVEL = 0x0004,        // Bit 2, +limit 감속정지 신호 액티브 됨
        IPMECHANICAL_NSLM_LEVEL = 0x0008,        // Bit 3, -limit 감속정지 신호 액티브 됨
        IPMECHANICAL_ALARM_LEVEL = 0x0010,        // Bit 4, Alarm 신호 액티브 됨
        IPMECHANICAL_INP_LEVEL = 0x0020,        // Bit 5, Inposition 신호 액티브 됨
        IPMECHANICAL_ENC_DOWN_LEVEL = 0x0040,        // Bit 6, 엔코더 DOWN(B상) 신호 입력 Level
        IPMECHANICAL_ENC_UP_LEVEL = 0x0080,        // Bit 7, 엔코더 UP(A상) 신호 입력 Level
        IPMECHANICAL_EXMP_LEVEL = 0x0100,        // Bit 8, EXMP 신호 입력 Level
        IPMECHANICAL_EXPP_LEVEL = 0x0200,        // Bit 9, EXPP 신호 입력 Level
        IPMECHANICAL_MARK_LEVEL = 0x0400,        // Bit 10, MARK# 신호 액티브 됨
        IPMECHANICAL_SSTOP_LEVEL = 0x0800,        // Bit 11, SSTOP 신호 액티브 됨
        IPMECHANICAL_ESTOP_LEVEL = 0x1000,        // Bit 12, ESTOP 신호 액티브 됨
        IPMECHANICAL_SYNC_LEVEL = 0x2000,        // Bit 13, SYNC 신호 입력 Level
        IPMECHANICAL_MODE8_16_LEVEL = 0x4000         // Bit 14, MODE8_16 신호 입력 Level
    }


    public enum AXT_MOTION_QIDETECT_DESTINATION_SIGNAL : uint
    {
        Signal_PosEndLimit = 0x0,           // +Elm(End limit) +방향 리미트 센서 신호
        Signal_NegEndLimit = 0x1,           // -Elm(End limit) -방향 리미트 센서 신호
        Signal_PosSloLimit = 0x2,           // +Slm(Slow Down limit) 신호 - 사용하지 않음
        Signal_NegSloLimit = 0x3,           // -Slm(Slow Down limit) 신호 - 사용하지 않음
        Signal_HomeSensor = 0x4,           // IN0(ORG)  원점 센서 신호
        Signal_EncodZPhase = 0x5,           // IN1(Z상)  Encoder Z상 신호
        Signal_UniInput02 = 0x6,           // IN2(범용) 범용 입력 2번 신호
        Signal_UniInput03 = 0x7            // IN3(범용) 범용 입력 3번 신호
    }

    public enum AXT_MOTION_QIMECHANICAL_SIGNAL : uint
    {
        QIMECHANICAL_PELM_LEVEL = 0x00001,       // Bit 0, +Limit 급정지 신호 현재 상태
        QIMECHANICAL_NELM_LEVEL = 0x00002,       // Bit 1, -Limit 급정지 신호 현재 상태
        QIMECHANICAL_PSLM_LEVEL = 0x00004,       // Bit 2, +limit 감속정지 현재 상태.
        QIMECHANICAL_NSLM_LEVEL = 0x00008,       // Bit 3, -limit 감속정지 현재 상태
        QIMECHANICAL_ALARM_LEVEL = 0x00010,       // Bit 4, Alarm 신호 신호 현재 상태
        QIMECHANICAL_INP_LEVEL = 0x00020,       // Bit 5, Inposition 신호 현재 상태
        QIMECHANICAL_ESTOP_LEVEL = 0x00040,       // Bit 6, 비상 정지 신호(ESTOP) 현재 상태.
        QIMECHANICAL_ORG_LEVEL = 0x00080,       // Bit 7, 원점 신호 헌재 상태
        QIMECHANICAL_ZPHASE_LEVEL = 0x00100,       // Bit 8, Z 상 입력 신호 현재 상태
        QIMECHANICAL_ECUP_LEVEL = 0x00200,       // Bit 9, ECUP 터미널 신호 상태.
        QIMECHANICAL_ECDN_LEVEL = 0x00400,       // Bit 10, ECDN 터미널 신호 상태.
        QIMECHANICAL_EXPP_LEVEL = 0x00800,       // Bit 11, EXPP 터미널 신호 상태
        QIMECHANICAL_EXMP_LEVEL = 0x01000,       // Bit 12, EXMP 터미널 신호 상태
        QIMECHANICAL_SQSTR1_LEVEL = 0x02000,       // Bit 13, SQSTR1 터미널 신호 상태
        QIMECHANICAL_SQSTR2_LEVEL = 0x04000,       // Bit 14, SQSTR2 터미널 신호 상태
        QIMECHANICAL_SQSTP1_LEVEL = 0x08000,       // Bit 15, SQSTP1 터미널 신호 상태
        QIMECHANICAL_SQSTP2_LEVEL = 0x10000,       // Bit 16, SQSTP2 터미널 신호 상태
        QIMECHANICAL_MODE_LEVEL = 0x20000        // Bit 17, MODE 터미널 신호 상태.
    }

    public enum AXT_MOTION_QIEND_STATUS : uint
    {
        QIEND_STATUS_0 = 0x00000001,    // Bit 0, 정방향 리미트 신호(PELM)에 의한 종료
        QIEND_STATUS_1 = 0x00000002,    // Bit 1, 역방향 리미트 신호(NELM)에 의한 종료
        QIEND_STATUS_2 = 0x00000004,    // Bit 2, 정방향 부가 리미트 신호(PSLM)에 의한 구동 종료
        QIEND_STATUS_3 = 0x00000008,    // Bit 3, 역방향 부가 리미트 신호(NSLM)에 의한 구동 종료
        QIEND_STATUS_4 = 0x00000010,    // Bit 4, 정방향 소프트 리미트 급정지 기능에 의한 구동 종료
        QIEND_STATUS_5 = 0x00000020,    // Bit 5, 역방향 소프트 리미트 급정지 기능에 의한 구동 종료
        QIEND_STATUS_6 = 0x00000040,    // Bit 6, 정방향 소프트 리미트 감속정지 기능에 의한 구동 종료
        QIEND_STATUS_7 = 0x00000080,    // Bit 7, 역방향 소프트 리미트 감속정지 기능에 의한 구동 종료
        QIEND_STATUS_8 = 0x00000100,    // Bit 8, 서보 알람 기능에 의한 구동 종료.
        QIEND_STATUS_9 = 0x00000200,    // Bit 9, 비상 정지 신호 입력에 의한 구동 종료.
        QIEND_STATUS_10 = 0x00000400,    // Bit 10, 급 정지 명령에 의한 구동 종료.
        QIEND_STATUS_11 = 0x00000800,    // Bit 11, 감속 정지 명령에 의한 구동 종료.
        QIEND_STATUS_12 = 0x00001000,    // Bit 12, 전축 급정지 명령에 의한 구동 종료
        QIEND_STATUS_13 = 0x00002000,    // Bit 13, 동기 정지 기능 #1(SQSTP1)에 의한 구동 종료.
        QIEND_STATUS_14 = 0x00004000,    // Bit 14, 동기 정지 기능 #2(SQSTP2)에 의한 구동 종료.
        QIEND_STATUS_15 = 0x00008000,    // Bit 15, 인코더 입력(ECUP,ECDN) 오류 발생
        QIEND_STATUS_16 = 0x00010000,    // Bit 16, MPG 입력(EXPP,EXMP) 오류 발생
        QIEND_STATUS_17 = 0x00020000,    // Bit 17, 원점 검색 성공 종료.
        QIEND_STATUS_18 = 0x00040000,    // Bit 18, 신호 검색 성공 종료.
        QIEND_STATUS_19 = 0x00080000,    // Bit 19, 보간 데이터 이상으로 구동 종료.
        QIEND_STATUS_20 = 0x00100000,    // Bit 20, 비정상 구동 정지발생.
        QIEND_STATUS_21 = 0x00200000,    // Bit 21, MPG 기능 블록 펄스 버퍼 오버플로우 발생
        QIEND_STATUS_22 = 0x00400000,    // Bit 22, DON'CARE
        QIEND_STATUS_23 = 0x00800000,    // Bit 23, DON'CARE
        QIEND_STATUS_24 = 0x01000000,    // Bit 24, DON'CARE
        QIEND_STATUS_25 = 0x02000000,    // Bit 25, DON'CARE
        QIEND_STATUS_26 = 0x04000000,    // Bit 26, DON'CARE
        QIEND_STATUS_27 = 0x08000000,    // Bit 27, DON'CARE
        QIEND_STATUS_28 = 0x10000000,    // Bit 28, 현재/마지막 구동 드라이브 방향
        QIEND_STATUS_29 = 0x20000000,    // Bit 29, 잔여 펄스 제거 신호 출력 중.
        QIEND_STATUS_30 = 0x40000000,    // Bit 30, 비정상 구동 정지 원인 상태
        QIEND_STATUS_31 = 0x80000000     // Bit 31, 보간 드라이브 데이타 오류 상태.
    }

    public enum AXT_MOTION_QIDRIVE_STATUS : uint
    {
        QIDRIVE_STATUS_0 = 0x0000001,     // Bit 0, BUSY(드라이브 구동 중)
        QIDRIVE_STATUS_1 = 0x0000002,     // Bit 1, DOWN(감속 중)
        QIDRIVE_STATUS_2 = 0x0000004,     // Bit 2, CONST(등속 중)
        QIDRIVE_STATUS_3 = 0x0000008,     // Bit 3, UP(가속 중)
        QIDRIVE_STATUS_4 = 0x0000010,     // Bit 4, 연속 드라이브 구동 중
        QIDRIVE_STATUS_5 = 0x0000020,     // Bit 5, 지정 거리 드라이브 구동 중
        QIDRIVE_STATUS_6 = 0x0000040,     // Bit 6, MPG 드라이브 구동 중
        QIDRIVE_STATUS_7 = 0x0000080,     // Bit 7, 원점검색 드라이브 구동중
        QIDRIVE_STATUS_8 = 0x0000100,     // Bit 8, 신호 검색 드라이브 구동 중
        QIDRIVE_STATUS_9 = 0x0000200,     // Bit 9, 보간 드라이브 구동 중
        QIDRIVE_STATUS_10 = 0x0000400,     // Bit 10, Slave 드라이브 구동중
        QIDRIVE_STATUS_11 = 0x0000800,     // Bit 11, 현재 구동 드라이브 방향(보간 드라이브에서는 표시 정보 다름)
        QIDRIVE_STATUS_12 = 0x0001000,     // Bit 12, 펄스 출력후 서보위치 완료 신호 대기중.
        QIDRIVE_STATUS_13 = 0x0002000,     // Bit 13, 직선 보간 드라이브 구동중.
        QIDRIVE_STATUS_14 = 0x0004000,     // Bit 14, 원호 보간 드라이브 구동중.
        QIDRIVE_STATUS_15 = 0x0008000,     // Bit 15, 펄스 출력 중.
        QIDRIVE_STATUS_16 = 0x0010000,     // Bit 16, 구동 예약 데이터 개수(처음)(0-7)
        QIDRIVE_STATUS_17 = 0x0020000,     // Bit 17, 구동 예약 데이터 개수(중간)(0-7)
        QIDRIVE_STATUS_18 = 0x0040000,     // Bit 18, 구동 예약 데이터 갯수(끝)(0-7)
        QIDRIVE_STATUS_19 = 0x0080000,     // Bit 19, 구동 예약 Queue 비어 있음.
        QIDRIVE_STATUS_20 = 0x0100000,     // Bit 20, 구동 예약 Queue 가득 ?
        QIDRIVE_STATUS_21 = 0x0200000,     // Bit 21, 현재 구동 드라이브의 속도 모드(처음)
        QIDRIVE_STATUS_22 = 0x0400000,     // Bit 22, 현재 구동 드라이브의 속도 모드(끝)
        QIDRIVE_STATUS_23 = 0x0800000,     // Bit 23, MPG 버퍼 #1 Full
        QIDRIVE_STATUS_24 = 0x1000000,     // Bit 24, MPG 버퍼 #2 Full
        QIDRIVE_STATUS_25 = 0x2000000,     // Bit 25, MPG 버퍼 #3 Full
        QIDRIVE_STATUS_26 = 0x4000000      // Bit 26, MPG 버퍼 데이터 OverFlow
    }

    public enum AXT_MOTION_QIINTERRUPT_BANK1 : uint
    {
        QIINTBANK1_DISABLE = 0x00000000,    // INTERRUT DISABLED.
        QIINTBANK1_0 = 0x00000001,    // Bit 0,  인터럽트 발생 사용 설정된 구동 종료시.
        QIINTBANK1_1 = 0x00000002,    // Bit 1,  구동 종료시
        QIINTBANK1_2 = 0x00000004,    // Bit 2,  구동 시작시.
        QIINTBANK1_3 = 0x00000008,    // Bit 3,  카운터 #1 < 비교기 #1 이벤트 발생
        QIINTBANK1_4 = 0x00000010,    // Bit 4,  카운터 #1 = 비교기 #1 이벤트 발생
        QIINTBANK1_5 = 0x00000020,    // Bit 5,  카운터 #1 > 비교기 #1 이벤트 발생
        QIINTBANK1_6 = 0x00000040,    // Bit 6,  카운터 #2 < 비교기 #2 이벤트 발생
        QIINTBANK1_7 = 0x00000080,    // Bit 7,  카운터 #2 = 비교기 #2 이벤트 발생
        QIINTBANK1_8 = 0x00000100,    // Bit 8,  카운터 #2 > 비교기 #2 이벤트 발생
        QIINTBANK1_9 = 0x00000200,    // Bit 9,  카운터 #3 < 비교기 #3 이벤트 발생
        QIINTBANK1_10 = 0x00000400,    // Bit 10, 카운터 #3 = 비교기 #3 이벤트 발생
        QIINTBANK1_11 = 0x00000800,    // Bit 11, 카운터 #3 > 비교기 #3 이벤트 발생
        QIINTBANK1_12 = 0x00001000,    // Bit 12, 카운터 #4 < 비교기 #4 이벤트 발생
        QIINTBANK1_13 = 0x00002000,    // Bit 13, 카운터 #4 = 비교기 #4 이벤트 발생
        QIINTBANK1_14 = 0x00004000,    // Bit 14, 카운터 #4 < 비교기 #4 이벤트 발생
        QIINTBANK1_15 = 0x00008000,    // Bit 15, 카운터 #5 < 비교기 #5 이벤트 발생
        QIINTBANK1_16 = 0x00010000,    // Bit 16, 카운터 #5 = 비교기 #5 이벤트 발생
        QIINTBANK1_17 = 0x00020000,    // Bit 17, 카운터 #5 > 비교기 #5 이벤트 발생
        QIINTBANK1_18 = 0x00040000,    // Bit 18, 타이머 #1 이벤트 발생.
        QIINTBANK1_19 = 0x00080000,    // Bit 19, 타이머 #2 이벤트 발생.
        QIINTBANK1_20 = 0x00100000,    // Bit 20, 구동 예약 설정 Queue 비워짐.
        QIINTBANK1_21 = 0x00200000,    // Bit 21, 구동 예약 설정 Queue 가득?
        QIINTBANK1_22 = 0x00400000,    // Bit 22, 트리거 발생거리 주기/절대위치 Queue 비워짐.
        QIINTBANK1_23 = 0x00800000,    // Bit 23, 트리거 발생거리 주기/절대위치 Queue 가득?
        QIINTBANK1_24 = 0x01000000,    // Bit 24, 트리거 신호 발생 이벤트
        QIINTBANK1_25 = 0x02000000,    // Bit 25, 스크립트 #1 명령어 예약 설정 Queue 비워짐.
        QIINTBANK1_26 = 0x04000000,    // Bit 26, 스크립트 #2 명령어 예약 설정 Queue 비워짐.
        QIINTBANK1_27 = 0x08000000,    // Bit 27, 스크립트 #3 명령어 예약 설정 레지스터 실행되어 초기화 됨.
        QIINTBANK1_28 = 0x10000000,    // Bit 28, 스크립트 #4 명령어 예약 설정 레지스터 실행되어 초기화 됨.
        QIINTBANK1_29 = 0x20000000,    // Bit 29, 서보 알람신호 인가됨.
        QIINTBANK1_30 = 0x40000000,    // Bit 30, |CNT1| - |CNT2| >= |CNT4| 이벤트 발생.
        QIINTBANK1_31 = 0x80000000     // Bit 31, 인터럽트 발생 명령어|INTGEN| 실행.
    }

    public enum AXT_MOTION_QIINTERRUPT_BANK2 : uint
    {
        QIINTBANK2_DISABLE = 0x00000000,    // INTERRUT DISABLED.
        QIINTBANK2_0 = 0x00000001,    // Bit 0,  스크립트 #1 읽기 명령 결과 Queue 가 가득?.
        QIINTBANK2_1 = 0x00000002,    // Bit 1,  스크립트 #2 읽기 명령 결과 Queue 가 가득?.
        QIINTBANK2_2 = 0x00000004,    // Bit 2,  스크립트 #3 읽기 명령 결과 레지스터가 새로운 데이터로 갱신됨.
        QIINTBANK2_3 = 0x00000008,    // Bit 3,  스크립트 #4 읽기 명령 결과 레지스터가 새로운 데이터로 갱신됨.
        QIINTBANK2_4 = 0x00000010,    // Bit 4,  스크립트 #1 의 예약 명령어 중 실행 시 인터럽트 발생으로 설정된 명령어 실행됨.
        QIINTBANK2_5 = 0x00000020,    // Bit 5,  스크립트 #2 의 예약 명령어 중 실행 시 인터럽트 발생으로 설정된 명령어 실행됨.
        QIINTBANK2_6 = 0x00000040,    // Bit 6,  스크립트 #3 의 예약 명령어 실행 시 인터럽트 발생으로 설정된 명령어 실행됨.
        QIINTBANK2_7 = 0x00000080,    // Bit 7,  스크립트 #4 의 예약 명령어 실행 시 인터럽트 발생으로 설정된 명령어 실행됨.
        QIINTBANK2_8 = 0x00000100,    // Bit 8,  구동 시작
        QIINTBANK2_9 = 0x00000200,    // Bit 9,  서보 위치 결정 완료(Inposition)기능을 사용한 구동,종료 조건 발생.
        QIINTBANK2_10 = 0x00000400,    // Bit 10, 이벤트 카운터로 동작 시 사용할 이벤트 선택 #1 조건 발생.
        QIINTBANK2_11 = 0x00000800,    // Bit 11, 이벤트 카운터로 동작 시 사용할 이벤트 선택 #2 조건 발생.
        QIINTBANK2_12 = 0x00001000,    // Bit 12, SQSTR1 신호 인가 됨.
        QIINTBANK2_13 = 0x00002000,    // Bit 13, SQSTR2 신호 인가 됨.
        QIINTBANK2_14 = 0x00004000,    // Bit 14, UIO0 터미널 신호가 '1'로 변함.
        QIINTBANK2_15 = 0x00008000,    // Bit 15, UIO1 터미널 신호가 '1'로 변함.
        QIINTBANK2_16 = 0x00010000,    // Bit 16, UIO2 터미널 신호가 '1'로 변함.
        QIINTBANK2_17 = 0x00020000,    // Bit 17, UIO3 터미널 신호가 '1'로 변함.
        QIINTBANK2_18 = 0x00040000,    // Bit 18, UIO4 터미널 신호가 '1'로 변함.
        QIINTBANK2_19 = 0x00080000,    // Bit 19, UIO5 터미널 신호가 '1'로 변함.
        QIINTBANK2_20 = 0x00100000,    // Bit 20, UIO6 터미널 신호가 '1'로 변함.
        QIINTBANK2_21 = 0x00200000,    // Bit 21, UIO7 터미널 신호가 '1'로 변함.
        QIINTBANK2_22 = 0x00400000,    // Bit 22, UIO8 터미널 신호가 '1'로 변함.
        QIINTBANK2_23 = 0x00800000,    // Bit 23, UIO9 터미널 신호가 '1'로 변함.
        QIINTBANK2_24 = 0x01000000,    // Bit 24, UIO10 터미널 신호가 '1'로 변함.
        QIINTBANK2_25 = 0x02000000,    // Bit 25, UIO11 터미널 신호가 '1'로 변함.
        QIINTBANK2_26 = 0x04000000,    // Bit 26, 오류 정지 조건(LMT, ESTOP, STOP, ESTOP, CMD, ALARM) 발생.
        QIINTBANK2_27 = 0x08000000,    // Bit 27, 보간 중 데이터 설정 오류 발생.
        QIINTBANK2_28 = 0x10000000,    // Bit 28, Don't Care
        QIINTBANK2_29 = 0x20000000,    // Bit 29, 리미트 신호(PELM, NELM)신호가 입력 됨.
        QIINTBANK2_30 = 0x40000000,    // Bit 30, 부가 리미트 신호(PSLM, NSLM)신호가 입력 됨.
        QIINTBANK2_31 = 0x80000000     // Bit 31, 비상 정지 신호(ESTOP)신호가 입력됨.
    }
    public enum AXT_EVENT : uint
    {
        WM_USER = 0x0400,
        WM_AXL_INTERRUPT = (WM_USER + 1001)
    }

    public enum AXT_NETWORK_STATUS : uint
    {
        NET_STATUS_DISCONNECTED = 1,
        NET_STATUS_LOCK_MISMATCH = 5,
        NET_STATUS_CONNECTED = 6
    }

    public enum AXT_MOTION_OVERRIDE_MODE : uint
    {
        OVERRIDE_POS_START = 0,
        OVERRIDE_POS_END = 1
    }

    public enum AXT_MOTION_PROFILE_PRIORITY : uint
    {
        PRIORITY_VELOCITY = 0,
        PRIORITY_ACCELTIME = 1
    }

    public enum AXT_MOTION_FUNC_RETURN_MODE_DEF : uint
    {
        FUNC_RETURN_IMMEDIATE = 0,
        FUNC_RETURN_BLOCKING = 1,
        FUNC_RETURN_NON_BLOCKING = 2
    }

    public struct MOTION_INFO
    {
        public double dCmdPos;      // Command 위치[0x01]
        public double dActPos;      // Encoder 위치[0x02]
        public uint uMechSig;       // Mechanical Signal[0x04]
        public uint uDrvStat;       // Driver Status[0x08]
        public uint uInput;         // Universal Signal Input[0x10]
        public uint uOutput;        // Universal Signal Output[0x10]
        public uint uMask;          // 읽기 설정 Mask Ex) 0x1F, 모든정보 읽기
    }

    public struct _SIIIHBoardInfo
    {
        public int bIsSIIIHBoard;// SIIIH 보드인 경우 TRUE, 아니면 FALSE
        public int lNodeCount; // 해당 보드에 연결된 Node 개수
    }

    public struct _SCAN_RESULT
    {
        public int lTotalBoardCount; //SIIIH 포함 장착된 전체 보드 개수
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public _SIIIHBoardInfo[] BoardInfo;
    }

    public class CAXHS
    {
        public delegate void AXT_INTERRUPT_PROC(int nActiveNo, uint uFlag);

        public readonly static uint WM_USER = 0x0400;
        public readonly static uint WM_AXL_INTERRUPT = (WM_USER + 1001);
        public readonly static uint MAX_SERVO_ALARM_HISTORY = 15;

        public static int AXIS_EVN(int nAxisNo)
        {
            nAxisNo = (nAxisNo - (nAxisNo % 2));                // 쌍을 이루는 축의 짝수축을 찾음
            return nAxisNo;
        }

        public static int AXIS_ODD(int nAxisNo)
        {
            nAxisNo = (nAxisNo + ((nAxisNo + 1) % 2));          // 쌍을 이루는 축의 홀수축을 찾음
            return nAxisNo;
        }

        public static int AXIS_QUR(int nAxisNo)
        {
            nAxisNo = (nAxisNo % 4);                            // 쌍을 이루는 축의 홀수축을 찾음
            return nAxisNo;
        }

        public static int AXIS_N04(int nAxisNo, int nPos)
        {
            nAxisNo = (((nAxisNo / 4) * 4) + nPos);             // 한 칩의 축 위치로 변경(0~3)
            return nAxisNo;
        }

        public static int AXIS_N01(int nAxisNo)
        {
            nAxisNo = ((nAxisNo % 4) >> 2);                     // 0, 1축을 0으로 2, 3축을 1로 변경
            return nAxisNo;
        }

        public static int AXIS_N02(int nAxisNo)
        {
            nAxisNo = ((nAxisNo % 4) % 2);                       // 0, 2축을 0으로 1, 3축을 1로 변경
            return nAxisNo;
        }

        public static int m_SendAxis = 0;           // 현재 축번호

        public const int F_50M_CLK = 50000000;    /* 50.000 MHz */
    }

    public enum CNTPORT_DATA_WRITE : uint
    {
        CnCommand = 0x10,
        CnData1 = 0x12,
        CnData2 = 0x14,
        CnData3 = 0x16,
        CnData4 = 0x18,
        CnData12 = 0x44,
        CnData34 = 0x46
    }

    public enum _CNTRAM_DATA : uint
    {
        CnRamAddr1 = 0x28,
        CnRamAddr2 = 0x2A,
        CnRamAddr3 = 0x2C,
        CnRamAddrx1 = 0x48,
        CnRamAddr23 = 0x4A
    }

    public enum _PHASE_SEL
    {
        CnAbPhase = 0x0,
        CnZPhase = 0x1
    }

    public enum _COUNTER_INPUT
    {
        CnUpDownMode = 0x0,                                 // Up/Down
        CnSqr1Mode = 0x1,                                 // 1체배
        CnSqr2Mode = 0x2,                                 // 2체배
        CnSqr4Mode = 0x3                                  // 4체배
    }

    public enum _CNTCOMMAND
    {
        // CH-1 Group Register
        CnCh1CounterRead = 0x10,                         // CH1 COUNTER READ, 24BIT
        CnCh1CounterWrite = 0x90,                         // CH1 COUNTER WRITE
        CnCh1CounterModeRead = 0x11,                         // CH1 COUNTER MODE READ, 8BIT
        CnCh1CounterModeWrite = 0x91,                         // CH1 COUNTER MODE WRITE
        CnCh1TriggerRegionLowerDataRead = 0x12,                         // CH1 TRIGGER REGION LOWER DATA READ, 24BIT
        CnCh1TriggerRegionLowerDataWrite = 0x92,                         // CH1 TRIGGER REGION LOWER DATA WRITE
        CnCh1TriggerRegionUpperDataRead = 0x13,                         // CH1 TRIGGER REGION UPPER DATA READ, 24BIT
        CnCh1TriggerRegionUpperDataWrite = 0x93,                         // CH1 TRIGGER REGION UPPER DATA WRITE
        CnCh1TriggerPeriodRead = 0x14,                         // CH1 TRIGGER PERIOD READ, 24BIT, RESERVED
        CnCh1TriggerPeriodWrite = 0x94,                         // CH1 TRIGGER PERIOD WRITE
        CnCh1TriggerPulseWidthRead = 0x15,                         // CH1 TRIGGER PULSE WIDTH READ
        CnCh1TriggerPulseWidthWrite = 0x95,                         // CH1 RIGGER PULSE WIDTH WRITE
        CnCh1TriggerModeRead = 0x16,                         // CH1 TRIGGER MODE READ
        CnCh1TriggerModeWrite = 0x96,                         // CH1 RIGGER MODE WRITE
        CnCh1TriggerStatusRead = 0x17,                         // CH1 TRIGGER STATUS READ
        CnCh1NoOperation_97 = 0x97,
        CnCh1TriggerEnable = 0x98,
        CnCh1TriggerDisable = 0x99,
        CnCh1TimeTriggerFrequencyRead = 0x1A,
        CnCh1TimeTriggerFrequencyWrite = 0x9A,
        CnCh1ComparatorValueRead = 0x1B,
        CnCh1ComparatorValueWrite = 0x9B,
        CnCh1CompareatorConditionRead = 0x1D,
        CnCh1CompareatorConditionWrite = 0x9D,

        // CH-2 Group Register
        CnCh2CounterRead = 0x20,                         // CH2 COUNTER READ, 24BIT
        CnCh2CounterWrite = 0xA1,                         // CH2 COUNTER WRITE
        CnCh2CounterModeRead = 0x21,                         // CH2 COUNTER MODE READ, 8BIT
        CnCh2CounterModeWrite = 0xA1,                         // CH2 COUNTER MODE WRITE
        CnCh2TriggerRegionLowerDataRead = 0x22,                         // CH2 TRIGGER REGION LOWER DATA READ, 24BIT
        CnCh2TriggerRegionLowerDataWrite = 0xA2,                         // CH2 TRIGGER REGION LOWER DATA WRITE
        CnCh2TriggerRegionUpperDataRead = 0x23,                         // CH2 TRIGGER REGION UPPER DATA READ, 24BIT
        CnCh2TriggerRegionUpperDataWrite = 0xA3,                         // CH2 TRIGGER REGION UPPER DATA WRITE
        CnCh2TriggerPeriodRead = 0x24,                         // CH2 TRIGGER PERIOD READ, 24BIT, RESERVED
        CnCh2TriggerPeriodWrite = 0xA4,                         // CH2 TRIGGER PERIOD WRITE
        CnCh2TriggerPulseWidthRead = 0x25,                         // CH2 TRIGGER PULSE WIDTH READ, 24BIT
        CnCh2TriggerPulseWidthWrite = 0xA5,                         // CH2 RIGGER PULSE WIDTH WRITE
        CnCh2TriggerModeRead = 0x26,                         // CH2 TRIGGER MODE READ
        CnCh2TriggerModeWrite = 0xA6,                         // CH2 RIGGER MODE WRITE
        CnCh2TriggerStatusRead = 0x27,                         // CH2 TRIGGER STATUS READ
        CnCh2NoOperation_A7 = 0xA7,
        CnCh2TriggerEnable = 0xA8,
        CnCh2TriggerDisable = 0xA9,
        CnCh2TimeTriggerFrequencyRead = 0x2A,
        CnCh2TimeTriggerFrequencyWrite = 0xAA,
        CnCh2ComparatorValueRead = 0x2B,
        CnCh2ComparatorValueWrite = 0xAB,
        CnCh2CompareatorConditionRead = 0x2D,
        CnCh2CompareatorConditionWrite = 0xAD,

        // Ram Access Group Register
        CnRamDataWithRamAddress = 0x30,                         // READ RAM DATA WITH RAM ADDR PORT ADDRESS
        CnRamDataWrite = 0xB0,                         // RAM DATA WRITE
        CnRamDataRead = 0x31                          // RAM DATA READ, 32BIT
    }
    #endregion
}
