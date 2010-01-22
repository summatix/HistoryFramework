namespace HistoryFramework
{
    using System;

    /// <summary>
    /// Provides an interface to control execution of actions
    /// </summary>
    public interface IHistory
    {
        #region Properties

        /// <summary>
        /// Gets whether it is possible to undo the previous action
        /// </summary>
        bool CanRedo
        {
            get;
        }

        /// <summary>
        /// Gets whether it is possible to redo the previous undone action
        /// </summary>
        bool CanUndo
        {
            get;
        }

        /// <summary>
        /// Gets the last undone action, or null if no actions have been undone
        /// </summary>
        IAction RedoAction
        {
            get;
        }

        /// <summary>
        /// Gets the number of undone actions
        /// </summary>
        int RedoStackCount
        {
            get;
        }

        /// <summary>
        /// Gets or sets the number of actions this IHistoty instance can hold
        /// </summary>
        int Size
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the last executed action, or null if no actions have been executed
        /// </summary>
        IAction UndoAction
        {
            get;
        }

        /// <summary>
        /// Gets the number executed actions
        /// </summary>
        int UndoStackCount
        {
            get;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Clears the undo and redo stacks
        /// </summary>
        void Clear();

        /// <summary>
        /// Records and executes the given IAction, and executes any existing unexecuted actions
        /// </summary>
        /// <exception cref="ArgumentNullException">action is null</exception>
        /// <param name="action">The IAction instance to record and execute</param>
        void ExecuteAndRecord(IAction action);

        /// <summary>
        /// Executes the IAction instance provided to the last call of RecordAction(IAction) method
        /// </summary>
        /// <returns>true if the action was executed; otherwise false if no actions exist which have not been
        /// executed</returns>
        bool ExecutedLastRecordedAction();

        /// <summary>
        /// Records the given IAction instance, and executes any existing unexecuted actions
        /// </summary>
        /// <exception cref="ArgumentNullException">action is null</exception>
        /// <param name="action">The IAction instance to record</param>
        void RecordAction(IAction action);

        /// <summary>
        /// Executes the last IAction instance which was unexecuted
        /// </summary>
        /// <returns>true if an action was executed; otherwise false if there are no actions to execute</returns>
        bool Redo();

        /// <summary>
        /// Unexecutes the last IAction instance which was executed
        /// </summary>
        /// <returns>true if an action was unexecuted; otherwise false if there are no actions to unexecute</returns>
        bool Undo();

        #endregion Methods
    }
}