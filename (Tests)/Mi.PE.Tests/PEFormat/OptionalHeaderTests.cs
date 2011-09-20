using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mi.PE.PEFormat
{
    [TestClass]
    public class OptionalHeaderTests
    {
        [TestMethod]
        public void Directories_EmptyByDefaut()
        {
            var oh = new PEFile().OptionalHeader;
            Assert.AreEqual(0, oh.DataDirectories.Length);
        }

        [TestMethod]
        public void SetNumberOfRvaAndSizes_Positive_CreatesDataDirectories()
        {
            var oh = new PEFile().OptionalHeader;
            oh.NumberOfRvaAndSizes = 4;
            Assert.AreEqual(4, oh.DataDirectories.Length);
        }

        [TestMethod]
        public void SetNumberOfRvaAndSizes_Zero_EmptiesDataDirectories()
        {
            var oh = new PEFile().OptionalHeader;
            oh.NumberOfRvaAndSizes = 0;
            Assert.AreEqual(0, oh.DataDirectories.Length);
        }

        [TestMethod]
        public void SetNumberOfRvaAndSizes_Different_ClearsDataDirectories()
        {
            var oh = new PEFile().OptionalHeader;
            oh.NumberOfRvaAndSizes = 4;
            oh.DataDirectories[0] = new DataDirectory { Size = 43 };
            oh.NumberOfRvaAndSizes = 3;
            Assert.AreEqual((uint)0, oh.DataDirectories[0].Size);
        }

        [TestMethod]
        public void SetNumberOfRvaAndSizes_Same_KeepsDataDirectories()
        {
            var oh = new PEFile().OptionalHeader;
            oh.NumberOfRvaAndSizes = 4;
            oh.DataDirectories[0] = new DataDirectory { Size = 234222 };
            oh.NumberOfRvaAndSizes = 4;
            Assert.AreEqual((uint)234222, oh.DataDirectories[0].Size);
        }
    }
}