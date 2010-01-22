namespace HistoryFramework
{
    /// <summary>
    /// Encapsulates a task which can be executed and unexecuted
    /// </summary>
    public abstract class Action : IAction
    {
        #region Fields

        private bool _executed = false;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets the name of this IAction instance
        /// </summary>
        public virtual string Name
        {
            get
            {
                return ToString();
            }
        }

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Executes the task if it has not already been executed
        /// </summary>
        /// <returns>true if the task was executed; otherwise false if the task has previously been executed without
        /// unexecuting</returns>
        public bool Execute()
        {
            if (!_executed)
            {
                _executed = true;
                ExecuteTask();
            }

            return _executed;
        }

        /// <summary>
        /// Unexecutes the task if it has been executed
        /// </summary>
        /// <returns>true if the task was unexecuted; otherwise false if the task has not previously been
        /// executed</returns>
        public bool UnExecute()
        {
            if (_executed)
            {
                _executed = false;
                UnExecuteTask();
            }

            return !_executed;
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// Override to implement the task execution behaviour
        /// </summary>
        protected abstract void ExecuteTask();

        /// <summary>
        /// Override to implement the task unexecution behaviour
        /// </summary>
        protected abstract void UnExecuteTask();

        #endregion Protected Methods

        #endregion Methods
    }
}