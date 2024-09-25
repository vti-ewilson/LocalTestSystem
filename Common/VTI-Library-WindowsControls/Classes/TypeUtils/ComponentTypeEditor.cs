using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.Windows.Forms;
using System.ComponentModel;

namespace VTIWindowsControlLibrary.Classes.TypeUtils
{
    public class ComponentTypeEditor<T> : UITypeEditor
        where T : Component 
    {
        private IWindowsFormsEditorService editorService;
        private UserControl owner;
        private T selected = null;
        private String selectedName = string.Empty;

        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if ((context != null) && (provider != null))
            {
                // Access the Property Browser's UI display service
                editorService =
                  (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                owner = context.Instance as UserControl;

                if (editorService != null && owner != null && owner.ParentForm != null)
                {
                    // Create an instance of the UI editor control
                    ListBox list = new ListBox();
                    list.Dock = DockStyle.Fill;
                    list.BorderStyle = BorderStyle.None;
                    list.IntegralHeight = false;
                    list.SelectedIndexChanged += new EventHandler(list_SelectedIndexChanged);

                    List<string> strings = new List<string>();

                    foreach (Component comp in owner.ParentForm.Container.Components)
                        if (comp.Site.Component is T)
                        {
                            strings.Add(comp.Site.Name);
                            if (value != null && comp.Site.Component == value)
                            {
                                selectedName = comp.Site.Name;
                            }
                        }

                    strings.Sort();
                    list.Items.AddRange(strings.ToArray());
                    if (!string.IsNullOrEmpty(selectedName))
                        list.SelectedIndex = list.Items.IndexOf(selectedName);

                    // Display the UI editor control
                    editorService.DropDownControl(list);

                    // Return the new property value from the UI editor control
                    if (list.SelectedIndex > -1)
                    {
                        foreach (Component comp in owner.ParentForm.Container.Components)
                            if (comp.Site.Name == (string)list.Items[list.SelectedIndex])
                                selected = comp.Site.Component as T;
                    }
                    return selected;
                }
            }
            return base.EditValue(context, provider, value);
        }

        void list_SelectedIndexChanged(object sender, EventArgs e)
        {
            editorService.CloseDropDown();
        }
    }
}
