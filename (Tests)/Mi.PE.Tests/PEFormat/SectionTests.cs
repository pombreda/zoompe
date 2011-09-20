using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mi.PE.PEFormat
{
    [TestClass]
    public class SectionTests
    {
        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void Name_TooLong()
        {
            var pe = new PEFile();
            pe.PEHeader.NumberOfSections = 1;
            var sh = pe.Sections[0];
            sh.Name = "123456789";
        }

        [TestMethod]
        public void PhysicalAddress_EqualsVirtualSize()
        {
            var pe = new PEFile();
            pe.PEHeader.NumberOfSections = 1;
            var sh = pe.Sections[0];
            sh.VirtualSize = 23121222;
            Assert.AreEqual((uint)23121222, sh.PhysicalAddress);
            sh.PhysicalAddress = 94;
            Assert.AreEqual((uint)94, sh.VirtualSize);
        }

        [TestMethod]
        public void NameSet()
        {
            var pe = new PEFile();
            pe.PEHeader.NumberOfSections = 1;
            var sh = pe.Sections[0];
            sh.Name = "a";
            Assert.AreEqual("a", sh.Name);
            sh.Name = null;
            Assert.AreEqual(null, sh.Name);
        }

        [TestMethod]
        public void Content_EmptyByDefaut()
        {
            var pe = new PEFile();
            pe.PEHeader.NumberOfSections = 1;
            var sh = pe.Sections[0];
            Assert.AreEqual(0, sh.Content.Length);
        }

        [TestMethod]
        public void SetSizeOfRawData_Positive_CreatesContent()
        {
            var pe = new PEFile();
            pe.PEHeader.NumberOfSections = 1;
            var sh = pe.Sections[0];
            sh.SizeOfRawData = 4;
            Assert.AreEqual(4, sh.Content.Length);
        }

        [TestMethod]
        public void SetSizeOfRawData_Zero_EmptiesContent()
        {
            var pe = new PEFile();
            pe.PEHeader.NumberOfSections = 1;
            var sh = pe.Sections[0];
            sh.SizeOfRawData = 0;
            Assert.AreEqual(0, sh.Content.Length);
        }

        [TestMethod]
        public void SetSizeOfRawData_Different_ClearsContent()
        {
            var pe = new PEFile();
            pe.PEHeader.NumberOfSections = 1;
            var sh = pe.Sections[0];
            sh.SizeOfRawData = 4;
            sh.Content[0] = 254;
            sh.SizeOfRawData = 3;
            Assert.AreEqual((byte)0, sh.Content[0]);
        }

        [TestMethod]
        public void SetSizeOfRawData_Same_KeepsContent()
        {
            var pe = new PEFile();
            pe.PEHeader.NumberOfSections = 1;
            var sh = pe.Sections[0];
            sh.SizeOfRawData = 4;
            sh.Content[0] = 254;
            sh.SizeOfRawData = 4;
            Assert.AreEqual((byte)254, sh.Content[0]);
        }
    }
}