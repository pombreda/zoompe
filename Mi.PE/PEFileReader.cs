﻿using System;
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

        public PEFile ReadMetadata(Stream stream)
        {
            var reader = new BinaryStreamReader(stream, this.buffer);
            return ReadMetadata(reader);
        }

        public bool PopulateSectionContent { get; set; }

        public static PEFile ReadMetadata(BinaryStreamReader reader)
        {
            var dosHeader = ReadDosHeader(reader);
            
            byte[] dosStub;
            if (dosHeader.lfanew > DosHeader.Size)
            {
                dosStub = new byte[dosHeader.lfanew - DosHeader.Size];
                reader.ReadBytes(dosStub, 0, dosStub.Length);
            }
            else
            {
                dosStub = null;
            }

            reader.Position = dosHeader.lfanew;
            var peHeader = ReadPEHeader(reader);
            var optionalHeader = ReadOptionalHeader(reader);
            SectionHeader[] sectionHeaders = new SectionHeader[peHeader.NumberOfSections];
            for (int i = 0; i < sectionHeaders.Length; i++)
            {
                sectionHeaders[i] = ReadSectionHeader(reader);
            }

            return new PEFile
            {
                DosHeader = dosHeader,
                DosStub = dosStub,
                PEHeader = peHeader,
                OptionalHeader = optionalHeader,
                SectionHeaders = sectionHeaders
            };
        }

        private static DosHeader ReadDosHeader(BinaryStreamReader reader)
        {
            var mz = (MZSignature)reader.ReadInt16();
            if (mz != MZSignature.MZ)
                throw new BadImageFormatException("MZ signature expected, "+Format.ToString((ushort)mz)+" found.");

            return new DosHeader
            {
                Signature = mz,
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

        private static PEHeader ReadPEHeader(BinaryStreamReader reader)
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

        private static OptionalHeader ReadOptionalHeader(BinaryStreamReader reader)
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

        private static SectionHeader ReadSectionHeader(BinaryStreamReader reader)
        {
            // TODO: intern well-known strings?
            return new SectionHeader
            {
                Name = reader.ReadFixedZeroFilledUtf8String(8),
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