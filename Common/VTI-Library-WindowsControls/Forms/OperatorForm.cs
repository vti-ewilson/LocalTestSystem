using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using VTIWindowsControlLibrary.Classes;
using VTIWindowsControlLibrary.Components;

namespace VTIWindowsControlLibrary.Forms
{
    public partial class OperatorForm : Form
    {
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;

        public OperatorForm()
        {
            VtiEvent.Log.WriteVerbose("Initializing Operator Form...");
            InitializeComponent();
        }

        public OperatorForm(StringCollection PortNames, StringCollection PortColors, StringCollection Sequences, Boolean ShowPortNames)
        {
            Single ColumnWidth;
            int i, j;
            System.Windows.Forms.RichTextBox richTextBoxPrompt1;
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Panel panel1;

            InitializeComponent();

            this.SuspendLayout();

            ColumnWidth = 100F / (Single)PortNames.Count;

            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.OutsetDouble;
            this.tableLayoutPanel1.ColumnCount = PortNames.Count;
            for (i = 0; i < PortNames.Count; i++)
                this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, ColumnWidth));
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = ShowPortNames?3:2;
            if (ShowPortNames)
                this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.TabIndex = 1;

            for (i = 0; i < PortNames.Count; i++)
            {
                // add label for port name
                if (ShowPortNames)
                {
                    label1 = new Label();
                    this.tableLayoutPanel1.Controls.Add(label1, i, 0);
                    label1.Dock = System.Windows.Forms.DockStyle.Top;
                    label1.AutoSize = true;
                    label1.Font = new System.Drawing.Font("Arial", 18, System.Drawing.FontStyle.Bold);
                    label1.ForeColor = Color.FromName(PortColors[i].Split(',')[0]);
                    label1.BackColor = Color.FromName(PortColors[i].Split(',')[1]);
                    label1.Name = "labelPortName" + i.ToString();
                    label1.Text = PortNames[i];
                    label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                }

                // add rich text box operator prompt
                richTextBoxPrompt1 = new RichTextBox();
                tableLayoutPanel1.Controls.Add(richTextBoxPrompt1, i, ShowPortNames?1:0);
                richTextBoxPrompt1.BackColor = System.Drawing.Color.Black;
                richTextBoxPrompt1.Dock = System.Windows.Forms.DockStyle.Fill;
                richTextBoxPrompt1.Name = "richTextBoxPrompt" + i.ToString();
                richTextBoxPrompt1.TabIndex = 0;
                richTextBoxPrompt1.Text = "";
                richTextBoxPrompt1.ReadOnly = true;
                richTextBoxPrompt1.Cursor = Cursors.Default;
                richTextBoxPrompt1.TextChanged += new EventHandler(richTextBoxPrompt1_TextChanged);

                // add panel for sequences
                panel1 = new Panel();
                tableLayoutPanel1.Controls.Add(panel1, i, ShowPortNames?2:1);
                panel1.AutoSize = true;
                panel1.Dock = DockStyle.Fill;
                panel1.Name = "panelSequences" + i.ToString();

                // add sequences
                for (j = 0; j < Sequences.Count; j++)
                {
                    label1 = new Label();
                    panel1.Controls.Add(label1);
                    label1.Dock = System.Windows.Forms.DockStyle.Top;
                    label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    label1.Height = 18;
                    label1.Name = "labelSequence" + j.ToString();
                    label1.Text = Sequences[j];
                    label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                    label1.BringToFront();
                }
            }
            this.Controls.Add(this.tableLayoutPanel1);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        void richTextBoxPrompt1_TextChanged(object sender, EventArgs e)
        {
            // Select something other than the rich text box, to prevent the cursor from showing up
            // and to help control flicker
            if (((RichTextBox)sender).Focused)
                this.Controls["tableLayoutPanel1"].Controls["labelPortName0"].Select();
        }
    }
}