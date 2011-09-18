﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Mi.PE.PEFormat;
using System.Reflection;

namespace Mi.PE
{
    [TestClass]
    public class PEFileReaderTests
    {
        static readonly byte[] DefaultStub = new byte[]
        {
            0x0E, 0x1F, 0xBA, 0x0E, 0x00, 0xB4, 0x09, 0xCD, 0x21, 0xB8, 0x01, 0x4C, 0xCD, 0x21,
            (byte)'T', (byte)'h', (byte)'i', (byte)'s', (byte)' ',
            (byte)'p', (byte)'r', (byte)'o', (byte)'g', (byte)'r', (byte)'a', (byte)'m', (byte)' ',
            (byte)'c', (byte)'a', (byte)'n', (byte)'n', (byte)'o', (byte)'t', (byte)' ',
            (byte)'b', (byte)'e', (byte)' ',
            (byte)'r', (byte)'u', (byte)'n', (byte)' ',
            (byte)'i', (byte)'n', (byte)' ',
            (byte)'D', (byte)'O', (byte)'S', (byte)' ',
            (byte)'m', (byte)'o', (byte)'d', (byte)'e', (byte)'.',
            (byte)'\r', (byte)'\r', (byte)'\n', (byte)'$',
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        };

        [TestMethod]
        public void CallConstructor()
        {
            var reader = new PEFileReader();
        }

        [ExpectedException(typeof(BadImageFormatException))]
        [TestMethod]
        public void ReadDosHeader_InvalidMZ()
        {
            var reader = new PEFileReader();
            reader.ReadMetadata(new MemoryStream(new byte[10]));
        }

        [TestMethod]
        public void ReadDosHeader_NoDosStub()
        {
            byte[] bytes = Properties.Resources.console_anycpu;
            
            uint lfaNew = (uint)BitConverter.ToInt32(bytes, DosHeader.Size - 4);
            byte[] modifiedLfaNewBytes = BitConverter.GetBytes(DosHeader.Size);

            Array.Copy(
                modifiedLfaNewBytes, 0,
                bytes, DosHeader.Size - 4,
                4);

            Array.Copy(
                bytes, lfaNew,
                bytes, DosHeader.Size,
                bytes.Length - lfaNew);

            var stream = new MemoryStream(bytes, 0, bytes.Length - ((int)lfaNew - DosHeader.Size), false);

            var reader = new PEFileReader();
            var pe = reader.ReadMetadata(stream);

            Assert.AreEqual((uint)DosHeader.Size, pe.DosHeader.lfanew);
            Assert.AreEqual(0, pe.DosHeader.Stub.Length);
        }

        [ExpectedException(typeof(BadImageFormatException))]
        [TestMethod]
        public void ReadOptionalHeader_InvalidPEMagic()
        {
            byte[] bytes = Properties.Resources.console_anycpu;
            bytes[152] = 0;

            var reader = new PEFileReader();
            reader.ReadMetadata(new MemoryStream(bytes));
        }

        [TestMethod] public void PreReadAnyCPU_AssertDosStub() { AssertDosStub(PreReadSamplePEs.Console.AnyCPU.DosHeader.Stub); }
        [TestMethod] public void PreReadX86_AssertDosStub() { AssertDosStub(PreReadSamplePEs.Console.X86.DosHeader.Stub); }
        [TestMethod] public void PreReadX64_AssertDosStub() { AssertDosStub(PreReadSamplePEs.Console.X64.DosHeader.Stub); }
        [TestMethod] public void PreReadItanium_AssertDosStub() { AssertDosStub(PreReadSamplePEs.Console.Itanium.DosHeader.Stub); }
        [TestMethod] public void EmitAnyCPU_AssertDosStub() { AssertDosStub(EmitSamplePEs.Library.AnyCPU.DosHeader.Stub); }
        [TestMethod] public void EmitX86_AssertDosStub() { AssertDosStub(EmitSamplePEs.Library.X86.DosHeader.Stub); }
        [TestMethod] public void EmitX64_AssertDosStub() { AssertDosStub(EmitSamplePEs.Library.X64.DosHeader.Stub); }
        [TestMethod] public void EmitItanium_AssertDosStub() { AssertDosStub(EmitSamplePEs.Library.Itanium.DosHeader.Stub); }

        [TestMethod] public void PreReadAnyCPU_AssertDosHeader() { AssertDosHeader(PreReadSamplePEs.Console.AnyCPU.DosHeader); }
        [TestMethod] public void PreReadX86_AssertDosHeader() { AssertDosHeader(PreReadSamplePEs.Console.X86.DosHeader); }
        [TestMethod] public void PreReadX64_AssertDosHeader() { AssertDosHeader(PreReadSamplePEs.Console.X64.DosHeader); }
        [TestMethod] public void PreREadItanium_AssertDosHeader() { AssertDosHeader(PreReadSamplePEs.Console.Itanium.DosHeader); }
        [TestMethod] public void EmitAnyCPU_AssertDosHeader() { AssertDosHeader(EmitSamplePEs.Library.AnyCPU.DosHeader); }
        [TestMethod] public void EmitX86_AssertDosHeader() { AssertDosHeader(EmitSamplePEs.Library.X86.DosHeader); }
        [TestMethod] public void EmitX64_AssertDosHeader() { AssertDosHeader(EmitSamplePEs.Library.X64.DosHeader); }
        [TestMethod] public void EmitItanium_AssertDosHeader() { AssertDosHeader(EmitSamplePEs.Library.Itanium.DosHeader); }

        [TestMethod] public void EmitAnyCPU_AssertPEHeader() { AssertPEHeader(EmitSamplePEs.Library.AnyCPU.PEHeader, Machine.I386); }
        [TestMethod] public void Emitx86_AssertPEHeader() { AssertPEHeader(EmitSamplePEs.Library.X86.PEHeader, Machine.I386); }
        [TestMethod] public void Emitx64_AssertPEHeader() { AssertPEHeader(EmitSamplePEs.Library.X64.PEHeader, Machine.AMD64); }
        [TestMethod] public void EmitItanium_AssertPEHeader() { AssertPEHeader(EmitSamplePEs.Library.Itanium.PEHeader, Machine.IA64); }

        [TestMethod]
        public void EmitAnyCPU_AssertOptionalHeader()
        {
            var optionalHeader = EmitSamplePEs.Library.AnyCPU.OptionalHeader;
            AssertOptionalHeader(optionalHeader);
            Assert.AreEqual(PEMagic.NT32, optionalHeader.PEMagic);
            Assert.AreEqual((uint)512, optionalHeader.SizeOfCode);
            Assert.AreEqual((uint)8606, optionalHeader.AddressOfEntryPoint);
            Assert.AreEqual((uint)0x4000, optionalHeader.BaseOfData);
            Assert.AreEqual((uint)0x100000, optionalHeader.SizeOfStackReserve);
            Assert.AreEqual((uint)0x1000, optionalHeader.SizeOfStackCommit);
            Assert.AreEqual((uint)0x1000, optionalHeader.SizeOfHeapCommit);
            Assert.AreEqual((uint)512, optionalHeader.SizeOfInitializedData);
            Assert.AreEqual((uint)0x6000, optionalHeader.SizeOfImage);
        }

        [TestMethod]
        public void EmitX86_AssertOptionalHeader()
        {
            var optionalHeader = EmitSamplePEs.Library.X86.OptionalHeader;
            AssertOptionalHeader(optionalHeader);
            Assert.AreEqual(PEMagic.NT32, optionalHeader.PEMagic);
            Assert.AreEqual((uint)0x200, optionalHeader.SizeOfCode);
            Assert.AreEqual((uint)0x219e, optionalHeader.AddressOfEntryPoint);
            Assert.AreEqual((uint)0x4000, optionalHeader.BaseOfData);
            Assert.AreEqual((uint)0x100000, optionalHeader.SizeOfStackReserve);
            Assert.AreEqual((uint)0x1000, optionalHeader.SizeOfStackCommit);
            Assert.AreEqual((uint)0x1000, optionalHeader.SizeOfHeapCommit);
            Assert.AreEqual((uint)0x200, optionalHeader.SizeOfInitializedData);
            Assert.AreEqual((uint)0x6000, optionalHeader.SizeOfImage);
        }

        [TestMethod]
        public void EmitX64_AssertOptionalHeader()
        {
            var optionalHeader = EmitSamplePEs.Library.X64.OptionalHeader;
            AssertOptionalHeader(optionalHeader);
            Assert.AreEqual(PEMagic.NT64, optionalHeader.PEMagic);
            Assert.AreEqual((uint)512, optionalHeader.SizeOfCode);
            Assert.AreEqual((uint)0x21be, optionalHeader.AddressOfEntryPoint);
            Assert.AreEqual((uint)0, optionalHeader.BaseOfData);
            Assert.AreEqual((uint)0x400000, optionalHeader.SizeOfStackReserve);
            Assert.AreEqual((uint)0x4000, optionalHeader.SizeOfStackCommit);
            Assert.AreEqual((uint)0x2000, optionalHeader.SizeOfHeapCommit);
            Assert.AreEqual((uint)512, optionalHeader.SizeOfInitializedData);
            Assert.AreEqual((uint)0x6000, optionalHeader.SizeOfImage);
        }

        [TestMethod]
        public void EmitItanium_AssertOptionalHeader()
        {
            var optionalHeader = EmitSamplePEs.Library.Itanium.OptionalHeader;
            AssertOptionalHeader(optionalHeader);
            Assert.AreEqual(PEMagic.NT64, optionalHeader.PEMagic);
            Assert.AreEqual((uint)512, optionalHeader.SizeOfCode);
            Assert.AreEqual((uint)8672, optionalHeader.AddressOfEntryPoint);
            Assert.AreEqual((uint)0, optionalHeader.BaseOfData);
            Assert.AreEqual((uint)0x400000, optionalHeader.SizeOfStackReserve);
            Assert.AreEqual((uint)0x4000, optionalHeader.SizeOfStackCommit);
            Assert.AreEqual((uint)0x2000, optionalHeader.SizeOfHeapCommit);
            Assert.AreEqual((uint)512, optionalHeader.SizeOfInitializedData);
            Assert.AreEqual((uint)0x6000, optionalHeader.SizeOfImage);
        }


        [TestMethod]
        public void PreReadAnyCPU_AssertOptionalHeader()
        {
            var optionalHeader = PreReadSamplePEs.Console.AnyCPU.OptionalHeader;
            AssertOptionalHeader(optionalHeader);
            Assert.AreEqual(PEMagic.NT32, optionalHeader.PEMagic);
            Assert.AreEqual((uint)0x400, optionalHeader.SizeOfCode);
            Assert.AreEqual((uint)0x234e, optionalHeader.AddressOfEntryPoint);
            Assert.AreEqual((uint)0x4000, optionalHeader.BaseOfData);
            Assert.AreEqual((uint)0x100000, optionalHeader.SizeOfStackReserve);
            Assert.AreEqual((uint)0x1000, optionalHeader.SizeOfStackCommit);
            Assert.AreEqual((uint)0x1000, optionalHeader.SizeOfHeapCommit);
            Assert.AreEqual((uint)0x800, optionalHeader.SizeOfInitializedData);
            Assert.AreEqual((uint)0x8000, optionalHeader.SizeOfImage);
        }

        [TestMethod]
        public void PreReadX86_AssertOptionalHeader()
        {
            var optionalHeader = PreReadSamplePEs.Console.X86.OptionalHeader;
            AssertOptionalHeader(optionalHeader);
            Assert.AreEqual(PEMagic.NT32, optionalHeader.PEMagic);
            Assert.AreEqual((uint)0x400, optionalHeader.SizeOfCode);
            Assert.AreEqual((uint)0x233e, optionalHeader.AddressOfEntryPoint);
            Assert.AreEqual((uint)0x4000, optionalHeader.BaseOfData);
            Assert.AreEqual((uint)0x100000, optionalHeader.SizeOfStackReserve);
            Assert.AreEqual((uint)0x1000, optionalHeader.SizeOfStackCommit);
            Assert.AreEqual((uint)0x1000, optionalHeader.SizeOfHeapCommit);
            Assert.AreEqual((uint)0x800, optionalHeader.SizeOfInitializedData);
            Assert.AreEqual((uint)0x8000, optionalHeader.SizeOfImage);
        }

        [TestMethod]
        public void PreReadX64_AssertOptionalHeader()
        {
            var optionalHeader = PreReadSamplePEs.Console.X64.OptionalHeader;
            AssertOptionalHeader(optionalHeader);
            Assert.AreEqual(PEMagic.NT64, optionalHeader.PEMagic);
            Assert.AreEqual((uint)0x400, optionalHeader.SizeOfCode);
            Assert.AreEqual((uint)0, optionalHeader.AddressOfEntryPoint);
            Assert.AreEqual((uint)0, optionalHeader.BaseOfData);
            Assert.AreEqual((uint)0x400000, optionalHeader.SizeOfStackReserve);
            Assert.AreEqual((uint)0x4000, optionalHeader.SizeOfStackCommit);
            Assert.AreEqual((uint)0x2000, optionalHeader.SizeOfHeapCommit);
            Assert.AreEqual((uint)0x600, optionalHeader.SizeOfInitializedData);
            Assert.AreEqual((uint)0x6000, optionalHeader.SizeOfImage);
        }

        [TestMethod]
        public void PreReadItanium_AssertOptionalHeader()
        {
            var optionalHeader = PreReadSamplePEs.Console.Itanium.OptionalHeader;
            AssertOptionalHeader(optionalHeader);
            Assert.AreEqual(PEMagic.NT64, optionalHeader.PEMagic);
            Assert.AreEqual((uint)0x400, optionalHeader.SizeOfCode);
            Assert.AreEqual((uint)0, optionalHeader.AddressOfEntryPoint);
            Assert.AreEqual((uint)0, optionalHeader.BaseOfData);
            Assert.AreEqual((uint)0x400000, optionalHeader.SizeOfStackReserve);
            Assert.AreEqual((uint)0x4000, optionalHeader.SizeOfStackCommit);
            Assert.AreEqual((uint)0x2000, optionalHeader.SizeOfHeapCommit);
            Assert.AreEqual((uint)0x600, optionalHeader.SizeOfInitializedData);
            Assert.AreEqual((uint)0x6000, optionalHeader.SizeOfImage);
        }

        [TestMethod]
        public void PopulateSectionContent()
        {
            var reader = new PEFileReader { PopulateSectionContent = true };

            var pe = reader.ReadMetadata(new MemoryStream(Properties.Resources.console_anycpu));

            Assert.AreEqual(3, pe.Sections.Length);
            Assert.AreEqual((byte)72, pe.Sections[0].Content[8]);
        }

        static void AssertOptionalHeader(OptionalHeader optionalHeader)
        {
            Assert.AreEqual((byte)8, optionalHeader.MajorLinkerVersion);
            Assert.AreEqual((byte)0, optionalHeader.MinorLinkerVersion);
            Assert.AreEqual((uint)0, optionalHeader.SizeOfUninitializedData);
            Assert.AreEqual((uint)8192, optionalHeader.BaseOfCode);
            Assert.AreEqual((uint)0x400000, optionalHeader.ImageBase);
            Assert.AreEqual((uint)8192, optionalHeader.SectionAlignment);
            Assert.AreEqual((uint)512, optionalHeader.FileAlignment);
            Assert.AreEqual((ushort)4, optionalHeader.MajorOperatingSystemVersion);
            Assert.AreEqual((ushort)0, optionalHeader.MinorOperatingSystemVersion);
            Assert.AreEqual((ushort)0, optionalHeader.MajorImageVersion);
            Assert.AreEqual((ushort)0, optionalHeader.MinorImageVersion);
            Assert.AreEqual((ushort)4, optionalHeader.MajorSubsystemVersion);
            Assert.AreEqual((ushort)0, optionalHeader.MinorSubsystemVersion);
            Assert.AreEqual((uint)0, optionalHeader.Win32VersionValue);
            Assert.AreEqual((uint)512, optionalHeader.SizeOfHeaders);
            Assert.AreEqual((uint)0, optionalHeader.CheckSum);
            Assert.AreEqual(Subsystem.WindowsCUI, optionalHeader.Subsystem);
            Assert.AreEqual(DllCharacteristics.DynamicBase | DllCharacteristics.NxCompatible | DllCharacteristics.NoSEH | DllCharacteristics.TerminalServerAware, optionalHeader.DllCharacteristics);
            Assert.AreEqual((uint)0x100000, optionalHeader.SizeOfHeapReserve);
            Assert.AreEqual((uint)0, optionalHeader.LoaderFlags);
            Assert.AreEqual((uint)16, optionalHeader.NumberOfRvaAndSizes);
        }

        static void AssertPEHeader(PEHeader header, Machine machine)
        {
            Assert.AreEqual(PESignature.PE00, header.PESignature);
            Assert.AreEqual(machine, header.Machine);
            Assert.AreEqual((ushort)2, header.NumberOfSections);
            Assert.IsTrue(Math.Abs((DateTime.UtcNow - header.Timestamp.ToDateTime()).TotalHours) < 2, "PE timestamp is off by more than 2 hours.");
            Assert.AreEqual((uint)0, header.PointerToSymbolTable);
            Assert.AreEqual((uint)0, header.NumberOfSymbols);

            uint expectedSizeOfOptionalHeader = machine == Machine.I386 ?
                (uint)224 :
                (uint)240;
            Assert.AreEqual(expectedSizeOfOptionalHeader, header.SizeOfOptionalHeader);

            ImageCharacteristics expectedCharacteristics = machine == Machine.I386 ?
                ImageCharacteristics.ExecutableImage | ImageCharacteristics.Bit32Machine | ImageCharacteristics.Dll :
                ImageCharacteristics.ExecutableImage | ImageCharacteristics.LargeAddressAware | ImageCharacteristics.Dll;

            Assert.AreEqual(expectedCharacteristics, header.Characteristics);
        }

        static void AssertDosHeader(DosHeader dosHeader)
        {
            Assert.AreEqual(MZSignature.MZ, dosHeader.Signature);
            Assert.AreEqual(144, dosHeader.cblp);
            Assert.AreEqual(3, dosHeader.cp);
            Assert.AreEqual(0, dosHeader.crlc);
            Assert.AreEqual(4, dosHeader.cparhdr);
            Assert.AreEqual(0, dosHeader.minalloc);
            Assert.AreEqual(65535, dosHeader.maxalloc);
            Assert.AreEqual(0, dosHeader.ss);
            Assert.AreEqual(184, dosHeader.sp);
            Assert.AreEqual(0, dosHeader.csum);
            Assert.AreEqual(0, dosHeader.ip);
            Assert.AreEqual(0, dosHeader.cs);
            Assert.AreEqual(64, dosHeader.lfarlc);
            Assert.AreEqual(0, dosHeader.ovno);
            Assert.AreEqual((ulong)0, dosHeader.res1);
            Assert.AreEqual(0, dosHeader.oemid);
            Assert.AreEqual(0, dosHeader.oeminfo);
            Assert.AreEqual((uint)0, dosHeader.ReservedNumber0);
            Assert.AreEqual((uint)0, dosHeader.ReservedNumber1);
            Assert.AreEqual((uint)0, dosHeader.ReservedNumber2);
            Assert.AreEqual((uint)0, dosHeader.ReservedNumber3);
            Assert.AreEqual((uint)0, dosHeader.ReservedNumber4);
            Assert.AreEqual((uint)128, dosHeader.lfanew);
        }

        static void AssertDosStub(byte[] dosStub)
        {
            Assert.AreEqual(DefaultStub.Length, dosStub.Length, "DOS stub of wrong size.");
            for (int i = 0; i < dosStub.Length; i++)
            {
                Assert.AreEqual(DefaultStub[i], dosStub[i], "DOS stub[" + i + "]");
            }
        }
    }
}