namespace Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Windows.Threading;

    using HistoryFramework;

    using NUnit.Framework;

    [TestFixture]
    public class WpfHistoryTest : HistoryTest
    {
        #region Methods

        #region Protected Methods

        protected override IHistory InstantiateHistory(int size)
        {
            return new WpfHistory(size);
        }

        #endregion Protected Methods

        #endregion Methods
    }
}