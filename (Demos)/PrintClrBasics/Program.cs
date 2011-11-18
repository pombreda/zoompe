using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mi.PE;
using Mi.PE.Cli;
using Mi.PE.Internal;
using Mi.PE.PEFormat;
using Mi.PE.Unmanaged;

namespace PrintClrBasics
{
    class Program
    {
        static void Main(string[] args)
        {
            string mscolib = typeof(int).Assembly.Location;

            var pe = new PEFile();

            Console.WriteLine(Path.GetFileName(mscolib));
            var clrBasics = GetClrBasicsFor(mscolib, pe);

            PrintClrHeader(clrBasics);

            string self = typeof(Program).Assembly.Location;
            Console.WriteLine(Path.GetFileName(self));
            clrBasics = GetClrBasicsFor(self, pe);

            PrintClrHeader(clrBasics);
        }

        private static void PrintClrHeader(ClrModule clrMod)
        {
            Console.WriteLine("  Flags: " + clrMod.ImageFlags);
            Console.WriteLine("  CLR v" + clrMod.RuntimeVersion);
            Console.WriteLine("  Metadata v" + clrMod.MetadataVersion + " " + clrMod.MetadataVersionString);
            Console.WriteLine("  TableStream v" + clrMod.TableStreamVersion);
        }

        private static ClrModule GetClrBasicsFor(string file, PEFile pe)
        {
            var stream = new MemoryStream(File.ReadAllBytes(file));
            var reader = new BinaryStreamReader(stream, new byte[1024]);
            pe.ReadFrom(reader);

            var clrDirectory = pe.OptionalHeader.DataDirectories[(int)DataDirectoryKind.Clr];

            var rvaStream = new RvaStream(
                stream,
                pe.SectionHeaders.Select(
                s => new RvaStream.Range
                {
                    PhysicalAddress = s.PointerToRawData,
                    Size = s.VirtualSize,
                    VirtualAddress = s.VirtualAddress
                })
                .ToArray());

            rvaStream.Position = clrDirectory.VirtualAddress;

            var sectionReader = new BinaryStreamReader(rvaStream, new byte[32]);

            var clrmod = new ClrModule();
            clrmod.Read(sectionReader);

            return clrmod;
        }
    }
}