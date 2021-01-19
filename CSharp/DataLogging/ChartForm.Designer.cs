namespace DataLogging
{
    partial class ChartForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;


        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChartForm));
            this.chartControl = new ZedGraph.ZedGraphControl();
            this.lbl_Comport = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabMethod = new System.Windows.Forms.TabPage();
            this.bttnHelpMethod = new System.Windows.Forms.Button();
            this.lblnotConnectedOrFuseTempBad = new System.Windows.Forms.Label();
            this.bttn_OK_tabMethod = new System.Windows.Forms.Button();
            this.label25 = new System.Windows.Forms.Label();
            this.bttn_MethStart = new System.Windows.Forms.Button();
            this.bttn_ViewMethod = new System.Windows.Forms.Button();
            this.textBox_Error1 = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.lbl_MethodFileName = new System.Windows.Forms.Label();
            this.lbl_MethodName = new System.Windows.Forms.Label();
            this.eTrapDisplayBox = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lbl_TrapCycles = new System.Windows.Forms.Label();
            this.label35 = new System.Windows.Forms.Label();
            this.lbl_CurrentTemperature = new System.Windows.Forms.Label();
            this.lbl_TimerTime = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.lbl_SingleRun = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lbl_cryoTimeout = new System.Windows.Forms.Label();
            this.lbl_M_LowTemp = new System.Windows.Forms.Label();
            this.lbl_M_HighTemp_Time = new System.Windows.Forms.Label();
            this.lbl_M_HighTemp = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lbl_M_StandbyTemp = new System.Windows.Forms.Label();
            this.lbl_M_LowTime_PreInject = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lbl_M_LowTime_PostInject = new System.Windows.Forms.Label();
            this.bttn_LoadMethod = new System.Windows.Forms.Button();
            this.bttn_SaveMethod = new System.Windows.Forms.Button();
            this.bttn_SendToTrap = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanel5 = new System.Windows.Forms.FlowLayoutPanel();
            this.label27 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.label34 = new System.Windows.Forms.Label();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.tBxStandbyTemp = new System.Windows.Forms.TextBox();
            this.chkInUse = new System.Windows.Forms.CheckBox();
            this.tBxCoolTemperature = new System.Windows.Forms.TextBox();
            this.tBxPreInjTime = new System.Windows.Forms.TextBox();
            this.tBxPostInjTime = new System.Windows.Forms.TextBox();
            this.tBxHighTemperature = new System.Windows.Forms.TextBox();
            this.tBxHighTempTime = new System.Windows.Forms.TextBox();
            this.tBxPrepare2Time = new System.Windows.Forms.TextBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.label36 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.tabStatus = new System.Windows.Forms.TabPage();
            this.lbl_InstrumentCycles = new System.Windows.Forms.Label();
            this.lbl36 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.chkBx_IgnoreGCPrepare = new System.Windows.Forms.CheckBox();
            this.chkBx_IgnorePrepare_2 = new System.Windows.Forms.CheckBox();
            this.checkBox_UsePrepare2_Timer = new System.Windows.Forms.CheckBox();
            this.chkBx_IgnoreCyroTimeout = new System.Windows.Forms.CheckBox();
            this.chkBx_SingleRun = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.bttn_RestartCycle = new System.Windows.Forms.Button();
            this.bttn_NextStep = new System.Windows.Forms.Button();
            this.bttn_SwitchStepMode = new System.Windows.Forms.Button();
            this.chkBx_IgnoreSyncSignals = new System.Windows.Forms.CheckBox();
            this.txtBox_TempLowCorrectionSlope = new System.Windows.Forms.TextBox();
            this.bttn_tempLowSlope = new System.Windows.Forms.Button();
            this.bttn_tempMidSlope = new System.Windows.Forms.Button();
            this.bttn_tempHiSlope = new System.Windows.Forms.Button();
            this.txtBox_TempMidCorrectionSlope = new System.Windows.Forms.TextBox();
            this.txtBox_TempHiCorrectionSlope = new System.Windows.Forms.TextBox();
            this.bttnHelpService1 = new System.Windows.Forms.Button();
            this.bttn_OK_tabStatus = new System.Windows.Forms.Button();
            this.textBox_Error2 = new System.Windows.Forms.TextBox();
            this.lbl_ControlerStatus = new System.Windows.Forms.Label();
            this.txt_Box_CycleLine = new System.Windows.Forms.TextBox();
            this.txtBox_TTL_Values = new System.Windows.Forms.TextBox();
            this.txtBox_CycleTimers = new System.Windows.Forms.TextBox();
            this.txtBox_TTL = new System.Windows.Forms.TextBox();
            this.bttn_Temperature = new System.Windows.Forms.Button();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.bttn_heater = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.tabCommunication = new System.Windows.Forms.TabPage();
            this.button1 = new System.Windows.Forms.Button();
            this.label33 = new System.Windows.Forms.Label();
            this.lbl_FWReqSubVersion = new System.Windows.Forms.Label();
            this.lbl_FWReqMainVersion = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.bttnHelpService2 = new System.Windows.Forms.Button();
            this.lbl_SWVersion = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.bttn_ViewControlerCycle = new System.Windows.Forms.Button();
            this.bttn_Communication_View = new System.Windows.Forms.Button();
            this.textBox_Error3 = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.lbl_FWVersion = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.labelTimerTime = new System.Windows.Forms.Label();
            this.buttonTimer1 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.txtBox_ControlerCycle = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.chkBx_LowLevelComm = new System.Windows.Forms.CheckBox();
            this.chkBx_TopLevelComm = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtBox_Communication = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label37 = new System.Windows.Forms.Label();
            this.lbl_HWVersion = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabMethod.SuspendLayout();
            this.eTrapDisplayBox.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.flowLayoutPanel5.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.tabStatus.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabCommunication.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // chartControl
            // 
            this.chartControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chartControl.IsAntiAlias = true;
            this.chartControl.Location = new System.Drawing.Point(23, 24);
            this.chartControl.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.chartControl.Name = "chartControl";
            this.chartControl.ScrollGrace = 0D;
            this.chartControl.ScrollMaxX = 0D;
            this.chartControl.ScrollMaxY = 0D;
            this.chartControl.ScrollMaxY2 = 0D;
            this.chartControl.ScrollMinX = 0D;
            this.chartControl.ScrollMinY = 0D;
            this.chartControl.ScrollMinY2 = 0D;
            this.chartControl.Size = new System.Drawing.Size(656, 476);
            this.chartControl.TabIndex = 0;
            // 
            // lbl_Comport
            // 
            this.lbl_Comport.AutoSize = true;
            this.lbl_Comport.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Comport.Location = new System.Drawing.Point(959, 61);
            this.lbl_Comport.Name = "lbl_Comport";
            this.lbl_Comport.Size = new System.Drawing.Size(110, 23);
            this.lbl_Comport.TabIndex = 3;
            this.lbl_Comport.Text = "lbl_Comport";
            this.lbl_Comport.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.tabControl1.Controls.Add(this.tabMethod);
            this.tabControl1.Controls.Add(this.tabStatus);
            this.tabControl1.Controls.Add(this.tabCommunication);
            this.tabControl1.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.ItemSize = new System.Drawing.Size(125, 24);
            this.tabControl1.Location = new System.Drawing.Point(1, 1);
            this.tabControl1.MaximumSize = new System.Drawing.Size(1160, 820);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1160, 820);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl1.TabIndex = 5;
            // 
            // tabMethod
            // 
            this.tabMethod.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(165)))), ((int)(((byte)(250)))));
            this.tabMethod.Controls.Add(this.bttnHelpMethod);
            this.tabMethod.Controls.Add(this.lblnotConnectedOrFuseTempBad);
            this.tabMethod.Controls.Add(this.bttn_OK_tabMethod);
            this.tabMethod.Controls.Add(this.label25);
            this.tabMethod.Controls.Add(this.bttn_MethStart);
            this.tabMethod.Controls.Add(this.bttn_ViewMethod);
            this.tabMethod.Controls.Add(this.textBox_Error1);
            this.tabMethod.Controls.Add(this.label12);
            this.tabMethod.Controls.Add(this.lbl_MethodFileName);
            this.tabMethod.Controls.Add(this.lbl_MethodName);
            this.tabMethod.Controls.Add(this.eTrapDisplayBox);
            this.tabMethod.Controls.Add(this.bttn_LoadMethod);
            this.tabMethod.Controls.Add(this.bttn_SaveMethod);
            this.tabMethod.Controls.Add(this.bttn_SendToTrap);
            this.tabMethod.Controls.Add(this.groupBox1);
            this.tabMethod.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabMethod.Location = new System.Drawing.Point(4, 28);
            this.tabMethod.Name = "tabMethod";
            this.tabMethod.Size = new System.Drawing.Size(1152, 788);
            this.tabMethod.TabIndex = 2;
            this.tabMethod.Text = "Method";
            // 
            // bttnHelpMethod
            // 
            this.bttnHelpMethod.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(250)))), ((int)(((byte)(255)))));
            this.bttnHelpMethod.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.bttnHelpMethod.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bttnHelpMethod.Location = new System.Drawing.Point(24, 721);
            this.bttnHelpMethod.Name = "bttnHelpMethod";
            this.bttnHelpMethod.Size = new System.Drawing.Size(103, 48);
            this.bttnHelpMethod.TabIndex = 36;
            this.bttnHelpMethod.Text = "Help";
            this.toolTip1.SetToolTip(this.bttnHelpMethod, "Help");
            this.bttnHelpMethod.UseVisualStyleBackColor = false;
            this.bttnHelpMethod.Click += new System.EventHandler(this.bttnHelpMethod_Click);
            // 
            // lblnotConnectedOrFuseTempBad
            // 
            this.lblnotConnectedOrFuseTempBad.AutoSize = true;
            this.lblnotConnectedOrFuseTempBad.BackColor = System.Drawing.Color.Red;
            this.lblnotConnectedOrFuseTempBad.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblnotConnectedOrFuseTempBad.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblnotConnectedOrFuseTempBad.Location = new System.Drawing.Point(18, 11);
            this.lblnotConnectedOrFuseTempBad.MinimumSize = new System.Drawing.Size(1120, 60);
            this.lblnotConnectedOrFuseTempBad.Name = "lblnotConnectedOrFuseTempBad";
            this.lblnotConnectedOrFuseTempBad.Size = new System.Drawing.Size(1120, 60);
            this.lblnotConnectedOrFuseTempBad.TabIndex = 34;
            this.lblnotConnectedOrFuseTempBad.Text = "1. Please close the software.  2. Verify that eTrap is switched ON.  3. Restart t" +
    "he software.";
            this.lblnotConnectedOrFuseTempBad.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // bttn_OK_tabMethod
            // 
            this.bttn_OK_tabMethod.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(250)))), ((int)(((byte)(255)))));
            this.bttn_OK_tabMethod.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.bttn_OK_tabMethod.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bttn_OK_tabMethod.Location = new System.Drawing.Point(1025, 650);
            this.bttn_OK_tabMethod.Name = "bttn_OK_tabMethod";
            this.bttn_OK_tabMethod.Size = new System.Drawing.Size(103, 48);
            this.bttn_OK_tabMethod.TabIndex = 35;
            this.bttn_OK_tabMethod.Text = "OK";
            this.bttn_OK_tabMethod.UseVisualStyleBackColor = false;
            this.bttn_OK_tabMethod.Click += new System.EventHandler(this.bttn_OK_tabMethod_Click);
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label25.Location = new System.Drawing.Point(111, 44);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(45, 23);
            this.label25.TabIndex = 33;
            this.label25.Text = "File:";
            // 
            // bttn_MethStart
            // 
            this.bttn_MethStart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(250)))), ((int)(((byte)(255)))));
            this.bttn_MethStart.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.bttn_MethStart.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bttn_MethStart.Location = new System.Drawing.Point(327, 650);
            this.bttn_MethStart.Name = "bttn_MethStart";
            this.bttn_MethStart.Size = new System.Drawing.Size(103, 48);
            this.bttn_MethStart.TabIndex = 30;
            this.bttn_MethStart.Text = "Start";
            this.bttn_MethStart.UseVisualStyleBackColor = false;
            this.bttn_MethStart.Click += new System.EventHandler(this.bttn_MethStart_Click);
            // 
            // bttn_ViewMethod
            // 
            this.bttn_ViewMethod.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(250)))), ((int)(((byte)(255)))));
            this.bttn_ViewMethod.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.bttn_ViewMethod.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bttn_ViewMethod.Location = new System.Drawing.Point(24, 650);
            this.bttn_ViewMethod.Name = "bttn_ViewMethod";
            this.bttn_ViewMethod.Size = new System.Drawing.Size(103, 48);
            this.bttn_ViewMethod.TabIndex = 32;
            this.bttn_ViewMethod.Text = "View";
            this.toolTip1.SetToolTip(this.bttn_ViewMethod, "View and Print");
            this.bttn_ViewMethod.UseVisualStyleBackColor = false;
            this.bttn_ViewMethod.Click += new System.EventHandler(this.bttn_ViewMethod_Click);
            // 
            // textBox_Error1
            // 
            this.textBox_Error1.BackColor = System.Drawing.Color.White;
            this.textBox_Error1.Enabled = false;
            this.textBox_Error1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_Error1.ForeColor = System.Drawing.Color.Red;
            this.textBox_Error1.Location = new System.Drawing.Point(150, 721);
            this.textBox_Error1.Multiline = true;
            this.textBox_Error1.Name = "textBox_Error1";
            this.textBox_Error1.ReadOnly = true;
            this.textBox_Error1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_Error1.Size = new System.Drawing.Size(697, 49);
            this.textBox_Error1.TabIndex = 31;
            this.textBox_Error1.Visible = false;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(14, 11);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(142, 23);
            this.label12.TabIndex = 29;
            this.label12.Text = "e-Trap Method:";
            // 
            // lbl_MethodFileName
            // 
            this.lbl_MethodFileName.AutoSize = true;
            this.lbl_MethodFileName.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_MethodFileName.Location = new System.Drawing.Point(178, 44);
            this.lbl_MethodFileName.Name = "lbl_MethodFileName";
            this.lbl_MethodFileName.Size = new System.Drawing.Size(224, 23);
            this.lbl_MethodFileName.TabIndex = 28;
            this.lbl_MethodFileName.Text = "Current Controler Method";
            // 
            // lbl_MethodName
            // 
            this.lbl_MethodName.AutoSize = true;
            this.lbl_MethodName.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_MethodName.Location = new System.Drawing.Point(178, 11);
            this.lbl_MethodName.Name = "lbl_MethodName";
            this.lbl_MethodName.Size = new System.Drawing.Size(151, 23);
            this.lbl_MethodName.TabIndex = 26;
            this.lbl_MethodName.Text = "lbl_MethodName";
            // 
            // eTrapDisplayBox
            // 
            this.eTrapDisplayBox.BackColor = System.Drawing.Color.Transparent;
            this.eTrapDisplayBox.Controls.Add(this.groupBox3);
            this.eTrapDisplayBox.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.eTrapDisplayBox.Location = new System.Drawing.Point(18, 81);
            this.eTrapDisplayBox.Name = "eTrapDisplayBox";
            this.eTrapDisplayBox.Size = new System.Drawing.Size(670, 544);
            this.eTrapDisplayBox.TabIndex = 25;
            this.eTrapDisplayBox.TabStop = false;
            this.eTrapDisplayBox.Text = "e-Trap Display";
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.Transparent;
            this.groupBox3.Controls.Add(this.lbl_TrapCycles);
            this.groupBox3.Controls.Add(this.label35);
            this.groupBox3.Controls.Add(this.lbl_CurrentTemperature);
            this.groupBox3.Controls.Add(this.lbl_TimerTime);
            this.groupBox3.Controls.Add(this.label21);
            this.groupBox3.Controls.Add(this.lbl_SingleRun);
            this.groupBox3.Controls.Add(this.label19);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.lbl_cryoTimeout);
            this.groupBox3.Controls.Add(this.lbl_M_LowTemp);
            this.groupBox3.Controls.Add(this.lbl_M_HighTemp_Time);
            this.groupBox3.Controls.Add(this.lbl_M_HighTemp);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.lbl_M_StandbyTemp);
            this.groupBox3.Controls.Add(this.lbl_M_LowTime_PreInject);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.lbl_M_LowTime_PostInject);
            this.groupBox3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.groupBox3.Location = new System.Drawing.Point(9, 26);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(655, 509);
            this.groupBox3.TabIndex = 7;
            this.groupBox3.TabStop = false;
            this.groupBox3.Paint += new System.Windows.Forms.PaintEventHandler(this.Do_DrawMethodLines);
            // 
            // lbl_TrapCycles
            // 
            this.lbl_TrapCycles.AutoSize = true;
            this.lbl_TrapCycles.BackColor = System.Drawing.Color.Transparent;
            this.lbl_TrapCycles.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_TrapCycles.Location = new System.Drawing.Point(112, 474);
            this.lbl_TrapCycles.Name = "lbl_TrapCycles";
            this.lbl_TrapCycles.Size = new System.Drawing.Size(129, 23);
            this.lbl_TrapCycles.TabIndex = 27;
            this.lbl_TrapCycles.Text = "lbl_TrapCycles";
            this.lbl_TrapCycles.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.BackColor = System.Drawing.Color.Transparent;
            this.label35.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label35.Location = new System.Drawing.Point(6, 474);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(113, 23);
            this.label35.TabIndex = 26;
            this.label35.Text = "Trap Cycles:";
            // 
            // lbl_CurrentTemperature
            // 
            this.lbl_CurrentTemperature.AutoSize = true;
            this.lbl_CurrentTemperature.BackColor = System.Drawing.Color.Transparent;
            this.lbl_CurrentTemperature.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_CurrentTemperature.Location = new System.Drawing.Point(137, 23);
            this.lbl_CurrentTemperature.Name = "lbl_CurrentTemperature";
            this.lbl_CurrentTemperature.Size = new System.Drawing.Size(61, 23);
            this.lbl_CurrentTemperature.TabIndex = 21;
            this.lbl_CurrentTemperature.Text = "lbl_Cu";
            this.lbl_CurrentTemperature.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lbl_TimerTime
            // 
            this.lbl_TimerTime.AutoSize = true;
            this.lbl_TimerTime.BackColor = System.Drawing.Color.Transparent;
            this.lbl_TimerTime.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_TimerTime.Location = new System.Drawing.Point(291, 23);
            this.lbl_TimerTime.Name = "lbl_TimerTime";
            this.lbl_TimerTime.Size = new System.Drawing.Size(87, 23);
            this.lbl_TimerTime.TabIndex = 23;
            this.lbl_TimerTime.Text = "lbl_Timer";
            this.lbl_TimerTime.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.BackColor = System.Drawing.Color.Transparent;
            this.label21.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label21.Location = new System.Drawing.Point(223, 23);
            this.label21.MinimumSize = new System.Drawing.Size(123, 0);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(123, 23);
            this.label21.TabIndex = 24;
            this.label21.Text = "Time:";
            this.label21.Click += new System.EventHandler(this.label21_Click);
            // 
            // lbl_SingleRun
            // 
            this.lbl_SingleRun.AutoSize = true;
            this.lbl_SingleRun.BackColor = System.Drawing.Color.Transparent;
            this.lbl_SingleRun.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_SingleRun.Location = new System.Drawing.Point(400, 23);
            this.lbl_SingleRun.MinimumSize = new System.Drawing.Size(123, 0);
            this.lbl_SingleRun.Name = "lbl_SingleRun";
            this.lbl_SingleRun.Size = new System.Drawing.Size(124, 23);
            this.lbl_SingleRun.TabIndex = 25;
            this.lbl_SingleRun.Text = "lbl_SingleRun";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.BackColor = System.Drawing.Color.Transparent;
            this.label19.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.Location = new System.Drawing.Point(6, 23);
            this.label19.MinimumSize = new System.Drawing.Size(175, 0);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(175, 23);
            this.label19.TabIndex = 22;
            this.label19.Text = "Temperature:";
            this.label19.Click += new System.EventHandler(this.label19_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(195, 384);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(35, 25);
            this.label7.TabIndex = 11;
            this.label7.Text = "°C";
            this.label7.Click += new System.EventHandler(this.label7_Click);
            // 
            // lbl_cryoTimeout
            // 
            this.lbl_cryoTimeout.AutoSize = true;
            this.lbl_cryoTimeout.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_cryoTimeout.ForeColor = System.Drawing.Color.Red;
            this.lbl_cryoTimeout.Location = new System.Drawing.Point(30, 123);
            this.lbl_cryoTimeout.Name = "lbl_cryoTimeout";
            this.lbl_cryoTimeout.Size = new System.Drawing.Size(0, 23);
            this.lbl_cryoTimeout.TabIndex = 16;
            // 
            // lbl_M_LowTemp
            // 
            this.lbl_M_LowTemp.AutoSize = true;
            this.lbl_M_LowTemp.BackColor = System.Drawing.Color.Transparent;
            this.lbl_M_LowTemp.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_M_LowTemp.Location = new System.Drawing.Point(145, 384);
            this.lbl_M_LowTemp.MinimumSize = new System.Drawing.Size(54, 0);
            this.lbl_M_LowTemp.Name = "lbl_M_LowTemp";
            this.lbl_M_LowTemp.Size = new System.Drawing.Size(54, 25);
            this.lbl_M_LowTemp.TabIndex = 2;
            this.lbl_M_LowTemp.Text = "T1";
            this.lbl_M_LowTemp.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lbl_M_HighTemp_Time
            // 
            this.lbl_M_HighTemp_Time.AutoSize = true;
            this.lbl_M_HighTemp_Time.BackColor = System.Drawing.Color.Transparent;
            this.lbl_M_HighTemp_Time.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_M_HighTemp_Time.Location = new System.Drawing.Point(423, 169);
            this.lbl_M_HighTemp_Time.MinimumSize = new System.Drawing.Size(44, 0);
            this.lbl_M_HighTemp_Time.Name = "lbl_M_HighTemp_Time";
            this.lbl_M_HighTemp_Time.Size = new System.Drawing.Size(44, 25);
            this.lbl_M_HighTemp_Time.TabIndex = 6;
            this.lbl_M_HighTemp_Time.Text = "S3";
            this.lbl_M_HighTemp_Time.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lbl_M_HighTemp
            // 
            this.lbl_M_HighTemp.AutoSize = true;
            this.lbl_M_HighTemp.BackColor = System.Drawing.Color.Transparent;
            this.lbl_M_HighTemp.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_M_HighTemp.Location = new System.Drawing.Point(413, 131);
            this.lbl_M_HighTemp.MinimumSize = new System.Drawing.Size(54, 0);
            this.lbl_M_HighTemp.Name = "lbl_M_HighTemp";
            this.lbl_M_HighTemp.Size = new System.Drawing.Size(54, 25);
            this.lbl_M_HighTemp.TabIndex = 5;
            this.lbl_M_HighTemp.Text = "T3";
            this.lbl_M_HighTemp.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(463, 131);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(35, 25);
            this.label8.TabIndex = 12;
            this.label8.Text = "°C";
            this.label8.Click += new System.EventHandler(this.label8_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(463, 169);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(42, 25);
            this.label11.TabIndex = 15;
            this.label11.Text = "sec";
            this.label11.Click += new System.EventHandler(this.label11_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(195, 419);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(42, 25);
            this.label9.TabIndex = 13;
            this.label9.Text = "sec";
            this.label9.Click += new System.EventHandler(this.label9_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(76, 292);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 25);
            this.label6.TabIndex = 10;
            this.label6.Text = "°C";
            this.label6.Click += new System.EventHandler(this.label6_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(304, 419);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(42, 25);
            this.label10.TabIndex = 14;
            this.label10.Text = "sec";
            this.label10.Click += new System.EventHandler(this.label10_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(247, 179);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 23);
            this.label5.TabIndex = 9;
            this.label5.Text = "Inject";
            // 
            // lbl_M_StandbyTemp
            // 
            this.lbl_M_StandbyTemp.AutoSize = true;
            this.lbl_M_StandbyTemp.BackColor = System.Drawing.Color.Transparent;
            this.lbl_M_StandbyTemp.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_M_StandbyTemp.Location = new System.Drawing.Point(28, 292);
            this.lbl_M_StandbyTemp.MinimumSize = new System.Drawing.Size(54, 0);
            this.lbl_M_StandbyTemp.Name = "lbl_M_StandbyTemp";
            this.lbl_M_StandbyTemp.Size = new System.Drawing.Size(54, 25);
            this.lbl_M_StandbyTemp.TabIndex = 1;
            this.lbl_M_StandbyTemp.Text = "T1";
            this.lbl_M_StandbyTemp.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lbl_M_LowTime_PreInject
            // 
            this.lbl_M_LowTime_PreInject.AutoSize = true;
            this.lbl_M_LowTime_PreInject.BackColor = System.Drawing.Color.Transparent;
            this.lbl_M_LowTime_PreInject.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_M_LowTime_PreInject.Location = new System.Drawing.Point(154, 419);
            this.lbl_M_LowTime_PreInject.MinimumSize = new System.Drawing.Size(44, 0);
            this.lbl_M_LowTime_PreInject.Name = "lbl_M_LowTime_PreInject";
            this.lbl_M_LowTime_PreInject.Size = new System.Drawing.Size(44, 25);
            this.lbl_M_LowTime_PreInject.TabIndex = 3;
            this.lbl_M_LowTime_PreInject.Text = "S1";
            this.lbl_M_LowTime_PreInject.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(87, 179);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 23);
            this.label4.TabIndex = 8;
            this.label4.Text = "Prepare";
            // 
            // lbl_M_LowTime_PostInject
            // 
            this.lbl_M_LowTime_PostInject.AutoSize = true;
            this.lbl_M_LowTime_PostInject.BackColor = System.Drawing.Color.Transparent;
            this.lbl_M_LowTime_PostInject.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_M_LowTime_PostInject.Location = new System.Drawing.Point(261, 419);
            this.lbl_M_LowTime_PostInject.MinimumSize = new System.Drawing.Size(44, 0);
            this.lbl_M_LowTime_PostInject.Name = "lbl_M_LowTime_PostInject";
            this.lbl_M_LowTime_PostInject.Size = new System.Drawing.Size(44, 25);
            this.lbl_M_LowTime_PostInject.TabIndex = 4;
            this.lbl_M_LowTime_PostInject.Text = "S2";
            this.lbl_M_LowTime_PostInject.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // bttn_LoadMethod
            // 
            this.bttn_LoadMethod.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(250)))), ((int)(((byte)(255)))));
            this.bttn_LoadMethod.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.bttn_LoadMethod.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bttn_LoadMethod.Location = new System.Drawing.Point(872, 650);
            this.bttn_LoadMethod.Name = "bttn_LoadMethod";
            this.bttn_LoadMethod.Size = new System.Drawing.Size(103, 48);
            this.bttn_LoadMethod.TabIndex = 19;
            this.bttn_LoadMethod.Text = "Load";
            this.toolTip1.SetToolTip(this.bttn_LoadMethod, "Load Method");
            this.bttn_LoadMethod.UseVisualStyleBackColor = false;
            this.bttn_LoadMethod.Click += new System.EventHandler(this.bttn_LoadMethod_Click_1);
            // 
            // bttn_SaveMethod
            // 
            this.bttn_SaveMethod.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(250)))), ((int)(((byte)(255)))));
            this.bttn_SaveMethod.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.bttn_SaveMethod.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bttn_SaveMethod.Location = new System.Drawing.Point(1025, 650);
            this.bttn_SaveMethod.Name = "bttn_SaveMethod";
            this.bttn_SaveMethod.Size = new System.Drawing.Size(103, 48);
            this.bttn_SaveMethod.TabIndex = 17;
            this.bttn_SaveMethod.Text = "Save";
            this.toolTip1.SetToolTip(this.bttn_SaveMethod, "Save Method");
            this.bttn_SaveMethod.UseVisualStyleBackColor = false;
            this.bttn_SaveMethod.Click += new System.EventHandler(this.bttn_SaveMethod_Click);
            // 
            // bttn_SendToTrap
            // 
            this.bttn_SendToTrap.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(250)))), ((int)(((byte)(255)))));
            this.bttn_SendToTrap.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.bttn_SendToTrap.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bttn_SendToTrap.Location = new System.Drawing.Point(576, 650);
            this.bttn_SendToTrap.Name = "bttn_SendToTrap";
            this.bttn_SendToTrap.Size = new System.Drawing.Size(103, 48);
            this.bttn_SendToTrap.TabIndex = 16;
            this.bttn_SendToTrap.Text = "Apply";
            this.bttn_SendToTrap.UseVisualStyleBackColor = false;
            this.bttn_SendToTrap.Click += new System.EventHandler(this.bttn_SendToTrap_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.AutoSize = true;
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.flowLayoutPanel5);
            this.groupBox1.Controls.Add(this.flowLayoutPanel2);
            this.groupBox1.Controls.Add(this.flowLayoutPanel1);
            this.groupBox1.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(698, 81);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(441, 544);
            this.groupBox1.TabIndex = 20;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "e-Trap Method               (default)";
            // 
            // flowLayoutPanel5
            // 
            this.flowLayoutPanel5.BackColor = System.Drawing.Color.Transparent;
            this.flowLayoutPanel5.Controls.Add(this.label27);
            this.flowLayoutPanel5.Controls.Add(this.label28);
            this.flowLayoutPanel5.Controls.Add(this.label29);
            this.flowLayoutPanel5.Controls.Add(this.label30);
            this.flowLayoutPanel5.Controls.Add(this.label31);
            this.flowLayoutPanel5.Controls.Add(this.label32);
            this.flowLayoutPanel5.Controls.Add(this.label34);
            this.flowLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Left;
            this.flowLayoutPanel5.Location = new System.Drawing.Point(379, 26);
            this.flowLayoutPanel5.Name = "flowLayoutPanel5";
            this.flowLayoutPanel5.Size = new System.Drawing.Size(59, 515);
            this.flowLayoutPanel5.TabIndex = 2;
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.BackColor = System.Drawing.Color.Transparent;
            this.label27.Location = new System.Drawing.Point(0, 23);
            this.label27.Margin = new System.Windows.Forms.Padding(0, 23, 3, 3);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(30, 23);
            this.label27.TabIndex = 1;
            this.label27.Text = "°C";
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.BackColor = System.Drawing.Color.Transparent;
            this.label28.Location = new System.Drawing.Point(0, 126);
            this.label28.Margin = new System.Windows.Forms.Padding(0, 77, 3, 3);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(30, 23);
            this.label28.TabIndex = 2;
            this.label28.Text = "°C";
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.BackColor = System.Drawing.Color.Transparent;
            this.label29.Location = new System.Drawing.Point(0, 176);
            this.label29.Margin = new System.Windows.Forms.Padding(0, 24, 3, 3);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(37, 23);
            this.label29.TabIndex = 3;
            this.label29.Text = "sec";
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.BackColor = System.Drawing.Color.Transparent;
            this.label30.Location = new System.Drawing.Point(0, 226);
            this.label30.Margin = new System.Windows.Forms.Padding(0, 24, 3, 3);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(37, 23);
            this.label30.TabIndex = 4;
            this.label30.Text = "sec";
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.BackColor = System.Drawing.Color.Transparent;
            this.label31.Location = new System.Drawing.Point(0, 277);
            this.label31.Margin = new System.Windows.Forms.Padding(0, 25, 3, 3);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(30, 23);
            this.label31.TabIndex = 5;
            this.label31.Text = "°C";
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.BackColor = System.Drawing.Color.Transparent;
            this.label32.Location = new System.Drawing.Point(0, 329);
            this.label32.Margin = new System.Windows.Forms.Padding(0, 26, 3, 3);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(37, 23);
            this.label32.TabIndex = 6;
            this.label32.Text = "sec";
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.BackColor = System.Drawing.Color.Transparent;
            this.label34.Location = new System.Drawing.Point(0, 394);
            this.label34.Margin = new System.Windows.Forms.Padding(0, 39, 3, 3);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(37, 23);
            this.label34.TabIndex = 7;
            this.label34.Text = "sec";
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.BackColor = System.Drawing.Color.Transparent;
            this.flowLayoutPanel2.Controls.Add(this.tBxStandbyTemp);
            this.flowLayoutPanel2.Controls.Add(this.chkInUse);
            this.flowLayoutPanel2.Controls.Add(this.tBxCoolTemperature);
            this.flowLayoutPanel2.Controls.Add(this.tBxPreInjTime);
            this.flowLayoutPanel2.Controls.Add(this.tBxPostInjTime);
            this.flowLayoutPanel2.Controls.Add(this.tBxHighTemperature);
            this.flowLayoutPanel2.Controls.Add(this.tBxHighTempTime);
            this.flowLayoutPanel2.Controls.Add(this.tBxPrepare2Time);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(314, 26);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(65, 515);
            this.flowLayoutPanel2.TabIndex = 1;
            // 
            // tBxStandbyTemp
            // 
            this.tBxStandbyTemp.AcceptsReturn = true;
            this.tBxStandbyTemp.AcceptsTab = true;
            this.tBxStandbyTemp.BackColor = System.Drawing.Color.White;
            this.tBxStandbyTemp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tBxStandbyTemp.Dock = System.Windows.Forms.DockStyle.Top;
            this.tBxStandbyTemp.Location = new System.Drawing.Point(3, 18);
            this.tBxStandbyTemp.Margin = new System.Windows.Forms.Padding(3, 18, 3, 3);
            this.tBxStandbyTemp.Name = "tBxStandbyTemp";
            this.tBxStandbyTemp.Size = new System.Drawing.Size(58, 30);
            this.tBxStandbyTemp.TabIndex = 0;
            this.tBxStandbyTemp.TabStop = false;
            this.tBxStandbyTemp.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tBxStandbyTemp.Click += new System.EventHandler(this.textBox1_Click);
            this.tBxStandbyTemp.Enter += new System.EventHandler(this.textBox1_Enter);
            this.tBxStandbyTemp.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyDown);
            this.tBxStandbyTemp.Leave += new System.EventHandler(this.textBox1_Leave);
            // 
            // chkInUse
            // 
            this.chkInUse.AutoSize = true;
            this.chkInUse.Font = new System.Drawing.Font("Tahoma", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkInUse.Location = new System.Drawing.Point(25, 76);
            this.chkInUse.Margin = new System.Windows.Forms.Padding(25, 25, 3, 3);
            this.chkInUse.MaximumSize = new System.Drawing.Size(40, 40);
            this.chkInUse.Name = "chkInUse";
            this.chkInUse.Size = new System.Drawing.Size(15, 14);
            this.chkInUse.TabIndex = 8;
            this.chkInUse.UseVisualStyleBackColor = true;
            this.chkInUse.CheckedChanged += new System.EventHandler(this.chkInUse_CheckedChanged);
            this.chkInUse.Enter += new System.EventHandler(this.chkInUse_Leave);
            // 
            // tBxCoolTemperature
            // 
            this.tBxCoolTemperature.AcceptsReturn = true;
            this.tBxCoolTemperature.AcceptsTab = true;
            this.tBxCoolTemperature.BackColor = System.Drawing.Color.White;
            this.tBxCoolTemperature.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tBxCoolTemperature.Dock = System.Windows.Forms.DockStyle.Top;
            this.tBxCoolTemperature.Location = new System.Drawing.Point(3, 123);
            this.tBxCoolTemperature.Margin = new System.Windows.Forms.Padding(3, 30, 3, 3);
            this.tBxCoolTemperature.Name = "tBxCoolTemperature";
            this.tBxCoolTemperature.Size = new System.Drawing.Size(58, 30);
            this.tBxCoolTemperature.TabIndex = 1;
            this.tBxCoolTemperature.TabStop = false;
            this.tBxCoolTemperature.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tBxCoolTemperature.Click += new System.EventHandler(this.textBox1_Click);
            this.tBxCoolTemperature.Enter += new System.EventHandler(this.textBox1_Enter);
            this.tBxCoolTemperature.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyDown);
            this.tBxCoolTemperature.Leave += new System.EventHandler(this.textBox1_Leave);
            // 
            // tBxPreInjTime
            // 
            this.tBxPreInjTime.AcceptsReturn = true;
            this.tBxPreInjTime.AcceptsTab = true;
            this.tBxPreInjTime.BackColor = System.Drawing.Color.White;
            this.tBxPreInjTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tBxPreInjTime.Dock = System.Windows.Forms.DockStyle.Top;
            this.tBxPreInjTime.Location = new System.Drawing.Point(3, 174);
            this.tBxPreInjTime.Margin = new System.Windows.Forms.Padding(3, 18, 3, 3);
            this.tBxPreInjTime.Name = "tBxPreInjTime";
            this.tBxPreInjTime.Size = new System.Drawing.Size(58, 30);
            this.tBxPreInjTime.TabIndex = 2;
            this.tBxPreInjTime.TabStop = false;
            this.tBxPreInjTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tBxPreInjTime.Click += new System.EventHandler(this.textBox1_Click);
            this.tBxPreInjTime.Enter += new System.EventHandler(this.textBox1_Enter);
            this.tBxPreInjTime.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyDown);
            this.tBxPreInjTime.Leave += new System.EventHandler(this.textBox1_Leave);
            // 
            // tBxPostInjTime
            // 
            this.tBxPostInjTime.AcceptsReturn = true;
            this.tBxPostInjTime.AcceptsTab = true;
            this.tBxPostInjTime.BackColor = System.Drawing.Color.White;
            this.tBxPostInjTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tBxPostInjTime.Dock = System.Windows.Forms.DockStyle.Top;
            this.tBxPostInjTime.Location = new System.Drawing.Point(3, 225);
            this.tBxPostInjTime.Margin = new System.Windows.Forms.Padding(3, 18, 3, 3);
            this.tBxPostInjTime.Name = "tBxPostInjTime";
            this.tBxPostInjTime.Size = new System.Drawing.Size(58, 30);
            this.tBxPostInjTime.TabIndex = 3;
            this.tBxPostInjTime.TabStop = false;
            this.tBxPostInjTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tBxPostInjTime.Click += new System.EventHandler(this.textBox1_Click);
            this.tBxPostInjTime.Enter += new System.EventHandler(this.textBox1_Enter);
            this.tBxPostInjTime.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyDown);
            this.tBxPostInjTime.Leave += new System.EventHandler(this.textBox1_Leave);
            // 
            // tBxHighTemperature
            // 
            this.tBxHighTemperature.AcceptsReturn = true;
            this.tBxHighTemperature.AcceptsTab = true;
            this.tBxHighTemperature.BackColor = System.Drawing.Color.White;
            this.tBxHighTemperature.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tBxHighTemperature.Dock = System.Windows.Forms.DockStyle.Top;
            this.tBxHighTemperature.Location = new System.Drawing.Point(3, 276);
            this.tBxHighTemperature.Margin = new System.Windows.Forms.Padding(3, 18, 3, 3);
            this.tBxHighTemperature.Name = "tBxHighTemperature";
            this.tBxHighTemperature.Size = new System.Drawing.Size(58, 30);
            this.tBxHighTemperature.TabIndex = 4;
            this.tBxHighTemperature.TabStop = false;
            this.tBxHighTemperature.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tBxHighTemperature.Click += new System.EventHandler(this.textBox1_Click);
            this.tBxHighTemperature.Enter += new System.EventHandler(this.textBox1_Enter);
            this.tBxHighTemperature.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyDown);
            this.tBxHighTemperature.Leave += new System.EventHandler(this.textBox1_Leave);
            // 
            // tBxHighTempTime
            // 
            this.tBxHighTempTime.AcceptsReturn = true;
            this.tBxHighTempTime.AcceptsTab = true;
            this.tBxHighTempTime.BackColor = System.Drawing.Color.White;
            this.tBxHighTempTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tBxHighTempTime.Dock = System.Windows.Forms.DockStyle.Top;
            this.tBxHighTempTime.Location = new System.Drawing.Point(3, 327);
            this.tBxHighTempTime.Margin = new System.Windows.Forms.Padding(3, 18, 3, 3);
            this.tBxHighTempTime.Name = "tBxHighTempTime";
            this.tBxHighTempTime.Size = new System.Drawing.Size(58, 30);
            this.tBxHighTempTime.TabIndex = 5;
            this.tBxHighTempTime.TabStop = false;
            this.tBxHighTempTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tBxHighTempTime.Click += new System.EventHandler(this.textBox1_Click);
            this.tBxHighTempTime.Enter += new System.EventHandler(this.textBox1_Enter);
            this.tBxHighTempTime.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyDown);
            this.tBxHighTempTime.Leave += new System.EventHandler(this.textBox1_Leave);
            // 
            // tBxPrepare2Time
            // 
            this.tBxPrepare2Time.AcceptsReturn = true;
            this.tBxPrepare2Time.AcceptsTab = true;
            this.tBxPrepare2Time.BackColor = System.Drawing.Color.White;
            this.tBxPrepare2Time.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tBxPrepare2Time.Dock = System.Windows.Forms.DockStyle.Top;
            this.tBxPrepare2Time.Location = new System.Drawing.Point(3, 390);
            this.tBxPrepare2Time.Margin = new System.Windows.Forms.Padding(3, 30, 3, 3);
            this.tBxPrepare2Time.Name = "tBxPrepare2Time";
            this.tBxPrepare2Time.Size = new System.Drawing.Size(58, 30);
            this.tBxPrepare2Time.TabIndex = 6;
            this.tBxPrepare2Time.TabStop = false;
            this.tBxPrepare2Time.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tBxPrepare2Time.Click += new System.EventHandler(this.textBox1_Click);
            this.tBxPrepare2Time.Enter += new System.EventHandler(this.textBox1_Enter);
            this.tBxPrepare2Time.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyDown);
            this.tBxPrepare2Time.Leave += new System.EventHandler(this.textBox1_Leave);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.flowLayoutPanel1.Controls.Add(this.label36);
            this.flowLayoutPanel1.Controls.Add(this.label13);
            this.flowLayoutPanel1.Controls.Add(this.label14);
            this.flowLayoutPanel1.Controls.Add(this.label15);
            this.flowLayoutPanel1.Controls.Add(this.label16);
            this.flowLayoutPanel1.Controls.Add(this.label17);
            this.flowLayoutPanel1.Controls.Add(this.label18);
            this.flowLayoutPanel1.Controls.Add(this.label22);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 26);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(311, 515);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.BackColor = System.Drawing.Color.Transparent;
            this.label36.Location = new System.Drawing.Point(7, 22);
            this.label36.Margin = new System.Windows.Forms.Padding(7, 22, 3, 3);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(288, 23);
            this.label36.TabIndex = 6;
            this.label36.Text = "Standby Temperature          (70)";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.BackColor = System.Drawing.Color.Transparent;
            this.label13.Location = new System.Drawing.Point(7, 70);
            this.label13.Margin = new System.Windows.Forms.Padding(7, 22, 3, 3);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(145, 23);
            this.label13.TabIndex = 0;
            this.label13.Text = "Cryo Trap mode";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.BackColor = System.Drawing.Color.Transparent;
            this.label14.Location = new System.Drawing.Point(7, 126);
            this.label14.Margin = new System.Windows.Forms.Padding(7, 30, 3, 3);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(291, 23);
            this.label14.TabIndex = 1;
            this.label14.Text = "Cool Temperature               (-10)";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.BackColor = System.Drawing.Color.Transparent;
            this.label15.Location = new System.Drawing.Point(7, 176);
            this.label15.Margin = new System.Windows.Forms.Padding(7, 24, 3, 3);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(291, 23);
            this.label15.TabIndex = 2;
            this.label15.Text = "Cool Temp. Pre Inject Time   (30)";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.BackColor = System.Drawing.Color.Transparent;
            this.label16.Location = new System.Drawing.Point(7, 227);
            this.label16.Margin = new System.Windows.Forms.Padding(7, 25, 3, 3);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(288, 23);
            this.label16.TabIndex = 3;
            this.label16.Text = "Cool Temp. Post Inject Time   (2)";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.BackColor = System.Drawing.Color.Transparent;
            this.label17.Location = new System.Drawing.Point(7, 279);
            this.label17.Margin = new System.Windows.Forms.Padding(7, 26, 3, 3);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(292, 23);
            this.label17.TabIndex = 4;
            this.label17.Text = "High Temperature              (220)";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.BackColor = System.Drawing.Color.Transparent;
            this.label18.Location = new System.Drawing.Point(7, 331);
            this.label18.Margin = new System.Windows.Forms.Padding(7, 26, 3, 3);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(293, 23);
            this.label18.TabIndex = 5;
            this.label18.Text = "High Temperature Time        (60)";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(7, 392);
            this.label22.Margin = new System.Windows.Forms.Padding(7, 35, 3, 3);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(292, 23);
            this.label22.TabIndex = 0;
            this.label22.Text = "Prepare 2 Time                    (60)";
            // 
            // tabStatus
            // 
            this.tabStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(165)))), ((int)(((byte)(250)))));
            this.tabStatus.Controls.Add(this.lbl_InstrumentCycles);
            this.tabStatus.Controls.Add(this.lbl36);
            this.tabStatus.Controls.Add(this.groupBox5);
            this.tabStatus.Controls.Add(this.groupBox2);
            this.tabStatus.Controls.Add(this.txtBox_TempLowCorrectionSlope);
            this.tabStatus.Controls.Add(this.bttn_tempLowSlope);
            this.tabStatus.Controls.Add(this.bttn_tempMidSlope);
            this.tabStatus.Controls.Add(this.bttn_tempHiSlope);
            this.tabStatus.Controls.Add(this.txtBox_TempMidCorrectionSlope);
            this.tabStatus.Controls.Add(this.txtBox_TempHiCorrectionSlope);
            this.tabStatus.Controls.Add(this.bttnHelpService1);
            this.tabStatus.Controls.Add(this.bttn_OK_tabStatus);
            this.tabStatus.Controls.Add(this.textBox_Error2);
            this.tabStatus.Controls.Add(this.lbl_ControlerStatus);
            this.tabStatus.Controls.Add(this.txt_Box_CycleLine);
            this.tabStatus.Controls.Add(this.txtBox_TTL_Values);
            this.tabStatus.Controls.Add(this.txtBox_CycleTimers);
            this.tabStatus.Controls.Add(this.txtBox_TTL);
            this.tabStatus.Controls.Add(this.bttn_Temperature);
            this.tabStatus.Controls.Add(this.textBox3);
            this.tabStatus.Controls.Add(this.bttn_heater);
            this.tabStatus.Controls.Add(this.textBox2);
            this.tabStatus.Controls.Add(this.chartControl);
            this.tabStatus.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabStatus.Location = new System.Drawing.Point(4, 28);
            this.tabStatus.Name = "tabStatus";
            this.tabStatus.Padding = new System.Windows.Forms.Padding(3);
            this.tabStatus.Size = new System.Drawing.Size(1152, 788);
            this.tabStatus.TabIndex = 0;
            this.tabStatus.Text = "Service Actions";
            // 
            // lbl_InstrumentCycles
            // 
            this.lbl_InstrumentCycles.AutoSize = true;
            this.lbl_InstrumentCycles.BackColor = System.Drawing.Color.Transparent;
            this.lbl_InstrumentCycles.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_InstrumentCycles.Location = new System.Drawing.Point(831, 51);
            this.lbl_InstrumentCycles.Name = "lbl_InstrumentCycles";
            this.lbl_InstrumentCycles.Size = new System.Drawing.Size(157, 19);
            this.lbl_InstrumentCycles.TabIndex = 52;
            this.lbl_InstrumentCycles.Text = "lbl_InstrumentCycles";
            this.lbl_InstrumentCycles.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lbl36
            // 
            this.lbl36.AutoSize = true;
            this.lbl36.BackColor = System.Drawing.Color.Transparent;
            this.lbl36.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl36.Location = new System.Drawing.Point(693, 51);
            this.lbl36.Name = "lbl36";
            this.lbl36.Size = new System.Drawing.Size(142, 19);
            this.lbl36.TabIndex = 51;
            this.lbl36.Text = "Instrument Cycles:";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.button2);
            this.groupBox5.Controls.Add(this.chkBx_IgnoreGCPrepare);
            this.groupBox5.Controls.Add(this.chkBx_IgnorePrepare_2);
            this.groupBox5.Controls.Add(this.checkBox_UsePrepare2_Timer);
            this.groupBox5.Controls.Add(this.chkBx_IgnoreCyroTimeout);
            this.groupBox5.Controls.Add(this.chkBx_SingleRun);
            this.groupBox5.Location = new System.Drawing.Point(626, 519);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(515, 193);
            this.groupBox5.TabIndex = 50;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Setup";
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(250)))), ((int)(((byte)(255)))));
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(324, 26);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(129, 24);
            this.button2.TabIndex = 48;
            this.button2.Text = "Reset Trap Counter";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // chkBx_IgnoreGCPrepare
            // 
            this.chkBx_IgnoreGCPrepare.AutoSize = true;
            this.chkBx_IgnoreGCPrepare.Location = new System.Drawing.Point(9, 26);
            this.chkBx_IgnoreGCPrepare.Name = "chkBx_IgnoreGCPrepare";
            this.chkBx_IgnoreGCPrepare.Size = new System.Drawing.Size(257, 23);
            this.chkBx_IgnoreGCPrepare.TabIndex = 26;
            this.chkBx_IgnoreGCPrepare.Text = "Don\'t wait for GC Prepare Signal";
            this.toolTip1.SetToolTip(this.chkBx_IgnoreGCPrepare, "Do ignore GC Prepare Signal (Updates Controler immediately)");
            this.chkBx_IgnoreGCPrepare.UseVisualStyleBackColor = true;
            this.chkBx_IgnoreGCPrepare.CheckedChanged += new System.EventHandler(this.chkBx_IgnoreGCPrepare_CheckedChanged);
            // 
            // chkBx_IgnorePrepare_2
            // 
            this.chkBx_IgnorePrepare_2.AutoSize = true;
            this.chkBx_IgnorePrepare_2.Checked = true;
            this.chkBx_IgnorePrepare_2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBx_IgnorePrepare_2.Location = new System.Drawing.Point(9, 58);
            this.chkBx_IgnorePrepare_2.Name = "chkBx_IgnorePrepare_2";
            this.chkBx_IgnorePrepare_2.Size = new System.Drawing.Size(249, 23);
            this.chkBx_IgnorePrepare_2.TabIndex = 38;
            this.chkBx_IgnorePrepare_2.Text = "Don\'t wait for Prepare_2 Signal";
            this.toolTip1.SetToolTip(this.chkBx_IgnorePrepare_2, "Do ignore Prepare_2 Signal (Updates Controler immediately)");
            this.chkBx_IgnorePrepare_2.UseVisualStyleBackColor = true;
            this.chkBx_IgnorePrepare_2.CheckedChanged += new System.EventHandler(this.chkBx_IgnorePrepare_2_CheckedChanged);
            // 
            // checkBox_UsePrepare2_Timer
            // 
            this.checkBox_UsePrepare2_Timer.AutoSize = true;
            this.checkBox_UsePrepare2_Timer.Checked = true;
            this.checkBox_UsePrepare2_Timer.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_UsePrepare2_Timer.Location = new System.Drawing.Point(9, 85);
            this.checkBox_UsePrepare2_Timer.Name = "checkBox_UsePrepare2_Timer";
            this.checkBox_UsePrepare2_Timer.Size = new System.Drawing.Size(322, 23);
            this.checkBox_UsePrepare2_Timer.TabIndex = 47;
            this.checkBox_UsePrepare2_Timer.Text = "Use Prepare_2 Timer   (instead of Signal)";
            this.toolTip1.SetToolTip(this.checkBox_UsePrepare2_Timer, "Do ignore GC Prepare Signal (Updtaes Controler immediately)");
            this.checkBox_UsePrepare2_Timer.UseVisualStyleBackColor = true;
            this.checkBox_UsePrepare2_Timer.CheckedChanged += new System.EventHandler(this.checkBox_UsePrepare2_Timer_CheckedChanged);
            // 
            // chkBx_IgnoreCyroTimeout
            // 
            this.chkBx_IgnoreCyroTimeout.AutoSize = true;
            this.chkBx_IgnoreCyroTimeout.Location = new System.Drawing.Point(9, 114);
            this.chkBx_IgnoreCyroTimeout.Name = "chkBx_IgnoreCyroTimeout";
            this.chkBx_IgnoreCyroTimeout.Size = new System.Drawing.Size(457, 23);
            this.chkBx_IgnoreCyroTimeout.TabIndex = 24;
            this.chkBx_IgnoreCyroTimeout.Text = "Ignore 5 min. Cryo Timeout   (monitored during cool down)";
            this.toolTip1.SetToolTip(this.chkBx_IgnoreCyroTimeout, "Cryo timeout will be checked during cool down step. NOT during Pre Inject Time.");
            this.chkBx_IgnoreCyroTimeout.UseVisualStyleBackColor = true;
            this.chkBx_IgnoreCyroTimeout.CheckedChanged += new System.EventHandler(this.chkBx_IgnoreCyroTimeout_CheckedChanged);
            // 
            // chkBx_SingleRun
            // 
            this.chkBx_SingleRun.AutoSize = true;
            this.chkBx_SingleRun.Location = new System.Drawing.Point(9, 155);
            this.chkBx_SingleRun.Name = "chkBx_SingleRun";
            this.chkBx_SingleRun.Size = new System.Drawing.Size(325, 23);
            this.chkBx_SingleRun.TabIndex = 37;
            this.chkBx_SingleRun.Text = "Method start from MassHunter Integration";
            this.chkBx_SingleRun.UseVisualStyleBackColor = true;
            this.chkBx_SingleRun.CheckedChanged += new System.EventHandler(this.chkBx_SingleRun_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.bttn_RestartCycle);
            this.groupBox2.Controls.Add(this.bttn_NextStep);
            this.groupBox2.Controls.Add(this.bttn_SwitchStepMode);
            this.groupBox2.Controls.Add(this.chkBx_IgnoreSyncSignals);
            this.groupBox2.Location = new System.Drawing.Point(450, 519);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(142, 193);
            this.groupBox2.TabIndex = 49;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Debug";
            // 
            // bttn_RestartCycle
            // 
            this.bttn_RestartCycle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(250)))), ((int)(((byte)(255)))));
            this.bttn_RestartCycle.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.bttn_RestartCycle.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bttn_RestartCycle.Location = new System.Drawing.Point(12, 143);
            this.bttn_RestartCycle.Name = "bttn_RestartCycle";
            this.bttn_RestartCycle.Size = new System.Drawing.Size(117, 35);
            this.bttn_RestartCycle.TabIndex = 21;
            this.bttn_RestartCycle.Text = "Restart Cycle";
            this.bttn_RestartCycle.UseVisualStyleBackColor = false;
            this.bttn_RestartCycle.Click += new System.EventHandler(this.bttn_RestartCycle_Click);
            // 
            // bttn_NextStep
            // 
            this.bttn_NextStep.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(250)))), ((int)(((byte)(255)))));
            this.bttn_NextStep.Enabled = false;
            this.bttn_NextStep.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.bttn_NextStep.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bttn_NextStep.Location = new System.Drawing.Point(12, 102);
            this.bttn_NextStep.Name = "bttn_NextStep";
            this.bttn_NextStep.Size = new System.Drawing.Size(117, 35);
            this.bttn_NextStep.TabIndex = 19;
            this.bttn_NextStep.Text = "Do Next Step";
            this.bttn_NextStep.UseVisualStyleBackColor = false;
            this.bttn_NextStep.Click += new System.EventHandler(this.bttn_NextStep_Click);
            // 
            // bttn_SwitchStepMode
            // 
            this.bttn_SwitchStepMode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(250)))), ((int)(((byte)(255)))));
            this.bttn_SwitchStepMode.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.bttn_SwitchStepMode.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bttn_SwitchStepMode.Location = new System.Drawing.Point(11, 58);
            this.bttn_SwitchStepMode.Name = "bttn_SwitchStepMode";
            this.bttn_SwitchStepMode.Size = new System.Drawing.Size(117, 35);
            this.bttn_SwitchStepMode.TabIndex = 18;
            this.bttn_SwitchStepMode.Text = "Step Mode";
            this.bttn_SwitchStepMode.UseVisualStyleBackColor = false;
            this.bttn_SwitchStepMode.Click += new System.EventHandler(this.bttn_SwitchStepMode_Click);
            // 
            // chkBx_IgnoreSyncSignals
            // 
            this.chkBx_IgnoreSyncSignals.AutoSize = true;
            this.chkBx_IgnoreSyncSignals.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkBx_IgnoreSyncSignals.Location = new System.Drawing.Point(12, 32);
            this.chkBx_IgnoreSyncSignals.Name = "chkBx_IgnoreSyncSignals";
            this.chkBx_IgnoreSyncSignals.Size = new System.Drawing.Size(118, 20);
            this.chkBx_IgnoreSyncSignals.TabIndex = 20;
            this.chkBx_IgnoreSyncSignals.Text = "Fake Sync Sign.";
            this.toolTip1.SetToolTip(this.chkBx_IgnoreSyncSignals, "Replace Sync Signals with 10 seconds waiting (Updates Controller immediately)");
            this.chkBx_IgnoreSyncSignals.UseVisualStyleBackColor = true;
            this.chkBx_IgnoreSyncSignals.CheckedChanged += new System.EventHandler(this.chkBx_IgnoreSyncSignals_CheckedChanged);
            // 
            // txtBox_TempLowCorrectionSlope
            // 
            this.txtBox_TempLowCorrectionSlope.Enabled = false;
            this.txtBox_TempLowCorrectionSlope.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBox_TempLowCorrectionSlope.Location = new System.Drawing.Point(697, 297);
            this.txtBox_TempLowCorrectionSlope.Name = "txtBox_TempLowCorrectionSlope";
            this.txtBox_TempLowCorrectionSlope.Size = new System.Drawing.Size(40, 22);
            this.txtBox_TempLowCorrectionSlope.TabIndex = 45;
            this.txtBox_TempLowCorrectionSlope.Text = "txtBox_TempLowCorrectionSlope";
            this.txtBox_TempLowCorrectionSlope.Visible = false;
            // 
            // bttn_tempLowSlope
            // 
            this.bttn_tempLowSlope.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(250)))), ((int)(((byte)(255)))));
            this.bttn_tempLowSlope.Enabled = false;
            this.bttn_tempLowSlope.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.bttn_tempLowSlope.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bttn_tempLowSlope.Location = new System.Drawing.Point(743, 296);
            this.bttn_tempLowSlope.Name = "bttn_tempLowSlope";
            this.bttn_tempLowSlope.Size = new System.Drawing.Size(40, 24);
            this.bttn_tempLowSlope.TabIndex = 44;
            this.bttn_tempLowSlope.Text = "Low";
            this.bttn_tempLowSlope.UseVisualStyleBackColor = false;
            this.bttn_tempLowSlope.Visible = false;
            this.bttn_tempLowSlope.Click += new System.EventHandler(this.bttn_tempLowSlope_Click);
            // 
            // bttn_tempMidSlope
            // 
            this.bttn_tempMidSlope.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(250)))), ((int)(((byte)(255)))));
            this.bttn_tempMidSlope.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.bttn_tempMidSlope.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bttn_tempMidSlope.Location = new System.Drawing.Point(743, 295);
            this.bttn_tempMidSlope.Name = "bttn_tempMidSlope";
            this.bttn_tempMidSlope.Size = new System.Drawing.Size(103, 25);
            this.bttn_tempMidSlope.TabIndex = 43;
            this.bttn_tempMidSlope.Text = "Heater Slope";
            this.bttn_tempMidSlope.UseVisualStyleBackColor = false;
            this.bttn_tempMidSlope.Click += new System.EventHandler(this.bttn_tempMidSlope_Click);
            // 
            // bttn_tempHiSlope
            // 
            this.bttn_tempHiSlope.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(250)))), ((int)(((byte)(255)))));
            this.bttn_tempHiSlope.Enabled = false;
            this.bttn_tempHiSlope.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.bttn_tempHiSlope.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bttn_tempHiSlope.Location = new System.Drawing.Point(722, 322);
            this.bttn_tempHiSlope.Name = "bttn_tempHiSlope";
            this.bttn_tempHiSlope.Size = new System.Drawing.Size(41, 24);
            this.bttn_tempHiSlope.TabIndex = 42;
            this.bttn_tempHiSlope.Text = "High";
            this.bttn_tempHiSlope.UseVisualStyleBackColor = false;
            this.bttn_tempHiSlope.Visible = false;
            this.bttn_tempHiSlope.Click += new System.EventHandler(this.bttn_tempHiSlope_Click);
            // 
            // txtBox_TempMidCorrectionSlope
            // 
            this.txtBox_TempMidCorrectionSlope.Enabled = false;
            this.txtBox_TempMidCorrectionSlope.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBox_TempMidCorrectionSlope.Location = new System.Drawing.Point(697, 296);
            this.txtBox_TempMidCorrectionSlope.Name = "txtBox_TempMidCorrectionSlope";
            this.txtBox_TempMidCorrectionSlope.Size = new System.Drawing.Size(40, 22);
            this.txtBox_TempMidCorrectionSlope.TabIndex = 41;
            this.txtBox_TempMidCorrectionSlope.Text = "txtBox_TempCorrectionSlope";
            // 
            // txtBox_TempHiCorrectionSlope
            // 
            this.txtBox_TempHiCorrectionSlope.Enabled = false;
            this.txtBox_TempHiCorrectionSlope.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBox_TempHiCorrectionSlope.Location = new System.Drawing.Point(698, 324);
            this.txtBox_TempHiCorrectionSlope.Name = "txtBox_TempHiCorrectionSlope";
            this.txtBox_TempHiCorrectionSlope.Size = new System.Drawing.Size(40, 22);
            this.txtBox_TempHiCorrectionSlope.TabIndex = 40;
            this.txtBox_TempHiCorrectionSlope.Text = "txtBox_TempCorrectionOffset";
            this.txtBox_TempHiCorrectionSlope.Visible = false;
            // 
            // bttnHelpService1
            // 
            this.bttnHelpService1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(250)))), ((int)(((byte)(255)))));
            this.bttnHelpService1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.bttnHelpService1.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bttnHelpService1.Location = new System.Drawing.Point(23, 722);
            this.bttnHelpService1.Name = "bttnHelpService1";
            this.bttnHelpService1.Size = new System.Drawing.Size(103, 48);
            this.bttnHelpService1.TabIndex = 39;
            this.bttnHelpService1.Text = "Help";
            this.toolTip1.SetToolTip(this.bttnHelpService1, "Help");
            this.bttnHelpService1.UseVisualStyleBackColor = false;
            this.bttnHelpService1.Click += new System.EventHandler(this.bttnHelpService1_Click);
            // 
            // bttn_OK_tabStatus
            // 
            this.bttn_OK_tabStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(250)))), ((int)(((byte)(255)))));
            this.bttn_OK_tabStatus.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.bttn_OK_tabStatus.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bttn_OK_tabStatus.Location = new System.Drawing.Point(998, 722);
            this.bttn_OK_tabStatus.Name = "bttn_OK_tabStatus";
            this.bttn_OK_tabStatus.Size = new System.Drawing.Size(103, 48);
            this.bttn_OK_tabStatus.TabIndex = 36;
            this.bttn_OK_tabStatus.Text = "OK";
            this.bttn_OK_tabStatus.UseVisualStyleBackColor = false;
            this.bttn_OK_tabStatus.Click += new System.EventHandler(this.bttn_OK_tabStatus_Click);
            // 
            // textBox_Error2
            // 
            this.textBox_Error2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_Error2.ForeColor = System.Drawing.Color.Red;
            this.textBox_Error2.Location = new System.Drawing.Point(150, 721);
            this.textBox_Error2.Multiline = true;
            this.textBox_Error2.Name = "textBox_Error2";
            this.textBox_Error2.ReadOnly = true;
            this.textBox_Error2.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_Error2.Size = new System.Drawing.Size(697, 49);
            this.textBox_Error2.TabIndex = 25;
            // 
            // lbl_ControlerStatus
            // 
            this.lbl_ControlerStatus.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_ControlerStatus.Location = new System.Drawing.Point(693, 15);
            this.lbl_ControlerStatus.Name = "lbl_ControlerStatus";
            this.lbl_ControlerStatus.Size = new System.Drawing.Size(448, 36);
            this.lbl_ControlerStatus.TabIndex = 23;
            this.lbl_ControlerStatus.Text = "lbl_ControlerStatus";
            this.lbl_ControlerStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txt_Box_CycleLine
            // 
            this.txt_Box_CycleLine.BackColor = System.Drawing.Color.White;
            this.txt_Box_CycleLine.Location = new System.Drawing.Point(23, 519);
            this.txt_Box_CycleLine.Multiline = true;
            this.txt_Box_CycleLine.Name = "txt_Box_CycleLine";
            this.txt_Box_CycleLine.ReadOnly = true;
            this.txt_Box_CycleLine.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txt_Box_CycleLine.Size = new System.Drawing.Size(421, 193);
            this.txt_Box_CycleLine.TabIndex = 17;
            // 
            // txtBox_TTL_Values
            // 
            this.txtBox_TTL_Values.BackColor = System.Drawing.Color.White;
            this.txtBox_TTL_Values.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtBox_TTL_Values.Enabled = false;
            this.txtBox_TTL_Values.Location = new System.Drawing.Point(789, 360);
            this.txtBox_TTL_Values.Multiline = true;
            this.txtBox_TTL_Values.Name = "txtBox_TTL_Values";
            this.txtBox_TTL_Values.Size = new System.Drawing.Size(72, 107);
            this.txtBox_TTL_Values.TabIndex = 16;
            // 
            // txtBox_CycleTimers
            // 
            this.txtBox_CycleTimers.BackColor = System.Drawing.Color.White;
            this.txtBox_CycleTimers.Enabled = false;
            this.txtBox_CycleTimers.Location = new System.Drawing.Point(874, 355);
            this.txtBox_CycleTimers.Multiline = true;
            this.txtBox_CycleTimers.Name = "txtBox_CycleTimers";
            this.txtBox_CycleTimers.Size = new System.Drawing.Size(188, 145);
            this.txtBox_CycleTimers.TabIndex = 15;
            // 
            // txtBox_TTL
            // 
            this.txtBox_TTL.BackColor = System.Drawing.Color.White;
            this.txtBox_TTL.Enabled = false;
            this.txtBox_TTL.Location = new System.Drawing.Point(698, 355);
            this.txtBox_TTL.Multiline = true;
            this.txtBox_TTL.Name = "txtBox_TTL";
            this.txtBox_TTL.Size = new System.Drawing.Size(164, 145);
            this.txtBox_TTL.TabIndex = 14;
            // 
            // bttn_Temperature
            // 
            this.bttn_Temperature.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(250)))), ((int)(((byte)(255)))));
            this.bttn_Temperature.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.bttn_Temperature.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bttn_Temperature.Location = new System.Drawing.Point(1013, 88);
            this.bttn_Temperature.Name = "bttn_Temperature";
            this.bttn_Temperature.Size = new System.Drawing.Size(128, 40);
            this.bttn_Temperature.TabIndex = 13;
            this.bttn_Temperature.Text = "Temperature";
            this.bttn_Temperature.UseVisualStyleBackColor = false;
            this.bttn_Temperature.Click += new System.EventHandler(this.bttn_Temperature_Click);
            // 
            // textBox3
            // 
            this.textBox3.BackColor = System.Drawing.Color.White;
            this.textBox3.Enabled = false;
            this.textBox3.ForeColor = System.Drawing.Color.Black;
            this.textBox3.Location = new System.Drawing.Point(697, 88);
            this.textBox3.Multiline = true;
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(300, 57);
            this.textBox3.TabIndex = 12;
            // 
            // bttn_heater
            // 
            this.bttn_heater.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(250)))), ((int)(((byte)(255)))));
            this.bttn_heater.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.bttn_heater.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bttn_heater.Location = new System.Drawing.Point(1012, 163);
            this.bttn_heater.Name = "bttn_heater";
            this.bttn_heater.Size = new System.Drawing.Size(129, 40);
            this.bttn_heater.TabIndex = 5;
            this.bttn_heater.Text = "Heater";
            this.bttn_heater.UseVisualStyleBackColor = false;
            this.bttn_heater.Click += new System.EventHandler(this.bttn_heater_Click);
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.Color.White;
            this.textBox2.Enabled = false;
            this.textBox2.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox2.Location = new System.Drawing.Point(698, 163);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(300, 122);
            this.textBox2.TabIndex = 4;
            this.textBox2.Text = "pid_string";
            // 
            // tabCommunication
            // 
            this.tabCommunication.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(165)))), ((int)(((byte)(250)))));
            this.tabCommunication.Controls.Add(this.lbl_HWVersion);
            this.tabCommunication.Controls.Add(this.label37);
            this.tabCommunication.Controls.Add(this.button1);
            this.tabCommunication.Controls.Add(this.label33);
            this.tabCommunication.Controls.Add(this.lbl_FWReqSubVersion);
            this.tabCommunication.Controls.Add(this.lbl_FWReqMainVersion);
            this.tabCommunication.Controls.Add(this.label26);
            this.tabCommunication.Controls.Add(this.bttnHelpService2);
            this.tabCommunication.Controls.Add(this.lbl_SWVersion);
            this.tabCommunication.Controls.Add(this.label24);
            this.tabCommunication.Controls.Add(this.label23);
            this.tabCommunication.Controls.Add(this.bttn_ViewControlerCycle);
            this.tabCommunication.Controls.Add(this.bttn_Communication_View);
            this.tabCommunication.Controls.Add(this.textBox_Error3);
            this.tabCommunication.Controls.Add(this.label20);
            this.tabCommunication.Controls.Add(this.lbl_FWVersion);
            this.tabCommunication.Controls.Add(this.label3);
            this.tabCommunication.Controls.Add(this.label1);
            this.tabCommunication.Controls.Add(this.labelTimerTime);
            this.tabCommunication.Controls.Add(this.buttonTimer1);
            this.tabCommunication.Controls.Add(this.button4);
            this.tabCommunication.Controls.Add(this.txtBox_ControlerCycle);
            this.tabCommunication.Controls.Add(this.button3);
            this.tabCommunication.Controls.Add(this.chkBx_LowLevelComm);
            this.tabCommunication.Controls.Add(this.chkBx_TopLevelComm);
            this.tabCommunication.Controls.Add(this.label2);
            this.tabCommunication.Controls.Add(this.txtBox_Communication);
            this.tabCommunication.Controls.Add(this.lbl_Comport);
            this.tabCommunication.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabCommunication.Location = new System.Drawing.Point(4, 28);
            this.tabCommunication.Name = "tabCommunication";
            this.tabCommunication.Padding = new System.Windows.Forms.Padding(3);
            this.tabCommunication.Size = new System.Drawing.Size(1152, 788);
            this.tabCommunication.TabIndex = 1;
            this.tabCommunication.Text = "Service Logs";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.LightGray;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(1046, 113);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(78, 56);
            this.button1.TabIndex = 45;
            this.button1.Text = "Change Firmware manually";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label33.Location = new System.Drawing.Point(972, 146);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(11, 14);
            this.label33.TabIndex = 44;
            this.label33.Text = ".";
            // 
            // lbl_FWReqSubVersion
            // 
            this.lbl_FWReqSubVersion.AutoSize = true;
            this.lbl_FWReqSubVersion.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_FWReqSubVersion.Location = new System.Drawing.Point(980, 146);
            this.lbl_FWReqSubVersion.Name = "lbl_FWReqSubVersion";
            this.lbl_FWReqSubVersion.Size = new System.Drawing.Size(125, 14);
            this.lbl_FWReqSubVersion.TabIndex = 43;
            this.lbl_FWReqSubVersion.Text = "lbl_FWReqSubVersion";
            // 
            // lbl_FWReqMainVersion
            // 
            this.lbl_FWReqMainVersion.AutoSize = true;
            this.lbl_FWReqMainVersion.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_FWReqMainVersion.Location = new System.Drawing.Point(960, 146);
            this.lbl_FWReqMainVersion.Name = "lbl_FWReqMainVersion";
            this.lbl_FWReqMainVersion.Size = new System.Drawing.Size(128, 14);
            this.lbl_FWReqMainVersion.TabIndex = 42;
            this.lbl_FWReqMainVersion.Text = "lbl_FWReqMainVersion";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label26.Location = new System.Drawing.Point(799, 144);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(131, 16);
            this.label26.TabIndex = 41;
            this.label26.Text = "requested eTrap FW:";
            // 
            // bttnHelpService2
            // 
            this.bttnHelpService2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(250)))), ((int)(((byte)(255)))));
            this.bttnHelpService2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.bttnHelpService2.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bttnHelpService2.Location = new System.Drawing.Point(23, 725);
            this.bttnHelpService2.Name = "bttnHelpService2";
            this.bttnHelpService2.Size = new System.Drawing.Size(103, 48);
            this.bttnHelpService2.TabIndex = 40;
            this.bttnHelpService2.Text = "Help";
            this.toolTip1.SetToolTip(this.bttnHelpService2, "Help");
            this.bttnHelpService2.UseVisualStyleBackColor = false;
            this.bttnHelpService2.Click += new System.EventHandler(this.bttnHelpService2_Click);
            // 
            // lbl_SWVersion
            // 
            this.lbl_SWVersion.AutoSize = true;
            this.lbl_SWVersion.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_SWVersion.Location = new System.Drawing.Point(959, 17);
            this.lbl_SWVersion.Name = "lbl_SWVersion";
            this.lbl_SWVersion.Size = new System.Drawing.Size(128, 23);
            this.lbl_SWVersion.TabIndex = 39;
            this.lbl_SWVersion.Text = "lbl_SWVersion";
            this.lbl_SWVersion.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label24.Location = new System.Drawing.Point(818, 17);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(112, 23);
            this.label24.TabIndex = 38;
            this.label24.Text = "SW Version:";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label23.Location = new System.Drawing.Point(776, 229);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(115, 23);
            this.label23.TabIndex = 37;
            this.label23.Text = "eTrap Cycle:";
            // 
            // bttn_ViewControlerCycle
            // 
            this.bttn_ViewControlerCycle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(250)))), ((int)(((byte)(255)))));
            this.bttn_ViewControlerCycle.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.bttn_ViewControlerCycle.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bttn_ViewControlerCycle.Location = new System.Drawing.Point(883, 657);
            this.bttn_ViewControlerCycle.Name = "bttn_ViewControlerCycle";
            this.bttn_ViewControlerCycle.Size = new System.Drawing.Size(103, 48);
            this.bttn_ViewControlerCycle.TabIndex = 36;
            this.bttn_ViewControlerCycle.Text = "View";
            this.toolTip1.SetToolTip(this.bttn_ViewControlerCycle, "View and Print");
            this.bttn_ViewControlerCycle.UseVisualStyleBackColor = false;
            this.bttn_ViewControlerCycle.Click += new System.EventHandler(this.bttn_ViewControlerCycle_Click);
            // 
            // bttn_Communication_View
            // 
            this.bttn_Communication_View.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(250)))), ((int)(((byte)(255)))));
            this.bttn_Communication_View.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.bttn_Communication_View.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bttn_Communication_View.Location = new System.Drawing.Point(456, 657);
            this.bttn_Communication_View.Name = "bttn_Communication_View";
            this.bttn_Communication_View.Size = new System.Drawing.Size(103, 48);
            this.bttn_Communication_View.TabIndex = 35;
            this.bttn_Communication_View.Text = "View";
            this.toolTip1.SetToolTip(this.bttn_Communication_View, "View and Print");
            this.bttn_Communication_View.UseVisualStyleBackColor = false;
            this.bttn_Communication_View.Click += new System.EventHandler(this.bttn_Communication_View_Click);
            // 
            // textBox_Error3
            // 
            this.textBox_Error3.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_Error3.ForeColor = System.Drawing.Color.Red;
            this.textBox_Error3.Location = new System.Drawing.Point(150, 724);
            this.textBox_Error3.Multiline = true;
            this.textBox_Error3.Name = "textBox_Error3";
            this.textBox_Error3.ReadOnly = true;
            this.textBox_Error3.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_Error3.Size = new System.Drawing.Size(998, 49);
            this.textBox_Error3.TabIndex = 34;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.Location = new System.Drawing.Point(780, 113);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(150, 23);
            this.label20.TabIndex = 33;
            this.label20.Text = "eTrap Firmware:";
            // 
            // lbl_FWVersion
            // 
            this.lbl_FWVersion.AutoSize = true;
            this.lbl_FWVersion.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_FWVersion.Location = new System.Drawing.Point(959, 113);
            this.lbl_FWVersion.Name = "lbl_FWVersion";
            this.lbl_FWVersion.Size = new System.Drawing.Size(127, 23);
            this.lbl_FWVersion.TabIndex = 32;
            this.lbl_FWVersion.Text = "lbl_FWVersion";
            this.lbl_FWVersion.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(661, 456);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 19);
            this.label3.TabIndex = 31;
            this.label3.Text = "USB port";
            this.label3.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(614, 402);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 19);
            this.label1.TabIndex = 30;
            this.label1.Text = "label1";
            this.label1.Visible = false;
            // 
            // labelTimerTime
            // 
            this.labelTimerTime.AutoSize = true;
            this.labelTimerTime.BackColor = System.Drawing.SystemColors.Control;
            this.labelTimerTime.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelTimerTime.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTimerTime.Location = new System.Drawing.Point(618, 544);
            this.labelTimerTime.Name = "labelTimerTime";
            this.labelTimerTime.Size = new System.Drawing.Size(109, 20);
            this.labelTimerTime.TabIndex = 29;
            this.labelTimerTime.Text = "labelTimerTime";
            this.labelTimerTime.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.labelTimerTime.Visible = false;
            // 
            // buttonTimer1
            // 
            this.buttonTimer1.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonTimer1.Location = new System.Drawing.Point(655, 499);
            this.buttonTimer1.Name = "buttonTimer1";
            this.buttonTimer1.Size = new System.Drawing.Size(91, 42);
            this.buttonTimer1.TabIndex = 28;
            this.buttonTimer1.Text = "Timer 1";
            this.buttonTimer1.UseVisualStyleBackColor = true;
            this.buttonTimer1.Visible = false;
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(250)))), ((int)(((byte)(255)))));
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button4.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button4.Location = new System.Drawing.Point(1021, 657);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(103, 48);
            this.button4.TabIndex = 26;
            this.button4.Text = "Refresh";
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.button4_Click_1);
            // 
            // txtBox_ControlerCycle
            // 
            this.txtBox_ControlerCycle.BackColor = System.Drawing.Color.White;
            this.txtBox_ControlerCycle.Location = new System.Drawing.Point(776, 255);
            this.txtBox_ControlerCycle.Multiline = true;
            this.txtBox_ControlerCycle.Name = "txtBox_ControlerCycle";
            this.txtBox_ControlerCycle.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtBox_ControlerCycle.Size = new System.Drawing.Size(348, 377);
            this.txtBox_ControlerCycle.TabIndex = 24;
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(250)))), ((int)(((byte)(255)))));
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button3.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.Location = new System.Drawing.Point(603, 657);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(103, 48);
            this.button3.TabIndex = 8;
            this.button3.Text = "Clear";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // chkBx_LowLevelComm
            // 
            this.chkBx_LowLevelComm.AutoSize = true;
            this.chkBx_LowLevelComm.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkBx_LowLevelComm.Location = new System.Drawing.Point(23, 680);
            this.chkBx_LowLevelComm.Name = "chkBx_LowLevelComm";
            this.chkBx_LowLevelComm.Size = new System.Drawing.Size(110, 27);
            this.chkBx_LowLevelComm.TabIndex = 6;
            this.chkBx_LowLevelComm.Text = "Low Level";
            this.chkBx_LowLevelComm.UseVisualStyleBackColor = true;
            this.chkBx_LowLevelComm.CheckedChanged += new System.EventHandler(this.chkBx_LowLevelComm_CheckedChanged);
            // 
            // chkBx_TopLevelComm
            // 
            this.chkBx_TopLevelComm.AutoSize = true;
            this.chkBx_TopLevelComm.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkBx_TopLevelComm.Location = new System.Drawing.Point(23, 650);
            this.chkBx_TopLevelComm.Name = "chkBx_TopLevelComm";
            this.chkBx_TopLevelComm.Size = new System.Drawing.Size(210, 27);
            this.chkBx_TopLevelComm.TabIndex = 5;
            this.chkBx_TopLevelComm.Text = "Show Communication";
            this.chkBx_TopLevelComm.UseVisualStyleBackColor = true;
            this.chkBx_TopLevelComm.CheckedChanged += new System.EventHandler(this.chkBx_TopLevelComm_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(835, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 23);
            this.label2.TabIndex = 4;
            this.label2.Text = "COM Port:";
            // 
            // txtBox_Communication
            // 
            this.txtBox_Communication.BackColor = System.Drawing.Color.White;
            this.txtBox_Communication.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBox_Communication.Location = new System.Drawing.Point(23, 19);
            this.txtBox_Communication.Multiline = true;
            this.txtBox_Communication.Name = "txtBox_Communication";
            this.txtBox_Communication.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtBox_Communication.Size = new System.Drawing.Size(747, 613);
            this.txtBox_Communication.TabIndex = 0;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.pictureBox1.Enabled = false;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(1105, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(52, 41);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Visible = false;
            // 
            // textBox1
            // 
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Enabled = false;
            this.textBox1.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(767, 0);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(332, 43);
            this.textBox1.TabIndex = 7;
            this.textBox1.Visible = false;
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label37.Location = new System.Drawing.Point(776, 179);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(154, 23);
            this.label37.TabIndex = 46;
            this.label37.Text = "eTrap Hardware:";
            // 
            // lbl_HWVersion
            // 
            this.lbl_HWVersion.AutoSize = true;
            this.lbl_HWVersion.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_HWVersion.Location = new System.Drawing.Point(959, 179);
            this.lbl_HWVersion.Name = "lbl_HWVersion";
            this.lbl_HWVersion.Size = new System.Drawing.Size(130, 23);
            this.lbl_HWVersion.TabIndex = 47;
            this.lbl_HWVersion.Text = "lbl_HWVersion";
            // 
            // ChartForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(165)))), ((int)(((byte)(250)))));
            this.ClientSize = new System.Drawing.Size(1160, 821);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "ChartForm";
            this.RightToLeftLayout = true;
            this.Text = "Chromtech Controler";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ChartForm_FormClosing);
            this.Load += new System.EventHandler(this.Do_ChartForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabMethod.ResumeLayout(false);
            this.tabMethod.PerformLayout();
            this.eTrapDisplayBox.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.flowLayoutPanel5.ResumeLayout(false);
            this.flowLayoutPanel5.PerformLayout();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.tabStatus.ResumeLayout(false);
            this.tabStatus.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabCommunication.ResumeLayout(false);
            this.tabCommunication.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public ZedGraph.ZedGraphControl chartControl;
        private System.Windows.Forms.Label lbl_Comport;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabStatus;
        private System.Windows.Forms.TabPage tabCommunication;
        private System.Windows.Forms.TextBox txtBox_Communication;
        private System.Windows.Forms.TabPage tabMethod;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button bttn_heater;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Button bttn_Temperature;
        private System.Windows.Forms.TextBox txtBox_TTL;
        private System.Windows.Forms.TextBox txtBox_CycleTimers;
        private System.Windows.Forms.TextBox txtBox_TTL_Values;
        private System.Windows.Forms.TextBox txt_Box_CycleLine;
        private System.Windows.Forms.Label lbl_M_StandbyTemp;
        private System.Windows.Forms.Label lbl_M_LowTemp;
        private System.Windows.Forms.Label lbl_M_LowTime_PreInject;
        private System.Windows.Forms.Label lbl_M_LowTime_PostInject;
        private System.Windows.Forms.Label lbl_M_HighTemp;
        private System.Windows.Forms.Label lbl_M_HighTemp_Time;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button bttn_SendToTrap;
        private System.Windows.Forms.Button bttn_SaveMethod;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button bttn_SwitchStepMode;
        private System.Windows.Forms.Button bttn_NextStep;
        private System.Windows.Forms.CheckBox chkBx_IgnoreSyncSignals;
        private System.Windows.Forms.Button bttn_RestartCycle;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button bttn_LoadMethod;
        private System.Windows.Forms.CheckBox chkBx_LowLevelComm;
        private System.Windows.Forms.CheckBox chkBx_TopLevelComm;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label lbl_ControlerStatus;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.TextBox txtBox_ControlerCycle;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelTimerTime;
        private System.Windows.Forms.Button buttonTimer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.TextBox tBxStandbyTemp;
        private System.Windows.Forms.TextBox tBxCoolTemperature;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox tBxPreInjTime;
        private System.Windows.Forms.TextBox tBxPostInjTime;
        private System.Windows.Forms.TextBox tBxHighTemperature;
        private System.Windows.Forms.TextBox tBxHighTempTime;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label lbl_CurrentTemperature;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label lbl_TimerTime;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.GroupBox eTrapDisplayBox;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label lbl_FWVersion;
        private System.Windows.Forms.Label lbl_MethodName;
        private System.Windows.Forms.Label lbl_MethodFileName;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label lbl_cryoTimeout;
        private System.Windows.Forms.CheckBox chkBx_IgnoreCyroTimeout;
        private System.Windows.Forms.Button bttn_MethStart;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.TextBox textBox_Error1;
        private System.Windows.Forms.TextBox textBox_Error2;
        private System.Windows.Forms.TextBox textBox_Error3;
        private System.Windows.Forms.CheckBox chkBx_IgnoreGCPrepare;
        private System.Windows.Forms.Button bttn_ViewMethod;
        private System.Windows.Forms.Button bttn_Communication_View;
        private System.Windows.Forms.Button bttn_ViewControlerCycle;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label lbl_SWVersion;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label lblnotConnectedOrFuseTempBad;
        private System.Windows.Forms.Button bttn_OK_tabMethod;
        private System.Windows.Forms.Button bttn_OK_tabStatus;
        private System.Windows.Forms.CheckBox chkBx_SingleRun;
        private System.Windows.Forms.Label lbl_SingleRun;
        private System.Windows.Forms.CheckBox chkBx_IgnorePrepare_2;
        private System.Windows.Forms.Button bttnHelpMethod;
        private System.Windows.Forms.Button bttnHelpService1;
        private System.Windows.Forms.Button bttnHelpService2;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label lbl_FWReqMainVersion;
        private System.Windows.Forms.Label lbl_FWReqSubVersion;
        private System.Windows.Forms.TextBox txtBox_TempMidCorrectionSlope;
        private System.Windows.Forms.TextBox txtBox_TempHiCorrectionSlope;
        private System.Windows.Forms.Button bttn_tempMidSlope;
        private System.Windows.Forms.Button bttn_tempHiSlope;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel5;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.Button bttn_tempLowSlope;
        private System.Windows.Forms.TextBox txtBox_TempLowCorrectionSlope;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.CheckBox checkBox_UsePrepare2_Timer;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.TextBox tBxPrepare2Time;
        private System.Windows.Forms.Label lbl_TrapCycles;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label lbl_InstrumentCycles;
        private System.Windows.Forms.Label lbl36;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.CheckBox chkInUse;
        private System.Windows.Forms.Label lbl_HWVersion;
        private System.Windows.Forms.Label label37;
    }
}

