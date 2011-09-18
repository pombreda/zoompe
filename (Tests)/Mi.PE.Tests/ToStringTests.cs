using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Mi.PE.PEFormat;

namespace Mi.PE
{
    [TestClass]
    public class ToStringTests
    {
        [TestMethod]
        public void DataDirectory()
        {
            var dd = new DataDirectory
            {
                VirtualAddress = 0x10,
                Size = 0x11
            };

            Assert.AreEqual("10:11h", dd.ToString());
        }

        [TestMethod]
        public void DosHeader()
        {
            var dh = new DosHeader
            {
                Signature = MZSignature.MZ,
                lfanew = 0x102
            };

            Assert.AreEqual("[MZ].lfanew=102h", dh.ToString());
        }

        [TestMethod]
        public void DosHeader_NotMZ()
        {
            var dh = new DosHeader
            {
                Signature = (MZSignature)0x41,
                lfanew = 0x102
            };

            Assert.AreEqual("[Signature:00'A'h].lfanew=102h", dh.ToString());
        }

        [TestMethod]
        public void OptionalHeader()
        {
            var oh = new OptionalHeader
            {
                PEMagic = PEMagic.NT32,
                Subsystem = Subsystem.WindowsCUI,
                DllCharacteristics = DllCharacteristics.TerminalServerAware
            };

            Assert.AreEqual("NT32 WindowsCUI TerminalServerAware", oh.ToString());
        }

        [TestMethod]
        public void OptionalHeader_EmptyDataDirectories()
        {
            var oh = new OptionalHeader
            {
                PEMagic = PEMagic.NT32,
                Subsystem = Subsystem.WindowsCUI,
                DllCharacteristics = DllCharacteristics.TerminalServerAware,
                DataDirectories = new DataDirectory[4]
            };

            Assert.AreEqual("NT32 WindowsCUI TerminalServerAware", oh.ToString());
        }

        [TestMethod]
        public void OptionalHeader_NonEmptyDataDirectories()
        {
            var oh = new OptionalHeader
            {
                PEMagic = PEMagic.NT32,
                Subsystem = Subsystem.WindowsCUI,
                DllCharacteristics = DllCharacteristics.TerminalServerAware,
                DataDirectories = new[]
                {
                    new DataDirectory { Size = 5 },
                    new DataDirectory { Size = 6 },
                    new DataDirectory { Size = 100 },
                    new DataDirectory { VirtualAddress = 10, Size = 20 }
                }
            };

            Assert.AreEqual("NT32 WindowsCUI TerminalServerAware DataDirectories[ExportSymbols,ImportSymbols,Resources,Exception]", oh.ToString());
        }

        [TestMethod]
        public void PEHeader()
        {
            var peh = new PEHeader
            {
                Machine = Machine.I386,
                Characteristics = ImageCharacteristics.Bit32Machine
            };

            Assert.AreEqual("I386 Bit32Machine Sections[0]", peh.ToString());
        }

        [TestMethod]
        public void Section()
        {
            var sh = new Section
            {
                Name = "Dummy",
                PointerToRawData = 0x14e,
                SizeOfRawData = 0x1aff0,
                VirtualAddress = 0x1234c0,
                VirtualSize = 0x320ff
            };

            Assert.AreEqual("Dummy [14E:1AFF0h]=>Virtual[1234C0:320FFh]", sh.ToString());
        }
    }
}