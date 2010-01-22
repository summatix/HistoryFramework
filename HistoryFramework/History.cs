namespace HistoryFramework
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// Provides an interface to control execution of actions
    /// </summary>
    public class History : IHistory
    {
        #region Fields

        private const int DEFAULT_STACK_SIZE = 25;

        private IAction _lastRecorded;
        private SizedStack<IAction> _redoStack;
        private int _size;
        private SizedStack<IAction> _undoStack;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes the instance with the default size
        /// </summary>
        public History()
            : this(DEFAULT_STACK_SIZE)
        {
        }

        /// <summary>
        /// Initializes the instance with the given size
        /// </summary>
        /// <exception cref="ArgumentException">size is smaller than 1</exception>
        /// <param name="size">The number of actions to record</param>
        public History(int size)
        {
            Size = size;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets whether it is possible to undo the previous action
        /// </summary>
        public virtual bool CanRedo
        {
            get { return _redoStack.Count > 0; }
        }

        /// <summary>
        /// Gets whether it is possible to redo the previous undone action
        /// </summary>
        public virtual bool CanUndo
        {
            get { return _undoStack.Count > 0; }
        }

        /// <summary>
        /// Gets the last undone action, or null if no actions have been undone
        /// </summary>
        public IAction RedoAction
        {
            get
            {
                IAction action;
                _redoStack.TryPeek(out action);
                return action;
            }
        }

        /// <summary>
        /// Gets the number of undone actions
        /// </summary>
        public int RedoStackCount
        {
            get { return _redoStack.Count; }
        }

        /// <summary>
        /// Gets or sets the number of actions this Histoty instance can hold. When set all current undo and redo
        /// history is cleared
        /// </summary>
        /// <exception cref="ArgumentException">If the new value is smaller than 1</exception>
        [DefaultValue(25)]
        public int Size
        {
            get { return _size; }
            set
            {
                if (value < 1)
                {
                    throw new ArgumentException("size must be larger than 0");
                }

                _size = value;
                _undoStack = new SizedStack<IAction>(_size);
                _redoStack = new SizedStack<IAction>(_size);
            }
        }

        /// <summary>
        /// Gets the last executed action, or null if no actions have been executed
        /// </summary>
        public IAction UndoAction
        {
            get
            {
                IAction action;
                _undoStack.TryPeek(out action);
                return action;
            }
        }

        /// <summary>
        /// Gets the number executed actions
        /// </summary>
        public int UndoStackCount
        {
            get { return _undoStack.Count; }
        }

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Clears the undo and redo stacks
        /// </summary>
        public void Clear()
        {
            _undoStack.Clear();
            _redoStack.Clear();
        }

        /// <summary>
        /// Records and executes the given IAction, and executes any existing unexecuted actions
        /// </summary>
        /// <exception cref="ArgumentNullException">action is null</exception>
        /// <param name="action">The IAction instance to record and execute</param>
        public void ExecuteAndRecord(IAction action)
        {
            if (action == null)
            {
                throw new ArgumentNullException();
            }

            lock (this)
            {
                if (_lastRecorded != null)
                {
                    Execute();
                    _lastRecorded = null;
                }

                action.Execute();
                _undoStack.Push(action);
            }
        }

        /// <summary>
        /// Executes the IAction instance provided to the last call of RecordAction(IAction) method
        /// </summary>
        /// <returns>true if the action was executed; otherwise false if no actions exist which have not been
        /// executed</returns>
        public virtual bool ExecutedLastRecordedAction()
        {
            lock (this)
            {
                if (_lastRecorded != null)
                {
                    Execute();
                    _lastRecorded = null;

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Records the given IAction instance, and executes any existing unexecuted actions
        /// </summary>
        /// <exception cref="ArgumentNullException">action is null</exception>
        /// <param name="action">The IAction instance to record</param>
        public virtual void RecordAction(IAction action)
        {
            if (action == null)
            {
                throw new ArgumentNullException();
            }

            lock (this)
            {
                if (_lastRecorded != null)
                {
                    Execute();
                }

                _lastRecorded = action;
            }
        }

        /// <summary>
        /// Executes the last IAction instance which was unexecuted
        /// </summary>
        /// <returns>true if an action was executed; otherwise false if there are no actions to execute</returns>
        public virtual bool Redo()
        {
            bool toReturn = false;

            lock (this)
            {
                if (CanRedo)
                {
                    IAction action = _undoStack.Pop();
                    toReturn = action.Execute();
                    _undoStack.Push(action);
                }
            }

            return toReturn;
        }

        /// <summary>
        /// Unexecutes the last IAction instance which was executed
        /// </summary>
        /// <returns>true if an action was unexecuted; otherwise false if there are no actions to unexecute</returns>
        public virtual bool Undo()
        {
            bool toReturn = false;

            lock (this)
            {
                if (CanUndo)
                {
                    IAction action = _undoStack.Pop();
                    toReturn = action.UnExecute();
                    _redoStack.Push(action);
                }
            }

            return toReturn;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Executes the last recorded IAction instance
        /// </summary>
        private void Execute()
        {
            _undoStack.Push(_lastRecorded);
            _lastRecorded.Execute();
            _redoStack.Clear();
        }

        #endregion Private Methods

        #endregion Methods
    }
}