using System;
using System.Collections.Generic;
using System.Linq;
using Mi.PE.Internal;

namespace Mi.PE.Unmanaged
{
    public sealed class BaseRelocationBlock
    {
        /// <summary>
        ///  The image base plus the page RVA is added to each offset to create the VA where the base relocation must be applied.
        /// </summary>
        public uint PageRVA;

        /// <summary>
        ///  The total number of bytes in the base relocation block, including the <see cref="PageRVA"/> and <see cref="Size"/> fields and the Type/Offset fields that follow.
        /// </summary>
        public uint Size;

        public BaseRelocationEntry[] Entries;

        public static BaseRelocationBlock[] ReadBlocks(BinaryStreamReader reader, uint totalSize)
        {
            long endPosition = reader.Position + totalSize;

            var result = new List<BaseRelocationBlock>();
            while (reader.Position<endPosition)
            {
                var block = new BaseRelocationBlock();
                block.PageRVA = reader.ReadUInt32();
                block.Size = reader.ReadUInt32();

                var entries  = new BaseRelocationEntry[block.Size / 2];
                for (int i = 0; i < block.Entries.Length; i++)
                {
                    var entry = new BaseRelocationEntry();
                    ushort encodedEntry = reader.ReadUInt16();

                    entry.Type = (BaseRelocationType)(encodedEntry >> 12);
                    entry.Offset = (ushort)(encodedEntry & 0xFFF);

                    block.Entries[i] = entry;
                }

                block.Entries = entries;

                result.Add(block);
            }

            return result.ToArray();
        }
    }
}