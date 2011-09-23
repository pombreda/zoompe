using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Mi.PE.Cli
{
    using Mi.PE.Internal;
    using Mi.PE.PEFormat;

    public static class ClrHeaderReader
    {
        public static ClrHeader ReadClrHeader(BinaryStreamReader reader)
        {
            var result = new ClrHeader();

            result.Cb = reader.ReadUInt32();

            if (result.Cb < ClrHeader.Size)
            {
                throw new BadImageFormatException(
                    "Unexpectedly short " + typeof(ClrHeader).Name + " structure " +
                    result.Cb + " reported by the Cb field "+
                    "(expected at least " + ClrHeader.Size + ").");
            }

            result.MajorRuntimeVersion = reader.ReadUInt16();
            result.MinorRuntimeVersion = reader.ReadUInt16();

            result.MetaData = ReadDataDirectory(reader);

            result.Flags = (ClrImageFlags)reader.ReadInt32();

            result.EntryPointToken = reader.ReadUInt32();

            result.Resources = ReadDataDirectory(reader);
            result.StrongNameSignature = ReadDataDirectory(reader);
            result.CodeManagerTable = ReadDataDirectory(reader);
            result.VTableFixups = ReadDataDirectory(reader);
            result.ExportAddressTableJumps = ReadDataDirectory(reader);
            result.ManagedNativeHeader = ReadDataDirectory(reader);

            return result;
        }

        public static ClrMetadata ReadClrMetadata(BinaryStreamReader reader)
        {
            var metadata = new ClrMetadata();
            metadata.Signature = (ClrMetadataSignature)reader.ReadUInt32();
            metadata.MajorVersion = reader.ReadUInt16();
            metadata.MinorVersion = reader.ReadUInt16();
            metadata.Reserved = reader.ReadUInt32();

            int versionLength = reader.ReadInt32();
            string versionString = reader.ReadFixedZeroFilledAsciiString(versionLength);

            metadata.Version = versionString;

            metadata.Flags = (ClrMetadataFlags)reader.ReadInt16();

            ushort streamCount = reader.ReadUInt16();
            var streamHeaders = new StreamHeader[streamCount];
            for (int i = 0; i < streamHeaders.Length; i++)
            {
                streamHeaders[i] = ReadStreamHeader(reader);
            }

            metadata.StreamHeaders = streamHeaders;

            return metadata;
        }

        public static StreamHeader ReadStreamHeader(BinaryStreamReader reader)
        {
            var header = new StreamHeader();
            header.Offset = reader.ReadUInt32();
            header.Size = reader.ReadUInt32();
            header.Name = ReadAlignedNameString(reader);
            return header;
        }

        public static TableStream ReadTableStream(BinaryStreamReader reader)
        {
            var tStream = new TableStream();
            tStream.Reserved0 = reader.ReadInt32();
            tStream.MajorVersion = reader.ReadByte();
            tStream.MinorVersion = reader.ReadByte();
            tStream.HeapSizes = reader.ReadByte();
            tStream.Reserved1 = reader.ReadByte();
            tStream.Valid = reader.ReadUInt64();
            tStream.Sorted = reader.ReadUInt64();

            uint[] rows = new uint[CountNonzeroBits(tStream.Valid)];

            for (int i = 0; i < rows.Length; i++)
            {
                rows[i] = reader.ReadUInt32();
            }

            tStream.Rows = rows;

            return tStream;
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

        private static string ReadAlignedNameString(BinaryStreamReader reader)
        {
            var bytes = new List<byte>();
            while (true)
            {
                var b = reader.ReadByte();
                if (b == 0)
                    break;

                bytes.Add(b);
            }

            int skipCount = -1 + ((bytes.Count + 4) & ~3) - bytes.Count;

            reader.Position += skipCount;

            return Encoding.UTF8.GetString(bytes.ToArray(), 0, bytes.Count);
        }

        static DataDirectory ReadDataDirectory(BinaryStreamReader reader)
        {
            return new DataDirectory
            {
                VirtualAddress = reader.ReadUInt32(),
                Size = reader.ReadUInt32()
            };
        }
    }
}