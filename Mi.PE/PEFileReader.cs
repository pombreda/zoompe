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

        public PEFile ReadMetadata(Stream stream, bool readSectionContent)
        {
            var reader = new BinaryStreamReader(stream, this.buffer);
            return ReadMetadata(reader, readSectionContent);
        }

        public static PEFile ReadMetadata(BinaryStreamReader reader, bool readSectionContent)
        {
            var dosHeader = ReadDosHeader(reader);
            reader.Position = dosHeader.lfanew;
            var peHeader = ReadPEHeader(reader);
            var optionalHeader = ReadOptionalHeader(reader);
            SectionHeader[] sectionHeaders = new SectionHeader[peHeader.NumberOfSections];
            for (int i = 0; i < sectionHeaders.Length; i++)
            {
                sectionHeaders[i] = ReadSectionHeader(reader);
            }

            if (readSectionContent)
            {
                for (int i = 0; i < sectionHeaders.Length; i++)
                {
                    reader.Position = sectionHeaders[i].PhysicalAddress;
                    byte[] buf = new byte[sectionHeaders[i].VirtualSize];
                    reader.ReadBytes(buf);
                    sectionHeaders[i].Content = buf;
                }
            }

            return new PEFile
            {
                DosHeader = dosHeader,
                PEHeader = peHeader,
                OptionalHeader = optionalHeader,
                SectionHeaders = sectionHeaders
            };
        }

        public static DosHeader ReadDosHeader(BinaryStreamReader reader)
        {
            return new DosHeader
            {
                Signature = (MZSignature)reader.ReadInt16(),
                cblp = reader.ReadUInt16(),
                cp = reader.ReadUInt16(),
                crlc = reader.ReadUInt16(),
                cparhdr = reader.ReadUInt16(),
                minalloc = reader.ReadUInt16(),
                maxalloc = reader.ReadUInt16(),
                ss = reader.ReadUInt16(),
                sp = reader.ReadUInt16(),
                csum = reader.ReadUInt16(),
                ip = reader.ReadUInt16(),
                cs = reader.ReadUInt16(),
                lfarlc = reader.ReadUInt16(),
                ovno = reader.ReadUInt16(),

                res1 = reader.ReadUInt64(),

                oemid = reader.ReadUInt16(),
                oeminfo = reader.ReadUInt16(),

                ReservedNumber0 = reader.ReadUInt32(),
                ReservedNumber1 = reader.ReadUInt32(),
                ReservedNumber2 = reader.ReadUInt32(),
                ReservedNumber3 = reader.ReadUInt32(),
                ReservedNumber4 = reader.ReadUInt32(),

                lfanew = reader.ReadUInt32()
            };
        }

        public static PEHeader ReadPEHeader(BinaryStreamReader reader)
        {
            return new PEHeader
            {
                PESignature = (PESignature)reader.ReadInt32(),
                Machine = (Machine)reader.ReadInt16(),
                NumberOfSections = reader.ReadUInt16(),
                Timestamp = new ImageTimestamp(reader.ReadUInt32()),
                PointerToSymbolTable = reader.ReadUInt32(),
                NumberOfSymbols = reader.ReadUInt32(),
                SizeOfOptionalHeader = reader.ReadUInt16(),
                Characteristics = (ImageCharacteristics)reader.ReadInt16()
            };
        }

        public static OptionalHeader ReadOptionalHeader(BinaryStreamReader reader)
        {
            var peMagic = (PEMagic)reader.ReadInt16();

            if (peMagic != PEMagic.NT32
                && peMagic != PEMagic.NT64)
                throw new BadImageFormatException("Unsupported PE magic value " + peMagic + ".");

            var optionalHeader = new OptionalHeader();
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

            optionalHeader.SectionAlignment = reader.ReadInt32();
            optionalHeader.FileAlignment = reader.ReadUInt32();
            optionalHeader.MajorOperatingSystemVersion = reader.ReadUInt16();
            optionalHeader.MinorOperatingSystemVersion = reader.ReadUInt16();
            optionalHeader.MajorImageVersion = reader.ReadUInt16();
            optionalHeader.MinorImageVersion = reader.ReadUInt16();
            optionalHeader.MajorSubsystemVersion = reader.ReadUInt16();
            optionalHeader.MinorSubsystemVersion = reader.ReadUInt16();
            optionalHeader.Win32VersionValue = reader.ReadUInt32();
            optionalHeader.SizeOfImage = reader.ReadInt32();
            optionalHeader.SizeOfHeaders = reader.ReadInt32();
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
            optionalHeader.NumberOfRvaAndSizes = reader.ReadInt32();

            if (optionalHeader.NumberOfRvaAndSizes > 0)
            {
                var directories = new DataDirectory[optionalHeader.NumberOfRvaAndSizes];
                for (int i = 0; i < directories.Length; i++)
                {
                    directories[i] = new DataDirectory
                    {
                        VirtualAddress = reader.ReadUInt32(),
                        Size = reader.ReadUInt32()
                    };
                }
                optionalHeader.DataDirectories = directories;
            }

            return optionalHeader;
        }

        public static SectionHeader ReadSectionHeader(BinaryStreamReader reader)
        {
            // TODO: intern well-known strings?
            return new SectionHeader
            {
                Name = reader.ReadFixedZeroFilledString(8),
                VirtualSize = reader.ReadUInt32(),
                VirtualAddress = reader.ReadUInt32(),
                SizeOfRawData = reader.ReadUInt32(),
                PointerToRawData = reader.ReadUInt32(),
                PointerToRelocations = reader.ReadUInt32(),
                PointerToLinenumbers = reader.ReadUInt32(),
                NumberOfRelocations = reader.ReadUInt16(),
                NumberOfLinenumbers = reader.ReadUInt16(),
                Characteristics = (SectionCharacteristics)reader.ReadUInt32()
            };
        }
    }
}