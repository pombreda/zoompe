using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mi.PE.Cli
{
    using Mi.PE.Internal;

    public sealed class ClrMetadata
    {
        public ClrMetadataSignature Signature;
        public ushort MajorVersion;
        public ushort MinorVersion;
        public uint Reserved;

        /// <summary>
        /// Encoded zero-terminated.
        /// </summary>
        public string Version;

        public ClrMetadataFlags Flags;

        public StreamHeader[] StreamHeaders;

        public ClrMetadata Read(BinaryStreamReader reader)
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
            if (this.StreamHeaders == null)
                this.StreamHeaders = new StreamHeader[streamCount];
            else
                Array.Resize(ref this.StreamHeaders, streamCount);

            for (int i = 0; i < this.StreamHeaders.Length; i++)
            {
                if (this.StreamHeaders[i] == null)
                    this.StreamHeaders[i] = new StreamHeader();

                this.StreamHeaders[i].Read(reader);
            }

            return metadata;
        }
    }
}