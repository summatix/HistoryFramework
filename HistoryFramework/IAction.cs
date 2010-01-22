namespace HistoryFramework
{
    /// <summary>
    /// Encapsulates a task which can be executed and unexecuted
    /// </summary>
    public interface IAction
    {
        #region Properties

        /// <summary>
        /// Gets the name of this IAction instance
        /// </summary>
        string Name
        {
            get;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Executes the task if it has not already been executed
        /// </summary>
        /// <returns>true if the task was executed; otherwise false if the task has previously been executed without
        /// unexecuting</returns>
        bool Execute();

        /// <summary>
        /// Unexecutes the task if it has been executed
        /// </summary>
        /// <returns>true if the task was unexecuted; otherwise false if the task has not previously been
        /// executed</returns>
        bool UnExecute();

        #endregion Methods
    }
}