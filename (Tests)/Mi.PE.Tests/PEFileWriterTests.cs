using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mi.PE
{
    [TestClass]
    public class PEFileWriterTests
    {
        [TestMethod]
        public void Constructor()
        {
            new PEFile.Writer();
        }

        [TestMethod] public void PreReadAnyCPU() { AssertReadWriteRoundtrip(Properties.Resources.console_anycpu); }
        [TestMethod] public void PreReadX86() { AssertReadWriteRoundtrip(Properties.Resources.console_x86); }

        private static void AssertReadWriteRoundtrip(byte[] originalBytes)
        {
            var pe = PEFile.FromStream(new MemoryStream(originalBytes));

            var buf = new MemoryStream();
            pe.WriteTo(buf);

            byte[] outputBytes = buf.ToArray();
            Assert.AreEqual(originalBytes.Length, outputBytes.Length, "outputBytes.Length");

            for (int i = 0; i < outputBytes.Length; i++)
            {
                Assert.AreEqual(originalBytes[i], outputBytes[i], "outputBytes[" + i + "]");
            }
        }
    }
}