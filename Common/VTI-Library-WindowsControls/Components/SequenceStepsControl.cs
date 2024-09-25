using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using VTIWindowsControlLibrary.Classes;

namespace VTIWindowsControlLibrary.Components
{
    /// <summary>
    /// A UserControl that displays a list of <see cref="SequenceStep">SequenceSteps</see>
    /// </summary>
    public partial class SequenceStepsControl : UserControl
    {
        /// <summary>
        /// Represents a list of <see cref="SequenceStep">SequenceSteps</see>
        /// </summary>
        public class SequenceStepList : List<SequenceStep>
        {
            /// <summary>
            /// Event arguments for Sequence Step events
            /// </summary>
            public class SequenceStepEventArgs
            {
                private SequenceStep _step;

                /// <summary>
                /// Initializes a new instance of the <see cref="SequenceStepEventArgs">SequenceStepEventArgs</see>
                /// </summary>
                /// <param name="Step">Sequence step for the event</param>
                public SequenceStepEventArgs(SequenceStep Step)
                {
                    _step = Step;
                }

                /// <summary>
                /// Gets the sequence step for the event
                /// </summary>
                public SequenceStep Step
                {
                    get { return _step; }
                }
            }

            /// <summary>
            /// Delegate for calling sequence step events
            /// </summary>
            /// <param name="sender">Object calling the event</param>
            /// <param name="e">Event arguments</param>
            public delegate void SequenceStepEventHandler(object sender, SequenceStepEventArgs e);

            /// <summary>
            /// Occurs when an item is added to the Sequence Step List
            /// </summary>
            public event SequenceStepEventHandler ItemAdded;

            /// <summary>
            /// Raises the <see cref="ItemAdded">ItemAdded</see> event
            /// </summary>
            /// <param name="EventArgs">Sequence Step Event Arguments</param>
            protected virtual void OnItemAdded(SequenceStepEventArgs EventArgs)
            {
                if (ItemAdded != null) ItemAdded(this, EventArgs);
            }

            /// <summary>
            /// Occurs when an item is removed from the Sequence Step List
            /// </summary>
            public event SequenceStepEventHandler ItemRemoved;

            /// <summary>
            /// Raises the <see cref="ItemRemoved">ItemRemoved</see> event
            /// </summary>
            /// <param name="EventArgs">Sequence Step Event Arguments</param>
            protected virtual void OnItemRemoved(SequenceStepEventArgs EventArgs)
            {
                if (ItemRemoved != null) ItemRemoved(this, EventArgs);
            }

            /// <summary>
            /// Occurs when an item is change in the Sequence Step list
            /// </summary>
            public event SequenceStepEventHandler ItemChanged;

            /// <summary>
            /// Raises the <see cref="ItemChanged">ItemChanged</see> event
            /// </summary>
            /// <param name="EventArgs"></param>
            protected virtual void OnItemChanged(SequenceStepEventArgs EventArgs)
            {
                if (ItemChanged != null) ItemChanged(this, EventArgs);
            }

            /// <summary>
            /// Occurs when an item is change in the Sequence Step list
            /// </summary>
            public event EventHandler Cleared;

            /// <summary>
            /// Raises the <see cref="Cleared">Cleared</see> event
            /// </summary>
            /// <param name="EventArgs"></param>
            protected virtual void OnCleared(EventArgs EventArgs)
            {
                if (Cleared != null) Cleared(this, EventArgs);
            }

            /// <summary>
            /// Adds a <see cref="SequenceStep">SequenceStep</see> to the list
            /// </summary>
            /// <param name="Step">Sequence Step to be added.</param>
            public new void Add(SequenceStep Step)
            {
                base.Add(Step);
                Step.StepChanged += new EventHandler(Step_StepChanged);
                OnItemAdded(new SequenceStepEventArgs(Step));
            }

            /// <summary>
            /// Inserts a <see cref="SequenceStep">SequenceStep</see> into the list
            /// </summary>
            /// <param name="Index">Zero-based index at which Sequence Step should be inserted.</param>
            /// <param name="Step">Sequence Step to be inserted.</param>
            public new void Insert(int Index, SequenceStep Step)
            {
                SequenceStepList tempList = new SequenceStepList();
                int ii, cnt = this.Count;
                if (Index < 0)
                    Index = 0;
                if (Index >= this.Count)
                    Index = this.Count;
                for (ii = Index; ii < cnt; ii++)
                {
                    // keep track of the steps that come after Index
                    tempList.Add(this[Index]);
                    // remove the steps that come after Index
                    this.Remove(this[Index]);
                }
                // add the step at Index
                Add(Step);
                // add the steps that you just removed
                for (ii = Index; ii < cnt; ii++)
                {
                    Add(tempList[ii - Index]);
                }
            }

            private void Step_StepChanged(object sender, EventArgs e)
            {
                OnItemChanged(new SequenceStepEventArgs((SequenceStep)sender));
            }

            /// <summary>
            /// Removes a <see cref="SequenceStep">SequenceStep</see> from the list.
            /// </summary>
            /// <param name="Step">Sequence Step to be removed.</param>
            public new void Remove(SequenceStep Step)
            {
                Step.StepChanged -= new EventHandler(Step_StepChanged);
                base.Remove(Step);

                OnItemRemoved(new SequenceStepEventArgs(Step));
            }

            /// <summary>
            /// Removes all of the elements from the list.
            /// </summary>
            public new void Clear()
            {
                foreach (SequenceStep step in this)
                    step.StepChanged -= new EventHandler(Step_StepChanged);

                base.Clear();

                OnCleared(null);
            }

            /// <summary>
            /// Adds the elements of the specified collection to the end of the list.
            /// </summary>
            /// <param name="collection">The collection whose elements should be added to the end of the list.</param>
            public new void AddRange(IEnumerable<SequenceStep> collection)
            {
                foreach (SequenceStep step in collection)
                    this.Add(step);
            }
        }

        private SequenceStepList _sequences;

        /// <summary>
        /// Initializes a new instance of the <see cref="SequenceStepsControl">SequenceStepsControl</see>
        /// </summary>
        public SequenceStepsControl()
        {
            InitializeComponent();

            _sequences = new SequenceStepList();
            _sequences.ItemAdded += new SequenceStepList.SequenceStepEventHandler(_sequences_ItemAdded);
            _sequences.ItemRemoved += new SequenceStepList.SequenceStepEventHandler(_sequences_ItemRemoved);
            _sequences.ItemChanged += new SequenceStepList.SequenceStepEventHandler(_sequences_ItemChanged);
            _sequences.Cleared += new EventHandler(_sequences_Cleared);
        }

        private void _sequences_Cleared(object sender, EventArgs e)
        {
            ClearControls();
        }

        private void ClearControls()
        {
            if (this.InvokeRequired)
                this.Invoke(new Action(ClearControls));
            else
                this.Controls.Clear();
        }

        private void _sequences_ItemChanged(object sender, SequenceStepsControl.SequenceStepList.SequenceStepEventArgs e)
        {
            SetStepLabel(e.Step);
        }

        private void SetStepLabel(SequenceStep Step)
        {
            if (Step.Label.InvokeRequired)
                Step.Label.Invoke(new Action<SequenceStep>(SetStepLabel), Step);
            else if (Step.Label != null)
            {
                Step.Label.Text = Step.Text;
                Step.Label.ForeColor = Step.ForeColor;
                Step.Label.BackColor = Step.BackColor;
            }
        }

        private void _sequences_ItemRemoved(object sender, SequenceStepsControl.SequenceStepList.SequenceStepEventArgs e)
        {
            if (e.Step.Label != null)
                RemoveControl(e.Step.Label);
        }

        private void RemoveControl(Control Control)
        {
            if (this.InvokeRequired)
                this.Invoke(new Action<Control>(RemoveControl), Control);
            else
                this.Controls.Remove(Control);
        }

        private void _sequences_ItemAdded(object sender, SequenceStepsControl.SequenceStepList.SequenceStepEventArgs e)
        {
            AddStepLabel(e.Step);
        }

        private Font _font = new Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        /// <summary>
        /// Gets or sets the font of the text label displayed for the sequnce step. Default value is "Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))
        /// In Machine.cs, InitializeOperatorForm(), set _OpFormSingle.sequenceStepsControl1.LabelFont = (value).
        /// Must change the modifiers property of the sequenceStepsControl in the OpForm to Public.
        /// </summary>
        public Font LabelFont
        {
            get
            {
                return _font;
            }
            set
            {
                _font = value;
            }
        }

        private int _height = 20;
        /// <summary>
        /// Gets or sets the height of the label displayed for the sequnce step. Default value is 20.
        /// In Machine.cs, InitializeOperatorForm(), set _OpFormSingle.sequenceStepsControl1.LabelHeight = (value).
        /// Must change the modifiers property of the sequenceStepsControl in the OpForm to Public.
        /// </summary>
        public int LabelHeight
        {
            get
            {
                return _height;
            }
            set
            {
                _height = value;
            }
        }
        private void AddStepLabel(SequenceStep Step)
        {
            if (this.InvokeRequired)
                this.Invoke(new Action<SequenceStep>(AddStepLabel), Step);
            else
            {
                Step.Label = new Label();
                Step.Label.Dock = System.Windows.Forms.DockStyle.Top;
                Step.Label.Font = _font;
                Step.Label.Name = "label" + Step.Text.Replace(" ", "");
                Step.Label.Size = new System.Drawing.Size(this.Width, _height);
                Step.Label.Text = Step.Text;
                Step.Label.ForeColor = Step.ForeColor;
                Step.Label.BackColor = Step.BackColor;
                Step.Label.TextAlign = System.Drawing.ContentAlignment.TopCenter;
                this.Controls.Add(Step.Label);
                Step.Label.BringToFront();
            }
        }

        /// <summary>
        /// Gets the list of <see cref="SequenceStep">SequenceSteps</see> for the control.
        /// </summary>
        public SequenceStepList Sequences
        {
            get { return _sequences; }
        }
    }
}