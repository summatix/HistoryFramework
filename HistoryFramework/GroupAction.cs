namespace HistoryFramework
{
    /// <summary>
    /// Encapsulates a group of tasks which can be executed and unexecuted, where only the first item in the collection
    /// is executed, and only the last item in the collection is unexecuted
    /// </summary>
    public sealed class GroupAction : IAction
    {
        #region Fields

        private bool _executed = false;
        private IAction _firstAction = null;
        private IAction _lastAction = null;
        private string _name = string.Empty;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets the name of this GroupAction instance
        /// </summary>
        public string Name
        {
            get
            {
                if (_name == string.Empty && _firstAction != null)
                {
                    return _firstAction.Name;
                }

                return _name;
            }
            set
            {
                _name = value != null ? value : string.Empty;
            }
        }

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Adds a new IAction instance to the colletion of actions to execute and unexecute
        /// </summary>
        /// <param name="action">The IAction instance to add</param>
        public void AddAction(IAction action)
        {
            if (_firstAction == null)
            {
                _firstAction = action;
            }
            else
            {
                _lastAction = action;
            }
        }

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

                if (_firstAction == null)
                {
                    return false;
                }

                _firstAction.Execute();
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

                if (_lastAction == null)
                {
                    if (_firstAction == null)
                    {
                        return false;
                    }

                    _firstAction.UnExecute();
                }
                else
                {
                    _lastAction.UnExecute();
                }
            }

            return !_executed;
        }

        #endregion Public Methods

        #endregion Methods
    }
}