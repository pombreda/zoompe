using System;
using System.Collections.Generic;
using System.Linq;

namespace Mi.PE.Cli.Tables
{
    public struct Implementation
    {
        public enum TableKind
        {
            File = 0,
            AssemblyRef = 1,
            ExportedType = 2
        }

        const int HighBitCount = 2;

        const uint WideKindMask = uint.MaxValue << HighBitCount;
        const ushort NarrowKindMask = unchecked((ushort)(ushort.MaxValue << HighBitCount));

        readonly uint value;

        private Implementation(uint value)
        {
            this.value = value;
        }

        public TableKind Kind { get { return (TableKind)(value >> (32 - HighBitCount)); } }
        public uint Index { get { return value & ~WideKindMask; } }

        public static explicit operator Implementation(uint value)
        {
            return new Implementation(value);
        }

        public static explicit operator Implementation(ushort value)
        {
            ushort high = (ushort)(value & NarrowKindMask);
            ushort low = (ushort)(value & ~NarrowKindMask);

            uint extended = (uint)((high << 16) | low);

            return new Implementation(extended);
        }

        public static explicit operator uint(Implementation value)
        {
            return value.value;
        }

        public static explicit operator ushort(Implementation value)
        {
            ushort high = (ushort)(value.value >> 16);
            ushort low = (ushort)(value.value & ushort.MaxValue);

            ushort compacted = (ushort)(high | low);

            return compacted;
        }
    }
}