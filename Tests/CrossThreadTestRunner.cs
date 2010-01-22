namespace Tests
{
    using System;
    using System.Reflection;
    using System.Security.Permissions;
    using System.Threading;

    public class CrossThreadTestRunner
    {
        #region Fields

        private Exception _lastException;
        private ThreadStart _userDelegate;

        #endregion Fields

        #region Constructors

        public CrossThreadTestRunner(ThreadStart userDelegate)
        {
            _userDelegate = userDelegate;
        }

        #endregion Constructors

        #region Methods

        #region Public Methods

        public void Run()
        {
            Thread t = new Thread(MultiThreadedWorker);

            t.Start();
            t.Join();

            if (_lastException != null)
            {
                ThrowExceptionPreservingStack(_lastException);
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void MultiThreadedWorker()
        {
            try
            {
                _userDelegate.Invoke();
            }
            catch (Exception e)
            {
                _lastException = e;
            }
        }

        [ReflectionPermission(SecurityAction.Demand)]
        private void ThrowExceptionPreservingStack(Exception exception)
        {
            FieldInfo remoteStackTraceString = typeof(Exception).GetField("_remoteStackTraceString",
                BindingFlags.Instance | BindingFlags.NonPublic);
            remoteStackTraceString.SetValue(exception, exception.StackTrace + Environment.NewLine);
            throw exception;
        }

        #endregion Private Methods

        #endregion Methods
    }
}