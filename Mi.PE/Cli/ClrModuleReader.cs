using System;
using System.Collections.Generic;
using System.Linq;

namespace Mi.PE.Cli
{
    using Mi.PE.Cli.Tables;
    using Mi.PE.Internal;
    using Mi.PE.PEFormat;

    public sealed class ClrModuleReader
    {
        public const uint ClrHeaderSize = 72;

        readonly BinaryStreamReader m_Binary;
        readonly ClrModule module;
        
        Guid[] guids;

        ModuleEntry[] moduleTable;
        TypeRefEntry[] typeRefTable;
        TypeDefEntry[] typeDefTable;

        private ClrModuleReader(BinaryStreamReader binaryReader, ClrModule module)
        {
            this.m_Binary = binaryReader;
            this.module = module;
        }

        public static void Read(BinaryStreamReader reader, ClrModule module)
        {
            var modReader = new ClrModuleReader(reader, module);
            modReader.Read();
        }

        void Read()
        {
            // CLR header
            uint cb = this.Binary.ReadUInt32();

            if (cb < ClrHeaderSize)
                throw new BadImageFormatException(
                    "Unexpectedly short CLR header structure " + cb + " reported by Cb field " +
                    "(expected at least " + ClrHeaderSize + ").");

            this.module.RuntimeVersion = new Version(this.Binary.ReadUInt16(), this.Binary.ReadUInt16());

            var metadataDir = new DataDirectory();
            metadataDir.Read(this.Binary);

            this.module.ImageFlags = (ClrImageFlags)this.Binary.ReadInt32();

            uint entryPointToken = this.Binary.ReadUInt32();

            var resourcesDir = new DataDirectory();
            resourcesDir.Read(this.Binary);

            var strongNameSignatureDir = new DataDirectory();
            strongNameSignatureDir.Read(this.Binary);

            var codeManagerTableDir = new DataDirectory();
            codeManagerTableDir.Read(this.Binary);

            var vtableFixupsDir = new DataDirectory();
            vtableFixupsDir.Read(this.Binary);

            var exportAddressTableJumpsDir = new DataDirectory();
            exportAddressTableJumpsDir.Read(this.Binary);

            var managedNativeHeaderDir = new DataDirectory();
            managedNativeHeaderDir.Read(this.Binary);


            // CLR metadata
            this.Binary.Position = metadataDir.VirtualAddress;

            var mdSignature = (ClrMetadataSignature)this.Binary.ReadUInt32();
            if (mdSignature != ClrMetadataSignature.Signature)
                throw new InvalidOperationException("Invalid CLR metadata signature field " + ((uint)mdSignature).ToString("X") + "h.");

            this.module.MetadataVersion = new Version(
                this.Binary.ReadUInt16(),
                this.Binary.ReadUInt16());

            uint mdReserved = this.Binary.ReadUInt32();

            int versionLength = this.Binary.ReadInt32();
            string versionString = this.Binary.ReadFixedZeroFilledAsciiString(versionLength);

            this.module.MetadataVersionString = versionString;

            short mdFlags = this.Binary.ReadInt16();

            ushort streamCount = this.Binary.ReadUInt16();
            var streamHeaders = new StreamHeader[streamCount];

            for (int i = 0; i < streamHeaders.Length; i++)
            {
                streamHeaders[i] = new StreamHeader();
                streamHeaders[i].Read(this.Binary);
            }

            this.guids = null;
            this.module.TableStreamVersion = null;

            StreamHeader tableStreamHeader = null;
            foreach (var sh in streamHeaders)
            {
                this.Binary.Position = metadataDir.VirtualAddress + sh.Offset;

                switch (sh.Name)
                {
                    case "#GUID":
                        this.guids = new Guid[sh.Size / 128];
                        ReadGuids(this.Binary, this.guids);
                        break;

                    case "#~":
                    case "#-":
                        tableStreamHeader = sh;
                        break;

                    default:
                        break;
                }
            }

            this.Binary.Position = metadataDir.VirtualAddress + tableStreamHeader.Offset;
            this.ReadTableStream();
        }

        public BinaryStreamReader Binary { get { return m_Binary; } }

        public string ReadString()
        {
            throw new NotImplementedException();
        }

        public Guid? ReadGuid()
        {
            throw new NotImplementedException();
        }

        public byte[] ReadBlob()
        {
            throw new NotImplementedException();
        }

        public Version ReadVersion()
        {
            ushort major = this.Binary.ReadUInt16();
            ushort minor = this.Binary.ReadUInt16();
            ushort buildNumber = this.Binary.ReadUInt16();
            ushort revisionNumber = this.Binary.ReadUInt16();
            return new Version(major, minor, buildNumber, revisionNumber);
        }

        public HasConstant ReadHasConstant()
        {
            throw new NotImplementedException();
        }

        static void ReadGuids(BinaryStreamReader reader, Guid[] guids)
        {
            byte[] buf = new byte[128];
            for (int i = 0; i < guids.Length; i++)
            {
                reader.ReadBytes(buf, 0, buf.Length);
                guids[i] = new Guid(buf);
            }
        }


        public HasCustomAttribute ReadHasCustomAttribute()
        {
            throw new NotImplementedException();
        }

        void ReadTableStream()
        {
            int tsReserved0 = this.Binary.ReadInt32();
            byte tsMajorVersion = this.Binary.ReadByte();
            byte tsMinorVersion = this.Binary.ReadByte();

            module.TableStreamVersion = new Version(tsMajorVersion, tsMinorVersion);

            byte tsHeapSizes = this.Binary.ReadByte();
            byte tsReserved1 = this.Binary.ReadByte();
            ulong tsValid = this.Binary.ReadUInt64();
            ulong tsSorted = this.Binary.ReadUInt64();

            for (int i = 0; i < 64; i++)
            {
                if ((tsValid & (1UL << i)) == 0)
                    continue;

                uint rowCount = this.Binary.ReadUInt32();


                switch ((TableKind)i)
                {
                    case TableKind.Module: // 0x0
                        this.moduleTable = new ModuleEntry[rowCount];
                        break;

                    case TableKind.TypeRef: // 0x1
                        this.typeRefTable = new TypeRefEntry[rowCount];
                        break;

                    case TableKind.TypeDef: // 0x2
                        this.typeDefTable = new TypeDefEntry[rowCount];
                        break;

                    case TableKind.Field: // 0x4
                        break;

                    case TableKind.MethodDef: // 0x6
                        break;

                    case TableKind.Param: // 0x8
                        break;

                    case TableKind.InterfaceImpl: // 0x9
                        break;

                    case TableKind.MemberRef: // 0xA
                        break;

                    case TableKind.Constant: // 0xB
                        break;

                    case TableKind.CustomAttribute: // 0xC
                        break;

                    case TableKind.FieldMarshal: // 0xD
                        break;

                    case TableKind.DeclSecurity: // 0xE
                        break;

                    case TableKind.ClassLayout: // 0xF
                        break;

                    case TableKind.FieldLayout: // 0x10
                        break;

                    case TableKind.StandAloneSig: // 0x11
                        break;

                    case TableKind.EventMap: // 0x12
                        break;

                    case TableKind.Event: // 0x14
                        break;

                    case TableKind.PropertyMap: // 0x15
                        break;

                    case TableKind.Property: // 0x17
                        break;

                    case TableKind.MethodSemantics: // 0x18
                        break;

                    case TableKind.MethodImpl: // 0x19
                        break;

                    case TableKind.ModuleRef: // 0x1A
                        break;

                    case TableKind.TypeSpec: // 0x1B
                        break;

                    case TableKind.ImplMap: // 0x1C
                        break;

                    case TableKind.FieldRVA: // 0x1D
                        break;

                    case TableKind.AssemblyProcessor: // 0x21
                        break;

                    case TableKind.AssemblyOS: // 0x22
                        break;

                    case TableKind.AssemblyRef: // 0x23
                        break;

                    case TableKind.AssemblyRefProcessor: // 0x24
                        break;

                    case TableKind.AssemblyRefOS: // 0x25
                        break;

                    case TableKind.File: // 0x26
                        break;

                    case TableKind.ExportedType: // 0x27
                        break;

                    case TableKind.ManifestResource: // 0x28
                        break;

                    case TableKind.NestedClass: // 0x29
                        break;

                    case TableKind.GenericParam: // 0x2A
                        break;

                    case TableKind.MethodSpec: // 0x2B
                        break;

                    case TableKind.GenericParamConstraint: // 0x2C
                        break;

                    default:
                        break;
                }
            }

            for (int iTable = 0; iTable < 64; iTable++)
            {
            }
        }

        public TypeDefOrRef ReadTypeDefOrRef()
        {
            throw new NotImplementedException();
        }

        public uint ReadFieldIndex()
        {
            throw new NotImplementedException();
        }

        public uint ReadMethodIndex()
        {
            throw new NotImplementedException();
        }
    }
}