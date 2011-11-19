using System;
using System.Collections.Generic;
using System.Linq;

namespace Mi.PE.Cli
{
    using Mi.PE.Cli.Tables;
    using Mi.PE.Internal;
    using Mi.PE.PEFormat;

    public class ClrModuleReader
    {
        public const uint ClrHeaderSize = 72;

        readonly BinaryStreamReader m_Binary;

        public ClrModuleReader(BinaryStreamReader binaryReader)
        {
            this.m_Binary = binaryReader;
        }

        public void Read(ClrModule module)
        {
            // CLR header
            uint cb = this.Binary.ReadUInt32();

            if (cb < ClrHeaderSize)
                throw new BadImageFormatException(
                    "Unexpectedly short CLR header structure " + cb + " reported by Cb field " +
                    "(expected at least " + ClrHeaderSize + ").");

            module.RuntimeVersion = new Version(this.Binary.ReadUInt16(), this.Binary.ReadUInt16());

            var metadataDir = new DataDirectory();
            metadataDir.Read(this.Binary);

            module.ImageFlags = (ClrImageFlags)this.Binary.ReadInt32();

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

            module.MetadataVersion = new Version(
                this.Binary.ReadUInt16(),
                this.Binary.ReadUInt16());

            uint mdReserved = this.Binary.ReadUInt32();

            int versionLength = this.Binary.ReadInt32();
            string versionString = this.Binary.ReadFixedZeroFilledAsciiString(versionLength);

            module.MetadataVersionString = versionString;

            short mdFlags = this.Binary.ReadInt16();

            ushort streamCount = this.Binary.ReadUInt16();
            var streamHeaders = new StreamHeader[streamCount];

            for (int i = 0; i < streamHeaders.Length; i++)
            {
                streamHeaders[i] = new StreamHeader();
                streamHeaders[i].Read(this.Binary);
            }

            module.Guids = null;
            module.TableStreamVersion = null;

            ModuleEntry[] moduleEntries = null;

            foreach (var sh in streamHeaders)
            {
                this.Binary.Position = metadataDir.VirtualAddress + sh.Offset;

                switch (sh.Name)
                {
                    case "#GUID":
                        module.Guids = new Guid[sh.Size / 128];
                        ReadGuids(this.Binary, module.Guids);
                        break;

                    case "#~":
                    case "#-":
                        var ts = new TableStream();
                        ts.Read(null);
                        break;

                    default:
                        break;
                }
            }

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

        static void ReadGuids(BinaryStreamReader reader, Guid[] guids)
        {
            byte[] buf = new byte[128];
            for (int i = 0; i < guids.Length; i++)
            {
                reader.ReadBytes(buf, 0, buf.Length);
                guids[i] = new Guid(buf);
            }
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

        public HasCustomAttribute ReadHasCustomAttribute()
        {
            throw new NotImplementedException();
        }
    }
}