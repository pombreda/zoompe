using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mi.PE.Cli
{
    public sealed class ClrMetadata
    {
        public ClrMetadataSignature Signature { get; set; }
        public ushort MajorVersion { get; set; }
        public ushort MinorVersion { get; set; }
        public uint Reserved { get; set; }

        /// <summary>
        /// Encoded zero-terminated.
        /// </summary>
        public string Version { get; set; }

        public ClrMetadataFlags Flags { get; set; }

        public StreamHeader[] StreamHeaders { get; set; }
    }
}