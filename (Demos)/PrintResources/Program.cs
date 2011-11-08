using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Mi.PE;
using Mi.PE.Internal;
using Mi.PE.PEFormat;
using Mi.PE.Unmanaged;

namespace PrintResources
{
    class Program
    {
        static void Main(string[] args)
        {
            string kernel32 = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.System),
                "kernel32.dll");

            kernel32 = typeof(int).Assembly.Location;

            Console.WriteLine(Path.GetFileName(kernel32));
            var pe = new PEFile();
            var resources = GetResourcesFor(kernel32, pe);
        }

        static ResourceDirectory GetResourcesFor(string file, PEFile pe)
        {
            var stream = new MemoryStream(File.ReadAllBytes(file));
            var reader = new BinaryStreamReader(stream, new byte[1024]);
            pe.ReadFrom(reader);

            var resDataDir = pe.OptionalHeader.DataDirectories[(int)DataDirectoryKind.Resources];

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

            rvaStream.Position = resDataDir.VirtualAddress;

            var sectionReader = new BinaryStreamReader(rvaStream, new byte[32]);

            var res = new ResourceDirectory();
            res.Read(sectionReader);

            return res;
        }
    }
}
