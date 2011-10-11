using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Mi.PE;
using Mi.PE.Internal;

namespace ReadWriteRoundtrip
{
    class Program
    {
        static void Main(string[] args)
        {
            var pe = new PEFile();
            pe.ReadFrom(new BinaryStreamReader(new MemoryStream(Properties.Resources.console_anycpu), new byte[1024]));
            using (var output = File.Create("console.anycpu.exe"))
            {
                pe.WriteTo(output);
            }
        }
    }
}