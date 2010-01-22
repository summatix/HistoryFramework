namespace HistoryFramework
{
    using System;
    using System.ComponentModel;
    using System.Windows;

    /// <summary>
    /// Provides an interface to control execution of actions, and derives from System.Windows.DependencyObject
    /// </summary>
    public class WpfHistory : DependencyObject, IHistory
    {
        #region Dependency Properties

        private static readonly DependencyPropertyKey UndoActionPropertyKey = 
            DependencyProperty.RegisterReadOnly("UndoAction", typeof(IAction), typeof(WpfHistory),
                new PropertyMetadata());

        private static readonly DependencyPropertyKey UndoActionNamePropertyKey = 
            DependencyProperty.RegisterReadOnly("UndoActionName", typeof(string), typeof(WpfHistory),
                new PropertyMetadata(string.Empty));

        private static readonly DependencyPropertyKey RedoActionPropertyKey = 
            DependencyProperty.RegisterReadOnly("RedoAction", typeof(IAction), typeof(WpfHistory),
                new PropertyMetadata());

        private static readonly DependencyPropertyKey RedoActionNamePropertyKey = 
            DependencyProperty.RegisterReadOnly("RedoActionName", typeof(string), typeof(WpfHistory),
                new PropertyMetadata(string.Empty));

        private static readonly DependencyPropertyKey CanUndoPropertyKey = 
            DependencyProperty.RegisterReadOnly("CanUndo", typeof(bool), typeof(WpfHistory),
                new PropertyMetadata());

        private static readonly DependencyPropertyKey CanRedoPropertyKey = 
            DependencyProperty.RegisterReadOnly("CanRedo", typeof(bool), typeof(WpfHistory),
                new PropertyMetadata());

        /// <summary>
        /// Identifies the CanRedo dependency property
        /// </summary>
        public static readonly DependencyProperty CanRedoProperty = CanRedoPropertyKey.DependencyProperty;

        /// <summary>
        /// Identifies the UndoActionName dependency property
        /// </summary>
        public static readonly DependencyProperty UndoActionNameProperty = 
            UndoActionNamePropertyKey.DependencyProperty;

        /// <summary>
        /// Identifies the RedoActionName dependency property
        /// </summary>
        public static readonly DependencyProperty RedoActionNameProperty = 
            RedoActionNamePropertyKey.DependencyProperty;

        /// <summary>
        /// Identifies the CanUndo dependency property
        /// </summary>
        public static readonly DependencyProperty CanUndoProperty = CanUndoPropertyKey.DependencyProperty;

        /// <summary>
        /// Identifies the RedoAction dependency property
        /// </summary>
        public static readonly DependencyProperty RedoActionProperty = RedoActionPropertyKey.DependencyProperty;

        /// <summary>
        /// Identifies the UndoAction dependency property
        /// </summary>
        public static readonly DependencyProperty UndoActionProperty = UndoActionPropertyKey.DependencyProperty;

        /// <summary>
        /// Identifies the Size dependency property
        /// </summary>
        public static readonly DependencyProperty SizeProperty = 
            DependencyProperty.Register("Size", typeof(int), typeof(WpfHistory),
                new PropertyMetadata(DEFAULT_STACK_SIZE, Size_Changed), ValidateSize);

        #endregion Dependency Properties

        #region Fields

        private const int DEFAULT_STACK_SIZE = 25;

        private IAction _lastRecorded = null;
        SizedStack<IAction> _redoStack;
        SizedStack<IAction> _undoStack;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes the instance with the default size
        /// </summary>
        public WpfHistory()
            : this(DEFAULT_STACK_SIZE)
        {
        }

        /// <summary>
        /// Initializes the instance with the given size
        /// </summary>
        /// <exception cref="ArgumentException">size is smaller than 1</exception>
        /// <param name="size">The number of actions to record</param>
        public WpfHistory(int size)
        {
            if (size == Size)
            {
                _undoStack = new SizedStack<IAction>(size);
                _redoStack = new SizedStack<IAction>(size);
            }
            else
            {
                Size = size;
            }
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets whether it is possible to undo the previous action. This is a dependency property
        /// </summary>
        public virtual bool CanRedo
        {
            get
            {
                return (bool)GetValue(CanRedoProperty);
            }
            private set
            {
                SetValue(CanRedoPropertyKey, value);
            }
        }

        /// <summary>
        /// Gets whether it is possible to redo the previous undone action. This is a dependency property
        /// </summary>
        public virtual bool CanUndo
        {
            get
            {
                return (bool)GetValue(CanUndoProperty);
            }
            private set
            {
                SetValue(CanUndoPropertyKey, value);
            }
        }

        /// <summary>
        /// Gets the last undone action, or null if no actions have been undone. This is a dependency property
        /// </summary>
        public IAction RedoAction
        {
            get
            {
                return (IAction)GetValue(RedoActionProperty);
            }
            private set
            {
                SetValue(RedoActionPropertyKey, value);
            }
        }

        /// <summary>
        /// Gets the name of the last undone action, or an empty string if no actions have been undone. This is a
        /// dependency property
        /// </summary>
        public string RedoActionName
        {
            get
            {
                return (string)GetValue(RedoActionNameProperty);
            }
            private set
            {
                SetValue(RedoActionNamePropertyKey, value);
            }
        }

        /// <summary>
        /// Gets the number of undone actions
        /// </summary>
        public int RedoStackCount
        {
            get
            {
                return _redoStack.Count;
            }
        }

        /// <summary>
        /// Gets or sets the number of actions this Histoty instance can hold. When set all current undo and redo
        /// history is cleared
        /// </summary>
        [DefaultValue(25)]
        public int Size
        {
            get
            {
                return (int)GetValue(SizeProperty);
            }
            set
            {
                SetValue(SizeProperty, value);
            }
        }

        /// <summary>
        /// Gets the last executed action, or null if no actions have been executed. This is a dependency property
        /// </summary>
        public IAction UndoAction
        {
            get
            {
                return (IAction)GetValue(UndoActionProperty);
            }
            private set
            {
                SetValue(UndoActionPropertyKey, value);
            }
        }

        /// <summary>
        /// Gets the name of the last executed action, or an empty string if no actions have been executed. This is a
        /// dependency property
        /// </summary>
        public string UndoActionName
        {
            get
            {
                return (string)GetValue(UndoActionNameProperty);
            }
            private set
            {
                SetValue(UndoActionNamePropertyKey, value);
            }
        }

        /// <summary>
        /// Gets the number executed actions
        /// </summary>
        public int UndoStackCount
        {
            get
            {
                return _undoStack.Count;
            }
        }

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Clears the undo and redo stacks. This method must be called from the thread which owns this object
        /// </summary>
        public void Clear()
        {
            VerifyAccess();
            _undoStack.Clear();
            _redoStack.Clear();
            RedoAction = null;
            UndoAction = null;
            CanRedo = false;
            CanUndo = false;
        }

        /// <summary>
        /// Records and executes the given IAction, and executes any existing unexecuted actions. This method must be
        /// called from the thread which owns this object
        /// </summary>
        /// <exception cref="ArgumentNullException">action is null</exception>
        /// <param name="action">The IAction instance to record and execute</param>
        public void ExecuteAndRecord(IAction action)
        {
            VerifyAccess();

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
                UndoAction = action;
                CanUndo = true;
            }
        }

        /// <summary>
        /// Executes the IAction instance provided to the last call of RecordAction(IAction) method. This method must
        /// be called from the thread which owns this object
        /// </summary>
        /// <returns>true if the action was executed; otherwise false if no actions exist which have not been
        /// executed</returns>
        public bool ExecutedLastRecordedAction()
        {
            VerifyAccess();

            lock (this)
            {
                if (_lastRecorded != null)
                {
                    Execute();
                    UndoAction = _lastRecorded;
                    _lastRecorded = null;
                    CanUndo = true;

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Records the given IAction instance, and executes any existing unexecuted actions. This method must be
        /// called from the thread which owns this object
        /// </summary>
        /// <exception cref="ArgumentNullException">action is null</exception>
        /// <param name="action">The IAction instance to record</param>
        public void RecordAction(IAction action)
        {
            VerifyAccess();

            if (action == null)
            {
                throw new ArgumentNullException();
            }

            lock (this)
            {
                if (_lastRecorded != null)
                {
                    Execute();
                    UndoAction = _lastRecorded;
                }

                _lastRecorded = action;
            }
        }

        /// <summary>
        /// Executes the last IAction instance which was unexecuted. This method must be called from the thread which
        /// owns this object
        /// </summary>
        /// <returns>true if an action was executed; otherwise false if there are no actions to execute</returns>
        public virtual bool Redo()
        {
            VerifyAccess();

            bool toReturn = false;

            lock (this)
            {
                if (CanRedo)
                {
                    IAction action = _undoStack.Pop();
                    toReturn = action.Execute();
                    _undoStack.Push(action);
                    UndoAction = action;
                    IAction redoAction;
                    _redoStack.TryPeek(out redoAction);
                    RedoAction = redoAction;
                    CanRedo = _redoStack.Count > 0;
                    CanUndo = true;
                }
            }

            return toReturn;
        }

        /// <summary>
        /// Unexecutes the last IAction instance which was executed. This method must be called from the thread which
        /// owns this object
        /// </summary>
        /// <returns>true if an action was unexecuted; otherwise false if there are no actions to unexecute</returns>
        public virtual bool Undo()
        {
            VerifyAccess();

            bool toReturn = false;

            lock (this)
            {
                if (CanUndo)
                {
                    IAction action = _undoStack.Pop();
                    toReturn = action.UnExecute();
                    _redoStack.Push(action);
                    RedoAction = action;
                    CanRedo = true;
                    CanUndo = _undoStack.Count > 0;
                    IAction undoAction;
                    _undoStack.TryPeek(out undoAction);
                    UndoAction = undoAction;
                }
            }

            return toReturn;
        }

        #endregion Public Methods

        #region Private Static Methods

        /// <summary>
        /// Updates the RedoActionName property when the RedoAction is changed
        /// </summary>
        /// <param name="d">The WpfHistory object</param>
        /// <param name="e">The arguments that describe this event</param>
        private static void RedoAction_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WpfHistory history = (WpfHistory)d;
            IAction action = (IAction)e.NewValue;

            history.RedoActionName = action != null ? action.Name : string.Empty;
        }

        /// <summary>
        /// Creates new Undo and Redo stacks of the size of the new value
        /// </summary>
        /// <param name="d">The WpfHistory object</param>
        /// <param name="e">The arguments that describe this event</param>
        private static void Size_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WpfHistory history = (WpfHistory)d;
            int size = (int)e.NewValue;

            history._undoStack = new SizedStack<IAction>(size);
            history._redoStack = new SizedStack<IAction>(size);
        }

        /// <summary>
        /// Updates the UndoActionName property when the UndoAction is changed
        /// </summary>
        /// <param name="d">The WpfHistory object</param>
        /// <param name="e">The arguments that describe this event</param>
        private static void UndoAction_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WpfHistory history = (WpfHistory)d;
            IAction action = (IAction)e.NewValue;

            history.UndoActionName = action != null ? action.Name : string.Empty;
        }

        /// <summary>
        /// Returns whether value is larger then 0
        /// </summary>
        /// <param name="value">The int value to test</param>
        /// <returns>true if value is larger than 0; otherwise, false</returns>
        private static bool ValidateSize(object value)
        {
            return (int)value > 0;
        }

        #endregion Private Static Methods

        #region Private Methods

        /// <summary>
        /// Executes the last recorded IAction instance
        /// </summary>
        private void Execute()
        {
            _undoStack.Push(_lastRecorded);
            _lastRecorded.Execute();
            _redoStack.Clear();
            RedoAction = null;
            CanRedo = false;
        }

        #endregion Private Methods

        #endregion Methods
    }
}