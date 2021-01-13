using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization;


namespace DataLogging
{
    [Serializable]
    public class TestException : ApplicationException
    {
        public TestException(string Message,
                     Exception innerException)
            : base(Message, innerException) { }
        public TestException(string Message) : base(Message) { }
        public TestException() { }

        #region Serializeable Code
        public TestException(SerializationInfo info,
              StreamingContext context)
            : base(info, context) { }
        #endregion Serializeable Code
    }

    public partial class COM_and_Request : INotifyPropertyChanged
    {
        public COM_and_Request(){}

        public COM_and_Request(string _comfile)
        {
            _COMFile = _comfile;
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

        /// <summary>
        /// DEBUG = true
        /// Use _debugAdd to append a string to the DebugString Collection.
        /// The FORM may read out all the Debug Strings and delete them
        /// </summary>
        private const bool DEBUG = true;
        private StringCollection _debugstring = new StringCollection();
        public StringCollection  DebugString
        {
            get { return _debugstring; }
            set
            {
                _debugstring = value;
            }
        }

        private void _DebugAdd(string s)
        {
            if (DEBUG) { DebugString.Add("COM: " + s); }
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

        private void setRequestString()
        {
            switch (_currentRequest)
            {
                case NO_REQUEST:    _requeststring = "REQUEST NO"; break;
                case START:         _requeststring = "REQUEST START"; break;
                case STOP:          _requeststring = "REQUEST STOP"; break;
                case LOADMETHOD:    _requeststring = "REQUEST LOAD"; break;
                case SAVEMETHOD:    _requeststring = "REQUEST SAVE"; break;
                case EDITMETHOD:    _requeststring = "REQUEST EDITMETHOD"; break;
                case SHUTDOWN:      _requeststring = "REQUEST SHUTDOWN"; break;
                case MAINTENANCE:   _requeststring = "REQUEST MAINTENANCE"; break;
                case ERROR:         _requeststring = "REQUEST ERROR"; break;

                default:            _requeststring = "REQUEST UNKNOWN"; break;
            }
        }

        private int _lastRequest = 0 ;
        public int Last_Request
        {
            get { return _lastRequest; }
            set
            {
                _lastRequest = value;
            }
        }

        private int _currentRequest = NO_REQUEST ;
        public int CurrentRequest
        {
            get { return _currentRequest; }
            set
            {
                _currentRequest = value;
                setRequestString();
            }
        }

        private int _currentRequest_IntParm = 0;
        public int CurrentRequest_IntParm
        {
            get { return _currentRequest_IntParm; }
            set
            {
                _currentRequest_IntParm = value;
            }
        }
        private string _currentRequest_StringParm = "";
        public string CurrentRequest_StringParm
        {
            get { return _currentRequest_StringParm; }
            set
            {
                _currentRequest_StringParm = value;
            }
        }

        private string _requeststring = "";
        public string RequestString
        {
            get { return _requeststring; }
            set
            {
                _requeststring = value;
            }
        }

        private string _errorstring = "";
        public string ErrorString
        {
            get { return _errorstring; }
            set
            {
                _errorstring = value;
            }
        }

        private string _COMFile = "";
        public string COMFile
        {
            get { return _COMFile; }
            set
            {
                _COMFile = value;
                NotifyPropertyChanged("COMFILE");
            }
        }

        private string _ParmFile = "";
        public string ParmFile
        {
            get { return _ParmFile; }
            set
            {
                _ParmFile = value;
                NotifyPropertyChanged("ParmFILE");
            }
        }



        /// <summary>
        /// Tells MassHunter or any other SW that Startup is done
        /// by writing file "c:\Chromtech\eTrap\eTrap_startupDone.txt"
        /// </summary>
        private void Do_SignalStartupDone(string _homepath)
        {
            FileStream myStream = new FileStream(_homepath + "eTrap_startupDone.txt", FileMode.Create);
            StreamWriter sw = new StreamWriter(myStream);
            sw.WriteLine("startup done");
            sw.Close();
            _DebugAdd("Do_SignalStartupDone: wrote " + _homepath + "eTrap_startupDone.txt" );
        }


        public bool Do_CheckAndLoadMassHunter(string _homePath)
        {
            // Check if INITIAL MH COM File is present and change over to MH mode, if needed
            COMFile = _homePath + "eTrapCOM_0.txt";
            Do_CheckCOM();

            if (StartedFromMassHunter)
            {
                COMFile = _homePath + "eTrapCOM_" + CurrentRequest_IntParm.ToString() + ".txt";
                ParmFile = _homePath + "eTrapMHParms_" + CurrentRequest_IntParm.ToString() + ".txt";
                _DebugAdd ("Do_CheckAndLoadMassHunter: COMFile="+COMFile + "  ParmFile="+ParmFile + "  starting Do_CheckMHParms and Do_SignalStartupDone");

                Do_CheckMHParms();
                Do_SignalStartupDone(_homePath);                

                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// Check COMfile exists, loads and sets 'StartFromMassHunter' sets the REQUEST
        /// </summary>
        public void Do_CheckCOM()
        {
            // Check eTrap in use  Status set from MassHunter
            
            // Check COM to MassHunter
            try
            {
                if (!File.Exists(_COMFile)) { return; }
                else
                {
                    string oneLine = "";
                    try
                    {
                        StreamReader sr = new StreamReader(_COMFile);
                        while (sr.Peek() != -1)
                        {
                            oneLine = sr.ReadLine().ToUpper();
                            if (oneLine.Length > 0)
                            {
                                switch (oneLine)
                                {
                                    case "SHUTDOWN":
                                        CurrentRequest = SHUTDOWN;
                                        CurrentRequest_StringParm = "";
                                        CurrentRequest_IntParm = 0;
                                        _DebugAdd ("Do_CheckCOM: Found SHUTDOWN");
                                        break;
                                    case "1": CurrentRequest_IntParm = 1;
                                        StartedFromMassHunter = true;
                                       _DebugAdd ( "Do_CheckCOM: Found 1, Set StartedFromMassHunter=true");
                                        break;
                                    case "2": CurrentRequest_IntParm = 2;
                                        StartedFromMassHunter = true;
                                        _DebugAdd("Do_CheckCOM: Found 2, Set StartedFromMassHunter=true");
                                        break;
                                    case "3": CurrentRequest_IntParm = 3;
                                        StartedFromMassHunter = true;
                                        _DebugAdd("Do_CheckCOM: Found 3, Set StartedFromMassHunter=true"); 
                                        break;
                                    case "4": CurrentRequest_IntParm = 4;
                                        StartedFromMassHunter = true;
                                        _DebugAdd("Do_CheckCOM: Found 4, Set StartedFromMassHunter=true"); 
                                        break;

                                    case "DOINJECTION":
                                        CurrentRequest = START;
                                        CurrentRequest_StringParm = "";
                                        CurrentRequest_IntParm = 0;
                                        _DebugAdd("Do_CheckCOM: Found DOINJECTION");
                                        break;
                                    case "STOP":
                                        CurrentRequest = STOP;
                                        _DebugAdd("Do_CheckCOM: Found STOP");
                                        break;
                                    case "LOADMETHOD":
                                        CurrentRequest = LOADMETHOD;
                                        _DebugAdd("Do_CheckCOM: Found LOADMETHOD ");
                                        break;
                                    case "SAVEMETHOD":
                                        CurrentRequest = SAVEMETHOD;
                                        _DebugAdd("Do_CheckCOM: Found SAVEMETHOD " + "CurrentRequest= " + CurrentRequest + "_requestString=" + _requeststring );
                                        break;
                                    case "EDITMETHOD":
                                        CurrentRequest = EDITMETHOD;
                                        _DebugAdd("Do_CheckCOM: Found EDITMETHOD" );
                                        break;
                                    case "MAINTENANCE":
                                        CurrentRequest = MAINTENANCE;
                                        _DebugAdd("Do_CheckCOM: Found MAINTENANCE");
                                        break;
                                    default:
                                        break;
                                }
                            }

                        }
                        sr.Close();

                        // Delete NOT if EDITMETHOD
                        if (CurrentRequest != EDITMETHOD)
                            File.Delete(_COMFile);
                        
                    }
                    catch (TestException ex)
                    {
                        ErrorString = "Do_CheckCOM: " + ex.Message;
                        CurrentRequest = ERROR;
                        File.Delete(_COMFile);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorString = "Do_CheckCOM: " + ex.Message;
                CurrentRequest = ERROR;
                File.Delete(_COMFile);
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


        private string _MHMethFile = "";
        public string MHMethFile
        {
            get { return _MHMethFile; }
            set
            {
                _MHMethFile = value;
            }
        }

        private string _MHMethPath = "";
        public string MHMethPath
        {
            get { return _MHMethPath; }
            set
            {
                _MHMethPath = value;
            }
        }

        private string _MHInstName = "";
        public string MHInstName
        {
            get { return _MHInstName; }
            set
            {
                _MHInstName = value;
            }
        }


        /// <summary>
        /// Check MassHunter Parmfile exists, loads and sets the REQUEST
        /// </summary>
        public void Do_CheckMHParms()
        {
            string leftString = "";
            string rightString = "";

            try
            {
                if (!File.Exists(_ParmFile)) { return; }
                else
                {
                    _DebugAdd("Do_CheckMHParms: found " + _ParmFile);
                    string oneLine = "";
                    try
                    {
                        StreamReader sr = new StreamReader(_ParmFile);
                        while (sr.Peek() != -1)
                        {
                            oneLine = sr.ReadLine().ToUpper();
                            try
                            {
                                if (oneLine.IndexOf("=") > 0)
                                {
                                    leftString = oneLine.Substring(0, oneLine.IndexOf("="));
                                    rightString = oneLine.Substring(oneLine.IndexOf("=") + 1, (oneLine.Length) - (oneLine.IndexOf("=") + 1));
                                    _DebugAdd("Do_CheckMHParms: found " + leftString + "=" + rightString );
                                }
                                switch (leftString)
                                {
                                    case "_METHPATH$": 
                                        _MHMethPath = rightString;
                                        _DebugAdd("Do_CheckMHParms: _MHMethPath " + _MHMethPath);
                                        break;
                                    case "_METHFILE$": 
                                        _MHMethFile = rightString;
                                        _DebugAdd("Do_CheckMHParms: _MHMethFile " + _MHMethFile);
                                        break;
                                    case "_INSTNAME$": 
                                        _MHInstName = rightString;
                                        _DebugAdd("Do_CheckMHParms: _MHInstName " + _MHInstName);
                                        break;
                                    default: break;
                                }
                            }
                            catch (TestException ex)
                            {
                                ErrorString = "Do_CheckMHParms: " + ex.Message;
                                CurrentRequest = ERROR;
                            }
                        }
                        sr.Close();
                        //File.Delete(_ParmFile);
                        }
                    catch (TestException ex)
                    {
                        ErrorString = "Do_CheckMHParms: " + ex.Message;
                        CurrentRequest = ERROR;
                    }


                }
            }
            catch (Exception ex)
            {
                ErrorString = "Do_CheckCOM: " + ex.Message;
                CurrentRequest = ERROR;
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
