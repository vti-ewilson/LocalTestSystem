using System;
using System.Data.Linq;
using System.Linq;
using System.Windows.Forms;
using VTIWindowsControlLibrary.Data;
using VTIWindowsControlLibrary.Forms;

namespace VTIWindowsControlLibrary.Classes.ClientForms
{
    /// <summary>
    /// Represents the <see cref="SelectModelForm">Select Model Form</see> of the client application
    /// </summary>
    /// <remarks>
    /// This class isn't the Select Model Form itself, but contains a
    /// private static instance of the
    /// <see cref="TouchScreenButtonForm">TouchScreenButtonForm</see>,
    /// and has static methods for accessing it.
    /// </remarks>
    public class SelectModelForm
    {
        private static TouchScreenButtonForm touchScreenButtonForm;
        private static Table<Model> Models;
        private static Table<VTIWindowsControlLibrary.Data.VtiDataContext2.Model> Models2;
		private static ModelSettingsBase _CurrentModel;
        private static ModelSettingsBase _DefaultModel;

        /// <summary>
        /// Initializes the static instance of the <see cref="SelectModelForm">SelectModelForm</see>
        /// </summary>
        static SelectModelForm()
        {
            touchScreenButtonForm = new TouchScreenButtonForm();
            touchScreenButtonForm.UpdateList += new EventHandler(touchScreenButtonForm_UpdateList);
            touchScreenButtonForm.ButtonClicked += new TouchScreenButtonForm.TouchScreenButtonClickedEventHandler(touchScreenButtonForm_ButtonClicked);
            touchScreenButtonForm.Text = VtiLibLocalization.SelectModel;
            touchScreenButtonForm.TopMost = true;
            if(VtiLib.UseRemoteModelDB)
            {
				Models2 = VtiLib.Data3.GetTable<VTIWindowsControlLibrary.Data.VtiDataContext2.Model>();
			}
			else
            {
				Models = VtiLib.Data.GetTable<Model>();
			}
		}

        /// <summary>
        /// Shows the Select Model Form
        /// </summary>
        public static void Show(ModelSettingsBase CurrentModel, ModelSettingsBase DefaultModel)
        {
            Show(null, CurrentModel, DefaultModel);
        }

        /// <summary>
        /// Hide the Select Model Form
        /// </summary>
        public static void Hide()
        {
            touchScreenButtonForm.Hide();
        }

        /// <summary>
        /// Indicates whether the Select Model form is visible
        /// </summary>
        public static bool Visible()
        {
            bool bIsVisible = false;
            if (touchScreenButtonForm != null)
                bIsVisible = touchScreenButtonForm.Visible;
            return bIsVisible;
        }

        /// <summary>
        /// Shows the Select Model Form
        /// </summary>
        /// <param name="MdiParent">Form that will own the Select Model Form</param>
        /// <param name="CurrentModel">Reference to the current model for the cycle.</param>
        /// <param name="DefaultModel">Reference to the default model for the system.</param>
        public static void Show(Form MdiParent, ModelSettingsBase CurrentModel, ModelSettingsBase DefaultModel)
        {
            if (MdiParent != null) touchScreenButtonForm.MdiParent = MdiParent;
            UpdateList();
            _CurrentModel = CurrentModel;
            _DefaultModel = DefaultModel;
            touchScreenButtonForm.Show();
            touchScreenButtonForm.BringToFront();
        }

        private static void touchScreenButtonForm_ButtonClicked(object sender, TouchScreenButtonForm.TouchScreenButtonClickedEventArgs e)
        {
            //Object[] parameters = new object[1];
            //parameters[0] = e.TouchScreenCommand.Name;
            //VtiLib.Config.GetField("CurrentModel").FieldType.GetMethod("Load").Invoke(VtiLib.Config.GetField("CurrentModel").GetValue(null), parameters);
            if (e.TouchScreenCommand.Name == "DEFAULT")
                _CurrentModel.LoadFrom(_DefaultModel);
            else
                _CurrentModel.Load(e.TouchScreenCommand.Name);
            touchScreenButtonForm.Hide();
        }

        private static void touchScreenButtonForm_UpdateList(object sender, EventArgs e)
        {
            UpdateList();
        }

        private static void UpdateList()
        {
            TouchScreenButtonForm.TouchScreenCommand command;

            touchScreenButtonForm.CommandList.Clear();

            command = new TouchScreenButtonForm.TouchScreenCommand();
            command.Name = "DEFAULT";
            command.Text = VtiLib.Localization.GetString("Default") ?? "DEFAULT";
            touchScreenButtonForm.CommandList.Add(command);

            //foreach (Model model in Models)
            //{
            //    command = new TouchScreenButtonForm.TouchScreenCommand();
            //    command.Name = model.ModelNo;
            //    command.Text = model.ModelNo;
            //    touchScreenButtonForm.CommandList.Add(command);
            //}

            if(VtiLib.UseRemoteModelDB)
            {
				touchScreenButtonForm.CommandList.AddRange(Models2.Select(m =>
				new TouchScreenButtonForm.TouchScreenCommand
				{
					Name = m.ModelNo,
					Text = m.ModelNo
				}));
			}
            else
            {
				touchScreenButtonForm.CommandList.AddRange(Models.Select(m =>
				new TouchScreenButtonForm.TouchScreenCommand
				{
					Name = m.ModelNo,
					Text = m.ModelNo
				}));
			}
            
        }
    }
}