using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mi.PE.Cli
{
    using Mi.PE.Internal;
    using Mi.PE.PEFormat;

    public sealed class ClrModule
    {
        const uint ClrHeaderSize = 72;

        public Version RuntimeVersion;
        public ClrImageFlags ImageFlags;
        public Version MetadataVersion;
        public string MetadataVersionString;
        public Version TableStreamVersion;

        public void Read(BinaryStreamReader reader)
        {
            // CLR header
            uint cb = reader.ReadUInt32();

            if (cb < ClrHeaderSize)
                throw new BadImageFormatException(
                    "Unexpectedly short CLR header structure " + cb + " reported by Cb field " +
                    "(expected at least " + ClrHeaderSize + ").");

            this.RuntimeVersion = new Version(reader.ReadUInt16(), reader.ReadUInt16());

            var metadataDir = new DataDirectory();
            metadataDir.Read(reader);

            this.ImageFlags = (ClrImageFlags)reader.ReadInt32();

            uint entryPointToken = reader.ReadUInt32();

            var resourcesDir = new DataDirectory();
            resourcesDir.Read(reader);

            var strongNameSignatureDir = new DataDirectory();
            strongNameSignatureDir.Read(reader);

            var codeManagerTableDir = new DataDirectory();
            codeManagerTableDir.Read(reader);

            var vtableFixupsDir = new DataDirectory();
            vtableFixupsDir.Read(reader);

            var exportAddressTableJumpsDir = new DataDirectory();
            exportAddressTableJumpsDir.Read(reader);

            var managedNativeHeaderDir = new DataDirectory();
            managedNativeHeaderDir.Read(reader);


            // CLR metadata
            reader.Position = metadataDir.VirtualAddress;

            var mdSignature = (ClrMetadataSignature)reader.ReadUInt32();
            if (mdSignature != ClrMetadataSignature.Signature)
                throw new InvalidOperationException("Invalid CLR metadata signature field " + ((uint)mdSignature).ToString("X") + "h.");

            this.MetadataVersion = new Version(
                reader.ReadUInt16(),
                reader.ReadUInt16());

            uint mdReserved = reader.ReadUInt32();

            int versionLength = reader.ReadInt32();
            string versionString = reader.ReadFixedZeroFilledAsciiString(versionLength);

            this.MetadataVersionString = versionString;

            short mdFlags = reader.ReadInt16();

            ushort streamCount = reader.ReadUInt16();
            var streamHeaders = new StreamHeader[streamCount];

            for (int i = 0; i < streamHeaders.Length; i++)
            {
                streamHeaders[i] = new StreamHeader();
                streamHeaders[i].Read(reader);
            }

            Guid[] guids = null;


            foreach (var sh in streamHeaders)
            {
                reader.Position = metadataDir.VirtualAddress + sh.Offset;

                switch (sh.Name)
                {
                    case "#GUID":
                        guids = new Guid[sh.Size / 128];
                        ReadGuids(reader, guids);
                        break;

                    case "#~":
                    case "#-":
                        int tsReserved0 = reader.ReadInt32();
                        byte tsMajorVersion = reader.ReadByte();
                        byte tsMinorVersion = reader.ReadByte();

                        this.TableStreamVersion = new Version(tsMajorVersion, tsMinorVersion);

                        byte tsHeapSizes = reader.ReadByte();
                        byte tsReserved1 = reader.ReadByte();
                        ulong tsValid = reader.ReadUInt64();
                        ulong tsSorted = reader.ReadUInt64();

                        uint[] tsRows = new uint[CountNonzeroBits(tsValid)];

                        for (int i = 0; i < tsRows.Length; i++)
                        {
                            if ((tsValid & ((ulong)1 << i)) != 0)
                            {
                                tsRows[i] = reader.ReadUInt32();
                            }
                            else
                            {
                                tsRows[i] = 0;
                            }
                        }

                        break;

                    default:
                        break;
                }
            }


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

        private static int CountNonzeroBits(ulong x)
        {
            int tableCount = 0;
            while (x != 0)
            {
                if ((x & 1) != 0)
                    tableCount++;

                x = x / 2;
            }
            return tableCount;
        }
    }
}