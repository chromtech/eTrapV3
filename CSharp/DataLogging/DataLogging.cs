
// *** DataLogging  ***

// This example expands the previous SendandReceiveArguments example. The PC will now send a start command to the Arduino,
// and wait for a response from the Arduino. The Arduino will start sending analog data which the PC will plot in a chart
// This example shows how to :
// - use in combination with WinForms
// - use in combination with ZedGraph

using System;
using System.IO;
using System.Drawing;
using CommandMessenger;
using CommandMessenger.TransportLayer;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.Specialized;


namespace DataLogging
{

    enum Command
    {
        Acknowledge,
        Error,
        //Switch_CycleStepMode,
        //CycleNextStepPlease,

        GetFWAndSavedParms,
        GetFWAndSavedParmsResult,
        //CycleToEEPROM,
        ErrorWithArgs,

        AskControlerCycle,
        SendCycleToPC,

        AskForReadbacks,
        SendReadbacksToPC,

        ParmsToControler,
        ParmsReceived,

    };
    /// <summary>
    /// Communication to eTrap, Set eTrap states from User or from Request mode
    /// </summary>
    public class DataLogging : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        // Klasse aus DatenKlasse laden
        private DatenKlasse Vars;
        private COM_and_Request COM;

        // This class (kind of) contains presentation logic, and domain model.
        // ChartForm.cs contains the view components 
        private SerialTransport _serialTransport;
        private CmdMessenger _cmdMessenger;
        private ChartForm _form;

        private long _previousTemperatureUpdate;

        public bool NoReadbacksPlease = false;

        // Macro arrays
        const int macMaxLines = 25;
        //string[] macParmName  = new string[macMaxLines];
        //int[] macParmLowLimit = new int[macMaxLines];
        //int[] macParmHighLimit = new int[macMaxLines];
        int numMacroLines = 0;

        int[] macLineNo = new int[macMaxLines];
        int[] macVarNo = new int[macMaxLines];     // 0: No Var, >0: VariableName
        int[] macAtom = new int[macMaxLines];
        int[] macParm1 = new int[macMaxLines];
        int[] macParm2 = new int[macMaxLines];
        int[] macParm3 = new int[macMaxLines];

        string[] VariableName = new string[macMaxLines];
        int[] VariableValue = new int[macMaxLines];
        int[] VariableDefault = new int[macMaxLines];
        int[] VariableHighLimit = new int[macMaxLines];
        int[] VariableLowLimit = new int[macMaxLines];
        int numVariables = 0;

        int currentVariable = 0;
        int currentMacroLine = 0;


        #region  Atoms and Parameters
        enum ATOM   //								 PARM1(0-255)	PARM2				PARM3
        {
            SET_TEMP,			// 2 Parms  Set_Temp			(Temp,			Accuracy[0-10, 99])
            SWITCH_EVENT,		// 3 Parms  Switch_Event		(Event,			Signal State,		Pulse time)	Pulse Time 0= just set
            WAIT_SYNC_SIG,		// 2 Parms	Wait_Sync_Signal	(Signal,		Signal State )
            WAIT_TIMER,			// 2 Parms	Wait Timer			(Timer,			time(sec))
            START_TIMER,
            END_OF_CYCLE,       // must be the LAST Line in any Cycle
            SET_VARIABLE,
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
            PREPARE_2,


        };
        string[] _parm1String = { "TRAP_COOL", "GC_READY", "GC_PREPARE", "GC_START", "Prepare 2 Time (s)", "eTrap", "Single Run Mode", "PREPARE_2" };

        int getAtomToken(string _string)
        {
            int i = 0;
            while (i <= _atomString.Length)
            {
                if (_atomString[i].Equals(_string))
                    return i;       // return Token number
                i++;
            }
            return -1;              // Error
        }

        int getParm1Token(string _string)
        {
            int i = 0;
            while (i < _parm1String.Length)
            {
                if (_parm1String[i].Equals(_string))
                    return i;       // return Token number
                i++;
            }
            return -1;              // Error
        }

        int getVariableNumber(string _string)
        {
            int i = 1;
            while (i < VariableName.Length)
            {
                if (VariableName[i].Equals(_string))
                    return i;       // return Token number
                i++;
            }
            return -1;              // Error
        }

        #endregion


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
            if (DEBUG) { DebugString.Add("DataLog: " + s); }
        }

        private void sleep(Int32 millis)
        {
            System.Threading.Thread.Sleep(millis);
        }

        private void Do_SetCriticalVars()
        {
            if (Vars.IsBrukerDesign == false)
            {
                Vars.HomePath = "C:\\Chromtech\\eTrap\\";
                Vars.Config_Filename = Vars.HomePath + "eTrap Configuration.txt";
                Vars.macro_Name = "eTRAP";
            }
            else
            {
                Vars.HomePath = "C:\\Bruker PeakTrap\\";
                Vars.Config_Filename = Vars.HomePath + "PeakTrap Configuration.txt";
                Vars.macro_Name = "peakTrap";
            }
        }

        /// ------------------ MAIN  ----------------------
        // Setup function
        public void Do_Startup(ChartForm chartForm, DatenKlasse _vars, COM_and_Request _com)
        {
            COM = _com;

            Vars = _vars;                       // Vars mit der übergebenen Datenklasse belegen.

            // Preset some Vars:
            Vars.IsBrukerDesign = false;
            Do_SetCriticalVars();

            Vars.FirmwarePath = Vars.HomePath + "\\Firmware and Driver\\";
            Vars.SoftwareVersionString = "";    // kick it
            Vars.Logging = true;
            Vars.FirstLoggingDone = false;
            Vars.CycleRunning = 1;
            Vars.INSTRUMENT_CYCLES = -1;
            Vars.TRAP_CYCLES = -1;
            Vars.fakeSyncSignals = false;
            Vars.ignore_Prepare_2 = true;

            Vars.eTrap_CryoMode = true;     //2.8.1
            Vars.method_HighTemp = 0;
            Vars.method_HighTemp_Time = 0;
            Vars.method_LowTemp = 0;
            Vars.method_LowTime_PostInject = 0;
            Vars.method_LowTime_PreInject = 0;
            Vars.method_StandbyTemp = 0;

            Vars.pid_max_temperature_cool = 40;
            Vars.pid_min_temperature_cool = -70;

            Vars.IgnoreCryoTimeout = false;

            // getting the chart control on top of the chart form.
            _form = chartForm;

            // Set up chart
            _form.SetupChart();

            // load the Configuration file
            Vars.ConfigFileRestore();

            Do_StartupSerialPort();
            if (!Vars.eTrapIsConnected)     // 2nd time NEEDED in case the PC just restarted !
            {
                Do_StartupSerialPort();
              }

           // System.Windows.Forms.MessageBox.Show("DataLogging/ Vars.eTrapIsConnected Passed");
            // If Firmware and Software version are NOT 'identical': Update Firmware
            if (Vars.eTrapIsConnected && Do_GetAndCheckFWAndParms() == false )
            {
                Do_ExitSerialCOM(false);
                Do_UpdateFirmware(Vars.FirmwarePath, Vars.FirmwareFileName_FromVersionNo);            // update and restart Arduino via avrdude commandline tool
                Do_StartupSerialPort();
                if (!Vars.eTrapIsConnected)     // 2nd time NEEDED in case the PC just restarted !
                    Do_StartupSerialPort();
            }



            // Tell CmdMessenger to "Invoke" commands on the thread running the WinForms UI
            // maybe NULL, if no COM port found !!!
            try {
                _cmdMessenger.SetControlToInvokeOn(chartForm);
            }
                catch (Exception e)
            {
            }

            _previousTemperatureUpdate = TimeUtils.Millis;

            Do_GetControlerMethodParms();            
       
            Vars.method_Path = Vars.HomePath;
            Vars.method_ShortName = "test";
            Vars.method_FullPathAndFilename = Vars.HomePath + Vars.method_ShortName + ".cme";
            Vars.log_FileName = Vars.HomePath + "Logfile.txt";
            Vars.TrapUnUsed_FileName = Vars.HomePath + "UnUsed.txt";

            // Check if MassHunter COM File is present and change over to MH mode, if needed
            COM.Do_CheckAndLoadMassHunter(Vars.HomePath);
            Vars.StartedFromMassHunter = COM.StartedFromMassHunter;
            if (Vars.StartedFromMassHunter)
            {
                Vars.method_FullPathAndFilename = COM.MHMethPath + COM.MHMethFile + "\\eTrap.cme";
            }
            else
            {
                Vars.method_FullPathAndFilename = Vars.method_FullPathAndFilename_LastStandaloneMethod;
            }

            //checkTrapInUse(); // 2.8.1

            Do_LoadAndApplyMethod(Vars.method_FullPathAndFilename);
            Do_ListControlerCycle();
        }

        private bool updateFastFirmware(string fwPath, string fwFilename)
        {
            // write batch:
            bool _batch_ok = false;
            string _batch = fwPath + "FWUpdate.bat";
            int _milliseconds_timeout = 20000;   //20 sec

            if (File.Exists(fwPath + fwFilename))
            {
                try
                {
                    FileStream myStream = new FileStream(_batch, FileMode.Create);
                    StreamWriter sw = new StreamWriter(myStream);

                    sw.WriteLine("cd " + fwPath);
                    sw.WriteLine(@"avrdude.exe -p m328p -carduino -P \\.\COM" + Vars.ComportString + " -b115200 -D -Uflash:w:\"" + fwPath + fwFilename + "\":i - v > \"" + fwPath + "log.txt\" 2>&1 ");
                    sw.WriteLine("timeout 20 sec");
                    sw.WriteLine("exit");
                    sw.Close();

                    Splash.SetStatus(" Update Firmware: Created batch file.");
                    sleep(100);
                    _batch_ok = true;
                }
                catch (Exception e)
                {
                    ErrorString = "dataLogging:DoStartup(), Fast Firmware update: Failed to create batch file" + e.Message;
                    Splash.SetStatus(" Update Firmware: Create batch file failed");
                    sleep(100);
                    _DebugAdd(ErrorString);
                    return false;
                }
            }
            else
            {
                Splash.SetStatus(" Update Firmware: File does not exist (" +fwPath + fwFilename + ")");
                sleep(100);
                return false;
            }

            // Start the Firmware loader
            if (_batch_ok)
            {
                Splash.SetStatus(" Update Firmware: Starting with " + (_milliseconds_timeout/1000).ToString() + " sec timeout");
                sleep(100);
                System.Diagnostics.Process pProcess = new System.Diagnostics.Process();
                pProcess.StartInfo.FileName = _batch;
                pProcess.StartInfo.UseShellExecute = false;
                pProcess.StartInfo.EnvironmentVariables["PATH"] = fwPath;
                pProcess.StartInfo.RedirectStandardOutput = true;
                pProcess.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                pProcess.StartInfo.CreateNoWindow = true; //diplay a window
                pProcess.Start();
                pProcess.WaitForExit(_milliseconds_timeout);

                _batch_ok = false;
                if (pProcess.HasExited)
                {
                    return true;
                }
                else
                {
                    pProcess.Kill();
                    Splash.SetStatus(" Update Firmware: Timeout reached. Failed");
                    sleep(100);
                    return false;
                }
            }
            return false;
        }

        private bool updateSlowFirmware(string fwPath, string fwFilename)
        {
            // write batch:
            bool _batch_ok = false;
            string _batch = fwPath + "FWUpdate.bat";
            int _milliseconds_timeout = 20000;   //20 sec

            if (File.Exists(fwPath + fwFilename))
            {
                try
                {
                    FileStream myStream = new FileStream(_batch, FileMode.Create);
                    StreamWriter sw = new StreamWriter(myStream);

                    //_DebugAdd("dataLogging:Do_Startup(): Create Slow Firmware Update batch file: " + _batch);
                    //_DebugAdd("dataLogging:Do_Startup():   COM port= " + Vars.ComportString +
                    //                                    "  Firmware File= " + Vars.FirmwarePath + Vars.NEWFirmwareFileName);

                    sw.WriteLine("cd " + fwPath);
                    sw.WriteLine(@"avrdude.exe -p m328p -carduino -P \\.\COM" + Vars.ComportString + " -b57600 -D -Uflash:w:\"" + fwPath + fwFilename + "\":i - v > \"" + fwPath + "log.txt\" 2>&1 ");
                    sw.WriteLine("timeout 2");
                    sw.WriteLine("exit");
                    sw.Close();

                    Splash.SetStatus(" Update Legacy Firmware: Created batch file.");
                    sleep(100);
                    _batch_ok = true;
                }
                catch (Exception e)
                {
                    ErrorString = "dataLogging:DoStartup(), Legacy Firmware update: Failed to create batch file" + e.Message;
                    Splash.SetStatus(" Update Legacy Firmware: Create batch file failed");
                    sleep(100);
                    _DebugAdd(ErrorString);
                    return false;
                }
            }
            else
            {

                Splash.SetStatus(" Update Legacy Firmware: File does not exist (" + fwPath + fwFilename + ")");
                sleep(100);
                return false;
            }

            // Start the Firmware loader
            if (_batch_ok)
            {
                Splash.SetStatus(" Update Legacy Firmware: Starting with " + (_milliseconds_timeout/1000).ToString() + " sec timeout");
                sleep(100);
                System.Diagnostics.Process pProcess = new System.Diagnostics.Process();
                pProcess.StartInfo.FileName = _batch;
                pProcess.StartInfo.UseShellExecute = false;
                pProcess.StartInfo.EnvironmentVariables["PATH"] = fwPath;
                pProcess.StartInfo.RedirectStandardOutput = true;
                pProcess.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                pProcess.StartInfo.CreateNoWindow = true; //diplay a windows
                pProcess.Start();
                pProcess.WaitForExit(_milliseconds_timeout);

                _batch_ok = false;
                if (pProcess.HasExited)
                {
                    return true;
                }
                else
                {
                    pProcess.Kill();
                    Splash.SetStatus(" Update Legacy Firmware: Timeout reached. Failed");
                    sleep(100);
                    return false;
                }
            }
            return false;   
        }

        public bool Do_UpdateFirmware(string fwPath, string fwFilename)
        {
            File.Delete(fwPath + "log.txt");

            Splash.ShowSplashScreen();
            sleep(1000);
            

            Splash.SetStatus("Update Firmware. Updating high speed...");
            System.Threading.Thread.Sleep(100);
            updateFastFirmware(fwPath, fwFilename);

            if (File.Exists(fwPath + "log.txt") == false)
            {
                Splash.SetStatus("Update Firmware. Update not successful.");
                sleep(3000);
                return false;
            }

            if (fileContains(fwPath + "log.txt", "flash verified") == true)
            {
                Splash.SetStatus("Update Firmware. Update successful.");
                sleep(100);
                return true;
            }
            if (fileContains(fwPath + "log.txt", "not in sync") == true)
            {
                Splash.SetStatus("Update Firmware. Updating high speed not successfull." + Environment.NewLine + " Check Firmware. Updating low speed...");
                sleep(100);
                updateSlowFirmware(fwPath, fwFilename);
                if (fileContains(fwPath + "log.txt", "flash verified") == true)
                {
                    Splash.SetStatus(" Update Firmware. Update successful.");
                    sleep(100);
                    return true;
                }
                if (fileContains(fwPath + "log.txt", "not in sync") == true)
                {
                    Splash.SetStatus("Update Firmware. Update failed.");
                    sleep(100);
                    return false;
                }
            };
            return false;
        }

        private bool fileContains(string filename, string searchString)
        {
            string line;
            bool _ok=false;
            int _attempts = 0;
            StreamReader file;

            // try to read the file. Maybe blocked by avrdude!
            while ((_ok == false) && (_attempts < 20))
            {               
                try
                {
                    file = new StreamReader(filename);
                    _ok = true;
                    file.Close();
                }
                catch (Exception ex)
                {
                    Splash.SetStatus(" Update Firmware. Update not successful. Try again (" + Convert.ToString(_attempts) + "/20)...");
                    _attempts++;
                    sleep(5000);
                }
            }

            // Now file is free or we'll get the error            
            try
            {
                file = new StreamReader(filename);
                while ((line = file.ReadLine()) != null)
                {   if (line.Contains(searchString))
                    {
                        file.Close();
                        return true;
                    }
                }
                file.Close();
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Same in DataLogging, COM_and_Request, ChartForm
        /// </summary>
        private const int NO_REQUEST = 0;
        private const int START = 1;
        private const int STOP = 2;
        private const int LOADMETHOD = 3;
        private const int SAVEMETHOD = 4;
        private const int EDITMETHOD = 5;
        private const int SHUTDOWN = 6;
        private const int MAINTENANCE = 7;
        private const int UNKNOWN = 99;
        private const int ERROR = 999;

        /// <summary>
        /// Work on Request States set from COM_and_Request Class
        /// Sets NO_REQUEST after work on the request is done.
        /// </summary>
        public void Do_WorkOnRequest(object sender, EventArgs eArgs)
        {
            // count up Timer
            Vars.TimeSinceStartup += 1;

            // ONLY go on, if no request is running !
            if (COM.CurrentRequest != NO_REQUEST)
                return;

            string _str = "Do_WorkOnRequest: " + COM.RequestString + " (" + COM.CurrentRequest_StringParm + ",  " + COM.CurrentRequest_IntParm + ")";

            //checkTrapInUse();     2.8.1
            COM.Do_CheckCOM();
           // _DebugAdd("Do_WorkOnRequest: Do_CheckCOM done: " + COM.RequestString + " (" + COM.CurrentRequest_StringParm + ",  " + COM.CurrentRequest_IntParm + ")");



            if (COM.CurrentRequest == NO_REQUEST)
            {
                //_DebugAdd(_str + " Current Request = NO_REQUEST");
                return; }
            else
            {
            }


            _DebugAdd(_str + "COM.CurrentRequest= " + COM.CurrentRequest + "= " + COM.RequestString);

            switch (COM.CurrentRequest)
            {
                case MAINTENANCE:
                    _DebugAdd( _str);
                    break;
                case SHUTDOWN: 
                    _DebugAdd( _str);  
                    _form.Close();
                    break;
                case STOP: 
                    _DebugAdd(_str);  
                    Do_StopCycle();             
                    break;
                case START: 
                    _DebugAdd( _str);
                    if (Vars.eTrap_CryoMode)
                        Do_StartCycleForever();
                    else
                    {

                    }
                    break;
                case LOADMETHOD:
                    _DebugAdd( _str);
                    COM.Do_CheckMHParms();
                    Do_LoadAndApplyMethod(COM.MHMethPath + COM.MHMethFile + "\\etrap.cme");
                    break;
                case SAVEMETHOD:
                    _str = " case SAVEMETHOD:";
                    _DebugAdd( _str );
                    COM.Do_CheckMHParms();
                    _DebugAdd(_str + "COM.Do_CheckMHParms() done");

                    Vars.method_FullPathAndFilename = COM.MHMethPath + COM.MHMethFile + "\\etrap.cme";
                    _DebugAdd(_str + "Vars.method_FullPathAndFilename =" + Vars.method_FullPathAndFilename);

                    Do_CreateTrapCycle();
                    _DebugAdd(_str + "Do_CreateTrapCycle() done");

                    Do_SaveMacroFile();
                    _DebugAdd(_str + "Do_SaveMacroFile() done");

                    Do_SaveMethodFile();
                    _DebugAdd(_str + "Do_SaveMethodFile() done");

                    _form.Do_SetMethodIsSaved();          
                    break;

                case ERROR:
                        _DebugAdd( _str); 
                        ErrorString = "DataLog: Do_WorkOnRequest: " + COM.ErrorString;
                        break;

                case EDITMETHOD:
                        _DebugAdd( _str); 
                        break;

                default:                    
                    break;
               
            }
            _form.Do_WorkOnRequest_UpdateForm();

            // NO_REQUEST is set ONLY here 
            if (COM.CurrentRequest == EDITMETHOD)        //|| (COM.CurrentRequest == START)
                { ;}        // or after EDITMETHOD or START is closed by OK Button !
            else
                COM.CurrentRequest = NO_REQUEST;
            

        }

        public bool UseThisCOM(string name)
        {
            if (name == Vars.ExcludeCOM1)
                return false;
            if (name == Vars.ExcludeCOM2)
                return false;
            if (name == Vars.ExcludeCOM3)
                return false;
            if (name == Vars.ExcludeCOM4)
                return false;
            if (name == Vars.ExcludeCOM5)
                return false;
            return true;
        }

        public void Do_StartupSerialPort()
        {

            Splash.SetStatus("Serial Ports...");
            sleep(100);
            // Create Serial Port object and test it
            // Just walk through all ComPorts except the ones mentioned in 
            // eTrap Configuration
            bool _isOK = false;
            int _port = 0;
            int _numPorts = 0;

            _serialTransport = new SerialTransport(_port)
            {
                CurrentSerialSettings = { BaudRate = 115200, DtrEnable = false } // object initializer
            };
            _numPorts = _serialTransport.NumberOfPorts();
            _port = _numPorts-1;
            
            while ((_isOK == false) && (_port >= 0))
            {              
                _serialTransport = new SerialTransport(_port);
                string _n = _serialTransport.CurrentSerialSettings.PortName;
                if (UseThisCOM(_serialTransport.CurrentSerialSettings.PortName)==false)
                {
                    Splash.SetStatus("Serial Ports. Exclude " + _n);
                    sleep(200);
                    _serialTransport.Dispose();
                }
                else
                {
                    // Initialize the command messenger with the Serial Port transport layer
                    _cmdMessenger = new CmdMessenger(_serialTransport)
                    {
                        // Set if it is communicating with a 16- or 32-bit Arduino board
                        BoardType = BoardType.Bit16
                    };
                    Splash.SetStatus("Serial Ports. Testing " + _n); // _serialTransport.CurrentSerialSettings.PortName);
                    sleep(100);
                    _isOK = testSerialPort();

                    if (_isOK == false)
                    {
                        // Stop listening
                        _cmdMessenger.StopListening();
                        _cmdMessenger.Dispose();
                        _serialTransport.Dispose();
                        
                        // NEEDED in case the PC just restarted !
                        // and open again
                        _serialTransport = new SerialTransport(_port)
                        {
                            CurrentSerialSettings = { BaudRate = 115200, DtrEnable = false } // object initializer
                        };
                        _numPorts = _serialTransport.NumberOfPorts();
                        _cmdMessenger = new CmdMessenger(_serialTransport)
                        {
                            BoardType = BoardType.Bit16 // Set if it is communicating with a 16- or 32-bit Arduino board
                        };

                        _isOK = testSerialPort();
                        if (_isOK == false)
                        {
                            // Stop listening
                            _cmdMessenger.StopListening();
                            _cmdMessenger.Dispose();
                            _serialTransport.Dispose();
                        }
                    }
                }
                _port--;
            }

            if (_isOK)
            {
                Vars.eTrapIsConnected = true;
                Vars.eTrapNumBadReadbacks = 0;
                Splash.SetStatus("Serial Ports. Found COM" + Vars.ComportString);
                sleep(100);
            }
            else
            {
                Vars.eTrapIsConnected = false;
                Vars.eTrapNumBadReadbacks = 0;
                Splash.SetStatus("Serial Ports. Found no COM port");
                sleep(100);
            }
        }

        public bool testSerialPort()
        {
            Int16 rawInt1 = 0;
            Int16 rawInt2 = 0;
            Int16 rawInt3 = 0;
            bool isOK = false;



            // Set Received command strategy that removes commands that are older than 1 sec
            _cmdMessenger.AddReceiveCommandStrategy(new StaleGeneralStrategy(1000));
            _cmdMessenger.AddSendCommandStrategy(new StaleGeneralStrategy(1000));
            // Attach the callbacks to the Command Messenger
            AttachCommandCallBacks();
            // Attach to NewLinesReceived for logging purposes
            _cmdMessenger.NewLineReceived += NewLineReceived;
            // Attach to NewLineSent for logging purposes
            _cmdMessenger.NewLineSent += NewLineSent;            

            // Start listening
            _cmdMessenger.StartListening();

            // Use Firmware version to test
            var command = new SendCommand((int)Command.GetFWAndSavedParms, (int)Command.GetFWAndSavedParmsResult, 1000);
            var FWVersionResult = _cmdMessenger.SendCommand(command);
            if (FWVersionResult.Ok)
            {
                int t1 = FWVersionResult.ReadInt16Arg();
                int t2 = FWVersionResult.ReadInt16Arg();
                rawInt1 = FWVersionResult.ReadInt16Arg();
                rawInt2 = FWVersionResult.ReadInt16Arg();
                rawInt3 = FWVersionResult.ReadInt16Arg();
                
                if ((t1 > 0) || (t2 > 0))
                {
                    isOK = true;
                    Vars.FirmwareMainVersion = t1;
                    Vars.FirmwareSubVersion = t2;
                    Vars.tempLowCorrectionSlope = (Single)rawInt1 / 1000;
                    Vars.tempMidCorrectionSlope = (Single)rawInt2 / 1000;
                    Vars.tempHiCorrectionSlope = (Single)rawInt3 / 1000;
                    Vars.FirmwareVersionString = "";
                    Vars.FirmwareFileName_FromVersionNo = "";
                }
                else
                    isOK = false;
            }
            else
                isOK = false;


            if (isOK)
            {
                try
                {
                    Vars.Comport = Convert.ToInt16(_serialTransport.CurrentSerialSettings.PortName.Substring(3));
                }
                catch (Exception e)
                {
                    ErrorString="DataLogging/testSerialPort: "+e.Message;
                    Vars.Comport = 0;
                    isOK = false;
                }
            }

            return isOK;
        }

        // Exit function
        public void Do_ExitSerialCOM(bool afterComFailiure)
        {   // Stop listening
            // _cmdMessenger may be NULL if no COMPort was found!
            try
            {
                _cmdMessenger.StopListening();

                // Dispose Command Messenger
                _cmdMessenger.Dispose();

                // Dispose Serial Port object
                if (Vars.Comport > 0)
                    _serialTransport.Dispose();
                _serialTransport.StopListening();
            }
            catch (Exception e)
            { }

            if (afterComFailiure)
            {
                Do_SignalLostCommunication();                
            }            
        }

        public void Do_SignalLostCommunication()
        {
            Vars.eTrapIsConnected = false;
            Vars.Comport = 0;
            Vars.FirmwareMainVersion = 0;
            Vars.FirmwareSubVersion = 0;
            Vars.FirmwareVersionString = "";
            Vars.ControlerStatusString = "";

            this.ErrorString = "Lost USB Communication";          
        }

        /// Attach command call backs. 
        private void AttachCommandCallBacks()
        {
            _cmdMessenger.Attach(OnUnknownCommand);
            _cmdMessenger.Attach((int)Command.Acknowledge, OnAcknowledge);
            _cmdMessenger.Attach((int)Command.Error, OnError);
            _cmdMessenger.Attach((int)Command.ErrorWithArgs, OnErrorWithArgs);
        }

        #region // ------------------  CALLBACKS ---------------------

        // Called when a received command has no attached function.
        // In a WinForm application, console output gets routed to the output panel of your IDE
        
        void OnUnknownCommand(ReceivedCommand arguments)
        {   
            _form.appendLogString("Error: " + arguments.ReadStringArg());
        }
        void OnAcknowledge(ReceivedCommand arguments)
        {   _form.appendLowLevelLogString("Received >" + arguments.ReadStringArg());
        }
        void OnError(ReceivedCommand arguments)
        {
            _form.appendLogString("Received >" + arguments.ReadStringArg());
        }

        public void Do_ListControlerCycle()
        {
            if (!Vars.eTrapIsConnected)
                return;

                var command = new SendCommand((int)Command.AskControlerCycle, (int)Command.SendCycleToPC, 1000);

                var cmdCycle = _cmdMessenger.SendCommand(command);
                if (!cmdCycle.Ok)
                {
//                    Do_ExitSerialCOM(true);
                }
                else
                    //if (cmdCycle.Ok)
                {
                    int line, atom, parm1, parm2, parm3;
                    string cycleString = "";
                    atom = 0;
                    int i = 0;
                    while ((atom != (int)ATOM.END_OF_CYCLE) && (i < macMaxLines))
                    {
                        line = cmdCycle.ReadInt16Arg();
                        atom = cmdCycle.ReadInt16Arg();
                        parm1 = cmdCycle.ReadInt16Arg();
                        parm2 = cmdCycle.ReadInt16Arg();
                        parm3 = cmdCycle.ReadInt16Arg();

                        cycleString = Vars.make_cycleLineString(atom, parm1, parm2, parm3);
                        _form.appendToControlerCycleBox(line + ": " + cycleString);
                        i++;
                    }
                }
        }


        public void Do_GetControlerMethodParms()    // eTrap: OnAskControlerCycle
        {
            if (!Vars.eTrapIsConnected)
                return;
            
            var command = new SendCommand((int)Command.AskControlerCycle, (int)Command.SendCycleToPC, 1000);

            var cmdCycle = _cmdMessenger.SendCommand(command);
            if (!cmdCycle.Ok)
            {
     //           Do_ExitSerialCOM(true);
            }
            else
                //if (cmdCycle.Ok)
            {
                int line, atom, parm1, parm2, parm3;
                atom = 0;
                int i = 0;
                while ((atom != (int)ATOM.END_OF_CYCLE) && (i < macMaxLines))
                {
                    line = cmdCycle.ReadInt16Arg();
                    atom = cmdCycle.ReadInt16Arg();
                    parm1 = cmdCycle.ReadInt16Arg();
                    parm2 = cmdCycle.ReadInt16Arg();
                    parm3 = cmdCycle.ReadInt16Arg();

                    // extract variables:
                    switch (line)
                    {   
                        case 0:
                            Vars.method_Prepare_2_Time = parm2;
                            break;
                        case 2:
                            Vars.method_StandbyTemp = parm1;
                            break;
                        case 8:
                            Vars.method_LowTemp = parm1;
                            break;
                        case 9:
                            Vars.method_LowTime_PreInject = parm2;
                            break;
                        case 12:
                            Vars.method_LowTime_PostInject = parm2;
                            break;
                        case 15:
                            Vars.method_HighTemp = parm1;
                            break;
                        case 16:
                            Vars.method_HighTemp_Time = parm2;
                            break;
                        case 17:
                            Vars.SINGLE_RUN = (parm1 == 1 ? true : false);
                            break;
                        default:
                            break;
                    }

                    i++;
                }
            }
        }

        // Get all Readbacks (called every second)
        public void Do_GetReadbacks()
        {
            if (!Vars.eTrapIsConnected)
            {
                return;
            }
            if (NoReadbacksPlease)
                return;

            var command = new SendCommand((int)Command.AskForReadbacks,(int)Command.SendReadbacksToPC,1000);
            var cmdReadback = _cmdMessenger.SendCommand(command);

            if (!cmdReadback.Ok)
            {
                Vars.eTrapNumBadReadbacks += 1;
                if (Vars.eTrapNumBadReadbacks>5)
                {
                    Vars.eTrapIsConnected = false;
                    Do_ExitSerialCOM(true);
                }
            }
            else
            {
                Vars.eTrapNumBadReadbacks = 0;

                var chartTime = cmdReadback.ReadFloatArg();
                var analog1 = cmdReadback.ReadFloatArg();
                
                Vars.ControlerPower = (int)cmdReadback.ReadFloatArg();

                Vars.Timer1_seconds = cmdReadback.ReadInt16Arg();
                Vars.Timer1_minutes = cmdReadback.ReadInt16Arg();
                Vars.Timer1_hours = cmdReadback.ReadInt16Arg();
    
                Vars.currentGoalTemp = cmdReadback.ReadFloatArg();
                Vars.pid_min_temperature = cmdReadback.ReadFloatArg();
                Vars.pid_max_temperature = cmdReadback.ReadFloatArg();
                Vars.pid_portional = cmdReadback.ReadFloatArg();
                Vars.pid_integral = cmdReadback.ReadFloatArg();
                Vars.pid_duty_cycle = cmdReadback.ReadFloatArg();
                Vars.pid_integral_sum = cmdReadback.ReadFloatArg();
                Vars.heaterIsOn = cmdReadback.ReadInt16Arg();
                // for debug. Remove later
                Vars.error = cmdReadback.ReadFloatArg();
                Vars.p_error = cmdReadback.ReadFloatArg();
                Vars.i_error = cmdReadback.ReadFloatArg();
                Vars.correction_sum = cmdReadback.ReadFloatArg();

                Vars.GC_Prepare = (int)cmdReadback.ReadInt16Arg();
                Vars.GC_Ready = (int)cmdReadback.ReadInt16Arg();
                Vars.GC_Start = (int)cmdReadback.ReadInt16Arg();
                Vars.TRAP_Cool = (int)cmdReadback.ReadInt16Arg();
                Vars.Prepare_2 = (int)cmdReadback.ReadInt16Arg();

                Vars.CTimerTime_0 = cmdReadback.ReadUInt32Arg();
                Vars.CTimerTime_1 = cmdReadback.ReadUInt32Arg();
                Vars.CTimerTime_2 = cmdReadback.ReadUInt32Arg();
                Vars.CTimerTime_3 = cmdReadback.ReadUInt32Arg();
                Vars.CTimerTime_4 = cmdReadback.ReadUInt32Arg();

                Vars.CryoTimeout = (int)cmdReadback.ReadInt16Arg();
                Vars.Prepare_2_Time = (int)cmdReadback.ReadInt16Arg();

             //   Vars.SINGLE_RUN = ( cmdReadback.ReadInt16Arg()==1? true:false);
                var _test = cmdReadback.ReadInt16Arg();
                Vars.SINGLE_RUN = (_test == 1 ? true : false);
                Vars.CycleRunningLastState = Vars.CycleRunning;

                Vars.CycleRunning = (int)cmdReadback.ReadInt16Arg();
                if (Vars.CycleRunning > 0)
                {
                    Vars.CycleLine = cmdReadback.ReadInt16Arg();
                    Vars.Atom = cmdReadback.ReadInt16Arg();
                    Vars.Parm1 = cmdReadback.ReadInt16Arg();
                    Vars.Parm2 = cmdReadback.ReadInt16Arg();
                    Vars.Parm3 = cmdReadback.ReadInt16Arg();
                }


                // calculate Temps
                if (Vars.FirstLoggingDone == false)
                    Vars.currentTemp = analog1;
                else
                {
                    if (TimeUtils.HasExpired(ref _previousTemperatureUpdate, 1000))
                    {
                        if ( (Vars.CycleLine == 14) && (Vars.CycleRunning>0) )
                        {
                            Vars.currentTemp = analog1 * (Vars.currentGoalTemp / (Vars.currentGoalTemp * (float)0.6 + (float)57));
                        }
                        else
                        {
                            if ( (Vars.CycleLine == 16) && (Vars.CycleRunning>0) )  // Heat up
                            {
                                int t = (int)(Vars.CTimerTime_1 / 1000);
                                if (t < 40)
                                {
                                    float _lastTemp = Vars.currentTemp;
                                    Vars.currentTemp = analog1 * (Vars.currentGoalTemp / (Vars.currentGoalTemp * (float)0.6 + (float)57));
                                    Vars.currentTemp = (analog1 * t + Vars.currentTemp * 3) / (t + 3);
                                }
                                else
                                {
                                    if (Vars.currentGoalTemp - analog1 > 8)
                                        Vars.currentTemp = analog1;
                                    else
                                        Vars.currentTemp = (analog1 + Vars.currentTemp) / 2;
                                }
                            }
                            else
                            {
                                if (Vars.currentGoalTemp - analog1 > 8)
                                    Vars.currentTemp = analog1;
                                else
                                    Vars.currentTemp = (analog1 + Vars.currentTemp) / 2;
                            }
                        }

                    }

                }

                // Kick it to update...
                Vars.ControlerStatusString = "";  
                Vars.pid_string = "";
                Vars.runningCycleLineString = "";
                Vars.CryoTimeoutString = "";
                Vars.CTimerString = "";
                Vars.RunningTimerString = "";
                Vars.Timer1_TimeString = "";
                Vars.TemperatureString = "";
                Vars.TTLString = ""; 
                Vars.TTLStringVal = "";

            


                if (Vars.Logging == true)
                {
                    _form.UpdateGraph(chartTime, Vars.currentTemp);
                }

                // Kick it
                Vars.eTrapDisplayBoxString = "";

                if (Vars.CycleLine != Vars.LastCycleLine)
                {
                    Do_Update_Cycle_Counters();
                    Vars.LastCycleLine = Vars.CycleLine;
                }

                _form.Do_UpdateButtons();
                _form.Do_UpdateCycleGraphic();
                Vars.FirstLoggingDone = true;
            }
        }

        enum Parameter
        {   goalTemperature,
            heater,
            timer,
            cycleRestartEndless,
            cycleStop,
            changeVariables,
            cycleToEEprom,
            cycleStepMode,
            cycleNextStep, 
            ignoreCryoTimeout,
            ignoreGCPrepare,
            ignorePrepare_2,
            singleRunMode,
            temperatureCorrection,
            fakeSyncSignals
        }



        public void Do_Update_Cycle_Counters()
        {
            
                if (Vars.CycleLine == 16)      // High Temp.
                {
                    Vars.TRAP_CYCLES++;
                    Vars.INSTRUMENT_CYCLES++;
                }
            
        }

        /// <summary>
        /// Send full macro to trap and make it default !
        /// </summary>
        /// <returns></returns>
        public bool Do_SendToTrapAndMakeStartupMethod()
        {
            try
            {
                Do_CreateTrapCycle();
                Do_SendVariablesToControler();

                Do_CreateStartupMethod();

                _form.Do_SetMethodIsApplied();
                return true;
            }
            catch (Exception ex)
            {
                ErrorString="SendToTrap: " + ex.Message;
                return false;
            }
        }

        // General Routine to send some Parms
        public void Do_SendParms(int Parm)      // 2.8.1 not send ok
        {
            if (!Vars.eTrapIsConnected)
                return;
            
            string errStr = "Error ";
            NoReadbacksPlease = true;

            var command = new SendCommand((int)Command.ParmsToControler, (int)Command.ParmsReceived, 100);     // 100 is ok for all, but 1000 needed for cycleToEEprom

            switch (Parm)
            {
                case (int)Parameter.fakeSyncSignals:
                    errStr = errStr + "fakeSyncSignals";
                    command.AddArgument((int)Parameter.fakeSyncSignals);
                    command.AddArgument(1);
                    command.AddArgument(Vars.fakeSyncSignals == true ? 1 : 0);
                    break;
                case (int)Parameter.temperatureCorrection:
                    errStr = errStr + "temperatureCorrection";
                    command.AddArgument((Int16)Parameter.temperatureCorrection);
                    command.AddArgument(3);
                    command.AddArgument((Int16)(Vars.tempLowCorrectionSlope * 1000));
                    command.AddArgument((Int16)(Vars.tempMidCorrectionSlope * 1000));
                    command.AddArgument((Int16)(Vars.tempHiCorrectionSlope * 1000));                   
                    break;
                
                case (int)Parameter.ignorePrepare_2:
                    errStr = errStr + "ignorePrepare_2";
                    command.AddArgument((int)Parameter.ignorePrepare_2);
                    command.AddArgument(1);
                    command.AddArgument(Vars.ignore_Prepare_2 == true ? 1 : 0);
                    break;
                case (int)Parameter.ignoreGCPrepare:
                    errStr = errStr + "ignoreGCPrepare";
                    command.AddArgument((int)Parameter.ignoreGCPrepare);
                    command.AddArgument(1);
                    command.AddArgument(Vars.ignore_GC_Prepare == true ? 1 : 0);
                    break;
                case (int)Parameter.singleRunMode:
                    errStr = errStr + "singleRunMode";
                    command.AddArgument((int)Parameter.singleRunMode);
                    command.AddArgument(1);
                    command.AddArgument(Vars.SINGLE_RUN == true ? 1 : 0);
                    break;
                case (int)Parameter.ignoreCryoTimeout:
                    errStr = errStr + "ignoreCryoTimeout";
                    command.AddArgument((int)Parameter.ignoreCryoTimeout);
                    command.AddArgument(1);
                    command.AddArgument(Vars.IgnoreCryoTimeout == true ? 1 : 0);
                    break;
                case (int)Parameter.cycleStepMode:
                    errStr = errStr + "cycleToEEprom";
                    command.AddArgument((int)Parameter.cycleStepMode);
                    command.AddArgument(1);
                    command.AddArgument(Vars.CycleStepMode==true ? 1:0 );
                    break;
                case (int)Parameter.cycleNextStep:
                    errStr = errStr + "cycleToEEprom";
                    command.AddArgument((int)Parameter.cycleNextStep);
                    command.AddArgument(0);
                    break;                 
                case (int)Parameter.cycleToEEprom:
                    command.Timeout = 1000;                                 // writing to EEprom is very slow!
                    errStr = errStr + "cycleToEEprom";
                    command.AddArgument((int)Parameter.cycleToEEprom);
                    command.AddArgument(0);

                    break;                
                case (int)Parameter.changeVariables:
                    errStr = errStr + "changeVariables";
                    command.AddArgument((int)Parameter.changeVariables);
                    command.AddArgument(8); 
                    command.AddArgument(Vars.method_StandbyTemp);
                    command.AddArgument(Vars.method_LowTemp);
                    command.AddArgument(Vars.method_LowTime_PreInject);
                    command.AddArgument(Vars.method_LowTime_PostInject);
                    command.AddArgument(Vars.method_HighTemp);
                    command.AddArgument(Vars.method_HighTemp_Time);
                    command.AddArgument(Vars.SINGLE_RUN);
                    command.AddArgument(Vars.method_Prepare_2_Time);
                    // 2.8.1: 'eTrap Used' is NOT sent to Controler !
                    break;
                case (int)Parameter.goalTemperature:
                    errStr = errStr + "goalTemperature";
                    command.AddArgument((int)Parameter.goalTemperature);
                    command.AddArgument(3);                             // 3 Parms:
                    command.AddArgument(Vars.pid_min_temperature);
                    command.AddArgument(Vars.pid_max_temperature);
                    command.AddArgument(Vars.currentGoalTemp);
                    break;
                case (int)Parameter.heater:
                    errStr = errStr + "heater";
                    command.AddArgument((int)Parameter.heater);
                    command.AddArgument(1);                             // 1 Parm
                    Vars.heaterIsOn = ( Vars.heaterIsOn == 0 ? 1 : 0) ; // Switch the state!
                    command.AddArgument(Vars.heaterIsOn);
                    break;
                case (int)Parameter.timer:
                    errStr = errStr + "timer";
                    command.AddArgument((int)Parameter.timer);
                    command.AddArgument(1);
                    Vars.Timer1_On = (Vars.Timer1_On == 0 ? 1 : 0);
                    command.AddArgument(Vars.Timer1_On);
                    break;
                case (int)Parameter.cycleRestartEndless:
                    errStr = errStr + "cycleRestart";
                    command.AddArgument((int)Parameter.cycleRestartEndless);
                    command.AddArgument(0);
                    break;
                case (int)Parameter.cycleStop:
                    errStr = errStr + "cycleStop";
                    command.AddArgument((int)Parameter.cycleStop);
                    command.AddArgument(0);
                    break;
                default:
                    errStr = errStr + "default";
                    break;
            }


            // Send command
            var sendCmd = _cmdMessenger.SendCommand(command);
            if (!sendCmd.Ok)
            {
       //         Do_ExitSerialCOM(true);
            }
            else
               
               //if (!sendCmd.Ok)
            {   _form.appendLogString(errStr);
            }

            NoReadbacksPlease = false;
        }

        public void OnErrorWithArgs(ReceivedCommand arguments)
        {
            int arg = 0;
            var numArgs = arguments.ReadInt16Arg();
            var ErrString = "Error: " + arguments.ReadStringArg();
            for (int i = 0; i<numArgs; i++)
            {   arg = arguments.ReadInt16Arg();
                ErrString = ErrString + ", " + Convert.ToString(arg);
            }
            _form.appendLogString(ErrString);
        }

        public bool Do_GetAndCheckFWAndParms()
        {
            Splash.SetStatus("Check Firmware.");
            sleep(100);
            // Create command FloatAddition, which will wait for a return command FloatAdditionResult
            var command = new SendCommand((int)Command.GetFWAndSavedParms, (int)Command.GetFWAndSavedParmsResult, 1000);
            // Send command
            var FWVersionResult = _cmdMessenger.SendCommand(command);
            if (!FWVersionResult.Ok)
            {
                Vars.FirmwareMainVersion = 0;
                Vars.FirmwareSubVersion = 0;
                Vars.FirmwareVersionString = "";
                Do_ExitSerialCOM(false);

                Splash.SetStatus("Check Firmware. Nothing found. Exit.");
                sleep(100);
            }
            else

            {   // FWVersionResult is Ok
                Vars.FirmwareMainVersion = FWVersionResult.ReadInt16Arg();
                Vars.FirmwareSubVersion = FWVersionResult.ReadInt16Arg();

                Vars.tempLowCorrectionSlope = (float)Math.Round((float)FWVersionResult.ReadInt16Arg() / 1000,3);
                Vars.tempMidCorrectionSlope = (float)Math.Round((float)FWVersionResult.ReadInt16Arg() / 1000,3);
                Vars.tempHiCorrectionSlope = (float)Math.Round((float)FWVersionResult.ReadInt16Arg() / 1000,3);

                Vars.FirmwareVersionString = "";
                Vars.FirmwareFileName_FromVersionNo = "";
                // compare to Software Main and Sub version:
                if ((Vars.SoftwareMainVersion == Vars.FirmwareMainVersion)
                     && (Vars.SoftwareSubVersion == Vars.FirmwareSubVersion))
                {
                    Splash.SetStatus("Check Firmware. Found " + Vars.FirmwareVersionString + ". ok.");
                    sleep(100);
                    return true;
                }
                else
                {
                    Splash.SetStatus("Check Firmware. Found " + Vars.FirmwareVersionString + " but need " + Vars.SoftwareMainVersionString + "." +Vars.SoftwareSubVersionString);
                    sleep(100);
                    // If FIRST startup after manual FW burning, it's ok
                    if (File.Exists(Vars.FirmwarePath + "1stStart"))
                    {
                        File.Delete(Vars.FirmwarePath + "1stStart");
                        sleep(1000);
                        Splash.SetStatus("Check Firmware. '1stStart' flag: No Update this time!");
                        sleep(100);
                        return true;
                    }
                    else
                    {
                        Splash.SetStatus("Check Firmware. No '1stStart' flag: Update...");
                        sleep(100);
                        return false;
                    }
                }

            }

            return true;
        }


        #endregion

        private string _errorstring = "";
        public string ErrorString
        {
            get { return _errorstring; }
            set
            {
                _errorstring = value;
            }
        }



        // Log received line to console
        private void NewLineReceived(object sender, NewLineEvent.NewLineArgs e)
        {  
            _form.appendLowLevelLogString("Received > " + e.Command.CommandString());
            _form.appendCycleString(e.Command.CommandString());
            // _frm.drawCurrentMethodPosition


        }
        // Log sent line to console
        private void NewLineSent(object sender, NewLineEvent.NewLineArgs e)
        {
            _form.appendLowLevelLogString( "Sent > " + e.Command.CommandString());
        }


        #region  Single Command Routines

        public void SetCryoTimeout(bool i)
        {
            Vars.IgnoreCryoTimeout = i;
            Do_SendParms((int)Parameter.ignoreCryoTimeout);
        }

        public void SetSingleRunMode(bool i)
        {
            Vars.SINGLE_RUN = i;
            Do_SendParms((int)Parameter.singleRunMode);
        }

        public void SetFakeSyncSignals(bool i)
        {
            Vars.fakeSyncSignals = i;
            Do_SendParms((int)Parameter.fakeSyncSignals);
        }

        public void SetIgnoreGCPrepare(bool i)
        {
            Vars.ignore_GC_Prepare = i;
            Do_SendParms((int)Parameter.ignoreGCPrepare);
        }
        public void SetIgnorePrepare_2(bool i)
        {
            Vars.ignore_Prepare_2 = i;
            Do_SendParms((int)Parameter.ignorePrepare_2);
        }

        public void SetUsePrepare2_Timer(bool i)
        {
            if (i == true)
                Vars.method_Prepare_2_Time = 60; // ON = 60 seconds
            else
                Vars.method_Prepare_2_Time = 9999; // Off = 9999 seconds
        }

        public void SwitchCycleStepMode()
        {   Vars.CycleStepMode = (!Vars.CycleStepMode);
            Do_SendParms((int)Parameter.cycleStepMode);
        }

        public void Do_CycleNextStep()
        {   Vars.CycleNextStepPlease = true;
            Do_SendParms((int)Parameter.cycleNextStep);
        }

        // Used when loading a Method to Controller
        public void Do_StopCycle()
        {   Do_SendParms((int)Parameter.cycleStop);
        }

        // From Button Start
        public void Do_StartCycleForever()
        {
            if (Vars.heaterIsOn==0)
            {    SwitchHeater();    // -> DoSendParms will switch also
            }
            if (Vars.eTrap_CryoMode)
                Do_SendParms((int)Parameter.cycleRestartEndless);

        }       

        // Timer On/Off from Button
        public void SwitchTimer1()
        {   Do_SendParms((int)Parameter.timer);
        }

        // From Heater Button
        public void SwitchHeater()
        {   Do_SendParms((int)Parameter.heater);
        }

        // From Temperatures Button
        public void SetAllTemperatures()
        {   Do_SendParms((int)Parameter.goalTemperature);
        }

        // From Heater Offset Button
        public void SetAllTemperatureCorrections()
        {
            Do_SendParms((int)Parameter.temperatureCorrection);
        }

        #endregion



/// <summary>
/// If StartedFromMassHunter AND Method doesn't exist: Load last Standalone Method
/// </summary>
/// <param name="FullPath"></param>
/// <returns></returns>
        public bool Do_LoadAndApplyMethod(string FullPath)
        {
            string _fallBackMethod="";
            string _myFullPath = FullPath;

            ErrorString = "";
            try
            {
                if (!File.Exists(FullPath))
                {   
                    _fallBackMethod = Vars.method_FullPathAndFilename_LastStandaloneMethod;
                    if (!File.Exists(_fallBackMethod))
                        ErrorString = FullPath + " and FallBack Method " + _fallBackMethod + "  not found!";
                    else
                        _myFullPath = _fallBackMethod;
                }

                if (ErrorString.Length > 0)
                    return false;



                
                if (FullPath != _myFullPath)
                {
                    bool _startedFromMassHunter = Vars.StartedFromMassHunter;

                    // Load the Fallback Method:
                    Vars.StartedFromMassHunter = false;
                    Vars.method_FullPathAndFilename = _myFullPath;
                    Do_LoadMethodFile();

                    // and reset all
                    Vars.StartedFromMassHunter = _startedFromMassHunter;
                    Vars.method_FullPathAndFilename = FullPath;
                    // create the method inside the MH Method
                    Do_SaveMethodFile();
                }
                else
                {
                    Vars.method_FullPathAndFilename = FullPath;
                    Do_LoadMethodFile();
                }
                _form.WorkOn_InUse_Status(Vars.eTrap_CryoMode);     // 2.8.1
                Do_SendToTrapAndMakeStartupMethod();

                return true;
                
            }
            catch (Exception ex)
            {
                ErrorString = "Do_LoadAndApplyMethod: " + ex.Message;
                return false;
            }

        }

        #region Load Method
    


        private string remove_comment_mark(string st)
        {
            string s = "";
            int i = 2*st.LastIndexOf("\\");
            s = st.Substring(i,st.Length-i);
            return s;
        }
        private string extract_before_bracket(string st)
        {
            string s = "";
            int i = st.IndexOf("(");
            s = st.Substring(0, i);
            return s;
        }
        private string extract_inside_brackets(string st)
        {
            string s = "";
            int i1 = st.IndexOf("(")+1;
            int i2 = st.LastIndexOf(")");
            s = st.Substring(i1, i2-i1);
            return s;
        }


        public bool Do_LoadMethodFile()
        {
            string str = "";
            string parms_string = "";
            string[] parm_value = { };
            int num_parm_values = 0;

            bool _VariableAlreadyFound = false;
            bool _ParameterIsVariable = false;
            int _ParameterIsVariableNumber = 0;

            string _logStr1 = "";

            clearAllMacroArrays();
            currentMacroLine = 0;

            if ( !testFileOpen(Vars.method_FullPathAndFilename, "MethFileLoad") )   return false;

            try
            {   
                /// CT MethFile = *.cme  usually in ChemStation Folder (or in Home Folder)
                FileStream myStream = new FileStream(Vars.method_FullPathAndFilename, FileMode.Open);
                StreamReader sr = new StreamReader(myStream);

                //Vars.method_ShortName = remove_comment_mark(sr.ReadLine());
                str = remove_comment_mark(sr.ReadLine());                          // SSB 01:  overwrites short methodname that is set from MH Parms!
                // USER COMMENT AREA undefined number of lines starting with //


                // Hardware requirement:   (syringe:2.5ml-HS)
                // unused now  

                int i;

                // 
                // Method name (,,,) = Calling the MACRO  
                str = sr.ReadLine();
                Vars.macro_Name = extract_before_bracket(str);

                parms_string = extract_inside_brackets(str);
                parm_value = parms_string.Split(',');               // ALL VARIABLES here
                num_parm_values = parm_value.Length;
                int numberOfVariables = num_parm_values;

                for (i = 0; i < numberOfVariables; i++)
                    VariableValue[i+1]= Convert.ToInt16(parm_value[i]);      // we start at 1 in the Variable Array

                //Vars.method_StandbyTemp = Convert.ToInt16(parm[0]);
                //Vars.method_LowTemp = Convert.ToInt16(parm[1]);
                //Vars.method_LowTime_PreInject = Convert.ToInt16(parm[2]);
                //Vars.method_LowTime_PostInject = Convert.ToInt16(parm[3]);
                //Vars.method_LowTime_PostInject = Convert.ToInt16(parm[4]);
                //Vars.method_HighTemp = Convert.ToInt16(parm[5]);
                //Vars.method_HighTemp_Time = Convert.ToInt16(parm[6]);
                // N E W   V.2.0 = 7 Vars:
                //   Vars.SINGLE_RUN parm[7]
                // NEW      V.2.8.1  = 8 Vars
                //   Vars.SINGLE_RUN parm[8]


                // Get the Name and Range of the Variables:
                i = 0;
                while (i < numberOfVariables)
                {
                    parms_string = sr.ReadLine();
                    parm_value = parms_string.Split(';');
                    num_parm_values = parm_value.Length;
                    if (num_parm_values == 4)
                    {
                        VariableName[i+1] = parm_value[0];
                        VariableDefault[i+1] = Convert.ToInt16(parm_value[1]);
                        VariableLowLimit[i+1] = Convert.ToInt16(parm_value[2]);
                        VariableHighLimit[i+1] = Convert.ToInt16(parm_value[3]);
                        i++;
                    }


                }

                bool _isUpdate = false;
                // Update Macro to Version 2.0
                if (numberOfVariables == 7)
                {
                    _isUpdate = true;
                    numberOfVariables = 8;
                    VariableValue[numberOfVariables] = 1;

                    VariableName[numberOfVariables] = "Single Run Mode";
                    VariableDefault[numberOfVariables] = 1;
                    VariableLowLimit[numberOfVariables] = 0;
                    VariableHighLimit[numberOfVariables] = 1;
                }

                // Add new Variable for version 2.8.1
                //  eTrap Used
                if (numberOfVariables == 8)
                {
                    _isUpdate = true;
                    numberOfVariables = 9;
                    VariableValue[numberOfVariables] = 1;   // default: Used

                    VariableName[numberOfVariables] = "eTrap Used";
                    VariableDefault[numberOfVariables] = 1;
                    VariableLowLimit[numberOfVariables] = 0;
                    VariableHighLimit[numberOfVariables] = 1;
                }

                //2.8.1
                // activate all Vars that are NOT parameter inside the Macro:
                Vars.eTrap_CryoMode = (VariableValue[9] == 1 ? true : false);
                _logStr1 = " eTrap In Use";


                bool loadNextLine = true;

                while (loadNextLine)
                {
                    
                    _ParameterIsVariable = false;
                    _VariableAlreadyFound = false;
                    _ParameterIsVariableNumber = 0;
                    
                    parms_string = sr.ReadLine();
                    string _atomStr = extract_before_bracket(parms_string);     // Atom Name
                    int _ATOM = getAtomToken(_atomStr);                         // Atom Token = ATOM enum

                    if (_isUpdate && (_ATOM == (int)ATOM.END_OF_CYCLE))         // Force the version 2 Parameter
                        parms_string = "END_OF_CYCLE(Single Run Mode)";

                    int _PARM1 = 0;
                    int _PARM2 = 0;
                    int _PARM3 = 0;


                    parms_string = extract_inside_brackets(parms_string);
                    parm_value = parms_string.Split(',');                       // ALL Parameters here
                    num_parm_values = parm_value.Length;

                    i = 0;
                    while (i < num_parm_values)
                    {
                        
                        switch (i)
                        {
                            case 0:                                         // Object OR Timer No. OR Variable
                                if (parm_value[i].Length > 0)
                                {
                                    decimal myDec;
                                    var IsDecimal = decimal.TryParse(parm_value[i], out myDec);
                                    if (IsDecimal)
                                    {
                                        _PARM1 = (int)myDec;                    // Timer
                                    }
                                    else
                                    {
                                        _PARM1 = getParm1Token(parm_value[i]);      // Object OR Variable

                                        //// Is it a Variable ? (valid for PPrepare 2 Time (s)  AND  Single Run Mode)
                                        _ParameterIsVariableNumber = getVariableNumber(parm_value[i]);
                                        if (_ParameterIsVariableNumber > 0)
                                        {
                                            _VariableAlreadyFound = true;
                                            _ParameterIsVariable = true;

                                            if (_ParameterIsVariableNumber == 8)                        // Single Run Mode
                                                _PARM2 = VariableValue[_ParameterIsVariableNumber];
                                        }                                        
                                    }

                                    if (_PARM1 == -1)
                                    {
                                        ErrorString = "DataLog, MethFileLoad: " + parm_value[i] + " not parsed into _PARM1";
                                    }

                                }




                                break;
                        case 1:
                                if (parm_value[i].Length > 0)
                                {
                                    if (!_VariableAlreadyFound)     // If found before: don't search again !!
                                    {
                                        _ParameterIsVariableNumber = getVariableNumber(parm_value[i]);
                                        if (_ParameterIsVariableNumber > 0)
                                        {
                                            _ParameterIsVariable = true;
                                            _PARM2 = VariableValue[_ParameterIsVariableNumber];
                                        }
                                    }


                                    if (_ParameterIsVariableNumber > 0)
                                    {
                                        if (parm_value[i].Equals("On"))
                                        {
                                            _PARM2 = 1;
                                        }
                                        else
                                        {
                                            if (parm_value[i].Equals("Off"))
                                            {
                                                _PARM2 = 0;
                                            }
                                            else
                                            {
                                                if (_ParameterIsVariableNumber == 7)        // Set_Variable PREPARE_2_TIME
                                                {
                                                    _PARM2 = Convert.ToInt16(parm_value[i]);
                                                }
                                            }
                                        }
                                    }
                                }
                                break;
                            case 2:
                                if (parm_value[i].Length>0)
                                    _PARM3 = Convert.ToInt16( parm_value[i]);
                                break;
                            default:
                                break; 
                        }
                        i++;
                    }



                    if (_ParameterIsVariable)
                    {
                        appendMacroLine(_ParameterIsVariableNumber, _ATOM, _PARM1, _PARM2, _PARM3);

                        switch (_ParameterIsVariableNumber)
                        {
                            case 1:
                                Vars.method_StandbyTemp = _PARM2;
                                _logStr1 = " method_StandbyTemp";
                                break;
                            case 2:
                                Vars.method_LowTemp = _PARM2;
                                _logStr1 = " method_LowTemp";
                                break;
                            case 3:
                                Vars.method_LowTime_PreInject = _PARM2;
                                _logStr1 = " method_LowTime_PreInject";
                                break;
                            case 4:
                                Vars.method_LowTime_PostInject = _PARM2;
                                _logStr1 = " method_LowTime_PostInject";
                                break;
                            case 5:
                                Vars.method_HighTemp = _PARM2;
                                _logStr1 = " method_HighTemp";
                                break;
                            case 6:
                                Vars.method_HighTemp_Time = _PARM2;
                                _logStr1 = " method_HighTemp_Time";

                                break;
                            case 7:
                                Vars.method_Prepare_2_Time = _PARM2;
                                _logStr1 = " method_Prepare_2_Time";
                                break;
                            case 8:
                                Vars.SINGLE_RUN = (_PARM2==1? true:false);
                                _logStr1 = " SingleRun mode";
                                break;                                
                            default:
                                break;
                        }

                        
                        _DebugAdd("_ParameterIsVariableNumber: " + _ParameterIsVariableNumber
                        + ", _ATOM: " + _atomStr
                        + ", _PARM1: " + _PARM1 + _logStr1
                        + ", _PARM2: " + _PARM2
                        + ", _PARM3: " + _PARM3
                        );
                        
                    }
                    else
                    {
                        appendMacroLine(0, _ATOM, _PARM1, _PARM2, _PARM3);
                        _DebugAdd("_ParameterIsNOTVariable"
                            + ", _ATOM " + _atomStr
                            + ", _PARM1 " + _PARM1
                            + ", _PARM2 " + _PARM2
                            + ", _PARM3 " + _PARM3
                            );
                        
                    }


                    
                    if (_ATOM == (int)ATOM.END_OF_CYCLE) 
                    { 
                        loadNextLine = false; 
                    };
                    
                }


                sr.Close();

            }


            catch (Exception e)
            {
                ErrorString = "MethFileLoad: " + e.Message;
                return false; 
            }


            // Save as Fallback if not run from MH
            if (!Vars.StartedFromMassHunter)
            {
                Vars.method_FullPathAndFilename_LastStandaloneMethod = Vars.method_FullPathAndFilename;
            }

            return true;
        }

        #endregion

        #region Save  Method / Macros



        private void writeAllMacroVars(StreamWriter sr)
        {
            for (int i = 1; i <= numVariables; i++ )
                sr.WriteLine(VariableName[i]
                    + ";" + VariableDefault[i]             // default Val not supported yet. We just set the current val as default
                    + ";" + VariableLowLimit[i]
                    + ";" + VariableHighLimit[i]
                    );
        }

        // Merge Macro Line with Variables (only for FileSave)
        public string mergeVarsIntoMacroLine(int _varNo, int _atom, int _parm1, double _parm2, int _parm3)
        {
            string str = "";

            try
            {
                switch (_atom)
                {
                    case (int)ATOM.SET_VARIABLE:
                        str = str
                            + _atomString[(int)ATOM.SET_VARIABLE]
                            + "("
                            + VariableName[_varNo]
                            + "," + Convert.ToString(_parm2)
                            + ")";
                        break;
                    case (int)ATOM.SET_TEMP:                    //  (Object, VarName OR Temp, exactness, ?)  has Variable Name
                        str = str
                            + _atomString[(int)ATOM.SET_TEMP]
                            + "(" + "eTrap"
                            + ",";

                        if (_varNo > 0) { str = str + VariableName[_varNo]; }
                        else { str = str + Convert.ToString(_parm1); }

                        str = str + "," + Convert.ToString(_parm2)
                        + ")";
                        break;
                    case (int)ATOM.WAIT_TIMER:                  // (TimerNo, Varname,?  )  has always Variable Name
                        str = str
                            + _atomString[(int)ATOM.WAIT_TIMER]
                            + "(" + Convert.ToString(_parm1)
                            + ",";
                        if (_varNo > 0) { str = str + VariableName[_varNo]; }
                        else { str = str + Convert.ToString(_parm2); }
                        str = str + ","
                        + ")"; ;
                        break;

                    case (int)ATOM.START_TIMER:                 // (TimerNo,? )
                        str = str
                            + _atomString[(int)ATOM.START_TIMER]
                            + "(" + Convert.ToString(_parm1)
                            + "," + Convert.ToString(_parm2)
                            + ")"; ;
                        break;

                    case (int)ATOM.SWITCH_EVENT:                // (Object, State, PulseTime )
                        str = str
                            + _atomString[(int)ATOM.SWITCH_EVENT]
                                         + "(" + Vars._parm1String[(int)_parm1]
                                         + "," + Vars.convertToTTL((int)_parm2)
                                         + "," + Convert.ToString(_parm3)
                                         + ")";
                        break;
                    case (int)ATOM.WAIT_SYNC_SIG:               // (Object, State )    CTC: (Object, ?)
                        str = str
                            + _atomString[(int)ATOM.WAIT_SYNC_SIG]
                                         + "(" + Vars._parm1String[(int)_parm1]
                                         + "," + Vars.convertToTTL((int)_parm2)
                                         + ")";
                        break;
                    case (int)ATOM.END_OF_CYCLE:                // (Object)            CTC: 
                        str = str
                            + _atomString[(int)ATOM.END_OF_CYCLE]
                                         + "(" + VariableName[_varNo]
                                         + ")";
                        break;

                    default:
                        str = "error";
                        break;
                };

                return str;

            }

            catch (Exception ex)
            {
                ErrorString = "mergeVarsIntoMacroLine: Atom:" + _atomString[_atom] 
                    + " _parm1:"  + _parm1 
                    + ", _parm2:" + _parm2 
                    + ", _parm3:" + _parm3 
                    + ",   " + ex.Message;

                return "";
            }

        }

        private void writeAllMacroLines(StreamWriter sw)
        {
            //currentVariable = 0;
            for (int i = 0; i < numMacroLines; i++)
            {   // Merge Macro Line with Variables (only for FileSave)
                var str = mergeVarsIntoMacroLine(macVarNo[i],  macAtom[i], macParm1[i], macParm2[i], macParm3[i]);
                sw.WriteLine(str);
            }
        }

        public bool Do_SaveMacroFile()
        {
            try
            {   // Macrofile = *.cma
                if (Vars.StartedFromMassHunter)
                    Vars.macro_FileName = Vars.method_Path + Vars.method_ShortName + ".m\\" + Vars.macro_Name + ".cma";
                else
                    Vars.macro_FileName = Vars.method_Path + Vars.macro_Name + ".cma";

                _DebugAdd("dataLogging:Vars.method_Path: " + Vars.method_Path);
                _DebugAdd("dataLogging:Vars.method_Path: " + Vars.macro_Name);

                FileStream myStream = new FileStream(Vars.macro_FileName, FileMode.Create);
                StreamWriter sw = new StreamWriter(myStream);

                _DebugAdd("dataLogging:Do_SaveMacroFile(): FileStream open done with: " + Vars.macro_FileName);

                // Macro Name:
                sw.WriteLine("\\\\" + Vars.macro_Name);
                _DebugAdd("dataLogging:Do_SaveMacroFile(): WriteLine done with macro file name: " + Vars.macro_FileName);

                // USER COMMENT AREA undefined number of lines
                // unused now

                // Write all Macro Variables with name, def, low, high
                writeAllMacroVars(sw);
                _DebugAdd("dataLogging:Do_SaveMacroFile(): writeAllMacroVars done ");

                // MACRO itself
                writeAllMacroLines(sw);
                _DebugAdd("dataLogging:Do_SaveMacroFile(): writeAllMacroLines done ");

                sw.Close();
                _DebugAdd("dataLogging:Do_SaveMacroFile(): Close file done ");


            }
            catch (Exception e) {ErrorString= "MacroFileSave: " + e.Message; return false; }

            return true;

        }

        //2.5.1 ok
        private void writeMacroCallingLineInMethod(StreamWriter sw)
        {
            sw.WriteLine(Vars.macro_Name + "("
                            + Vars.method_StandbyTemp + ","
                            + Vars.method_LowTemp + ","
                            + Vars.method_LowTime_PreInject + ","
                            + Vars.method_LowTime_PostInject + ","
                            + Vars.method_HighTemp + ","
                            + Vars.method_HighTemp_Time + ","
                            + Vars.method_Prepare_2_Time + ","
                            + Convert.ToInt16 (Vars.SINGLE_RUN) + ","
                            + Convert.ToInt16 (Vars.eTrap_CryoMode)
                            + ")");
        }

        //2.5.1 ok
        private void writeMethodParmsForAcqMeth(StreamWriter sw)
        {
            sw.WriteLine("                eTRAP Macro name:              " + Vars.macro_Name );
            sw.WriteLine(" ");
            if (Vars.eTrap_CryoMode == false)
            {   sw.WriteLine("                   " + "Transfer Line Mode");
                sw.WriteLine("                   " + "Const Temperature (C)" + ":    " + Vars.method_StandbyTemp + "");
                sw.WriteLine("   ");
                sw.WriteLine("                   " + "Firmware Version" + ":         " + Vars.FirmwareVersionString);
                sw.WriteLine("                   " + "Driver Version" + ":             " + Vars.SoftwareVersionString);
            }
            else
            {   sw.WriteLine("                   " + "Cryo Trap Mode");
                sw.WriteLine("                   " + VariableName[1] + ":    " + Vars.method_StandbyTemp + "");
                sw.WriteLine("                   " + VariableName[2] + ":   " + Vars.method_LowTemp + "");
                sw.WriteLine("                   " + VariableName[3] + ":    " + Vars.method_LowTime_PreInject + "");
                sw.WriteLine("                   " + VariableName[4] + ":   " + Vars.method_LowTime_PostInject + "");
                sw.WriteLine("                   " + VariableName[5] + ":  " + Vars.method_HighTemp + "");
                sw.WriteLine("                   " + VariableName[6] + ":    " + Vars.method_HighTemp_Time + "");
                sw.WriteLine("   ");
                sw.WriteLine("                   " + "Number of Trap Cycles" + ":      " + Vars.TRAP_CYCLES + "");
                sw.WriteLine("                   " + "Firmware Version" + ":           " + Vars.FirmwareVersionString);
                sw.WriteLine("                   " + "Driver Version" + ":             " + Vars.SoftwareVersionString);
            }
        }

        public bool Do_SaveMethodFile()     // 2.8.1 ok
        {
            try
            {
                /// CT MethFile = *.qxm  usually in ChemStation Folder (or in Home Folder)
                FileStream myStream = new FileStream(Vars.method_FullPathAndFilename, FileMode.Create);
                StreamWriter sw = new StreamWriter(myStream);

                _DebugAdd("dataLogging:Do_SaveMethodFile(): Name: " + Vars.method_FullPathAndFilename);

                sw.WriteLine("\\\\" + Vars.method_ShortName);
                // USER COMMENT AREA undefined number of lines starting with //
                // unused now  
                _DebugAdd("dataLogging:Do_SaveMethodFile(): WriteLine done with: " + Vars.method_ShortName);

                // Macro(,,,) = Calling the MACRO  
                writeMacroCallingLineInMethod(sw);  //2.8.1 ok

                _DebugAdd("dataLogging:Do_SaveMethodFile(): writeMacroCallingLineInMethod done: ");

                // Macro Variables:
                sw.WriteLine("[MACRO " + Vars.macro_Name + "]");
                _DebugAdd("dataLogging:Do_SaveMethodFile(): WriteLine done with: " + Vars.macro_Name);

                writeAllMacroVars(sw);      // 2.8.1 ok
                _DebugAdd("dataLogging:Do_SaveMethodFile(): writeAllMacroVars done");

                // MACRO itself
                writeAllMacroLines(sw);     // 2.8.1 not needed? ok
                _DebugAdd("dataLogging:Do_SaveMethodFile(): writeAllMacroLines done");

                sw.Close();
                _DebugAdd("dataLogging:Do_SaveMethodFile(): file Close done");


                // Save as Fallback if not run from MH
                if (!Vars.StartedFromMassHunter)
                {
                    Vars.method_FullPathAndFilename_LastStandaloneMethod = Vars.method_FullPathAndFilename;
                    _DebugAdd("dataLogging:Do_SaveMethodFile(): Vars.StartedFromMassHunter not true");
                    //_DebugAdd("dataLogging:Do_SaveMethodFile(): Vars.method_FullPathAndFilename_LastStandaloneMethod = " + Vars.method_FullPathAndFilename_LastStandaloneMethod);

                    _DebugAdd("dataLogging:Do_SaveMethodFile(): Save Method inf  file");

                    myStream = new FileStream(Vars.method_Path + Vars.method_ShortName + ".inf", FileMode.Create);
                    sw = new StreamWriter(myStream);
                    writeMethodParmsForAcqMeth(sw);
                    sw.Close();
                }
                else
                {
                    _DebugAdd("dataLogging:Do_SaveMethodFile(): Vars.StartedFromMassHunter true >> save merge-file: " + Vars.HomePath + "currMethod.inf");

                    myStream = new FileStream(Vars.HomePath + "currMethod.inf", FileMode.Create);
                    sw = new StreamWriter(myStream);
                    writeMethodParmsForAcqMeth(sw);
                    sw.Close();
                }
            }


            catch (Exception e) {
                ErrorString= "MethFileSave  " +e.Message; 
                return false; 
            }

            return true;
        }
        #endregion

        #region  Building the Macros
        private void clearAllMacroArrays()
        {   for (int i = 0; i < macMaxLines; i++)
        {
          //  macParmName[i] = ""; macParmLowLimit[i] = 0; macParmHighLimit[i] = 0;

            macVarNo[i] = 0; macAtom[i] = 0; macParm1[i] = 0; macParm2[i] = 0; macParm3[i] = 0;

            VariableName[i] = ""; VariableDefault[i]=0; VariableLowLimit[i] = 0; VariableHighLimit[i] = 0;
            currentMacroLine = 0; numMacroLines = 0; currentVariable = 0;
            }
        }

        private void appendMacroLine(int variableNo, int atom, int parm1, int parm2, int parm3)
        {
            if (variableNo > 0)
            {
                macVarNo[currentMacroLine] = variableNo;
            }
            else
            {
                macVarNo[currentMacroLine] = 0;
            }

            macLineNo[currentMacroLine]          = currentMacroLine;

            macAtom[currentMacroLine]            = atom;
            macParm1[currentMacroLine]           = parm1;
            macParm2[currentMacroLine]           = parm2;
            macParm3[currentMacroLine]           = parm3;

            currentMacroLine++;
            numMacroLines = currentMacroLine;
        }


        public void appendMacVariable(string varname, int def, int min, int max)
        {
            currentVariable++;
            numVariables = currentVariable; 
            VariableName[currentVariable]        = varname;
            VariableDefault[currentVariable]     = def;
            VariableLowLimit[currentVariable]    = min; 
            VariableHighLimit[currentVariable]   = max;                                  
        }
        #endregion

        // CREATE  eTrap C Y C L E 
        public void Do_CreateTrapCycle()
        {
            try
            {
                clearAllMacroArrays();

                // moved to Do_SetCriticalVars:  
                //Vars.macro_Name = "eTRAP";

                // set up the Variables
                numVariables = 0;
                currentVariable = 0;
                appendMacVariable("Standby Temperature (C)", 30, 15, 50);       // 1
                appendMacVariable("Trap Low Temperature (C)", 20, -40, 40);     // 2
                appendMacVariable("Trap PreInject Time (s)", 10, 0, 180);
                appendMacVariable("Trap PostInject Time (s)", 10, 10, 300);
                appendMacVariable("Trap High Temperature (C)", 100, 0, 300);
                appendMacVariable("Trap High Temp Time (s)", 30, 30, 1200);
                appendMacVariable("Prepare 2 Time (s)", 30, 0, 9999);     // 9999 = unused
                appendMacVariable("Single Run Mode", 1, 0, 1);                  // true/false
                appendMacVariable("eTrap Used", 1, 0, 1);               // 2.8.1


                // set up the MAcro  (Parameter 1 = Variable Number !)
                currentMacroLine = 0;

                //              VarNo.  Atom                    Parm1                       Parm2                           Parm3
                appendMacroLine(7, (int)ATOM.SET_VARIABLE, (int)PARM1.PREPARE_2_TIME, Vars.method_Prepare_2_Time, 0);
                appendMacroLine(0, (int)ATOM.SWITCH_EVENT, (int)PARM1.TRAP_COOL, 0, 0);     // Security

                appendMacroLine(1, (int)ATOM.SET_TEMP, Vars.method_StandbyTemp, 99, 0);
                if (Vars.fakeSyncSignals)
                    appendMacroLine(0, (int)ATOM.WAIT_TIMER, 4, 10, 0);
                else
                    appendMacroLine(0, (int)ATOM.WAIT_SYNC_SIG, (int)PARM1.GC_PREPARE, 0, 0);
                
                // V 4.0 Macro changed order
                appendMacroLine(0, (int)ATOM.WAIT_SYNC_SIG, (int)PARM1.PREPARE_2, 0, 0);
                appendMacroLine(0, (int)ATOM.SWITCH_EVENT, (int)PARM1.GC_READY, 0, 0);
                appendMacroLine(1, (int)ATOM.SET_TEMP, Vars.method_StandbyTemp, 1, 0);                

                appendMacroLine(0, (int)ATOM.SWITCH_EVENT, (int)PARM1.TRAP_COOL, 1, 0);
                appendMacroLine(2, (int)ATOM.SET_TEMP, Vars.method_LowTemp, 1, 0);
                appendMacroLine(3, (int)ATOM.WAIT_TIMER, 0, Vars.method_LowTime_PreInject, 0);
                appendMacroLine(0, (int)ATOM.SWITCH_EVENT, (int)PARM1.GC_READY, 1, 0);

                if (Vars.fakeSyncSignals)
                    appendMacroLine(0, (int)ATOM.WAIT_TIMER, 4, 10, 0);
                else
                    appendMacroLine(0, (int)ATOM.WAIT_SYNC_SIG, (int)PARM1.GC_START, 0, 0);

                appendMacroLine(4, (int)ATOM.WAIT_TIMER, 0, Vars.method_LowTime_PostInject, 0);
                appendMacroLine(0, (int)ATOM.SWITCH_EVENT, (int)PARM1.TRAP_COOL, 0, 0);

                appendMacroLine(5, (int)ATOM.SET_TEMP, Vars.method_HighTemp, 1, 0);                 // V 3.0 exchange Position of Start Timer / Set Temp
                appendMacroLine(0, (int)ATOM.START_TIMER, 1, 0, 0);                

                appendMacroLine(6, (int)ATOM.WAIT_TIMER, 1, Vars.method_HighTemp_Time, 0);
                appendMacroLine(8, (int)ATOM.END_OF_CYCLE, (int)PARM1.SINGLE_RUN_MODE, 0, 0);
            }
            catch (Exception ex) {ErrorString="setupMacro_eTRAP"+ ex.Message;
            }

        }


        #region Sending MACRO and METHOD to Controller
        // send Macro + Method to Controller 
        public void Do_SendVariablesToControler()
        {   Do_SendParms((int)Parameter.changeVariables);
        }

        #endregion

        public void Do_CreateStartupMethod()
        {   Do_SendParms((int)Parameter.cycleToEEprom);
        }


        private bool testFileOpen(string fileName, string calledFrom)
        {
            try
            {
                FileStream myStream = new FileStream(fileName, FileMode.Open);
                myStream.Close();
                return true;
            }
            catch (Exception e)
            {
                ErrorString=calledFrom + e.Message;
                return false;
            }
        }



        


    }




}
