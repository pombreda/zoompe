using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Mi.PE;

namespace ReadWriteRoundtrip
{
    class Program
    {
        static void Main(string[] args)
        {
            var pe = PEFile.FromStream(new MemoryStream(Properties.Resources.console_anycpu));
            using (var output = File.Create("console.anycpu.exe"))
            {
                pe.WriteTo(output);
            }
        }
    }
}