namespace HistoryFramework
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Reprsents a fixed size last-in-first-out (LIFO) collection of instances of the same arbitrary type
    /// </summary>
    /// <typeparam name="T">Specifies the type of elements in the stack</typeparam>
    public sealed class SizedStack<T>
    {
        #region Fields

        private int _maxSize;
        private LinkedList<T> _stack;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes the object
        /// </summary>
        /// <exception cref="ArgumentException">If maxSize is less than 1</exception>
        /// <param name="maxSize">The maximum number of elements the SizedStack can hold</param>
        public SizedStack(int maxSize)
        {
            if (maxSize < 1)
            {
                throw new ArgumentException("MaxSize must be larger than 0");
            }

            _stack = new LinkedList<T>();
            _maxSize = maxSize;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the number of elements contained in the SizedStack
        /// </summary>
        public int Count
        {
            get
            {
                return _stack.Count;
            }
        }

        /// <summary>
        /// Gets the maximum size of the SizedStack
        /// </summary>
        public int MaxSize
        {
            get
            {
                return _maxSize;
            }
        }

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Removes all objects from the SizedStack
        /// </summary>
        public void Clear()
        {
            _stack.Clear();
        }

        /// <summary>
        /// Gets the object at the top of the SizedStack
        /// </summary>
        /// <exception cref="InvalidOperationException">If the SizedStack is empty</exception>
        /// <returns>The object from the top of the SizedStack</returns>
        public T Peek()
        {
            if (_stack.Count == 0)
            {
                throw new InvalidOperationException();
            }

            return _stack.Last.Value;
        }

        /// <summary>
        /// Removes and returns the object at the top of the SizedStack
        /// </summary>
        /// <exception cref="InvalidOperationException">If the SizedStack is empty</exception>
        /// <returns>The object removed from the top of the SizedStack</returns>
        public T Pop()
        {
            if (_stack.Count == 0)
            {
                throw new InvalidOperationException();
            }

            LinkedListNode<T> last = _stack.Last;

            _stack.RemoveLast();

            return last.Value;
        }

        /// <summary>
        /// Inserts a object at the top of the SizedStack
        /// </summary>
        /// <param name="item">The object to push onto the SizedStack</param>
        public void Push(T item)
        {
            if (Count == _maxSize)
            {
                RemoveFirstItem();
            }

            _stack.AddLast(item);
        }

        /// <summary>
        /// Gets the object at the top of the SizedStack
        /// </summary>
        /// <param name="value">When this method returns, contains the value at the top of the SizedStack; otherwise,
        /// the default value for the type of the value parameter. This parameter is passed uninitialized</param>
        /// <returns>true if the SizedStack is not empty; otherwise, false</returns>
        public bool TryPeek(out T value)
        {
            if (_stack.Count == 0)
            {
                value = default(T);
                return false;
            }

            value = _stack.Last.Value;
            return true;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Removes the first item from the stack
        /// </summary>
        private void RemoveFirstItem()
        {
            _stack.RemoveFirst();
        }

        #endregion Private Methods

        #endregion Methods
    }
}