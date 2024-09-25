using System;
using System.Collections.Generic;
using System.Globalization;
using System.Resources;
using System.Threading;
using System.Windows.Forms;
using VTIWindowsControlLibrary;
using VTIWindowsControlLibrary.Classes;

namespace LocalTestSystem
{
    internal static class Program
    {
        private static Forms.MainForm mainForm;
        public static Forms.MainForm MainForm { get { return mainForm; } }
        static bool success;
        static Mutex mut = new Mutex(true, "LocalTestSystem", out success);
        //static Mutex mut = new Mutex(true, "FlowTest", out success);

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (!success)
            {// application is already running 
                MessageBox.Show("Flow Restriction is already running", "Warning", MessageBoxButtons.OK);
                Environment.Exit(0);
            }
            // prevent garbage collector from deleting the mutex object
            GC.KeepAlive(mut);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Add a catch-all error handler
            VtiExceptionHandler eh = new VtiExceptionHandler();
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(eh.OnThreadException);

            VtiEvent.Log.Level = VTIWindowsControlLibrary.Classes.SystemDiagnostics.TraceLevel.Verbose; // Verbose until the config can be read

            mainForm = new Forms.MainForm();

            Machine.Initialize();

            Application.Run(mainForm);
        }
    }
}
