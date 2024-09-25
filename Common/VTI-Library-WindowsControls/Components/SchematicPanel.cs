using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using VTIWindowsControlLibrary.Classes.Util;

namespace VTIWindowsControlLibrary.Components
{
    /// <summary>
    /// Represents a Panel to be used in a Schematic Form.
    /// </summary>
    /// <remarks>
    /// This panel takes care of displaying a Windows Metafile (WMF) background
    /// image, and resizing and relocating checkboxes when the panel is resized.
    /// </remarks>
    public partial class SchematicPanel : Panel
    {
        private Metafile metafile1;
        private Graphics.EnumerateMetafileProc metafileDelegate;
        private Rectangle destRect;
        private string _MetafileName;
        private Boolean initialLayoutComplete;
        private Size originalSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="SchematicPanel">SchematicPanel</see>
        /// </summary>
        public SchematicPanel()
        {
            InitializeComponent();

            this.Paint += new PaintEventHandler(WmfPanel_Paint);
            this.Layout += new LayoutEventHandler(WmfPanel_Layout);
            this.SizeChanged += new EventHandler(WmfPanel_SizeChanged);
        }

        private void WmfPanel_SizeChanged(object sender, EventArgs e)
        {
            //Size newSize;
            //Point newLocation;
            if ((initialLayoutComplete) && (!this.DesignMode))
            {
                Single xfactor = (Single)this.Size.Width / (Single)this.originalSize.Width;
                Single yfactor = (Single)this.Size.Height / (Single)this.originalSize.Height;

                //SchematicCheckBox schematicCheckBox;

                //foreach (Control control in this.Controls)
                //{
                //    if (control is SchematicCheckBox)
                //    {
                //        schematicCheckBox = (SchematicCheckBox)control;

                //        newLocation = new Point((int)(schematicCheckBox.OriginalLocation.X * xfactor),
                //        (int)(schematicCheckBox.OriginalLocation.Y * yfactor));
                //        if (schematicCheckBox.Appearance == Appearance.Button)
                //        {
                //            newSize = new Size((int)(schematicCheckBox.OriginalSize.Width * xfactor),
                //                (int)(schematicCheckBox.OriginalSize.Height * yfactor));

                //            if (schematicCheckBox.OriginalSize.Width == 90 && schematicCheckBox.OriginalSize.Height == 90 &&
                //                newSize.Width >= 90 && newSize.Height >= 90)
                //            {
                //                newLocation = new Point(newLocation.X + (newSize.Width - 90) / 2, newLocation.Y + (newSize.Height - 90) / 2);
                //                newSize = new Size(90, 90);
                //            }

                //            schematicCheckBox.Size = newSize;
                //        }
                //        schematicCheckBox.Location = newLocation;
                //    }
                //}

                Controls.OfType<SchematicCheckBox>().ForEach(c =>
                    {
                        Point newLocation = new Point((int)(c.OriginalLocation.X * xfactor),
                        (int)(c.OriginalLocation.Y * yfactor));
                        if (c.Appearance == Appearance.Button)
                        {
                            Size newSize = new Size((int)(c.OriginalSize.Width * xfactor),
                                (int)(c.OriginalSize.Height * yfactor));

                            if (c.OriginalSize.Width == 90 && c.OriginalSize.Height == 90 &&
                                newSize.Width >= 90 && newSize.Height >= 90)
                            {
                                newLocation = new Point(newLocation.X + (newSize.Width - 90) / 2, newLocation.Y + (newSize.Height - 90) / 2);
                                newSize = new Size(90, 90);
                            }

                            c.Size = newSize;
                        }
                        c.Location = newLocation;
                    });

                Controls.OfType<SchematicLabel>().ForEach(d =>
                {
                    Point newLocation = new Point((int)(d.OriginalLocation.X * xfactor),
                    (int)(d.OriginalLocation.Y * yfactor));
                    //if (d.Appearance == Appearance.Button)
                    //{
                    Size newSize = new Size((int)(d.OriginalSize.Width * xfactor),
                        (int)(d.OriginalSize.Height * yfactor));

                    if (d.OriginalSize.Width == 90 && d.OriginalSize.Height == 90 &&
                        newSize.Width >= 90 && newSize.Height >= 90)
                    {
                        newLocation = new Point(newLocation.X + (newSize.Width - 90) / 2, newLocation.Y + (newSize.Height - 90) / 2);
                        newSize = new Size(90, 90);
                    }

                    d.Size = newSize;
                    //}
                    d.Location = newLocation;
                });
            }
            this.Refresh();
        }

        private void WmfPanel_Layout(object sender, LayoutEventArgs e)
        {
            if ((!this.DesignMode) && (!initialLayoutComplete))
            {
                initialLayoutComplete = true;
                originalSize = Size;
                //SchematicCheckBox schematicCheckBox;

                //foreach (Control control in this.Controls)
                //{
                //    if (control is SchematicCheckBox)
                //    {
                //        schematicCheckBox = (SchematicCheckBox)control;
                //        schematicCheckBox.OriginalLocation = schematicCheckBox.Location;
                //        schematicCheckBox.OriginalSize = schematicCheckBox.Size;
                //        // turn off autosize on buttons, so they will now resize with the panel
                //        if (schematicCheckBox.Appearance == Appearance.Button)
                //            schematicCheckBox.AutoSize = false;
                //    }
                //}

                Controls.OfType<SchematicCheckBox>().ForEach(c =>
                    {
                        c.OriginalLocation = c.Location;
                        c.OriginalSize = c.Size;
                        // turn off autosize on buttons, so they will now resize with the panel
                        if (c.Appearance == Appearance.Button)
                            c.AutoSize = false;
                    });
                Controls.OfType<SchematicLabel>().ForEach(d =>
                    {
                        d.OriginalLocation = d.Location;
                        d.OriginalSize = d.Size;
                        // turn off autosize, so they will now resize with the panel
                        d.AutoSize = false;
                    });
            }
        }

        private void WmfPanel_Paint(object sender, PaintEventArgs e)
        {
            if (metafile1 != null)
            {
                destRect = new Rectangle(0, 0, this.Width, this.Height);
                e.Graphics.EnumerateMetafile(metafile1, destRect, metafileDelegate);
            }
        }

        /// <summary>
        /// Gets or sets the FileName of the Metafile to be displayed at Design Time
        /// </summary>
        public string MetafileName
        {
            get { return _MetafileName; }
            set
            {
                _MetafileName = value;
                this.SetMetafile();
            }
        }

        /// <summary>
        /// Used to load a Metafile from an embedded resource at Run Time.
        /// </summary>
        /// <param name="Metafile">Metafile from the embedded resource file.</param>
        public void LoadMetafileFromByteArray(byte[] Metafile)
        {
            metafile1 = new Metafile(new MemoryStream(Metafile));
            metafileDelegate = new Graphics.EnumerateMetafileProc(MetafileCallback);
            this.Refresh();
        }

        private void SetMetafile()
        {
            if (_MetafileName != string.Empty && _MetafileName != null)
            {
                try
                {
                    metafile1 = new Metafile(_MetafileName);
                    metafileDelegate = new Graphics.EnumerateMetafileProc(MetafileCallback);
                    this.Refresh();
                }
                catch
                {
                    metafile1 = null;
                    this.Refresh();
                }
            }
            else
            {
                metafile1 = null;
                this.Refresh();
            }
        }

        private bool MetafileCallback(
           EmfPlusRecordType recordType,
           int flags,
           int dataSize,
           IntPtr data,
           PlayRecordCallback callbackData)
        {
            byte[] dataArray = null;
            if (data != IntPtr.Zero)
            {
                // Copy the unmanaged record to a managed byte buffer
                // that can be used by PlayRecord.
                dataArray = new byte[dataSize];
                Marshal.Copy(data, dataArray, 0, dataSize);
            }

            metafile1.PlayRecord(recordType, flags, dataSize, dataArray);

            return true;
        }
    }
}