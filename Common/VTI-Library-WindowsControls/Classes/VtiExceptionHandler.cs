using System.Threading;
using System.Windows.Forms;

namespace VTIWindowsControlLibrary.Classes
{
    /// <summary>
    /// Represents a global exception handler for the client application.
    /// </summary>
    /// <remarks>
    /// If any unhandled exception occurs in the application, it will be trapped
    /// by the <see cref="OnThreadException">OnThreadException</see> method.
    /// An entry will be written to the <see cref="VTIWindowsControlLibrary.Components.VtiEventLog">VtiEventLog</see>.
    /// </remarks>
    public class VtiExceptionHandler
    {
        /// <summary>
        /// Handles any unhandled exception that occurs in the client application
        /// </summary>
        /// <param name="sender">Object that resulted in the exception.</param>
        /// <param name="t">Event arguments for the exception.</param>
        public void OnThreadException(object sender, ThreadExceptionEventArgs t)
        {
            DialogResult result = DialogResult.Cancel;
            try
            {
                VtiEvent.Log.WriteError(t.Exception.ToString(), VTIWindowsControlLibrary.Enums.VtiEventCatType.Application_Error);
            }
            catch
            {
                try
                {
                    MessageBox.Show("Fatal Error", "Fatal Error", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Stop);
                }
                finally
                {
                    Application.Exit();
                }
            }

            // Exits the program when the user clicks Abort.
            if (result == DialogResult.Abort)
                Application.Exit();
        }
    }
}