using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace DataLogging
{
    public partial class Splash : Form
    {
        static Thread thread = null;
        static Splash frmSplash = null;

        private double opacityIncrement = .20;  //05
        private double opacityDecrement = .20;  //08
        private const int TIMER_INTERVAL = 20;
        private string stringStatus="";


        // A static entry point to launch SplashScreen.
        static private void ShowForm()
        {
            frmSplash = new Splash();
            Application.Run(frmSplash);
        }

        static public void SetStatus(string newStatus)
        {
            if (frmSplash == null)
                return;
            frmSplash.stringStatus = newStatus;
        }

        // A static method to close the SplashScreen
        static public void CloseForm()
        {
            if (frmSplash != null)
            {
                // Make it start going away.
                frmSplash.opacityIncrement = -frmSplash.opacityDecrement;
            }
            thread = null;  // we do not need these any more.
            frmSplash = null;
        }

        static public void ShowSplashScreen()
        {
            // Make sure it is only launched once.
            if (frmSplash != null)
                return;

            thread = new Thread(new ThreadStart(Splash.ShowForm));
            thread.IsBackground = true;
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            

            while (frmSplash == null || frmSplash.IsHandleCreated == false)
            {
                System.Threading.Thread.Sleep(TIMER_INTERVAL);
            }
            
        }

        public Splash()
            {
                InitializeComponent();
            }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            if (stringStatus.Length>0)
            {
                txtBxSplash.AppendText(stringStatus + Environment.NewLine);
                stringStatus = "";
            }

            if (opacityIncrement > 0)
            {
                if (this.Opacity < 1)
                    this.Opacity += opacityIncrement;
            }
            else
            {
                if (this.Opacity > 0)
                    this.Opacity += opacityIncrement;
                else
                    this.Close();
            }
        }
    }
}
