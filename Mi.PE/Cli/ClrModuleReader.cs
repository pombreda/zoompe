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
        TableStream tableStream;
        
        Guid[] guids;

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
            tableStream = new TableStream();
            tableStream.Read(this);
        }

        uint ReadTableIndexByLength(uint length)
        {
            throw new NotImplementedException();
        }

        public TypeDefOrRef ReadTypeDefOrRef()
        {
            uint maxCount = Math.Max(
                Math.Max(
                    (uint)tableStream.Tables[(int)TableKind.TypeDef].Length,
                    (uint)tableStream.Tables[(int)TableKind.TypeRef].Length),
                (uint)tableStream.Tables[(int)TableKind.TypeSpec].Length);

            return (TypeDefOrRef)ReadTableIndexByLength(maxCount);
        }

        public MemberRefParent ReadMemberRefParent()
        {
            uint maxCount = Math.Max(
                Math.Max(
                    Math.Max(
                        (uint)tableStream.Tables[(int)TableKind.TypeDef].Length,
                        (uint)tableStream.Tables[(int)TableKind.TypeRef].Length),
                    (uint)tableStream.Tables[(int)TableKind.TypeSpec].Length),
                Math.Max(
                    (uint)tableStream.Tables[(int)TableKind.MethodDef].Length,
                    (uint)tableStream.Tables[(int)TableKind.ModuleRef].Length));

            return (MemberRefParent)ReadTableIndexByLength(maxCount);
        }

        public CustomAttributeType ReadCustomAttributeType()
        {
            uint maxCount = Math.Max(
                (uint)tableStream.Tables[(int)TableKind.MethodDef].Length,
                (uint)tableStream.Tables[(int)TableKind.ModuleRef].Length);

            return (CustomAttributeType)ReadTableIndexByLength(maxCount);
        }

        public HasFieldMarshal ReadHasFieldMarshal()
        {
            uint maxCount = Math.Max(
                (uint)tableStream.Tables[(int)TableKind.Field].Length,
                (uint)tableStream.Tables[(int)TableKind.Param].Length);

            return (HasFieldMarshal)ReadTableIndexByLength(maxCount);
        }

        public HasDeclSecurity ReadHasDeclSecurity()
        {
            uint maxCount = Math.Max(
                Math.Max(
                    (uint)tableStream.Tables[(int)TableKind.TypeDef].Length,
                    (uint)tableStream.Tables[(int)TableKind.MethodDef].Length),
                (uint)tableStream.Tables[(int)TableKind.AssemblyOS].Length);

            return (HasDeclSecurity)ReadTableIndexByLength(maxCount);
        }

        public HasSemantics ReadHasSemantics()
        {
            uint maxCount = Math.Max(
                (uint)tableStream.Tables[(int)TableKind.Event].Length,
                (uint)tableStream.Tables[(int)TableKind.Property].Length);

            return (HasSemantics)ReadTableIndexByLength(maxCount);
        }

        public MethodDefOrRef ReadMethodDefOrRef()
        {
            uint maxCount = Math.Max(
                (uint)tableStream.Tables[(int)TableKind.MethodDef].Length,
                (uint)tableStream.Tables[(int)TableKind.MemberRef].Length);

            return (MethodDefOrRef)ReadTableIndexByLength(maxCount);
        }

        public MemberForwarded ReadMemberForwarded()
        {
            uint maxCount = Math.Max(
                (uint)tableStream.Tables[(int)TableKind.Field].Length,
                (uint)tableStream.Tables[(int)TableKind.MethodDef].Length);

            return (MemberForwarded)ReadTableIndexByLength(maxCount);
        }

        public Implementation ReadImplementation()
        {
            uint maxCount = Math.Max(
                Math.Max(
                    (uint)tableStream.Tables[(int)TableKind.Field].Length,
                    (uint)tableStream.Tables[(int)TableKind.AssemblyRef].Length),
                (uint)tableStream.Tables[(int)TableKind.ExportedType].Length);

            return (Implementation)ReadTableIndexByLength(maxCount);
        }

        public TypeOrMethodDef ReadTypeOrMethodDef()
        {
            uint maxCount = Math.Max(
                (uint)tableStream.Tables[(int)TableKind.TypeDef].Length,
                (uint)tableStream.Tables[(int)TableKind.MethodDef].Length);

            return (TypeOrMethodDef)ReadTableIndexByLength(maxCount);
        }

        public uint ReadTableIndex(TableKind table)
        {
            return ReadTableIndexByLength((uint)this.tableStream.Tables[(int)table].Length);
        }
    }
}