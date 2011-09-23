using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mi.PE.Internal
{
    [TestClass]
    public class BinaryStreamWriterTests
    {
        [TestMethod]
        public void Constructor()
        {
            var writer = new BinaryStreamWriter(new MemoryStream());
        }

        [TestMethod]
        public void WriteByte()
        {
            var outputBuf = new MemoryStream();
            var writer = new BinaryStreamWriter(outputBuf);
            writer.WriteByte(231);
            Assert.AreEqual((long)1, outputBuf.Length, "outputBuf.Length");
            Assert.AreEqual((byte)231, outputBuf.ToArray().Single(), "outputBuf[0]"); 
        }
    }
}