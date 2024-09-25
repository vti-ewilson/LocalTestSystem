using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace VTIWindowsControlLibrary.Components
{
    /// <summary>
    /// Subclass of a <see cref="RichTextBox">RichTextBox</see> that has been
    /// extended to provide capabilities for an operator prompt for the client application.
    /// </summary>
    public partial class RichTextPrompt : RichTextBox
    {
        private Font _DefaultFont;
        private Color _DefaultColor;

        #region Delegates

        private Action _ClearCallback;
        private Action<String, Font, Color> _AppendTextCallback;
        private Action<String, Font, Color> _PrependTextCallback;
        private Action<String, String> _ReplaceTextCallback;
        private Action<String, String> _ReplaceAllTextCallback;
        private Action<String, String, Font, Color> _ReplaceRegexCallback;
        private Action<String, String, Font, Color> _ReplaceAllRegexCallback;
        private Func<String> _GetTextCallback;

        #endregion Delegates

        /// <summary>
        /// Initializes a new instance of the <see cref="RichTextPrompt">RichTextPrompt</see>
        /// </summary>
        public RichTextPrompt()
        {
            InitializeComponent();
            Init();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RichTextPrompt">RichTextPrompt</see>
        /// </summary>
        /// <param name="container">Control that will contain the <see cref="RichTextPrompt">RichTextPrompt</see></param>
        public RichTextPrompt(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
            Init();
        }

        private void Init()
        {
            _DefaultFont = new Font("Arial", 18, FontStyle.Regular);
            _DefaultColor = Color.Yellow;
            _ClearCallback = new Action(Clear);
            _AppendTextCallback = new Action<string, Font, Color>(AppendText);
            _PrependTextCallback = new Action<string, Font, Color>(PrependText);
            _ReplaceTextCallback = new Action<string, string>(ReplaceText);
            _ReplaceAllTextCallback = new Action<string, string>(ReplaceAllText);
            _ReplaceRegexCallback = new Action<string, string, Font, Color>(ReplaceRegex);
            _ReplaceAllRegexCallback = new Action<string, string, Font, Color>(ReplaceAllRegex);
            _GetTextCallback = new Func<string>(GetText);
        }



        #region Public Properties

        /// <summary>
        /// Gets the text string contained in the operator prompt <see cref="RichTextPrompt">GetText</see>
        /// </summary>
        public string GetText()
        {
            if (this.InvokeRequired)
            {
                return this.Invoke(_GetTextCallback, null) as string;
            }
            else
            {
                return this.Text;
            }
        }

        //public String Text
        //{
        //    get
        //    {
        //        return GetText();
        //    }
        //    set
        //    {
        //        this.SuspendLayout();
        //        this.Clear();
        //        this.AppendText(value);
        //        this.ResumeLayout();
        //    }
        //}

        /// <summary>
        /// Gets or sets the Default <see cref="System.Drawing.Font">Font</see> for
        /// messages appended to the prompt.
        /// </summary>
        public new Font DefaultFont
        {
            get { return _DefaultFont; }
            set { _DefaultFont = value; }
        }

        /// <summary>
        /// Gets or sets the Default <see cref="System.Drawing.Color">Color</see> for
        /// messages appended to the prompt.
        /// </summary>
        public Color DefaultColor
        {
            get { return _DefaultColor; }
            set { _DefaultColor = value; }
        }

        #endregion Public Properties

        #region Public Members

        /// <summary>
        /// Clears all text from the operator prompt.
        /// </summary>
        public new void Clear()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(_ClearCallback, null);
            }
            else
            {
                base.Clear();
            }
        }

        /// <summary>
        /// Appends text to the operator prompt using the
        /// <see cref="DefaultFont">Default Font</see> and
        /// <see cref="DefaultColor">Default Color</see>
        /// </summary>
        /// <param name="text">Text to be appended to the operator prompt.</param>
        public new void AppendText(String text)
        {
            this.AppendText(text, DefaultFont, DefaultColor);
        }

        /// <summary>
        /// Appends text to the operator prompt using the
        /// <see cref="DefaultFont">Default Font</see> and
        /// the specified Color.
        /// </summary>
        /// <param name="text">Text to be appended to the operator prompt.</param>
        /// <param name="color">Color of the text to be appended to the operator prompt.</param>
        public void AppendText(String text, Color color)
        {
            this.AppendText(text, DefaultFont, color);
        }

        /// <summary>
        /// Appends text to the operator prompt using the specified Font and Color.
        /// </summary>
        /// <param name="text">Text to be appended to the operator prompt.</param>
        /// <param name="font">Font to be used for the text.</param>
        /// <param name="color">Color of the text to be appended to the operator prompt.</param>
        public void AppendText(String text, Font font, Color color)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(_AppendTextCallback, text, font, color);
            }
            else
            {
                this.SuspendLayout();
                this.ReadOnly = false;
                if (this.Rtf != null) this.SelectionStart = this.Rtf.Length;
                this.SelectionFont = font;
                this.SelectionColor = color;
                base.AppendText(text);
                this.ReadOnly = true;
                this.ResumeLayout();
            }
        }

        /// <summary>
        /// Adds text to the top of the operator prompt using the
        /// <see cref="DefaultFont">Default Font</see> and
        /// <see cref="DefaultColor">Default Color</see>
        /// </summary>
        /// <param name="text">Text to be added to the operator prompt.</param>
        public void PrependText(String text)
        {
            this.PrependText(text, DefaultFont, DefaultColor);
        }

        /// <summary>
        /// Adds text to the top of the operator prompt using the
        /// <see cref="DefaultFont">Default Font</see> and
        /// specified Color
        /// </summary>
        /// <param name="text">Text to be added to the operator prompt.</param>
        /// <param name="color">Color of the text to be added to the operator prompt.</param>
        public void PrependText(String text, Color color)
        {
            this.PrependText(text, DefaultFont, color);
        }

        /// <summary>
        /// Adds text to the top of the operator prompt using the
        /// specified Font and Color.
        /// </summary>
        /// <param name="text">Text to be added to the operator prompt.</param>
        /// <param name="font">Font to be used for the text.</param>
        /// <param name="color">Color of the text to be added to the operator prompt.</param>
        public void PrependText(String text, Font font, Color color)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(_PrependTextCallback, text, font, color);
            }
            else
            {
                this.SuspendLayout();
                this.ReadOnly = false;
                this.SelectionStart = 0;
                this.SelectionLength = 0;
                this.SelectionFont = font;
                this.SelectionColor = color;
                this.SelectedText = text;
                this.ReadOnly = true;
                this.ResumeLayout();
            }
        }

        /// <summary>
        /// Replaces the SearchText with the ReplacementText in the operator prompt.
        /// </summary>
        /// <param name="SearchText">Text to be replaced.</param>
        /// <param name="ReplacementText">Replacement text.</param>
        public void ReplaceText(String SearchText, String ReplacementText)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(_ReplaceTextCallback, SearchText, ReplacementText);
            }
            else
            {
                this.SuspendLayout();
                this.ReadOnly = false;
                if (this.Find(SearchText.Replace("\r\n", "\r").Replace("\n", "\r")) != -1)
                    this.SelectedText = ReplacementText;
                this.ReadOnly = true;
                this.ResumeLayout();
            }
        }

        /// <summary>
        /// Replaces all instances of the SearchText in the operator prompt with the ReplacementText.
        /// </summary>
        /// <param name="SearchText">Text to be replaced.</param>
        /// <param name="ReplacementText">Replacement text.</param>
        public void ReplaceAllText(String SearchText, String ReplacementText)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(_ReplaceAllTextCallback, SearchText, ReplacementText);
            }
            else
            {
                this.SuspendLayout();
                this.ReadOnly = false;
                while (this.Find(SearchText.Replace("\r\n", "\r").Replace("\n", "\r")) != -1)
                    this.SelectedText = ReplacementText;
                this.ReadOnly = true;
                this.ResumeLayout();
            }
        }

        /// <summary>
        /// Removes the specified text from the operator prompt.
        /// </summary>
        /// <param name="TextToRemove">Text to be removed from the operator prompt.</param>
        public void RemoveText(String TextToRemove)
        {
            this.ReplaceRegex(TextToRemove + @"\s*", string.Empty);
        }

        /// <summary>
        /// Removes all instances of a specified text string from the operator prompt.
        /// </summary>
        /// <param name="TextToRemove">Text to be removed from the operator prompt.</param>
        public void RemoveAllText(String TextToRemove)
        {
            this.ReplaceAllRegex(TextToRemove + @"\s*", string.Empty);
        }

        /// <summary>
        /// Uses <see cref="System.Text.RegularExpressions">Regular Expressions</see>
        /// to find and replace text in the operator prompt.
        /// The new text will use the
        /// <see cref="DefaultFont">Default Font</see> and
        /// <see cref="DefaultColor">Default Color</see>.
        /// </summary>
        /// <param name="regex">Regex string to use for the search.</param>
        /// <param name="ReplacementText">Replacement Text.</param>
        public void ReplaceRegex(String regex, String ReplacementText)
        {
            this.ReplaceRegex(regex, ReplacementText, DefaultFont, DefaultColor);
        }

        /// <summary>
        /// Uses <see cref="System.Text.RegularExpressions">Regular Expressions</see>
        /// to find and replace text in the operator prompt.
        /// The new text will use the
        /// <see cref="DefaultFont">Default Font</see> and
        /// the specified color.
        /// </summary>
        /// <param name="regex">Regex string to use for the search.</param>
        /// <param name="ReplacementText">Replacement Text.</param>
        /// <param name="color">Color for the replacement text.</param>
        public void ReplaceRegex(String regex, String ReplacementText, Color color)
        {
            this.ReplaceRegex(regex, ReplacementText, DefaultFont, color);
        }

        /// <summary>
        /// Uses <see cref="System.Text.RegularExpressions">Regular Expressions</see>
        /// to find and replace text in the operator prompt.
        /// The new text will use the
        /// specified Font and
        /// <see cref="DefaultColor">Default Color</see>.
        /// </summary>
        /// <param name="regex">Regex string to use for the search.</param>
        /// <param name="ReplacementText">Replacement Text.</param>
        /// <param name="font">Font to be used for the replacement text.</param>
        public void ReplaceRegex(String regex, String ReplacementText, Font font)
        {
            this.ReplaceRegex(regex, ReplacementText, font, DefaultColor);
        }

        /// <summary>
        /// Uses <see cref="System.Text.RegularExpressions">Regular Expressions</see>
        /// to find and replace text in the operator prompt.
        /// The new text will use the
        /// specified Font and Color.
        /// </summary>
        /// <param name="regex">Regex string to use for the search.</param>
        /// <param name="ReplacementText">Replacement Text.</param>
        /// <param name="font">Font to be used for the replacement text.</param>
        /// <param name="color">Color to be used for the replacement text.</param>
        public void ReplaceRegex(String regex, String ReplacementText, Font font, Color color)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(_ReplaceRegexCallback, regex, ReplacementText, font, color);
            }
            else
            {
                this.SuspendLayout();
                this.ReadOnly = false;
                System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex(regex);
                System.Text.RegularExpressions.Match m = r.Match(this.Text);
                while (m.Success)
                {
                    this.Select(m.Index, m.Length);
                    this.SelectionFont = font;
                    this.SelectionColor = color;
                    this.SelectedText = ReplacementText;
                    if (m.Index + ReplacementText.Length >= this.Text.Length) break;
                    if (m.Length == 0) break;
                    m = r.Match(this.Text, m.Index + ReplacementText.Length);
                }
                this.ReadOnly = true;
                this.ResumeLayout();
            }
        }

        /// <summary>
        /// Uses <see cref="System.Text.RegularExpressions">Regular Expressions</see>
        /// to find and replace all instances of text matching the regex in the operator prompt.
        /// The new text will use the
        /// <see cref="DefaultFont">Default Font</see> and
        /// <see cref="DefaultColor">Default Color</see>.
        /// </summary>
        /// <param name="regex">Regex string to use for the search.</param>
        /// <param name="ReplacementText">Replacement Text.</param>
        public void ReplaceAllRegex(String regex, String ReplacementText)
        {
            this.ReplaceAllRegex(regex, ReplacementText, DefaultFont, DefaultColor);
        }

        /// <summary>
        /// Uses <see cref="System.Text.RegularExpressions">Regular Expressions</see>
        /// to find and replace all instances of text matching the regex in the operator prompt.
        /// The new text will use the
        /// <see cref="DefaultFont">Default Font</see> and
        /// the specified color.
        /// </summary>
        /// <param name="regex">Regex string to use for the search.</param>
        /// <param name="ReplacementText">Replacement Text.</param>
        /// <param name="color">Color for the replacement text.</param>
        public void ReplaceAllRegex(String regex, String ReplacementText, Color color)
        {
            this.ReplaceAllRegex(regex, ReplacementText, DefaultFont, color);
        }

        /// <summary>
        /// Uses <see cref="System.Text.RegularExpressions">Regular Expressions</see>
        /// to find and replace all instances of text matching the regex in the operator prompt.
        /// The new text will use the
        /// specified Font and
        /// <see cref="DefaultColor">Default Color</see>.
        /// </summary>
        /// <param name="regex">Regex string to use for the search.</param>
        /// <param name="ReplacementText">Replacement Text.</param>
        /// <param name="font">Font to be used for the replacement text.</param>
        public void ReplaceAllRegex(String regex, String ReplacementText, Font font)
        {
            this.ReplaceAllRegex(regex, ReplacementText, font, DefaultColor);
        }

        /// <summary>
        /// Uses <see cref="System.Text.RegularExpressions">Regular Expressions</see>
        /// to find and replace all instances of text matching the regex in the operator prompt.
        /// The new text will use the
        /// specified Font and Color.
        /// </summary>
        /// <param name="regex">Regex string to use for the search.</param>
        /// <param name="ReplacementText">Replacement Text.</param>
        /// <param name="font">Font to be used for the replacement text.</param>
        /// <param name="color">Color to be used for the replacement text.</param>
        public void ReplaceAllRegex(String regex, String ReplacementText, Font font, Color color)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(_ReplaceAllRegexCallback, regex, ReplacementText, font, color);
            }
            else
            {
                this.SuspendLayout();
                this.ReadOnly = false;
                System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex(regex);
                System.Text.RegularExpressions.Match m = r.Match(this.Text);
                while (m.Success)
                {
                    this.Select(m.Index, m.Length);
                    this.SelectionFont = font;
                    this.SelectionColor = color;
                    this.SelectedText = ReplacementText;
                    if (m.Index + ReplacementText.Length > this.Text.Length) break;
                    m = r.Match(this.Text, m.Index + ReplacementText.Length);
                }
                this.ReadOnly = true;
                this.ResumeLayout();
            }
        }

        /// <summary>
        /// Appends text to the operator prompt. Uses a simple search to
        /// ensure that only one instance of the text is added to the prompt.
        /// The new text will use the
        /// <see cref="DefaultFont">Default Font</see> and
        /// <see cref="DefaultColor">Default Color</see>.
        /// </summary>
        /// <param name="text">Text to be appended to the operator prompt.</param>
        public void AppendTextOnce(String text)
        {
            this.AppendTextOnce(text, DefaultFont, DefaultColor);
        }

        /// <summary>
        /// Appends text to the operator prompt. Uses a simple search to
        /// ensure that only one instance of the text is added to the prompt.
        /// The new text will use the
        /// <see cref="DefaultFont">Default Font</see> and
        /// the specified color.
        /// </summary>
        /// <param name="text">Text to be appended to the operator prompt.</param>
        /// <param name="color">Color for the new text.</param>
        public void AppendTextOnce(String text, Color color)
        {
            this.AppendTextOnce(text, DefaultFont, color);
        }

        /// <summary>
        /// Appends text to the operator prompt. Uses a simple search to
        /// ensure that only one instance of the text is added to the prompt.
        /// The new text will use the
        /// specified Font and
        /// <see cref="DefaultColor">Default Color</see>.
        /// </summary>
        /// <param name="text">Text to be appended to the operator prompt.</param>
        /// <param name="font">Font to be used for the new text.</param>
        public void AppendTextOnce(String text, Font font)
        {
            this.AppendTextOnce(text, font, DefaultColor);
        }

        /// <summary>
        /// Appends text to the operator prompt. Uses a simple search to
        /// ensure that only one instance of the text is added to the prompt.
        /// The new text will use the
        /// specified Font and Color.
        /// </summary>
        /// <param name="text">Text to be appended to the operator prompt.</param>
        /// <param name="font">Font to be used for the new text.</param>
        /// <param name="color">Color for the new text.</param>
        public void AppendTextOnce(String text, Font font, Color color)
        {
            this.AppendTextOnce(text, font, color, @"\s*" + text.Trim());
        }

        /// <summary>
        /// Appends text to the operator prompt. Uses a Regex search to
        /// ensure that only one instance of the text is added to the prompt.
        /// The new text will use the
        /// <see cref="DefaultFont">Default Font</see> and
        /// <see cref="DefaultColor">Default Color</see>.
        /// </summary>
        /// <param name="text">Text to be appended to the operator prompt.</param>
        /// <param name="regexReplace">Regular expression to be used for the search.</param>
        public void AppendTextOnce(String text, String regexReplace)
        {
            this.AppendTextOnce(text, DefaultFont, DefaultColor, regexReplace);
        }

        /// <summary>
        /// Appends text to the operator prompt. Uses a Regex search to
        /// ensure that only one instance of the text is added to the prompt.
        /// The new text will use the
        /// <see cref="DefaultFont">Default Font</see> and
        /// the specified color.
        /// </summary>
        /// <param name="text">Text to be appended to the operator prompt.</param>
        /// <param name="color">Color for the new text.</param>
        /// <param name="regexReplace">Regular expression to be used for the search.</param>
        public void AppendTextOnce(String text, Color color, String regexReplace)
        {
            this.AppendTextOnce(text, DefaultFont, color, regexReplace);
        }

        /// <summary>
        /// Appends text to the operator prompt. Uses a Regex search to
        /// ensure that only one instance of the text is added to the prompt.
        /// The new text will use the
        /// specified Font and
        /// <see cref="DefaultColor">Default Color</see>.
        /// </summary>
        /// <param name="text">Text to be appended to the operator prompt.</param>
        /// <param name="font">Font to be used for the new text.</param>
        /// <param name="regexReplace">Regular expression to be used for the search.</param>
        public void AppendTextOnce(String text, Font font, String regexReplace)
        {
            this.AppendTextOnce(text, font, DefaultColor, regexReplace);
        }

        /// <summary>
        /// Appends text to the operator prompt. Uses a Regex search to
        /// ensure that only one instance of the text is added to the prompt.
        /// The new text will use the
        /// specified Font and Color.
        /// </summary>
        /// <param name="text">Text to be appended to the operator prompt.</param>
        /// <param name="font">Font to be used for the new text.</param>
        /// <param name="color">Color for the new text.</param>
        /// <param name="regexReplace">Regular expression to be used for the search.</param>
        public void AppendTextOnce(String text, Font font, Color color, String regexReplace)
        {
            this.ReplaceAllRegex(regexReplace, string.Empty);
            this.AppendText(text, font, color);
        }

        /// <summary>
        /// Addes text to the top of operator prompt. Uses a simple search to
        /// ensure that only one instance of the text is added to the prompt.
        /// The new text will use the
        /// <see cref="DefaultFont">Default Font</see> and
        /// <see cref="DefaultColor">Default Color</see>.
        /// </summary>
        /// <param name="text">Text to be added to the operator prompt.</param>
        public void PrependTextOnce(String text)
        {
            this.PrependTextOnce(text, DefaultFont, DefaultColor);
        }

        /// <summary>
        /// Addes text to the top of operator prompt. Uses a simple search to
        /// ensure that only one instance of the text is added to the prompt.
        /// The new text will use the
        /// <see cref="DefaultFont">Default Font</see> and
        /// the specified color.
        /// </summary>
        /// <param name="text">Text to be added to the operator prompt.</param>
        /// <param name="color">Color for the new text.</param>
        public void PrependTextOnce(String text, Color color)
        {
            this.PrependTextOnce(text, DefaultFont, color);
        }

        /// <summary>
        /// Addes text to the top of operator prompt. Uses a simple search to
        /// ensure that only one instance of the text is added to the prompt.
        /// The new text will use the
        /// specified Font and
        /// <see cref="DefaultColor">Default Color</see>.
        /// </summary>
        /// <param name="text">Text to be added to the operator prompt.</param>
        /// <param name="font">Font to be used for the new text.</param>
        public void PrependTextOnce(String text, Font font)
        {
            this.PrependTextOnce(text, font, DefaultColor);
        }

        /// <summary>
        /// Addes text to the top of operator prompt. Uses a simple search to
        /// ensure that only one instance of the text is added to the prompt.
        /// The new text will use the
        /// specified Font and Color.
        /// </summary>
        /// <param name="text">Text to be added to the operator prompt.</param>
        /// <param name="font">Font to be used for the new text.</param>
        /// <param name="color">Color for the new text.</param>
        public void PrependTextOnce(String text, Font font, Color color)
        {
            this.PrependTextOnce(text, font, color, @"\s*" + text.Trim() + @"\s*");
        }

        /// <summary>
        /// Addes text to the top of the operator prompt. Uses a Regex search to
        /// ensure that only one instance of the text is added to the prompt.
        /// The new text will use the
        /// <see cref="DefaultFont">Default Font</see> and
        /// <see cref="DefaultColor">Default Color</see>.
        /// </summary>
        /// <param name="text">Text to be added to the operator prompt.</param>
        /// <param name="regexReplace">Regular expression to be used for the search.</param>
        public void PrependTextOnce(String text, String regexReplace)
        {
            this.PrependTextOnce(text, DefaultFont, DefaultColor, regexReplace);
        }

        /// <summary>
        /// Addes text to the top of the operator prompt. Uses a Regex search to
        /// ensure that only one instance of the text is added to the prompt.
        /// The new text will use the
        /// <see cref="DefaultFont">Default Font</see> and
        /// the specified color.
        /// </summary>
        /// <param name="text">Text to be added to the operator prompt.</param>
        /// <param name="color">Color for the new text.</param>
        /// <param name="regexReplace">Regular expression to be used for the search.</param>
        public void PrependTextOnce(String text, Color color, String regexReplace)
        {
            this.PrependTextOnce(text, DefaultFont, color, regexReplace);
        }

        /// <summary>
        /// Addes text to the top of the operator prompt. Uses a Regex search to
        /// ensure that only one instance of the text is added to the prompt.
        /// The new text will use the
        /// specified Font and
        /// <see cref="DefaultColor">Default Color</see>.
        /// </summary>
        /// <param name="text">Text to be added to the operator prompt.</param>
        /// <param name="font">Font to be used for the new text.</param>
        /// <param name="regexReplace">Regular expression to be used for the search.</param>
        public void PrependTextOnce(String text, Font font, String regexReplace)
        {
            this.PrependTextOnce(text, font, DefaultColor, regexReplace);
        }

        /// <summary>
        /// Addes text to the top of the operator prompt. Uses a Regex search to
        /// ensure that only one instance of the text is added to the prompt.
        /// The new text will use the
        /// specified Font and Color.
        /// </summary>
        /// <param name="text">Text to be added to the operator prompt.</param>
        /// <param name="font">Font to be used for the new text.</param>
        /// <param name="color">Color for the new text.</param>
        /// <param name="regexReplace">Regular expression to be used for the search.</param>
        public void PrependTextOnce(String text, Font font, Color color, String regexReplace)
        {
            this.ReplaceAllRegex(regexReplace, string.Empty);
            this.PrependText(text, font, color);
        }

        #endregion Public Members
    }
}