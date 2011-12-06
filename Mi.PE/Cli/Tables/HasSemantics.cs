using System;
using System.Collections.Generic;
using System.Linq;

namespace Mi.PE.Cli.Tables
{
    public struct HasSemantics
    {
        public enum TableKind
        {
            Event = 0,
            Property = 1
        }

        public const int HighBitCount = 1;

        const uint WideKindMask = uint.MaxValue << HighBitCount;
        const ushort NarrowKindMask = unchecked((ushort)(ushort.MaxValue << HighBitCount));

        readonly uint value;

        private HasSemantics(uint value)
        {
            this.value = value;
        }

        public TableKind Kind { get { return (TableKind)(value >> (32 - HighBitCount)); } }
        public uint Index { get { return value & ~WideKindMask; } }

        public static explicit operator HasSemantics(uint value)
        {
            return new HasSemantics(value);
        }

        public static explicit operator HasSemantics(ushort value)
        {
            ushort high = (ushort)(value & NarrowKindMask);
            ushort low = (ushort)(value & ~NarrowKindMask);

            uint extended = (uint)((high << 16) | low);

            return new HasSemantics(extended);
        }

        public static explicit operator uint(HasSemantics value)
        {
            return value.value;
        }

        public static explicit operator ushort(HasSemantics value)
        {
            ushort high = (ushort)(value.value >> 16);
            ushort low = (ushort)(value.value & ushort.MaxValue);

            ushort compacted = (ushort)(high | low);

            return compacted;
        }
    }
}