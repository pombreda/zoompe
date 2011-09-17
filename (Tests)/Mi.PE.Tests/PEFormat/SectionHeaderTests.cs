using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mi.PE.PEFormat
{
    [TestClass]
    public class SectionHeaderTests
    {
        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void Name_TooLong()
        {
            var sh = new SectionHeader();
            sh.Name = "123456789";
        }

        [TestMethod]
        public void PhysicalAddress_EqualsVirtualSize()
        {
            var sh = new SectionHeader();
            sh.VirtualSize = 23121222;
            Assert.AreEqual((uint)23121222, sh.PhysicalAddress);
            sh.PhysicalAddress = 94;
            Assert.AreEqual((uint)94, sh.VirtualSize);
        }

        [TestMethod]
        public void NameSet()
        {
            var sh = new SectionHeader();
            sh.Name = "a";
            Assert.AreEqual("a", sh.Name);
            sh.Name = null;
            Assert.AreEqual(null, sh.Name);
        }
    }
}