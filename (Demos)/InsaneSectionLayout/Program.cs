using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Mi.PE;
using Mi.PE.PEFormat;

namespace InsaneSectionLayout
{
    class Program
    {
        static void Main(string[] args)
        {
            var pe = PEFile.FromStream(new MemoryStream(Properties.Resources.console_anycpu));

            CombineAllSections(pe);

            using (var peFileStream = File.Create("console.anycpu.insane.exe"))
            {
                pe.WriteTo(peFileStream);
            }

            var pe2 = PEFile.FromStream(new MemoryStream(Properties.Resources.console_anycpu));

            BreakIntoSmallSections(pe2);

            using (var peFileStream = File.Create("console.anycpu.insane.broken.exe"))
            {
                pe2.WriteTo(peFileStream);
            }
        }

        private static void BreakIntoSmallSections(PEFile pe)
        {
            uint lowestPointerToRawData = uint.MaxValue;
            uint lowestVirtualAddress = uint.MaxValue;
            uint highestVirtualAddress = 0;

            foreach (var s in pe.Sections)
            {
                lowestPointerToRawData = Math.Min(lowestPointerToRawData, s.PointerToRawData);
                lowestVirtualAddress = Math.Min(lowestVirtualAddress, s.VirtualAddress);
                highestVirtualAddress = Math.Max(highestVirtualAddress, s.VirtualAddress + (uint)s.Content.Length);
            }

            byte[] allSectionContent = new byte[highestVirtualAddress - lowestVirtualAddress];
            foreach (var s in pe.Sections)
            {
                uint pos = s.VirtualAddress - lowestVirtualAddress;
                Array.Copy(
                    s.Content, 0,
                    allSectionContent, pos,
                    s.Content.Length);
            }

            pe.PEHeader.NumberOfSections = (ushort)(allSectionContent.Length / 512);
            for (int i = 0; i < pe.Sections.Count; i++)
            {
                pe.Sections[i].Name = "S"+i;
                pe.Sections[i].VirtualAddress = lowestVirtualAddress + (uint)(i * 512);
                pe.Sections[i].PointerToRawData = lowestPointerToRawData + (uint)(i * 512);
                pe.Sections[i].VirtualSize = 512;
                pe.Sections[i].SizeOfRawData = 512;
                pe.Sections[i].Characteristics = SectionCharacteristics.MemoryRead;

                Array.Copy(
                    allSectionContent, i * 512,
                    pe.Sections[i].Content, 0,
                    512);
            }
        }

        private static void CombineAllSections(PEFile pe)
        {
            uint lowestPointerToRawData = uint.MaxValue;
            uint lowestVirtualAddress = uint.MaxValue;
            uint highestVirtualAddress = 0;

            foreach (var s in pe.Sections)
            {
                lowestPointerToRawData = Math.Min(lowestPointerToRawData, s.PointerToRawData);
                lowestVirtualAddress = Math.Min(lowestVirtualAddress, s.VirtualAddress);
                highestVirtualAddress = Math.Max(highestVirtualAddress, s.VirtualAddress + (uint)s.Content.Length);
            }

            byte[] allSectionContent = new byte[highestVirtualAddress - lowestVirtualAddress];
            foreach (var s in pe.Sections)
            {
                uint pos = s.VirtualAddress - lowestVirtualAddress;
                Array.Copy(
                    s.Content, 0,
                    allSectionContent, pos,
                    s.Content.Length);
            }

            pe.PEHeader.NumberOfSections = 1;
            pe.Sections[0].Name = "Insane";
            pe.Sections[0].VirtualAddress = lowestVirtualAddress;
            pe.Sections[0].PointerToRawData = lowestPointerToRawData;
            pe.Sections[0].VirtualSize = (uint)allSectionContent.Length;
            pe.Sections[0].SizeOfRawData = (uint)allSectionContent.Length;
            pe.Sections[0].Characteristics = SectionCharacteristics.MemoryRead;

            Array.Copy(
                allSectionContent, pe.Sections[0].Content,
                allSectionContent.Length);
        }
    }
}