using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mi.PE.PEFormat
{
    [TestClass]
    public class DosHeaderTests
    {
        [TestMethod]
        public void Stub_NullByDefault()
        {
            var dosHeader = new PEFile().DosHeader;
            Assert.IsNull(dosHeader.Stub);
        }
    }
}