using System;
using System.Runtime.Remoting.Contexts;

namespace VTIWindowsControlLibrary.Classes.ManualCommands
{
    /// <summary>
    /// Adds the ManualCommandInterceptProperty to the ContextBoundObject
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ManualCommandInterceptAttribute : ContextAttribute
    {
        #region Constructors (1)

        /// <summary>
        /// Initializes a new instance of the <see cref="ManualCommandInterceptAttribute">ManualCommandInterceptAttribute</see> class
        /// </summary>
        public ManualCommandInterceptAttribute()
            : base("ManualCommandIntercept")
        {
        }

        #endregion Constructors

        #region Methods (4)

        #region Public Methods (4)

        /// <summary>
        /// Called when the context is frozen.
        /// </summary>
        /// <param name="newContext">The context to freeze</param>
        public override void Freeze(Context newContext)
        {
        }

        /// <summary>
        /// Adds the current context message to a given property
        /// </summary>
        /// <param name="ctorMsg">The
        /// <see cref="System.Runtime.Remoting.Activation.IConstructionCallMessage">System.Runtime.Remoting.Activation.IConstructionCallMessage</see>
        /// to which to add the context property</param>
        public override void GetPropertiesForNewContext(System.Runtime.Remoting.Activation.IConstructionCallMessage ctorMsg)
        {
            ctorMsg.ContextProperties.Add(new ManualCommandInterceptProperty());
        }

        /// <summary>
        /// Returns a Boolean value indicating whether the context parameter meets the
        /// context attribute's requirements
        /// </summary>
        /// <param name="ctx">The context in which to check.</param>
        /// <param name="ctorMsg">The
        /// <see cref="System.Runtime.Remoting.Activation.IConstructionCallMessage">System.Runtime.Remoting.Activation.IConstructionCallMessage</see>
        /// to which to add the context property.</param>
        /// <returns>true if the passed in context is okay; otherwise, false.</returns>
        public override bool IsContextOK(Context ctx, System.Runtime.Remoting.Activation.IConstructionCallMessage ctorMsg)
        {
            ManualCommandInterceptProperty p = ctx.GetProperty("ManualCommandIntercept") as ManualCommandInterceptProperty;
            if (p == null)
                return false;
            return true;
        }

        /// <summary>
        /// Returns a Boolean value indicating whether the context property is compatible
        /// with the new context.
        /// </summary>
        /// <param name="newCtx">The new context in which the property has been created.</param>
        /// <returns>true if the context property is okay with the new context; otherwise, false.</returns>
        public override bool IsNewContextOK(Context newCtx)
        {
            ManualCommandInterceptProperty p = newCtx.GetProperty("ManualCommandIntercept") as ManualCommandInterceptProperty;
            if (p == null)
                return false;
            return true;
        }

        #endregion Public Methods

        #endregion Methods
    }
}