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
        //    string kernel32 = Path.Combine(
        //        Environment.GetFolderPath(Environment.SpecialFolder.System),
        //        "kernel32.dll");

        //    var pe = new PEFile();

        //    Console.WriteLine(Path.GetFileName(kernel32));
        //    var relocBlocks = GetClrBasicsFor(kernel32, pe);

        //    PrintBaseRelocations(relocBlocks);

        //    string self = typeof(Program).Assembly.Location;
        //    Console.WriteLine(Path.GetFileName(self));
        //    relocBlocks = GetClrBasicsFor(self, pe);

        //    PrintBaseRelocations(relocBlocks);
        //}

        //private static void PrintBaseRelocations(BaseRelocationBlock[] relocBlocks)
        //{
        //    foreach (var b in relocBlocks)
        //    {
        //        Console.WriteLine(b.PageRVA.ToString("X")+"h ("+b.Size+")");
        //        foreach (var e in b.Entries)
        //        {
        //            Console.WriteLine("    " + e.Offset.ToString("X").PadLeft(4, '0') + "h " + e.Type);
        //        }
        //    }
        //}

        //private static BaseRelocationBlock[] GetClrBasicsFor(string file, PEFile pe)
        //{
        //    var stream = new MemoryStream(File.ReadAllBytes(file));
        //    var reader = new BinaryStreamReader(stream, new byte[1024]);
        //    pe.ReadFrom(reader);

        //    var clrDirectory = pe.OptionalHeader.DataDirectories[(int)DataDirectoryKind.Clr];

        //    var rvaStream = new RvaStream(
        //        stream,
        //        pe.SectionHeaders.Select(
        //        s => new RvaStream.Range
        //        {
        //            PhysicalAddress = s.PointerToRawData,
        //            Size = s.VirtualSize,
        //            VirtualAddress = s.VirtualAddress
        //        })
        //        .ToArray());

        //    rvaStream.Position = clrDirectory.VirtualAddress;

        //    var sectionReader = new BinaryStreamReader(rvaStream, new byte[32]);

        //    var reader = ClrHeaderReader.ReadClrHeader(sectionReader);
        //    return result;
        }
    }
}