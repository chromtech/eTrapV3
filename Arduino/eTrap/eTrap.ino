// *** eTrap *** 3.0

// int				-32.768 to 32.768					2-byte	= 16 bit	= int16  in C#
// long				-2,147,483,648 to 2,147,483,647		4 byte  = 32 bit	= double in C#
// unsigned long	0  to  4,294,967,295				4 bytes = 32 bits
//	float			-3.4028235E+38 to 3.4028235E+38		4 bytes = 32 bits	= single  in C#

#include <CmdMessenger.h>  // CmdMessenger
#include <TimerOne.h>		// Timer
#include <EEPROM.h>

// Version
int mainFWVersion = 3;
int subFWVersion = 0;

const int CycleVersion = 5;

// Varian GC
const bool IS_BRUKER = false;  // FW 2.x. True for eTrap_FW_2.x_BrukerVarianGC.hex  flase for eTrap_FW_2.x.hex

//	For Hardware Simulation:  Set both to true!
bool FAKE_SYNC_SIGNALS					    = false;			// Sent from PC ! since 2.6.1
bool FAKE_TEMPERATURE_AND_HEATERS			= false;			// NOT Sent from PC

// Set time that simulates waiting for a Sync Signal
unsigned long FAKE_SYNC_SINGALS_WAIT_SECONDS = 10 ;	// 10 seconds waiting for each Sync Signals;			

const int ON	= HIGH;
const int OFF	= LOW;

// Attach a new CmdMessenger object to the default Serial port
CmdMessenger cmdMessenger = CmdMessenger(Serial);


volatile int EEaddress = 0;


int _newInt			=	0;

// Hardware Security Settings
float	HW_Minimum_Temperature				=	20;
float	HW_Minimum_Temperature_with_Cool	=	-70;

float	HW_Maximum_Temperature				=	400;
float	HW_Maximum_Temperature_with_Cool	=	40;

unsigned long	HW_Cryotimeout_Millis		=	5 * 60000;	//5 min  * 60 * 1000

float HW_Standby_Temperature				= 70;

// PID settings
float  goalTemperature			= HW_Standby_Temperature;
float	_max_temperature		= HW_Maximum_Temperature;
float	_min_temperature		= HW_Minimum_Temperature;
float	_proportional			= 1.8;		// Used ONLY for High Temp
float	_integral				= 0.009;	// Used ONLY for High Temp
bool	_integral_on			= false;
float	_integral_sum 			= 0;
float error						= 0;
float p_error					= 0;
float i_error					= 0;
float correction_sum			= 0;
float heatUpSlope				= 1;	// =1 curr. unused

// For testing the heating Rate:
float lastHeatRateTemp			= 0;
float currentHeatRateTemp		= 0;
float lastHeatRate				= 0;	// copied from currentHeatRate
float currentHeatRate			= 0;	// calced from lastHeatRateTemp / currentHeatRateTemp
float rateToSwitchOnIntegral	= 0;	// If rate is lower :  Swicth on the Integral

// Update intervals inside loop  100 = 0.1 second interval, 10 Hz frequency
const unsigned long interval_Temperature = 50;	// was 100  0.8 sec read Temperature and Security Issues
const unsigned long interval_Htr = 50;			// was 100  0.8 sec update PID and set 
const unsigned long interval_Average = 5;		// 5 msec read Analog In 1 for average
const unsigned long interval_Cycle = 80;		//  update Cycle 
const unsigned long interval_Security = 1000;	//  update Cycle 
const unsigned long interval_HeatingRate = 1000;	//  update 

// Heater Tic  :  Frequency for switching activate the Heater
const unsigned long Millis_HeaterTics = 300;		// Fix to 1 sec  = 1000 msec !
unsigned long Millis_HeaterThisTic = 0;		

// Heater Bang  :  Duration of the Bang = How long is the Heater ON   Minimum = 0.2 sec  Max = 2 sec
unsigned long Millis_HeaterBang = 0;
unsigned long Millis_HeaterThisBang = 0;

unsigned long HeaterBang_min_Hot = 100;		//  200 msec with Trafo
unsigned long HeaterBang_min_Cool = 20;		//   20 msec with Peltiers

unsigned long HeaterBang_min = HeaterBang_min_Hot;
unsigned long HeaterBang_max = 300;



// Data logging Update
//bool LoggingData                   = false;
long startLoggingMillis            = 0;
//unsigned long previousMillis_Logging = 0;		// unused


// Input voltage and update
float AnalogIn7_Power				= 0;
const int AnalogPin7_Power			= 7;


// Temperature and Update
const int AnalogPin0_Temperature               = 0;
unsigned long Millis_Temperature	= 0;
float currentTemperature            = 30;    // Measured and corrected temperature
float secondsTemperature			= 0;
float AnalogIn0_Temperature			= 0;
float CurrentTemperatureRAW			= 50;	// Raw temperature


float TempCorrectionSlope			= 1;	// = TempCorrectionSlope_MidTemp = 'Slope' from GUI
float TempCorrectionSlope_LowTemp	= 1;	// curr. unused
float TempCorrectionSlope_MidTemp	= 1;	// Set from GUI 'Slope'
float TempCorrectionSlope_HiTemp	= 1;	// curr. unused

// PWM Update
const int PWMPin					= 3;	// D3 OUT = Pin 4 ?
int HeaterIsOn						= 0;
unsigned long Millis_Htr = 0;

//Cycle Update
unsigned long Millis_Cycle = 0;

// Security Update
unsigned long Millis_Security = 0;

// Cryo Timeout
unsigned long Millis_CryoTimeout = 0;

// do average analog in
int Average_count = 0;
unsigned long Millis_Average = 0;

// Heating Rate Update
unsigned long Millis_HeatingRate = 0;

// Blinking led variables 
bool _state_Led						= OFF;   // Current state of Led
const int _blinkLed_Pin					= 13;  // Pin of internal Led

// Timer vars
bool Timer_is_on					= false;
int every_x_milliseconds_Timer		= 1;
unsigned long Milliseconds	= 0;
int Timer_seconds				= 0;
int Timer_minutes				= 0;
int Timer_hours				= 0;



// TTL IN and OUT
const int pin_GC_Prepare			= 12;	// D12 IN = 
const int dir_GC_Prepare			= INPUT;
	  int state_GC_Prepare			= OFF;

const int pin_GC_Ready				= 11;	// D11 OUT
      int dir_GC_Ready_OUT			= OUTPUT;
	  int dir_GC_Ready_IN			= INPUT;
	  int dir_GC_Ready_STATE		= dir_GC_Ready_IN;
	  int state_GC_Ready			= ON;

const int pin_GC_Start				= 10;	// D10 IN
const int dir_GC_Start				= INPUT;
	  int state_GC_Start			= OFF;

const int pin_TRAP_Cool				=  9;	// D9 OUT
const int dir_TRAP_Cool				= OUTPUT;
	  int state_TRAP_Cool			= OFF;

	  // Version 2.x: Prepare 2 added
const int pin_Prepare_2				= 8;	// D8 IN = Pin 11   >  PAL TTL OUT 3 (pin 6)
const int dir_Prepare_2				= INPUT;
	  int state_Prepare_2			= OFF;

// other Pins connected to PAL Plug
const int pin_Unused_IN_2			= 7;	// D7 IN = Pin 5   >  PAL TTL OUT 2 (pin 5)
const int dir_Unused_IN_2			= INPUT;
	  int state_Unused_IN_2			= OFF;
const int pin_Unused_IN_1			= 6;	// D6 IN = Pin 9   >  PAL TTL OUT 2 (pin 4)
const int dir_Unused_IN_1			= INPUT;
	  int state_Unused_IN_1			= OFF;

const int pin_Unused_OUT_3			= 5;	// D5 OUT = Pin 8   >  PAL TTL IN 3 (pin 3)
const int dir_Unused_OUT_3			= OUTPUT;
	  int state_Unused_OUT_3		= OFF;
const int pin_Unused_OUT_2			= 4;	// D4 OUT = Pin 7   >  PAL TTL IN 3 (pin 2)
const int dir_Unused_OUT_2			= OUTPUT;
	  int state_Unused_OUT_2		= OFF;
const int pin_Unused_OUT_1			= 2;	// D2 OUT = Pin 5   >  PAL TTL IN 3 (pin 1)
const int dir_Unused_OUT_1			= OUTPUT;
	  int state_Unused_OUT_1		= OFF;

//_________________________________________
//  C Y C L E S
//	Cycle Lines
#define max_Lines				25	// char = max 255
	int Line					= 0;	// current Line of the Cycle
	int ATOM[max_Lines];				// Atom Sequence
	int PARM1[max_Lines];				// Parameter 1 of an atom
	int PARM2[max_Lines];				// Parameter 2 of an atom
	int PARM3[max_Lines];				// Parameter 3 of an atom
	 



//	Cycle Atoms
	enum ATOM
	{						//								 PARM1(0-255)	PARM2				PARM3
		SET_TEMP,			// 2 Parms  Set_Temp			(Temp,			Accuracy[0-10, 99])
		SWITCH_EVENT,		// 3 Parms  Switch_Event		(Event,			Signal State,		Pulse time)	Pulse Time 0= just set
		WAIT_SYNC_SIG,		// 2 Parm	Wait_Sync_Signal	(Signal,		Signal State )
		WAIT_TIMER,			// 2 Parms	Wait Timer			(Timer,			time(sec))
		START_TIMER,
		END_OF_CYCLE,		// Signal to end the cycle
		SET_VARIABLE
	};

	// PARM1
	enum PARM1
	{	TRAP_COOL,
		GC_READY,
		GC_PREPARE,
		GC_START,
		PREPARE_2_TIME,
        eTrap,
		SINGLE_RUN_MODE,
		PREPARE_2
	};

	// Timer Arrays
	#define max_timers	5
	unsigned long	START_TIME_OF_TIMER[max_timers]	= {0};
	unsigned long	WAIT_TIME_OF_TIMER[max_timers]	= {0};
	unsigned long	CURRENT_TIMER_TIME[max_timers]	= {0};
	bool TIMER_IS_STARTED[max_timers]	={false};


//	Cycle and Controler Status
	bool STATUS_LINE_IS_DONE				= false;	// 
	bool STATUS_CYCLE_RUNNING				= false;	// Do we run anything?
	bool STATUS_CYCLE_STEP_MODE				= false;
	bool STATUS_CYCLE_NEXT_STEP_PLEASE		= false;	// used in Step Mode
	bool STATUS_CYCLE_RESTART_PLEASE		= false;
	bool STATUS_SYSTEM_IS_SECURE			= false;
	bool STATUS_PC_WANT_HEATERS_OFF			= false;	// In case User switch off, do not switch on again!
	bool STATUS_WAIT_FOR_TEMP				= false;	// Set if Temp is reached
	bool STATUS_CRYO_TIMEOUT				= false;	// Set if HW_CryoTimeout.  
														// Reset Cycle, Trap_Cool off, but GC_Ready OFF. 
														// Reset only by loading of new method
	bool STATUS_CRYO_TIMEOUT_IGNORE			= false;
	bool MESSAGE_CRYO_TIMEOUT_TO_PC			= false;	// leave this true until new method is loaded
	bool STATUS_WAIT_FOR_START_SIGNAL		= false;	// for Cryo Timeout

// Temperature and Heaters: No PWM will be send out
//							Temperature will go up and down according to the goalTemperature
	bool fakeTemperatureRising				= true;
	//bool fakeTemperatureFalling				= false;

// Sync Signals	
	const int fakeSyncSignals_timer			= 4;			// This is the TIMER, not the time !!!
	bool IGNORE_GC_PREPARE					= false;		// Sent from PC (= 'Dont wait for GC Prepare Signal' checkBox)
	bool IGNORE_PREPARE_2					= true;			// Sent from PC (= 'Dont wait for Prepare_2 Signal'  checkBox)


//	System Setup:
	bool InSingleRunMode					= true;					// default: Standby at startup
	float SETUP_STANDBY_TEMPERATURE			= HW_Standby_Temperature;	// changed during setup()

//	Prepare_2 Timer optional instead of Prepare_2 Signal
	bool USE_PREPARE_2_TIMER = false;
	unsigned long	Prepare_2_WaitTime		= 60;
	const int		PREPARE_2_TIMER			= 3;



// Commands
enum commands
{	// This is the list of recognized commands. These can be commands that can either be sent or received. 
	// In order to receive, attach a callback function to these events
	kAcknowledge,			// Command to acknowledge that cmd was received
	kError,				// Command to report errors
  //kSwitch_CycleStepMode,		// Set to Step Mode
  //kNextStepPlease,

	  kGetFWAndSavedParms,
	  kGetFWAndSavedParmsResult,

	  kErrorWithArgs,		// Error with some Args  (like kPlotDataPoint  in DoLogging)

	  kAskControlerCycle,
	  kSendCycleToPC,

	  kAskForReadbacks,
	  kSendReadbacksToPC,

	  kParmsToControler,
	  kParmsReceived,
};


void attachCommandCallbacks()
{	// Commands we send from the PC and want to receive on the Arduino.
	// We must define a callback function in our Arduino program for each entry in the list below.
	
	cmdMessenger.attach(OnUnknownCommand);
	//cmdMessenger.attach(kSwitch_CycleStepMode, OnSwitch_CycleStepMode);
	//cmdMessenger.attach(kNextStepPlease, OnNextStepPlease);
	cmdMessenger.attach(kGetFWAndSavedParms, OnGetFWAndSavedParms);

	cmdMessenger.attach(kAskControlerCycle, OnAskControlerCycle);

	cmdMessenger.attach(kAskForReadbacks, OnAskForReadbacks);

	cmdMessenger.attach(kParmsToControler, OnParmsToControler);
}

/// ------------------  C A L L B A C K S -----------------------
void OnGetFWAndSavedParms()
{	  
  // Send back the FW Versin 0.9
  // cmdMessenger.sendCmd(kGetFWAndSavedParms, Main, Sub);

	getFirmwareAndParms();

	cmdMessenger.sendCmdStart(kGetFWAndSavedParmsResult);
	cmdMessenger.sendCmdArg(mainFWVersion);
	cmdMessenger.sendCmdArg(subFWVersion);
	cmdMessenger.sendCmdArg((int)(TempCorrectionSlope_LowTemp*1000));
	cmdMessenger.sendCmdArg((int)(TempCorrectionSlope_MidTemp*1000));
	cmdMessenger.sendCmdArg((int)(TempCorrectionSlope_HiTemp*1000));
	cmdMessenger.sendCmdEnd();
}

void OnAskControlerCycle()
{	
	bool done = false;
	int myLine = 0;

	cmdMessenger.sendCmdStart(kSendCycleToPC);
	while ( (!done) && (myLine<max_Lines) )
	{	
		cmdMessenger.sendCmdArg(myLine);
		cmdMessenger.sendCmdArg(ATOM[myLine]);
		cmdMessenger.sendCmdArg(PARM1[myLine]);
		cmdMessenger.sendCmdArg(PARM2[myLine]);
		cmdMessenger.sendCmdArg(PARM3[myLine]);	
		if (ATOM[myLine] == END_OF_CYCLE)			
			done = true;

		myLine++;		
	}
	cmdMessenger.sendCmdEnd();
}

void _switch_StepMode(bool mode)
{	STATUS_CYCLE_STEP_MODE	= mode;
	STATUS_CYCLE_NEXT_STEP_PLEASE	= false;
}

void _nextStepPlease()
{	if (STATUS_CYCLE_STEP_MODE)
		STATUS_CYCLE_NEXT_STEP_PLEASE	= true; 
}

// Called when a received command has no attached function
void OnUnknownCommand()
{
  cmdMessenger.sendCmd(kError,"Command without attached callback");
}
// Callback function that responds that Arduino is ready (has booted up)
void OnArduinoReady()
{  cmdMessenger.sendCmd(kAcknowledge,"Chromtech Controler is ready");
}

// S E T U P and L O O P
void setup() 
{
	if (FAKE_TEMPERATURE_AND_HEATERS)		// check FAKE_TEMPERATURE_AND_HEATERS
	{	currentTemperature		= 70;
		CurrentTemperatureRAW	= 50;
	}

	// Reference 1.1V for Analog A0-A7		1024/1.1 = 930.9  ?
	//analogReference(INTERNAL);		// for LM35 : /9.31   ? aber es geht...

	// verified
	// Ref 3.3V from Pin 17		3.3/1024 = =0.003223;  *1000=3.223
	analogReference(EXTERNAL);		

	
	//  Timer Setup
	timer_reset();
	Timer1.initialize(every_x_milliseconds_Timer*1000);
	Timer1.attachInterrupt(millisecond_timer);

	// Listen on serial connection for messages from the pc
	Serial.begin(115200); 

	// Set Pins for 
	pinMode(_blinkLed_Pin, OUTPUT);					// blink LED
	pinMode(pin_GC_Prepare, dir_GC_Prepare);	// pin_GC_Prepare
	//pinMode(pin_GC_Ready, dir_GC_Ready);		// pin_GC_Ready  can be Input or output
	if (IS_BRUKER)
	{
		pinMode(pin_GC_Ready, dir_GC_Ready_OUT);		// pin_GC_Ready  can be Input or output
		set_pin_GC_Ready(OFF);					// Varian GC: Ready for PAL
	}
	else
	{
		
		set_pin_GC_Ready(ON);	// change to Input state
	}



	pinMode(pin_TRAP_Cool, dir_TRAP_Cool);		// Trap Cooling 
		set_pin_TRAP_Cool_and_Consequences(OFF);
	pinMode(pin_GC_Start, dir_GC_Start);		// pin_GC_Start

	// Adds newline to every command
	cmdMessenger.printLfCr();   

	// Attach my application's user-defined callback methods
	attachCommandCallbacks();

	// Send the status to the PC that says the Arduino has booted
	cmdMessenger.sendCmd(kAcknowledge,"CController has started!");

  	// EEPROM:  check Firmware and stored InSingleRunMode flag
	checkFWandSETUPs();

	// restore OR create the Cycle and Set some Global Parms from the Cycle
	reset_cycle_array();

	Do_CreateCycle();
	if ( !eeRestoreCycle() )
	{
		Do_CreateCycle();			
		saveFWandSETUPs();			// save SINGLE_RUN_MODE flag set above	
		eeSaveCycle();				// and save the Built in Standard Cycle
									// will be overwritten from PC later...
	}

	reset_all_timers();
	Line=0;
	STATUS_CYCLE_STEP_MODE		= false;
	STATUS_WAIT_FOR_TEMP		= false;
	STATUS_LINE_IS_DONE			= false;
	STATUS_CRYO_TIMEOUT			= false;
	STATUS_CRYO_TIMEOUT_IGNORE	= false;
	STATUS_CYCLE_RESTART_PLEASE = false;
	USE_PREPARE_2_TIMER = false;

	if ( !InSingleRunMode )
	{	
		STATUS_CYCLE_RUNNING		= true;
		STATUS_CYCLE_RESTART_PLEASE = true;
	}
	else
	{	STATUS_CYCLE_RUNNING		= false;
		STATUS_CYCLE_RESTART_PLEASE = false;
	}
  
}
void loop() 
{
	// Process incoming serial data, and perform callbacks
	cmdMessenger.feedinSerialData();

	// Do average AnalogIn 1 and In 7
	if ( Millis_Average > interval_Average)
	{	average_AnalogPin0_Temperature();
		Millis_Average = 0;
	}

	// System Security
	if ( Millis_Security > interval_Security)
	{	updateSecurityIssues();
		Millis_Security = 0;
	}

	// System Security
	if ( Millis_HeatingRate > interval_HeatingRate)
	{	updateHeatingRateIssues();
		Millis_HeatingRate = 0;
	}

	// Do Temperatures after certain sample interval
	if ( Millis_Temperature > interval_Temperature ) 
	{		measure_Temperatures_from_PT1000();
			Millis_Temperature	= 0;
	}
  
	// Do PID Calculations after certain sample interval
	if ( Millis_Htr > interval_Htr )
	{   update_pid();
		Millis_Htr = 0;
	}

	if (state_TRAP_Cool == OFF)					// Cooling only
	{
		Millis_CryoTimeout = 0;
	}

	Update_TTLs();		// for Security and Cycle Update

	// Update PWM
	if (STATUS_SYSTEM_IS_SECURE)
		if (STATUS_PC_WANT_HEATERS_OFF)
			HeaterIsOn = 0;
		else
			HeaterIsOn = 1;
	else
		HeaterIsOn = 0;


	// Heater Tics and Bangs
	//  1. If this Bang is over: switch off heater
	if ( Millis_HeaterThisBang > Millis_HeaterBang )
	{   
		analogWrite(PWMPin, OFF);		// Bang is over: Switch off
	}

	//  2. Is it Time to start a new Tic?
	if ( Millis_HeaterThisTic > Millis_HeaterTics )
	{   
		if ( ( FAKE_TEMPERATURE_AND_HEATERS ) && ( HeaterIsOn ) )
			analogWrite(PWMPin, OFF);
		else
		{
			if (Millis_HeaterBang <= HeaterBang_min)	// If Bang is required
				analogWrite(PWMPin, OFF);
			else
			{
				if (STATUS_SYSTEM_IS_SECURE)
					analogWrite(PWMPin, 255);			// Usually Start a new Bang
			}
		}
		Millis_HeaterThisBang = 0;
		Millis_HeaterThisTic = 0;
	}


	// Security:
	if (!STATUS_SYSTEM_IS_SECURE)		
		analogWrite(PWMPin, OFF);


	// work on Cycle  and Cryo Timeout
	if ( Millis_Cycle > interval_Cycle )
	{   
		if (STATUS_CRYO_TIMEOUT)
		{	
			set_pin_TRAP_Cool_and_Consequences(OFF);	// remove the CoolTrap
			set_pin_GC_Ready(OFF);						// Block GC from getting ready	// VarianGC: READY for PAL Low (is already low)
			//STATUS_CRYO_TIMEOUT				= false;
			_stopCycle();
			//STATUS_CYCLE_RESTART_PLEASE		= true;
			MESSAGE_CRYO_TIMEOUT_TO_PC		= true;
		}
		else
		{
			if (STATUS_SYSTEM_IS_SECURE)  
				Do_WorkOnCycle();
			else
				{	
					
					set_pin_TRAP_Cool_and_Consequences(OFF);	// minimum Security for now
					_stopCycle();
				}
		
		}

		Millis_Cycle = 0;
	}

}


// EEProm:  Save and Restore Cycle
//		EEadress = 20			= Startadress
//		EEadress	+0			= Macroversion
//		EEadress	+2			= reserved for future
//		EEadress	+4			= Start of Macro
//		EEadress	+maxLines-1	= last Macro line
void eeSaveCycle()
{	
	EEaddress = 20;
	eeWriteInt(CycleVersion);
	eeWriteInt(0);			// For future use

	int i=0;
	while ( i< max_Lines)	
	{	eeWriteInt(ATOM[i]);
		eeWriteInt(PARM1[i]);
		eeWriteInt(PARM2[i]);
		eeWriteInt(PARM3[i]);
		i++;
	}
}
bool eeRestoreCycle()
{	
	int _storedCycleVersion	= 0;
	//bool needUpdate_to_2	= false;
	//bool needUpdate_to_3	= false;
	//bool needUpdate_to_current	= false;
	int i					= 0;
	//int _start				= 0;
	int _unused				= 0;
	//bool _lineError			= false;
	int a, p1, p2, p3;

	EEaddress = 20;

	// First 2 items are CycleVersion and unused:
	_storedCycleVersion = eeReadInt();

	if (_storedCycleVersion != CycleVersion)	return false;

	_unused = eeReadInt();	
	i = 0;

	// Now restore the Cycle
	while ( i < max_Lines )
	{	
		a	= eeReadInt();		p1	= eeReadInt();		p2	= eeReadInt();		p3	= eeReadInt();
		if (!isAtomOK(i, a))
			return false;
		ATOM[i]		= a;		PARM1[i]	= p1;		PARM2[i]	= p2;		PARM3[i]	= p3;
		i++;
	}

	// Last item in line 17!
	if ( ATOM[17] == END_OF_CYCLE )
		{	
			// set some Globals from the restored Cycle:
			SETUP_STANDBY_TEMPERATURE = (float)PARM1[2];	// use Cycle StandbyTemperature
			//goalTemperature = SETUP_STANDBY_TEMPERATURE;
			set_goalTemperature (SETUP_STANDBY_TEMPERATURE);

			InSingleRunMode = (PARM1[17]==1? true:false);
			return true;
		}
	else
		{return false;
		}

}
bool isAtomOK(int _testLine, int _testAtom)
{
	if (ATOM[_testLine]	!= _testAtom)
		return false;

	return true;
}

// FIRMWARE VERSION  This is run at setup()
// we can use EEaddress 0-8.
//	0				= mainFWVersion
//	2				= subFWVersion
//	4				= InSingleRunMode Flag 
//  6				= TempCorrectionSlope_LowTemp
//  8				= TempCorrectionSlope_MidTemp
//  10				= TempCorrectionSlope_HiTemp
//	12-18			= unused
//	20				= CycleVersion
//	22				= unused
//	24 - max_Lines	= Cycle
void checkFWandSETUPs()
{	
	bool _doWrite = false;
	float b;
	int a;

	EEaddress = 0;							// GLOBAL !!	
	int m = eeReadInt();	// 0
	int s = eeReadInt();	// 2

	if ( (mainFWVersion!=m) || (subFWVersion!=s) )
		_doWrite = true;

	a = eeReadInt();		// 4
	if ( a == 1 )			
		InSingleRunMode = true;
	else
	{	
		// if unused before we may see anything... So set it to false
		InSingleRunMode = false;		// default: Cycle
		_doWrite = true;
	}

	b = (float)eeReadInt()/1000;				// 6 TempCorrectionSlope_LowTemp
	if ( (b >= 0.1) && (b<=10) )	
		TempCorrectionSlope_LowTemp = b;
	else
	{	TempCorrectionSlope_LowTemp = 1;		// default: TempCorrectionSlope_LowTemp = 1
		_doWrite = true;
	}

	b = (float)eeReadInt()/1000;				// 8 TempCorrectionSlope_MidTemp
	if ( (b >= 0.1) && (b<=10) )	
		TempCorrectionSlope_MidTemp = b;
	else
	{	TempCorrectionSlope_MidTemp = 1;		// default: TempCorrectionSlope_MidTemp = 1
		_doWrite = true;
	}

	b = (float)eeReadInt()/1000;				// 10 TempCorrectionSlope_HiTemp
	if ( (b >= 0.1) && (b<=10) )		
		TempCorrectionSlope_HiTemp = b;
	else
	{	TempCorrectionSlope_HiTemp = 1;		// default: TempCorrectionSlope_HiTemp = 0
		_doWrite = true;
	}

	// If one Parameter was not set: save current defaults
	if ( _doWrite )
		saveFWandSETUPs();	
}
void saveFWandSETUPs()
{
	EEaddress = 0;									// GLOBAL !!
	eeWriteInt(mainFWVersion);						// 0
	eeWriteInt(subFWVersion);						// 2
	eeWriteInt( InSingleRunMode==true? 1:0 );		// 4 InSingleRunMode

	eeWriteInt( (int)(TempCorrectionSlope_LowTemp*1000) );	// 6 TempCorrectionSlope_LowTemp
	eeWriteInt( (int)(TempCorrectionSlope_MidTemp*1000) );	// 8 TempCorrectionSlope_MidTemp
	eeWriteInt( (int)(TempCorrectionSlope_HiTemp*1000) );	// 10 TempCorrectionSlope_HiTemp	
}


// only invoked by PC
void getFirmwareAndParms()
{	EEaddress = 0;							// GLOBAL !!	
	mainFWVersion = eeReadInt();
	subFWVersion = eeReadInt();

	int i=eeReadInt();
	InSingleRunMode	= (i>0 ? true:false);

	TempCorrectionSlope_LowTemp =  eeReadInt();
	TempCorrectionSlope_MidTemp =  eeReadInt();
	TempCorrectionSlope_HiTemp =  eeReadInt();

	TempCorrectionSlope_LowTemp = TempCorrectionSlope_LowTemp/1000;
	TempCorrectionSlope_MidTemp = TempCorrectionSlope_MidTemp/1000;
	TempCorrectionSlope_HiTemp = TempCorrectionSlope_HiTemp/1000;
}

// Event for Timer Interrupt
void millisecond_timer()
{	
	// set Milliseconds timer
	Milliseconds++;

	Millis_Temperature++;
	Millis_Htr++;
	Millis_Cycle++;
	Millis_Security++;
	Millis_Average++;
	Millis_HeaterThisTic++;
	Millis_HeaterThisBang++;
	Millis_CryoTimeout++;		
	Millis_HeatingRate++;

	// set hh:mm:ss
	if (Timer_is_on)
	{	if (++Timer_seconds>59)
		{	Timer_seconds=0; 
			if (++Timer_minutes>59)
			{	Timer_minutes=0;
				Timer_hours++;
			}	
		}
	}
}

void timer_reset()
{
	Milliseconds	=0;
	Timer_seconds	=0;
	Timer_minutes	=0;
	Timer_hours		=0;
}

//   C Y C L E

// version 2:	added Line 0 PAL incubation time
// version 3:	added Prepare_2 in line 6
//				END_OF_CYCLE get PARM1 = SINGLE_RUN_MODE flag. default = true
//				chg 14/15   Start Timer High Temp / High Temp
// Cycle Version 4  Some changes for Varian GCs: changed Order of GC_Prepare and Prepare 2
// Cycle version 5	Line 6: change to NOT wait to reach Temperature. Starts Cooling from ALL Temperatures
// 
void Do_CreateCycle()
	{
		Line = 0;	ATOM[Line] = SET_VARIABLE;		PARM1[Line] = PREPARE_2_TIME;		PARM2[Line] = 0;		PARM3[Line] = 0;		// 0 Wait For Prepare 2 Timer = 0
		Line++;		ATOM[Line] = SWITCH_EVENT;		PARM1[Line] = TRAP_COOL;			PARM2[Line] = OFF;		PARM3[Line] = 0;		// 1 Security
		Line++;		ATOM[Line] = SET_TEMP;			PARM1[Line] = 50;					PARM2[Line] = 99;		PARM3[Line] = 0;		// 2 Standby Temp
		Line++;		ATOM[Line] = WAIT_SYNC_SIG;		PARM1[Line] = GC_PREPARE;			PARM2[Line] = OFF;		PARM3[Line] = 0;		// 3 Wait for GC Prepare

		// V 4 change Order
		Line++;		ATOM[Line] = WAIT_SYNC_SIG;		PARM1[Line] = PREPARE_2;			PARM2[Line] = OFF;		PARM3[Line] = 0;		// 4 Wait PREPARE_2 going low
		Line++;		ATOM[Line] = SWITCH_EVENT;		PARM1[Line] = GC_READY;				PARM2[Line] = OFF;		PARM3[Line] = 0;		// 5 GC Ready to OFF		
		Line++;		ATOM[Line] = SET_TEMP;			PARM1[Line] = 50;					PARM2[Line] = 99;		PARM3[Line] = 0;		// 6 Security Temp = Standby Temp Do NOT Wait

		Line++;		ATOM[Line] = SWITCH_EVENT;		PARM1[Line] = TRAP_COOL;			PARM2[Line] = ON;		PARM3[Line] = 0;		// 7 6 Cool Trap ON
		Line++;		ATOM[Line] = SET_TEMP;			PARM1[Line] = -5;					PARM2[Line] = 1;		PARM3[Line] = 0;		// 8 7 Low Temp

		Line++;		ATOM[Line] = WAIT_TIMER;		PARM1[Line] = 0;					PARM2[Line] = 30;		PARM3[Line] = 0;		// 9 Pre Inject Low Time
		Line++;		ATOM[Line] = SWITCH_EVENT;		PARM1[Line] = GC_READY;				PARM2[Line] = ON;		PARM3[Line] = 0;		// 10 Set GC to Ready

		Line++;		ATOM[Line] = WAIT_SYNC_SIG;		PARM1[Line] = GC_START;				PARM2[Line] = OFF;		PARM3[Line] = 0;		// 11 Wait GC Start

		Line++;		ATOM[Line] = WAIT_TIMER;		PARM1[Line] = 0;					PARM2[Line] = 5;		PARM3[Line] = 0;		// 12 Post Inject Low Time
		Line++;		ATOM[Line] = SWITCH_EVENT;		PARM1[Line] = TRAP_COOL;			PARM2[Line] = OFF;		PARM3[Line] = 0;		// 13 Cool Trap OFF

		Line++;		ATOM[Line] = SET_TEMP;			PARM1[Line] = 60;					PARM2[Line] = 1;		PARM3[Line] = 0;		// 14 High Temp
		Line++;		ATOM[Line] = START_TIMER;		PARM1[Line] = 1;					PARM2[Line] = 0;		PARM3[Line] = 0;		// 15 Start Timer High Temp

		Line++;		ATOM[Line] = WAIT_TIMER;		PARM1[Line] = 1;					PARM2[Line] = 90;		PARM3[Line] = 0;		// 16 Wait Timer High Temp Is already Started
		
																  InSingleRunMode = true;
		Line++;		ATOM[Line] = END_OF_CYCLE;		PARM1[Line] = SINGLE_RUN_MODE;					PARM2[Line] = 0;		PARM3[Line] = 0; // 17 End of Cycle AND SINGLE_RUN_MODE-flag in PARM1 default=true


		// set some Globals from the restored Cycle:
		SETUP_STANDBY_TEMPERATURE = (float)PARM1[2];	// use Cycle StandbyTemperature
		//goalTemperature = SETUP_STANDBY_TEMPERATURE;
		set_goalTemperature ( SETUP_STANDBY_TEMPERATURE);

		InSingleRunMode = PARM1[17];							// for completeness
	};

void Do_WorkOnCycle()
	{	
		int T			= 0;	// T	= current Timer of Cycle Line
		int _Tf			= fakeSyncSignals_timer;
		int _TP2		= PREPARE_2_TIMER;			
		bool _prepareRunFound	= false;
		bool _prepare2Found		= false;
		float _calc_T			= 0;
		float _accuracy			= 0;


		if (!STATUS_CYCLE_RUNNING)
		{
			ledON();
			return;
		}
		
		if (STATUS_CYCLE_RESTART_PLEASE)
		{
			Line=0;
			STATUS_LINE_IS_DONE				= false;
			reset_all_timers();
			STATUS_CYCLE_RESTART_PLEASE		= false;
			STATUS_WAIT_FOR_TEMP			= false;
			USE_PREPARE_2_TIMER				= false;
			STATUS_CRYO_TIMEOUT				= false;
			MESSAGE_CRYO_TIMEOUT_TO_PC		= false;
			
		}

		// Update only the started Timers
		for (int i= 0; i < max_timers; i++ )
		{		if (TIMER_IS_STARTED[i])
				CURRENT_TIMER_TIME[i] = Milliseconds - START_TIME_OF_TIMER[i];
		}

		T= PARM1[Line];

		ledSwitch();

		switch (ATOM[Line])
		{
			
			case SET_VARIABLE:
				if (PARM1[Line] == PREPARE_2_TIME)
				{
					if (PARM2[Line] < 9999)
					{
						Prepare_2_WaitTime = (unsigned long)PARM2[Line];
						USE_PREPARE_2_TIMER = true;
					}
				}
				STATUS_LINE_IS_DONE		= true;
				break;

			case SET_TEMP:
					if (STATUS_WAIT_FOR_TEMP	== false)
					{	
						set_goalTemperature ( (float)limit(PARM1[Line],(int)_min_temperature,(int)_max_temperature) );
						STATUS_WAIT_FOR_TEMP	= true;
					}

					if (PARM2[Line]==99)	// Do NOT Wait for Temp
						{
							STATUS_LINE_IS_DONE		= true;
							STATUS_WAIT_FOR_TEMP	= false;
						}
					else
						{
							_calc_T = goalTemperature;
							_accuracy = PARM2[Line];

							if (Line == 14)		// Set High Temperature
							{
								_calc_T = goalTemperature*0.649 + 55.66;	// Calculate the approx. REAL temp inside the capillary !
								_accuracy = 5;
							}
													
							// WORKS BECAUSE WE SCAN TEMPERATURE FAST. 
							// If accuracy is small, we could miss the temp the first time
							// and get it only later during the regulation...
							int delta = int(_calc_T - currentTemperature);		// delta = Temp-Accuracy
							if (abs(delta) <= _accuracy )						// delta < Accuracy:  DONE  
							{	STATUS_LINE_IS_DONE		= true;
								STATUS_WAIT_FOR_TEMP	= false;
							}
							else
								STATUS_WAIT_FOR_TEMP	=  true;
						}

				break;

			case WAIT_SYNC_SIG:			// 2 Parms	Wait_Sync_Signal	(Signal, Signal State )
						switch (PARM1[Line]) 
						{
								case PREPARE_2:
									

									// check Use Prepare_2 TIMER instead of Prepare_2 Signal
									if (USE_PREPARE_2_TIMER)
									{
										if (!TIMER_IS_STARTED[_TP2])
										{
											WAIT_TIME_OF_TIMER[_TP2] = (Prepare_2_WaitTime * 1000);
											START_TIME_OF_TIMER[_TP2] = Milliseconds;
											TIMER_IS_STARTED[_TP2] = true;
											// debug
											//ledFlicker(50);
										}
										else
										{
											if (CURRENT_TIMER_TIME[_TP2] >= WAIT_TIME_OF_TIMER[_TP2])
											{
												TIMER_IS_STARTED[_TP2] = false;
												CURRENT_TIMER_TIME[_TP2] = 0;
												WAIT_TIME_OF_TIMER[_TP2] = 0;
												_prepare2Found = true;
											}
										}
									}
									else
									{	// check FAKE_SNYC_SIGNALS
										if (FAKE_SYNC_SIGNALS)
										{
											if (!TIMER_IS_STARTED[_Tf])
											{
												WAIT_TIME_OF_TIMER[_Tf] = (FAKE_SYNC_SINGALS_WAIT_SECONDS * 1000);
												START_TIME_OF_TIMER[_Tf] = Milliseconds;
												TIMER_IS_STARTED[_Tf] = true;
												// debug
												//ledFlicker(50);
											}
											else
											{
												if (CURRENT_TIMER_TIME[_Tf] >= WAIT_TIME_OF_TIMER[_Tf])
												{
													TIMER_IS_STARTED[_Tf] = false;
													CURRENT_TIMER_TIME[_Tf] = 0;
													WAIT_TIME_OF_TIMER[_Tf] = 0;
													_prepare2Found = true;
												}
											}
										}
									}
									// check real Signals
									if ( (!FAKE_SYNC_SIGNALS) && (!USE_PREPARE_2_TIMER) )
									{	if ((state_Prepare_2 == (int)PARM2[Line]) || (IGNORE_PREPARE_2 == true))	// or faked signal
											_prepare2Found = true;										
									}
									
									// Do it:
									if ( _prepare2Found ) 
									{	STATUS_LINE_IS_DONE		= true;
									}
									break;
						
								case GC_PREPARE:
									if ( (FAKE_SYNC_SIGNALS) )						// FAKE_SNYC_SIGNALS
									{	
										if ( !TIMER_IS_STARTED[_Tf] ) 				
										{	
											WAIT_TIME_OF_TIMER[_Tf] = FAKE_SYNC_SINGALS_WAIT_SECONDS * 1000;
											START_TIME_OF_TIMER[_Tf] = Milliseconds;
											TIMER_IS_STARTED[_Tf] = true;
											// debug
											//ledFlicker(50);
										}
										else
										{										
											//ledSwitch();
											if (CURRENT_TIMER_TIME[_Tf] > WAIT_TIME_OF_TIMER[_Tf] )
											{
												TIMER_IS_STARTED[_Tf]		= false;
												CURRENT_TIMER_TIME[_Tf]		= 0;
												WAIT_TIME_OF_TIMER[_Tf]		= 0;
												_prepareRunFound			= true;
												// debug
												//ledFlicker(50);
											}

										}
									}
									else																// real Signal
									{
										if (state_GC_Prepare==(int)PARM2[Line])
										{	
											if (IS_BRUKER)												// NO  Security !
											{
												_prepareRunFound = true;
											}
											else
											{
												if ((state_GC_Start == ON) || (IGNORE_GC_PREPARE == true))				// Security !
												{
													
													_prepareRunFound = true;
												}
											}
										}
									}


									if ( _prepareRunFound ) 
									{
										STATUS_LINE_IS_DONE		= true;
									}							
									
						
									break;

						case GC_START:
								if (FAKE_SYNC_SIGNALS)
								{	
									if ( TIMER_IS_STARTED[_Tf] == false )						// FAKE_SNYC_SIGNALS
									{	
										WAIT_TIME_OF_TIMER[_Tf] = (FAKE_SYNC_SINGALS_WAIT_SECONDS * 1000);
										START_TIME_OF_TIMER[_Tf] = Milliseconds;
										TIMER_IS_STARTED[_Tf] = true;
										STATUS_WAIT_FOR_START_SIGNAL = true;
									}
									else
									{	
										if (CURRENT_TIMER_TIME[_Tf] >= WAIT_TIME_OF_TIMER[_Tf] )
										{
											TIMER_IS_STARTED[_Tf]		= false;
											CURRENT_TIMER_TIME[_Tf]	= 0;
											WAIT_TIME_OF_TIMER[_Tf]	= 0;

											// IsVarianGC
											if (IS_BRUKER) 
												set_pin_GC_Ready(OFF);
											STATUS_LINE_IS_DONE		= true;
										}
									}

								}
								else
								{
									
									if (state_GC_Start==(int)PARM2[Line])
									{	
										if (IS_BRUKER)
										{
											set_pin_GC_Ready(OFF);
											STATUS_LINE_IS_DONE = true;
										}
												
										else if ((state_GC_Prepare == ON) || (IGNORE_GC_PREPARE == true))
												{
													STATUS_LINE_IS_DONE = true;
												}
										else
											STATUS_WAIT_FOR_START_SIGNAL = true;
											
									}
									else
										STATUS_WAIT_FOR_START_SIGNAL = true;
								}
								break;

						default:
								break;
						}

				break;

			case SWITCH_EVENT:			// 3 Parms  Switch_Event		(Event, Signal State, Pulse time)	Pulse Time 0= just set
					switch (PARM1[Line])
						{
							case GC_READY:
								set_pin_GC_Ready(PARM2[Line]);								
								STATUS_LINE_IS_DONE		= true;
								break;
							case TRAP_COOL:
								set_pin_TRAP_Cool_and_Consequences(PARM2[Line]);
								STATUS_LINE_IS_DONE		= true;
								break;
							default:
								break;
						}
				break;
		
			case START_TIMER:				// 2 Parms	Wait Timer			(Timer, time(sec))
						START_TIME_OF_TIMER[T]	= Milliseconds;
						WAIT_TIME_OF_TIMER[T]	= 0;				// FLAG for WAIT_TIMER (don't use -1 (unsigned long!))
						TIMER_IS_STARTED[T]		= true;
						STATUS_LINE_IS_DONE		= true;
					break;

			case WAIT_TIMER:				// 2 Parms	Wait Timer			(Timer, time(sec))
						if ( (TIMER_IS_STARTED[T]) && (WAIT_TIME_OF_TIMER[T]<1) )		// from Start Timer. Now we got WAIT_TIMER
						{
								WAIT_TIME_OF_TIMER[T] = (unsigned long)PARM2[Line]*1000;	// seconds to millis
						}
						else
						{
							// If Timer is NOT YET started: Start now			
							if ( TIMER_IS_STARTED[T]==false )
								{
									WAIT_TIME_OF_TIMER[T] = (unsigned long)PARM2[Line]*1000;	// seconds to millis
									START_TIME_OF_TIMER[T] = Milliseconds;
									TIMER_IS_STARTED[T] = true;
								}	
							else
								{
									if (CURRENT_TIMER_TIME[T] >= WAIT_TIME_OF_TIMER[T] )
									{
										TIMER_IS_STARTED[T]		= false;
										CURRENT_TIMER_TIME[T]	= 0;
										WAIT_TIME_OF_TIMER[T]	= 0;
										STATUS_LINE_IS_DONE		= true;								
									}
								}	
						}
					break;

			case END_OF_CYCLE:
				if (PARM1[Line]==0)	
					{	STATUS_CYCLE_RESTART_PLEASE	= true;
						InSingleRunMode = false;
						_restartCycle();
					}
				else
					{	STATUS_CYCLE_RESTART_PLEASE = false;
						InSingleRunMode = true;
						_stopCycle();
					}

				break;

			default:
				break;
		}

		  			
		if ( STATUS_CYCLE_STEP_MODE == true )			// In STEP MODE
		{	
			if ( (STATUS_CYCLE_NEXT_STEP_PLEASE) )
			{ 
				STATUS_LINE_IS_DONE				= true;
			}

			if ( (STATUS_CYCLE_NEXT_STEP_PLEASE) && (STATUS_LINE_IS_DONE) )
			{ 
				Line++;
				STATUS_LINE_IS_DONE				= false;
				STATUS_CYCLE_NEXT_STEP_PLEASE	= false;
				STATUS_WAIT_FOR_TEMP			= false;
				STATUS_WAIT_FOR_START_SIGNAL	= false;
			}
		}
		else									// NOT in step mode
		{		
			if (STATUS_LINE_IS_DONE)  
			{
				Line++;
				STATUS_LINE_IS_DONE				= false;
				STATUS_WAIT_FOR_TEMP			= false;
				STATUS_WAIT_FOR_START_SIGNAL	= false;
			}
		}

  }

  // Do the logging to PC
void OnAskForReadbacks() {
   
	cmdMessenger.sendCmdStart(kSendReadbacksToPC);
	cmdMessenger.sendCmdArg(secondsTemperature,2);   
	cmdMessenger.sendCmdArg(currentTemperature,1);  
	cmdMessenger.sendCmdArg(AnalogIn7_Power,1);

	// global Timer
	cmdMessenger.sendCmdArg(Timer_seconds);
	cmdMessenger.sendCmdArg(Timer_minutes);
	cmdMessenger.sendCmdArg(Timer_hours);
  
	// Temperature and PWM
	cmdMessenger.sendCmdArg(goalTemperature);
	cmdMessenger.sendCmdArg(_min_temperature);
	cmdMessenger.sendCmdArg(_max_temperature);
	cmdMessenger.sendCmdArg(_proportional);
	cmdMessenger.sendCmdArg(_integral);

	cmdMessenger.sendCmdArg((float)(Millis_HeaterBang-HeaterBang_min)/HeaterBang_max*100);
	cmdMessenger.sendCmdArg(_integral_sum);
	cmdMessenger.sendCmdArg(HeaterIsOn);

	cmdMessenger.sendCmdArg(TempCorrectionSlope);
	//cmdMessenger.sendCmdArg(p_error);
	cmdMessenger.sendCmdArg(heatUpSlope);		// GUI's unused 'HeatUpSlope'
	//cmdMessenger.sendCmdArg(i_error);
	cmdMessenger.sendCmdArg(CurrentTemperatureRAW);	// FW 2.3
	
	cmdMessenger.sendCmdArg(correction_sum);

	// TTLs
	cmdMessenger.sendCmdArg(state_GC_Prepare);
	cmdMessenger.sendCmdArg(state_GC_Ready);
	cmdMessenger.sendCmdArg(state_GC_Start);
	cmdMessenger.sendCmdArg(state_TRAP_Cool);
	cmdMessenger.sendCmdArg(state_Prepare_2);

	// Timers
	//for (int i=0; i<max_timers; i++)
	//	cmdMessenger.sendCmdArg(CURRENT_TIMER_TIME[i]);	
	cmdMessenger.sendCmdArg(CURRENT_TIMER_TIME[0]);
	cmdMessenger.sendCmdArg(CURRENT_TIMER_TIME[1]);
	cmdMessenger.sendCmdArg(CURRENT_TIMER_TIME[2]);
	cmdMessenger.sendCmdArg(CURRENT_TIMER_TIME[3]);		// for Use_Prepare_2_Timer (4 in GUI)
	cmdMessenger.sendCmdArg(CURRENT_TIMER_TIME[4]);		// For Fake_Sync_Signals   (5 in GUI)

	cmdMessenger.sendCmdArg((int)MESSAGE_CRYO_TIMEOUT_TO_PC);

	cmdMessenger.sendCmdArg((int)Prepare_2_WaitTime);

	cmdMessenger.sendCmdArg(InSingleRunMode==true? 1:0);

	// current Cycle Line  (if running)
	cmdMessenger.sendCmdArg((int)STATUS_CYCLE_RUNNING);
	if (STATUS_CYCLE_RUNNING>0)
	{
		cmdMessenger.sendCmdArg(Line);
		cmdMessenger.sendCmdArg(ATOM[Line]);
		cmdMessenger.sendCmdArg(PARM1[Line]);
		cmdMessenger.sendCmdArg(PARM2[Line]);
		cmdMessenger.sendCmdArg(PARM3[Line]);
	}

	cmdMessenger.sendCmdEnd();
}

// Parameters for 'OnParmsToControler'
enum Parameters
{	pGoalTemperature,
	pHeater,
	pTimer,
	pCycleRestart,
	pCycleStop,
	pChangeVariables,
	pCycleToEEPROM,
	cycleStepMode,
    cycleNextStep,
	ignoreCryoTimeout,
	ignoreGCPrepare,
	ignorePrepare_2,
	singleRunMode,
	temperatureCorrection,
	fakeSyncSignals
};

// Read the commands sent down from PC to Controler
void OnParmsToControler()
{
	if (cmdMessenger.isArgOk())
	{	
		//int dummy;
		//int x[20];
		//int i = 0;
		int item[10];
		int num_items;
		//int myLine = 0;
		//bool goOn = true;
		int command		= cmdMessenger.readInt16Arg();		// COMMAND

		num_items	= cmdMessenger.readInt16Arg();		
		for (int i = 0; i<num_items; i++)		item[i] = cmdMessenger.readInt16Arg();

		switch (command)
		{	
			case (int)fakeSyncSignals:
				FAKE_SYNC_SIGNALS= (item[0] == 1 ? true : false);
			break;

			case (int)temperatureCorrection:	
				TempCorrectionSlope_LowTemp =  (float)item[0]/1000;
				TempCorrectionSlope_MidTemp =  (float)item[1]/1000;
				TempCorrectionSlope_HiTemp = (float)item[2]/1000;
				saveFWandSETUPs();
				break;
		
			case (int)singleRunMode:
				InSingleRunMode	=	(item[0]==1? true:false);
				PARM1[17]	=	(item[0]==1? true:false);
				break;
			case (int)ignorePrepare_2:
				IGNORE_PREPARE_2 = (item[0]==1 ? true:false);
				break;
			case (int)ignoreGCPrepare:
				IGNORE_GC_PREPARE = (item[0]==1 ? true:false);
				break;
			case (int)ignoreCryoTimeout:
				STATUS_CRYO_TIMEOUT_IGNORE = (item[0]==1 ? true:false);
				break;
			case (int)cycleStepMode:
				_switch_StepMode(item[0]==1 ? true:false);
				break;			
			case (int)cycleNextStep:
					_nextStepPlease();
					break;
			case (int)pCycleToEEPROM:
					eeSaveCycle();
					break;
			case (int)pChangeVariables:
				_stopCycle();
				PARM1[2]	=	item[0];		//method_StandbyTemp
				PARM1[6]	=	item[0];		//method_StandbyTemp		// V4
				//goalTemperature = item[0];		// go to Standby Temperature
				set_goalTemperature ((float)item[0]);

				PARM1[8]	=	item[1];		//method_LowTemp
				PARM2[9]	=	item[2];		//method_LowTime_PreInject
				PARM2[12]	=	item[3];		//method_LowTime_PostInject

				PARM1[14]	=	item[4];		//method_HighTemp			// V3
				PARM2[16]	=	item[5];		//method_HighTemp_Time

				PARM1[17]	=	(item[6]==1?	true:false);	// parm1 in END_OF_CYCLE 
				InSingleRunMode	=	(item[6]==1?	true:false);	// = InSingleRunMode

				// version 2.7	 
				PARM2[0]	=	item[7];		// For Use_Prepare_2_Timer (Set if >0
				Prepare_2_WaitTime = item[7];				
				 
				break;
			case (int)pGoalTemperature:
				_min_temperature = (float)item[0];
				_max_temperature =  (float)item[1];				
				//goalTemperature = (float)item[2];
				set_goalTemperature ( (float)item[2] );
				break;
			case (int)pHeater:
				HeaterIsOn = item[0];
				STATUS_PC_WANT_HEATERS_OFF = !HeaterIsOn;
				/*if (HeaterIsOn==true)
					STATUS_PC_WANT_HEATERS_OFF = false;
				else
					STATUS_PC_WANT_HEATERS_OFF = true;*/
				break;
			case (int)pTimer:
				if (item[0]==0)	Timer_is_on = false;
				else
				{	Timer_seconds = 0;
					Timer_minutes = 0;
					Timer_hours = 0;
					Timer_is_on = true;
				}
				break;
			case (int)pCycleRestart:
				_restartCycle();	
				break;
			case (int)pCycleStop:
				_stopCycle();
				break;

			default:
				break;
		}
	}
	cmdMessenger.sendCmd(kParmsReceived);
}

// invoked from PC and at end of Cycle according to IsSingleRun mode
void _stopCycle()
{	reset_all_timers();
	reset_all_signals();
	Line=0;
	STATUS_CYCLE_RUNNING	= false;
	STATUS_CYCLE_RESTART_PLEASE = false;
	set_goalTemperature ( (float)PARM1[2] );
}

// invoked from PC and at end of Cycle according to IsSingleRun mode
void _restartCycle()
{	
	if (IS_BRUKER)			set_pin_GC_Ready(OFF);			// Varian GC: Set TTL Low for PAL
	else					set_pin_GC_Ready(ON);				// chg to Input, allow GC to get ready
	STATUS_CYCLE_RESTART_PLEASE = true;
	STATUS_CYCLE_RUNNING		= true;
	MESSAGE_CRYO_TIMEOUT_TO_PC	= false;
	STATUS_CRYO_TIMEOUT			= false;
}

/*  */
void set_goalTemperature(float _newTemp)
{	
	//float _lastGoal = goalTemperature;
	goalTemperature = limit_float(_newTemp,_min_temperature,_max_temperature);	
	

	if (Line==14)
	{	// This is for Heatup during Cycle to High Temp
		_proportional = 1.8;		// 1.8
		_integral = 0.009;	// 0.009
		rateToSwitchOnIntegral = 4;		// if lower than 4 degree: switch on integral
		_integral_on = false;
		// reset HeatRate monitor
		lastHeatRateTemp = currentTemperature;
		currentHeatRateTemp = currentTemperature;
		lastHeatRate = -99999;
		currentHeatRate = -99999;
	}
	else
	{	
		_proportional			= 300;
		_integral				= 0;
		_integral_on			= false;
	}
	TempCorrectionSlope	= TempCorrectionSlope_MidTemp;
}

// Do the P I D stuff
void update_pid()
{	// all vars are global	
	if (state_TRAP_Cool == OFF)					// Heating
	{
		HeaterBang_min = HeaterBang_min_Hot;
	}
	else
	{											// Cooling
		HeaterBang_min = HeaterBang_min_Cool;
	}

	error = 0;
	p_error = 0;
	i_error = 0;
	correction_sum = 0;

	if (currentTemperature > _max_temperature+20)
	{	Millis_HeaterBang = 0;
		_integral_sum = 0;

	}
	else
	{	error = (goalTemperature - currentTemperature);
			
		if ( ( currentTemperature > 0 ) && ( _integral_on) )
		{	
			_integral_sum =	_integral_sum + error;	
		}
		else
		{	
			_integral_sum = 0; 
		}
		
		p_error = error * _proportional;
		i_error = _integral_sum * _integral; 
		correction_sum  = p_error + i_error;
		float c = correction_sum;

		if (c > HeaterBang_max)
			c = HeaterBang_max;
		if (c < HeaterBang_min)		
			c = HeaterBang_min;	
		Millis_HeaterBang = c;
	}
}

//// For testing the heating Rate:
//float lastHeatRateTemp			= 0;
//float currentHeatRateTemp		= 0;
//float lastHeatRate				= 0;	// copied from currentHeatRate
//float currentHeatRate			= 0;	// calced from lastHeatRateTemp / currentHeatRateTemp
//float rateToSwitchOnIntegral	= 0;	// If rate is lower :  Switch on the Integral
void updateHeatingRateIssues()
{
	if(  (Line<14) || (Line>16)  )
	{
		_integral_on = false;
	}

	if ( (Line==14) && (!_integral_on) )		// High Temperature until we reach the Integral Zone
	{
		lastHeatRateTemp	= currentHeatRateTemp;
		currentHeatRateTemp = currentTemperature;
		lastHeatRate		= currentHeatRate;
		currentHeatRate		= currentHeatRateTemp - lastHeatRateTemp;

		if ( currentHeatRate < lastHeatRate )
		{
			// if the heat rate is lower than rateToSwitchOnIntegral: Set integral ON
			if  (currentHeatRate < rateToSwitchOnIntegral)
				_integral_on = true;								// ONLY PLACE FOR INTEGRAL ON
		}
	}
}

void updateSecurityIssues() 
{ 
	bool _status_controler_is_on		=	false;
	bool _status_gc_is_connected		=	true;
	 
	if ( FAKE_TEMPERATURE_AND_HEATERS )
	{
		STATUS_SYSTEM_IS_SECURE		= true;
		STATUS_CRYO_TIMEOUT			= false;
		AnalogIn7_Power				= 11.5;
		_status_controler_is_on		= true;
		return;
	}


	// Get the Input Voltage (Pin 30, VIN, with Voltage Devider)
	// if 12   V:  Controler is switched on
	// if  4-5 V:  Controler is switched OFF  only USB gives power.
	// if 14V:		No Controler connected (only Arduino)
	// If Controler is off: We have to reset the Cycle trun off HeaterPower
	AnalogIn7_Power		=  (float)analogRead(AnalogPin7_Power)*14/1000;
	if ( (AnalogIn7_Power>10) && (AnalogIn7_Power<13) )
		_status_controler_is_on		= true;

	
	_status_gc_is_connected = true;		// debug
		
	if ( (_status_controler_is_on) && (_status_gc_is_connected) )
		STATUS_SYSTEM_IS_SECURE = true;
	else
		STATUS_SYSTEM_IS_SECURE = false;


	//// Care for Timeout for Peltier Cooler
	if (!STATUS_CRYO_TIMEOUT_IGNORE)
	{
		if ( ( STATUS_WAIT_FOR_TEMP) && (state_TRAP_Cool==ON) && ( Millis_CryoTimeout > HW_Cryotimeout_Millis) )		// Cooling  and  too long
			{	
				STATUS_CRYO_TIMEOUT = true;
			}
	}

}

/* Converts PT1000 AND applies FIXed corrections */
void measure_Temperatures_from_PT1000() 
{	
	// Readout of Analog 0 Pin connected to PT1000 Konverter (0-6V)
	secondsTemperature	= (float) (Milliseconds-startLoggingMillis) /1000.0 ;	// Time since Startup !
	
															//  check FAKE_TEMPERATURE_AND_HEATERS  
	if ( FAKE_TEMPERATURE_AND_HEATERS )
	{
		if ( currentTemperature > goalTemperature )
		{	fakeTemperatureRising	=false;
			//fakeTemperatureFalling	= true;
		}
		if ( currentTemperature < goalTemperature )
		{	fakeTemperatureRising	= true;
			//fakeTemperatureFalling	= false;
		}

		if (HeaterIsOn==false)
			fakeTemperatureRising = false;

		if (fakeTemperatureRising)
			CurrentTemperatureRAW += 0.3;
		else
			CurrentTemperatureRAW -= 0.3;
		
		if ( (!HeaterIsOn) && ( CurrentTemperatureRAW < 20 ))
			CurrentTemperatureRAW = 20.3;

		measure_Temperatures_from_PT1000_correctTemperature();
		return;
	}
	
	
	// Read directly:		AnalogIn0_Temperature				= (float)analogRead(AnalogPin0_Temperature)*3.223;
	// Or get from averaged AnalogIn0_Temperature:
	AnalogIn0_Temperature				= (AnalogIn0_Temperature/Average_count) *3.223;
	// f(x)=0.13-50 funktion for PT1000 Converter Board 
	// -50C = 0 mV,   350C = 3200 mV
	CurrentTemperatureRAW	= (AnalogIn0_Temperature*0.13) - 50;

	measure_Temperatures_from_PT1000_correctTemperature();

	// reset for next average round
	Average_count			= 0;
	AnalogIn0_Temperature	= 0;
}

/* Do Temp Correction: CoolTemp=Raw ;  ALL others = FIXed Correction */
void measure_Temperatures_from_PT1000_correctTemperature()
{		
	if ( ( state_TRAP_Cool == ON ) && ( STATUS_CYCLE_RUNNING == true) )	// Calc for COOL State
	{	currentTemperature = CurrentTemperatureRAW;
	}
	else
	{																	// Calc for HEATING
		currentTemperature = (CurrentTemperatureRAW * 1.4676) - 8.8683;
	}
	// modify with user-defined slope  and  
	currentTemperature = currentTemperature / TempCorrectionSlope;
}

void average_AnalogPin0_Temperature()
{	Average_count++;
	AnalogIn0_Temperature += (float)analogRead(AnalogPin0_Temperature);
}


// LED  Stuff
	// toggle Led
void ledSwitch()
{	digitalWrite(_blinkLed_Pin, _state_Led?HIGH:LOW);
	_state_Led = !_state_Led;
}
void ledOFF()
{	digitalWrite(_blinkLed_Pin, LOW);
	_state_Led = LOW;
}
void ledON()
{	digitalWrite(_blinkLed_Pin, HIGH);
	_state_Led = HIGH;
}
	// switch on/off counts time
void ledFlicker(int counts)
{	for (int i = 0; i<counts; i++){
		digitalWrite(_blinkLed_Pin, _state_Led?HIGH:LOW);
		_state_Led = !_state_Led;
		delay (50);
		}
}

// TTL Stuff
void Update_TTLs()
{	get_pin_GC_Prepare();
	get_pin_GC_Ready();
	get_pin_TRAP_Cool();
	get_pin_GC_Start();
	get_pin_Prepare_2();
}
float get_pin_GC_Prepare()
{	
	if (FAKE_SYNC_SIGNALS)													// check FAKE_TEMPERATURE_AND_HEATERS
		state_GC_Prepare = 1.0;

	if (IGNORE_GC_PREPARE)
		state_GC_Prepare = 0.0;


	if ((!FAKE_SYNC_SIGNALS) && (!IGNORE_GC_PREPARE))
		state_GC_Prepare = (float)digitalRead(pin_GC_Prepare);		// 0.0 for FW 1.8

	return (float)state_GC_Prepare;
}
float get_pin_Prepare_2()
{	if (FAKE_SYNC_SIGNALS)													// check FAKE_TEMPERATURE_AND_HEATERS
		state_Prepare_2 = 1.0;

	if (IGNORE_PREPARE_2)
		state_Prepare_2 = 0.0;

	if ((!FAKE_SYNC_SIGNALS) && (!IGNORE_PREPARE_2))
		state_Prepare_2 = (float)digitalRead(pin_Prepare_2);

	return (float)state_Prepare_2;
}
float get_pin_GC_Ready()
{	
	if (FAKE_SYNC_SIGNALS)
		return (float)state_GC_Ready;	
	else
	{
		if (dir_GC_Ready_STATE == dir_GC_Ready_IN)
			state_GC_Ready = digitalRead(pin_GC_Ready);
		return (float)state_GC_Ready;				// Is OUT or IN, so return the var
	}
}	
float get_pin_GC_Start()
{	if ( FAKE_SYNC_SIGNALS )													// check FAKE_TEMPERATURE_AND_HEATERS
		state_GC_Start = 1.0;
	else	
		state_GC_Start = digitalRead(pin_GC_Start);

	return (float)state_GC_Start;
}
float get_pin_TRAP_Cool()
{	return (float)state_TRAP_Cool;				// Is OUT, so return the var
}
void set_pin_GC_Ready(int state)
{
	state_GC_Ready = (float)state;
	if (!FAKE_SYNC_SIGNALS)
	{
		// Agilent: LOW: Output to low.  HIGH: Go to Input State (don't change the TTL line)
		if (!IS_BRUKER)
		{
			if (state_GC_Ready > LOW)
			{
				pinMode(pin_GC_Ready, dir_GC_Ready_IN);
			}
			else
			{
				pinMode(pin_GC_Ready, dir_GC_Ready_OUT);
				digitalWrite(pin_GC_Ready, state);
			}			
		}

		if (IS_BRUKER)
		{
			digitalWrite(pin_GC_Ready, state);
		}
		
	}
}
void set_pin_TRAP_Cool_and_Consequences(int state)
{	state_TRAP_Cool = state;
	if ( !FAKE_TEMPERATURE_AND_HEATERS )													// check FAKE_TEMPERATURE_AND_HEATERS
		digitalWrite(pin_TRAP_Cool, state);

	set_pid_limits();
}


	// USEFULL FUNCTIONS
void reset_all_timers()
{
	for (int i = 0; i < max_timers; i++)				// Reset all Timers
	{	TIMER_IS_STARTED[i]		= false;
		WAIT_TIME_OF_TIMER[i]	= 0;
		START_TIME_OF_TIMER[i]	= 0;
		CURRENT_TIMER_TIME[i]	= 0;
	}
}
void reset_all_signals()
{
	set_pin_TRAP_Cool_and_Consequences(OFF);
	if (IS_BRUKER)		set_pin_GC_Ready(OFF);
	else				      set_pin_GC_Ready(ON);	// ON chg to Input state
}
void reset_cycle_array()
{	for (int i = 0; i<max_Lines; i++)
	{	ATOM[i] = 0;
		PARM1[i] = 0;
		PARM2[i] = 0;
		PARM3[i] = 0;
	}
}


void set_pid_limits()
{
	if (state_TRAP_Cool == ON)
	{
		_min_temperature = HW_Minimum_Temperature_with_Cool;
		_max_temperature = HW_Maximum_Temperature_with_Cool;
	}
	else
	{	// Trap OFF
		_min_temperature = HW_Minimum_Temperature;
		_max_temperature = HW_Maximum_Temperature;
	}
}

float limit_float(float val, float small, float big)
{	float r = val;
	if (val < small)		
		r=small;
	if (val > big)		
		r=big;

	return r;
}

int limit(int val, int small, int big)
{	if (val < small)	return small;
	if (val > big)		return big;
	return val;
} 
void eeWriteInt(int intVal)
{	
	int a = (unsigned int)intVal/256;
	int b = (unsigned int)intVal % 256;

	EEPROM.write(EEaddress++,a);
	EEPROM.write(EEaddress++,b);
}
int eeReadInt()
{	
	int a= EEPROM.read(EEaddress++);
	int b= EEPROM.read(EEaddress++);

	return (a*256+b) ;
}
