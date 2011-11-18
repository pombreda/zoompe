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

        public void Read(BinaryStreamReader reader)
        {
            this.Signature = (ClrMetadataSignature)reader.ReadUInt32();
            this.MajorVersion = reader.ReadUInt16();
            this.MinorVersion = reader.ReadUInt16();
            this.Reserved = reader.ReadUInt32();

            int versionLength = reader.ReadInt32();
            string versionString = reader.ReadFixedZeroFilledAsciiString(versionLength);

            this.Version = versionString;

            this.Flags = (ClrMetadataFlags)reader.ReadInt16();

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
        }
    }
}