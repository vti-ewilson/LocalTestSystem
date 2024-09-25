using System;
using System.Runtime.Remoting.Contexts;

namespace VTIWindowsControlLibrary.Classes.ManualCommands
{
    /// <summary>
    /// When added to the ContextBoundObject via the ManualCommandInterceptAttribute,
    /// the ManualCommandInterceptProperty inserts the ManualCommandInterceptSink into the
    /// windows message queue, allowing us to intercept calls to methods within the
    /// ContextBoundObject (i.e., the ManualCommands class)
    /// </summary>
    public class ManualCommandInterceptProperty : IContextProperty, IContributeObjectSink
    {
        #region Constructors (1)

        /// <summary>
        /// Initializes a new instance of the <see cref="ManualCommandInterceptProperty">ManualCommandInterceptProperty</see>
        /// </summary>
        public ManualCommandInterceptProperty()
            : base()
        {
        }

        #endregion Constructors

        #region Properties (1)

        /// <summary>
        /// Gets the Name for the <see cref="ManualCommandInterceptProperty">ManualCommandInterceptProperty</see>
        /// </summary>
        public string Name
        {
            get
            {
                return "ManualCommandIntercept";
            }
        }

        #endregion Properties

        #region Methods (3)

        #region Public Methods (3)

        /// <summary>
        /// Called when the context is frozen.
        /// </summary>
        /// <param name="newContext">The context to freeze</param>
        public void Freeze(Context newContext)
        {
        }

        /// <summary>
        /// Insert a message sink into the windows message queue
        /// </summary>
        /// <param name="obj">Object</param>
        /// <param name="nextSink">Next message sink in the queue</param>
        /// <returns>A new <see cref="ManualCommandInterceptSink">ManualCommandInterceptSink</see></returns>
        public System.Runtime.Remoting.Messaging.IMessageSink GetObjectSink(MarshalByRefObject obj, System.Runtime.Remoting.Messaging.IMessageSink nextSink)
        {
            return new ManualCommandInterceptSink(nextSink);
        }

        /// <summary>
        /// Returns a Boolean value indicating whether the context property is compatible
        /// with the new context.
        /// </summary>
        /// <param name="newCtx">The new context in which the property has been created.</param>
        /// <returns>true if the context property is okay with the new context; otherwise, false.</returns>
        public bool IsNewContextOK(Context newCtx)
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