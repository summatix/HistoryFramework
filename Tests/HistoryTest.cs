namespace Tests
{
    using System;
    using System.Diagnostics;
    using System.Threading;
    using System.Windows.Threading;

    using HistoryFramework;

    using NUnit.Framework;

    [TestFixture]
    public class HistoryTest
    {
        #region Fields

        private IHistory _history;

        #endregion Fields

        #region Constructors

        public HistoryTest()
        {
            _history = InstantiateHistory(25);
        }

        #endregion Constructors

        #region Methods

        #region Public Methods

        [Test,
        ExpectedException(typeof(ArgumentException))]
        public void Constructor()
        {
            _history = InstantiateHistory(0);
        }

        [Test]
        public void TestCase1()
        {
            TestAction action = new TestAction();
            _history.ExecuteAndRecord(action);

            Assert.IsTrue(_history.CanUndo);
            Assert.IsFalse(_history.CanRedo);
            Assert.AreEqual(action, _history.UndoAction);
            Assert.AreEqual(_history.UndoStackCount, 1);
        }

        [Test]
        public void TestCase2()
        {
            TestAction action = new TestAction();
            _history.ExecuteAndRecord(action);

            _history.Undo();
            Assert.IsFalse(_history.CanUndo);
            Assert.IsTrue(_history.CanRedo);
            Assert.AreEqual(null, _history.UndoAction);
            Assert.AreEqual(action, _history.RedoAction);
            Assert.AreEqual(_history.UndoStackCount, 0);
            Assert.AreEqual(_history.RedoStackCount, 1);
        }

        [Test]
        public void TestCase3()
        {
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

        [Test]
        public void TestCase4()
        {
            TestAction action = new TestAction();
            _history.RecordAction(new TestAction());
            _history.ExecuteAndRecord(action);

            Assert.IsTrue(_history.CanUndo);
            Assert.IsFalse(_history.CanRedo);
            Assert.AreEqual(_history.UndoStackCount, 2);
            Assert.AreEqual(action, _history.UndoAction);
        }

        [Test]
        public void TestCase5()
        {
            TestAction action = new TestAction();
            _history.RecordAction(new TestAction());
            _history.RecordAction(action);
            _history.ExecutedLastRecordedAction();

            Assert.IsTrue(_history.CanUndo);
            Assert.IsFalse(_history.CanRedo);
            Assert.AreEqual(_history.UndoStackCount, 2);
            Assert.AreEqual(action, _history.UndoAction);
        }

        [Test]
        public void TestConstructorSetsSize()
        {
            _history = InstantiateHistory(89);
            Assert.IsTrue(_history.Size == 89);
        }

        [Test]
        public void TestSize()
        {
            _history.Size = 6056;
            Assert.IsTrue(_history.Size == 6056);
        }

        #endregion Public Methods

        #region Protected Methods

        protected virtual IHistory InstantiateHistory(int size)
        {
            return new History(size);
        }

        [SetUp]
        protected void SetUp()
        {
            _history.Clear();
        }

        #endregion Protected Methods

        #endregion Methods
    }

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
}