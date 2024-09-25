using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using VTIWindowsControlLibrary.Classes.ManualCommands;

namespace VTIWindowsControlLibrary.Components
{
    /// <summary>
    /// Implements a tool-window to which "Manual Commands" can be added.
    /// </summary>
    /// <remarks>
    /// <para>
    /// A "Manual Command" is any method which has been decorated with the
    /// <see cref="VTIWindowsControlLibrary.Classes.ManualCommands.ManualCommandAttribute">ManualCommandAttribute</see>.
    /// The "Manual Command" methods must be members of a class that is a sub-class of
    /// <see cref="VTIWindowsControlLibrary.Classes.ManualCommands.ManualCommandsBase">ManualCommandsBase</see>.
    /// </para>
    /// <para>
    /// The "Manual Commands" will appear as a <see cref="Button">Button</see> within the control.
    /// </para>
    /// </remarks>
    public partial class MiniCommandControl : UserControl
    {
        private ManualCommandList _commands;
        private bool _autoAdjustWidth = true;
        /// <summary>
        /// List to which Manual Commands can be added
        /// </summary>
        public class ManualCommandList : List<Action>
        {
            private readonly Font _defaultButtonTextFont = new Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            private readonly Size _defaultButtonSize = new Size(120, 50);
            private readonly Color _defaultButtonColor = Color.FromName("Gainsboro");
            private readonly Action _defaultNewCommand = new Action(() => { });
            /// <summary>
            /// Event arguments for the <see cref="ManualCommandList">ManualCommandList</see>
            /// </summary>
            public class ManualCommandListEventArgs
            {
                private Action _command;
                private Action _newCommand;
                private Color _buttonColor;
                private Size _buttonSize;
                private Font _buttonTextFont;
                private int _numColumns;

                /// <summary>
                /// Initializes a new instance of the <see cref="ManualCommandListEventArgs">ManualCommandListEventArgs</see> class
                /// </summary>
                /// <param name="Command">Delegate for the Manual Command method</param>
                /// <param name="NewCommand">Command which will replace the old command when <see cref="Replace(Action, Action)">Replace (Action, Action) is called</see></param>
                /// <param name="ButtonBackColor">"Specifies the button's back color"</param>
                /// <param name="ButtonSize">"Specifies the size of the button"</param>
                /// <param name="NumColumns">"Specifies the number of columns to create in the <see cref="flowLayoutPanel1"/>flowLayoutPanel</param>
                public ManualCommandListEventArgs(Action Command, Action NewCommand, Color ButtonBackColor, Size ButtonSize, Font ButtonTextFont, int NumColumns)
                {
                    _command = Command;
                    _newCommand = NewCommand;
                    _buttonColor = ButtonBackColor;
                    _buttonSize = ButtonSize;
                    _buttonTextFont = ButtonTextFont;
                    _numColumns = NumColumns;
                }

                /// <summary>
                /// Gets the value of the <see cref="Action">Action</see> for the Manual Command
                /// </summary>
                public Action Command
                {
                    get { return _command; }
                }
                public Action NewCommand
                {
                    get { return _newCommand; }
                }
                public Color ButtonBackColor
                {
                    get { return _buttonColor; }
                }
                public Size ButtonSize
                {
                    get { return _buttonSize; }
                }
                public Font ButtonTextFont
                {
                    get { return _buttonTextFont; }
                }
                public int numColumns
                {
                    get { return _numColumns; }
                }
            }

            /// <summary>
            /// Delegate for event handlers for the <see cref="ManualCommandList">ManualCommandList</see>
            /// </summary>
            /// <param name="sender">The source of the event</param>
            /// <param name="e">An instance of the <see cref="ManualCommandListEventArgs">ManualCommandListEventArgs</see> that contains the event data</param>
            public delegate void ManualCommandListEventHandler(object sender, ManualCommandListEventArgs e);

            /// <summary>
            /// Occurs when an item is added to the <see cref="ManualCommandList">ManualCommandList</see>
            /// </summary>
            public event ManualCommandListEventHandler ItemAdded;

            /// <summary>
            /// Occurs when an item is removed from the <see cref="ManualCommandList">ManualCommandList</see>
            /// </summary>
            public event ManualCommandListEventHandler ItemRemoved;

            /// <summary>
            /// Occurs when the back color for a button is changed
            /// </summary>
            public event ManualCommandListEventHandler BackColorChanged;

            /// <summary>
            /// Occurs when an existing button's command is replaced with another command
            /// </summary>
            public event ManualCommandListEventHandler ItemReplaced;

            /// <summary>
            /// Occurs when the number of columns in the <see cref="flowLayoutPanel1"/> changed
            /// </summary>
            public event ManualCommandListEventHandler NumColumnsChanged;

            /// <summary>
            /// Adds an item to the <see cref="ManualCommandList">ManualCommandList. Default button color is 'Gainsboro'. Default button size is (120, 50).</see>
            /// </summary>
            /// <param name="item"><see cref="Action">Action</see> for the Manual Command method to be added</param>
            public new void Add(Action item)
            {
                base.Add(item);

                if (ItemAdded != null) ItemAdded(this, new ManualCommandListEventArgs(item, _defaultNewCommand, _defaultButtonColor, _defaultButtonSize, _defaultButtonTextFont, 1));
            }

            /// <summary>
            /// Adds an item to the <see cref="ManualCommandList">ManualCommandList.</see>
            /// </summary>
            /// <param name="item"><see cref="Action">Action</see> for the Manual Command method to be added</param>
            /// <param name="buttonBackColor">Back color of the button. Default button color is 'Gainsboro'</param>
            public void Add(Action item, Color buttonBackColor)
            {
                base.Add(item);

                if (ItemAdded != null) ItemAdded(this, new ManualCommandListEventArgs(item, _defaultNewCommand, buttonBackColor, _defaultButtonSize, _defaultButtonTextFont, 1));
            }

            /// <summary>
            /// Adds an item to the <see cref="ManualCommandList">ManualCommandList.</see>
            /// </summary>
            /// <param name="item"><see cref="Action">Action</see> for the Manual Command method to be added</param>
            /// <param name="buttonBackColor">Back color of the button. Default button color is 'Gainsboro'</param>
            /// <param name="buttonSize">Size of the button. Default size is (120, 50)</param>
            /// <param name="buttonTextFont">Font of the text displayed on the button. Default is Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));</param>
            public void Add(Action item, Color buttonBackColor, Size buttonSize, Font buttonTextFont)
            {
                base.Add(item);

                if (ItemAdded != null) ItemAdded(this, new ManualCommandListEventArgs(item, _defaultNewCommand, buttonBackColor, buttonSize, buttonTextFont, 1));
            }

            /// <summary>
            /// Removes an item to the <see cref="ManualCommandList">ManualCommandList</see>
            /// </summary>
            /// <param name="item"><see cref="Action">Action</see> for the Manual Command method to be removed</param>
            public new void Remove(Action item)
            {
                base.Remove(item);

                if (ItemRemoved != null) ItemRemoved(this, new ManualCommandListEventArgs(item, _defaultNewCommand, _defaultButtonColor, _defaultButtonSize, _defaultButtonTextFont, 1));
            }

            /// <summary>
            /// Changes the button's back color
            /// </summary>
            /// <param name="item"><see cref="Action">Action</see> for the Manual Command method to be added</param>
            /// <param name="buttonBackColor">Back color of the button. Default button color is 'Gainsboro'</param>
            public void ChangeButtonColor(Action item, Color buttonBackColor)
            {
                if (base.Contains(item))
                {
                    BackColorChanged(this, new ManualCommandListEventArgs(item, _defaultNewCommand, buttonBackColor, _defaultButtonSize, _defaultButtonTextFont, 1));
                }
            }

            /// <summary>
            /// Replaces the button's old command with the new command
            /// </summary>
            /// <param name="oldCommand"><see cref="Action">Action</see> which will be replaced</param>
            /// <param name="newCommand"><see cref="Action">Action</see> which will be replace the previous command</param>
            public void Replace(Action oldCommand, Action newCommand)
            {
                if (base.Contains(oldCommand))
                {
                    base.Remove(oldCommand);
                    base.Add(newCommand);
                    if (ItemReplaced != null) ItemReplaced(this, new ManualCommandListEventArgs(oldCommand, newCommand, _defaultButtonColor, _defaultButtonSize, _defaultButtonTextFont, 1));
                }
            }

            /// <summary>
            /// Replaces the button's old command with the new command and sets the back color of the new command to the selected color
            /// </summary>
            /// <param name="oldCommand"><see cref="Action">Action</see> which will be replaced</param>
            /// <param name="newCommand"><see cref="Action">Action</see> which will be replace the previous command</param>
            /// <param name="buttonBackColor">Back color of the button. Default button color is 'Gainsboro'</param>
            public void Replace(Action oldCommand, Action newCommand, Color buttonBackColor)
            {
                if (base.Contains(oldCommand))
                {
                    base.Remove(oldCommand);
                    base.Add(newCommand);
                    if (ItemReplaced != null) ItemReplaced(this, new ManualCommandListEventArgs(oldCommand, newCommand, buttonBackColor, _defaultButtonSize, _defaultButtonTextFont, 1));
                }
            }

            //not implemented yet
            //public void SetNumColumns(int numColumns)
            //{
            //    if (numColumns >= 1)
            //    {
            //        NumColumnsChanged(this, new ManualCommandListEventArgs(_defaultNewCommand, _defaultNewCommand, _defaultButtonColor, _defaultButtonSize, 1));
            //    }
            //}
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MiniCommandControl">MiniCommandControl</see> class
        /// </summary>
        public MiniCommandControl()
        {
            InitializeComponent();

            _commands = new ManualCommandList();
            _commands.ItemAdded += new ManualCommandList.ManualCommandListEventHandler(_commands_ItemAdded);
            _commands.ItemRemoved += new ManualCommandList.ManualCommandListEventHandler(_commands_ItemRemoved);
            _commands.ItemReplaced += _commands_ItemReplaced;
            _commands.BackColorChanged += _commands_BackColorChanged;
            _commands.NumColumnsChanged += _commands_NumColumnsChanged;
            //no multicolumn
            flowLayoutPanel1.HorizontalScroll.Visible = false;
            flowLayoutPanel1.AutoScroll = true;

            //flowLayoutPanel1.WrapContents = false;
            //flowLayoutPanel1.AutoSize = true;

            //For command button wrapping
            flowLayoutPanel1.WrapContents = true;
            flowLayoutPanel1.AutoSize = false;

        }

        private void _commands_NumColumnsChanged(object sender, ManualCommandList.ManualCommandListEventArgs e)
        {
            //todo - add split container to OpFormSingle with two panels
            //one for prompt and other for miniCommandControl so you can adjust miniCommandControl width
            //then add a TableLayoutPanel(?) inside of the miniCommandControl and programatically add/remove columns
        }

        private void _commands_ItemReplaced(object sender, ManualCommandList.ManualCommandListEventArgs e)
        {
            var buttonList = flowLayoutPanel1.Controls.OfType<Button>().Where(b => e.Command == (b.Tag as Action));
            foreach (Button button in buttonList)
            {
                foreach (ManualCommandAttribute attribute in e.NewCommand.Method.GetCustomAttributes(typeof(ManualCommandAttribute), false))
                {
                    //String resName;
                    //button.Text = attribute.CommandText;
                    button.Text = VtiLib.Localization.GetString("ManualCommand" + attribute.CommandText.Replace(" ", "")) ?? attribute.CommandText;
                    button.BackColor = e.ButtonBackColor;
                    //resName = VtiLib.Localization.GetString("ManualCommand" + e.NewCommand.Method.Name);
                    //if (resName != null) button.Text = resName;
                    button.Tag = e.NewCommand;
                    //button.Click += new EventHandler(button1_Click);
                }
            }
        }

        private void _commands_BackColorChanged(object sender, ManualCommandList.ManualCommandListEventArgs e)
        {
            var buttonList = flowLayoutPanel1.Controls.OfType<Button>().Where(b => e.Command == (b.Tag as Action)).ToList();
            if (buttonList.Count > 0)
            {
                buttonList.ForEach(x => x.BackColor = e.ButtonBackColor);
            }
        }

        private void _commands_ItemRemoved(object sender, MiniCommandControl.ManualCommandList.ManualCommandListEventArgs e)
        {
            //Button button1 = null;

            //foreach (Button button in flowLayoutPanel1.Controls)
            //    if (e.Command == (button.Tag as Action))
            //    {
            //        button1 = button;
            //        break;
            //    }
            Button button1 = flowLayoutPanel1.Controls.OfType<Button>().FirstOrDefault(b => e.Command == (b.Tag as Action));
            if (button1 != null) flowLayoutPanel1.Controls.Remove(button1);

            //Auto-decrease miniCommandToolbar width based on if horizontal scroll is not visible
            if (_autoAdjustWidth && _commands.Count > 0)
            {
                var CurrentOpFormSingle = Application.OpenForms["OperatorFormSingleNested"];
                if (CurrentOpFormSingle != null)
                {
                    var currentMiniCmdToolbar = CurrentOpFormSingle.Controls.OfType<SplitContainer>().FirstOrDefault().Controls.OfType<SplitterPanel>().FirstOrDefault().Controls.OfType<MiniCommandControl>().FirstOrDefault();
                    if (currentMiniCmdToolbar != null)
                    {
                        while (!flowLayoutPanel1.HorizontalScroll.Visible)
                        {
                            currentMiniCmdToolbar.Width -= 10;
                        }
                        //increase again so that horizontal scroll bar is not visible again
                        currentMiniCmdToolbar.Width += 10;
                    }
                }
            }
        }

        private void _commands_ItemAdded(object sender, MiniCommandControl.ManualCommandList.ManualCommandListEventArgs e)
        {
            Button button1;

            button1 = new Button();
            button1.BackColor = e.ButtonBackColor;
            button1.Font = e.ButtonTextFont;
            button1.Name = "button" + e.Command.Method.Name;
            if (this.Dock == DockStyle.Top || this.Dock == DockStyle.Bottom || this.Dock == DockStyle.Fill)
                button1.Size = new Size(flowLayoutPanel1.Width - flowLayoutPanel1.Margin.Left - flowLayoutPanel1.Margin.Right, 50);
            else
                button1.Size = e.ButtonSize;

            foreach (ManualCommandAttribute attribute in e.Command.Method.GetCustomAttributes(typeof(ManualCommandAttribute), false))
            {
                //String resName;
                //button1.Text = attribute.CommandText;
                button1.Text = VtiLib.Localization.GetString("ManualCommand" + attribute.CommandText.Replace(" ", "")) ?? attribute.CommandText;

                ////resName = VtiLib.Localization.GetString("ManualCommand" + e.Command.Method.Name);
                //resName = VtiLib.Localization.GetString("ManualCommand" + t.FirstOrDefault().Text);
                //if (resName != null) button1.Text = resName;
                button1.Tag = e.Command;
                button1.Click += new EventHandler(button1_Click);
            }

            flowLayoutPanel1.Controls.Add(button1);
            //no multicolumn
            if (flowLayoutPanel1.VerticalScroll.Visible)
            {
                //add padding so part of button is not hidden by vertical scrol bar
                flowLayoutPanel1.Padding = new Padding(0, 0, SystemInformation.VerticalScrollBarWidth, 0);
            }
            else
            {
                flowLayoutPanel1.Padding = new Padding(0, 0, 0, 0);
            }

            //Auto-increase miniCommandToolbar width based on if horizontal scroll is visible
            if (_autoAdjustWidth)
            {
                var CurrentOpFormSingle = Application.OpenForms["OperatorFormSingleNested"];
                if (CurrentOpFormSingle != null)
                {
                    var currentMiniCmdToolbar = CurrentOpFormSingle.Controls.OfType<SplitContainer>().FirstOrDefault().Controls.OfType<SplitterPanel>().FirstOrDefault().Controls.OfType<MiniCommandControl>().FirstOrDefault();
                    if (currentMiniCmdToolbar != null)
                    {
                        while (flowLayoutPanel1.HorizontalScroll.Visible && currentMiniCmdToolbar.Width != currentMiniCmdToolbar.MaximumSize.Width)
                        {
                            currentMiniCmdToolbar.Width += 10;
                        }
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            Action item = button.Tag as Action;
            //item.Method.Invoke(VtiLib.Machine.GetField("ManualCommands").GetValue(null), null);
            item.Invoke();
        }

        /// <summary>
        /// Gets the list of commands in the control
        /// </summary>
        public ManualCommandList Commands
        {
            get { return _commands; }
        }

        public bool AutoAdjustWidth
        {
            get { return _autoAdjustWidth; }
            set { _autoAdjustWidth = value; }
        }

        private void toolStripButtonClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}