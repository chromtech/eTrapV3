using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using CommandMessenger;
using ZedGraph;
using MsgBox;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Specialized;



namespace DataLogging
{
    public partial class ChartForm : Form
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
            if (DEBUG) { DebugString.Add("_form: " + s); }
        }

        private bool IsDebugMode = true;

        // 2.7 Add Bruker Design option
        //private bool IsBrukerDesign = false;
        private Color tabsBackColor = ColorTranslator.FromHtml("#0071BC");   // Public for Do_DrawMethodLines()
        //

        public COM_and_Request COM = new COM_and_Request();        
        public DatenKlasse Vars = new DatenKlasse();



        // In a small C# application all code would typically end up in this class.
        // For a cleaner, MVP-like setup I moved higher logic to Datalogging.cs,        
        private readonly DataLogging DataLog;
        public long _previousChartUpdate;
        private IPointListEdit _analog1List;

        public string CRLF = Environment.NewLine;
        public string lastCycleString = "";


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

        public void sleep(Int32 millis)
        {
            System.Threading.Thread.Sleep(millis);
        }

        public ChartForm()
        {

            
            Splash.ShowSplashScreen();
            
            sleep(100);
            InitializeComponent();
            DataLog = new DataLogging();

            sleep(100);
            Splash.SetStatus("Init Communication.");
            sleep(100);
            COM = new COM_and_Request();
            DataLog.Do_Startup(this, Vars, COM);
            
            Splash.SetStatus("Init Timers.");
            sleep(100);
            Timer readback_Timer = new Timer();
            readback_Timer.Interval = 1000;
            readback_Timer.Start();
            readback_Timer.Tick += new EventHandler(AskForReadbacks);

            Timer REQUEST_Timer = new Timer();
            REQUEST_Timer.Interval = 500;
            REQUEST_Timer.Start();
            REQUEST_Timer.Tick += new EventHandler(DataLog.Do_WorkOnRequest);

            Timer ERROR_Timer = new Timer();
            REQUEST_Timer.Interval = 500;
            REQUEST_Timer.Start();
            REQUEST_Timer.Tick += new EventHandler(this.Do_ShowErrors);

            Timer DEBUG_Timer = new Timer();
            REQUEST_Timer.Interval = 500;
            REQUEST_Timer.Start();
            REQUEST_Timer.Tick += new EventHandler(this.Do_ShowDebug);

            Splash.CloseForm();
            sleep(1000);
        }

        

        private void Do_ShowErrors(object sender, EventArgs eArgs)
        {
            if (DataLog.ErrorString.Length > 0)
            {
                _showError("ERR DataLog: ", DataLog.ErrorString);
                DataLog.ErrorString = "";
            }

            if (COM.ErrorString.Length > 0)
            {
                _showError("ERR COM: ", COM.ErrorString);
                COM.ErrorString = "";
            }
            if (this.ErrorString.Length > 0)
            {
                _showError("ERR Form: ", this.ErrorString);
                this.ErrorString = "";
            }


            if ((!Vars.eTrapIsConnected) && (Vars.TimeSinceStartup > 60))
            {
                lbl_eTrapNotConnected.BackColor = Color.Red;
                lbl_eTrapNotConnected.Text = "eTrap disconnected or switched off! 1. Close software.  2. Verify eTrap is ON.  3. Restart software. ";
                lbl_eTrapNotConnected.Visible = true;  
            }

            /// After SW Start, we see a green Message if connected (for 4 sec)
            /// and a red message if not connected
            /// red Not Connected Message will be forever in MH mode
            /// and will go away after 30 sec in standalone mode
            if (Vars.eTrapIsConnected)
            {
                // If Connected Show Success for 4 sec
                if (Vars.TimeSinceStartup < 5)
                {
                    lbl_eTrapNotConnected.BackColor = Color.LightGreen;
                    lbl_eTrapNotConnected.Text = "Connected to eTrap.";
                    lbl_eTrapNotConnected.Visible = true;
                }
                else
                {
                    lbl_eTrapNotConnected.Visible = false;
                }

                // if MassUnter: remove Form between 4 and 10 seconds after startup
                // NOT later, cause this will interfere with other functions
                if (Vars.StartedFromMassHunter)
                {   if (Vars.TimeSinceStartup < 10 && Vars.TimeSinceStartup>4)
                    {
                        this.Visible = false;
                    }
                }
            }
            else
            {
                    lbl_eTrapNotConnected.BackColor = Color.Red;
                    lbl_eTrapNotConnected.Text = "1. Please close the software.  2. Verify eTrap is ON.  3. Restart software.";
                    lbl_eTrapNotConnected.Visible = true;     
            }



        }





        public void Do_ShowDebug(object sender, EventArgs eArgs)
        {
            while (this.DebugString.Count > 0)
            {
                if (IsDebugMode)
                appendTopLevelLogString(this.DebugString[this.DebugString.Count - 1]);

                this.DebugString.RemoveAt(this.DebugString.Count - 1);
            }

            while (Vars.DebugString.Count > 0)
            {
                if (IsDebugMode)
                    appendTopLevelLogString(Vars.DebugString[Vars.DebugString.Count - 1]);
                Vars.DebugString.RemoveAt(Vars.DebugString.Count - 1);
            }

            while (  DataLog.DebugString.Count>0)
            {
                if (IsDebugMode)
                    appendTopLevelLogString(DataLog.DebugString[DataLog.DebugString.Count - 1]);
                DataLog.DebugString.RemoveAt(DataLog.DebugString.Count - 1);
            }
            while (COM.DebugString.Count > 0)
            {
                if (IsDebugMode)
                    appendTopLevelLogString(COM.DebugString[COM.DebugString.Count-1]);
                COM.DebugString.RemoveAt(COM.DebugString.Count - 1);
            }
        }



            
        private List<Control> _brukerGUI_GetAllControls(Control container, List<Control> list)
        {
            foreach (Control c in container.Controls)
            {

                if (c.Controls.Count > 0)
                    list = _brukerGUI_GetAllControls(c, list);
                else
                    list.Add(c);
            }

            return list;
        }
        private List<Control> _brukerGUI_GetAllControls(Control container)
        {
            return _brukerGUI_GetAllControls(container, new List<Control>());
        }
        

        // if is 'IsBrukerDesign'
        private void Do_BrukerGUI()
        {
            // change Form Height to use  the Bruker Blue Top Banner
            this.Size = new Size(1176,900);
            
            // change Font to Segoe UI
            {
            List<Control> allControls = _brukerGUI_GetAllControls(this);
                allControls.ForEach(k => k.Font = new System.Drawing.Font("Segoe UI", k.Font.Size) );
            }           
            
            // define colors
            Color BrukerBlue = ColorTranslator.FromHtml("#0071BC");
            Color BrukerBlueDark = ColorTranslator.FromHtml("#00497F");
            Color BrukeWaterGreyK50 = ColorTranslator.FromHtml("#748A96");
            Color BrukerWaterGreyK20 = ColorTranslator.FromHtml("#BACAD3");
            Color BrukerWaterGrayK10 = ColorTranslator.FromHtml("#D8E2E8");

            // Create blue Top Banner with Bruker Logo
            pictureBox1.Visible = true;
            pictureBox1.BackColor = BrukerBlueDark;
            pictureBox1.BringToFront();
            textBox1.Location = new Point(-7, 0);
            textBox1.Size = new Size(1177, 43);
            textBox1.Visible = true;
            textBox1.BackColor = BrukerBlueDark;

            tabsBackColor = BrukerWaterGrayK10;  // First Tab Backgrounds:
            Color methodInputBackColor = BrukerWaterGreyK20;

            // Tabs Backgrounds:
            this.BackColor = tabsBackColor;
            tabMethod.BackColor = tabsBackColor;
            tabStatus.BackColor = tabsBackColor;
            tabCommunication.BackColor = tabsBackColor;


            // first Tab Input Boxen
            tBxCoolTemperature.BackColor = methodInputBackColor;
            tBxHighTemperature.BackColor = methodInputBackColor;
            tBxHighTempTime.BackColor = methodInputBackColor;
            tBxPrepare2Time.BackColor = methodInputBackColor;
            tBxPostInjTime.BackColor = methodInputBackColor;
            tBxPreInjTime.BackColor = methodInputBackColor;
            tBxStandbyTemp.BackColor = methodInputBackColor;

            // Display and Method Page
            eTrapDisplayBox.Text = "PeakTrap Display";
            groupBox1.Text = "PeakTrap   Method             (default)";
            label12.Text = "PeakTrap Method:";
            lbl_eTrapNotConnected.Text = "1. Please close the software.  2. Verify that PeakTrap is switched ON.  3. Restart the software.";
            this.Text = "Bruker PeakTrap";
            label25.Location = new Point(132, 44);


            // Setup on page 2: disable Prepare 2 Timer  for Bruker
            checkBox_UsePrepare2_Timer.CheckState = CheckState.Unchecked;
            checkBox_UsePrepare2_Timer.Enabled = false;
            checkBox_UsePrepare2_Timer.Visible = false;
            chkBx_IgnorePrepare_2.Enabled = true;
            tBxPrepare2Time.Visible = false;
            label22.Visible = false;
            label34.Visible = false;

            label20.Text = "PeakTrap Firmware";
            label23.Text = "PeakTrap Cycle";


        }


        private void Do_ChartForm_Load(object sender, EventArgs e)
        {
            
            try
            {
                if (Vars.IsBrukerDesign)
                    { Do_BrukerGUI(); }

                // ONLY FORM Settings here!
                // The real Startup is  DataLogging.DoStartup


                // Status Page
                textBox3.DataBindings.Add("Text", Vars, "TemperatureString");
                textBox2.DataBindings.Add("Text", Vars, "pid_string");
                labelTimerTime.DataBindings.Add("Text", Vars, "Timer1_TimeString");
                txtBox_TempLowCorrectionSlope.DataBindings.Add("Text", Vars, "tempLowCorrectionSlope");
                txtBox_TempMidCorrectionSlope.DataBindings.Add("Text", Vars, "tempMidCorrectionSlope");
                txtBox_TempHiCorrectionSlope.DataBindings.Add("Text", Vars, "tempHiCorrectionSlope");

                lbl_InstrumentCycles.DataBindings.Add("Text",Vars, "INSTRUMENT_CYCLES");

                txtBox_TTL.DataBindings.Add("Text", Vars, "TTLString");
                txtBox_TTL_Values.DataBindings.Add("Text", Vars, "TTLStringVal");
                txtBox_CycleTimers.DataBindings.Add("Text", Vars, "CTimerString");

                label1.DataBindings.Add("Text", Vars, "Comport");
                lbl_ControlerStatus.DataBindings.Add("Text", Vars, "ControlerStatusString");
                if (Vars.SINGLE_RUN == true)                
                    chkBx_SingleRun.CheckState = CheckState.Checked;                
                else                
                    chkBx_SingleRun.CheckState = CheckState.Unchecked;

                if (Vars.ignore_Prepare_2 == true)
                    chkBx_IgnorePrepare_2.CheckState = CheckState.Checked;
                else
                    chkBx_IgnorePrepare_2.CheckState = CheckState.Unchecked;

                if (Vars.method_Prepare_2_Time == 9999)
                {
                    checkBox_UsePrepare2_Timer.CheckState = CheckState.Unchecked;
                    chkBx_IgnorePrepare_2.Enabled = true;
                }
                else
                {
                    checkBox_UsePrepare2_Timer.CheckState = CheckState.Checked;
                    chkBx_IgnorePrepare_2.Enabled = false;
                }

                // Method Page  Graphic
                lbl_SingleRun.DataBindings.Add("Text", Vars, "SINGLE_RUN_string");
                lbl_CurrentTemperature.DataBindings.Add("Text", Vars, "currentTempForDisplay");
                lbl_TimerTime.DataBindings.Add("Text", Vars, "RunningTimerString");
                lbl_cryoTimeout.DataBindings.Add("Text", Vars, "CryoTimeoutString");
                lbl_TrapCycles.DataBindings.Add("Text", Vars, "TRAP_CYCLES");

                lbl_M_StandbyTemp.DataBindings.Add("Text", Vars, "method_StandbyTemp");
                lbl_M_LowTemp.DataBindings.Add("Text", Vars, "method_LowTemp");
                lbl_M_LowTime_PreInject.DataBindings.Add("Text", Vars, "method_LowTime_PreInject");
                lbl_M_LowTime_PostInject.DataBindings.Add("Text", Vars, "method_LowTime_PostInject");
                lbl_M_HighTemp.DataBindings.Add("Text", Vars, "method_HighTemp");
                lbl_M_HighTemp_Time.DataBindings.Add("Text", Vars, "method_HighTemp_Time");

                // Table
                tBxStandbyTemp.DataBindings.Add("Text", Vars, "method_StandbyTemp");
                // 2.8.1
                chkInUse.DataBindings.Add("Checked", Vars, "eTrap_CryoMode");  
                tBxCoolTemperature.DataBindings.Add("Text", Vars, "method_LowTemp");
                tBxPreInjTime.DataBindings.Add("Text", Vars, "method_LowTime_PreInject");
                tBxPostInjTime.DataBindings.Add("Text", Vars, "method_LowTime_PostInject");
                tBxHighTemperature.DataBindings.Add("Text", Vars, "method_HighTemp");
                tBxHighTempTime.DataBindings.Add("Text", Vars, "method_HighTemp_Time");
                tBxPrepare2Time.DataBindings.Add("Text", Vars, "method_Prepare_2_Time");
                // Top
                lbl_MethodName.DataBindings.Add("Text", Vars, "method_ShortName");
                lbl_MethodFileName.DataBindings.Add("Text", Vars, "method_FullPathAndFilename");
                lbl_eTrapNotConnected.Visible = false;
                eTrapDisplayBox.DataBindings.Add("Text", Vars, "eTrapDisplayBoxString");
                // Buttons
                bttn_OK_tabMethod.Visible = false;


                // Communication Page
                lbl_Comport.DataBindings.Add("Text", Vars, "ComportString");
                lbl_FWVersion.DataBindings.Add("Text", Vars, "FirmwareVersionString");
                lbl_SWVersion.DataBindings.Add("Text", Vars, "SoftwareVersionString");
                lbl_FWReqMainVersion.DataBindings.Add("Text", Vars, "SoftwareMainVersionString");
                lbl_FWReqSubVersion.DataBindings.Add("Text", Vars, "SoftwareSubVersionString");

                chkBx_TopLevelComm.CheckState = CheckState.Checked;
                chkBx_LowLevelComm.CheckState = CheckState.Unchecked;

                _previousChartUpdate = TimeUtils.Millis;


                Do_UpdateCycleGraphic();
                Do_UpdateStatusLabels();
            }
            catch (Exception ex)
            {
                ErrorString = "Do_ChartForm_Load: " + ex.Message;
            }
        }

        public void AskForReadbacks(object sender, EventArgs eArgs)
        {
            DataLog.Do_GetReadbacks();
            Do_UpdateStatusLabels();
        }


        public void appendCycleString(string st)
        {
            if ((Vars.runningCycleLineString != null) & (Vars.runningCycleLineString != lastCycleString))
            {
                txt_Box_CycleLine.AppendText(Environment.NewLine + Vars.runningCycleLineString);
                lastCycleString = Vars.runningCycleLineString;
            }
        }

        public void appendToControlerCycleBox(string st)
        {
            txtBox_ControlerCycle.AppendText(st);
            txtBox_ControlerCycle.AppendText(Environment.NewLine);
        }

        public void appendLowLevelLogString(string st)
        {
            try
            {
                if (chkBx_LowLevelComm.CheckState == CheckState.Checked)
                {
                    txtBox_Communication.AppendText(st + Environment.NewLine);
                }
            }
            catch(Exception ex)
            {
                ErrorString = "appendLowLevelLogString: " + ex.Message;
            }
        }

        public void appendTopLevelLogString(string st)
        {
            try
            {
                if (chkBx_TopLevelComm.CheckState == CheckState.Checked)
                {
                    txtBox_Communication.AppendText(st + Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                ErrorString = "appendTopLevelLogString: " + ex.Message;
            }
        }

        public void appendLogString(string st)
        {
            try
            {
                txtBox_Communication.AppendText(st);
                txtBox_Communication.AppendText(Environment.NewLine);
            }
            catch (Exception ex)
            {
                ErrorString = "appendLogString: String=" + st + "  ERROR: " + ex.Message;
            }
        }

        // ------------------  CHARTING ROUTINES ---------------------

        // Set up the chart
        public void SetupChart()
        {
            // get a reference to the GraphPane
            var myPane = chartControl.GraphPane;

            // Set the Titles
            if (!Vars.IsBrukerDesign)
                myPane.Title.Text = "eTrap Temperature";
            else
                myPane.Title.Text = "PeakTrap Temperature";

            myPane.XAxis.Title.Text = "Time [sec]";
            myPane.YAxis.Title.Text = "Temperature [°C]";

            // Create data arrays for rolling points
            _analog1List = new RollingPointPairList(3000);
            //_analog2List = new RollingPointPairList(3000);

            // Create a smoothened red curve Temperature = Analog 1
            LineItem myCurve = myPane.AddCurve("Temperature", _analog1List, Color.Red, SymbolType.None);
            myCurve.Line.IsSmooth = true;
            myCurve.Line.SmoothTension = 0.05f;

            // Create a smoothened blue curve   Analog 2
            //LineItem myCurve2 = myPane.AddCurve("-", _analog2List, Color.Blue, SymbolType.None);

            //myCurve2.Line.IsSmooth = true;
            //myCurve2.Line.SmoothTension = 0.05f;
            // Tell ZedGraph to re-calculate the axes since the data have changed
            chartControl.AxisChange();
        }
        public void ClearGraph()
        {   
            chartControl.GraphPane.CurveList.Clear();
            chartControl.Invalidate();
        }

        public void UpdateGraph(double time, double analog1)
        {
            try
            {
                // set window width
                const double windowWidth = 100.0;       //30

                if (!TimeUtils.HasExpired(ref _previousChartUpdate, 1000)) return;

                // Add data points to the circular lists
                _analog1List.Add(time, analog1);

                // Console.WriteLine("Update chart");

                // get and update x-scale to scroll with data with an certain window
                var xScale = chartControl.GraphPane.XAxis.Scale;
                xScale.Max = time + xScale.MajorStep;
                xScale.Min = xScale.Max - windowWidth;

                var yScale = chartControl.GraphPane.YAxis.Scale;
                var oldmin = yScale.Min; var oldmax = yScale.Max;
                if ((analog1 - 3) < oldmin)
                    yScale.Min = analog1 - 3;

                if ((analog1 + 3) > oldmax)
                    yScale.Max = analog1 + 3;

                // Make sure the axes are rescaled to accommodate actual data
                chartControl.AxisChange();

                // Force a redraw
                chartControl.Invalidate();
            }
            catch(Exception ex)
            {
                ErrorString = "UpdateGraph: " + ex.Message;
            }
        }


        public void Do_WorkOnRequest_UpdateForm()
        {
            if (Vars.StartedFromMassHunter)
            {
                Do_WorkOnRequest_TabsHide();
                Do_WorkOnRequest_ButtonsHide();

                switch (COM.CurrentRequest)
                {
                    case EDITMETHOD:
                        this.Visible = true;
                        this.WindowState = System.Windows.Forms.FormWindowState.Normal;
                        break;
                    case MAINTENANCE:
                        this.Visible = true;
                        this.WindowState = System.Windows.Forms.FormWindowState.Normal;
                        break;
                    case START:
                        if (Vars.eTrap_CryoMode)
                        {
                            this.Visible = true;
                            this.WindowState = System.Windows.Forms.FormWindowState.Normal;
                        }
                        break;
                    default:
                        //this.Visible = true;
                        //this.WindowState = System.Windows.Forms.FormWindowState.Normal;
                        appendLogString("Do_WorkOnRequest_UpdateForm: switch default: " + COM.CurrentRequest.ToString());
                        break;
                }
            }
        }

        private void Do_WorkOnRequest_ButtonsHide()
        {
            if (Vars.StartedFromMassHunter)
            {
                switch (COM.CurrentRequest)
                {
                    case EDITMETHOD:
                        bttn_LoadMethod.Visible = false;
                        bttn_SaveMethod.Visible = false;
                        bttn_MethStart.Visible = false;
                        bttn_OK_tabMethod.Visible = true;
                        break;

                    case MAINTENANCE:
                        bttn_LoadMethod.Visible = true;
                        bttn_SaveMethod.Visible = true;
                        bttn_MethStart.Visible = true;
                        bttn_OK_tabMethod.Visible = false;
                        break;

                    case START:
                        bttn_LoadMethod.Visible = false;
                        bttn_SaveMethod.Visible = false;
                        bttn_MethStart.Visible = true;
                        bttn_OK_tabMethod.Visible = true;
                        break;

                    default:
                        break;
                }
            }
        }


        public void Do_UpdateButtons()
        {
            if (Vars.heaterIsOn == 0) bttn_heater.BackColor = Color.FromArgb(243, 250, 255);
            else                        bttn_heater.BackColor = Color.Red;

            if (Vars.Timer1_On == 1) buttonTimer1.BackColor = Color.Red;
            else buttonTimer1.BackColor = Color.FromArgb(243, 250, 255);

            if (Vars.CycleStepMode == true)
            {
                bttn_SwitchStepMode.BackColor = Color.Red;
                bttn_SwitchStepMode.Text = "Step mode is on";
                bttn_NextStep.Enabled = true;
            }
            else
            {
                bttn_SwitchStepMode.BackColor = Color.FromArgb(243, 250, 255);
                bttn_SwitchStepMode.Text = "Step mode is off";
                bttn_NextStep.Enabled = false;
            }
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            Vars.ConfigFileSave();
            DataLog.Do_StopCycle();
            DataLog.Do_CreateStartupMethod();
            
            if (disposing)
            {
                DataLog.Do_ExitSerialCOM(false);
                if (components != null)
                    components.Dispose();
            }
            base.Dispose(disposing);
        }




        //-------------------- BUTTON ROUTINES  ----------------------


        private string GetInput(string message, string title)
        {   InputBox.SetLanguage(InputBox.Language.English);    // always
            //Save the DialogResult as res
            DialogResult res = InputBox.ShowDialog(title,
                message,                                  //Text message (mandatory), Title (optional)
                InputBox.Icon.Question,                         //Set icon type (default info)
                InputBox.Buttons.OkCancel,                      //Set buttons (default ok)
                InputBox.Type.TextBox,                          //Set type (default nothing)
                null,                                           //String field as ComboBox items (default null)
                true,                                           //Set visible in taskbar (default false)
                new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular)); //Set font (default by system)

            if (res == System.Windows.Forms.DialogResult.OK || res == System.Windows.Forms.DialogResult.Yes)
                return InputBox.ResultValue;
            else
                return null;
        }

        private bool GetConfirm(string message, string title)
        {
            InputBox.SetLanguage(InputBox.Language.English);    // always
            //Save the DialogResult as res
            DialogResult res = InputBox.ShowDialog(title,
                message,                                  //Text message (mandatory), Title (optional)
                InputBox.Icon.Question,                         //Set icon type (default info)
                InputBox.Buttons.YesNo,                      //Set buttons (default ok)
                InputBox.Type.Nothing,                          //Set type (default nothing)
                null,                                           //String field as ComboBox items (default null)
                true,                                           //Set visible in taskbar (default false)
                new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular)); //Set font (default by system)

            if (res == System.Windows.Forms.DialogResult.OK || res == System.Windows.Forms.DialogResult.Yes)
                return true;
            else
                return false;
        }


        private void buttonTimer1_Click(object sender, EventArgs e)
        {   DataLog.SwitchTimer1();
            Do_UpdateButtons();
        }

        private void bttn_heater_Click(object sender, EventArgs e)
        {
            if (Vars.CycleRunning == 1)
                return;

            bool isOK;
            if (Vars.heaterIsOn==1)
                isOK = GetConfirm("FOR SERVICE PERSONNEL ONLY", "This may damage your eTrap!" + CRLF + CRLF + "Switch Heater OFF?");
            else
                isOK = GetConfirm("FOR SERVICE PERSONNEL ONLY", "This may damage your eTrap!" + CRLF + CRLF + "Switch Heater ON?");
            
            if (isOK)
            {
                DataLog.NoReadbacksPlease = true;
                sleep(500);
                DataLog.SwitchHeater();
                DataLog.NoReadbacksPlease = false;
                Do_UpdateButtons();
            }
        }

        private void bttn_Temperature_Click(object sender, EventArgs e)
        {
            var result = GetInput(  "New Temperature",
                                    "New temperature" + CRLF + "(" + Vars.pid_min_temperature + " to " + Vars.pid_max_temperature + ")");
            try
            {
                if (result != null)
                {
                    // will throw exception if string is entered
                    var check = Convert.ToInt32(result);
                    Vars.currentGoalTemp = check;
                    DataLog.SetAllTemperatures();
                }
            }
            catch (Exception ex)
            {
                ErrorString = "bttn_Temperature_Click: " + ex.Message;
            }      
        }




        private void lbl_M_StandbyTemp_Click(object sender, EventArgs e)
        {
        }

        private void lbl_M_LowTemp_Click(object sender, EventArgs e)
        {
        }

        private void lbl_M_HighTemp_Click(object sender, EventArgs e)
        {
        }

        private void lbl_M_LowTime_PreInject_Click(object sender, EventArgs e)
        {
        }

        private void lbl_M_LowTime_PostInject_Click(object sender, EventArgs e)
        {
        }

        private void lbl_M_HighTemp_Time_Click(object sender, EventArgs e)
        {
        }

  
        private void bttn_SwitchStepMode_Click(object sender, EventArgs e)
        {
            DataLog.SwitchCycleStepMode();
            Do_UpdateButtons();
        }

        private void bttn_NextStep_Click(object sender, EventArgs e)
        {
            DataLog.Do_CycleNextStep();
        }

        private void chkBx_IgnoreSyncSignals_CheckedChanged(object sender, EventArgs e)
        {
            // Also slocks the two Prepare checkBoxes. 
            // because in eTrap the settings will be overwritten by IgnoreSyncSignals
            DataLog.Do_StopCycle();
            switch (chkBx_IgnoreSyncSignals.CheckState)
            {
                case CheckState.Checked:
                    chkBx_IgnoreGCPrepare.Enabled = false;
                    chkBx_IgnorePrepare_2.Enabled = false;
                    checkBox_UsePrepare2_Timer.Enabled = false;
                    DataLog.SetFakeSyncSignals(true);
                    break;
                case CheckState.Unchecked:
                    chkBx_IgnoreGCPrepare.Enabled = true;
                    if (checkBox_UsePrepare2_Timer.CheckState == CheckState.Unchecked)
                        chkBx_IgnorePrepare_2.Enabled = true;

                    checkBox_UsePrepare2_Timer.Enabled = true;

                    DataLog.SetFakeSyncSignals(false);
                    break;
            }
            DataLog.Do_CreateTrapCycle();
            DataLog.Do_SendVariablesToControler();
        }

        private void chkBx_IgnoreGCPrepare_CheckedChanged(object sender, EventArgs e)
        {
            DataLog.Do_StopCycle();

            switch (chkBx_IgnoreGCPrepare.CheckState)
            {
                case CheckState.Checked:
                    DataLog.SetIgnoreGCPrepare(true);
                    break;
                case CheckState.Unchecked:
                    DataLog.SetIgnoreGCPrepare(false);
                    break;
            }            
            DataLog.Do_CreateTrapCycle();
            DataLog.Do_SendVariablesToControler();
            Vars.ConfigFileSave();
        }


        private void bttn_RestartCycle_Click(object sender, EventArgs e)
        {
            DataLog.Do_StartCycleForever();
        }


        private void bttn_SaveMethod_Click(object sender, EventArgs e)
        {
            // Displays an OpenFileDialog so the user can select a Cursor.
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Controller Method|*.CME";
            saveFileDialog1.Title = "Select a Method File";

            // Show the Dialog.
            // If the user clicked OK in the dialog and
            // a .cme file was selected, open it.
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Assign the cursor in the Stream to the Form's Cursor property.
                if ((saveFileDialog1.FileName != ""))
                {
                    Vars.method_FullPathAndFilename = saveFileDialog1.FileName;
                }

                // apply current Settings to Macro/Method
                DataLog.Do_CreateTrapCycle();
                DataLog.Do_SaveMacroFile();
                DataLog.Do_SaveMethodFile();

                Do_SetMethodIsSaved();
                
            }

        }


        private void bttn_LoadMethod_Click_1(object sender, EventArgs e)
        {
            // Displays an OpenFileDialog so the user can select a Cursor.
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Controller Method|*.CME";
            openFileDialog1.Title = "Select a Controller Method";

            // Show the Dialog.
            // If the user clicked OK in the dialog and
            // file was selected, open it.
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((openFileDialog1.FileName != ""))
                {
                    if (!DataLog.Do_LoadAndApplyMethod(openFileDialog1.FileName))
                    {
                        ErrorString= "bttn_LoadMethod_Click_1: " + DataLog.ErrorString;
                    }
                    else
                    {   Do_SetMethodIsSaved();
                        Do_SetMethodIsApplied();
                    }
                }
            }
        }


        private void chkBx_TopLevelComm_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void chkBx_LowLevelComm_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            txtBox_Communication.Clear();
        }


        private void label6_Click(object sender, EventArgs e)
        {
            lbl_M_StandbyTemp_Click(sender, e);
        }

        private void label7_Click(object sender, EventArgs e)
        {
            lbl_M_LowTemp_Click(sender, e);
        }

        private void label9_Click(object sender, EventArgs e)
        {
            lbl_M_LowTime_PreInject_Click(sender, e);
        }

        private void label10_Click(object sender, EventArgs e)
        {
            lbl_M_LowTime_PostInject_Click(sender, e);
        }

        private void label8_Click(object sender, EventArgs e)
        {
            lbl_M_HighTemp_Click(sender, e);
        }

        private void label11_Click(object sender, EventArgs e)
        {
            lbl_M_HighTemp_Time_Click(sender, e);
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            txtBox_ControlerCycle.Clear();
            DataLog.Do_ListControlerCycle();
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            txtBox_ControlerCycle.Clear();
        }


        private void textBox1_Enter(object sender, EventArgs e)
        {
            DataLog.NoReadbacksPlease = true;

            TextBox tBx = (TextBox)sender;
            switch (tBx.Name)
            {
                case "tBxPrepare2Time":
                    tBx.Text = Convert.ToString(Vars.method_Prepare_2_Time);
                    break;
                case "tBxCoolTemperature":
                    tBx.Text = Convert.ToString(Vars.method_LowTemp);
                    break;
                case "tBxStandbyTemp":
                    tBx.Text = Convert.ToString(Vars.method_StandbyTemp);
                    break;
                case "tBxPreInjTime":
                    tBx.Text = Convert.ToString(Vars.method_LowTime_PreInject);
                    break;
                case "tBxPostInjTime":
                    tBx.Text = Convert.ToString(Vars.method_LowTime_PostInject);
                    break;
                case "tBxHighTemperature":
                    tBx.Text = Convert.ToString(Vars.method_HighTemp);
                    break;
                case "tBxHighTempTime":
                    tBx.Text = Convert.ToString(Vars.method_HighTemp_Time);
                    break;
                default:
                    tBx.Text = "0";
                    break;
            }

            if (tBx.Text != String.Empty)
            {
                //textBox1.ForeColor = Color.Red;
                tBx.BackColor = Color.WhiteSmoke;
                // Move the selection pointer to the end of the text of the control.
                tBx.Select(0,tBx.Text.Length);
            }
        }



        private void textBox1_Leave(object sender, EventArgs e)
        {
            int i = 0;
            bool isOK = true;
            TextBox tBx = (TextBox)sender;

            try { i = Convert.ToInt16(tBx.Text); }
            catch (Exception ex)
            {
                ErrorString="textBox1_Leave: "+ ex.Message;
                isOK = false; }

            switch (tBx.Name)
            {
                case "tBxPrepare2Time":
                    if (isOK) Vars.method_Prepare_2_Time = i;
                    break;
                case "tBxStandbyTemp":
                    if (isOK) Vars.method_StandbyTemp = Vars.limit(i, (int)Vars.pid_min_temperature, (int)Vars.pid_max_temperature);
                    break;
                case "tBxCoolTemperature":
                    if (isOK)   Vars.method_LowTemp = Vars.limit(i, (int)Vars.pid_min_temperature_cool, (int)Vars.pid_max_temperature_cool) ;
                    break;
                case "tBxPreInjTime":
                if (isOK) Vars.method_LowTime_PreInject = i;
                    break;
                case "tBxPostInjTime":
                    if (isOK) Vars.method_LowTime_PostInject = i;
                    break;
                case "tBxHighTemperature":
                    if (isOK) Vars.method_HighTemp = Vars.limit(i, (int)Vars.pid_min_temperature, (int)Vars.pid_max_temperature);
                    break;
                case "tBxHighTempTime":
                    if (isOK) Vars.method_HighTemp_Time = i;
                    break;
                default:
                    tBx.Text = "0";
                    break;
            }

            if (isOK)
            {
                bttn_SendToTrap.Text = "Apply*";
                bttn_ViewMethod.Enabled = false ;
                bttn_SaveMethod.Text = "Save*";
                bttn_SaveMethod.Enabled = true;
            }

            // Reset the colors and selection of the TextBox after focus is lost.
           // textBox1.ForeColor = Color.Black;
            tBx.BackColor = Color.White;
            tBx.Select(0, 0);


            // remove Cursor
            tBx.Enabled = false;
            tBx.Enabled = true;

            DataLog.NoReadbacksPlease = false;
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            KeyEventArgs evnt = (KeyEventArgs)e;
            if (evnt.KeyCode == Keys.Enter)
                textBox1_Leave(sender, e);  //or any other control that has a focus, like button.Focus()

        }

        private void textBox1_Click(object sender, EventArgs e)
        {
        }



        // Graphic Lines
        private int[] standbyLine           = new int[] {  30, 260,  120, 260 };
        private int[] constantStandbyLine   = new int[] {  30, 260,  570, 260 };

        private int[] coolDownLine          = new int[] { 120, 260,  140, 350 };
        private int[] coolPreInjectLine     = new int[] { 140, 350,  265, 350 };
        private int[] coolPostInjectLine    = new int[] { 265, 350,  350, 350 };
        private int[] heatUpLine            = new int[] { 350, 350,  390, 100 };
        private int[] highTempLine          = new int[] { 390, 100,  600, 100 };

        private int[] syncPrepareLine       = new int[] { 120, 220, 120, 250 };
        private int[] syncInjectLine        = new int[] { 265, 220, 265, 340 };


        private void Do_DrawMethodLines(object sender, PaintEventArgs e)
        {
            System.Drawing.Graphics gr = this.groupBox3.CreateGraphics();
            //this.groupBox3.BackColor = Color.FromArgb(20, 165, 250);

            Pen penYellow = new Pen(System.Drawing.Color.Yellow, 3);
            Pen penGreen = new Pen(System.Drawing.Color.Green, 3);
            Pen penBlue = new Pen(System.Drawing.Color.Blue, 3);
            Pen penRed = new Pen(System.Drawing.Color.Red, 3);


            if (Vars.IsBrukerDesign)
            { gr.Clear(tabsBackColor); }
            else
            {   gr.Clear(Color.FromArgb(20, 165, 250));
            }

            if (Vars.CycleRunning > 0)
            {
                drawLine(standbyLine, gr, penYellow);
                drawLine(coolDownLine, gr, penYellow);
                drawLine(coolPreInjectLine, gr, penYellow);
                drawLine(coolPostInjectLine, gr, penYellow);
                drawLine(heatUpLine, gr, penYellow);
                drawLine(highTempLine, gr, penYellow);

                drawLine(syncPrepareLine, gr, penYellow);
                drawLine(syncInjectLine, gr, penYellow);

                switch (Vars.CycleLine)
                {
                    case 0:
                    case 1:
                    case 2: drawLine(standbyLine, gr, penBlue); break;

                    case 3: 
                    case 4:
                    case 5:
                    case 6: drawLine(syncPrepareLine, gr, penBlue); break;

                    case 7:                      
                    case 8: drawLine(coolDownLine, gr, penBlue); break;

                    case 9: 
                    case 10: drawLine(coolPreInjectLine, gr, penBlue); break;

                    case 11: drawLine(syncInjectLine, gr, penBlue); break;
                    case 12: drawLine(coolPostInjectLine, gr, penBlue); break;

                    case 13: 
                    case 14: 
                    case 15: drawLine(heatUpLine, gr, penRed);                        break;

                    case 16:
                    case 17: drawLine(highTempLine, gr, penRed); break;
                    default:  break;
                }
               
            }
            else
            {
                drawLine(constantStandbyLine, gr, penBlue);
            }
        }

        private void Do_UpdateStatusLabels()
        {
            if ((Vars.eTrap_CryoMode == false) || (Vars.eTrapIsConnected == false))
                lbl_SingleRun.ForeColor = Color.Red;
            else
                lbl_SingleRun.ForeColor = Color.Blue;


            if (Vars.currentTempForDisplay == "Failed")
                lbl_CurrentTemperature.ForeColor = Color.Red;
            else
                lbl_CurrentTemperature.ForeColor = Color.Black;


            if (Vars.ControlerStatusString.IndexOf("OFF") > 0)
                lbl_ControlerStatus.ForeColor = Color.Red;
            else
                lbl_ControlerStatus.ForeColor = Color.Black;

        }

        private void Do_UpdateCycleLabels(int state)
        {
            bool vis = (state > 0 ? true : false);

            lbl_M_HighTemp.Visible = vis;
            lbl_M_HighTemp_Time.Visible = vis;
            lbl_M_LowTemp.Visible = vis;
            lbl_M_LowTime_PreInject.Visible = vis;
            lbl_M_LowTime_PostInject.Visible = vis;

            label5.Visible = vis;
            label4.Visible = vis;
            label11.Visible = vis;
            label8.Visible = vis;
            label7.Visible = vis;
            label9.Visible = vis;
            label10.Visible = vis;

            if (vis)            { bttn_MethStart.Text = "Stop"; }
            else                { bttn_MethStart.Text = "Start";
                                    if (COM.StartedFromMassHunter)  { bttn_MethStart.Visible = false; }
                                }
        }

        private void drawLine(int[] points, Graphics gr, Pen pen)
        {   gr.DrawLine(pen, points[0], points[1], points[2], points[3]);
        }

        

        public void Do_UpdateCycleGraphic()
        {
            if (Vars.CycleRunning != Vars.CycleRunningLastState)
            {
                groupBox3.Refresh();
                Do_UpdateCycleLabels(Vars.CycleRunning);
                Vars.CycleRunningLastState = Vars.CycleRunning;
            }
        }
        private void chkBx_IgnoreCyroTimeout_CheckedChanged(object sender, EventArgs e)
        {           
            // Update Controller            
            DataLog.Do_StopCycle();
            // Acquire the state of the CheckBox.
            switch (chkBx_IgnoreCyroTimeout.CheckState)
            {
                case CheckState.Checked:
                    DataLog.SetCryoTimeout(true);
                    break;
                case CheckState.Unchecked:
                    DataLog.SetCryoTimeout(false);
                    break;
            }

            DataLog.Do_CreateTrapCycle();
            DataLog.Do_SendVariablesToControler();
            Vars.ConfigFileSave();
        }



        private void bttn_MethStart_Click(object sender, EventArgs e)
        {
            if (this.bttn_MethStart.Text.Contains("Start"))
            {
                DataLog.Do_StartCycleForever();
            }
            else
            {
                DataLog.Do_StopCycle();
            }
        }


        private void _showError(string senderName, string exceptionString)
        {
            try
            {
                string s = Environment.NewLine + senderName + ": " + exceptionString;
                textBox_Error1.Visible = Visible;
                textBox_Error1.AppendText(s);

                textBox_Error2.Visible = Visible;
                textBox_Error2.AppendText(s);

                textBox_Error3.Visible = Visible;
                textBox_Error3.AppendText(s);
            }
            catch (Exception ex) { ErrorString = "_showError: " + ex.Message; } 
        }



        private void bttn_ViewMethod_Click(object sender, EventArgs e)
        {
            string err = Vars.OpenMethodInEditor();
            if (err.Length > 0)
            {
                ErrorString = "bttn_ViewMethodClick: " + err;
            }
        }

        private void bttn_SendToTrap_Click(object sender, EventArgs e)
        {
            bool isOK = true;
            try
            {
                if (this.bttn_MethStart.Text.Contains("Start"))
                { isOK = GetConfirm("Apply Method to eTrap", "Apply Method?");
                }
                else
                { isOK = GetConfirm("Stop eTrap and Apply Method", "Stop and Apply Method?");
                }

                if (isOK) 
                {   DataLog.Do_SendToTrapAndMakeStartupMethod(); 
                }
                else 
                { return; }

            }
            catch (Exception ex) { ErrorString = "Button Apply Method: " + ex.Message; }          // Exception will do nothing
        }

        public void Do_SetMethodChanged()
        {   bttn_SendToTrap.Text = "Apply*";
            bttn_SaveMethod.Text = "Save*";
            bttn_SaveMethod.Enabled = true;
            bttn_ViewMethod.Enabled = false;
        }
        public void Do_SetMethodIsSaved()
        {
            bttn_SaveMethod.Text = "Save";
            bttn_ViewMethod.Enabled = true;
        }
        public void Do_SetMethodIsApplied()
        {
            bttn_SendToTrap.Text = "Apply";
        }
        public void Do_SetMethodNOTApplied()
        {
            bttn_SendToTrap.Text = "Apply*";
        }

        private void bttn_Communication_View_Click(object sender, EventArgs e)
        {
            string err = Vars.OpenTextBoxInEditor(txtBox_Communication, Vars.log_FileName);
            if (err.Length > 0)
            {
                ErrorString = "bttn_Communication_View_Click: " + err;
            }         

        }

        private void bttn_ViewControlerCycle_Click(object sender, EventArgs e)
        {
            string err = Vars.OpenTextBoxInEditor(txtBox_ControlerCycle,Vars.HomePath + "Log_eTrapCycle.txt");
            if (err.Length > 0)
            {
                ErrorString = "bttn_ViewControlerCycle_Click: " + err;
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

        /// <summary>
        /// Hides Tabs according to REQUEST MODE
        /// </summary>
        private void Do_WorkOnRequest_TabsHide()
        {
            if (Vars.StartedFromMassHunter)
            {
                switch (COM.CurrentRequest)
                {
                    case MAINTENANCE:
                        tabControl1.TabPages.Remove(tabMethod);
                        tabControl1.SelectedIndex = 0;
                        COM.Last_Request = COM.CurrentRequest;
                        break;
                    case EDITMETHOD:
                        tabControl1.TabPages.Remove(tabStatus);
                        tabControl1.TabPages.Remove(tabCommunication);
                        tabControl1.SelectedIndex = 0;
                        COM.Last_Request = COM.CurrentRequest;
                        break;
                    case START:
                        tabControl1.TabPages.Remove(tabStatus);
                        tabControl1.TabPages.Remove(tabCommunication);
                        tabControl1.SelectedIndex = 0;
                        COM.Last_Request = COM.CurrentRequest;
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// OK is only visible, in EDITMETHOD and MAINTENANCE when started from MH
        /// We hide window and add the removed tabs
        /// </summary>
        private void Do_OK_buttons()
        {
            if (Vars.StartedFromMassHunter)
            {
                switch (COM.Last_Request)
                {
                    case MAINTENANCE:
                        tabControl1.TabPages.Add(tabMethod);
                        tabControl1.SelectedIndex = 0;
                        break;
                    case EDITMETHOD:
                        tabControl1.TabPages.Add(tabStatus);
                        tabControl1.TabPages.Add(tabCommunication);
                        tabControl1.SelectedIndex = 0;

                        // Activate COM to MH again
                        File.Delete(COM.COMFile);
                        COM.CurrentRequest = NO_REQUEST;
                        break;

                    case START:
                        tabControl1.TabPages.Add(tabStatus);
                        tabControl1.TabPages.Add(tabCommunication);
                        tabControl1.SelectedIndex = 0;

                        // Activate COM to MH again
                        COM.CurrentRequest = NO_REQUEST;
                        break;

                    default:
                        break;
                }
                bttn_OK_tabMethod.Visible = false;
                this.Visible = false;
            }
        }

        private void bttn_OK_tabStatus_Click(object sender, EventArgs e)
        {
            Do_OK_buttons();
        }
        private void bttn_OK_tabMethod_Click(object sender, EventArgs e)
        {
            Do_OK_buttons();
        }

        private void chkBx_SingleRun_CheckedChanged(object sender, EventArgs e)
        {
            DataLog.Do_StopCycle();
            switch (chkBx_SingleRun.CheckState)
            {
                case CheckState.Checked:
                    DataLog.SetSingleRunMode(true);
                    break;
                case CheckState.Unchecked:
                    DataLog.SetSingleRunMode(false);
                    break;
            }            
            DataLog.Do_CreateTrapCycle();
            DataLog.Do_SendVariablesToControler();
            Vars.ConfigFileSave();
        }

        private void label19_Click(object sender, EventArgs e)
        {

        }

        private void label21_Click(object sender, EventArgs e)
        {

        }

        private void ChartForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            string result = "OK";
            if (Vars.autoRestart)
                Environment.Exit(0);


            if (Vars.StartedFromMassHunter==false)
            {   result = MessageBox.Show("Exit eTrap Driver (turn off heaters)?", " Exit eTrap Driver ", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2).ToString();
                if (result != "OK")
                {   e.Cancel = true;
                }
            }
            else
            {
                if (COM.CurrentRequest != SHUTDOWN)
                {
                    MessageBox.Show("Please close with MassHunter", " Exit eTrap Driver ", MessageBoxButtons.OKCancel, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button2).ToString();
                    e.Cancel = true;
                    result = "CANCEL";
                }
            }

            if (result == "OK")
            {   // Switch Heaters off
                Vars.heaterIsOn = 1;
                DataLog.NoReadbacksPlease = true;
                sleep(500);
                DataLog.SwitchHeater();
                DataLog.NoReadbacksPlease = false;
                Do_UpdateButtons();

                DataLog.Do_ExitSerialCOM(false);
                Environment.Exit(0);
            }
        }

        private void chkBx_IgnorePrepare_2_CheckedChanged(object sender, EventArgs e)
        {
            DataLog.Do_StopCycle();
            switch (chkBx_IgnorePrepare_2.CheckState)
            {
                case CheckState.Checked:
                    DataLog.SetIgnorePrepare_2(true);
                    break;
                case CheckState.Unchecked:
                    DataLog.SetIgnorePrepare_2(false);
                    break;
            }
            DataLog.Do_CreateTrapCycle();
            DataLog.Do_SendVariablesToControler();
            Vars.ConfigFileSave();
        }

        private void bttnHelpMethod_Click(object sender, EventArgs e)
        {
            string page = @"C:\chromtech\eTrap\program\Build html documentation\MethodPage.html";            
            System.Diagnostics.Process.Start("IEXPLORE.EXE", page); 
        }

        private void bttnHelpService1_Click(object sender, EventArgs e)
        {
            string page = @"C:\chromtech\eTrap\program\Build html documentation\ServiceActions.html"; ;
            System.Diagnostics.Process.Start("IEXPLORE.EXE", page); 
        }

        private void bttnHelpService2_Click(object sender, EventArgs e)
        {
            string page = @"C:\chromtech\eTrap\program\Build html documentation\ServiceLogs.html"; ;
            System.Diagnostics.Process.Start("IEXPLORE.EXE", page);
        }

        private void bttn_tempHiSlope_Click(object sender, EventArgs e)
        {
            bool isOK = GetConfirm("FOR SERVICE PERSONNEL ONLY", "This may damage your Trap!" + CRLF + CRLF + "Change Heater High Temp Slope?");

            if (isOK)
            {

                var result = GetInput("New Heater High Temp Slope",
                                        "New Heater High Temp Slope" + CRLF + "(0.1 to 10, default = 1)");
                try
                {
                    if (result != null)
                    {
                        // will throw exception if string is entered
                        var check = Convert.ToDouble(result);
                        Vars.tempHiCorrectionSlope = (float)check;
                        DataLog.SetAllTemperatureCorrections();
                    }
                }
                catch (Exception ex)
                {
                    ErrorString = "bttn_tempHiSlope_Click: " + ex.Message;
                }
            }
        }

        private void bttn_tempMidSlope_Click(object sender, EventArgs e)
        {
            bool isOK = GetConfirm("FOR SERVICE PERSONNEL ONLY", "This may damage your Trap!" + CRLF + CRLF +"Change Heater Temp Slope?");

            if (isOK)
            {


                var result = GetInput("New Heater Slope",
                                        "New Heater Slope" + CRLF + "(0.1 to 10, default = 1)");
                try
                {
                    if (result != null)
                    {
                        // will throw exception if string is entered
                        var check = Convert.ToDouble(result);
                        Vars.tempMidCorrectionSlope = (float)check;
                        DataLog.SetAllTemperatureCorrections();
                    }
                }
                catch (Exception ex)
                {
                    ErrorString = "bttn_tempMidSlope_Click: " + ex.Message;
                }
            }


        }

        private void bttn_tempLowSlope_Click(object sender, EventArgs e)
        {
            bool isOK = GetConfirm("FOR SERVICE PERSONNEL ONLY", "This may damage your Trap!" + CRLF + CRLF + "Change Heater Low Temp Slope?");

            if (isOK)
            {


                var result = GetInput("New Heater Low Slope",
                                        "New Heater Low Slope" + CRLF + "(0.1 to 10, default = 1)");
                try
                {
                    if (result != null)
                    {
                        // will throw exception if string is entered
                        var check = Convert.ToDouble(result);
                        Vars.tempLowCorrectionSlope = (float)check;
                        DataLog.SetAllTemperatureCorrections();
                    }
                }
                catch (Exception ex)
                {
                    ErrorString = "bttn_tempLowSlope_Click: " + ex.Message;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Int16 comPort = 0;
            string _filename="", _path="";


            bool isOK = GetConfirm("FOR SERVICE PERSONNEL ONLY", "This may damage your Trap!" + CRLF + CRLF + "Install Trap Firmware ?");
            if (isOK)
            {
                var result = GetInput("Select COM Port", "Type COM Port number");
                try
                {
                    if (result != null)
                    {   Vars.Comport = Convert.ToInt16(result); 
                        // this will change ComportString...
                    }
                }
                catch (Exception ex)
                {
                    ErrorString = "button1_Click: " + ex.Message;
                }
            }


            // Displays an OpenFileDialog so the user can select a Cursor.
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = Vars.FirmwarePath;
            openFileDialog1.Filter = "Controller Firmware|*.HEX";
            openFileDialog1.Title = "Select a Controller Firmware";

            // Show the Dialog.
            // If the user clicked OK in the dialog and
            // file was selected, open it.
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (openFileDialog1.FileName != "")
                {
                    _filename = Path.GetFileName( openFileDialog1.FileName);
                    _path = Path.GetDirectoryName(openFileDialog1.FileName)+"\\";
                }
            }

            DataLog.NoReadbacksPlease = true;
            DataLog.Do_ExitSerialCOM(false);
            sleep(1000);

            if (DataLog.Do_UpdateFirmware(_path, _filename) == true)
            {
                Splash.CloseForm();
                sleep(2000);

                GetConfirm("RESTART Software", "Update successfull" + CRLF + CRLF + "Close Software.");
                if (isOK)
                {
                    // prepare for ONE Start without auto FW loading
                    File.Create( Vars.FirmwarePath +"1stStart").Dispose();
                    // Don't ask for close Application:
                    Vars.autoRestart = true;
                    Application.Exit();
                }               
            }
            else
            {
                Splash.CloseForm();
                sleep(2000);
                GetConfirm("RESTART Software", "Update failed" + CRLF + CRLF + "Close Software.");
                if (isOK)
                {
                    // prepare for ONE Start without auto FW loading
                    // Don't ask for close Application:
                    Vars.autoRestart = true;
                    Application.Exit();
                }
            }
        }

        private void checkBox_UsePrepare2_Timer_CheckedChanged(object sender, EventArgs e)
        {
            DataLog.Do_StopCycle();
            switch (checkBox_UsePrepare2_Timer.CheckState)
            {
                case CheckState.Checked:
                    this.chkBx_IgnorePrepare_2.Enabled = false;
                    DataLog.SetUsePrepare2_Timer(true);
                    this.tBxPrepare2Time.Visible = true;
                    this.label22.Visible = true;
                    this.label34.Visible = true;

                    break;
                case CheckState.Unchecked:
                    DataLog.SetUsePrepare2_Timer(false);
                    this.chkBx_IgnorePrepare_2.Enabled = true;

                    this.tBxPrepare2Time.Visible = false;
                    this.label22.Visible = false;
                    this.label34.Visible = false;

                    if (chkBx_IgnorePrepare_2.Checked == true)
                        DataLog.SetIgnorePrepare_2(true);
                    else
                        DataLog.SetIgnorePrepare_2(false);                    

                    break;
            }

            DataLog.Do_CreateTrapCycle();
            DataLog.Do_SendVariablesToControler();
            Vars.ConfigFileSave();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bool isOK = GetConfirm("Reset Trap Counter", "Reset counter for new Trap" + CRLF + CRLF + "Reset?");
            if (isOK)
            {
                Vars.TRAP_CYCLES = 0;
            }
        }

        // 2.8.1  
        private void chkInUse_CheckedChanged(object sender, EventArgs e)
        {
            switch (chkInUse.CheckState)
            {
                case CheckState.Checked:
                    // No need to send to eTrap Controler, so no  DataLog.SetIgnoreGCPrepare(true);
                    WorkOn_InUse_Status(true);
                    break;
                case CheckState.Unchecked:
                    WorkOn_InUse_Status(false);
                    break;
            }
            //DataLog.Do_CreateTrapCycle();
            // DataLog.Do_SendVariablesToControler();
        }

        private void chkInUse_Leave(object sender, EventArgs e)
        {
            bttn_SaveMethod.Text = "Save*";
            bttn_SaveMethod.Enabled = true;
        }

        public void WorkOn_InUse_Status(bool enable)
        {
            Vars.eTrap_CryoMode = enable;

            // change Standby Temperature Defaults!
            if (Vars.eTrap_CryoMode)
            {
                lbl_SingleRun.ForeColor = Color.Blue;

                label36.Text = "Standby Temperature          (70)";
                if (Vars.method_StandbyTemp > 100)
                    tBxStandbyTemp.BackColor = Color.Red;
            }
            else
            {
                lbl_SingleRun.ForeColor = Color.Red;

                label36.Text = "Standby Temperature         (250)";
                if (Vars.method_StandbyTemp < 200)
                    tBxStandbyTemp.BackColor = Color.Red;
            }
            
            tBxCoolTemperature.Visible = enable; label14.Visible = enable; label28.Visible = enable;
            tBxPreInjTime.Visible = enable; label15.Visible = enable; label29.Visible = enable;
            tBxPostInjTime.Visible = enable; label16.Visible = enable; label30.Visible = enable;
            tBxHighTemperature.Visible = enable; label17.Visible = enable; label31.Visible = enable;
            tBxHighTempTime.Visible = enable; label18.Visible = enable; label32.Visible = enable;

            if (checkBox_UsePrepare2_Timer.CheckState == CheckState.Checked)
            {
                tBxPrepare2Time.Visible = enable; label22.Visible = enable; label34.Visible = enable;
            }


            

        }

    }

    
    
}

