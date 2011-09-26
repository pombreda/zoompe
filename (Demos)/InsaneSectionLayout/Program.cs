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

            using (var peFileStream = File.Create("console.anycpu.insane.exe"))
            {
                pe.WriteTo(peFileStream);
            }
        }
    }
}