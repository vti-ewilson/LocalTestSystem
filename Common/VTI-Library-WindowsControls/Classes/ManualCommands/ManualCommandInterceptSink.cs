using System.Runtime.Remoting.Messaging;

namespace VTIWindowsControlLibrary.Classes.ManualCommands
{
    /// <summary>
    /// ManualCommandInterceptSink
    ///
    /// Okay, we finally get to the business end of the stick.
    ///
    /// This class implements the IMessageSink interface, in order to process windows messages.
    /// When a message comes through the SyncProcessMessage method, we check to see
    /// if the method has a ManualCommand attribute applied, and, if so, we proceed to
    /// check for user security, etc, to see if the method should be allows to execute.
    /// </summary>
    public class ManualCommandInterceptSink : IMessageSink
    {
        #region Fields (1)

        #region Private Fields (1)

        private IMessageSink nextSink;

        #endregion Private Fields

        #endregion Fields

        #region Constructors (1)

        //private Object Config;
        /// <summary>
        /// Initializes a new instance of the <see cref="ManualCommandInterceptSink">ManualCommandInterceptSink</see> class
        /// </summary>
        /// <param name="nextSink">The next message sink in the queue.</param>
        public ManualCommandInterceptSink(IMessageSink nextSink)
        {
            // Daisy-chain to the next message sink
            this.nextSink = nextSink;
        }

        #endregion Constructors

        #region Properties (1)

        /// <summary>
        /// Gets the next message sink in the queue
        /// </summary>
        public IMessageSink NextSink
        {
            get
            {
                return this.nextSink;
            }
        }

        #endregion Properties

        #region Methods (2)

        #region Public Methods (2)

        /// <summary>
        /// Asynchronously processes the given message
        /// </summary>
        /// <param name="msg">The message to process.</param>
        /// <param name="replySink">The reply sink for the reply message.</param>
        /// <returns>A reply message in response to the request.</returns>
        public IMessageCtrl AsyncProcessMessage(IMessage msg, IMessageSink replySink)
        {
            IMessageCtrl rtnMsgCtrl = nextSink.AsyncProcessMessage(msg, replySink);
            return rtnMsgCtrl;
        }

        /// <summary>
        /// Synchronously processes the given message
        /// </summary>
        /// <param name="msg">The message to process</param>
        /// <returns>A reply message in response to the request.</returns>
        public IMessage SyncProcessMessage(IMessage msg)
        {
            IMessage rtnMsg = null;
            IMethodReturnMessage mrm;
            IMethodCallMessage mcm = (msg as IMethodCallMessage);

            //if (ManualCommandsBase.CheckMethodPermission(mcm.MethodBase))
            if (mcm.MethodBase.HasPermission())
                rtnMsg = nextSink.SyncProcessMessage(msg);
            else
                rtnMsg = new ReturnMessage(null, mcm);

            mrm = (rtnMsg as IMethodReturnMessage);
            return mrm;
        }

        #endregion Public Methods

        #endregion Methods
    }
}