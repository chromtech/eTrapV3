using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;
using System.Deployment.Application;

namespace DataLogging
{

    public class DatenKlasse : INotifyPropertyChanged
    {
        /// <summary>
        /// DEBUG = true
        /// Use _debugAdd to append a string to the DebugString Collection.
        /// The FORM may read out all the Debug Strings and delete them
        /// </summary>
        private const bool DEBUG = true;
        private StringCollection _debugstring = new StringCollection();
        public StringCollection DebugString
        {
            get { return _debugstring; }
            set
            {
                _debugstring = value;
            }
        }
        private void _DebugAdd(string s)
        {
            if (DEBUG) { DebugString.Add("Vars: " + s); }
        }




        public DatenKlasse()
        {
            _Logging = true;
            _pid_max_temperature = 350;
            _pid_min_temperature = -25;
        }

        private string dMakeUS(double value)
        {
            //return value.ToString("N", new CultureInfo("en-US"));
            return value.ToString("#,#0.0######", CultureInfo.InvariantCulture);
        }

        // Utils:
        public double limit_dbl(double value, double low, double high)
        {
            if (value < low) { value = low; }
            if (value > high) { value = high; }
            return value;
        }
        public int limit(int value, int low, int high)
        {
            if (value < low) { value = low; }
            if (value > high) { value = high; }
            return value;
        }
        /*
                private int  { get; set; }
                public int 
                {
                    get { return ; }
                    set
                    {
                         = value;
                    }
                }
        */
        public Status ControlerStatus { get; set; }

        public enum Status
        {
            STANDBY,
            TEST
        }

        private int _ControlerPower { get; set; }
        public int ControlerPower
        {
            get { return _ControlerPower; }
            set
            {
                _ControlerPower = value;
            }
        }

        private int _TimeSinceStartup = 0;
        public int TimeSinceStartup
        {
            get { return _TimeSinceStartup; }
            set
            {
                _TimeSinceStartup = value;
                if (_TimeSinceStartup > 100)
                    _TimeSinceStartup = 100;
            }
        }



        private int _eTrapNumBadReadbacks = 0;
        public int eTrapNumBadReadbacks
        {
            get { return _eTrapNumBadReadbacks; }
            set
            {
                _eTrapNumBadReadbacks = value;
                NotifyPropertyChanged("eTrapNumBadReadbacks");
            }
        }


        private bool _autoRestart = false;
        public bool autoRestart
        {
            get { return _autoRestart; }
            set
            {
                _autoRestart = value;
                //NotifyPropertyChanged("eTrapIsConnected");
            }
        }

        private bool _eTrapIsConnected = false;
        public bool eTrapIsConnected
        {
            get { return _eTrapIsConnected; }
            set
            {
                _eTrapIsConnected = value;
                NotifyPropertyChanged("eTrapIsConnected");
            }
        }

        private string _eTrapDisplayBoxString { get; set; }
        public string eTrapDisplayBoxString
        {
            get { return _eTrapDisplayBoxString; }
            set
            {
                if (_CycleRunning == 0)
                { if (!IsBrukerDesign)
                        _eTrapDisplayBoxString = " e-Trap - Standby ";
                    else
                        _eTrapDisplayBoxString = " PeakTrap - Standby ";
                }
                else
                {
                    if (_FirstLoggingDone == true)
                    {
                        _eTrapDisplayBoxString = Do_eTrapDisplayString(_CycleLine, _Atom, _Parm1, _Parm2, _Parm3);
                    }
                }
                NotifyPropertyChanged("eTrapDisplayBoxString");
            }
        }

        private string _ControlerStatusString = " ";
        public string ControlerStatusString
        {
            get { return _ControlerStatusString; }
            set
            {
                string _gcString = "GC on/ ";
                string _cString = "Controller on/ ";
                string _trapString = "Trap connected/ ";

                if ((_ControlerPower < 5) || (_ControlerPower > 12))
                    _cString = "Controller off/ ";

                if (_currentTemp > 450)
                    _trapString = "Trap disconnected or Temp Fuse OFF/ ";

                if ((_GC_Start == GC_Prepare) && (_GC_Start == 0))
                    _gcString = "GC off/ ";
                if ((_GC_Start == 0) && (GC_Prepare == 1))
                    _gcString = "GC off/ ";

                if (CryoTimeout > 0)
                    _ControlerStatusString = "Cryo Timeout - Load Method to Restart";
                else
                    _ControlerStatusString = _cString + _trapString + _gcString;

                NotifyPropertyChanged("ControlerStatusString");
            }

        }


        private int _SoftwareMainVersion = 3;
        public int SoftwareMainVersion
        {
            get { return _SoftwareMainVersion; }
            set
            {
                _SoftwareMainVersion = value;
            }
        }
        private string _SoftwareMainVersionString = "0";
        public string SoftwareMainVersionString
        {
            get { return _SoftwareMainVersionString; }
            set
            {
                _SoftwareMainVersionString = SoftwareMainVersion.ToString();
                NotifyPropertyChanged("SoftwareMainVersionString");
            }
        }

        private int _SoftwareSubVersion = 0;
        public int SoftwareSubVersion
        {
            get { return _SoftwareSubVersion; }
            set
            {
                _SoftwareSubVersion = value;
            }
        }
        private string _SoftwareSubVersionString = "0";
        public string SoftwareSubVersionString
        {
            get { return _SoftwareSubVersionString; }
            set
            {
                _SoftwareSubVersionString = SoftwareSubVersion.ToString();
                NotifyPropertyChanged("SoftwareSubVersionString");
            }
        }


        private string _SoftwareVersionString = "2.8.3.x (12/20)";
        public string SoftwareVersionString
        {
            get { return _SoftwareVersionString; }
            set
            {
                if (ApplicationDeployment.IsNetworkDeployed)
                {
                    _SoftwareVersionString = ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString(2) + " (01/21)";
                    SoftwareMainVersion = ApplicationDeployment.CurrentDeployment.CurrentVersion.Major;
                    SoftwareSubVersion = ApplicationDeployment.CurrentDeployment.CurrentVersion.Minor;
                }
                else
                { _SoftwareVersionString = "3.0.1.x (01/21)";
                    SoftwareMainVersion = 3;
                    SoftwareSubVersion = 0;
                }
                NotifyPropertyChanged("SoftwareVersionString");
                // kick it
                SoftwareMainVersionString = "";
                SoftwareSubVersionString = "";
            }
        }

        private int _FirmwareMainVersion { get; set; }
        public int FirmwareMainVersion
        {
            get { return _FirmwareMainVersion; }
            set
            {
                _FirmwareMainVersion = value;
            }
        }
        private int _FirmwareSubVersion { get; set; }
        public int FirmwareSubVersion
        {
            get { return _FirmwareSubVersion; }
            set
            {
                _FirmwareSubVersion = value;
            }
        }
        private string _FirmwareVersionString { get; set; }
        public string FirmwareVersionString
        {
            get { return _FirmwareVersionString; }
            set
            {
                _FirmwareVersionString = FirmwareMainVersion + "." + FirmwareSubVersion;
                NotifyPropertyChanged("FirmwareVersionString");
            }
        }

        private string _FirmwareFileName_FromVersionNumber { get; set; }
        public string FirmwareFileName_FromVersionNo
        {
            get { return _FirmwareFileName_FromVersionNumber; }
            set
            {
                _FirmwareFileName_FromVersionNumber = "eTrap_FW_" + SoftwareMainVersionString + "." + SoftwareSubVersionString + ".hex";    // NOT FirmwareVersionString. We load a NEW FW   
            }
        }


        // Method
        private string _macro_FileName { get; set; }
        public string macro_FileName
        {
            get { return _macro_FileName; }
            set
            {
                _macro_FileName = value;
                NotifyPropertyChanged("macro_FileName");
            }
        }
        private string _macro_Name { get; set; }
        public string macro_Name
        {
            get { return _macro_Name; }
            set
            {
                _macro_Name = value;
                NotifyPropertyChanged("macro_Name");
            }
        }

        // IsBrukerDesign:
        private bool _IsBrukerDesign = false;
        public bool IsBrukerDesign
        {
            get { return _IsBrukerDesign; }
            set
            {
                _IsBrukerDesign = value;
            }
        }


        private bool _StartedFromMassHunter = false;
        public bool StartedFromMassHunter
        {
            get { return _StartedFromMassHunter; }
            set
            {
                _StartedFromMassHunter = value;
            }
        }

        private bool _StartedFromMassHunter_LastTime = false;
        public bool StartedFromMassHunter_LastTime
        {
            get { return _StartedFromMassHunter_LastTime; }
            set
            {
                _StartedFromMassHunter_LastTime = value;
            }
        }

        /// <summary>
        /// Extract the Path from current method_FullPathAndFilename
        /// </summary>
        private void _makeMethodPath()
        {
            bool _massHunterMode = false;

            if (method_FullPathAndFilename.LastIndexOf(".M\\") > 0)
                _massHunterMode = true;

            if (!StartedFromMassHunter)
            {
                _method_Path = method_FullPathAndFilename.Substring(0, method_FullPathAndFilename.LastIndexOf("\\") + 1);
            }
            else
            {
                if (_massHunterMode)
                {   // "C:\MASSHUNTER\GCMS\1\METHODS\DEFAULT.M\eTrap.cme"
                    _method_Path = method_FullPathAndFilename.Substring(0, method_FullPathAndFilename.LastIndexOf(".M\\"));
                    _method_Path = _method_Path.Substring(0, _method_Path.LastIndexOf("\\") + 1);
                }
                else
                {   // in case MH user moved to the Chromtech folder
                    _method_Path = method_FullPathAndFilename.Substring(0, method_FullPathAndFilename.LastIndexOf("\\") + 1);
                }
            }

            _DebugAdd("_makeMethodPath: _method_Path=" + _method_Path);
        }

        private void _makeShortMethodname()
        {
            string myStr1 = ""; string myStr2 = ""; string myStr3 = "";

            myStr1 = method_FullPathAndFilename.Substring(0, method_Path.Length);
            if (string.Compare(method_Path, myStr1) > -1)
            {
                myStr2 = method_FullPathAndFilename.Substring(method_Path.Length - 1, method_FullPathAndFilename.Length - method_Path.Length + 1);

                bool _massHunterMode = false;
                if (myStr2.LastIndexOf("\\") > -1)
                    _massHunterMode = true;

                if (!StartedFromMassHunter)
                {
                    // "c:\chromtech\eTrap\test.cme"  -->  "test"
                    myStr3 = myStr2.Substring(1, myStr2.Length - 5);
                }
                else
                {
                    if (_massHunterMode)
                    {
                        // "C:\MASSHUNTER\GCMS\1\METHODS\DEFAULT.M\eTrap.cme"  --> "DEFAULT"
                        myStr3 = myStr2.Substring(1, myStr2.LastIndexOf("\\") - 3);
                    }
                    else
                    {   // in case MH user moved to the Chromtech folder
                        myStr3 = myStr2.Substring(1, myStr2.Length - 4);
                    }

                }
                _method_ShortName = myStr3;
            }

            _DebugAdd("_makeShortMethodname: _method_ShortName=" + _method_ShortName);

        }

        private string _method_ShortName { get; set; }
        public string method_ShortName
        {
            get { return _method_ShortName; }
            set
            {
                _method_ShortName = value;
                NotifyPropertyChanged("method_ShortName");
            }
        }

        private string _method_FullPathAndFilename { get; set; }
        public string method_FullPathAndFilename {
            get { return _method_FullPathAndFilename; }
            set {
                _method_FullPathAndFilename = value;
                _DebugAdd("method_FullPathAndFilename: _method_FullPathAndFilename=" + _method_FullPathAndFilename);

                _makeMethodPath();
                _makeShortMethodname();
                NotifyPropertyChanged("method_FullPathAndFilename");
            }
        }


        private string _method_FullPathAndFilename_LastUsedTrapMethod { get; set; }
        public string method_FullPathAndFilename_LastStandaloneMethod
        {
            get { return _method_FullPathAndFilename_LastUsedTrapMethod; }
            set
            {
                _method_FullPathAndFilename_LastUsedTrapMethod = value;
                NotifyPropertyChanged("");
            }
        }

        private string _log_FileName { get; set; }
        public string log_FileName
        {
            get { return _log_FileName; }
            set
            {
                _log_FileName = value;
                NotifyPropertyChanged("log_FileName");
            }
        }

        private string _Config_FileName { get; set; }
        public string Config_Filename
        {
            get { return _Config_FileName; }
            set
            {
                _Config_FileName = value;
                NotifyPropertyChanged("Config_Filename");
            }
        }

        private string  _ExcludeCOM1{get; set;}
        public string ExcludeCOM1
        {
            get { return _ExcludeCOM1; }
            set
            {
                _ExcludeCOM1 = value;
            }
        }
        private string _ExcludeCOM2 { get; set; }
        public string ExcludeCOM2
        {
            get { return _ExcludeCOM2; }
            set
            {
                _ExcludeCOM2 = value;
            }
        }
        private string _ExcludeCOM3 { get; set; }
        public string ExcludeCOM3
        {
            get { return _ExcludeCOM3; }
            set
            {
                _ExcludeCOM3 = value;
            }
        }
        private string _ExcludeCOM4 { get; set; }
        public string ExcludeCOM4
        {
            get { return _ExcludeCOM4; }
            set
            {
                _ExcludeCOM4 = value;
            }
        }
        private string _ExcludeCOM5 { get; set; }
        public string ExcludeCOM5
        {
            get { return _ExcludeCOM5; }
            set
            {
                _ExcludeCOM5 = value;
            }
        }

        private int _MH_InstNumber { get; set; }
        public int MH_InstNumber
        {
            get { return _MH_InstNumber; }
            set
            {
                _MH_InstNumber = value;
                NotifyPropertyChanged("MH_InstNumber");
            }
        }

        private string _TrapUnUsed_FileName { get; set; }
        public string TrapUnUsed_FileName
        {
            get {return _TrapUnUsed_FileName;}
            set 
            {   _TrapUnUsed_FileName = value;
            }
        }

        /// <summary>
        /// Used to signal external SW that Startup is done
        /// </summary>
        private string _StartupDone_FileName { get; set; }
        public string StartupDone_Filename
        {
            get { return _StartupDone_FileName; }
            set
            {
                _StartupDone_FileName = value;
                NotifyPropertyChanged("StartupDone_Filename");
            }
        }

        private string _HomePath;
        public string HomePath
        {
            get { return _HomePath; }
            set
            {
                _HomePath = value;
                NotifyPropertyChanged("HomePath");
            }
        }

        private string _method_Path { get; set; }
        public string method_Path
        {
            get { return _method_Path; }
            set
            {
                _method_Path = value;
                NotifyPropertyChanged("method_Path");
            }
        }

        private string _FirmwarePath { get; set; }
        public string FirmwarePath
        {
            get { return _FirmwarePath; }
            set
            {
                _FirmwarePath = value;
            }
        }

        private int _method_StandbyTemp { get; set; }
        public int method_StandbyTemp
        {
            get { return _method_StandbyTemp; }
            set
            {
                if (eTrap_CryoMode)
                    _method_StandbyTemp = limit(value, 70, 250);
                else
                    _method_StandbyTemp = limit(value, 70, 320);

                NotifyPropertyChanged("method_StandbyTemp");
            }
        }

        private int _method_LowTemp { get; set; }
        public int method_LowTemp
        {
            get { return _method_LowTemp; }
            set
            {
                _method_LowTemp = limit(value, -40, 40);
                NotifyPropertyChanged("method_LowTemp");
            }
        }

        private int _method_LowTime_PreInject { get; set; }
        public int method_LowTime_PreInject
        {
            get { return _method_LowTime_PreInject; }
            set
            {
                _method_LowTime_PreInject = limit(value, 1, 600);
                NotifyPropertyChanged("method_LowTime_PreInject");
            }
        }

        private int _method_LowTime_PostInject { get; set; }
        public int method_LowTime_PostInject
        {
            get { return _method_LowTime_PostInject; }
            set
            {
                _method_LowTime_PostInject = limit(value,1,1200);       //20 min
                NotifyPropertyChanged("method_LowTime_PostInject");
            }
        }

        private int _method_HighTemp { get; set; }
        public int method_HighTemp
        {
            get { return _method_HighTemp; }
            set
            {
                _method_HighTemp = limit(value, 151, 320);
                NotifyPropertyChanged("method_HighTemp");
            }
        }

        private int _method_HighTemp_Time { get; set; }
        public int method_HighTemp_Time
        {
            get { return _method_HighTemp_Time; }
            set
            {
                _method_HighTemp_Time = limit(value, 10, 7200);     //2 h
                NotifyPropertyChanged("method_HighTemp_Time");
            }
        }

        // 2.7 New Prepare_2_Timer  (instead of PAL_Incubation Time)
        private int _method_Prepare_2_Time { get; set; }
        public int method_Prepare_2_Time
        {
            get { return _method_Prepare_2_Time; }
            set
            {
                if (value == 9999)
                    _method_Prepare_2_Time=9999;    // Flag for unused
                else
                    _method_Prepare_2_Time = limit (value,0,9998);   // used
                
                NotifyPropertyChanged("method_Prepare_2_Time");
            }
        }

        private int _Prepare_2_Time { get; set; }
        public int Prepare_2_Time
        {
            get { return _Prepare_2_Time; }
            set
            {
                _Prepare_2_Time = value;
                NotifyPropertyChanged("Prepare_2_Time");
            }
        }

        // 2.8.1 New InUse
        private bool _eTrap_Used { get; set; }
        public bool eTrap_CryoMode
        {
            get { return _eTrap_Used; }
            set
            {
                _eTrap_Used = value;
                NotifyPropertyChanged("eTrap_CryoMode");
            }
        }


        //   Cycle:  5  T I M E R 
        private double _ctimertime_0{get; set;}
        public double CTimerTime_0
        {   get { return _ctimertime_0;}
            set {
                _ctimertime_0 = value;
            }
        }
        private double _ctimertime_1 { get; set; }
        public double CTimerTime_1
        {
            get { return _ctimertime_1; }
            set
            {
                _ctimertime_1 = value;
            }
        }
        private double _ctimertime_2 { get; set; }
        public double CTimerTime_2
        {
            get { return _ctimertime_2; }
            set
            {
                _ctimertime_2 = value;
            }
        }
        private double _ctimertime_3 { get; set; }
        public double CTimerTime_3
        {
            get { return _ctimertime_3; }
            set
            {
                _ctimertime_3 = value;
            }
        }
        private double _ctimertime_4 { get; set; }
        public double CTimerTime_4
        {
            get { return _ctimertime_4; }
            set
            {
                _ctimertime_4 = value;
            }
        }

        private double _currentTimerGoal { get; set; }
        public double currentTimerGoal
        {
            get { return _currentTimerGoal; }
            set
            {
                _currentTimerGoal = value;
            }
        }

        private string _runningTimerString { get; set; }
        public string RunningTimerString
        {
            get { return _runningTimerString; }
            set
            {
                _runningTimerString = "0";
                if (_ctimertime_0 > 0)
                    _runningTimerString = Convert.ToString(_currentTimerGoal-(Convert.ToInt32(_ctimertime_0 / 1000)));
                if (_ctimertime_1 > 0)
                    _runningTimerString = Convert.ToString(_currentTimerGoal-(Convert.ToInt32(_ctimertime_1 / 1000)));
                if (_ctimertime_2 > 0)
                    _runningTimerString = Convert.ToString(_currentTimerGoal-(Convert.ToInt32(_ctimertime_2 / 1000)));
                if (_ctimertime_3 > 0)
                    _runningTimerString = Convert.ToString(_currentTimerGoal-(Convert.ToInt32(_ctimertime_3 / 1000)));
                if (_ctimertime_4 > 0)
                    _runningTimerString = Convert.ToString(_currentTimerGoal-(Convert.ToInt32(_ctimertime_4 / 1000)));

                if (Convert.ToInt32(_runningTimerString) < 0)
                    _runningTimerString = "0";

                NotifyPropertyChanged("RunningTimerString");
            }
        }

        private string _cTimerString { get; set; }
        public string CTimerString
        {
            get { return _cTimerString; }
            set
            {
                _cTimerString = "Timer 1 = " + Convert.ToString(Convert.ToInt32(_ctimertime_0 / 1000)) + "\r\n"
                                + "Timer 2 = " + Convert.ToString(Convert.ToInt32(_ctimertime_1 / 1000)) + "\r\n"
                                + "Timer 3 = " + Convert.ToString(Convert.ToInt32(_ctimertime_2 / 1000)) + "\r\n"
                                + "Timer 4 = " + Convert.ToString(Convert.ToInt32(_ctimertime_3 / 1000)) + "\r\n"
                                + "Timer 5 = " + Convert.ToString(Convert.ToInt32(_ctimertime_4 / 1000));

                NotifyPropertyChanged("CTimerString");
            }
        }

        // Installed Trap Counter
        private int _TRAP_CYCLES { get; set; }
        public int TRAP_CYCLES
        {
            get { return _TRAP_CYCLES; }
            set
            {
                _TRAP_CYCLES = value;
                NotifyPropertyChanged("TRAP_CYCLES");
            }
        }

        // eTrap Instrument Counter
        private int _INSTRUMENT_CYCLES { get; set; }
        public int INSTRUMENT_CYCLES
        {
            get { return _INSTRUMENT_CYCLES; }
            set
            {
                _INSTRUMENT_CYCLES = value;
                NotifyPropertyChanged("INSTRUMENT_CYCLES");
            }
        }

        private int _SETUP_STANDBY_TEMPERATURE { get; set; }
        public int SETUP_STANDBY_TEMPERATURE
        {
            get { return _SETUP_STANDBY_TEMPERATURE; }
            set
            {
                _SETUP_STANDBY_TEMPERATURE = value;
                NotifyPropertyChanged("SETUP_STANDBY_TEMPERATURE");
            }
        }

        private bool _SINGLE_RUN = true;
        public bool SINGLE_RUN
        {
            get { return _SINGLE_RUN; }
            set
            {
                _SINGLE_RUN = value;
                SINGLE_RUN_string = "";     // kick it
                NotifyPropertyChanged("SINGLE_RUN");
            }
        }

        private string _SINGLE_RUN_string = "Single Run Mode";
        public string SINGLE_RUN_string
        {
            get { return _SINGLE_RUN_string; }
            set
            {
                if (eTrapIsConnected==false)
                {
                    _SINGLE_RUN_string = "Trap NOT CONNECTED";
                }   
                else
                {
                    if (eTrap_CryoMode == false)
                        _SINGLE_RUN_string = "Transfer line mode";
                    else
                    {
                        if (SINGLE_RUN == true)
                            _SINGLE_RUN_string = "Cryo trap mode";
                        else
                            _SINGLE_RUN_string = "Cryo trap mode / Autostart";
                    }
                }

                NotifyPropertyChanged("SINGLE_RUN_string");
            }
        }



        // Do not wait for GC Prepare signal
        private bool _ignore_GC_Prepare { get; set; }
        public bool ignore_GC_Prepare
        {
            get { return _ignore_GC_Prepare; }
            set
            {
                _ignore_GC_Prepare = value;
                NotifyPropertyChanged("ignore_GC_Prepare");
            }
        }

        // Do not wait for GC Prepare 2
        private bool _ignore_Prepare_2 { get; set; }
        public bool ignore_Prepare_2
        {
            get { return _ignore_Prepare_2; }
            set
            {
                _ignore_Prepare_2 = value;
                NotifyPropertyChanged("ignore_GC_Prepare_2");
            }
        }

        //// immediate replacements for Sync Signals = TTLs
        //private bool _fake_GC_Prepare { get; set; }
        //public bool fake_GC_Prepare
        //{
        //    get { return _fake_GC_Prepare; }
        //    set
        //    {
        //        _fake_GC_Prepare = value;
        //        NotifyPropertyChanged("fake_GC_Prepare");
        //    }
        //}

        private bool _fakeSyncSignals { get; set; }
        public bool fakeSyncSignals
        {
            get { return _fakeSyncSignals; }
            set
            {
                _fakeSyncSignals = value;
                NotifyPropertyChanged("fakeSyncSignals");
            }
        }
        private bool _fake_TRAP_Cool { get; set; }
        public bool fake_TRAP_Cool
        {
            get { return _fake_TRAP_Cool; }
            set
            {
                _fake_TRAP_Cool = value;
                NotifyPropertyChanged("fake_TRAP_Cool");
            }
        }


        // Sync Signals = TTL Stuff  from Controller
        
        // V 2.0 Prepare 2 added
        private int _Prepare_2 { get; set; }
        public int Prepare_2
        {
            get { return _Prepare_2; }
            set
            {
                _Prepare_2 = value;
                NotifyPropertyChanged("Prepare_2");
            }
        }

        private int _GC_Prepare { get; set; }
        public int GC_Prepare
        {
            get { return _GC_Prepare; }
            set
            {
                _GC_Prepare = value;
                NotifyPropertyChanged("GC_Prepare");
            }
        }
        private int _GC_Ready { get; set; }
        public int GC_Ready
        {
            get { return _GC_Ready; }
            set
            {
                _GC_Ready = value;
                NotifyPropertyChanged("GC_Ready");
            }
        }
        private int _GC_Start { get; set; }
        public int GC_Start
        {
            get { return _GC_Start; }
            set
            {
                _GC_Start = value;
                NotifyPropertyChanged("GC_Start");
            }
        }
        private int _TRAP_Cool { get; set; }
        public int TRAP_Cool
        {
            get { return _TRAP_Cool; }
            set
            {
                _TRAP_Cool = value;
                NotifyPropertyChanged("TRAP_Cool");
            }
        }
        private string _TTLString { get; set; }
        public string TTLString
        {
            get { return _TTLString; }
            set
            {   _TTLString = "Prepare" + "\r\n"
                           + "Ready" + "\r\n"
                           + "Start" + "\r\n"
                           + "Cool Trap" + "\r\n"
                           + "Prepare 2";
                NotifyPropertyChanged("TTLString");
            }
        }
        private string _TTLStringVal { get; set; }
        public string TTLStringVal
        {
            get { return _TTLStringVal; }
            set
            {
                string p = "low"; string r = "low"; string s = "low"; string c = "low"; string p2 = "low";
                if (_GC_Prepare > 0) p = "High";
                if (_GC_Ready > 0) r = "High";
                if (_GC_Start > 0) s = "High";
                if (_TRAP_Cool > 0) c = "High";
                if (_Prepare_2 > 0) p2 = "High";

                _TTLStringVal = "= " + p + "\r\n"
                           + "= " + r + "\r\n"
                           + "= " + s + "\r\n"
                           + "= " + c + "\r\n"
                           + "= " + p2;
                NotifyPropertyChanged("TTLStringVal");
            }
        }




        enum ATOM   //								 PARM1(0-255)	PARM2				PARM3
        {
            SET_TEMP,			// 2 Parms  Set_Temp			(Temp,			Accuracy[0-10, 99])
            SWITCH_EVENT,		// 3 Parms  Switch_Event		(Event,			Signal State,		Pulse time)	Pulse Time 0= just set
            WAIT_SYNC_SIG,		// 2 Parms	Wait_Sync_Signal	(Signal,		Signal State )
            WAIT_TIMER,			// 2 Parms	Wait Timer			(Timer,			time(sec))
            START_TIMER,
            END_OF_CYCLE,       // 1 Parm   END_OF_CYCLE        (True/False)
            SET_VARIABLE
        };
        string[] _atomString = { "SET_TEMP", "SWITCH_EVENT", "WAIT_SYNC_SIG", "WAIT_TIMER", "START_TIMER", "END_OF_CYCLE", "SET_VARIABLE" };

        enum PARM1
        {
            TRAP_COOL,
            GC_READY,
            GC_PREPARE,
            GC_START,
            PREPARE_2_TIME,
            eTrap,
            SINGLE_RUN_MODE,
            PREPARE_2
        };
        public string[] _parm1String = { "TRAP_COOL", "GC_READY", "GC_PREPARE", "GC_START", "PREPARE_2_TIME", "eTrap", "Single Run Mode", "PREPARE_2" };

        public string convertToTTL(int val)
        {
            if (val == 0) return "Off";
            else return "On";
        }
        public string convertToBOOL(bool val)
        {
            if (val == false) return "False";
            else return "True";
        }

 


        public string Do_eTrapDisplayString(int _cycleLine, int _atom, int _parm1, int _parm2, int _parm3)
        {
            string _eTrapString_kernel = "";
            string _niceStatusString = "";

            switch (_cycleLine)
            {
                case 0: _niceStatusString = "Wait Prepare 2 Time ( ";                               break;
                case 1: _niceStatusString = "Cooling OFF ";                                         break;
                case 2: _niceStatusString = "Wait for Standby Temperature ( ";                      break;
                case 3: _niceStatusString = "Wait for GC Prepare Signal ";                          break;
                case 4:
                    if (method_Prepare_2_Time < 9999)
                        _niceStatusString = "Wait for Prepare_2 Time ";                    
                    else
                        _niceStatusString = "Wait for Prepare_2 Signal ";                    
                    break;
                case 5: _niceStatusString = "Set GC not Ready";                                     break;
                case 6: _niceStatusString = "Wait for secure temperature to turn on Trap ( ";       break;

                case 7: _niceStatusString = "Cooling ON ";                                          break;
                case 8: _niceStatusString = "Wait for cool Trap ( ";                                break;

                case 9: _niceStatusString = "Wait Pre Inject Time ( ";                              break;

                case 10: _niceStatusString = "Set GC Ready Signal";                                 break;
                case 11: _niceStatusString = "Wait for GC Start signal ";                           break;

                case 12: _niceStatusString = "Wait Post Inject Time ( ";                            break;
                case 13: _niceStatusString = "Cooling OFF ";                                        break;
                case 14: _niceStatusString = "Wait for High Temperature ( ";                        break;
                case 15: _niceStatusString = "Start High Temperature Timer ( ";                     break;
                case 16: _niceStatusString = "Wait High Temperature Time ( ";                       break;
                case 17: 
                    if (SINGLE_RUN)
                        _niceStatusString = "End of Single Cycle. Single Run=";                                         
                    else
                        _niceStatusString = "End of Cycle. Start next Cycle. Single Run="; 
                    break;
                default: _niceStatusString = "unknown cycleLine ";                                  break;
            }


            try
            {
                switch (_atom)
                {
                    case (int)ATOM.SET_VARIABLE:
                        _eTrapString_kernel = _eTrapString_kernel
                            + _parm1String[(int)_parm1] 
                            + Convert.ToString(_parm2)
                            +" sec )";
                        break;
                    case (int)ATOM.SET_TEMP:
                        _eTrapString_kernel = _eTrapString_kernel
                            + _niceStatusString
                            + Convert.ToString(_parm1)
                            + "°C )";
                        currentTimerGoal = 0;
                        break;
                    case (int)ATOM.WAIT_TIMER:
                        _eTrapString_kernel = _eTrapString_kernel
                            + _niceStatusString
                            + Convert.ToString(_parm2)
                            + " sec )";
                        currentTimerGoal = _parm2;
                        break;

                    case (int)ATOM.START_TIMER:
                        _eTrapString_kernel = _eTrapString_kernel
                            + _niceStatusString
                            +  Convert.ToString(_parm2)
                            + " sec )";
                        if (_cycleLine==15)
                            currentTimerGoal = method_HighTemp_Time;    // look into method, since time is in cycle line 16 (Wait_TIMER)
                        break;

                    case (int)ATOM.SWITCH_EVENT:
                        _eTrapString_kernel = _eTrapString_kernel
                            + _niceStatusString
                            + convertToTTL((int)_parm2);
                        break;
                    case (int)ATOM.WAIT_SYNC_SIG:
                        _eTrapString_kernel = _eTrapString_kernel
                            + _niceStatusString;
                        // change currentTimerGoal if needed:
                        switch (_cycleLine)
                        {
                            case 1:
                            case 2:
                                //currentTimerGoal = 0;
                                break;
                            case 3:
                                if (fakeSyncSignals)
                                    currentTimerGoal = 10;
                                break;
                            case 4:
                                if (method_Prepare_2_Time < 9999)
                                    currentTimerGoal = method_Prepare_2_Time;
                                else
                                {   if (fakeSyncSignals)
                                        currentTimerGoal = 10;   //seconds
                                }
                                break;
                            case 11:
                                if (fakeSyncSignals)
                                    currentTimerGoal = 10;
                                break;   
                        }
                        break;

                    case (int)ATOM.END_OF_CYCLE:
                        _eTrapString_kernel = _eTrapString_kernel
                            + _niceStatusString
                            + convertToBOOL(SINGLE_RUN);
                        currentTimerGoal = 0;
                        break;

                    default:
                        _eTrapString_kernel = "error";
                        break;
                };

            }
            catch
            {
                _eTrapString_kernel = "Err: Atom=" + _atom + "parm1=" + _parm1 + "parm2=" + _parm2 + "parm3=" + _parm3;
            }

            return _eTrapString_kernel; ;
        }


        public string make_cycleLineString(int _atom, int _parm1, int _parm2, int _parm3)
        {
            string _CycleLineString_kernel = "";

            try
            {
                switch (_atom)
                {
                    case (int)ATOM.SET_VARIABLE:
                        _CycleLineString_kernel = _CycleLineString_kernel
                            + _atomString[(int)ATOM.SET_VARIABLE]
                            + "(" + _parm1String[(int)_parm1]
                            + "," + Convert.ToString(_parm2)
                            + ")";
                        break;
                    case (int)ATOM.SET_TEMP:
                        _CycleLineString_kernel = _CycleLineString_kernel
                            + _atomString[(int)ATOM.SET_TEMP]
                            + "(" + Convert.ToString(_parm1)
                            + "," + Convert.ToString(_parm2)
                            + ")";
                        break;
                    case (int)ATOM.WAIT_TIMER:
                        _CycleLineString_kernel = _CycleLineString_kernel
                            + _atomString[(int)ATOM.WAIT_TIMER]
                            + "(" + Convert.ToString(_parm1 + 1)
                            + "," + Convert.ToString(_parm2)
                            + ")"; ;
                        break;

                    case (int)ATOM.START_TIMER:
                        _CycleLineString_kernel = _CycleLineString_kernel
                            + _atomString[(int)ATOM.START_TIMER]
                            + "(" + Convert.ToString(_parm1 + 1)
                            + "," + Convert.ToString(_parm2)
                            + ")"; ;
                        break;

                    case (int)ATOM.SWITCH_EVENT:
                        _CycleLineString_kernel = _CycleLineString_kernel
                            + _atomString[(int)ATOM.SWITCH_EVENT]
                                         + "(" + _parm1String[(int)_parm1]
                                         + "," + convertToTTL((int)_parm2)
                                         + "," + Convert.ToString(_parm3)
                                         + ")";
                        break;
                    case (int)ATOM.WAIT_SYNC_SIG:
                        _CycleLineString_kernel = _CycleLineString_kernel
                            + _atomString[(int)ATOM.WAIT_SYNC_SIG]
                                         + "(" + _parm1String[(int)_parm1]
                                         + "," + convertToTTL((int)_parm2)
                                         + ")";
                        break;
                    case (int)ATOM.END_OF_CYCLE:
                        _CycleLineString_kernel = _CycleLineString_kernel
                            + _atomString[(int)ATOM.END_OF_CYCLE]
                            + "(SINGLE_RUN=" +  convertToBOOL((_parm1==1? true:false))
                                         + ")";
                        break;

                    default:
                        _CycleLineString_kernel = "error";
                        break;
                };

            }
            catch
            {
                _CycleLineString_kernel = "Err: Atom=" + _atom + "parm1=" + _parm1 + "parm2=" + _parm2 + "parm3=" +_parm3;
                }

            return _CycleLineString_kernel; ;
        }

        private string _runningCycleLineString { get; set; }
        public string runningCycleLineString
        {
            get { return _runningCycleLineString; }
            set
            {
                if (_CycleRunning == 0)
                    _runningCycleLineString = " Cycle not running";
                else
                {
                    if (_FirstLoggingDone == true)
                    {
                        _runningCycleLineString = Convert.ToString(_CycleLine) + ": ";
                        _runningCycleLineString = _runningCycleLineString + make_cycleLineString(_Atom, _Parm1, _Parm2, _Parm3);
                    }
                }
                NotifyPropertyChanged("CycleLineString");
            }
        }

        private bool _CycleStepMode { get; set; }
        public bool CycleStepMode
        {
            get { return _CycleStepMode; }
            set
            {
                _CycleStepMode = value;
            }
        }

        private bool _CycleNextStepPlease { get; set; }
        public bool CycleNextStepPlease
        {
            get { return _CycleNextStepPlease; }
            set
            {
                _CycleNextStepPlease = value;
            }
        }

        private bool _IgnoreCryoTimeout { get; set; }
        public bool IgnoreCryoTimeout
        {
            get { return _IgnoreCryoTimeout; }
            set
            {
                _IgnoreCryoTimeout = value;
            }
        }

        //-----------------------------------------
        // Hardware version 3.0:
        // HW	2 = standard 3.0    Ralf Controler	+	CT eTrap V1
        // HW   3 = V3				CT controler	+	CT eTrap V1
        private int _HWVersion { get; set; }
        public int HWVersion
        {
            get { return _HWVersion; }
            set
            {
                _HWVersion= value;
                NotifyPropertyChanged("hwversion");
            }
        }

        private int _FuseTemp { get; set; }
        public int FuseTemp
        {
            get { return _FuseTemp; }
            set
            {
                _FuseTemp = value;
                NotifyPropertyChanged("fusetemp");
            }
        }
        // ----------------------------------------


        private int _CryoTimeout { get; set; }
        public int CryoTimeout
        {
            get { return _CryoTimeout; }
            set
            {
                _CryoTimeout = value;
            }
        }

        private string _CryoTimeoutString { get; set; }
        public string CryoTimeoutString
        {
            get { return _CryoTimeoutString; }
            set
            {
                if (CryoTimeout > 0)
                    _CryoTimeoutString = "Cryo Timeout - Load Method to Restart";
                else
                    _CryoTimeoutString = "";

                NotifyPropertyChanged("CryoTimeoutString");
            }
        }

        private int _CycleRunning { get; set; }
        public int CycleRunning
        {
            get { return _CycleRunning; }
            set
            {
                _CycleRunning = value;
            }
        }
        private int _CycleRunningLastState { get; set; }
        public int CycleRunningLastState
        {
            get { return _CycleRunningLastState; }
            set
            {
                _CycleRunningLastState = value;
            }
        }

        private int _CycleLine { get; set; }
        public int CycleLine
        {   get { return _CycleLine; }
            set { _CycleLine = value;}
        }

        private int _LastCycleLine { get; set; }
        public int LastCycleLine
        {
            get { return _LastCycleLine; }
            set { _LastCycleLine = value; }
        }

        private int _Atom { get; set; }
        public int Atom
        {
            get { return _Atom; }
            set
            {
                _Atom = value;
            }
        }
        private int _Parm1 { get; set; }
        public int Parm1
        {
            get { return _Parm1; }
            set
            {
                _Parm1 = value;
            }
        }
        private int _Parm2 { get; set; }
        public int Parm2
        {
            get { return _Parm2; }
            set
            {
                _Parm2 = value;
            }
        }
        private int _Parm3 { get; set; }
        public int Parm3
        {
            get { return _Parm3; }
            set
            {
                _Parm3 = value;
            }
        }


        private int _heaterIsOn { get; set; }
        public int heaterIsOn
        {
            get { return _heaterIsOn; }
            set
            {
                _heaterIsOn = value;
                NotifyPropertyChanged("heaterIsOn");
            }
        }

        private float _error { get; set; }
        public float error
        {
            get { return _error; }
            set
            {
                _error = value;
            }
        }
        private float _p_error { get; set; }
        public float p_error
        {
            get { return _p_error; }
            set
            {
                _p_error = value;
            }
        }
        private float _i_error { get; set; }
        public float i_error
        {
            get { return _i_error; }
            set
            {
                _i_error = value;
            }
        }
        private float _correction_sum { get; set; }
        public float correction_sum
        {
            get { return _correction_sum; }
            set
            {
                _correction_sum = value;
            }
        }
        private float _pid_integral_sum { get; set; }
        public float pid_integral_sum
        {
            get { return _pid_integral_sum; }
            set
            {
                _pid_integral_sum = value;
                NotifyPropertyChanged("pid_integral_sum");
            }
        }
        private float _pid_duty_cycle { get; set; }
        public float pid_duty_cycle
        {
            get { return _pid_duty_cycle; }
            set
            {
                _pid_duty_cycle = value;
                NotifyPropertyChanged("pid_duty_cycle");
            }
        }


        private float _pid_max_temperature_cool { get; set; }
        public float pid_max_temperature_cool
        {
            get { return _pid_max_temperature_cool; }
            set
            {
                _pid_max_temperature_cool = value;
                NotifyPropertyChanged("pid_max_temperature_cool");
            }
        }
        private float _pid_min_temperature_cool { get; set; }
        public float pid_min_temperature_cool
        {
            get { return _pid_min_temperature_cool; }
            set
            {
                _pid_min_temperature_cool = value;
                NotifyPropertyChanged("pid_min_temperature_cool");
            }
        }

        private float _pid_max_temperature { get; set; }
        public float pid_max_temperature
        {
            get { return _pid_max_temperature; }
            set
            {
                _pid_max_temperature = value;
                NotifyPropertyChanged("pid_max_temperature");
            }
        }
        private float _pid_min_temperature { get; set; }
        public float pid_min_temperature
        {
            get { return _pid_min_temperature; }
            set
            {
                _pid_min_temperature = value;
                NotifyPropertyChanged("pid_min_temperature");
            }
        }
        private float _pid_integral { get; set; }
        public float pid_integral
        {
            get { return _pid_integral; }
            set
            {
                _pid_integral = value;
                NotifyPropertyChanged("pid_integral");
            }
        }
        private float _pid_portional { get; set; }
        public float pid_portional
        {
            get { return _pid_portional; }
            set
            {
                _pid_portional = value;
                NotifyPropertyChanged("pid_portional");
            }
        }
        private float _currentGoalTemp { get; set; }
        public float currentGoalTemp
        {
            get { return _currentGoalTemp; }
            set
            { _currentGoalTemp = (float) limit_dbl(value,_pid_min_temperature,_pid_max_temperature);
            NotifyPropertyChanged("currentGoalTemp");
            }
        }

        private string _TemperatureString { get; set; }
        public string TemperatureString
        {
            get { return _TemperatureString; }
            set
            {
                string str = "";
                if ( (ControlerPower < 10) || (ControlerPower>12) || (currentTemp>360) )
                    str = "--";
                else
                    str = Convert.ToString(currentTemp);

                _TemperatureString = "current temperature= " + str + "\r\n"
                                   + "set       temperature= " + Convert.ToString(currentGoalTemp) + "\r\n";


            NotifyPropertyChanged("TemperatureString");
            }
        }

        private string _pid_string { get; set; }
        public string pid_string
        {
            get { return _pid_string; }
            set
            {
                if (_HWVersion <=2)
                {
                    if (_heaterIsOn == 1) _pid_string = "Heater ON";
                    else _pid_string = "Heater off";

                    _pid_string = _pid_string
                                + "    " + _pid_duty_cycle.ToString("####0.0") + "%" + "\r\n"
                                + "  PID Parameters:" + "\r\n"
                                + "  Temperature  min =" + _pid_min_temperature.ToString("##0")
                                + "   max =" + _pid_max_temperature.ToString("##0") + "\r\n"
                                + "  Hardware version =" + _HWVersion.ToString("0") + "\r\n"
                                + "  (Temperature Fuse=" + _FuseTemp.ToString("0") + ")"
                                + "  integral sum =" + _pid_integral_sum.ToString("##000.0") + "\r\n"
                                + "  duty cycle =" + _pid_duty_cycle.ToString("##000.0")
                                + "  _slp =" + _error.ToString("####0.00") + "\r\n"
                                + "  _hu_slp =" + _p_error.ToString("####0.00")
                                + "  _r_tmp =" + _i_error.ToString("####0.0") + "\r\n"
                                + "  _correction_sum =" + _correction_sum.ToString("#0000.0")
                                ;
                }
                else
                {
                    if (_heaterIsOn == 1) _pid_string = "Heater ON";
                    else _pid_string = "Heater off";

                    _pid_string = _pid_string
                                + "    " + _pid_duty_cycle.ToString("####0.0") + "%" + "\r\n"
                                + "  PID Parameters:" + "\r\n"
                                + "  Temperature  min =" + _pid_min_temperature.ToString("##0")
                                + "   max =" + _pid_max_temperature.ToString("##0") + "\r\n"
                                + "  Hardware version =" + _HWVersion.ToString("0") + "\r\n"
                                + "  Temperature Fuse=" + _FuseTemp.ToString("0")
                                + "  integral sum =" + _pid_integral_sum.ToString("##000.0") + "\r\n"
                                + "  duty cycle =" + _pid_duty_cycle.ToString("##000.0")
                                + "  _slp =" + _error.ToString("####0.00") + "\r\n"
                                + "  _hu_slp =" + _p_error.ToString("####0.00")
                                + "  _r_tmp =" + _i_error.ToString("####0.0") + "\r\n"
                                + "  _correction_sum =" + _correction_sum.ToString("#0000.0")
                                ;
                }
                NotifyPropertyChanged("pid_string");
            }
        }


        private string _currentTempForDisplay { get; set; }
        public string currentTempForDisplay
        {
            get { return _currentTempForDisplay; }
            set
            {
                _currentTempForDisplay = value;
                NotifyPropertyChanged("currentTempForDisplay");
            }
        }

        private float _currentTemp { get; set; }
        public float currentTemp
        {
            get { return _currentTemp; }
            set
            {
                _currentTemp =  (float)((int)(value*10))/10;
                if (_currentTemp > 450)
                    _currentTempForDisplay = "Failed";
                else
                    _currentTempForDisplay = Convert.ToString((int)_currentTemp);

                NotifyPropertyChanged("currentTemp");
            }
        }

        private float _tempLowCorrectionSlope { get; set; }
        public float tempLowCorrectionSlope                   // range: 0-10
        {
            get { return _tempLowCorrectionSlope; }
            set
            {
                _tempLowCorrectionSlope = (Single)limit_dbl(value, 0.1, 10);

                NotifyPropertyChanged("tempLowCorrectionSlope");
            }
        }

        private float _tempHiCorrectionSlope { get; set; }
        public float tempHiCorrectionSlope                   // range: 0-10
        {
            get { return _tempHiCorrectionSlope; }
            set
            {
                _tempHiCorrectionSlope = (Single)limit_dbl(value, 0.1, 10);

                NotifyPropertyChanged("tempHiCorrectionSlope");
            }
        }
        private float _tempMidCorrectionSlope { get; set; }
        public float tempMidCorrectionSlope
        {
            get { return _tempMidCorrectionSlope; }
            set
            {
                _tempMidCorrectionSlope = (float)limit_dbl((double)value, 0.1, 10);

                NotifyPropertyChanged("tempMidCorrectionSlope");
            }
        }

        private int _Comport { get; set; }
        public int Comport
        {
            get { return _Comport; }
            set
            { _Comport = value;
            NotifyPropertyChanged("Comport");
                ComportString = ""; ;
            }
        }
        private string _ComportString { get; set; }
        public string ComportString
        {
            get { return _ComportString; }
            set
            {
                _ComportString = Convert.ToString( _Comport);
                NotifyPropertyChanged("ComportString");
            }
        }

        private bool _FirstLoggingDone { get; set; }
        public bool FirstLoggingDone
        {
            get { return _FirstLoggingDone; }
            set
            {
                _FirstLoggingDone = value;
            }
        }

        private bool _Logging { get; set; }
        public bool Logging
        {
            get { return _Logging; }
            set
            {   _Logging = value;
            NotifyPropertyChanged("Logging");
            }
        }

        private string _Timer1_TimeString { get; set; }
        public string Timer1_TimeString
        {   get {return _Timer1_TimeString;}
            set { _Timer1_TimeString =  _Timer1_hours.ToString("00")
                                    + ":" + _Timer1_minutes.ToString("00")
                                    + ":" + _Timer1_seconds.ToString("00");
            NotifyPropertyChanged("Timer1_TimeString");
            }
        }

        private int _Timer1_On { get; set; }
        public int Timer1_On
        {
            get { return _Timer1_On; }
            set
            {
                _Timer1_On = value;
                NotifyPropertyChanged("Timer1_On");
            }
        }

        private int _Timer1_seconds { get; set; }
        public int Timer1_seconds
        {
            get { return _Timer1_seconds; }
            set
            {   _Timer1_seconds = value;
            }
        }

        private int _Timer1_minutes { get; set; }
        public int Timer1_minutes
        {   get { return _Timer1_minutes; }
            set {   _Timer1_minutes = value;}
        }

        private int _Timer1_hours { get; set; }
        public int Timer1_hours
        {   get { return _Timer1_hours; }
            set {_Timer1_hours = value;}
        }



        public const string CRLF = "\r\n";
        public const string TAB = "\t";


        public void ConfigFileSave()     // Save PAL1 Configuration.txt to c:\
        {
            bool usePrepare_2_Timer;
            if (method_Prepare_2_Time == 9999)
                usePrepare_2_Timer = false;
            else
                usePrepare_2_Timer = true;


            FileStream myStream = new FileStream(Config_Filename, FileMode.Create);
            StreamWriter sw = new StreamWriter(myStream);

            sw.WriteLine("Configuration file=" + _Config_FileName);
            sw.WriteLine("Home path=" + _HomePath);
            sw.WriteLine("Method path=" + _method_Path );
            sw.WriteLine("Last method=" + _method_FullPathAndFilename);
            sw.WriteLine("Last standalone method=" + _method_FullPathAndFilename_LastUsedTrapMethod);
            sw.WriteLine("Started from MassHunter=" + StartedFromMassHunter.ToString());

            sw.WriteLine("Do not wait for GC Prepare signal=" + ignore_GC_Prepare.ToString());
            sw.WriteLine("Do not wait for Prepare_2 signal=" + ignore_Prepare_2.ToString());
            sw.WriteLine("Use Prepare_2 Timer=" + usePrepare_2_Timer.ToString());
            sw.WriteLine("Ignore Cryo Timeout=" + IgnoreCryoTimeout.ToString());
            sw.WriteLine("Single Run Mode=" + SINGLE_RUN.ToString());

            sw.WriteLine("Total Cycles of Instrument=" + INSTRUMENT_CYCLES.ToString());
            sw.WriteLine("Cycles of installed trap capillary=" + TRAP_CYCLES.ToString());

            sw.WriteLine("Exclude1=" + ExcludeCOM1);
            sw.WriteLine("Exclude2=" + ExcludeCOM2);
            sw.WriteLine("Exclude3=" + ExcludeCOM3);
            sw.WriteLine("Exclude4=" + ExcludeCOM4);
            sw.WriteLine("Exclude5=" + ExcludeCOM5);
            sw.Close();
        }

        public bool ConfigFileRestore()     // Load PAL1 Configuration.txt from  c:\Chromtech\eTrap  = HomePath
        {
            bool foundPrepare2Timer = false;
            string leftString = "";
            string rightString = "";

            string oneLine = "";
            string _initialConfigFile = "";

            if (!File.Exists(Config_Filename))
            {
                if (!IsBrukerDesign)
                    _initialConfigFile = HomePath + "\\program\\eTrap Configuration.txt";
                else
                    _initialConfigFile = HomePath + "\\program\\PeakTrap Configuration.txt";

                if (File.Exists(_initialConfigFile) == true)
                {   File.Copy(_initialConfigFile, Config_Filename, true);
                    // if no Config, then No method. Copy:
                    method_FullPathAndFilename = HomePath + "test.cme";
                    File.Copy(HomePath + "\\program\\test.cme", method_FullPathAndFilename,true);
                }
            }

            if (!File.Exists(Config_Filename))
            {   return false;
            }
            else
            {
                StreamReader sr = new StreamReader(Config_Filename);
                while (sr.Peek() != -1)
                {
                    oneLine = sr.ReadLine();
                    if (oneLine.IndexOf("=") > 0)
                    {
                        leftString = oneLine.Substring(0, oneLine.IndexOf("="));
                        rightString = oneLine.Substring(oneLine.IndexOf("=") + 1, (oneLine.Length) - (oneLine.IndexOf("=") + 1));
                        try
                        {
                            switch (leftString)
                            {
                                case "Configuration file":      Config_Filename = rightString; break;
                                case "Home path":               HomePath = rightString; break;
                                case "Method path":             method_Path = rightString; break;
                                case "Last method":             method_FullPathAndFilename = rightString; break;
                                case "Last standalone method":  method_FullPathAndFilename_LastStandaloneMethod = rightString; break;
                                case "Started from MassHunter": StartedFromMassHunter_LastTime = Convert.ToBoolean(rightString);  break;

                                case "Do not wait for GC Prepare signal":   ignore_GC_Prepare = Convert.ToBoolean(rightString); break;
                                case "Do not wait for Prepare_2 signal":    ignore_Prepare_2 = Convert.ToBoolean(rightString); break;
                                case "Use Prepare_2 Timer":                 method_Prepare_2_Time = Convert.ToInt32(rightString); foundPrepare2Timer = true; break;
                                case "Ignore Cryo Timeout":                 IgnoreCryoTimeout = Convert.ToBoolean(rightString); break;
                                case "Single Run Mode":                     SINGLE_RUN = Convert.ToBoolean(rightString); break;

                                case "Total Cycles of Instrument": INSTRUMENT_CYCLES = Convert.ToInt32(rightString);break;
                                case "Cycles of installed trap capillary":      TRAP_CYCLES = Convert.ToInt32(rightString); break;

                                case "Exclude1": ExcludeCOM1 = rightString; break;
                                case "Exclude2": ExcludeCOM2 = rightString; break;
                                case "Exclude3": ExcludeCOM3 = rightString; break;
                                case "Exclude4": ExcludeCOM4 = rightString; break;
                                case "Exclude5": ExcludeCOM5 = rightString; break;

                                default:
                                    break;
                            }
                        }
                        catch (System.FormatException)
                        {
                           // MessageBox.Show("'" + leftString + "=" + rightString + "': String to Double failed (wrong string format)", "PAL1.ConfigFileRestore");

                        }
                    }
                }
                sr.Close();

                if (!foundPrepare2Timer)
                    method_Prepare_2_Time = 9999;   // disable Prepare_2 Timer
                if (INSTRUMENT_CYCLES == -1)
                    INSTRUMENT_CYCLES = 0;
                if (TRAP_CYCLES == -1)
                    TRAP_CYCLES = 0;

                return true;
            }
        }

        /// <summary>
                /// Write Content of TextBox into file and load it in Editor
        /// </summary>
        /// <param name="_textbox"></param>
        /// <param name="_nameOfLogfile"></param>
        /// <returns>empty String=ok, Filled String contains error</returns>
        public string OpenTextBoxInEditor(TextBox _textbox, string _filename)
        {
            try
            {
                File.WriteAllText(_filename, _textbox.Text);
                var process = new Process();
                process.StartInfo = new ProcessStartInfo()
                {   UseShellExecute = true,
                    FileName = _filename
                };
                process.Start();

                return "";
            }
            catch (Exception ex)
            {
                return "OpenTextBoxInEditor: " + ex.Message;
            }

        }
        

        // Methoden Textfile für User schreiben und in Editor anzeigen
        public string OpenMethodInEditor()
        {
            try
            {
                DateTime _createTime = File.GetCreationTime(method_FullPathAndFilename);
                DateTime _lastWriteTime = File.GetLastWriteTime(method_FullPathAndFilename);

                FileStream myStream = new FileStream((method_FullPathAndFilename + ".txt"), FileMode.Create);
                StreamWriter sw = new StreamWriter(myStream);

                sw.WriteLine(CRLF + "\t\t\t\t\tDate: " + DateTime.Now);
                sw.WriteLine(CRLF + CRLF + "\tMethod:\t\t\"" + method_ShortName + "\"");

                sw.WriteLine(CRLF
                           + "\t         File: \t" + method_FullPathAndFilename);
                sw.WriteLine("\t      created : " + _createTime
                      + CRLF + "\tlast modified : " + _lastWriteTime);

                sw.WriteLine(CRLF + CRLF + CRLF + "\teTrap Cycle:" + CRLF);
                sw.WriteLine("\tStandby Temperature:\t\t\t" + method_StandbyTemp + "°C");
                sw.WriteLine("\tCool Temperature:\t\t\t" + method_LowTemp + "°C");
                sw.WriteLine("\tCool Temperature Pre Inject Time:\t" + method_LowTime_PreInject + "s");
                sw.WriteLine("\tCool Temperature Post Inject Time:\t" + method_LowTime_PostInject + "s");
                sw.WriteLine("\tHigh Temperature:\t\t\t" + method_HighTemp + "°C");
                sw.WriteLine("\tHigh Temperature Time:\t\t\t" + method_HighTemp_Time + "s");

                sw.WriteLine(CRLF + CRLF + CRLF + "\teTrap Parameters:" + CRLF);
                sw.WriteLine("\tPrepare 2 Time:\t" + method_Prepare_2_Time + "s");

                sw.Close();


                var fileToOpen = method_FullPathAndFilename + ".txt";
                var process = new Process();
                process.StartInfo = new ProcessStartInfo()
                {
                    UseShellExecute = true,
                    FileName = fileToOpen
                };

                process.Start();

                return "";
            }
            catch (Exception ex)
            {
                
                return "Err in OpenMethodInEditor: " + ex.Message;
            }
        }


        #region ---- INotifyPropertyChanged Implementierung ----
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
        #endregion
    }
}
