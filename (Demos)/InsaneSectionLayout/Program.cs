using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Mi.PE;
using Mi.PE.Internal;
using Mi.PE.PEFormat;

namespace InsaneSectionLayout
{
    class Program
    {
        static void Main(string[] args)
        {
            var pe = new PEFile();
            var stream = new MemoryStream(Properties.Resources.console_anycpu);
            var reader = new BinaryStreamReader(stream, new byte[1024]);
            pe.ReadFrom(reader);
            byte[][] sectionContent = new byte[pe.SectionHeaders.Length][];
            for (int i = 0; i < sectionContent.Length; i++)
			{
                sectionContent[i] = new byte[pe.SectionHeaders[i].SizeOfRawData];
                reader.Position = pe.SectionHeaders[i].PointerToRawData;
                reader.ReadBytes(sectionContent[i], 0, sectionContent[i].Length);
			}

            byte[] combinedContent = CombineAllSections(pe, sectionContent);

            using (var peFileStream = File.Create("console.anycpu.insane.exe"))
            {
                var writer = new BinaryStreamWriter(peFileStream);
                pe.WriteTo(writer);
            }

            var pe2 = new PEFile();
            stream = new MemoryStream(Properties.Resources.console_anycpu);
            reader = new BinaryStreamReader(stream, new byte[1024]);
            pe2.ReadFrom(reader);

            BreakIntoSmallSections(pe2);

            using (var peFileStream = File.Create("console.anycpu.insane.broken.exe"))
            {
                var writer = new BinaryStreamWriter(peFileStream);
                pe2.WriteTo(writer);
            }
        }

        private static void BreakIntoSmallSections(PEFile pe)
        {
            uint lowestPointerToRawData = uint.MaxValue;
            uint lowestVirtualAddress = uint.MaxValue;
            uint highestVirtualAddress = 0;

            foreach (var s in pe.SectionHeaders)
            {
                lowestPointerToRawData = Math.Min(lowestPointerToRawData, s.PointerToRawData);
                lowestVirtualAddress = Math.Min(lowestVirtualAddress, s.VirtualAddress);
                highestVirtualAddress = Math.Max(highestVirtualAddress, s.VirtualAddress + (uint)s.Content.Length);
            }

            byte[] allSectionContent = new byte[highestVirtualAddress - lowestVirtualAddress];
            foreach (var s in pe.SectionHeaders)
            {
                uint pos = s.VirtualAddress - lowestVirtualAddress;
                Array.Copy(
                    s.Content, 0,
                    allSectionContent, pos,
                    s.Content.Length);
            }

            const uint sectionSize = 512;
            pe.PEHeader.NumberOfSections = (ushort)(allSectionContent.Length / sectionSize);
            for (int i = 0; i < pe.SectionHeaders.Count; i++)
            {
                pe.SectionHeaders[i].Name = "S"+i;
                pe.SectionHeaders[i].VirtualAddress = lowestVirtualAddress + (uint)(i * sectionSize);
                pe.SectionHeaders[i].PointerToRawData = lowestPointerToRawData + (uint)(i * sectionSize);
                pe.SectionHeaders[i].VirtualSize = sectionSize;
                pe.SectionHeaders[i].SizeOfRawData = sectionSize;
                pe.SectionHeaders[i].Characteristics = SectionCharacteristics.MemoryRead;

                Array.Copy(
                    allSectionContent, i * sectionSize,
                    pe.SectionHeaders[i].Content, 0,
                    sectionSize);
            }
        }

        private static byte[] CombineAllSections(PEFile pe, byte[][] sections)
        {
            uint lowestPointerToRawData = uint.MaxValue;
            uint lowestVirtualAddress = uint.MaxValue;
            uint highestVirtualAddress = 0;

            foreach (var s in pe.SectionHeaders)
            {
                lowestPointerToRawData = Math.Min(lowestPointerToRawData, s.PointerToRawData);
                lowestVirtualAddress = Math.Min(lowestVirtualAddress, s.VirtualAddress);
                highestVirtualAddress = Math.Max(highestVirtualAddress, s.VirtualAddress + (uint)s.Content.Length);
            }

            byte[] allSectionContent = new byte[highestVirtualAddress - lowestVirtualAddress];
            foreach (var s in pe.SectionHeaders)
            {
                uint pos = s.VirtualAddress - lowestVirtualAddress;
                Array.Copy(
                    s.Content, 0,
                    allSectionContent, pos,
                    s.Content.Length);
            }

            pe.PEHeader.NumberOfSections = 1;
            pe.SectionHeaders[0].Name = "Insane";
            pe.SectionHeaders[0].VirtualAddress = lowestVirtualAddress;
            pe.SectionHeaders[0].PointerToRawData = lowestPointerToRawData;
            pe.SectionHeaders[0].VirtualSize = (uint)allSectionContent.Length;
            pe.SectionHeaders[0].SizeOfRawData = (uint)allSectionContent.Length;
            pe.SectionHeaders[0].Characteristics = SectionCharacteristics.MemoryRead;

            Array.Copy(
                allSectionContent, pe.SectionHeaders[0].Content,
                allSectionContent.Length);
        }
    }
}