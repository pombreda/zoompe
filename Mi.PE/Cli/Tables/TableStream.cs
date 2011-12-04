using System;
using System.Collections.Generic;
using System.Linq;

namespace Mi.PE.Cli.Tables
{
    using Mi.PE.Internal;

    public sealed partial class TableStream
    {
        public Version Version;

        public Array[] Tables;

        public void Read(BinaryStreamReader reader)
        {
            int tsReserved0 = reader.ReadInt32();
            byte tsMajorVersion = reader.ReadByte();
            byte tsMinorVersion = reader.ReadByte();

            this.Version = new Version(tsMajorVersion, tsMinorVersion);

            byte heapSizes = reader.ReadByte();
            byte reserved1 = reader.ReadByte();
            ulong valid = reader.ReadUInt64();
            ulong sorted = reader.ReadUInt64();

            ReadAndInitializeRowCounts(reader, valid);
        }
    }
}