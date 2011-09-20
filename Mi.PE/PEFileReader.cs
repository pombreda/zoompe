using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Mi.PE
{
    using Mi.PE.Internal;
    using Mi.PE.PEFormat;

    public sealed class PEFileReader
    {
        const int BufferSize = 1024;
        readonly byte[] buffer = new byte[BufferSize];

        public PEFileReader()
        {
        }

        public bool PopulateSectionContent { get; set; }

        public PEFile ReadMetadata(Stream stream)
        {
            var reader = new BinaryStreamReader(stream, this.buffer);
            return ReadMetadata(reader);
        }

        public PEFile ReadMetadata(BinaryStreamReader reader)
        {
            var pe = new PEFile();

            ReadDosHeader(reader, pe.DosHeader);
            
            reader.Position = pe.DosHeader.lfanew;
            ReadPEHeader(reader, pe.PEHeader);
            ReadOptionalHeader(reader, pe.OptionalHeader);
            
            for (int i = 0; i < pe.Sections.Count; i++)
            {
                ReadSectionHeader(reader, pe.Sections[i]);
            }

            if (this.PopulateSectionContent)
            {
                foreach (var s in pe.Sections)
                {
                    if (s.SizeOfRawData > 0)
                    {
                        reader.Position = s.PointerToRawData;
                        reader.ReadBytes(s.Content, 0, checked((int)s.SizeOfRawData));
                    }
                }
            }

            return pe;
        }

        static void ReadDosHeader(BinaryStreamReader reader, DosHeader dosHeader)
        {
            dosHeader.Signature = (MZSignature)reader.ReadInt16();
            if (dosHeader.Signature != MZSignature.MZ)
                throw new BadImageFormatException("MZ signature expected, "+((ushort)dosHeader.Signature).ToString("X4")+"h found.");

            dosHeader.cblp = reader.ReadUInt16();
            dosHeader.cp = reader.ReadUInt16();
            dosHeader.crlc = reader.ReadUInt16();
            dosHeader.cparhdr = reader.ReadUInt16();
            dosHeader.minalloc = reader.ReadUInt16();
            dosHeader.maxalloc = reader.ReadUInt16();
            dosHeader.ss = reader.ReadUInt16();
            dosHeader.sp = reader.ReadUInt16();
            dosHeader.csum = reader.ReadUInt16();
            dosHeader.ip = reader.ReadUInt16();
            dosHeader.cs = reader.ReadUInt16();
            dosHeader.lfarlc = reader.ReadUInt16();
            dosHeader.ovno = reader.ReadUInt16();

            dosHeader.res1 = reader.ReadUInt64();
            
            dosHeader.oemid = reader.ReadUInt16();
            dosHeader.oeminfo = reader.ReadUInt16();

            dosHeader.ReservedNumber0 = reader.ReadUInt32();
            dosHeader.ReservedNumber1 = reader.ReadUInt32();
            dosHeader.ReservedNumber2 = reader.ReadUInt32();
            dosHeader.ReservedNumber3 = reader.ReadUInt32();
            dosHeader.ReservedNumber4 = reader.ReadUInt32();
            dosHeader.lfanew = reader.ReadUInt32();

            if (dosHeader.Stub!=null)
                reader.ReadBytes(dosHeader.Stub, 0, dosHeader.Stub.Length);
        }

        static void ReadPEHeader(BinaryStreamReader reader, PEHeader peHeader)
        {
            peHeader.PESignature = (PESignature)reader.ReadInt32();
            peHeader.Machine = (Machine)reader.ReadInt16();
            peHeader.NumberOfSections = reader.ReadUInt16();
            peHeader.Timestamp = new ImageTimestamp(reader.ReadUInt32());
            peHeader.PointerToSymbolTable = reader.ReadUInt32();
            peHeader.NumberOfSymbols = reader.ReadUInt32();
            peHeader.SizeOfOptionalHeader = reader.ReadUInt16();
            peHeader.Characteristics = (ImageCharacteristics)reader.ReadInt16();
        }

        static void ReadOptionalHeader(BinaryStreamReader reader, OptionalHeader optionalHeader)
        {
            var peMagic = (PEMagic)reader.ReadInt16();

            if (peMagic != PEMagic.NT32
                && peMagic != PEMagic.NT64)
                throw new BadImageFormatException("Unsupported PE magic value " + peMagic + ".");

            optionalHeader.PEMagic = peMagic;

            optionalHeader.MajorLinkerVersion = reader.ReadByte();
            optionalHeader.MinorLinkerVersion = reader.ReadByte();
            optionalHeader.SizeOfCode = reader.ReadUInt32();
            optionalHeader.SizeOfInitializedData = reader.ReadUInt32();
            optionalHeader.SizeOfUninitializedData = reader.ReadUInt32();
            optionalHeader.AddressOfEntryPoint = reader.ReadUInt32();
            optionalHeader.BaseOfCode = reader.ReadUInt32();

            if (peMagic == PEMagic.NT32)
            {
                optionalHeader.BaseOfData = reader.ReadUInt32();
                optionalHeader.ImageBase = reader.ReadUInt32();
            }
            else
            {
                optionalHeader.ImageBase = reader.ReadUInt64();
            }

            optionalHeader.SectionAlignment = reader.ReadUInt32();
            optionalHeader.FileAlignment = reader.ReadUInt32();
            optionalHeader.MajorOperatingSystemVersion = reader.ReadUInt16();
            optionalHeader.MinorOperatingSystemVersion = reader.ReadUInt16();
            optionalHeader.MajorImageVersion = reader.ReadUInt16();
            optionalHeader.MinorImageVersion = reader.ReadUInt16();
            optionalHeader.MajorSubsystemVersion = reader.ReadUInt16();
            optionalHeader.MinorSubsystemVersion = reader.ReadUInt16();
            optionalHeader.Win32VersionValue = reader.ReadUInt32();
            optionalHeader.SizeOfImage = reader.ReadUInt32();
            optionalHeader.SizeOfHeaders = reader.ReadUInt32();
            optionalHeader.CheckSum = reader.ReadUInt32();
            optionalHeader.Subsystem = (Subsystem)reader.ReadUInt16();
            optionalHeader.DllCharacteristics = (DllCharacteristics)reader.ReadUInt16();

            if (peMagic == PEMagic.NT32)
            {
                optionalHeader.SizeOfStackReserve = reader.ReadUInt32();
                optionalHeader.SizeOfStackCommit = reader.ReadUInt32();
                optionalHeader.SizeOfHeapReserve = reader.ReadUInt32();
                optionalHeader.SizeOfHeapCommit = reader.ReadUInt32();
            }
            else
            {
                optionalHeader.SizeOfStackReserve = reader.ReadUInt64();
                optionalHeader.SizeOfStackCommit = reader.ReadUInt64();
                optionalHeader.SizeOfHeapReserve = reader.ReadUInt64();
                optionalHeader.SizeOfHeapCommit = reader.ReadUInt64();
            }

            optionalHeader.LoaderFlags = reader.ReadUInt32();
            optionalHeader.NumberOfRvaAndSizes = reader.ReadUInt32();

            for (int i = 0; i < optionalHeader.DataDirectories.Length; i++)
            {
                optionalHeader.DataDirectories[i] = new DataDirectory
                {
                    VirtualAddress = reader.ReadUInt32(),
                    Size = reader.ReadUInt32()
                };
            }
        }

        static void ReadSectionHeader(BinaryStreamReader reader, Section section)
        {
            section.Name = reader.ReadFixedZeroFilledUtf8String(8);
            section.VirtualSize = reader.ReadUInt32();
            section.VirtualAddress = reader.ReadUInt32();
            section.SizeOfRawData = reader.ReadUInt32();
            section.PointerToRawData = reader.ReadUInt32();
            section.PointerToRelocations = reader.ReadUInt32();
            section.PointerToLinenumbers = reader.ReadUInt32();
            section.NumberOfRelocations = reader.ReadUInt16();
            section.NumberOfLinenumbers = reader.ReadUInt16();
            section.Characteristics = (SectionCharacteristics)reader.ReadUInt32();
        }
    }
}