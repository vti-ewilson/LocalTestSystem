using System.Windows.Forms;
using VTIWindowsControlLibrary.Components;
using VTIWindowsControlLibrary.Forms;

namespace VTIWindowsControlLibrary.Classes.ClientForms
{
    /// <summary>
    /// Represents the <see cref="EditCycleForm">Edit Cycle Form</see> of the client application
    /// </summary>
    /// <remarks>
    /// This class isn't the <see cref="EditCycleForm">EditCycleForm</see> itself, but contains a
    /// private static instance of it, and has static methods for accessing it.
    /// </remarks>
    public class EditCycle
    {
        #region Globals

        public static EditCycleForm editCycle;

        #endregion Globals

        #region Construction

        /// <summary>
        /// Initializes the static instance of the <see cref="EditCycle">EditCycle</see> class
        /// </summary>
        static EditCycle()
        {
            editCycle = new EditCycleForm();
            //editCycle.buttonSearch.Visible = false;
        }

        #endregion Construction

        #region Public Methods

        /// <summary>
        /// Shows the <see cref="EditCycleForm">EditCycleForm</see>
        /// </summary>
        public static void Show()
        {
            Show(null);
        }

        /// <summary>
        /// Shows the <see cref="EditCycleForm">EditCycleForm</see>
        /// </summary>
        /// <param name="MdiParent">Form that is to be the owner of this form.</param>
        public static void Show(Form MdiParent)
        {
            if (MdiParent != null) editCycle.MdiParent = MdiParent;
            editCycle.Show();
            editCycle.BringToFront();
        }

        /// <summary>
        /// Assigns the client side sequence steps to <see cref="EditCycleForm">EditCycleForm</see>
        /// </summary>
        public static void AssignSequenceSteps(SequenceStepsControl.SequenceStepList[] seqStepList)
        {
            editCycle.sequenceStepList = seqStepList;
            //editCycle.buttonSearch.Visible = true;
        }

        /// <summary>
        /// Assigns the client side sequence steps to <see cref="EditCycleForm">EditCycleForm</see>
        /// </summary>
        /// <param name="MdiParent">Form that is to be the owner of this form.</param>
        public static void AssignOperatingSequences()
        {
            editCycle.AssignOperatingSequences();
        }

        /// <summary>
        /// Shows the <see cref="EditCycleForm">EditCycleForm</see> as a modal dialog box.
        /// </summary>
        public static void ShowDialog()
        {
            editCycle.ShowDialog();
        }

        #endregion Public Methods
    }
}