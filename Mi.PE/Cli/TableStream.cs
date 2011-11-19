using System;
using System.Collections.Generic;
using System.Linq;

namespace Mi.PE.Cli
{
    using Mi.PE.Internal;

    public sealed class TableStream
    {
        public Version Version;
        public Guid[] Guids;
        public ModuleEntry[] Modules;

        public void Read(BinaryStreamReader reader)
        {
            int tsReserved0 = reader.ReadInt32();
            byte tsMajorVersion = reader.ReadByte();
            byte tsMinorVersion = reader.ReadByte();

            this.Version = new Version(tsMajorVersion, tsMinorVersion);

            byte tsHeapSizes = reader.ReadByte();
            byte tsReserved1 = reader.ReadByte();
            ulong tsValid = reader.ReadUInt64();
            ulong tsSorted = reader.ReadUInt64();

            uint[] tsRowCounts = new uint[64];
            for (int i = 0; i < 64; i++)
            {
                if ((tsValid & (1UL << i)) == 0)
                    continue;

                tsRowCounts[i] = reader.ReadUInt32();
            }

            for (int iTable = 0; iTable < 64; iTable++)
            {
                uint rowCount = tsRowCounts[iTable];

                if (rowCount == 0)
                    continue;


            }


        }
    }
}