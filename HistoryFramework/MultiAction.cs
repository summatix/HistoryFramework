namespace HistoryFramework
{
    using System.Collections.Generic;

    /// <summary>
    /// Encapsulates a collection of tasks which can each be executed and unexecuted
    /// </summary>
    public sealed class MultiAction : IAction
    {
        #region Fields

        private HashSet<IAction> _actions = new HashSet<IAction>();
        private bool _executed = false;
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
                IAction first = _actions.GetEnumerator().Current;
                if (_name == string.Empty && first != null)
                {
                    return first.Name;
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
        /// <returns>true if the IAction was added; otherwise, false if the IAction already exists within the
        /// collection</returns>
        public bool AddAction(IAction action)
        {
            return _actions.Add(action);
        }

        /// <summary>
        /// Executes each task in the collection if they have not already been executed
        /// </summary>
        /// <returns>true if the tasks were executed; otherwise false if the tasks have previously been executed
        /// without unexecuting</returns>
        public bool Execute()
        {
            if (!_executed)
            {
                _executed = true;

                foreach (IAction action in _actions)
                {
                    action.Execute();
                }
            }

            return _executed;
        }

        /// <summary>
        /// Unexecutes each task in the collection if they have been executed
        /// </summary>
        /// <returns>true if the tasks were unexecuted; otherwise false if the tasks have not previously been
        /// executed</returns>
        public bool UnExecute()
        {
            if (_executed)
            {
                _executed = false;

                foreach (IAction action in _actions)
                {
                    action.UnExecute();
                }
            }

            return !_executed;
        }

        #endregion Public Methods

        #endregion Methods
    }
}