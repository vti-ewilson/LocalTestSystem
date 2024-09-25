using System;
using System.Reflection;
using System.Windows.Forms;
using VTIWindowsControlLibrary.Enums;

namespace VTIWindowsControlLibrary.Classes.ManualCommands
{
    internal static class ManualCommandExtensions
    {
        #region Methods (2)

        #region Public Methods (1)

        public static void InvokeWithPermission(this MethodInfo method)
        {
            VtiLib.overrideUser = null;
            if (method.HasPermission())
            {
                method.Invoke(VtiLib.ManualCommands, null);
            }
        }

        #endregion Public Methods
        #region Internal Methods (1)

        internal static Boolean HasPermission(this MethodBase method)
        {
            // Get the current operator ID from the Config class from the host application, using reflection
            string OpID = VtiLib.Config._OpID;

            // Get any/all ManualCommand attributes
            ManualCommandAttribute[] attributes = (ManualCommandAttribute[])method.GetCustomAttributes(typeof(ManualCommandAttribute), false);

            // execute any method without the "ManualCommand" attribute
            if (attributes.Length == 0)
                return true;
            // if method has any ManualCommand attributes, check to see if the method should be executed
            else
            {
                // Log an event that the Manual Command has been selected
                VtiEvent.Log.WriteWarning(attributes[0].CommandText, VtiEventCatType.Manual_Command);

                // if ManualCommand doesn't require a login, pass the message to the next sink (thus allowing it to execute)
                if ((attributes[0].Permission == CommandPermissionType.None))
                    return true;
                else
                {
                    // if ManualCommand requires a login, check for permission
                    if (OpID != String.Empty)
                    {
                        // if any logged in user will do,  pass the message to the next sink
                        if (attributes[0].Permission == CommandPermissionType.AnyLoggedInUser)
                            return true;
                        // if the ManualCommand requires permission, check the permission and pass the message on if permission is ok
                        else if (VtiLib.Data.CheckCommand(OpID, attributes[0].CommandText, (attributes[0].Permission == CommandPermissionType.CheckPermissionWithWarning)))
                            return true;
                        // Permission not granted, so don't execute the method!
                        // Don't pass on the message, and return a null return message.
                        else
                            //rtnMsg = new ReturnMessage(null, mcm);
                            return false;
                    }
                    // if ManualCommand requires a login and no user is logged, display a message
                    else
                    {
                        //rtnMsg = new ReturnMessage(null, mcm);
                        VtiEvent.Log.WriteWarning(VtiLib.Localization.GetString("OpNotLoggedIn"), VtiEventCatType.Login);
                        MessageBox.Show(VtiLibLocalization.PleaseLogIn, Application.ProductName);
                        return false;
                    }
                }
            }
        }

        #endregion Internal Methods

        #endregion Methods
    }
}