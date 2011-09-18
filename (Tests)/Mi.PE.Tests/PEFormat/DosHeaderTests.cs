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
            var dosHeader = new DosHeader();
            Assert.IsNull(dosHeader.Stub);
        }

        [TestMethod]
        public void SetLfanew_GreaterThanDosHeaderSize_CreatesStub()
        {
            var dosHeader = new DosHeader();
            dosHeader.lfanew = DosHeader.Size + 4;
            Assert.AreEqual(4, dosHeader.Stub.Length, "Stub size");
        }

        [TestMethod]
        public void SetLfanew_LesserThanDosHeaderSize_NullsStub()
        {
            var dosHeader = new DosHeader();
            dosHeader.lfanew = DosHeader.Size + 4;
            dosHeader.Stub[0] = 123;
            dosHeader.lfanew = DosHeader.Size - 4;
            Assert.IsNull(dosHeader.Stub);
        }

        [TestMethod]
        public void SetLfanew_Different_ClearStub()
        {
            var dosHeader = new DosHeader();
            dosHeader.lfanew = DosHeader.Size + 4;
            dosHeader.Stub[0] = 123;
            dosHeader.lfanew = DosHeader.Size + 3;
            Assert.AreEqual(0, dosHeader.Stub[0], "Stub[0]");
        }

        [TestMethod]
        public void SetLfanew_Same_KeepsStub()
        {
            var dosHeader = new DosHeader();
            dosHeader.lfanew = DosHeader.Size + 4;
            dosHeader.Stub[0] = 123;
            dosHeader.lfanew = DosHeader.Size + 4;
            Assert.AreEqual((byte)123, dosHeader.Stub[0], "Stub[0]");
        }
    }
}