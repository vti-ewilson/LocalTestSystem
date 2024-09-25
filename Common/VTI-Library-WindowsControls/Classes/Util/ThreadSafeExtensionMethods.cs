using System;
using System.Drawing;
using System.Windows.Forms;

namespace VTIWindowsControlLibrary.Classes.Util
{
    /// <summary>
    /// Implements thread-safe setters for various controls
    /// </summary>
    public static class ThreadSafeExtensionMethods
    {
        private static Action<Control, string> setControlTextCallback = null;
        private static Action<Control, Color> setControlForeColorCallback = null;
        private static Action<Control, bool> setControlVisibleCallback = null;
        private static Action<Control> refreshControlCallback = null;
        private static Action<CheckBox, bool> setCheckBoxCheckedCallback = null;

        /// <summary>
        /// Initializes the <see cref="ThreadSafeExtensionMethods"/> class.
        /// </summary>
        static ThreadSafeExtensionMethods()
        {
            setControlTextCallback = new Action<Control, string>(SetText);
            setControlForeColorCallback = new Action<Control, Color>(SetForeColor);
            setControlVisibleCallback = new Action<Control, bool>(SetVisible);
            refreshControlCallback = new Action<Control>(RefreshSafe);
            setCheckBoxCheckedCallback = new Action<CheckBox, bool>(SetChecked);
        }

        /// <summary>
        /// Thread-safe method to set the Text of a <see cref="Control">Control</see>
        /// </summary>
        /// <param name="control">Control whose text is to be set</param>
        /// <param name="value">Text for the Control</param>
        public static void SetText(this Control control, string value)
        {
            if (control.InvokeRequired)
                control.Invoke(setControlTextCallback, control, value);
            else
                control.Text = value;
        }

        /// <summary>
        /// Thread-safe method to set the ForeColor of a <see cref="Control">Control</see>
        /// </summary>
        /// <param name="control">Control whose ForeColor is to be set</param>
        /// <param name="value">ForeColor for the Control</param>
        public static void SetForeColor(this Control control, Color value)
        {
            if (control.InvokeRequired)
                control.Invoke(setControlTextCallback, control, value);
            else
                control.ForeColor = value;
        }

        /// <summary>
        /// Thread-safe method to set the Text of a <see cref="Control">Control</see>
        /// </summary>
        /// <param name="control">Control whose text is to be set</param>
        /// <param name="value">Text for the Control</param>
        public static void SetVisible(this Control control, bool value)
        {
            if (control.InvokeRequired)
                control.Invoke(setControlVisibleCallback, control, value);
            else
                control.Visible = value;
        }

        /// <summary>
        /// Sets the checked state of a check box.
        /// </summary>
        /// <param name="checkBox">The check box.</param>
        /// <param name="value">Checked state.</param>
        public static void SetChecked(this CheckBox checkBox, bool value)
        {
            if (checkBox.InvokeRequired)
                checkBox.Invoke(setCheckBoxCheckedCallback, checkBox, value);
            else
                checkBox.Checked = value;
        }

        /// <summary>
        /// Refreshes the control in a thread-safe manner.
        /// </summary>
        /// <param name="control">The control.</param>
        public static void RefreshSafe(this Control control)
        {
            if (control.InvokeRequired)
                control.Invoke(refreshControlCallback, control);
            else
                control.Refresh();
        }

        /// <summary>
        /// Performs a thread-safe action.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="action">The action.</param>
        public static void ThreadSafeAction(this Control control, Action action)
        {
            if (control.InvokeRequired) control.Invoke(action);
            else action.Invoke();
        }
    }
}