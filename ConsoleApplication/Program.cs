namespace ConsoleApplication
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;

    using HistoryFramework;

    using NUnit.Framework;

    class Program
    {
        #region Methods

        #region Private Static Methods

        static void Main(string[] args)
        {
            IHistory _history = new WpfHistory();
            _history.Clear();
            TestAction action = new TestAction();
            _history.RecordAction(action);

            Assert.IsFalse(_history.CanUndo);
            Assert.IsFalse(_history.CanRedo);

            _history.ExecutedLastRecordedAction();

            Assert.IsTrue(_history.CanUndo);
            Assert.IsFalse(_history.CanRedo);
            Assert.AreEqual(_history.UndoStackCount, 1);
            Assert.AreEqual(action, _history.UndoAction);
        }

        #endregion Private Static Methods

        #endregion Methods

        #region Nested Types

        class TestAction : HistoryFramework.Action
        {
            #region Methods

            #region Protected Methods

            protected override void ExecuteTask()
            {
                Debug.Write("TestAction executed");
            }

            protected override void UnExecuteTask()
            {
                Debug.Write("TestAction unexecuted");
            }

            #endregion Protected Methods

            #endregion Methods
        }

        #endregion Nested Types
    }
}