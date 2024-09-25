using System;

namespace VTIWindowsControlLibrary.Classes
{
    /// <summary>
    /// An abstract class which when inherited by another class turns the
    /// sub-class into a singleton implementation with a default static instance.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class GenericSingleton<T>
      where T : GenericSingleton<T>
    {
        private static volatile T _instance;

        /// <summary>
        /// Gets or sets the instance.
        /// </summary>
        /// <value>The instance.</value>
        protected static T Instance
        {
            get { return _instance; }
            set
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = value;
                            _instance.InitializeInstance();
                        }
                    }
                }
            }
        }

        private static object syncRoot = new Object();

        /// <summary>
        /// Initializes the instance.
        /// </summary>
        protected abstract void InitializeInstance();
    }
}