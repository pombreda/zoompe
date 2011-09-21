using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mi.PE
{
    [TestClass]
    public class PEFileTests
    {
        [TestMethod]
        public void SettingPEHeaderNumberOfSections_ChangesSectionsCount()
        {
            var pe = new PEFile();
            Assert.AreEqual(0, pe.Sections.Count);
            pe.PEHeader.NumberOfSections = 3;
            Assert.AreEqual(3, pe.Sections.Count);
        }

        [TestMethod]
        public void SettingPEHeaderNumberOfSections_ClearsSections()
        {
            var pe = new PEFile();
            pe.PEHeader.NumberOfSections = 1;
            pe.Sections[0].Name = "dummy";
            pe.PEHeader.NumberOfSections = 3;
            Assert.AreEqual(null, pe.Sections[0].Name);
        }

        [TestMethod]
        public void SettingSamePEHeaderNumberOfSections_LeavesSectionsIntact()
        {
            var pe = new PEFile();
            pe.PEHeader.NumberOfSections = 1;
            pe.Sections[0].Name = "dummy";
            pe.PEHeader.NumberOfSections = 1;
            Assert.AreEqual("dummy", pe.Sections[0].Name);
        }
    }
}