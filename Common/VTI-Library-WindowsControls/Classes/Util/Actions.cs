using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace VTIWindowsControlLibrary.Classes.Util
{
    /// <summary>
    /// Provides static methods for performing actions with retries and/or timeouts.
    /// </summary>
    public class Actions
    {
        #region Fields (4) 

        #region Private Fields (4) 

        private static Queue<Action> queue;
        private static bool stopQueue = false;
        private static Thread thrdQueue;
        private static EventWaitHandle waitQueue;

        #endregion Private Fields 

        #endregion Fields 

        #region Constructors (1) 

        static Actions()
        {
            waitQueue = new EventWaitHandle(false, EventResetMode.AutoReset);
            queue = new Queue<Action>();
            thrdQueue = new Thread(new ThreadStart(ProcessQueue));
            thrdQueue.Start();
            thrdQueue.Name = "Actions Queue";
            thrdQueue.IsBackground = true;
        }

        #endregion Constructors 

        #region Methods (7) 

        #region Public Methods (6) 

        /// <summary>
        /// Places the <c>Action</c> delegate on a queue to be processed on
        /// a thread seperate from the calling thread.
        /// </summary>
        /// <param name="Action"></param>
        public static void QueueAction(Action Action)
        {
            queue.Enqueue(Action);
            waitQueue.Set();
        }

        /// <summary>
        /// Retry the action with a given number of retries.  If the action throws an exception,
        /// it will trigger a retry.
        /// </summary>
        /// <param name="Retries">Number of retries</param>
        /// <param name="Action">Action to be performed</param>
        public static void Retry(int Retries, Action Action)
        {
            Retry(Retries, Action, null);
        }

        /// <summary>
        /// Retry the action with a given number of retries.  If the action throws an exception,
        /// it will delay a given number of milliseconds and then trigger a retry.
        /// </summary>
        /// <param name="Retries">Number of retries</param>
        /// <param name="Action">Action to be performed</param>
        /// <param name="DelayBetweenRetries">The number of milliseconds to wait between retries.</param>
        public static void Retry(int Retries, Action Action, int DelayBetweenRetries)
        {
            Retry(Retries, Action, delegate { Thread.Sleep(DelayBetweenRetries); });
        }

        /// <summary>
        /// Retry the action with a given number of retries and a specified action to
        /// be performed between retries.  If the primary action throws an exception,
        /// the ActionBetweenRetries will be invoked prior to retrying the action.
        /// </summary>
        /// <param name="Retries">Number of retries</param>
        /// <param name="Action">Action to be performed</param>
        /// <param name="ActionBetweenRetries">Action to be performed between retries</param>
        public static void Retry(int Retries, Action Action, Action ActionBetweenRetries)
        {
            int retries;
            bool retry;
            retries = 0;
            Exception innerException = null;
            do
            {
                retry = false;
                try
                {
                    Action.Invoke();
                }
                catch (Exception e)
                {
                    retry = true;
                    retries++;
                    innerException = e;
                    if (ActionBetweenRetries != null)
                        ActionBetweenRetries.Invoke();
                }
            } while (retry && retries < Retries);
            if (retries == Retries)
            {
                throw new Exception("Maximum number of retries exceeded", innerException);
            }
        }

        /// <summary>
        /// Stops the queue processing thread.
        /// </summary>
        public static void StopQueue()
        {
            stopQueue = true;
            waitQueue.Set();
        }

        /// <summary>
        /// Wait up to the timeout for a function to return a true value
        /// </summary>
        /// <param name="Timeout">Timeout in milliseconds</param>
        /// <param name="Function">Function to wait for</param>
        public static void WaitForUpTo(long Timeout, Func<bool> Function)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            do
            {
                Thread.Sleep(1);
                Application.DoEvents();
            } while (!Function() && sw.ElapsedMilliseconds < Timeout);
            if (sw.ElapsedMilliseconds >= Timeout)
            {
                throw new TimeoutException();
            }
        }

        #endregion Public Methods 
        #region Private Methods (1) 

        private static void ProcessQueue()
        {
            Action action;
            while (!stopQueue)
            {
                while (queue.Count > 0)
                {
                    action = queue.Dequeue();
                    if (action != null) action.Invoke();
                }
                waitQueue.WaitOne();
            }
        }

        #endregion Private Methods 

        #endregion Methods 
    }
}