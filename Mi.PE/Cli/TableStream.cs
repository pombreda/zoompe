using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mi.PE.Cli
{
    using Mi.PE.Internal;

    public sealed class TableStream
    {
        public int Reserved0;
        public byte MajorVersion;
        public byte MinorVersion;
        public byte HeapSizes;
        public byte Reserved1;
        public ulong Valid;
        public ulong Sorted;
        public uint[] Rows;

        public void Read(BinaryStreamReader reader)
        {
            this.Reserved0 = reader.ReadInt32();
            this.MajorVersion = reader.ReadByte();
            this.MinorVersion = reader.ReadByte();
            this.HeapSizes = reader.ReadByte();
            this.Reserved1 = reader.ReadByte();
            this.Valid = reader.ReadUInt64();
            this.Sorted = reader.ReadUInt64();

            uint[] rows = new uint[CountNonzeroBits(this.Valid)];

            for (int i = 0; i < rows.Length; i++)
            {
                rows[i] = reader.ReadUInt32();
            }

            this.Rows = rows;
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