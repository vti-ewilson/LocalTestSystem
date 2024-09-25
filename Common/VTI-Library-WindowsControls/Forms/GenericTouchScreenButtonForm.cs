using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace VTIWindowsControlLibrary.Forms
{
    /// <summary>
    /// Provides a basic Touch Screen Button Form, which is used by ManualCommandsForm, SelectModelForm, etc.
    /// </summary>
    public partial class TouchScreenButtonForm<T> : Form
    {
        #region Nested Classes

        /// <summary>
        /// Represents a single command button on the form
        /// </summary>
        public class TouchScreenCommand
        {
            /// <summary>
            /// Name of the command button
            /// </summary>
            public string Name;

            /// <summary>
            /// Text to be displayed on the command button
            /// </summary>
            public string Text;

            /// <summary>
            /// An object that the client application can associate with the command button
            /// </summary>
            public T UserObject;

            /// <summary>
            /// ToolTip to be displayed on the command button
            /// </summary>
            public string ToolTipText;
        }

        /// <summary>
        /// Event arguments for events related to the Touch Screen Button Form
        /// </summary>
        public class TouchScreenButtonClickedEventArgs
        {
            private TouchScreenCommand _touchScreenCommand;

            /// <summary>
            /// Initializes a new instance of the <see cref="TouchScreenButtonClickedEventArgs">TouchScreenButtonClickedEventArgs</see>
            /// </summary>
            /// <param name="touchScreenCommand">Command button associated with the event.</param>
            public TouchScreenButtonClickedEventArgs(TouchScreenCommand touchScreenCommand)
            {
                _touchScreenCommand = touchScreenCommand;
            }

            /// <summary>
            /// Gets the command button associated with the event.
            /// </summary>
            public TouchScreenCommand TouchScreenCommand
            {
                get { return _touchScreenCommand; }
            }
        }

        /// <summary>
        /// Compares the Text fields of the TouchScreenCommands for sorting
        /// </summary>
        private class TouchScreenCommandComparer : IComparer<TouchScreenCommand>
        {
            public int Compare(TouchScreenCommand x, TouchScreenCommand y)
            {
                string left, right;
                left = string.IsNullOrEmpty(x.Text) ? x.Name : x.Text;
                right = string.IsNullOrEmpty(y.Text) ? y.Name : y.Text;
                return left.CompareTo(right);
            }
        }

        #endregion Nested Classes

        #region Delegates

        /// <summary>
        /// Delegate for calling events for the Touch Screen Button Form
        /// </summary>
        /// <param name="sender">Object calling the event</param>
        /// <param name="e">Event arguments</param>
        public delegate void TouchScreenButtonClickedEventHandler(object sender, TouchScreenButtonClickedEventArgs e);

        #endregion Delegates

        #region Event Handlers

        /// <summary>
        /// Occurs when the list of command buttons needs to be updated
        /// </summary>
        public event EventHandler UpdateList;

        /// <summary>
        /// Raises the <see cref="UpdateList">UpdateList</see> Event
        /// </summary>
        protected virtual void OnUpdateList()
        {
            if (UpdateList != null)
                UpdateList(this, null);
        }

        /// <summary>
        /// Occurs when a command button is clicked.
        /// </summary>
        public event TouchScreenButtonClickedEventHandler ButtonClicked;

        /// <summary>
        /// Raises the <see cref="ButtonClicked">ButtonClicked</see> Event
        /// </summary>
        /// <param name="touchScreenCommand">The touch screen command that was clicked.</param>
        protected virtual void OnButtonClicked(TouchScreenCommand touchScreenCommand)
        {
            if (ButtonClicked != null)
                ButtonClicked(this, new TouchScreenButtonClickedEventArgs(touchScreenCommand));
        }

        #endregion Event Handlers

        #region Globals

        private List<TouchScreenCommand> _commandList;
        private ToolTip toolTip = new ToolTip();
        TextFormatFlags toolTipFlags = TextFormatFlags.VerticalCenter
            | TextFormatFlags.LeftAndRightPadding
            | TextFormatFlags.HorizontalCenter
            | TextFormatFlags.NoClipping;
        Font toolTipFont = new Font("Arial", 12.0f, FontStyle.Bold);
       
        #endregion Globals

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="TouchScreenButtonForm">TouchScreenButtonForm</see>
        /// </summary>
        public TouchScreenButtonForm()
        {
            InitializeComponent();

            _commandList = new List<TouchScreenCommand>();
        }

        #endregion Construction

        #region Public Methods

        /// <summary>
        /// Refreshes the buttons on the form
        /// </summary>
        public void RefreshButtons()
        {
            Button newButton;
            Cursor = Cursors.WaitCursor;
            SuspendLayout();

            // clear the existing buttons
            flowLayoutPanel1.Controls.Clear();

            // sort the list
            _commandList.Sort(new TouchScreenCommandComparer());

            // add them to the form
            foreach (TouchScreenCommand command in _commandList)
            {
                newButton = new Button();
                newButton.Text = command.Text;
                if (!string.IsNullOrEmpty(command.ToolTipText))
                {
                    toolTip = new ToolTip();
                    toolTip.Active = true;
                    toolTip.OwnerDraw = true;
                    toolTip.UseAnimation = true;
                    toolTip.AutoPopDelay = 32000;
                    toolTip.Draw += new DrawToolTipEventHandler(toolTip_Draw);
                    toolTip.Popup += new PopupEventHandler(toolTip_Popup);
                    Regex rgx = new Regex("(.{50}\\s)"); 
                    string wrappedText = rgx.Replace(command.ToolTipText, "$1\n");
                    toolTip.SetToolTip(newButton, wrappedText);
                }
                newButton.Name = "button" + flowLayoutPanel1.Controls.Count.ToString();
                newButton.Tag = command;
                newButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12, FontStyle.Bold);
                newButton.Width = 220;
                newButton.Height = 60;
                newButton.Click += new System.EventHandler(buttonCommand_Click);
                flowLayoutPanel1.Controls.Add(newButton);
                flowLayoutPanel1.Controls[newButton.Name].SendToBack();
            }

            ResumeLayout();
            Cursor = Cursors.Default;
        }

        void toolTip_Popup(object sender, PopupEventArgs e)
        {
            // on popup set the size of tool tip
            string toolTipText = (sender as ToolTip).GetToolTip(e.AssociatedControl);
            using (var g = e.AssociatedControl.CreateGraphics())
            {
                var textSize = Size.Add(TextRenderer.MeasureText(
                    g, toolTipText, toolTipFont, Size.Empty, toolTipFlags), new Size(10, 5));
                e.ToolTipSize = textSize;
            }
        }

        void toolTip_Draw(object sender, DrawToolTipEventArgs e) => DrawToolTip(e);

        private void DrawToolTip(DrawToolTipEventArgs e)
        {
            using (var linearGradientBrush = new System.Drawing.Drawing2D.LinearGradientBrush(e.Bounds, Color.Ivory, Color.Beige, 45f))
            {
                e.Graphics.FillRectangle(linearGradientBrush, e.Bounds);
            }

            var shadowBounds = new Rectangle(new Point(e.Bounds.X + 1, e.Bounds.Y + 1), e.Bounds.Size);

            TextRenderer.DrawText(e.Graphics, e.ToolTipText, toolTipFont, shadowBounds, Color.LightGray, toolTipFlags);
            TextRenderer.DrawText(e.Graphics, e.ToolTipText, toolTipFont, e.Bounds, Color.Black, toolTipFlags);
        }

        #endregion Public Methods

        #region Public Properties

        /// <summary>
        /// Gets the list of command buttons on the form.
        /// </summary>
        public List<TouchScreenCommand> CommandList
        {
            get { return _commandList; }
        }

        #endregion Public Properties

        #region Events

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Hide();
            OnUpdateList();
        }

        public void buttonUpPush()
        {
            SuspendLayout();
            flowLayoutPanel1.Top += 198;
            while (flowLayoutPanel1.Top > 0) flowLayoutPanel1.Top -= 66;
            ResumeLayout();
        }

        private void buttonUp_Click(object sender, EventArgs e)
        {
            SuspendLayout();
            flowLayoutPanel1.Top += 198;
            while (flowLayoutPanel1.Top > 0) flowLayoutPanel1.Top -= 66;
            ResumeLayout();
        }

        public void buttonDownPush()
        {
            SuspendLayout();
            flowLayoutPanel1.Top -= 198;
            while (flowLayoutPanel1.Height + flowLayoutPanel1.Top < 200)
                flowLayoutPanel1.Top += 66;
            ResumeLayout();
        }

        private void buttonDown_Click(object sender, EventArgs e)
        {
            SuspendLayout();
            flowLayoutPanel1.Top -= 198;
            while (flowLayoutPanel1.Height + flowLayoutPanel1.Top < 200)
                flowLayoutPanel1.Top += 66;
            ResumeLayout();
        }

        private void buttonCommand_Click(object sender, EventArgs e)
        {
            OnButtonClicked((TouchScreenCommand)((Button)sender).Tag);
        }

        private void TouchScreenButtonForm_Load(object sender, EventArgs e)
        {
            OnUpdateList();
        }

        private void TouchScreenButtonForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if ((e.CloseReason == CloseReason.UserClosing))
            {
                e.Cancel = true;
                Hide();
                OnUpdateList();
            }
        }

        #endregion Events

        private void TouchScreenButtonForm_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible) RefreshButtons();
        }
    }
}