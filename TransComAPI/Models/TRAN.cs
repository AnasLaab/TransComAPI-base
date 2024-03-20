using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Globalization;
using System.IO.Ports;
using System.Runtime.InteropServices;

public class TRAN
{
    #region Constant
    public const int RECORD_MAX_COUNT = 80;
    public const int TEMPLATE_MAX_COUNT = 24;
    public const int SIDES_COUNT_MAX = 2;
    public const int PHASE_COUNT_MAX = 3;
    public const int TAPS_COUNT_MAX = 35;
    public const int TEST_BALANCE_COUNT_MAX = 3;
    public const int TEST_PLOT_POINT_MAX_COUNT = 100;
    public const int TEST_HEAT_RUN_COUNT_MAX = 200;
    public const int TEST_DEMAGNETIZATION_COUNT_MAX = 20;
    public const int TEST_TAP_CHANGER_COUNT_MAX = 1;
    public const int TEST_STRING_BUFFER_LENGTH = 32;
    public const int HEADER_FIELD_LEN = 32;
    #endregion

    #region public enumerators
    public enum MODULE_SELECT
    {
        TTR = 1,
        WR,
    }
    public enum MODULE_IO_SELECT
    {
        INPUT = 1,
        OUTPUT,
        OUTPUT_SENSOR,

        PB_INPUT = 0x11,
        PB_OUTPUT,
        PB_OUTPUT_SENSOR,
    }
    public enum DISPLAY_AUTO_OFF
    {
        DISABLE = 0,
        IN_30_SEC,
        IN_1_MIN,
        IN_5_MIN,
        IN_10_MIN,
    }
    public enum TEST_FREQUENCY
    {
        Hz_50 = 0,
        Hz_60,
    }
    public enum TEST_VOLTAGE
    {
        V_1 = 0,
        V_4,
        V_10,
        V_40,
        V_100,
        V_250,
        V_200,
        V_04,
    }
    public enum TEST_TEMPERATURE_UNIT
    {
        C = 0,
        F,
        K,
    }
    public enum TEST_MATERIAL
    {
        COPPER = 0,
        ALUMINUM,
        CUSTOM,
    }
    public enum TEST_TYPE
    {
        NONE = 0,
        SINGLE_PHASE,
        TRUE_THREE_PHASE,
        THREE_PHASE,
        MAGNETIC_BALANCE,
        WINDING,
        RESISTANCE,
        HEAT_RUN,
        TAP_CHANGER,
        DEMAGNETIZATION,
    }
    public enum TEST_VECTOR
    {
        D = 0,
        Y,
        Z,
        T,
        YN,
        ZN,
        UNKNOWN,
        P1,
    }
    public enum TEST_VECTOR_NUMBERS
    {
        V_0 = 0,
        V_1 = 1,
        V_2 = 2,
        V_3 = 3,
        V_4 = 4,
        V_5 = 5,
        V_6 = 6,
        V_7 = 7,
        V_8 = 8,
        V_9 = 9,
        V_10 = 10,
        V_11 = 11,
        V_30LAG = 12,
        V_30LEAD = 13,
        V_UNKNOWN = 14,
    }
    public enum TEST_CONNECTION
    {
        PHASE_TO_NULL = 0,
        PHASE_TO_PHASE,
    }
    public enum TEST_TAP_WAIT_MODE
    {
        STOP = 0,
        KEEP,
    }
    public enum TEST_SIDE
    {
        H = 0,
        X = 1,
        ALL = 2,
        NONE = 3,
    }
    public enum TEST_PHASE
    {
        L1 = 0,
        L2 = 1,
        L3 = 2,
        N = 3,
    }
    public enum PHASE_TYPE
    {
        SINGLE = 0,
        TWO,
        THREE,
    }
    public enum TEST_STOP_MODE
    {
        CONTINUOUS = 0,
        AUTO,
        MANUAL,
        INTERVAL,
    }
    public enum INPUT_CHANNEL_TTR
    {
        X = 0,
        C,
        I,
        H,
        COUNT,
    }
    public enum INPUT_CHANNEL_WR
    {
        U1 = 0,
        U2,
        I,
        I_FAST,
        VMAIN,
        CLAMP_V,
        TEMP_HS,
        IGEN_IIN,
        IGEN_VOUT,
        COUNT,
    }
    public enum INPUT_GAIN
    {
        X1 = 0,
        X2,
        X4,
        X8,
        X16,
        X32,
        COUNT,
    }
    public enum INPUT_RANGE
    {
        LOW = 0,
        MED,
        HIGH,
        AUTO,
        KEEP,
    }


    public enum TEST_CONFIG
    {
        NONE = 0,

        AVAILABLE = 0x01,
        TYPE,
        SIDE,
        CONNECTION,

        SETTING_PRIMARY = 0x11,
        SETTING_SECONDARY,
        SETTING_VOLTAGE,
        SETTING_FREQUENCY,
        SETTING_PASS_FAIL,

        PHASE_COUNT = 0x21,
        PHASE_SELECT,
        PHASE_PRIMARY,
        PHASE_SECONDARY,

        TAP_AUTO = 0x31,
        TAP_DIRECTION,
        TAP_WAIT_MODE,
        TAP_SIDE,
        TAP_UP_DOWN,
        TAP_COUNT,
        TAP_SELECT,
        TAP_CONTACT_TIME,
        TAP_WAIT_TIME,
        TAP_X_VALUE,
        TAP_PRIMARY,
        TAP_SECONDARY,

        VECTOR_PRIMARY = 0x41,
        VECTOR_SECONDARY,
        VECTOR_NUMBER,

        TEMPERATURE_ENABLE = 0x51,
        TEMPERATURE_SENSOR,
        TEMPERATURE_VALUE,
        TEMPERATURE_REFERENCE,
        TEMPERATURE_COEFFICIENT,
        TEMPERATURE_UNIT,
        TEMPERATURE_MATERIAL,

        CURRENT_PRIMARY = 0x61,
        CURRENT_SECONDARY,

        STOP_MODE = 0x71,
        STOP_CONDITION,
        STOP_TIME,
        STOP_TIME_OUT,
        STOP_PASS_FAIL,
        STOP_COUNT,
        STOP_INTERVAL,

        INTERVAL_ENABLE = 0x81,
        INTERVAL_COUNT,
        INTERVAL_VALUE,

        END,
    }
    ;
    public enum TEST_INFO
    {
        NONE = 0,

        USER_COMPANY = 0x01,
        USER_STATION,
        USER_OPERATOR,
        TRANSFORMER_MANUFACTURE,
        TRANSFORMER_MODEL,
        TRANSFORMER_SERIAL,
        TRANSFORMER_POWER,

        END,
    }
    public enum TEST_MODE
    {
        IDLE = 0,
        START,
        WAIT_START,
        POWER_ON,
        AMP_ENABLE,
        PRE_TEST,
        CALIBRATION,
        WAIT_CALIBRATION,
        CHECK,
        SET_VOLTAGE,
        TAKE_OFFSET,
        TEST_WAIT,
        TAP_SET,
        TAP_WAIT_CONTACT,
        TAP_WAIT_WAIT,
        WAIT_TAP,
        RANGE_FIND,
        FINE_TUNE,
        MEASUREMENT,
        WINDING_SET,
        WINDING_MEASUREMENT,
        WINDING_CHECK,
        RESISTANCE_SET,
        RESISTANCE_MEASUREMENT,
        DEMAGNETIZATION_SET,
        DEMAGNETIZATION_MEASUREMENT,
        DEMAGNETIZATION_DISCHARGE,
        DEMAGNETIZATION_DISCHARGE_WAIT,
        DEMAGNETIZATION_DONE,
        TAP_CHANGER_SET,
        TAP_CHANGER_MEASUREMENT,
        TAP_CHANGE_DONE,
        HEAT_RUN_SET,
        HEAT_RUN_WAIT_START,
        HEAT_RUN_MEASUREMENT,
        DISCHARGE,
        FAULT,
        POWER_OFF,
        SHUTDOWN,
        STOP,
        DONE,
    }
    public enum TEST_STATUS
    {
        IDLE = 0,
        READY,
        ERROR,
        OK,
        CANCEL,
        START,
        WAIT,
        AMP_ENABLE,
        PRE_TEST,
        CALIBRATION,
        TAKE_OFFSET,
        OFFSET_DONE,
        WINDING,
        RESISTANCE,
        HEAT_RUN,
        TAP_CHANGE,
        DEMAGNETIZATION,
        CHECKING,
        TIMEOUT,
        SET_VOLTAGE,
        RANGE_FIND,
        FINE_TUNE,
        MEASUREMENT,
        POWER_ON,
        RUN,
        POWER_OFF,
        STOP,
        DONE,
    }
    public enum TEST_FAULT
    {
        OK = 0,
        OUTPUT,
        CONTROL,
        CALIBRATION,
        EMERGENCY_STOP,
        TIMEOUT,
        CABLE,
        HIGH_TEMPERATURE,
        FAN,
        H_FLOAT,
        X_FLOAT,
        H_SHORT,
        X_SHORT,
        OVER_RATIO,
        REVERSE_CONN,
        OVER_CURRENT,
        EXTERNAL_VOLTAGE,
        TPA,
        TPC,
        DONE,
    }
    public enum TEST_MEASUREMENT
    {
        NONE = 0,

        TURN_RATIO_DONE = 0x01,
        TURN_RATIO_CURRENT,
        TURN_RATIO_COEFFICIENT,
        TURN_RATIO_VALUE,
        TURN_RATIO_PHASE_DIFFERENCE,
        TURN_RATIO_PHASE_ERROR,

        MAGNETIC_BALANCE_DONE = 0x11,
        MAGNETIC_BALANCE_CURRENT,
        MAGNETIC_BALANCE_VAULE,
        MAGNETIC_BALANCE_COUNTER,

        WINDING_DONE = 0x21,
        WINDING_CURRENT,
        WINDING_RESISTANCE_MEASURED,
        WINDING_RESISTANCE_CALCULATED,
        WINDING_RESISTANCE_WINDING,
        WINDING_PASS_FAIL,
        WINDING_ERROR,
        WINDING_ERROR_DURATION,

        RESISTANCE_DONE = 0x31,
        RESISTANCE_CURRENT,
        RESISTANCE_MEASURED,
        RESISTANCE_CALCULATED,
        RESISTANCE_PASS_FAIL,
        RESISTANCE_ERROR,
        RESISTANCE_ERROR_DURATION,

        TAP_CHANGER_DONE = 0x41,
        TAP_CHANGER_TIME,
        TAP_CHANGER_CURRENT,
        TAP_CHANGER_VOLTAGE,
        TAP_CHANGER_RESISTANCE,
        TAP_CHANGER_ERROR,
        TAP_CHANGER_ERROR_DURATION,
        TAP_CHANGER_HISTORY_START_TIME,
        TAP_CHANGER_HISTORY_DURATION,
        TAP_CHANGER_HISTORY_COUNT,
        TAP_CHANGER_HISTORY_BUFFER,

        HEAT_RUN_DONE = 0x51,
        HEAT_RUN_START,
        HEAT_RUN_TIME,
        HEAT_RUN_DELAY,
        HEAT_RUN_CURRENT,
        HEAT_RUN_VOLTAGE,
        HEAT_RUN_RESISTANCE,
        HEAT_RUN_RESISTANCE_CALCULATED,
        HEAT_RUN_RESISTANCE_WINDING,
        HEAT_RUN_T_COLD,
        HEAT_RUN_R_COLD,
        HEAT_RUN_ERROR,
        HEAT_RUN_ERROR_DURATION,
        HEAT_RUN_APPROXIMATION_T0,
        HEAT_RUN_APPROXIMATION_R0,
        HEAT_RUN_APPROXIMATION_R_INF,
        HEAT_RUN_APPROXIMATION_TAU,
        HEAT_RUN_APPROXIMATION_CORRELATION,
        HEAT_RUN_HISTORY_START_TIME,
        HEAT_RUN_HISTORY_DURATION,
        HEAT_RUN_HISTORY_COUNT,
        HEAT_RUN_HISTORY_BUFFER,

        DEMAGNETIZATION_DONE = 0x71,
        DEMAGNETIZATION_CURRENT,
        DEMAGNETIZATION_HISTORY_START_TIME,
        DEMAGNETIZATION_HISTORY_DURATION,
        DEMAGNETIZATION_HISTORY_COUNT,
        DEMAGNETIZATION_HISTORY_BUFFER,

        END,
    }
    #endregion

    #region public classures
    public class Test_CableConnection_t
    {
        public TEST_PHASE H1;
        public TEST_PHASE H2;
        public TEST_PHASE H1ext;
        public TEST_PHASE H2ext;
        public TEST_PHASE X1;
        public TEST_PHASE X2;
        public TEST_PHASE X1ext;
        public TEST_PHASE X2ext;
    }
    public class Test_CalibrationData_t
    {
        public float Scale;
        public float Offset;
    }
    public class RTC_DateTime_t
    {
        public UInt32 UnixTime;
        public UInt16 Year;
        public Byte Month;
        public Byte Day;
        public Byte Hour;
        public Byte Minute;
        public Byte Second;
        public Byte DayOfWeek;
    }
    public class Test_Header_t
    {
        public string Company;
        public string Station;
        public string Operator;
        public string Manufacturer;
        public string Model;
        public string Serial;
        public string Power;
        public string Device;
        public string Version;
    }
    public class Test_VectorParam_t
    {
        public TEST_VECTOR Primary;
        public TEST_VECTOR Secondary;
        public TEST_VECTOR_NUMBERS Number;
        public string VectorString;
        public string PhaseString;
    }
    public class Test_TapParam_t
    {
        public bool Auto;
        public bool Direction;
        public bool Repower;
        public TEST_TAP_WAIT_MODE WaitMode;
        public TEST_SIDE Side;
        public bool UpDown;
        public byte Count;
        public byte Select;
        public UInt16 ContactTime;
        public UInt16 WaitTime;
        public List<UInt32> Items = new List<UInt32>();
        public Test_TapParam_t()
        {
            for (int i = 0; i < TAPS_COUNT_MAX; ++i)
            {
                Items.Add(0);
            }
        }
    }
    public class Test_TemperatureParam_t
    {
        public bool Enable;
        public bool Sensor;
        public TEST_TEMPERATURE_UNIT Unit;
        public TEST_MATERIAL Material;
        public double Value;
        public double Reference;
        public double Coefficient;
    }
    public class Test_StopModeParam_t
    {
        public TEST_STOP_MODE Mode;
        public UInt16 Count;
        public UInt16 Interval;
        public float Condition;
        public float Time;
        public float TimeOut;
        public float PassFail;
    }
    public class Test_IntervalParam_t
    {
        public bool Enable;
        public byte Count;
        public byte TotalHours;
        public byte TotalMinutes;
        public byte TotalSeconds;
        public float Value;
    }
    public class Test_ApproximationParam_t
    {
        public bool Enable;
        public bool Exponential;
        public float RCold;
        public float TCold;
    }
    public class Test_CurrentParam_t
    {
        public float Primary;
        public float Secondary;
    }
    public class Test_Setting_t
    {
        public UInt32 Primary;
        public UInt32 Secondary;
        public byte PhaseCount;
        public byte FrequencyValue;
        public TEST_FREQUENCY Frequency;
        public TEST_VOLTAGE Voltage;
        public UInt16 VoltageValue;
        public float PassFailError;
        public float TurnRatio;
    }
    public class Test_Phase_t
    {
        public TEST_PHASE Primary;
        public TEST_PHASE Secondary;
        public TEST_PHASE Select;
    }
    public class Test_Config_t
    {
        public bool Available;
        public bool Enable;
        public TEST_CONNECTION Connection;
        public TEST_SIDE Side;
        public TEST_SIDE SideSelected;
        public RTC_DateTime_t Time = new RTC_DateTime_t();
        public Test_Header_t Header = new Test_Header_t();
        public TEST_TYPE Type;
        public Test_Setting_t Setting = new Test_Setting_t();
        public Test_Phase_t Phase = new Test_Phase_t();
        public Test_TapParam_t Tap = new Test_TapParam_t();
        public Test_VectorParam_t Vector = new Test_VectorParam_t();
        public Test_TemperatureParam_t Temperature = new Test_TemperatureParam_t();
        public Test_CurrentParam_t Current = new Test_CurrentParam_t();
        public Test_StopModeParam_t Stop = new Test_StopModeParam_t();
        public Test_IntervalParam_t Interval = new Test_IntervalParam_t();
        public Test_ApproximationParam_t Approximation = new Test_ApproximationParam_t();
    }
    public class Test_MeasurementTurnRatio_t
    {
        public bool Done;
        public bool Updated;
        public bool PassFailEnable;
        public bool IsPassed;
        public float Current;
        public float Coefficient;
        public float Ratio;
        public float PhaseDiff;
        public float Error;
    }
    public class Test_MeasurementMagnetic_t
    {
        public bool Done;
        public bool Updated;
        public bool PassFailEnable;
        public bool IsPassed;
        public float Current;
        public List<float> Voltage = new List<float>();
        public List<float> Balance = new List<float>();
        public List<byte> BalanceCounter = new List<byte>();
        public Test_MeasurementMagnetic_t()
        {
            for (int i = 0; i < TEST_BALANCE_COUNT_MAX; ++i)
            {
                Voltage.Add(0.0f);
                Balance.Add(0.0f);
                BalanceCounter.Add(0);
            }
        }
    }
    public class Test_MeasurementApproximation_t
    {
        public float T0;
        public float R0;
        public float Rinf;
        public float Tau;
        public float Slope;
        public float Intersect;
        public float Correlation;
    }
    public class Test_HistoryPoint_t
    {
        public UInt16 Time;
        public float Value;
    }
    public class Test_HistoryTapChangerHelper_t
    {
        public bool Updated;
        public UInt32 StartTime;
        public float Duration;
        public byte Count;
        public List<Test_HistoryPoint_t> Buffer = new List<Test_HistoryPoint_t>();
        public Test_HistoryTapChangerHelper_t()
        {
            for (int i = 0; i < TEST_TAP_CHANGER_COUNT_MAX; ++i)
            {
                Buffer.Add(new Test_HistoryPoint_t());
            }
        }
    }
    public class Test_HistoryHeatRunHelper_t
    {
        public bool Updated;
        public UInt32 StartTime;
        public float Duration;
        public byte Count;
        public List<Test_HistoryPoint_t> Buffer = new List<Test_HistoryPoint_t>();
        public Test_HistoryHeatRunHelper_t()
        {
            for (int i = 0; i < TEST_HEAT_RUN_COUNT_MAX; ++i)
            {
                Buffer.Add(new Test_HistoryPoint_t());
            }
        }
    }
    public class Test_HistoryDemagnetizationHelper_t
    {
        public bool Updated;
        public UInt32 StartTime;
        public float Duration;
        public byte Count;
        public List<Test_HistoryPoint_t> Buffer = new List<Test_HistoryPoint_t>();
        public Test_HistoryDemagnetizationHelper_t()
        {
            for (int i = 0; i < TEST_DEMAGNETIZATION_COUNT_MAX; ++i)
            {
                Buffer.Add(new Test_HistoryPoint_t());
            }
        }
    }
    public class Test_MeasurementWinding_t
    {
        public bool Done;
        public bool Updated;
        public bool GetData;
        public bool PassFailEnable;
        public bool IsPassed;
        public float Current;
        public float ResistanceMeasured;
        public float ResistanceCalculated;
        public float ResistanceWinding;
        public float PassFail;
        public float Error;
        public float ErrorDuration;
    }
    public class Test_MeasurementTapChanger_t
    {
        public bool Done;
        public bool Updated;
        public bool PassFailEnable;
        public bool IsPassed;
        public float Time;
        public float Current;
        public float Voltage;
        public float Resistance;
        public float Error;
        public float ErrorDuration;
        public Test_HistoryTapChangerHelper_t History = new Test_HistoryTapChangerHelper_t();
    }
    public class Test_MeasurementHeatRun_t
    {
        public bool Done;
        public bool Updated;
        public bool Start;
        public bool PassFailEnable;
        public bool IsPassed;
        public float Time;
        public float Delay;
        public float Current;
        public float Voltage;
        public float Resistance;
        public float ResistanceCalculated;
        public float ResistanceWinding;
        public float Error;
        public float ErrorDuration;
        public Test_MeasurementApproximation_t Approximation = new Test_MeasurementApproximation_t();
        public Test_HistoryHeatRunHelper_t History = new Test_HistoryHeatRunHelper_t();
    }
    public class Test_MeasurementDemagnetization_t
    {
        public bool Done;
        public bool Updated;
        public bool PassFailEnable;
        public bool IsPassed;
        public float Current;
        public Test_HistoryDemagnetizationHelper_t History = new Test_HistoryDemagnetizationHelper_t();
    }
    public class Test_FlagHelper_t
    {
        public bool Enable;
        public bool NeedToRefreshUI;
        public bool PrintLog;
        public bool FirstRun;
        public byte Reserved;
    }
    public class Test_Measurement_t
    {
        public List<List<Test_MeasurementTurnRatio_t>> TurnRatio = new List<List<Test_MeasurementTurnRatio_t>>();
        public List<Test_MeasurementMagnetic_t> Magnetic = new List<Test_MeasurementMagnetic_t>();
        public List<List<List<Test_MeasurementWinding_t>>> Winding = new List<List<List<Test_MeasurementWinding_t>>>();
        public List<List<Test_MeasurementWinding_t>> Resistance = new List<List<Test_MeasurementWinding_t>>();
        public List<Test_MeasurementTapChanger_t> TapChanger = new List<Test_MeasurementTapChanger_t>();
        public Test_MeasurementHeatRun_t HeatRun = new Test_MeasurementHeatRun_t();
        public Test_MeasurementDemagnetization_t Demagnetization = new Test_MeasurementDemagnetization_t();
        public Test_Measurement_t()
        {
            for (int p = 0; p < PHASE_COUNT_MAX; ++p)
            {
                TurnRatio.Add(new List<Test_MeasurementTurnRatio_t>());
                for (int t = 0; t < TAPS_COUNT_MAX; ++t)
                {
                    TurnRatio[p].Add(new Test_MeasurementTurnRatio_t());
                }
            }
            for (int p = 0; p < PHASE_COUNT_MAX; ++p)
            {
                Magnetic.Add(new Test_MeasurementMagnetic_t());
            }
            for (int s = 0; s < SIDES_COUNT_MAX; ++s)
            {
                Winding.Add(new List<List<Test_MeasurementWinding_t>>());
                for (int p = 0; p < PHASE_COUNT_MAX; ++p)
                {
                    Winding[s].Add(new List<Test_MeasurementWinding_t>());
                    for (int t = 0; t < TAPS_COUNT_MAX; ++t)
                    {
                        Winding[s][p].Add(new Test_MeasurementWinding_t());
                    }
                }
            }
            for (int s = 0; s < SIDES_COUNT_MAX; ++s)
            {
                Resistance.Add(new List<Test_MeasurementWinding_t>());
                for (int p = 0; p < PHASE_COUNT_MAX; ++p)
                {
                    Resistance[s].Add(new Test_MeasurementWinding_t());
                }
            }
            for (int t = 0; t < TAPS_COUNT_MAX; ++t)
            {
                TapChanger.Add(new Test_MeasurementTapChanger_t());
            }
        }
    }
    public class Test_Helper_t
    {
        public Test_Config_t Config = new Test_Config_t();
        public Test_FlagHelper_t Flag = new Test_FlagHelper_t();
        public Test_Measurement_t Measurement = new Test_Measurement_t();
        public TEST_MODE ModeOld;
        public TEST_MODE Mode;
        public TEST_STATUS Status;
        public TEST_FAULT Fault;
    }
    #endregion

    public TRAN()
    {
    }
    
    private UInt16 GetDataFromBuffer_UInt16(byte[] buffer, int index)
    {
        byte[] b = new byte[2];
        for (int i = 0; i < b.Length; ++i)
        {
            b[i] = buffer[i + index];
        }
        //Array.Reverse(b);
        return BitConverter.ToUInt16(b, 0);
    }
    private UInt32 GetDataFromBuffer_UInt32(byte[] buffer, int index)
    {
        byte[] b = new byte[4];
        for (int i = 0; i < b.Length; ++i)
        {
            b[i] = buffer[i + index];
        }
        return BitConverter.ToUInt32(b, 0);
    }
    private float GetDataFromBuffer_Float(byte[] buffer, int index)
    {
        byte[] b = new byte[4];
        for (int i = 0; i < b.Length; ++i)
        {
            b[i] = buffer[i + index];
        }
        return BitConverter.ToSingle(b, 0);
    }
    public Test_Helper_t ParseRecordData(byte[] buffer)
    {
        int index = 0;
        Test_Helper_t test = new Test_Helper_t();
        test.Config.Available = Convert.ToBoolean((buffer[index] >> 0) & 0x01);
        test.Config.Enable = Convert.ToBoolean((buffer[index] >> 1) & 0x01);
        test.Config.Connection = (TEST_CONNECTION)((buffer[index] >> 2) & 0x03);
        test.Config.Side = (TEST_SIDE)((buffer[index] >> 4) & 0x03);
        test.Config.SideSelected = (TEST_SIDE)((buffer[index] >> 6) & 0x03);
        index++;
        test.Config.Time.UnixTime = GetDataFromBuffer_UInt32(buffer, index); index += 4;
        test.Config.Time.Year = GetDataFromBuffer_UInt16(buffer, index); index += 2;
        test.Config.Time.Month = buffer[index++];
        test.Config.Time.Day = buffer[index++];
        test.Config.Time.Hour = buffer[index++];
        test.Config.Time.Minute = buffer[index++];
        test.Config.Time.Second = buffer[index++];
        test.Config.Time.DayOfWeek = buffer[index++];

        test.Config.Header.Company = new string(System.Text.Encoding.ASCII.GetChars(buffer, index, HEADER_FIELD_LEN)); index += HEADER_FIELD_LEN;
        test.Config.Header.Station = new string(System.Text.Encoding.ASCII.GetChars(buffer, index, HEADER_FIELD_LEN)); index += HEADER_FIELD_LEN;
        test.Config.Header.Operator = new string(System.Text.Encoding.ASCII.GetChars(buffer, index, HEADER_FIELD_LEN)); index += HEADER_FIELD_LEN;
        test.Config.Header.Manufacturer = new string(System.Text.Encoding.ASCII.GetChars(buffer, index, HEADER_FIELD_LEN)); index += HEADER_FIELD_LEN;
        test.Config.Header.Model = new string(System.Text.Encoding.ASCII.GetChars(buffer, index, HEADER_FIELD_LEN)); index += HEADER_FIELD_LEN;
        test.Config.Header.Serial = new string(System.Text.Encoding.ASCII.GetChars(buffer, index, HEADER_FIELD_LEN)); index += HEADER_FIELD_LEN;
        test.Config.Header.Power = new string(System.Text.Encoding.ASCII.GetChars(buffer, index, HEADER_FIELD_LEN)); index += HEADER_FIELD_LEN;
        test.Config.Header.Device = new string(System.Text.Encoding.ASCII.GetChars(buffer, index, HEADER_FIELD_LEN)); index += HEADER_FIELD_LEN;
        test.Config.Header.Version = new string(System.Text.Encoding.ASCII.GetChars(buffer, index, HEADER_FIELD_LEN)); index += HEADER_FIELD_LEN;

        test.Config.Type = (TEST_TYPE)(buffer[index++]);                                                        
        test.Config.Setting.Primary = GetDataFromBuffer_UInt32(buffer, index); index += 4;
        test.Config.Setting.Secondary = GetDataFromBuffer_UInt32(buffer, index); index += 4;
        test.Config.Setting.PhaseCount = buffer[index++];
        test.Config.Setting.FrequencyValue = buffer[index++];
        test.Config.Setting.Frequency = (TEST_FREQUENCY)buffer[index++];
        test.Config.Setting.Voltage = (TEST_VOLTAGE)buffer[index++];
        test.Config.Setting.VoltageValue = GetDataFromBuffer_UInt16(buffer, index); index += 2;
        test.Config.Setting.PassFailError = GetDataFromBuffer_Float(buffer, index); index += 4;
        test.Config.Setting.TurnRatio = GetDataFromBuffer_Float(buffer, index); index += 4;

        test.Config.Phase.Primary = (TEST_PHASE)((buffer[index] >> 0) & 0x02);
        test.Config.Phase.Secondary = (TEST_PHASE)((buffer[index] >> 2) & 0x02);
        test.Config.Phase.Select = (TEST_PHASE)((buffer[index] >> 4) & 0x02);
        index++;
        test.Config.Tap.Auto = Convert.ToBoolean((buffer[index] >> 0) & 0x01);
        test.Config.Tap.Direction = Convert.ToBoolean((buffer[index] >> 1) & 0x01);
        test.Config.Tap.Repower = Convert.ToBoolean((buffer[index] >> 2) & 0x01);
        test.Config.Tap.WaitMode = (TEST_TAP_WAIT_MODE)((buffer[index] >> 3) & 0x02);
        test.Config.Tap.Side = (TEST_SIDE)((buffer[index] >> 5) & 0x02);
        test.Config.Tap.UpDown = Convert.ToBoolean((buffer[index] >> 7) & 0x01);
        index++;
        test.Config.Tap.Count = buffer[index++];
        test.Config.Tap.Select = buffer[index++];
        test.Config.Tap.ContactTime = GetDataFromBuffer_UInt16(buffer, index); index += 2;
        test.Config.Tap.WaitTime = GetDataFromBuffer_UInt16(buffer, index); index += 2;
        for (int i = 0; i < TAPS_COUNT_MAX; ++i)
        {
            test.Config.Tap.Items[i] = GetDataFromBuffer_UInt32(buffer, index); index += 4;
        }

        test.Config.Vector.Primary = (TEST_VECTOR)((buffer[index] >> 0) & 0x0F);
        test.Config.Vector.Secondary = (TEST_VECTOR)((buffer[index] >> 4) & 0x0F);
        index++;
        test.Config.Vector.Number = (TEST_VECTOR_NUMBERS)(buffer[index++]);
        test.Config.Vector.VectorString = new string(System.Text.Encoding.ASCII.GetChars(buffer, index, 16)); index += 16;
        test.Config.Vector.PhaseString = new string(System.Text.Encoding.ASCII.GetChars(buffer, index, 16)); index += 16;

        test.Config.Temperature.Enable = Convert.ToBoolean((buffer[index] >> 0) & 0x01);
        test.Config.Temperature.Sensor = Convert.ToBoolean((buffer[index] >> 1) & 0x01);
        test.Config.Temperature.Unit = (TEST_TEMPERATURE_UNIT)((buffer[index] >> 2) & 0x02);
        test.Config.Temperature.Material = (TEST_MATERIAL)((buffer[index] >> 4) & 0x0F);
        index++;
        test.Config.Temperature.Value = GetDataFromBuffer_Float(buffer, index); index += 4;
        test.Config.Temperature.Reference = GetDataFromBuffer_Float(buffer, index); index += 4;
        test.Config.Temperature.Coefficient = GetDataFromBuffer_Float(buffer, index); index += 4;

        test.Config.Current.Primary = GetDataFromBuffer_Float(buffer, index); index += 4;
        test.Config.Current.Secondary = GetDataFromBuffer_Float(buffer, index); index += 4;

        test.Config.Stop.Mode = (TEST_STOP_MODE)(buffer[index++]);
        test.Config.Stop.Count = (buffer[index++]);                                                          
        test.Config.Stop.Interval = GetDataFromBuffer_UInt16(buffer, index); index += 2;
        test.Config.Stop.Condition = GetDataFromBuffer_Float(buffer, index); index += 4;
        test.Config.Stop.Time = GetDataFromBuffer_Float(buffer, index); index += 4;
        test.Config.Stop.TimeOut = GetDataFromBuffer_Float(buffer, index); index += 4;
        test.Config.Stop.PassFail = GetDataFromBuffer_Float(buffer, index); index += 4;

        test.Config.Interval.Enable = Convert.ToBoolean(buffer[index++]);
        test.Config.Interval.Count = buffer[index++];
        test.Config.Interval.TotalHours = buffer[index++];
        test.Config.Interval.TotalMinutes = buffer[index++];
        test.Config.Interval.TotalSeconds = buffer[index++];
        test.Config.Interval.Value = GetDataFromBuffer_Float(buffer, index); index += 4;

        test.Config.Approximation.Enable = Convert.ToBoolean((buffer[index] >> 0) & 0x01);
        test.Config.Approximation.Exponential = Convert.ToBoolean((buffer[index] >> 1) & 0x01);
        index++;
        test.Config.Approximation.RCold = GetDataFromBuffer_Float(buffer, index); index += 4;
        test.Config.Approximation.TCold = GetDataFromBuffer_Float(buffer, index); index += 4;

        test.Flag.Enable = Convert.ToBoolean((buffer[index] >> 0) & 0x01);
        test.Flag.NeedToRefreshUI = Convert.ToBoolean((buffer[index] >> 1) & 0x01);
        test.Flag.PrintLog = Convert.ToBoolean((buffer[index] >> 2) & 0x01);
        test.Flag.FirstRun = Convert.ToBoolean((buffer[index] >> 3) & 0x01);
        index += 1;                                                                                               

        if (buffer[index++] == 'T')
        {
            for (int p = 0; p < PHASE_COUNT_MAX; ++p)
            {
                for (int t = 0; t < TAPS_COUNT_MAX; ++t)
                {
                    test.Measurement.TurnRatio[p][t].Done = Convert.ToBoolean((buffer[index] >> 0) & 0x01);
                    test.Measurement.TurnRatio[p][t].Updated = Convert.ToBoolean((buffer[index] >> 1) & 0x01);
                    test.Measurement.TurnRatio[p][t].PassFailEnable = Convert.ToBoolean((buffer[index] >> 2) & 0x01);
                    test.Measurement.TurnRatio[p][t].IsPassed = Convert.ToBoolean((buffer[index] >> 3) & 0x01);
                    index++;
                    test.Measurement.TurnRatio[p][t].Current = GetDataFromBuffer_Float(buffer, index); index += 4;
                    test.Measurement.TurnRatio[p][t].Coefficient = GetDataFromBuffer_Float(buffer, index); index += 4;
                    test.Measurement.TurnRatio[p][t].Ratio = GetDataFromBuffer_Float(buffer, index); index += 4;
                    test.Measurement.TurnRatio[p][t].PhaseDiff = GetDataFromBuffer_Float(buffer, index); index += 4;
                    test.Measurement.TurnRatio[p][t].Error = GetDataFromBuffer_Float(buffer, index); index += 4;
                }
            }
        }
        if (buffer[index++] == 'M')
        {
            for (int p = 0; p < PHASE_COUNT_MAX; ++p)
            {
                test.Measurement.Magnetic[p].Done = Convert.ToBoolean((buffer[index] >> 0) & 0x01);
                test.Measurement.Magnetic[p].Updated = Convert.ToBoolean((buffer[index] >> 1) & 0x01);
                test.Measurement.Magnetic[p].PassFailEnable = Convert.ToBoolean((buffer[index] >> 2) & 0x01);
                test.Measurement.Magnetic[p].IsPassed = Convert.ToBoolean((buffer[index] >> 3) & 0x01);
                index++;
                test.Measurement.Magnetic[p].Current = GetDataFromBuffer_Float(buffer, index); index += 4;
                for (int b = 0; b < TEST_BALANCE_COUNT_MAX; ++b)
                {
                    test.Measurement.Magnetic[p].Voltage[b] = GetDataFromBuffer_Float(buffer, index); index += 4;
                }
                for (int b = 0; b < TEST_BALANCE_COUNT_MAX; ++b)
                {
                    test.Measurement.Magnetic[p].Balance[b] = GetDataFromBuffer_Float(buffer, index); index += 4;
                }
                for (int b = 0; b < TEST_BALANCE_COUNT_MAX; ++b)
                {
                    test.Measurement.Magnetic[p].BalanceCounter[b] = buffer[index++];
                }
            }
        }
        if (buffer[index++] == 'W')
        {
            for (int s = 0; s < SIDES_COUNT_MAX; ++s)
            {
                for (int p = 0; p < PHASE_COUNT_MAX; ++p)
                {
                    for (int t = 0; t < TAPS_COUNT_MAX; ++t)
                    {
                        test.Measurement.Winding[s][p][t].Done = Convert.ToBoolean((buffer[index] >> 0) & 0x01);
                        test.Measurement.Winding[s][p][t].Updated = Convert.ToBoolean((buffer[index] >> 1) & 0x01);
                        test.Measurement.Winding[s][p][t].GetData = Convert.ToBoolean((buffer[index] >> 2) & 0x01);
                        test.Measurement.Winding[s][p][t].PassFailEnable = Convert.ToBoolean((buffer[index] >> 3) & 0x01);
                        test.Measurement.Winding[s][p][t].IsPassed = Convert.ToBoolean((buffer[index] >> 4) & 0x01);
                        index++;
                        test.Measurement.Winding[s][p][t].Current = GetDataFromBuffer_Float(buffer, index); index += 4;
                        test.Measurement.Winding[s][p][t].ResistanceMeasured = GetDataFromBuffer_Float(buffer, index); index += 4;
                        test.Measurement.Winding[s][p][t].ResistanceCalculated = GetDataFromBuffer_Float(buffer, index); index += 4;
                        test.Measurement.Winding[s][p][t].ResistanceWinding = GetDataFromBuffer_Float(buffer, index); index += 4;
                        test.Measurement.Winding[s][p][t].PassFail = GetDataFromBuffer_Float(buffer, index); index += 4;
                        test.Measurement.Winding[s][p][t].Error = GetDataFromBuffer_Float(buffer, index); index += 4;
                        test.Measurement.Winding[s][p][t].ErrorDuration = GetDataFromBuffer_Float(buffer, index); index += 4;
                    }
                }
            }
        }
        if (buffer[index++] == 'R')
        {
            for (int s = 0; s < SIDES_COUNT_MAX; ++s)
            {
                for (int p = 0; p < PHASE_COUNT_MAX; ++p)
                {
                    test.Measurement.Resistance[s][p].Done = Convert.ToBoolean((buffer[index] >> 0) & 0x01);
                    test.Measurement.Resistance[s][p].Updated = Convert.ToBoolean((buffer[index] >> 1) & 0x01);
                    test.Measurement.Resistance[s][p].GetData = Convert.ToBoolean((buffer[index] >> 2) & 0x01);
                    test.Measurement.Resistance[s][p].PassFailEnable = Convert.ToBoolean((buffer[index] >> 3) & 0x01);
                    test.Measurement.Resistance[s][p].IsPassed = Convert.ToBoolean((buffer[index] >> 4) & 0x01);
                    index++;
                    test.Measurement.Resistance[s][p].Current = GetDataFromBuffer_Float(buffer, index); index += 4;
                    test.Measurement.Resistance[s][p].ResistanceMeasured = GetDataFromBuffer_Float(buffer, index); index += 4;
                    test.Measurement.Resistance[s][p].ResistanceCalculated = GetDataFromBuffer_Float(buffer, index); index += 4;
                    test.Measurement.Resistance[s][p].ResistanceWinding = GetDataFromBuffer_Float(buffer, index); index += 4;
                    test.Measurement.Resistance[s][p].PassFail = GetDataFromBuffer_Float(buffer, index); index += 4;
                    test.Measurement.Resistance[s][p].Error = GetDataFromBuffer_Float(buffer, index); index += 4;
                    test.Measurement.Resistance[s][p].ErrorDuration = GetDataFromBuffer_Float(buffer, index); index += 4;
                }
            }
        }
        if (buffer[index++] == 'C')
        {
            for (int t = 0; t < TAPS_COUNT_MAX; ++t)
            {
                test.Measurement.TapChanger[t].Done = Convert.ToBoolean((buffer[index] >> 0) & 0x01);
                test.Measurement.TapChanger[t].Updated = Convert.ToBoolean((buffer[index] >> 1) & 0x01);
                test.Measurement.TapChanger[t].PassFailEnable = Convert.ToBoolean((buffer[index] >> 2) & 0x01);
                test.Measurement.TapChanger[t].IsPassed = Convert.ToBoolean((buffer[index] >> 3) & 0x01);
                index++;
                test.Measurement.TapChanger[t].Time = GetDataFromBuffer_Float(buffer, index); index += 4;
                test.Measurement.TapChanger[t].Current = GetDataFromBuffer_Float(buffer, index); index += 4;
                test.Measurement.TapChanger[t].Voltage = GetDataFromBuffer_Float(buffer, index); index += 4;
                test.Measurement.TapChanger[t].Resistance = GetDataFromBuffer_Float(buffer, index); index += 4;
                test.Measurement.TapChanger[t].Error = GetDataFromBuffer_Float(buffer, index); index += 4;
                test.Measurement.TapChanger[t].ErrorDuration = GetDataFromBuffer_Float(buffer, index); index += 4;

                test.Measurement.TapChanger[t].History.Updated = Convert.ToBoolean((buffer[index] >> 0) & 0x01);
                test.Measurement.TapChanger[t].History.StartTime = GetDataFromBuffer_UInt32(buffer, index); index += 4;
                test.Measurement.TapChanger[t].History.StartTime >>= 1;
                test.Measurement.TapChanger[t].History.Duration = GetDataFromBuffer_Float(buffer, index); index += 4;
                test.Measurement.TapChanger[t].History.Count = buffer[index++];
                for (int d = 0; d < TEST_TAP_CHANGER_COUNT_MAX; ++d)
                {
                    test.Measurement.TapChanger[t].History.Buffer[d].Time = GetDataFromBuffer_UInt16(buffer, index); index += 2;
                    test.Measurement.TapChanger[t].History.Buffer[d].Value = GetDataFromBuffer_Float(buffer, index); index += 4;
                }
            }
        }
        if (buffer[index++] == 'H')
        {
            test.Measurement.HeatRun.Done = Convert.ToBoolean((buffer[index] >> 0) & 0x01);
            test.Measurement.HeatRun.Updated = Convert.ToBoolean((buffer[index] >> 1) & 0x01);
            test.Measurement.HeatRun.Start = Convert.ToBoolean((buffer[index] >> 1) & 0x01);
            test.Measurement.HeatRun.PassFailEnable = Convert.ToBoolean((buffer[index] >> 3) & 0x01);
            test.Measurement.HeatRun.IsPassed = Convert.ToBoolean((buffer[index] >> 4) & 0x01);
            index++;
            test.Measurement.HeatRun.Time = GetDataFromBuffer_Float(buffer, index); index += 4;
            test.Measurement.HeatRun.Delay = GetDataFromBuffer_Float(buffer, index); index += 4;
            test.Measurement.HeatRun.Current = GetDataFromBuffer_Float(buffer, index); index += 4;
            test.Measurement.HeatRun.Voltage = GetDataFromBuffer_Float(buffer, index); index += 4;
            test.Measurement.HeatRun.Resistance = GetDataFromBuffer_Float(buffer, index); index += 4;
            test.Measurement.HeatRun.ResistanceCalculated = GetDataFromBuffer_Float(buffer, index); index += 4;
            test.Measurement.HeatRun.ResistanceWinding = GetDataFromBuffer_Float(buffer, index); index += 4;
            test.Measurement.HeatRun.Error = GetDataFromBuffer_Float(buffer, index); index += 4;
            test.Measurement.HeatRun.ErrorDuration = GetDataFromBuffer_Float(buffer, index); index += 4;
            test.Measurement.HeatRun.Approximation.T0 = GetDataFromBuffer_Float(buffer, index); index += 4;
            test.Measurement.HeatRun.Approximation.R0 = GetDataFromBuffer_Float(buffer, index); index += 4;
            test.Measurement.HeatRun.Approximation.Rinf = GetDataFromBuffer_Float(buffer, index); index += 4;
            test.Measurement.HeatRun.Approximation.Tau = GetDataFromBuffer_Float(buffer, index); index += 4;
            test.Measurement.HeatRun.Approximation.Slope = GetDataFromBuffer_Float(buffer, index); index += 4;
            test.Measurement.HeatRun.Approximation.Intersect = GetDataFromBuffer_Float(buffer, index); index += 4;
            test.Measurement.HeatRun.Approximation.Correlation = GetDataFromBuffer_Float(buffer, index); index += 4;
            test.Measurement.HeatRun.History.Updated = Convert.ToBoolean((buffer[index] >> 0) & 0x01);
            test.Measurement.HeatRun.History.StartTime = GetDataFromBuffer_UInt32(buffer, index); index += 4;
            test.Measurement.HeatRun.History.StartTime >>= 1;
            test.Measurement.HeatRun.History.Duration = GetDataFromBuffer_Float(buffer, index); index += 4;
            test.Measurement.HeatRun.History.Count = buffer[index++];
            for (int d = 0; d < TEST_HEAT_RUN_COUNT_MAX; ++d)
            {
                test.Measurement.HeatRun.History.Buffer[d].Time = GetDataFromBuffer_UInt16(buffer, index); index += 2;
                test.Measurement.HeatRun.History.Buffer[d].Value = GetDataFromBuffer_Float(buffer, index); index += 4;
            }
        }
        if (buffer[index++] == 'D')
        {
            test.Measurement.Demagnetization.Done = Convert.ToBoolean((buffer[index] >> 0) & 0x01);
            test.Measurement.Demagnetization.Updated = Convert.ToBoolean((buffer[index] >> 1) & 0x01);
            test.Measurement.Demagnetization.PassFailEnable = Convert.ToBoolean((buffer[index] >> 2) & 0x01);
            test.Measurement.Demagnetization.IsPassed = Convert.ToBoolean((buffer[index] >> 3) & 0x01);
            index++;
            test.Measurement.Demagnetization.Current = GetDataFromBuffer_Float(buffer, index); index += 4;
            test.Measurement.Demagnetization.History.Updated = Convert.ToBoolean((buffer[index] >> 0) & 0x01);
            test.Measurement.Demagnetization.History.StartTime = GetDataFromBuffer_UInt32(buffer, index); index += 4;
            test.Measurement.Demagnetization.History.StartTime >>= 1;
            test.Measurement.Demagnetization.History.Duration = GetDataFromBuffer_Float(buffer, index); index += 4;
            test.Measurement.Demagnetization.History.Count = buffer[index++];
            for (int d = 0; d < TEST_DEMAGNETIZATION_COUNT_MAX; ++d)
            {
                test.Measurement.Demagnetization.History.Buffer[d].Time = GetDataFromBuffer_UInt16(buffer, index); index += 2;
                test.Measurement.Demagnetization.History.Buffer[d].Value = GetDataFromBuffer_Float(buffer, index); index += 4;
            }
        }
        test.ModeOld = (TEST_MODE)buffer[index++];
        test.Mode = (TEST_MODE)buffer[index++];
        test.Status = (TEST_STATUS)buffer[index++];
        test.Fault = (TEST_FAULT)buffer[index++];

        return test;
    }
}
