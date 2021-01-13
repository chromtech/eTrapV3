#region CmdMessenger - MIT - (c) 2013 Thijs Elenbaas.
/*
  CmdMessenger - library that provides command based messaging

  Permission is hereby granted, free of charge, to any person obtaining
  a copy of this software and associated documentation files (the
  "Software"), to deal in the Software without restriction, including
  without limitation the rights to use, copy, modify, merge, publish,
  distribute, sublicense, and/or sell copies of the Software, and to
  permit persons to whom the Software is furnished to do so, subject to
  the following conditions:

  The above copyright notice and this permission notice shall be
  included in all copies or substantial portions of the Software.

  Copyright 2013 - Thijs Elenbaas
*/
#endregion

using System;
using System.ComponentModel;
using System.IO.Ports;
using System.Reflection;
using System.Linq;
using System.Threading;

namespace CommandMessenger.TransportLayer
{
    public enum ThreadRunStates
    {
        Start,
        Stop,
        Abort,
    }

    /// <summary>Fas
    /// Manager for serial port data
    /// </summary>
    public class SerialTransport : DisposableObject, ITransport
    {
        private readonly QueueSpeed _queueSpeed = new QueueSpeed(4,10);
        private Thread _queueThread;
        private ThreadRunStates _threadRunState;
        private readonly object _threadRunStateLock = new object();
        private readonly object _serialReadWriteLock = new object();

        /// <summary> Gets or sets the run state of the thread . </summary>
        /// <value> The thread run state. </value>
        public ThreadRunStates ThreadRunState  
        {
            set
            {
                lock (_threadRunStateLock)
                {
                    _threadRunState = value;
                }
            }
            get
            {
                ThreadRunStates result;
                lock (_threadRunStateLock)
                {
                    result = _threadRunState;
                }
                return result;
            }
        }

        /// <summary> Default constructor. </summary>
        public SerialTransport(int PortListItem)
        {
            Initialize(PortListItem);
        }

        public int NumberOfPorts()
        {
            try { 
            _currentSerialSettings.PortNameCollection = SerialPort.GetPortNames();
            return _currentSerialSettings.PortNameCollection.Length;
            }
            catch (Exception ex)
            {
                //System.Windows.Forms.MessageBox.Show("SerialTransport/NumberOfPorts: " + ex.ToString());
                return 0;
            }
        }

        /// <summary> Initializes this object. </summary>
        public void Initialize(int PortListItem)
        {

            //PortListItem;
            //_queueSpeed.Name = "Serial";
            // Find installed serial ports on hardware
            try
            {
                _currentSerialSettings.PortNameCollection = SerialPort.GetPortNames();

            }
            catch(Exception ex)
            {
                //System.Windows.Forms.MessageBox.Show("SerialTransport/Initialize(" + PortListItem.ToString() + "):  " + ex.ToString());
            }
                // If serial ports are found, we select the number
                if (_currentSerialSettings.PortNameCollection.Length > (Int32)PortListItem)
                {
                    _currentSerialSettings.PortName = _currentSerialSettings.PortNameCollection[PortListItem];

                    // Create queue thread and wait for it to start
                    _queueThread = new Thread(ProcessQueue)
                    {
                        Priority = ThreadPriority.Normal,
                        Name = "Serial"
                    };
                    ThreadRunState = ThreadRunStates.Start;
                    _queueThread.Start();
                    while (!_queueThread.IsAlive) { Thread.Sleep(50); }
                }
           
        }

        #region Fields

        private SerialPort _serialPort;                                         // The serial port
        private SerialSettings _currentSerialSettings = new SerialSettings();   // The current serial settings
        public event EventHandler NewDataReceived;                              // Event queue for all listeners interested in NewLinesReceived events.

        #endregion

        #region Properties

        /// <summary> Gets or sets the current serial port settings. </summary>
        /// <value> The current serial settings. </value>
        public SerialSettings CurrentSerialSettings
        {
            get { return _currentSerialSettings; }
            set { _currentSerialSettings = value; }
        }



        /// <summary> Gets the serial port. </summary>
        /// <value> The serial port. </value>
        public SerialPort SerialPort
        {
            get { return _serialPort; }
        }

        #endregion

        #region Methods

        protected  void ProcessQueue()
        {
            // Endless loop
            while (ThreadRunState != ThreadRunStates.Abort)
            {
                var bytes = BytesInBuffer();
                _queueSpeed.SetCount(bytes);
                _queueSpeed.CalcSleepTimeWithoutLoad();
                _queueSpeed.Sleep();
                if (ThreadRunState == ThreadRunStates.Start)
                {
                    if (bytes > 0)
                    {
                        if (NewDataReceived != null) NewDataReceived(this, null);
                    }
                }
            }
            _queueSpeed.Sleep(50);
        }        

        /// <summary> Connects to a serial port defined through the current settings. </summary>
        /// <returns> true if it succeeds, false if it fails. </returns>
        public bool StartListening()
        {
            // Closing serial port if it is open
            if (IsOpen())
                Close();

            if (_currentSerialSettings.PortName.Length > 0)
            {
                // Setting serial port settings
                _serialPort = new SerialPort(
                    _currentSerialSettings.PortName,
                    _currentSerialSettings.BaudRate,
                    _currentSerialSettings.Parity,
                    _currentSerialSettings.DataBits,
                    _currentSerialSettings.StopBits)
                {
                    DtrEnable = _currentSerialSettings.DtrEnable
                };

                //System.Windows.Forms.MessageBox.Show("SerialTransport/StartListening to Port: " + _serialPort.PortName);

                // Subscribe to event and open serial port for data
                ThreadRunState = ThreadRunStates.Start;
                try
                {
                    if (_serialPort != null && PortExists())
                    {
                       _serialPort.Open();
                    }
                }
                catch (Exception ex)
                {
                    //System.Windows.Forms.MessageBox.Show("SerialTransport/StartListening: " + ex.ToString());
                    return false;
                }
                return (_serialPort != null && PortExists());
            }
            else
                return false;
        }

        /// <summary> Opens the serial port. </summary>
        /// <returns> true if it succeeds, false if it fails. </returns>
        //public bool Open()
        //{
        //    System.Windows.Forms.MessageBox.Show("SerialTransport/Open() called " );

        //    if (_serialPort != null && PortExists() && !_serialPort.IsOpen)
        //    {
        //        System.Windows.Forms.MessageBox.Show("SerialTransport/Open() inside if... ");
        //        try
        //        {

        //            //_serialPort.Open();
        //            return _serialPort.IsOpen;
        //        }
        //        catch (Exception ex)
        //        {
        //            System.Windows.Forms.MessageBox.Show("SerialTransport/Open(): " + ex.ToString());
        //            return false;
        //        }
        //    }

        //    return true;
        //}

        public bool IsConnected()
        {
            return (_serialPort != null && PortExists());
        }

        /// <summary> Queries if a given port exists. </summary>
        /// <returns> true if it succeeds, false if it fails. </returns>
        public bool PortExists()
        {
            try
            {
                return SerialPort.GetPortNames().Contains(_serialPort.PortName);
            }
            catch (Exception ex)
            {
                //System.Windows.Forms.MessageBox.Show("SerialTransport/PortExists: " + ex.ToString());
                return false;
            }
        }

        /// <summary> Closes the serial port. </summary>
        /// <returns> true if it succeeds, false if it fails. </returns>
        public bool Close()
        {
            try
            {
                if (SerialPort == null || !PortExists())
                    return false;
                if (!_serialPort.IsOpen)
                    return true;
                _serialPort.Close();
                return true;
            }
            catch (Exception ex)
            {
                //System.Windows.Forms.MessageBox.Show("SerialTransport/Close: " + ex.ToString());
                return false;
            }            
        }

        /// <summary> Query ifthe serial port is open. </summary>
        /// <returns> true if open, false if not. </returns>
        public bool IsOpen()
        {
            try
            {
                return _serialPort != null && PortExists();
            }
            catch (Exception ex)
            {
                //System.Windows.Forms.MessageBox.Show("SerialTransport/IsOpen: " + ex.ToString());
                return false;
            }
        }

        /// <summary> Stops listening to the serial port. </summary>
        /// <returns> true if it succeeds, false if it fails. </returns>
        public bool StopListening()
        {
            ThreadRunState = ThreadRunStates.Stop;
            var state = Close();
            return state;
        }

        /// <summary> Writes a parameter to the serial port. </summary>
        /// <param name="buffer"> The buffer to write. </param>
        public void Write(byte[] buffer)
        {
            try
            {
                if ((_serialPort != null && PortExists()))
                {
                    lock (_serialReadWriteLock)
                    {
                        _serialPort.Write(buffer, 0, buffer.Length);
                    }
                }
            }
            catch (Exception ex)
            {
                //System.Windows.Forms.MessageBox.Show("SerialTransport/Write: " + ex.ToString());
                return;
            }
        }

        /// <summary> Retrieves the possible baud rates for the currently selected serial port. </summary>
        /// <returns> true if it succeeds, false if it fails. </returns>
        public bool UpdateBaudRateCollection()
        {
            try
            {
                if (_serialPort!=null)
                {
                    var fieldInfo = _serialPort.BaseStream.GetType()
                                               .GetField("commProp", BindingFlags.Instance | BindingFlags.NonPublic);
                    if (fieldInfo != null)
                    {
                        object p = fieldInfo.GetValue(_serialPort.BaseStream);
                        var fieldInfoValue = p.GetType()
                                              .GetField("dwSettableBaud",
                                                        BindingFlags.Instance | BindingFlags.NonPublic |
                                                        BindingFlags.Public);
                        if (fieldInfoValue != null)
                        {
                            var dwSettableBaud = (Int32) fieldInfoValue.GetValue(p);
                            Close();
                            _currentSerialSettings.UpdateBaudRateCollection(dwSettableBaud);
                        }
                    }
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary> Reads the serial buffer into the string buffer. </summary>
        public byte[] Read()
        {
            var buffer = new byte[0];
            if (_serialPort != null && PortExists())
            {
                try
                {
                    lock (_serialReadWriteLock)
                    {
                        var dataLength = _serialPort.BytesToRead;
                        buffer = new byte[dataLength];
                        int nbrDataRead = _serialPort.Read(buffer, 0, dataLength);
                        if (nbrDataRead == 0) return new byte[0];
                    }
                }
                catch(Exception ex)
                {
                    //System.Windows.Forms.MessageBox.Show("SerialTransport/Read: " + ex.ToString());
                    return buffer;
                }
            }
            return buffer;
        }

        /// <summary> Gets the bytes in buffer. </summary>
        /// <returns> Bytes in buffer </returns>
        public int BytesInBuffer()
        {
            try
            { 
                if (IsOpen())
                {
                    return _serialPort.BytesToRead;
                }
                else
                    return 0;
            }
            catch
            {
                return 0;
            }

            //return IsOpen()? _serialPort.BytesToRead:0;
        }

        /// <summary> Kills this object. </summary>
        public void Kill()
        {
            // Signal thread to stop
            ThreadRunState = ThreadRunStates.Stop;

            //Wait for thread to die
            if (Join(500))          // returns false if not living
            {
                if (_queueThread.IsAlive) 
                    _queueThread.Abort();
            }
                // Releasing serial port 
                if (IsOpen()) Close();
                if (_serialPort != null)
                {
                    _serialPort.Dispose();
                    _serialPort = null;
                }
            

        }

        /// <summary> Joins the thread. </summary>
        /// <param name="millisecondsTimeout"> The milliseconds timeout. </param>
        /// <returns> true if it succeeds, false if it fails. </returns>
        public bool Join(int millisecondsTimeout)
        {
            try
            {
                if (_queueThread.IsAlive == false) return true;
                return _queueThread.Join(TimeSpan.FromMilliseconds(millisecondsTimeout));
            }
            catch (Exception e) { return false; }
            
        }

        // Dispose
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                
                if (_queueThread!=null)
                    _queueThread.Abort();
                
                Kill();
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}