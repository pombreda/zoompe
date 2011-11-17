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
    }
}