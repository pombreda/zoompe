using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mi.PE;
using Mi.PE.Internal;
using Mi.PE.PEFormat;

namespace PrintImports
{
    class Program
    {
        static void Main(string[] args)
        {
            string kernel32 = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.System),
                "kernel32.dll");

            Console.WriteLine(Path.GetFileName(kernel32));
            var imports = GetImportsFor(kernel32);

            foreach (var i in imports)
            {
                Console.WriteLine("  " + i.ToString());
            }

            string self = typeof(Program).Assembly.Location;
            Console.WriteLine(Path.GetFileName(self));
            imports = GetImportsFor(self);

            foreach (var i in imports)
            {
                Console.WriteLine("  " + i.ToString());
            }
        }

        private static Mi.PE.Unmanaged.Import[] GetImportsFor(string file)
        {
            var pe = PEFile.ReadFrom(new MemoryStream(File.ReadAllBytes(file)));

            var importDirectory = pe.OptionalHeader.DataDirectories[(int)DataDirectoryKind.ImportSymbols];

            var relevantSection = pe.Sections.First(s => importDirectory.VirtualAddress >= s.VirtualAddress && importDirectory.VirtualAddress < s.VirtualAddress + s.VirtualSize);

            var sectionReader = new SectionContentReader(relevantSection.Content, (int)relevantSection.VirtualAddress);
            sectionReader.VirtualPosition = (int)importDirectory.VirtualAddress;

            var imports = Mi.PE.Unmanaged.Import.ReadImports(sectionReader);
            return imports;
        }
    }
}