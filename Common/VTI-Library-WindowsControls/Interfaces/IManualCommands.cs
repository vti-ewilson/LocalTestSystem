using System;
using System.Windows.Forms;

namespace VTIWindowsControlLibrary.Interfaces
{
    /// <summary>
    /// Interface which must be implemented by the ManualCommands class of the client application.
    /// </summary>
    public interface IManualCommands
    {
        /// <summary>
        /// Checks to see if the specified scanner text is a manual command.
        /// </summary>
        /// <param name="scannerText">The scanner text.</param>
        /// <returns></returns>
        bool CheckForCommand(string scannerText);

        /// <summary>
        /// Executes the manual command specified by the scanner text.
        /// </summary>
        /// <param name="scannerText">The scanner text.</param>
        void ExecuteCommand(string scannerText);

        /// <summary>
        /// Hides the Manual Commands Form.
        /// </summary>
        void Hide();

        /// <summary>
        /// Hides the command.
        /// </summary>
        /// <param name="command">The command.</param>
        void HideCommand(Action command);

        /// <summary>
        /// Hides the command.
        /// </summary>
        /// <param name="command">The command.</param>
        void HideCommand(string command);

        /// <summary>
        /// Shows the Manual Commands Form.
        /// </summary>
        void Show();

        /// <summary>
        /// Shows the Manual Commands Form with the specified MDI parent.
        /// </summary>
        /// <param name="mdiParent">The MDI parent.</param>
        void Show(Form mdiParent);

        /// <summary>
        /// Shows the command.
        /// </summary>
        /// <param name="command">The command.</param>
        void ShowCommand(Action command);

        /// <summary>
        /// Shows the command.
        /// </summary>
        /// <param name="command">The command.</param>
        void ShowCommand(string command);

        void GenerateBarcodePDF(string outputPDFName);

        void GenerateManual(string outFile);
	}
}