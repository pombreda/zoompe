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
            var clrHeader = GetClrBasicsFor(mscolib, pe);

            PrintClrHeader(clrHeader);

            string self = typeof(Program).Assembly.Location;
            Console.WriteLine(Path.GetFileName(self));
            clrHeader = GetClrBasicsFor(self, pe);

            PrintClrHeader(clrHeader);
        }

        private static void PrintClrHeader(ClrHeader clrHeader)
        {
            Console.WriteLine("RuntimeVersion: v"+clrHeader.MajorRuntimeVersion + "." + clrHeader.MinorRuntimeVersion);
        }

        private static ClrHeader GetClrBasicsFor(string file, PEFile pe)
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

            var header = ClrHeaderReader.ReadClrHeader(sectionReader);

            sectionReader.Position = header.MetaData.VirtualAddress;

            return header;
        }
    }
}