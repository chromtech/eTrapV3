using System;
using System.Diagnostics;
using System.Windows.Forms;



namespace DataLogging
{   
    static class Program
    {
        public static int PriorProcess()
        // Returns a System.Diagnostics.Process pointing to
        // a pre-existing process with the same name as the
        // current one, if any; or null if the current process
        // is unique.
        {
            Process curr = Process.GetCurrentProcess();
            Process[] procs = Process.GetProcessesByName(curr.ProcessName);                       
            return procs.Length;
         }

        /// <summary>
        /// The main entry point for the application.
        /// Note that the main code is in DataLogging (Do_Startup)
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (PriorProcess() > 1)
            {
                System.Windows.Forms.MessageBox.Show("eTrap is already running (see TaskManager/Processes: CControler.exe");
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ChartForm());            
        }


    }
}
